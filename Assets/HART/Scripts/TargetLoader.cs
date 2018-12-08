using Mapbox.Unity.Map;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TargetLoader : MonoBehaviour {

    [Tooltip("Prefab to spawn")]
    public GameObject prefab;
    public AbstractMap map;
    void OnEnable()
    {

        if (prefab != null)
        {
            if (map == null)
            {
                map = GameObject.FindWithTag("Map").GetComponent<AbstractMap>();
            }
            // Load all objects to render
            DirectoryInfo dir = new DirectoryInfo(Application.streamingAssetsPath);
            FileInfo[] info = dir.GetFiles("*.*");
            foreach (FileInfo i in info)
            {

                if (i.Extension == ".json")
                {
                    
                    
                    GameObject target = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity,transform);
                    target.GetComponent<dataController>().GameDataFileName = i.Name;
                    target.GetComponent<radarManager>().mapManager = map;
                    target.SetActive(true);
                }
            }
        }
    }
}
