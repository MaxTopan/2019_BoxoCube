using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCollider : MonoBehaviour
{
    public Sprite[] icons = new Sprite[3];
    public SpriteRenderer[] tapIcons = new SpriteRenderer[2];

    private bool timerStarted = false;
    private bool displaying = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!timerStarted)
            {
                StartCoroutine(DisplayTutorial());
                timerStarted = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            displaying = false;
            timerStarted = false;
        }
    }

    private IEnumerator DisplayTutorial()
    {
        displaying = true;
        yield return new WaitForSeconds(1f);

        tapIcons[0].gameObject.SetActive(true);
        tapIcons[1].gameObject.SetActive(true);

#if UNITY_STANDALONE
        tapIcons[0].sprite = icons[2];
        tapIcons[1].flipX = false;
#endif

        while (displaying)
        {
#if UNITY_ANDROID
            tapIcons[0].sprite = icons[0];
            tapIcons[1].enabled = true;
            yield return new WaitForSeconds(0.5f);
            tapIcons[0].sprite = icons[1];
            tapIcons[1].enabled = false;
            yield return new WaitForSeconds(0.5f);

#elif UNITY_STANDALONE
            tapIcons[1].enabled = true;
            yield return new WaitForSeconds(0.5f);
            tapIcons[1].enabled = false;
            yield return new WaitForSeconds(0.5f);
#endif

        }

        tapIcons[0].gameObject.SetActive(false);
        tapIcons[1].gameObject.SetActive(false);
    }
}
