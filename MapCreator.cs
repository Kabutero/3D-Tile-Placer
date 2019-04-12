using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public struct Coordinates
{
    public float x, y, z;
}
public class MapCreator : MonoBehaviour {
    public GameObject landTile;
    public GameObject currentTile;
    public GameObject testCharacter;
    public GameObject currentCharacter;
    public TileController CoordSet;
    public UnitController unitSet;
    public Coordinates[,,] MapCoordinates;
    public Button clear, create;
    public GameObject[,,] tileRef;
    public GameObject[,,] unitRef;
    public int EvenRows;
    enum LandformType { Mesa, Hill, Slope, Tunnel}
    int xTrack, yTrack, zTrack;
    float mesaHeight, mesaLength, mesaWidth;
    int xStart, yStart, zStart;
    int maxHeight;
    int maxStruct;
    int landform, radius;
    public Vector3 tilePlacement;
	// Use this for initialization
	void Start () {
        Button clearbtn = clear.GetComponent<Button>();
        Button createbtn = create.GetComponent<Button>();
        clearbtn.onClick.AddListener(Clear);
        createbtn.onClick.AddListener(GenerateMap);
        MapCoordinates = new Coordinates[50, 50, 50];
        tileRef = new GameObject[50, 50, 50];
        unitRef = new GameObject[50, 50, 50];
        EvenRows = 1;
        GenerateMap();
        Test();
	}
	
