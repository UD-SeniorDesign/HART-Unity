using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class dataController : MonoBehaviour
{
    [Tooltip("File name of the gps json data. Must be in the StreamingAssets Folder")]
    private string gameDataFileName = "";
    private List<gpsData> GPSData;
    private bool read = false;

    public string GameDataFileName
    {
        get
        {
            return gameDataFileName;
        }

        set
        {
            gameDataFileName = value;
        }
    }

    // Use this for initialization
    void OnEnable()
    {
        GPSData = new List<gpsData>();
        ReadFile();
    }
    private void ReadFile()
    {
        
        string filePath = Path.Combine(Application.streamingAssetsPath, GameDataFileName);
        //Debug.Log(GameDataFileName);
        string fileData;
        try
        {
            byte[] bytes = UnityEngine.Windows.File.ReadAllBytes(filePath);
            fileData = System.Text.Encoding.ASCII.GetString(bytes);
            string[] lines = fileData.Split('\n');

            char[] toTrim = { '[', ',', ']' };
            foreach (string json in lines)
            {
                string tmp = json.Trim(toTrim);
                //Debug.Log(tmp);
                GPSData.Add(JsonUtility.FromJson<gpsData>(tmp));
                //Debug.Log(GPSData[GPSData.Count - 1].Elevation);
            }
            read = true;
        }
        catch (FileNotFoundException)
        {
            Debug.Log("File " + filePath + " not found");
            return;
        }
        
    }
    public gpsData getGPS(int idx)
    {
        return read?GPSData[idx % GPSData.Count]:null;
    }
}
