using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class dataController : MonoBehaviour
{
    [Tooltip("File name of the gps json data. Must be in the StreamingAssets Folder")]
    private string gameDataFileName = "";
    private List<gpsDataDated> GPSData;
    private bool read = false;
    private int idx = 0;
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
        GPSData = new List<gpsDataDated>();
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
                GPSData.Add(JsonUtility.FromJson<gpsDataDated>(tmp));
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
    /// <summary>
    /// Returns the current gps location and then updates the index. Will return null if a file has not been loaded
    /// </summary>
    /// <returns>gpsData or null</returns>
    public gpsDataDated getGPS()
    {
        // Prevent overflow
        if (idx > GPSData.Count)
            idx = 0;
        return read?GPSData[idx++ % GPSData.Count]:null;
    }
}
