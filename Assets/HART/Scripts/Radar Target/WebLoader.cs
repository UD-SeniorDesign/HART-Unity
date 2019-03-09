using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Mapbox.Unity.Map;

public class WebLoader : MonoBehaviour
{

    public string url = "http://72.78.75.212:9900/data?";
    public bool demoLoop = false;
    public bool commercialFlight = false;
    public float lngMin = -74.181003f;
    public float lngMax = -73.619899f;
    public float latMin = 40.647120f;
    public float latMax = 40.948716f;
    public bool debug = false;
    private AbstractMap map;

    public frame dataFrame;
    private string demoLoopURL = "demoLoop=1&";
    private string commericalFlightURL = "commercialFlights=1&lngMin={0}&lngMax={1}&latMin={2}&latMax={3}&";
    public float delay = 1f;
    // http://72.78.75.212:9900/data?demoLoop=1&commercialFlights=1&lngMin=-76.623080&lngMax=-73.828576&latMin=38.938079&latMax=40.632118

    private float lastUpdate = 0f;

    public frame DataFrame
    {
        get
        {
            return dataFrame;
        }

        set
        {
            dataFrame = value;
        }
    }

    void Start()
    {
        lastUpdate = Time.time;
        try
        {
            map = GameObject.FindGameObjectWithTag("map").GetComponent<AbstractMap>();
        }
        catch (System.Exception)
        {
        }
        
    }
    void Update()
    {
        if (map == null)
        {
            try
            {
                map = GameObject.FindGameObjectWithTag("map").GetComponent<AbstractMap>();
            }
            catch (System.Exception)
            {
            }
        }
            
        if (Time.time - lastUpdate > delay)
        {
            StartCoroutine(GetText());
            lastUpdate = Time.time;
        }

    }
    IEnumerator GetText()
    {
        
        string request = url;
        if (demoLoop)
            request += demoLoopURL;
        if (commercialFlight)
            request += string.Format(commericalFlightURL, lngMin, lngMax, latMin, latMax).TrimEnd('&');
        if (debug)
            Debug.Log(request);
        UnityWebRequest www = UnityWebRequest.Get(request);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            
            dataFrame = JsonUtility.FromJson<frame>(www.downloadHandler.text);
            if (debug)
                Debug.Log(dataFrame);
        }
    }

}
