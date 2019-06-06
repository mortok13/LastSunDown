using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class trackController : MonoBehaviour
{
    [SerializeField]
    private Block[] TRotBlocks;
    [SerializeField]
    private Block[] backgroundBlocks;
    [SerializeField]
    private Block[] roadBlocks;
    [SerializeField]
    private Block[] crossroadBlocks;
    [SerializeField]
    private GameObject[] coins;
    [SerializeField]
    private Block[] singleRotateBlocks;
    public static float NotRotChance;
    private Queue<GameObject> track;
    private GameObject trackClusterForDelete;
    private Block lastPlacedRightBGblock;
    private Block lastPlacedLeftBGBlock;

    //main track GO array
    private GameObject curTrackCluster;
    private int rSeed;
                        // x, z
    private static Vector2 leftBGCurPoint;
    private static Vector2 rightBGCurPoint;
                            // x, z
    private static Vector2 roadCurPoint;
    private static int deleteIndex;
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
                    leftBGCurPoint.y += resContTrnsfrm.z + 0.5f + lastPlacedLeftBGBlock.sizeXZ.y - 1;
                    rightBGCurPoint.x = resContTrnsfrm.x + 1;
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
                        Instantiate(backgroundBlocks[temp].block, new Vector3 (rightBGCurPoint.x, 0, rightBGCurPoint.y + backgroundBlocks[temp].sizeXZ.x/2), Quaternion.Euler(0, backgroundAngle + 180, 0), curTrackCluster.transform);
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
                        Instantiate(backgroundBlocks[temp].block, new Vector3 (leftBGCurPoint.x, 0, leftBGCurPoint.y + backgroundBlocks[temp].sizeXZ.x/2), Quaternion.Euler(0, backgroundAngle, 0), curTrackCluster.transform);
                        leftBGCurPoint.x += backgroundBlocks[temp].sizeXZ.x;
                        lastPlacedLeftBGBlock = backgroundBlocks[temp];
                    }
                    leftBGCurPoint.x = resContTrnsfrm.x - 0.5f;
                    leftBGCurPoint.y = resContTrnsfrm.z + 1;
                }
                break;
            case 2:
                if(side)          // up
                {
                    indexY = 1;
                    leftBGCurPoint.x = resContTrnsfrm.x - 1;
                    leftBGCurPoint.y = resContTrnsfrm.z - 0.5f;
                    rightBGCurPoint.x = resContTrnsfrm.x + 1;
                    rightBGCurPoint.y = resContTrnsfrm.z + 0.5f + (lastPlacedRightBGblock == null ? 0 : lastPlacedRightBGblock.sizeXZ.y);
                }
                break;
        }
        Debug.Log("Rotation case: " + indexX + " - " + indexY);
        NotRotChance = 4.8f;
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
                //roadAngle = 180;
                roadAngle = 0;
            }
            //vert road
            else
            {
                roadAngle = 270;
            }

            backgroundAngle = -90 * _curRotMode;        
        }
    }

    private static bool _isWaiting;

    public static bool IsWaiting
    {
        get
        {
            return _isWaiting;
        }
        private set
        {
            _isWaiting = value;
        }
    }
    private static bool _chosenSide;
    public static bool ChosenSide
    {
        get
        {
            return _chosenSide;
        }
        set
        {
            _chosenSide = value;
            IsWaiting = false;
        }
    }
    public int getSeed()
    {
        return rSeed;
    }
    void Awake()
    {
        track = new Queue<GameObject>();
        _chosenSide = false;
        _isWaiting = false;
        NotRotChance = 2.8f;
        curRotMode = (int)RotModes.right;
        leftBGCurPoint.x = 0;
        leftBGCurPoint.y = 1;
        roadCurPoint.x = 1;
        roadCurPoint.y = 0;
        deleteIndex = 0;
        rSeed = Mathf.FloorToInt(System.DateTime.Now.Millisecond * 
                                 (System.DateTime.Now.Second+1) * 
                                 (System.DateTime.Now.Minute+1) * 
                                 (System.DateTime.Now.Hour+1) * 
                                 System.DateTime.Now.Day * 
                                 System.DateTime.Now.Month * 
                                 System.DateTime.Now.Year);
        // ~86 313 600 variations
        
        Random.InitState(rSeed);
        string seedStr = rSeed.ToString("X");
        Debug.Log($"seed is {seedStr}");
        RunStats.Seed = seedStr;


    }

    void Start()
    {
        curTrackCluster = Instantiate(roadBlocks[Random.Range(0, roadBlocks.Length)].block ,new Vector3(0,0,0), Quaternion.identity, transform);
        Instantiate(backgroundBlocks[5].block, new Vector3 (leftBGCurPoint.x + backgroundBlocks[5].sizeXZ.x/2 - 0.5f, 0, leftBGCurPoint.y), Quaternion.identity, curTrackCluster.transform);
        leftBGCurPoint.x += backgroundBlocks[5].sizeXZ.x/2 + 1;
        Debug.Log (leftBGCurPoint);
        trackClusterForDelete = curTrackCluster;
      //  Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();
       // Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();
        StartCoroutine("ControlGenerate");
        //RunStats.Distance = 5;

       // Destroy(track[1]);
    }

    private void Create()
    {
        int temp;
        //Create road + background
        if(Random.value < NotRotChance - 0.5f)     
        {
            if(NotRotChance > 0.8f)
            {
                NotRotChance--;
            }
            temp = Random.Range(0, roadBlocks.Length);
            curTrackCluster = Instantiate(roadBlocks[temp].block, new Vector3(roadCurPoint.x, 0, roadCurPoint.y), Quaternion.Euler(0, roadAngle /*- 180 * Random.Range(0, 2)*/, 0), transform);
            roadCurPoint+= deltaRoadPoint[curRotMode];
            CreateRoad();
            CreateBackground();
            CreateRoad();
            Debug.Log("leftBGCurPoint: " + leftBGCurPoint + "\nrightBGCurPoint: " + rightBGCurPoint);


            Debug.Log("roadCurPoint: " + roadCurPoint);
        }
        //Create rotation
        else
        {
            if(Random.value < 0.5f)
            {
                Vector2 tmpBGCurPoint;
                temp = Random.Range(0, TRotBlocks.Length);
                float rightOrLeft;
                switch(_curRotMode)
                {
                    case 0:
                        curTrackCluster = Instantiate(TRotBlocks[temp].block, new Vector3(roadCurPoint.x, 0, roadCurPoint.y), Quaternion.Euler(0, 180, 0), transform);
                        tmpBGCurPoint = new Vector2(leftBGCurPoint.x, leftBGCurPoint.y + lastPlacedLeftBGBlock.sizeXZ.y);
                        tmpBGCurPoint.x-=0.5f;
                        tmpBGCurPoint.y-=0.5f;
                        temp = Random.Range(0, backgroundBlocks.Length);
                        Instantiate(backgroundBlocks[temp].block, new Vector3(tmpBGCurPoint.x, 0, tmpBGCurPoint.y + backgroundBlocks[temp].sizeXZ.x/2), Quaternion.Euler(0,-90,0), curTrackCluster.transform);
                        tmpBGCurPoint.y += backgroundBlocks[temp].sizeXZ.x;
                        leftBGCurPoint.x++;
                        CreateBackground();
                        rightBGCurPoint = new Vector2(leftBGCurPoint.x - lastPlacedLeftBGBlock.sizeXZ.x + 0.5f, leftBGCurPoint.y - 0.5f + lastPlacedLeftBGBlock.sizeXZ.y);
                        temp = Random.Range(0, backgroundBlocks.Length);
                        Instantiate(backgroundBlocks[temp].block, new Vector3(rightBGCurPoint.x, 0,rightBGCurPoint.y + backgroundBlocks[temp].sizeXZ.x/2), Quaternion.Euler(0, 90,0), curTrackCluster.transform);
                        rightBGCurPoint.y += backgroundBlocks[temp].sizeXZ.x;
                        rightOrLeft = rightBGCurPoint.y < tmpBGCurPoint.y ? tmpBGCurPoint.y : rightBGCurPoint.y; 
                        for(int i = (int)roadCurPoint.y + 1; i < rightOrLeft; i++)
                        {
                            temp = Random.Range(0, roadBlocks.Length);
                            Instantiate(roadBlocks[temp].block, new Vector3(roadCurPoint.x, 0, i), Quaternion.Euler(0, 90, 0), curTrackCluster.transform);
                            if(Random.value < (2f/5))
                            {
                                Instantiate(coins[Random.Range(0, coins.Length)], new Vector3(roadCurPoint.x, 0, i), Quaternion.Euler(0, 90, 0), curTrackCluster.transform);
                            }
                        }
                        roadCurPoint.x++;
                        CreateRoad();
                    break;
                    case 1:
                        temp = Random.Range(0, 2);
                        //temp = 0;
                        switch(temp)
                        {
                            case 0:
                            if(roadCurPoint.y < rightBGCurPoint.y + 0.5f)
                            {
                                return;
                            }
                            temp = Random.Range(0, TRotBlocks.Length);
                            curTrackCluster = Instantiate(TRotBlocks[temp].block, new Vector3(roadCurPoint.x, 0, roadCurPoint.y), Quaternion.Euler(0, -90, 0), transform);
                            tmpBGCurPoint = new Vector2(roadCurPoint.x + 0.5f, roadCurPoint.y + 1);
                            temp = Random.Range(0, backgroundBlocks.Length);
                            Instantiate(backgroundBlocks[temp].block, new Vector3(tmpBGCurPoint.x + backgroundBlocks[temp].sizeXZ.x/2, 0, tmpBGCurPoint.y), Quaternion.identity, curTrackCluster.transform);
                            tmpBGCurPoint.x += backgroundBlocks[temp].sizeXZ.x;
                            rightBGCurPoint.y = tmpBGCurPoint.y + backgroundBlocks[temp].sizeXZ.y - 0.5f;
                            for(int i = (int)roadCurPoint.x + 1; i < tmpBGCurPoint.x; i++)
                            {
                                temp = Random.Range(0, roadBlocks.Length);
                                Instantiate(roadBlocks[temp].block, new Vector3(i, 0, roadCurPoint.y), Quaternion.identity, curTrackCluster.transform);
                                if(Random.value < (2f/5))
                                {
                                    Instantiate(coins[Random.Range(0, coins.Length)], new Vector3(i, 0, roadCurPoint.y), Quaternion.identity, curTrackCluster.transform);
                                }
                            }
                            roadCurPoint.y++;
                            for(int i = (int)roadCurPoint.y; i <= tmpBGCurPoint.y - 0.5f + backgroundBlocks[temp].sizeXZ.y; i++)
                            {
                                temp = Random.Range(0, roadBlocks.Length);
                                Instantiate(roadBlocks[temp].block, new Vector3(roadCurPoint.x, 0, i), Quaternion.Euler(0, roadAngle, 0), curTrackCluster.transform);
                                if(Random.value < (2f/5))
                                {
                                    Instantiate(coins[Random.Range(0, coins.Length)], new Vector3(roadCurPoint.x, 0, i), Quaternion.Euler(0, roadAngle, 0), curTrackCluster.transform);
                                }
                                roadCurPoint.y++;
                            }
                          //  rightBGCurPoint.y = tmpBGCurPoint.y + 0.5f;
                            
                            break;
                            // доделать нужно
                            case 1:
                                // while(roadCurPoint.y < leftBGCurPoint.y || roadCurPoint.y < rightBGCurPoint.y)
                                // {
                                //     CreateRoadBlock();
                                // }
                                temp = Random.Range(0, TRotBlocks.Length);
                                curTrackCluster = Instantiate(TRotBlocks[temp].block, new Vector3(roadCurPoint.x, 0, roadCurPoint.y), Quaternion.identity, transform);
                                tmpBGCurPoint = new Vector2(roadCurPoint.x, roadCurPoint.y + 1);
                                temp = Random.Range(0, backgroundBlocks.Length);
                                Instantiate(backgroundBlocks[temp].block, new Vector3(tmpBGCurPoint.x, 0, tmpBGCurPoint.y), Quaternion.identity, curTrackCluster.transform);
                                Vector2 scndTmpBGCurPoint = new Vector2(tmpBGCurPoint.x, tmpBGCurPoint.y);
                                tmpBGCurPoint.x += backgroundBlocks[temp].sizeXZ.x/2;
                                scndTmpBGCurPoint.x -= backgroundBlocks[temp].sizeXZ.x/2;
                                temp = Random.Range(0, backgroundBlocks.Length);
                                Instantiate(backgroundBlocks[temp].block, new Vector3(tmpBGCurPoint.x + backgroundBlocks[temp].sizeXZ.x/2, 0, tmpBGCurPoint.y), Quaternion.identity, curTrackCluster.transform);
                                tmpBGCurPoint.x += backgroundBlocks[temp].sizeXZ.x;
                                temp = Random.Range(0, backgroundBlocks.Length);
                                Instantiate(backgroundBlocks[temp].block, new Vector3(scndTmpBGCurPoint.x - backgroundBlocks[temp].sizeXZ.x/2, 0, tmpBGCurPoint.y), Quaternion.identity, curTrackCluster.transform);
                                scndTmpBGCurPoint.x -= backgroundBlocks[temp].sizeXZ.x;
                                for(int i = (int)roadCurPoint.x + 1; i <= tmpBGCurPoint.x; i++)
                                {
                                    temp = Random.Range(0, roadBlocks.Length);
                                    Instantiate(roadBlocks[temp].block, new Vector3(i, 0, roadCurPoint.y), Quaternion.identity, curTrackCluster.transform);
                                    if(Random.value < (2f/5))
                                    {
                                        Instantiate(coins[Random.Range(0, coins.Length)], new Vector3(i, 0, roadCurPoint.y), Quaternion.Euler(0, roadAngle, 0), curTrackCluster.transform);
                                    }
                                }
                                for(int i = (int)roadCurPoint.x - 1; i >= scndTmpBGCurPoint.x; i--)
                                {
                                    temp = Random.Range(0, roadBlocks.Length);
                                    Instantiate(roadBlocks[temp].block, new Vector3(i, 0, roadCurPoint.y), Quaternion.identity, curTrackCluster.transform);
                                    if(Random.value < (2f/5))
                                    {
                                        Instantiate(coins[Random.Range(0, coins.Length)], new Vector3(i, 0, roadCurPoint.y), Quaternion.Euler(0, roadAngle, 0), curTrackCluster.transform);
                                    }
                                }
                            break;
                            case 2:
                                if(roadCurPoint.y < leftBGCurPoint.y + 0.5f)
                                {
                                    return;
                                }
                                temp = Random.Range(0, TRotBlocks.Length);
                                curTrackCluster = Instantiate(TRotBlocks[temp].block, new Vector3(roadCurPoint.x, 0, roadCurPoint.y), Quaternion.Euler(0, 90, 0), transform);
                                tmpBGCurPoint = new Vector2(roadCurPoint.x - 0.5f, roadCurPoint.y + 1);
                                temp = Random.Range(0, backgroundBlocks.Length);
                                Instantiate(backgroundBlocks[temp].block, new Vector3(tmpBGCurPoint.x - backgroundBlocks[temp].sizeXZ.x/2, 0, tmpBGCurPoint.y), Quaternion.identity, curTrackCluster.transform);
                                tmpBGCurPoint.x -= backgroundBlocks[temp].sizeXZ.x;
                                leftBGCurPoint.y = tmpBGCurPoint.y + backgroundBlocks[temp].sizeXZ.y - 0.5f;
                                for(int i = (int)roadCurPoint.x - 1; i > tmpBGCurPoint.x; i--)
                                {
                                    temp = Random.Range(0, roadBlocks.Length);
                                    Instantiate(roadBlocks[temp].block, new Vector3(i, 0, roadCurPoint.y), Quaternion.identity, curTrackCluster.transform);
                                    if(Random.value < (2f/5))
                                    {
                                        Instantiate(coins[Random.Range(0, coins.Length)], new Vector3(i, 0, roadCurPoint.y), Quaternion.identity, curTrackCluster.transform);
                                    }
                                }
                                roadCurPoint.y++;
                                for(int i = (int)roadCurPoint.y; i <= tmpBGCurPoint.y - 0.5f + backgroundBlocks[temp].sizeXZ.y; i++)
                                {
                                    temp = Random.Range(0, roadBlocks.Length);
                                    Instantiate(roadBlocks[temp].block, new Vector3(roadCurPoint.x, 0, i), Quaternion.Euler(0, roadAngle, 0), curTrackCluster.transform);
                                    if(Random.value < (2f/5))
                                    {
                                        Instantiate(coins[Random.Range(0, coins.Length)], new Vector3(roadCurPoint.x, 0, i), Quaternion.Euler(0, roadAngle, 0), curTrackCluster.transform);
                                    }
                                    roadCurPoint.y++;
                                }   
                            break;
                        }
                    break;
                    
                    case 2:
                        curTrackCluster = Instantiate(TRotBlocks[temp].block, new Vector3(roadCurPoint.x, 0, roadCurPoint.y), Quaternion.Euler(0, 180, 0), transform);
                        tmpBGCurPoint = new Vector2(rightBGCurPoint.x, rightBGCurPoint.y + lastPlacedRightBGblock.sizeXZ.y);
                        tmpBGCurPoint.x+=0.5f;
                        tmpBGCurPoint.y-=0.5f;
                        temp = Random.Range(0, backgroundBlocks.Length);
                        Instantiate(backgroundBlocks[temp].block, new Vector3(tmpBGCurPoint.x, 0, tmpBGCurPoint.y + backgroundBlocks[temp].sizeXZ.x/2), Quaternion.Euler(0, 90, 0), curTrackCluster.transform);
                        tmpBGCurPoint.y += backgroundBlocks[temp].sizeXZ.x;
                        rightBGCurPoint.x--;
                        CreateBackground();
                        leftBGCurPoint = new Vector2(rightBGCurPoint.x + lastPlacedRightBGblock.sizeXZ.x - 0.5f, rightBGCurPoint.y - 0.5f + lastPlacedRightBGblock.sizeXZ.y);
                        temp = Random.Range(0, backgroundBlocks.Length);
                        Instantiate(backgroundBlocks[temp].block, new Vector3(leftBGCurPoint.x, 0, leftBGCurPoint.y + backgroundBlocks[temp].sizeXZ.x/2), Quaternion.Euler(0, -90, 0), curTrackCluster.transform);
                        leftBGCurPoint.y += backgroundBlocks[temp].sizeXZ.x;
                        rightOrLeft = leftBGCurPoint.y < tmpBGCurPoint.y ? tmpBGCurPoint.y : leftBGCurPoint.y; 
                        for(int i = (int)roadCurPoint.y + 1; i < rightOrLeft; i++)
                        {
                            temp = Random.Range(0, roadBlocks.Length);
                            Instantiate(roadBlocks[temp].block, new Vector3(roadCurPoint.x, 0, i), Quaternion.Euler(0, 90, 0), curTrackCluster.transform);
                            if(Random.value < (2f/5))
                            {
                                Instantiate(coins[Random.Range(0, coins.Length)], new Vector3(roadCurPoint.x, 0, i), Quaternion.Euler(0, 90, 0), curTrackCluster.transform);
                            }
                        }
                        roadCurPoint.x--;
                        CreateRoad();
                    break;
                }
                IsWaiting = true;
                NotRotChance = 4.8f;
            }
            else
            {
                temp = Random.Range(0, singleRotateBlocks.Length);
                curTrackCluster = Instantiate(singleRotateBlocks[temp].block, new Vector3(roadCurPoint.x, 0, roadCurPoint.y), Quaternion.identity, transform);
                switch(_curRotMode)
                {
                    //case left
                    case 0:
                        curTrackCluster.tag = "rotLeft";
                        CreateSingleRot(curTrackCluster,curRotMode, false);  
                        curRotMode++;
                    break;
                    //case right
                    case 1:
                    if((int)Random.Range(0,2) == 1)
                    {
                        if(roadCurPoint.y != rightBGCurPoint.y + 0.5f)
                        {
                            Destroy(curTrackCluster);
                            return;
                        }
                        curTrackCluster.tag = "rotRight";
                        curTrackCluster.transform.Rotate(0,180,0);

                        CreateSingleRot(curTrackCluster, curRotMode, true);  
                        curRotMode--;
                    }
                    else
                    {
                        if(roadCurPoint.y != leftBGCurPoint.y + 0.5f)
                        {
                            Destroy(curTrackCluster);
                            return;
                        }
                        curTrackCluster.tag = "rotLeft";
                        curTrackCluster.transform.Rotate(0,-90,0);
                        CreateSingleRot(curTrackCluster, curRotMode, false);  
                        curRotMode++;
                    }
                    break;
                    case 2:
                        curTrackCluster.tag = "rotRight";
                        curTrackCluster.transform.Rotate(0,90, 0);
                        CreateSingleRot(curTrackCluster, curRotMode, true);
                        curRotMode--;
                    break;
                    case 3:
                    break;
                }
            }
            roadCurPoint += deltaRoadPoint[curRotMode];
        }
        track.Enqueue(curTrackCluster);
    }

    private void CreateBackground()
    {
        int temp = Random.Range(0, backgroundBlocks.Length);
        switch(curRotMode)
        {
            case 0:
                Instantiate(backgroundBlocks[temp].block, new Vector3 (leftBGCurPoint.x + backgroundBlocks[temp].sizeXZ.x/2, 0, leftBGCurPoint.y), Quaternion.Euler(0, backgroundAngle, 0), curTrackCluster.transform);
                leftBGCurPoint.x += backgroundBlocks[temp].sizeXZ.x;
                lastPlacedLeftBGBlock = backgroundBlocks[temp];
                break;
            case 1:
                while(leftBGCurPoint.y < roadCurPoint.y + 0.5f)
                {   
                    Instantiate(backgroundBlocks[temp].block, new Vector3 (leftBGCurPoint.x, 0, leftBGCurPoint.y + backgroundBlocks[temp].sizeXZ.x/2), Quaternion.Euler(0, backgroundAngle, 0), curTrackCluster.transform);
                    leftBGCurPoint.y += backgroundBlocks[temp].sizeXZ.x;
                    lastPlacedLeftBGBlock = backgroundBlocks[temp];
                    temp = Random.Range(0, backgroundBlocks.Length);

                }
                while(rightBGCurPoint.y < roadCurPoint.y+0.5f)
                {
                    Instantiate(backgroundBlocks[temp].block, new Vector3 (rightBGCurPoint.x, 0, rightBGCurPoint.y + backgroundBlocks[temp].sizeXZ.x/2), Quaternion.Euler(0, backgroundAngle + 180, 0), curTrackCluster.transform);
                    rightBGCurPoint.y += backgroundBlocks[temp].sizeXZ.x;
                    lastPlacedRightBGblock = backgroundBlocks[temp];
                    temp = Random.Range(0, backgroundBlocks.Length);
                }
                break;
            case 2:
                    // Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                        Instantiate(backgroundBlocks[temp].block, new Vector3(rightBGCurPoint.x - backgroundBlocks[temp].sizeXZ.x/2, 0, rightBGCurPoint.y), Quaternion.Euler(0, backgroundAngle + 180, 0), curTrackCluster.transform);
                        rightBGCurPoint.x -= backgroundBlocks[temp].sizeXZ.x;
                        lastPlacedRightBGblock = backgroundBlocks[temp];
                        temp = Random.Range(0, backgroundBlocks.Length);
                break;
            case 3:
                while(leftBGCurPoint.y > roadCurPoint.y || rightBGCurPoint.y > roadCurPoint.y)
                {
                    if(leftBGCurPoint.y > roadCurPoint.y)
                    {
                        Instantiate(backgroundBlocks[temp].block, new Vector3(leftBGCurPoint.x, 0, leftBGCurPoint.y - backgroundBlocks[temp].sizeXZ.x/2), Quaternion.Euler(0, backgroundAngle, 0), curTrackCluster.transform);
                        leftBGCurPoint.y-=backgroundBlocks[temp].sizeXZ.x;
                        lastPlacedLeftBGBlock = backgroundBlocks[temp];
                        temp = Random.Range(0, backgroundBlocks.Length);
                    }
                
                    if(rightBGCurPoint.y > roadCurPoint.y)
                    {
                        Instantiate(backgroundBlocks[temp].block, new Vector3(rightBGCurPoint.x, 0, rightBGCurPoint.y - backgroundBlocks[temp].sizeXZ.x/2), Quaternion.Euler(0, backgroundAngle + 180, 0), curTrackCluster.transform);
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
                for(int i = (int)roadCurPoint.y; i < leftOrRight; i++ )
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
        Instantiate(roadBlocks[temp].block, new Vector3(roadCurPoint.x, 0, roadCurPoint.y), Quaternion.Euler(0, roadAngle /*- 180 * Random.Range(0, 2)*/, 0), curTrackCluster.transform);
        if(Random.value < (2f/5))
        {
            Instantiate(coins[Random.Range(0, coins.Length)], new Vector3(roadCurPoint.x, 0, roadCurPoint.y), Quaternion.Euler(0,roadAngle + 90, 0), curTrackCluster.transform);
        }
        roadCurPoint += deltaRoadPoint[curRotMode];
    }

    private IEnumerator ControlDelete()
    {
        yield return null;
    }
    private IEnumerator ControlGenerate()
    {
        while(true)
        {
            Create();
            yield return new WaitWhile(() => _isWaiting);
        }
        StopCoroutine("Control");
    }
}
