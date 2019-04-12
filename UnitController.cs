using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct pathFinding
{
    int xPos, yPos, zPos;
}
public class UnitController : MonoBehaviour {
    public MapCreator mapRef;
    public GameObject landTile;
    public TileController tileRef;
    public GameController gameRef;
    public pathFinding[] directions;
    public Vector3[] tileDirections;
    public int arrayXRef, arrayYRef, arrayZRef;
    public int dirIndex;
    public int ArrayXRef
    {
        get
        {
            return arrayXRef;
        }
        set
        {
            arrayXRef = value;
        }
    }
    public int ArrayYRef
    {
        get
        {
            return arrayYRef;
        }
        set
        {
            arrayYRef = value;
        }
    }
    public int ArrayZRef
    {
        get
        {
            return arrayZRef;
        }
        set
        {
            arrayZRef = value;
        }
    }
    float currentX, currentY, currentZ;
    public float targetX, targetY, targetZ;
    bool move;
    public float speed, turnSpeed;
	// Use this for initialization
	void Start () {
        gameRef = FindObjectOfType<GameController>();
        mapRef = gameRef.GetComponent<MapCreator>();
        currentX = transform.position.x;
        currentY = transform.position.y;
        currentZ = transform.position.z;
        dirIndex = 0;
        move = false;
	}
    public void pathFinder(int x, int y, int z)
    {
        dirIndex = 0;
        int i = 1;
        int xDistance = Mathf.Abs(x - ArrayXRef);
        Debug.Log(xDistance);
        int xDir = (x - ArrayXRef) / Mathf.Abs(x - ArrayXRef);
        int zDistance = Mathf.Abs(z - ArrayZRef);
        Debug.Log(zDistance);
        int zDir = (z - ArrayZRef) / Mathf.Abs(z - ArrayZRef);
        tileDirections = new Vector3[xDistance + zDistance];
        while(ArrayXRef + (i * xDir) != x)
        {
            landTile = mapRef.tileRef[ArrayXRef + i, ArrayYRef, ArrayZRef];
            tileRef = landTile.GetComponent<TileController>();
            tileDirections[i-1] = landTile.transform.position;
            Debug.Log(tileDirections[i - 1]);
            i++;
        }
        ArrayXRef = x;
        int j = 1;
        while(ArrayZRef + (j * zDir) != z)
        {
            landTile = mapRef.tileRef[x, ArrayYRef, ArrayZRef + j];
            tileRef = landTile.GetComponent<TileController>();
            tileDirections[i-1] = landTile.transform.position;
            Debug.Log(tileDirections[i - 1]);
            i++;
            j++;
        }
        ArrayZRef = z;
        move = true;
    }
    public void MoveToLocation(float x, float y, float z)
    {
        targetX = x;
        targetY = y;
        targetZ = z;
    }
    void FixedUpdate()
    {
        if (move)
        {
            MoveToLocation(tileDirections[dirIndex].x, tileDirections[dirIndex].y, tileDirections[dirIndex].z);
            currentX = transform.position.x;
            currentY = transform.position.y;
            currentZ = transform.position.z;
            if (transform.position == new Vector3(targetX, targetY, targetZ))
            {
                dirIndex++;
                if (tileDirections[dirIndex] == new Vector3(0, 0, 0))
                {
                    move = false;
                }
                if (dirIndex >= tileDirections.Length)
                {
                    move = false;
                }
            }
        }
    }
	// Update is called once per frame
	void Update () {
        float step = speed * Time.deltaTime;
        float turnStep = turnSpeed * Time.deltaTime;
        Vector3 target = new Vector3(targetX, targetY, targetZ);
		if(move)
        {
            Vector3 turnTarget = target - transform.position;
            transform.position = Vector3.MoveTowards(transform.position, target, step);
            Vector3 lookAt = Vector3.RotateTowards(transform.forward, turnTarget, turnStep, 0.0f);
            transform.rotation = Quaternion.LookRotation(lookAt);
        }
	}
}
