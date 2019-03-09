﻿using System;
using System.Collections;
using System.Collections.Generic;
using Mapbox.Unity.Location;
using Mapbox.Unity.Map;
using UnityEngine;

public class radarManager : MonoBehaviour
{

    [SerializeField]
    public AbstractMap mapManager;

    public delegate void mapInit();

    public GameObject player;

    private gpsDataDated currGPS;
    private int idx = 0;
    private dataController data;
    private float size = 0;
    private Vector3 orgScale;
    private bool debug = false;
    
    private void OnEnable()
    {
        if (player == null)
        {
            player = gameObject;
        }
        if (mapManager == null)
        {
            Debug.Log("Did you forget to add the map?");
            idx = -1;
            // Disable gameobject to stop issues
            gameObject.SetActive(false);
        }
        else
        {
            data = this.GetComponent<dataController>();
            orgScale = player.transform.localScale;
            try
            {
                debug = GameObject.FindGameObjectWithTag("gameManager").GetComponent<gameState>().debugging;
            }
            catch (Exception)
            {

                debug = false;
            }
            mapManager.OnInitialized += delegate { updateMap();};
            mapManager.OnUpdated += delegate { updateMap(); };
        }

    }
    
    /// <summary>
    /// Updates game model scale to be inline with map scale
    /// </summary>
    public void updateMap()
    {
        Debug.Log("Map was updated");
        
        transform.localScale = orgScale * mapManager.WorldRelativeScale;
        
        size = player.GetComponent<Renderer>().bounds.size.y / 2;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (idx != -1)
        {
            currGPS = data.getGPS();
            if (currGPS != null)
            {
                Vector3 pos = mapManager.GeoToWorldPosition(new Mapbox.Utils.Vector2d(currGPS.Latitude, currGPS.Longitude));
                pos.y += size + (currGPS.Elevation * mapManager.WorldRelativeScale);
                player.transform.position = pos;
            }
        }
    }
}