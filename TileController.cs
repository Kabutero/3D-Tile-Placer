using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour {
    private float xCoord, yCoord, zCoord;
    public float XCoord
    {
        get
        {
            return xCoord;
        }
        set
        {
            xCoord = value;
        }
    }
    public float YCoord
    {
        get
        {
            return yCoord;
        }
        set
        {
            yCoord = value;
        }
    }
    public float ZCoord
    {
        get
        {
            return zCoord;
        }
        set
        {
            zCoord = value;
        }
    }
    public GameController gameController;
    public GameObject currentUnit;
    public UnitController unitC;
    public PlayerController playerRef;
	// Use this for initialization
	void Start () {

	}
    public void OnMouseDown()
    {
        Debug.Log("Clicked");
        Debug.Log("Tile Located at [" + xCoord + ", " + yCoord + ", " + zCoord + "]");
        currentUnit = GameObject.FindGameObjectWithTag("Player");
        unitC = currentUnit.GetComponent<UnitController>();
        //unitC.MoveToLocation(this.transform.position.x, this.transform.position.y, this.transform.position.z, Mathf.RoundToInt(XCoord), Mathf.RoundToInt(YCoord), Mathf.RoundToInt(ZCoord));
        unitC.pathFinder(Mathf.RoundToInt(XCoord), Mathf.RoundToInt(YCoord), Mathf.RoundToInt(ZCoord));
    }
	public void GetCoordinates()
    {

    }
    public void SetCoordinates(int x, int y, int z)
    {
        xCoord = x;
        yCoord = y;
        zCoord = z;
    }
	// Update is called once per frame
	void Update () {
		
	}
}
