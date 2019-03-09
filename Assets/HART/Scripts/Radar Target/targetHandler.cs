using Mapbox.Unity.Map;
using UnityEngine;

public class targetHandler : MonoBehaviour
{
    public delegate void mapInit();

    private AbstractMap map;
    private Vector3 orgScale;
    private void Start()
    {
        try
        {
            map = GameObject.FindGameObjectWithTag("Map").GetComponent<AbstractMap>();
            map.OnInitialized += delegate { updateMap(); };
            map.OnUpdated += delegate { updateMap(); };
        }
        catch (System.Exception)
        {

        }

        transform.localScale = Vector3.one * 10;
        orgScale = transform.localScale;
        if (gameObject.name.StartsWith("a"))
            orgScale *= 7;
        updateMap();
    }
    private void Update()
    {
        if (map == null)
        {
            try
            {
                map = GameObject.FindGameObjectWithTag("Map").GetComponent<AbstractMap>();
                map.OnInitialized += delegate { updateMap(); };
                map.OnUpdated += delegate { updateMap(); };
                updateMap();
            }
            catch (System.Exception)
            {

            }
        }
    }
    public void updateMap()
    {
        transform.localScale = orgScale * map.WorldRelativeScale;

        //Debug.Log("Map updated, changing scale of " + gameObject.name + " to " + transform.localScale);
    }
}
