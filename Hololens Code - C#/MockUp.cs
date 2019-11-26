using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockUp : MonoBehaviour {


    public GameObject cylinder;
    // Use this for initialization
    void Start () {
        cylinder = GameObject.Find("Cylinder");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public int Pan(int direction)
    {
        switch (direction)
        {
            case 1:
                cylinder.transform.Translate(new Vector3(0, 0, 1));
                break;
            case 2:
                cylinder.transform.Translate(new Vector3(0, 0, -1));
                break;
            case 3:
                cylinder.transform.Translate(new Vector3(1, 0, 0));
                break;
            case 4:
                cylinder.transform.Translate(new Vector3(-1, 0, 0));
                break;
            default:
                break;
        }
        return 0;
    }

    public int Zoom(int factor)
    {
        float newSize;

        switch (factor)
        {
            case 1:
                newSize = cylinder.transform.localScale.x + 0.1f;
                if (newSize <= 1)
                {
                    cylinder.transform.localScale = new Vector3(newSize, newSize, newSize);
                }
                break;
            case 2:
                newSize = cylinder.transform.localScale.x - 0.1f;
                if (newSize >= 0.1)
                    cylinder.transform.localScale = new Vector3(newSize, newSize, newSize);
                break;
            default:
                break;
        }
        return 0;
    }

    public int Rotate(int rotation)
    {
        switch (rotation)
        {
            case 1:
                cylinder.transform.Rotate(new Vector3(cylinder.transform.rotation.x, cylinder.transform.rotation.y + 30.0f, cylinder.transform.rotation.z));
                break;
            case 2:
                cylinder.transform.Rotate(new Vector3(cylinder.transform.rotation.x, cylinder.transform.rotation.y - 30.0f, cylinder.transform.rotation.z));
                break;
            default:
                break;
        }
        return 0;
    }

    public byte[] ReceiveText(byte[] text)
    {
        switch(text[0])
        {
            case 0:
                break;
            default:
                Debug.Log(System.Text.Encoding.UTF8.GetString(text, 0, text.Length));
                break;
        }

        return new byte[36];
    }

    public byte Scale(byte data)
    {
        switch (data)
        {
            case 1:
                Debug.Log("We scaled the map down");
                break;
            case 2:
                Debug.Log("We scaled the map up");
                break;
            default:
                break;
        }
        return 0;
    }
}
