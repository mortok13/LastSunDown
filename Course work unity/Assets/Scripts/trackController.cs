using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class trackController : MonoBehaviour
{
    [SerializeField]
    private Block[] backgroundBlocks;
    [SerializeField]
    private Block[] roadBlocks;
    [SerializeField]
    private Block[] crossroadBlocks;
    [SerializeField]
    private Block[] singleRotateBlocks;
    public static float NotRotChance;


    private Block lastPlacedRightBGblock;
    private Block lastPlacedLeftBGBlock;

    //main track GO array
    private GameObject[] track = new GameObject[10];
    private int rSeed;
                        // x, z
    private static Vector2 leftBGCurPoint;
    private static Vector2 rightBGCurPoint;
                            // x, z
    private static Vector2 roadCurPoint;
    private static int deleteIndex;
    private static short curIndex;
    private static float roadAngle;
    //TODO: Сократить deltaRoadPoint до двух элементов(оставить [0] и [2])
    private Vector2[] deltaRoadPoint = {new Vector2(1,0), new Vector2(0,1), new Vector2(-1,0), new Vector2(0, -1)};
    private static float backgroundAngle;
    //////////////////////////////////////////////////////////////////////////////
    //Creating a single rotation block cluster
                                                //false - left, right - true
    void CreateSingleRot(GameObject resultContainer,int indexX, bool side)
    {
        
        int indexY = -1;
        Vector3 resContTrnsfrm = resultContainer.transform.position;
        switch(indexX)
        {
            case 0:
            //CreateRotCluster(indexX, 1 - indexY % 3)
                if(!side)       // up
                {
                    indexY = 1;
                    leftBGCurPoint.x = resContTrnsfrm.x - 1;
                                                        //
                    leftBGCurPoint.y += resContTrnsfrm.z + 0.5f + lastPlacedLeftBGBlock.sizeXZ.y;
                    rightBGCurPoint.x = resContTrnsfrm.x + 1;
                    rightBGCurPoint.y = resContTrnsfrm.z - 0.5f;
                }
                else            // down
                {
                    indexY = 3;
                    while(leftBGCurPoint.x < roadCurPoint.x)
                    {
                        int temp = Random.Range(0, backgroundBlocks.Length);  
                        Instantiate(backgroundBlocks[temp].block, new Vector3 (leftBGCurPoint.x + backgroundBlocks[temp].sizeXZ.x/2, 0, leftBGCurPoint.y), Quaternion.Euler(0, backgroundAngle, 0), track[curIndex].transform);
                        leftBGCurPoint.x += backgroundBlocks[temp].sizeXZ.x;
                        lastPlacedLeftBGBlock = backgroundBlocks[temp];
                    }
                    leftBGCurPoint.x = resContTrnsfrm.x + 1;
                    leftBGCurPoint.y = resContTrnsfrm.z + 0.5f;
                    rightBGCurPoint.x = resContTrnsfrm.x - 1;
                    rightBGCurPoint.y = resContTrnsfrm.z - 0.5f;
                }
                break;
            case 1:
                if(!side)       // left
                {
                    indexY = 2;
                    while(rightBGCurPoint.y < roadCurPoint.y)
                    {
                        int temp = Random.Range(0, backgroundBlocks.Length);  
                        Instantiate(backgroundBlocks[temp].block, new Vector3 (rightBGCurPoint.x, 0, rightBGCurPoint.y + backgroundBlocks[temp].sizeXZ.x/2), Quaternion.Euler(0, backgroundAngle + 180, 0), track[curIndex].transform);
                        rightBGCurPoint.y += backgroundBlocks[temp].sizeXZ.x;
                        lastPlacedLeftBGBlock = backgroundBlocks[temp];
                    }
                    rightBGCurPoint.x = resContTrnsfrm.x + 0.5f;
                    rightBGCurPoint.y = resContTrnsfrm.z + 1;
                }
                else            // right
                {
                    indexY = 0;
                    while(leftBGCurPoint.y < resContTrnsfrm.y + 2)
                    {
                        int temp = Random.Range(0, backgroundBlocks.Length);  
                        Instantiate(backgroundBlocks[temp].block, new Vector3 (leftBGCurPoint.x, 0, leftBGCurPoint.y + backgroundBlocks[temp].sizeXZ.x/2), Quaternion.Euler(0, backgroundAngle, 0), track[curIndex].transform);
                        leftBGCurPoint.x += backgroundBlocks[temp].sizeXZ.x;
                        lastPlacedLeftBGBlock = backgroundBlocks[temp];
                    }
                    leftBGCurPoint.x = resContTrnsfrm.x - 0.5f;
                    leftBGCurPoint.y = resContTrnsfrm.z + 1;
                }
                break;
            case 2:
                if(!side)       // down
                {
                    indexY = 3;
                    leftBGCurPoint.x = resContTrnsfrm.x + 1;
                    leftBGCurPoint.y = resContTrnsfrm.z - 0.5f;
                    while(rightBGCurPoint.x > roadCurPoint.x)
                    {
                        int temp = Random.Range(0, backgroundBlocks.Length);  
                        Instantiate(backgroundBlocks[temp].block, new Vector3 (rightBGCurPoint.x - backgroundBlocks[temp].sizeXZ.x/2 , 0, rightBGCurPoint.y), Quaternion.Euler(0, 180 + backgroundAngle, 0), track[curIndex].transform);
                        rightBGCurPoint.x -= backgroundBlocks[temp].sizeXZ.x;
                        lastPlacedRightBGblock = backgroundBlocks[temp];
                    }
                    rightBGCurPoint.x = resContTrnsfrm.x - 1;
                    rightBGCurPoint.y = resContTrnsfrm.z + 0.5f;
                    
                }
                else            // up
                {
                    indexY = 1;
                    leftBGCurPoint.x = resContTrnsfrm.x - 1;
                    leftBGCurPoint.y = resContTrnsfrm.z - 0.5f;
                    rightBGCurPoint.x = resContTrnsfrm.x + 1;
                    rightBGCurPoint.y = resContTrnsfrm.z + 0.5f + (lastPlacedRightBGblock == null ? 0 : lastPlacedRightBGblock.sizeXZ.y);
                }
                break;
            case 3:
                if(!side)       // right
                {
                    indexY = 0;
                    leftBGCurPoint.x = resContTrnsfrm.x + 0.5f + lastPlacedRightBGblock.sizeXZ.y;
                    leftBGCurPoint.y = resContTrnsfrm.z + 1;
                    while(rightBGCurPoint.y > roadCurPoint.y)
                    {
                        int temp = Random.Range(0, backgroundBlocks.Length);                       
                        Instantiate(backgroundBlocks[temp].block, new Vector3(rightBGCurPoint.x, 0, rightBGCurPoint.y - backgroundBlocks[temp].sizeXZ.x/2), Quaternion.Euler(0, 180 + backgroundAngle, 0), track[curIndex].transform);
                        rightBGCurPoint.y-=backgroundBlocks[temp].sizeXZ.x;
                        lastPlacedRightBGblock = backgroundBlocks[temp]; 
                    }
                }
                else            // left
                {
                    indexY = 2;
                    rightBGCurPoint.x = resContTrnsfrm.x - 0.5f - (lastPlacedRightBGblock == null ? 0 : lastPlacedRightBGblock.sizeXZ.y);
                    rightBGCurPoint.y = resContTrnsfrm.z + 1;
                    while(leftBGCurPoint.y > roadCurPoint.y)
                    {
                        int temp = Random.Range(0, backgroundBlocks.Length);                       
                        Instantiate(backgroundBlocks[temp].block, new Vector3(leftBGCurPoint.x, 0, leftBGCurPoint.y - backgroundBlocks[temp].sizeXZ.x/2), Quaternion.Euler(0, backgroundAngle, 0), track[curIndex].transform);
                        leftBGCurPoint.y-=backgroundBlocks[temp].sizeXZ.x;
                        lastPlacedLeftBGBlock = backgroundBlocks[temp];
                    }
                }
                break;
        }
        Debug.Log("Rotation case: " + indexX + " - " + indexY);
        NotRotChance = 2.8f;
    }
    //////////////////////////////////////////////////////////////////////////////
    // 0 - right, 1 - up, 2 - left, 3 - down;
    private static int _curRotMode;
    private int curRotMode {
        get
        {
            return _curRotMode;
        }
        
        set
        {
            if(value > (int)RotModes.down)
            {
                _curRotMode = (int)RotModes.right;
            }
            else if(value < (int)RotModes.right)
            {
                _curRotMode = (int)RotModes.down;
            }
            else
            {
                _curRotMode = value;         
            }

            //horiz road
            if(_curRotMode % 2 == 0)
            {
                roadAngle = 180;
            }
            //vert road
            else
            {
                roadAngle = 270;
            }

            backgroundAngle = -90 * _curRotMode;        
        }
    }

    public int getSeed()
    {
        return rSeed;
    }
    void Awake()
    {
        NotRotChance = 2.8f;
        curRotMode = (int)RotModes.right;
        leftBGCurPoint.x = 0;
        leftBGCurPoint.y = 1;
        roadCurPoint.x = 1;
        roadCurPoint.y = 0;
        deleteIndex = 0;
        curIndex = 1;
        rSeed = Mathf.FloorToInt(System.DateTime.Now.Millisecond * System.DateTime.Now.Second * System.DateTime.Now.Minute * System.DateTime.Now.Hour);
        // ~86 313 600 variations
        
        Random.InitState(rSeed);
        string seedStr = rSeed.ToString("X");
        Debug.Log($"seed is {seedStr}");
        


    }

    void Start()
    {
        track[0] = Instantiate(roadBlocks[Random.Range(0, roadBlocks.Length)].block ,new Vector3(0,0,0), Quaternion.identity, transform);
        Instantiate(backgroundBlocks[5].block, new Vector3 (leftBGCurPoint.x + backgroundBlocks[5].sizeXZ.x/2 - 0.5f, 0, leftBGCurPoint.y), Quaternion.identity, track[0].transform);
        leftBGCurPoint.x += backgroundBlocks[5].sizeXZ.x/2 + 1;
        Debug.Log (leftBGCurPoint);
 Create();Create();Create();Create();Create();Create();Create(); Create();Create();Create();Create();Create();Create();Create();Create();Create();
 Create();Create();Create();Create();Create();Create();Create();
        RunStats.Distance = 5;

       // Destroy(track[1]);
    }

    private void Create()
    {
        
        Debug.Log($"curIndex is {curIndex}");
        int temp;
        //Create road + background
        if(Random.value < NotRotChance)     
        {
            if(NotRotChance > 0.8f)
            {
                NotRotChance--;
            }
            temp = Random.Range(0, roadBlocks.Length);
            track[curIndex] = Instantiate(roadBlocks[temp].block, new Vector3(roadCurPoint.x, 0, roadCurPoint.y), Quaternion.Euler(0, roadAngle - 180 * Random.Range(0, 2), 0), transform);
            roadCurPoint+= deltaRoadPoint[curRotMode];
            CreateRoad();
            CreateBackground();
                   //     float leftOrRight;
            CreateRoad();

            Debug.Log("leftBGCurPoint: " + leftBGCurPoint + "\nrightBGCurPoint: " + rightBGCurPoint);


            Debug.Log("roadCurPoint: " + roadCurPoint);
        }
        //Create rotation
        else
        {
            temp = Random.Range(0, singleRotateBlocks.Length);
            track[curIndex] = Instantiate(singleRotateBlocks[temp].block, new Vector3(roadCurPoint.x, 0, roadCurPoint.y), Quaternion.identity, transform);
            switch(_curRotMode)
            {
                //case left
                case 0:
                    track[curIndex].tag = "rotLeft";
                    CreateSingleRot(track[curIndex],curRotMode, false);  
                    curRotMode++;
                break;
                //case right
                case 1:
                if((int)Random.Range(0,2) == 1)
                {
                    if(roadCurPoint.y != rightBGCurPoint.y + 0.5f)
                    {
                        Destroy(track[curIndex]);
                        return;
                    }
                    track[curIndex].tag = "rotRight";
                    track[curIndex].transform.Rotate(0,180,0);
                    CreateSingleRot(track[curIndex], curRotMode, true);  
                    curRotMode--;
                }
                else
                {
                    if(roadCurPoint.y != leftBGCurPoint.y + 0.5f)
                    {
                        Destroy(track[curIndex]);
                        return;
                    }
                    track[curIndex].tag = "rotLeft";
                    track[curIndex].transform.Rotate(0,-90,0);
                    CreateSingleRot(track[curIndex], curRotMode, false);  
                    curRotMode++;
                }
                break;
                case 2:
                    track[curIndex].tag = "rotRight";
                    track[curIndex].transform.Rotate(0,90, 0);
                    CreateSingleRot(track[curIndex], curRotMode, true);
                    curRotMode--;
                break;
                case 3:
                break;
            }
            roadCurPoint += deltaRoadPoint[curRotMode];
        }
        curIndex++;
        if(curIndex > track.Length - 1)
        {
            curIndex = 0;
        }
    }

    private void CreateBackground()
    {
        int temp = Random.Range(0, backgroundBlocks.Length);
        switch(curRotMode)
        {
            case 0:
                Instantiate(backgroundBlocks[temp].block, new Vector3 (leftBGCurPoint.x + backgroundBlocks[temp].sizeXZ.x/2, 0, leftBGCurPoint.y), Quaternion.Euler(0, backgroundAngle, 0), track[curIndex].transform);
                leftBGCurPoint.x += backgroundBlocks[temp].sizeXZ.x;
                lastPlacedLeftBGBlock = backgroundBlocks[temp];
                break;
            case 1:
                while(leftBGCurPoint.y < roadCurPoint.y)
                {   
                    Instantiate(backgroundBlocks[temp].block, new Vector3 (leftBGCurPoint.x, 0, leftBGCurPoint.y + backgroundBlocks[temp].sizeXZ.x/2 - 1), Quaternion.Euler(0, backgroundAngle, 0), track[curIndex].transform);
                    leftBGCurPoint.y += backgroundBlocks[temp].sizeXZ.x;
                    lastPlacedLeftBGBlock = backgroundBlocks[temp];
                    temp = Random.Range(0, backgroundBlocks.Length);

                }
                while(rightBGCurPoint.y < roadCurPoint.y)
                {
                    Instantiate(backgroundBlocks[temp].block, new Vector3 (rightBGCurPoint.x, 0, rightBGCurPoint.y + backgroundBlocks[temp].sizeXZ.x/2), Quaternion.Euler(0, backgroundAngle + 180, 0), track[curIndex].transform);
                    rightBGCurPoint.y += backgroundBlocks[temp].sizeXZ.x;
                    lastPlacedRightBGblock = backgroundBlocks[temp];
                    temp = Random.Range(0, backgroundBlocks.Length);
                }
                break;
            case 2:
                    // Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                    while(rightBGCurPoint.x > roadCurPoint.x)
                    {
                        Instantiate(backgroundBlocks[temp].block, new Vector3(rightBGCurPoint.x - backgroundBlocks[temp].sizeXZ.x/2, 0, rightBGCurPoint.y), Quaternion.Euler(0, backgroundAngle + 180, 0), track[curIndex].transform);
                        rightBGCurPoint.x -= backgroundBlocks[temp].sizeXZ.x;
                        lastPlacedRightBGblock = backgroundBlocks[temp];
                        temp = Random.Range(0, backgroundBlocks.Length);
                    }
                break;
            case 3:
                while(leftBGCurPoint.y > roadCurPoint.y || rightBGCurPoint.y > roadCurPoint.y)
                {
                    if(leftBGCurPoint.y > roadCurPoint.y)
                    {
                        Instantiate(backgroundBlocks[temp].block, new Vector3(leftBGCurPoint.x, 0, leftBGCurPoint.y - backgroundBlocks[temp].sizeXZ.x/2), Quaternion.Euler(0, backgroundAngle, 0), track[curIndex].transform);
                        leftBGCurPoint.y-=backgroundBlocks[temp].sizeXZ.x;
                        lastPlacedLeftBGBlock = backgroundBlocks[temp];
                        temp = Random.Range(0, backgroundBlocks.Length);
                    }
                
                    if(rightBGCurPoint.y > roadCurPoint.y)
                    {
                        Instantiate(backgroundBlocks[temp].block, new Vector3(rightBGCurPoint.x, 0, rightBGCurPoint.y - backgroundBlocks[temp].sizeXZ.x/2), Quaternion.Euler(0, backgroundAngle + 180, 0), track[curIndex].transform);
                        rightBGCurPoint.y -= backgroundBlocks[temp].sizeXZ.x;
                        lastPlacedRightBGblock = backgroundBlocks[temp];
                        temp = Random.Range(0, backgroundBlocks.Length);
                    }
                }
                break;
        }
    }

    private void CreateRoad()
    {
        float leftOrRight;
        switch(curRotMode)
        {
            case 0:
                for(int i = (int)roadCurPoint.x; i < leftBGCurPoint.x; i++)
                {
                    CreateRoadBlock();
                }
                break;
            case 1:
                leftOrRight = leftBGCurPoint.y < rightBGCurPoint.y ? rightBGCurPoint.y : leftBGCurPoint.y;
                for(int i = (int)roadCurPoint.y; i < leftOrRight - 1; i++ )
                {
                    CreateRoadBlock();
                }
                break;
            case 2:
                for(int i = (int)roadCurPoint.x; i > rightBGCurPoint.x; i--)
                {
                    CreateRoadBlock();
                }
                break;
            case 3:
                leftOrRight = leftBGCurPoint.y > rightBGCurPoint.y ? rightBGCurPoint.y : leftBGCurPoint.y;
                for(int i = (int)roadCurPoint.y; i > leftOrRight; i--)
                {
                    CreateRoadBlock();
                }
                break;
        }
    }

    private void CreateRoadBlock()
    {
        int temp = Random.Range(0, roadBlocks.Length);
        Instantiate(roadBlocks[temp].block, new Vector3(roadCurPoint.x, 0, roadCurPoint.y), Quaternion.Euler(0, roadAngle - 180 * Random.Range(0, 2), 0), track[curIndex].transform);
        roadCurPoint += deltaRoadPoint[curRotMode];
    }
}