    void GenerateMap()
    {
        //spawns initial ground level
        for(float x = 0; x < 50; x += 1.734f)
        {
            for(float z = 0; z < 50; z += 1.502f)
            {
                if(EvenRows % 2 != 0)
                {
                    tilePlacement = new Vector3(x, .12f, z);
                    tileRef[xTrack, 0, zTrack] = Instantiate(landTile, tilePlacement, Quaternion.Euler(90, 0, 0));
                    currentTile = tileRef[xTrack, 0, zTrack].GetComponent<GameObject>();
                    CoordSet = tileRef[xTrack, 0, zTrack].GetComponent<TileController>();
                    CoordSet.XCoord = xTrack;
                    CoordSet.ZCoord = zTrack;
                    CoordSet.YCoord = 0;
                    //currentTile;
                    //CoordSet.SetCoordinates(xTrack, 0, zTrack);
                }
                else
                {
                    tilePlacement = new Vector3(x + (1.734f/2), .12f, z);
                    tileRef[xTrack, 0, zTrack] = Instantiate(landTile, tilePlacement, Quaternion.Euler(90, 0, 0));
                    CoordSet = tileRef[xTrack, 0, zTrack].GetComponent<TileController>();
                    CoordSet.XCoord = xTrack;
                    CoordSet.ZCoord = zTrack;
                    CoordSet.YCoord = 0;
                    //CoordSet.SetCoordinates(xTrack, 0, zTrack);
                }
                MapCoordinates[xTrack, 0, zTrack].x = x;
                MapCoordinates[xTrack, 0, zTrack].y = 0.12f;
                MapCoordinates[xTrack, 0, zTrack].z = z;
                EvenRows++;
                zTrack++;
            }
            zTrack = 0;
            xTrack++;
            
        }
        xTrack = 0;
        unitRef[0, 0, 0] = Instantiate(testCharacter, new Vector3(0, .24f, 0), Quaternion.Euler(0, 0, 0));
        //currentCharacter = unitRef[0, 0, 0].GetComponent<GameObject>();
        unitSet = unitRef[0,0,0].GetComponent<UnitController>();
        unitSet.ArrayXRef = 0;
        unitSet.ArrayYRef = 0;
        unitSet.ArrayZRef = 0;
     
        for (int i = 0; i < 10; i++)
        {
            int collisionCheck = 0;
            do
            {
                mesaHeight = Random.Range(1, 15);
                mesaLength = Random.Range(4, 15);
                mesaWidth = Random.Range(5, 15);
                int height = Random.Range(1, 30);
                radius = Random.Range(1, 10);
                int scale = Random.Range(1, 8);
                xStart = Random.Range(1, 40);
                zStart = Random.Range(1, 40);
                landform = Random.Range(1, 3);
                if (landform == 1)
                {
                    MesaTest(height, Mathf.RoundToInt(mesaWidth), Mathf.RoundToInt(mesaLength), xStart, zStart, tileRef);
                }
                else
                {
                    HillTest(radius, height, scale, xStart, zStart);
                }
                collisionCheck++;
            } while (checkForCollision(xStart, zStart, Mathf.RoundToInt(mesaHeight), radius, Mathf.RoundToInt(mesaWidth), Mathf.RoundToInt(mesaLength), landform) && collisionCheck < 10);
        }
        mesaHeight = Random.Range(1, 15);
        mesaLength = Random.Range(4, 15);
        mesaWidth = Random.Range(5, 15);
        xStart = Random.Range(1, 20);
        yStart = Random.Range(1, 20);
        zStart = Random.Range(1, 20);
        //spawn test mesa
        //SpawnMesa(mesaHeight, mesaLength, mesaWidth, xStart, yStart, zStart, tileRef);
        //MesaTest(Mathf.RoundToInt(mesaHeight), Mathf.RoundToInt(mesaWidth), Mathf.RoundToInt(mesaLength), 2, 2, tileRef);
        //MesaTest(4, 10, 10, 4, 4, tileRef);
        //SpawnHill();
        //SpawnHillTest(6, 20, 4, 0, 12);
        //SpawnCircle(3, 10, 10, 10);
        //PlaceAtLowest(4, 4, tileRef);
        //SpawnHillFinal(1, 3, 15, 15, tileRef);
        //CircleTest(9, 10, 10, tileRef);
        //CircleTest(8, 10, 10, tileRef);
        //CircleTest(7, 10, 10, tileRef);
        //HillTest(5, 4, 1, 10, 10);
    }
    //Is used to test new and updated code
    void Test()
    {
        MesaTest(10, 10, 10, 10, 10, tileRef);
        CreateRavine(15, 15, 15, 1);
        
        //CreateTunnel(10, 10, 10, 10, 10, 10);
        //SpawnArchway(10, 10, 10, 10, 40);
        //SpawnSlope(10, 1, 10, 30, 10, 10, 1);
    }
    void PopulateMap(int maxHeight, int structuresNum)
    {
        for(int i = 0; i < structuresNum; i++)
        {
            int landChoice = Random.Range(1, 1);
            switch (landChoice)
            {
                case 1:
                    int height = Random.Range(1, maxHeight);
                    int radius = Random.Range(1, 12);
                    int xLoc = Random.Range(0, 49);
                    int zLoc = Random.Range(0, 49);
                    HillTest(radius, height, Mathf.RoundToInt(height / radius), xLoc, zLoc);
                    break;
                case 2:
                    int mHeight = Random.Range(1, maxHeight);
                    int width = Random.Range(1, 30);
                    int length = Random.Range(1, 30);
                    int mXLoc = Random.Range(0, 49);
                    int mZLoc = Random.Range(0, 49);
                    MesaTest(mHeight, width, length, mXLoc, mZLoc, tileRef);
                    break;
            }

        }
    }
    bool checkForCollision(int x, int z, int h, int r, int w, int l, int t)
    {
        for(int j = z; j < (z+w+4); j++)
        {
            for(int i = x; i < (x+l+4); i++)
            {
                if(tileRef[x, 1, z] != null)
                {
                    return true;
                }
            }
        }
        return false;
    }
    void Clear()
    {
        for(int y = 0; y < 50; y++)
        {
            for(int x = 0; x < 50; x++)
            {
                for(int z = 0; z < 50; z++)
                {
                    Destroy(tileRef[x, y, z]);
                }
            }
        }
    }
    //Deprecated code. The more efficient method is MesaTest. All deprecated code will eventually be renamed and consolidated at the end of each script.
    void SpawnMesa(float height, float length, float width, int xCoord, int yCoord, int zCoord, GameObject[,,]tileRef)
    {
        int xTrack = xCoord;
        int yTrack = yCoord;
        int zTrack = zCoord;
        EvenRows = 1;
       for(float y = 0; y < height; y += .24f)
        {
            for (float x = xCoord* 1.734f; x <= length + (xCoord * 1.734f); x += 1.734f)
            {
                EvenRows = 1;
                for(float z = zCoord * 1.502f; z <= width + (zCoord * 1.502f); z += 1.502f)
                {
                    if (EvenRows % 2 != 0)
                    {
                        tilePlacement = new Vector3(x, y, z);
                        Instantiate(landTile, tilePlacement, Quaternion.Euler(90, 0, 0));
                    }
                    else
                    {
                        tilePlacement = new Vector3(x + (1.734f / 2), y, z);
                        Instantiate(landTile, tilePlacement, Quaternion.Euler(90, 0, 0));
                    }
                    EvenRows++;
                }
            }
        }
    }
    void MesaTest(int height, int width, int length, int xStart, int zStart, GameObject[,,] tileRef)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = xStart; x < width + xStart; x++)
            {
                for (int z = zStart; z < length + zStart; z++)
                {
                    PlaceAtLowest(x, z, tileRef);
                    System.Console.Write("Tile Placed");
                }
            }
        }
    }
    /*DISCLAIMER: I have NO idea why HillTest works, while SpawnHillFinal doesn't. Will look into it at some point after I'm done riding the high from getting it to work.
     * HOW IT WORKS:
     * r = radius of the hill's base
     * h = the maximum height of the hill
     * s = how high each level of the hill is (ex. with s of 4, each radius is placed 4 times
     * x = the x location
     * z = the z location
     * Starts by spawning s number of circles of r radius, decrementing h once per circle, then decrementing r and repeating
     * If h ever = 0, then the hill is at its maximum height. As such, the function returns without going any higher.
     * */
    void HillTest(int r, int h, int s, int x, int z)
    {
        int sl = h / r;
        for(int a = r; a > 0; a--)
        {
            for (int b = s; b > 0; b--)
            {
                CircleTest(a, x, z, tileRef);
                h--;
                if(h == 0)
                {
                    return;
                }
            }
        }
    }

    //Deprecated due to not working at all.
    void SpawnHillFinal(int height, int radius, int xLoc, int zLoc, GameObject[,,] tileRef)
    {
        int xReset = xLoc;
        int zReset = zLoc;
        for(int i = radius; i > 0; i--)
        {
            CircleTest(i, xLoc, zLoc, tileRef);
            xLoc = xReset;
            zLoc = zReset;
        }
    }
    //Deprecated due to hard coding. Merely a test of SpawnCircle if anything.
    void SpawnHill()
    {
        SpawnCircle(3, 10, 10, 10);
        SpawnCircle(2, 10, 10.24f, 10);
        SpawnCircle(1, 10, 10.48f, 10);
    }
    //Deprecated due to requiring engine coordinates rather than the coordinates provided by the tile array.
    void SpawnHillTest(float radius, float height, int xCoord, int yCoord, int zCoord)
    {
        int scale = Mathf.RoundToInt(height / radius);

        float y = .24f;
        if (radius <= height)
        {
            while (radius > 0)
            {
                for (int i = 0; i < scale; i++)
                {
                    SpawnCircle(Mathf.RoundToInt(radius), xCoord, yCoord + y, zCoord);
                    y += .24f;
                }
                radius--;
            }
        }
        else
        {
            while(height > 0)
            {
                SpawnCircle(Mathf.RoundToInt(radius), xCoord, yCoord + y, zCoord);
                radius--;
                height--;
                y += .24f;
            }
        }
    }
    void SpawnOverhang()
    {

    }
    void SpawnBridge()
    {

    }
    void CreateRavine(int xStart, int zStart, int length, int width)
    {
        int xDirCheck;
        for (int i = 0; i < length; i++)
        {
            if((zStart + i) % 2 == 0)
            {
                xDirCheck = Random.Range(0, 2);
            }
            else
            {
                xDirCheck = Random.Range(-1, 1);
            }

            xStart += xDirCheck;
            //int zDirCheck = Random.Range(-1, 2);
            //zStart += zDirCheck;
            if (xStart > 49 || xStart < 0 || zStart > 49 || zStart < 0)
            {
                return;
            }
            for (int y = 49; y > 0; y--)
            {
                for (int w = 0; w < width; w++)
                {
                    Destroy(tileRef[xStart + w, y, zStart + i]);
                }
            }
        }
    }
    void SpawnArchway(int x, int z, int l, int w, int h)
    {
        int s = h / w;
        int y = PlaceAtLowestReturnY(x, z, tileRef);
        Destroy(tileRef[x, y, z]);
        /*
        int xReturn = x;
        int zReturn = z;
        int archPeak = w / 2;
        x -= archPeak;
        int y = PlaceAtLowestReturnY(x, z, tileRef);
        for (int k = y; k < y + h; k++)
        {
            for (int scale = 0; scale < s; scale++)
            {
                for (int i = x; i < x + w; i++)
                {
                    //PlaceTile(i, k, z, tileRef);
                    PlaceAtLowest(i, z, tileRef);
                }
            }
            x++;
            w -= 2;
        }
        */
        for (int i = 0; i < w/2; i++)
        {
            y = -1 * Mathf.RoundToInt(s * Mathf.Pow(i, 2)) + h;
            if(y < 0)
            {
                return;
            }
            int yNext = -1 * Mathf.RoundToInt(s * Mathf.Pow(i+1, 2)) + h;
            //Instantiate(landTile, new Vector3(i, y, z), Quaternion.Euler(90, 0, 0));
            //y = -(i ^ 2) + 10;
            //Instantiate(landTile, new Vector3(-i, y, z), Quaternion.Euler(90, 0, 0));
            while (y >= yNext)
            {
                if(y < 0)
                {
                    return;
                }
                PlaceTile(x + i, y, z, tileRef);
                PlaceTile(x - i, y, z, tileRef);
                y--;
            }
        }
    }
    void SpawnSlope(int x, int y, int z, int h, int l, int w, int d)
    {
        int s = h / l;
        for(int i = x; i < x+w; i++)
        {
            for (int j = z; j < z + l; j++)
            {
                for (int k = y + h; k > 0; k--)
                {
                    PlaceAtLowest(i, j, tileRef);
                }
                
            }
            h -= s;
        }
    }
    void SpawnTriPiece()
    {

    }
    void SpawnFloatingIsland()
    {

    }
    void CreateTunnel(int x, int y, int z, int l, int w, int h)
    {
        for(int i = 0; i < l; i++)
        {
            SpawnArchway(x, z + i, l, w, h);
        }
    }
    void PlaceTile(int x, int y, int z, GameObject[,,] tileRef)
    {
        float xCoord = tileRef[x, 0, z].transform.position.x;
        float zCoord = tileRef[x, 0, z].transform.position.z;
        tileRef[x, y, z] = Instantiate(landTile, new Vector3(xCoord, (y * .24f) + .12f, zCoord), Quaternion.Euler(90, 0, 0));
    }
    /*PlaceAtLowest is used to ensure that no floating hexagons are accidentaly spawned.
     * HOW IT WORKS
    --Begins by setting y to 40, which for now is well below the ceiling permitted by the bounds of the tile array (will be changed to be scalable upon further fine tuning
    --Checks for the closest existing tile positioned directly below the tile waiting to be placed
    --Records the x and z positions of said tile, as it is easier to get the position from an existing tile than make an algorithm to derive the new tile's coordinates
    --Then spawns a new tile directly above said tile, and adds the new tile to the tile array
    */
    void PlaceAtLowest(int x, int z, GameObject[,,] tileRef)
    {
        if(x > 49 || z > 49 || x < 0 || z < 0)
        {
            return;
        }
        int y = 49;
        while(tileRef[x, y, z] == null)
        {
            y--;
            if (y < 0)
                return;

        }
        float xCoord = tileRef[x, y, z].transform.position.x;
        float zCoord = tileRef[x, y, z].transform.position.z;
        y++;
        if (y > 49)
            return;
        tileRef[x, y, z] = Instantiate(landTile, new Vector3(xCoord, (y * .24f) + .12f, zCoord), Quaternion.Euler(90, 0, 0));
        CoordSet.SetCoordinates(x, y, z);
        
    }
    //Used for creating archways in order to establish a proper base to build the rest of the structure, as the y coordinate is required in order to create the opening in the archway
    int PlaceAtLowestReturnY(int x, int z, GameObject[,,] tileRef)
    {
        if (x > 49 || z > 49 || x < 0 || z < 0)
        {
            return 0;
        }
        int y = 49;
        while (tileRef[x, y, z] == null)
        {
            y--;
            if (y < 0)
                return 0;

        }
        float xCoord = tileRef[x, y, z].transform.position.x;
        float zCoord = tileRef[x, y, z].transform.position.z;
        y++;
        if (y > 49)
            return 0;
        tileRef[x, y, z] = Instantiate(landTile, new Vector3(xCoord, (y * .24f) + .12f, zCoord), Quaternion.Euler(90, 0, 0));
        return y;
    }
    /* Just like I said, I made an algorithm. this algorithm creates a circle with radius r with no overlapping tiles (keep in mind that radius does not include the centermost tile)
     * HOW IT WORKS
     * Begins by creating a line of (r * 2) - 1 tiles. This line serves as the center line of the circle
     * Then, spawn a line containing 1 less tile than the line before it a distance above and below the center line
     * Every odd numbered line is offset by 1 in order to be in the correct place
     * */
    void CircleTest(int radius, int xStart, int zStart, GameObject[,,] tileRef)
    {
        int zReturn = zStart;
        int xReturn = xStart;
        int tileSides = radius - 1;
        int lengthRef = (radius * 2) - 1;
        xStart -= tileSides;
        for (int j = 0; j < radius; j++)
        {
            for (int i = 0; i < lengthRef; i++)
            {
                PlaceAtLowest(xStart + i, zStart + j, tileRef);
                if(j != 0)
                {
                    PlaceAtLowest(xStart + i, zStart - j, tileRef);
                }
            }
            if(j%2 != 0)
            {
                xStart++;
            }
            lengthRef--;
        }
        xStart = xReturn;
        zStart = zReturn;
        //PlaceAtLowest(xStart, zStart, tileRef);

        /*for (int j = 0; j < tileSides; j++)
        {
            for (int i = 0; i < radius; i++)
            {
                PlaceAtLowest((xStart - radius) + i, zStart + j, tileRef);
                PlaceAtLowest((xStart + radius) - i, zStart + j, tileRef);
                //PlaceAtLowest(xStart + i, zStart - j, tileRef);
                //PlaceAtLowest(xStart - i, zStart - j, tileRef);
            }
            radius--;
        }
        */
    }
    /* SpawnCircle is a key piece of code for making hills, mountains, and depressions
     * HOW IT WORKS
     * Records the input of where the circle is centered, as well as it's radius
     * Depending on the radius, initiates a different case to make a circle of appropriate radius
     * (NOTE: Will be making an algorithm to streamline this process at a future date)
     * */
    void SpawnCircle(int radius, float xLoc, float yLoc, float zLoc)
    {
        switch(radius)
        {
            case 1:
                //center
                Instantiate(landTile, new Vector3(xLoc, yLoc, zLoc), Quaternion.Euler(90, 0, 0));
                break;
            case 2:
                //center
                Instantiate(landTile, new Vector3(xLoc, yLoc, zLoc), Quaternion.Euler(90, 0, 0));
                //first ring
                Instantiate(landTile, new Vector3(xLoc + 1.734f, yLoc, zLoc), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - 1.734f, yLoc, zLoc), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + (1.734f * .5f), yLoc, zLoc + 1.502f), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * .5f), yLoc, zLoc + 1.502f), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + (1.734f * .5f), yLoc, zLoc - 1.502f), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * .5f), yLoc, zLoc - 1.502f), Quaternion.Euler(90, 0, 0));
                break;
            case 3:
                //center
                Instantiate(landTile, new Vector3(xLoc, yLoc, zLoc), Quaternion.Euler(90, 0, 0));
                //first ring
                Instantiate(landTile, new Vector3(xLoc + 1.734f, yLoc, zLoc), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - 1.734f, yLoc, zLoc), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + (1.734f * .5f), yLoc, zLoc + 1.502f), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * .5f), yLoc, zLoc + 1.502f), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + (1.734f * .5f), yLoc, zLoc - 1.502f), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * .5f), yLoc, zLoc - 1.502f), Quaternion.Euler(90, 0, 0));
                //second ring
                Instantiate(landTile, new Vector3(xLoc + (1.734f * 2), yLoc, zLoc), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * 2), yLoc, zLoc), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + (1.734f * 1.5f), yLoc, zLoc + 1.502f), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * 1.5f), yLoc, zLoc + 1.502f), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + (1.734f * 1.5f), yLoc, zLoc - 1.502f), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * 1.5f), yLoc, zLoc - 1.502f), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc, yLoc, zLoc + (1.502f * 2)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc, yLoc, zLoc - (1.502f * 2)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + 1.734f, yLoc, zLoc - (1.502f * 2)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + 1.734f, yLoc, zLoc + (1.502f * 2)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - 1.734f, yLoc, zLoc - (1.502f * 2)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - 1.734f, yLoc, zLoc + (1.502f * 2)), Quaternion.Euler(90, 0, 0));
                break;
            case 4:
                //center
                Instantiate(landTile, new Vector3(xLoc, yLoc, zLoc), Quaternion.Euler(90, 0, 0));
                //first ring
                Instantiate(landTile, new Vector3(xLoc + 1.734f, yLoc, zLoc), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - 1.734f, yLoc, zLoc), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + (1.734f * .5f), yLoc, zLoc + 1.502f), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * .5f), yLoc, zLoc + 1.502f), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + (1.734f * .5f), yLoc, zLoc - 1.502f), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * .5f), yLoc, zLoc - 1.502f), Quaternion.Euler(90, 0, 0));
                //second ring
                Instantiate(landTile, new Vector3(xLoc + (1.734f * 2), yLoc, zLoc), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * 2), yLoc, zLoc), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + (1.734f * 1.5f), yLoc, zLoc + 1.502f), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * 1.5f), yLoc, zLoc + 1.502f), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + (1.734f * 1.5f), yLoc, zLoc - 1.502f), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * 1.5f), yLoc, zLoc - 1.502f), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc, yLoc, zLoc + (1.502f * 2)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc, yLoc, zLoc - (1.502f * 2)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + 1.734f, yLoc, zLoc - (1.502f * 2)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + 1.734f, yLoc, zLoc + (1.502f * 2)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - 1.734f, yLoc, zLoc - (1.502f * 2)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - 1.734f, yLoc, zLoc + (1.502f * 2)), Quaternion.Euler(90, 0, 0));
                //third ring
                Instantiate(landTile, new Vector3(xLoc + (1.734f* 3), yLoc, zLoc), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * 3), yLoc, zLoc), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + (1.734f * 2.5f), yLoc, zLoc + (1.502f)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + (1.734f * 2.5f), yLoc, zLoc - (1.502f)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * 2.5f), yLoc, zLoc + (1.502f)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * 2.5f), yLoc, zLoc - (1.502f)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + (1.734f * 2), yLoc, zLoc + (1.502f * 2f)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + (1.734f * 2), yLoc, zLoc - (1.502f * 2f)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * 2), yLoc, zLoc + (1.502f * 2f)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * 2), yLoc, zLoc - (1.502f * 2f)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + (1.734f * 1.5f), yLoc, zLoc + (1.502f * 3f)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + (1.734f * 1.5f), yLoc, zLoc - (1.502f * 3f)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * 1.5f), yLoc, zLoc + (1.502f * 3f)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * 1.5f), yLoc, zLoc - (1.502f * 3f)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + (1.734f * .5f), yLoc, zLoc + (1.502f * 3)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + (1.734f * .5f), yLoc, zLoc - (1.502f * 3)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * .5f), yLoc, zLoc + (1.502f * 3)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * .5f), yLoc, zLoc - (1.502f * 3)), Quaternion.Euler(90, 0, 0));
                break;
            case 5:
                //center
                Instantiate(landTile, new Vector3(xLoc, yLoc, zLoc), Quaternion.Euler(90, 0, 0));
                //first ring
                Instantiate(landTile, new Vector3(xLoc + 1.734f, yLoc, zLoc), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - 1.734f, yLoc, zLoc), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + (1.734f * .5f), yLoc, zLoc + 1.502f), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * .5f), yLoc, zLoc + 1.502f), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + (1.734f * .5f), yLoc, zLoc - 1.502f), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * .5f), yLoc, zLoc - 1.502f), Quaternion.Euler(90, 0, 0));
                //second ring
                Instantiate(landTile, new Vector3(xLoc + (1.734f * 2), yLoc, zLoc), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * 2), yLoc, zLoc), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + (1.734f * 1.5f), yLoc, zLoc + 1.502f), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * 1.5f), yLoc, zLoc + 1.502f), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + (1.734f * 1.5f), yLoc, zLoc - 1.502f), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * 1.5f), yLoc, zLoc - 1.502f), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc, yLoc, zLoc + (1.502f * 2)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc, yLoc, zLoc - (1.502f * 2)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + 1.734f, yLoc, zLoc - (1.502f * 2)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + 1.734f, yLoc, zLoc + (1.502f * 2)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - 1.734f, yLoc, zLoc - (1.502f * 2)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - 1.734f, yLoc, zLoc + (1.502f * 2)), Quaternion.Euler(90, 0, 0));
                //third ring
                Instantiate(landTile, new Vector3(xLoc + (1.734f * 3), yLoc, zLoc), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * 3), yLoc, zLoc), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + (1.734f * 2.5f), yLoc, zLoc + (1.502f)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + (1.734f * 2.5f), yLoc, zLoc - (1.502f)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * 2.5f), yLoc, zLoc + (1.502f)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * 2.5f), yLoc, zLoc - (1.502f)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + (1.734f * 2), yLoc, zLoc + (1.502f * 2f)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + (1.734f * 2), yLoc, zLoc - (1.502f * 2f)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * 2), yLoc, zLoc + (1.502f * 2f)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * 2), yLoc, zLoc - (1.502f * 2f)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + (1.734f * 1.5f), yLoc, zLoc + (1.502f * 3f)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + (1.734f * 1.5f), yLoc, zLoc - (1.502f * 3f)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * 1.5f), yLoc, zLoc + (1.502f * 3f)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * 1.5f), yLoc, zLoc - (1.502f * 3f)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + (1.734f * .5f), yLoc, zLoc + (1.502f * 3)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + (1.734f * .5f), yLoc, zLoc - (1.502f * 3)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * .5f), yLoc, zLoc + (1.502f * 3)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * .5f), yLoc, zLoc - (1.502f * 3)), Quaternion.Euler(90, 0, 0));
                //fourth ring
                Instantiate(landTile, new Vector3(xLoc + (1.734f * 4), yLoc, zLoc), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * 4), yLoc, zLoc), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + (1.734f * 3.5f), yLoc, zLoc + (1.502f)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + (1.734f * 3.5f), yLoc, zLoc - (1.502f)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * 3.5f), yLoc, zLoc + (1.502f)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * 3.5f), yLoc, zLoc - (1.502f)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + (1.734f * 3), yLoc, zLoc + (1.502f * 2)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + (1.734f * 3), yLoc, zLoc - (1.502f * 2)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * 3), yLoc, zLoc + (1.502f * 2)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * 3), yLoc, zLoc - (1.502f * 2)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + (1.734f * 2.5f), yLoc, zLoc + (1.502f * 3)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + (1.734f * 2.5f), yLoc, zLoc - (1.502f * 3)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * 2.5f), yLoc, zLoc + (1.502f * 3)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * 2.5f), yLoc, zLoc - (1.502f * 3)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + (1.734f * 2f), yLoc, zLoc + (1.502f * 4)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + (1.734f * 2f), yLoc, zLoc - (1.502f * 4)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * 2f), yLoc, zLoc + (1.502f * 4)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * 2f), yLoc, zLoc - (1.502f * 4)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + (1.734f * 1f), yLoc, zLoc + (1.502f * 4)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc + (1.734f * 1f), yLoc, zLoc - (1.502f * 4)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * 1f), yLoc, zLoc + (1.502f * 4)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * 1f), yLoc, zLoc - (1.502f * 4)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc, yLoc, zLoc + (1.502f * 4)), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc, yLoc, zLoc - (1.502f * 4)), Quaternion.Euler(90, 0, 0));
                break;
            case 6:
                //fifth ring
                Instantiate(landTile, new Vector3(xLoc + (1.734f * 5), yLoc, zLoc), Quaternion.Euler(90, 0, 0));
                Instantiate(landTile, new Vector3(xLoc - (1.734f * 5), yLoc, zLoc), Quaternion.Euler(90, 0, 0));
                break;
            case 7:
                break;
            case 8:
                break;
            case 9:
                break;
        }
    }
	// Update is called once per frame
	void Update () {
		
	}
}
