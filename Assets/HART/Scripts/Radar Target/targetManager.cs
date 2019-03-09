using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;

public class targetManager : MonoBehaviour
{
    public GameObject targetPrefab;

    private frame dataFrame;
    private int lastTick = -1;
    private int tick = 0;
    private AbstractMap map;
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            map = GameObject.FindGameObjectWithTag("Map").GetComponent<AbstractMap>();

        }
        catch (System.Exception)
        {

        }

    }

    // Update is called once per frame
    void Update()
    {
        if (map == null)
        {
            try
            {
                map = GameObject.FindGameObjectWithTag("Map").GetComponent<AbstractMap>();
                
            }
            catch (System.Exception)
            {

            }
        }
        dataFrame = gameObject.GetComponent<WebLoader>().DataFrame;

        if (dataFrame != null)
        {
            
            tick = dataFrame.tick;
            if (tick != lastTick)
            {
                if (dataFrame.targets.Length > 0)
                {
                    foreach (gpsData target in dataFrame.targets)
                    {
                        GameObject radarTarget = GameObject.Find("DataManager/" + target.id);

                        if (radarTarget != null)
                        {

                            Vector3 pos = map.GeoToWorldPosition(new Mapbox.Utils.Vector2d(target.Latitude, target.Longitude));
                            //Debug.Log(map.QueryElevationInMetersAt(new Mapbox.Utils.Vector2d(target.Latitude, target.Longitude)));
                            pos.y += radarTarget.GetComponent<Renderer>().bounds.size.y + (target.Elevation * map.WorldRelativeScale);
                            
                            radarTarget.transform.position = pos;
                        }
                        else
                        {
                            Vector3 pos = map.GeoToWorldPosition(new Mapbox.Utils.Vector2d(target.Latitude, target.Longitude));
                            pos.y += (target.Elevation * map.WorldRelativeScale);
                            radarTarget = Instantiate(targetPrefab, pos, Quaternion.identity, transform);
                            radarTarget.name = target.id;

                        }
                    }
                }
                lastTick = tick;
            }
        }
    }
}
