using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct unitCoords
{
    int xLoc;
    int yLoc;
    int zLoc;
}
public class UnitDataHolder : MonoBehaviour {
    public unitCoords[] unitLocRef;
    public TileController mapRef;
    public int xCoord, yCoord, zCoord;
	// Use this for initialization
	void Start () {
		
	}
    void SetLocation(int x, int y, int z)
    {

    }
    void GetLocation()
    {

    }
	// Update is called once per frame
	void Update () {
		
	}
}
