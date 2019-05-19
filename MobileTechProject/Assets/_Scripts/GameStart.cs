using System.Collections;
using System.Collections.Generic;
//using UnityEngine.UI;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    [SerializeField] private Sprite androidIcon, pcIconLeft, pcIconRight;
    [SerializeField] private GameObject tapPivot;
    private SpriteRenderer[] sprites;
    private MeshRenderer[] textMeshes;
    private float startDelayTimer = 1.2f;
    private bool startCalled = false;
    private bool spinningIcons = true;

    // Use this for initialization
    void Awake()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>(); // 0 = IconLeft, 1 = ArrowLeft, 2 = IconRight, 3 = ArrowRight
        textMeshes = GetComponentsInChildren<MeshRenderer>();

        foreach (SpriteRenderer s in sprites)
        {
            Debug.Log(s.name);
        }

#if UNITY_STANDALONE
        sprites[0].sprite = pcIconLeft;
        sprites[2].sprite = pcIconRight;

        sprites[1].flipX = false;
        sprites[3].flipX = true;

        sprites[4].enabled = false;

#elif UNITY_ANDROID
        sprites[1].enabled = false; 
        sprites[3].flipX = false;

        StartCoroutine(AnimateTap());
#endif

        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey && !startCalled)
        {
            StartCoroutine(StartGame());
            startCalled = true;
        }
    }

#if UNITY_ANDROID
    /// <summary>
    /// Rotates the tap pivot by 180 degrees to give the effect of animation. Ends at (0, 0, 0)
    /// </summary>
    private IEnumerator AnimateTap()
    {
        Vector2 rotate = new Vector2(0, 180);
        while (spinningIcons)
        {
            tapPivot.transform.Rotate(rotate);
            yield return new WaitForSecondsRealtime(0.7f);
        }
        tapPivot.transform.rotation = Quaternion.Euler(Vector3.zero); // set icon pivot rotation to (0, 0, 0)
    }
#endif

    public IEnumerator StartGame()
    {
        Time.timeScale = 1;

        // cause the text to disappear
        textMeshes[0].gameObject.SetActive(false);
        textMeshes[1].gameObject.SetActive(false);

        yield return new WaitForSeconds(startDelayTimer);

        spinningIcons = false; // stop tap icons from spinning

        // sets all icons to inactive
        foreach (SpriteRenderer s in sprites)
        {
            s.gameObject.SetActive(false);
        }
    }
}
