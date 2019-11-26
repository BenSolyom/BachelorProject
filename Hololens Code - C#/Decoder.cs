using System;
using System.Collections.Generic;
using UnityEngine;
using MRNanoMap.MRMap;
using MRNanoMap.UI;
using MRNanoMap.MRMap.Model;
using UnityEngine.XR.WSA.Input;

public class Decoder : MonoBehaviour
{
    public byte receivedData = 0;
    public byte movementData = 0;
    public byte zoomData = 0;
    private byte rotateData = 0;
    private byte scaleData = 0;
    private byte tapData = 0;
    private byte[] textData = new byte[36];
    //private MockUp MockUp;
    private GameObject map;
    private GameObject[] terrains;

    private void Start()
    {

        //MockUp = GameObject.FindGameObjectWithTag("MockUp").GetComponent<MockUp>();
    }

    void Update()
    {

        if (TiledMap.IsReady && map == null)
        {
            map = GameObject.Find("Map");
        }
        try
        {
            if (terrains != null && terrains.Length == 9)
            {
                scaleData = ScaleController();
            }
            else if (TiledMap.IsReady)
            {
                terrains = GameObject.FindGameObjectsWithTag("TerrainTile");
                MapLocationTracker.Instance.trackedLocationText.text = terrains.Length + "";
            }
        }
        catch (Exception e)
        {
            MapLocationTracker.Instance.trackedLocationText.text = e.Message;
        }

        if (map != null)
            zoomData = ZoomController();

        movementData = PanMapController();
        rotateData = RotationController();
        //tapData = TapSelectionController();
        textData = TextController();

    }


    public void Decode(byte[] data)
    {
        Debug.Log("Decoder called");
        switch (data[0])
        {
            case 1:
                movementData = data[1];
                break;
            case 2:
                zoomData = data[1];
                break;
            case 3:
                rotateData = data[1];
                break;
            case 4:
                Array.Copy(data, 1, textData, 0, data.Length - 1);
                break;
            case 5:
                scaleData = data[1];
                break;
            case 6:
                tapData = data[1];
                break;
            default:
                break;
        }
    }

    private byte ZoomController()
    {
        if (zoomData != 0)
        {
            RangeSelectionController zoomSelector = GameObject.Find("ZoomSelector").GetComponent<RangeSelectionController>();
            zoomSelector.ZoomChanged(zoomData);
        }
        return 0;
    }

    private byte PanMapController()
    {
        TiledMap tiledMap = TiledMap.Instance;

        switch (movementData)
        {
            case 1:
                tiledMap.SetTile(new Tile
                {
                    X = TiledMap.Tile.X,
                    Y = TiledMap.Tile.Y - 1,
                    Zoom = TiledMap.Tile.Zoom
                }); //north
                break;
            case 2:
                tiledMap.SetTile(new Tile
                {
                    X = TiledMap.Tile.X,
                    Y = TiledMap.Tile.Y + 1,
                    Zoom = TiledMap.Tile.Zoom
                }); //south
                break;
            case 3:
                tiledMap.SetTile(new Tile
                {
                    X = TiledMap.Tile.X + 1,
                    Y = TiledMap.Tile.Y,
                    Zoom = TiledMap.Tile.Zoom
                }); //east
                break;
            case 4:
                tiledMap.SetTile(new Tile
                {
                    X = TiledMap.Tile.X - 1,
                    Y = TiledMap.Tile.Y,
                    Zoom = TiledMap.Tile.Zoom
                }); //west
                break;
            default:
                break;
        }
        return 0;
    }

    private byte RotationController()
    {
        switch (rotateData)
        {
            case 1:
                map.transform.Rotate(new Vector3(map.transform.rotation.x, map.transform.rotation.y + 30.0f, map.transform.rotation.z));
                break; // positive rotation (counter-clockwise)
            case 2:
                map.transform.Rotate(new Vector3(map.transform.rotation.x, map.transform.rotation.y - 30.0f, map.transform.rotation.z));
                break; // negative rotation (clockwise)
            default:
                break;
        }
        return 0;
    }

    private byte ScaleController()
    {
        float terrainTileSize = terrains[0].GetComponent<Terrain>().terrainData.size.x;
        float mapSize = map.transform.localScale.x;
        switch (scaleData)
        {
            case 1:
                if (mapSize > 0.4f && terrainTileSize > 0.2f)
                {
                    Debug.Log(mapSize);
                    map.transform.localScale = new Vector3(mapSize - 0.15f, 1, mapSize - 0.15f);
                    foreach (GameObject item in terrains)
                    {
                        item.GetComponent<Terrain>().terrainData.size = new Vector3(terrainTileSize - 0.1f, 1, terrainTileSize - 0.1f);
                    }
                }
                break; // shrinking map
            case 2:
                if (mapSize < 1 && terrainTileSize < 0.6f)
                {
                    Debug.Log(mapSize);
                    map.transform.localScale = new Vector3(mapSize + 0.15f, 1, mapSize + 0.15f);
                    foreach (GameObject item in terrains)
                    {
                        item.GetComponent<Terrain>().terrainData.size = new Vector3(terrainTileSize + 0.1f, 1, terrainTileSize + 0.1f);
                    }
                }
                break; // enlarging map
            default:
                break;
        }
        return 0;
    }

    private byte[] TextController()
    {
        switch (textData[0])
        {
            case 0:
                break;
            default:
                TiledMap.Instance.SetExaggeration(Int32.Parse(System.Text.Encoding.UTF8.GetString(textData, 0, textData.Length)));
                break;
        }
        return new byte[36];
    }

    /*private byte TapSelectionController()
    {
        GestureRecognizer _gestureRecognizer;
        if (tapData != 0)
        {
            _gestureRecognizer = new GestureRecognizer();
            _gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.Hold | GestureSettings.DoubleTap);

            _gestureRecognizer.TappedEvent += OnTapped;


            
            _gestureRecognizer.StartCapturingGestures();
        }

        return 0;
    }

    private void OnTapped(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        throw new NotImplementedException();
    }*/
}