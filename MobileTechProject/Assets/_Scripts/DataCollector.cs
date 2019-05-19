using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataCollector : MonoBehaviour
{
    StreamWriter sw;
    string filePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory);
    int fuelForward;
    int fuelBackward;

    public void FuelUsed(bool forward)
    {
        if (forward)
        {
            fuelForward++;
        }
        else
        {
            fuelBackward++;
        }
    }

    public void ResetRunData(int score)
    {
        string values = "";

        if (score > 50)
        {
            int totalFuel = fuelForward + fuelBackward;

            values = score.ToString("D3") + "\t" + totalFuel.ToString("D3") + "\t" + fuelForward.ToString("D3") + "\t" + fuelBackward.ToString("D3");

            Log(values);

            fuelForward = 0;
            fuelBackward = 0;
        }
    }

    public void Log(string data)
    {
        if (!File.Exists(filePath + "\\FuelData.txt"))
        {
            using (StreamWriter writer = new StreamWriter(filePath + "\\FuelData.txt"))
            {
                writer.WriteLine("Dist.\tFuel\tForward\tBack");
            }
        }

        using (StreamWriter writer = new StreamWriter(filePath + "\\FuelData.txt", append: true))
        {


            writer.WriteLine(data);
        }
    }
}