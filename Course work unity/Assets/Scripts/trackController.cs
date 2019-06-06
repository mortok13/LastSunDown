using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class trackController : MonoBehaviour
{
    private GameObject player;
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
    private static Vector2 TRotRoadLeftCurPoint;
    private static Vector2 TRotRoadRightCurPoint;
    private static Vector2 TRotLeftBGCurPoint;
                        // x, z
    private static Vector2 TRotRightBGCurPoint;
    private static Vector2 leftBGCurPoint;
    private static Vector2 rightBGCurPoint;
                            // x, z
    private static Vector2 roadCurPoint;
    private static float roadAngle;
    //TODO: Сократить deltaRoadPoint до двух элементов(оставить [0] и [2])
    private Vector2[] deltaRoadPoint = {new Vector2(1,0), new Vector2(0,1), new Vector2(-1,0), new Vector2(0, -1)};
    private static float backgroundAngle;
    //////////////////////////////////////////////////////////////////////////////
    
    //Creating a single rotation block cluster
                                                //false - left, right - true
    void CreateSingleRot(GameObject resultContainer,int indexX, bool side)
    {
        Vector3 resContTrnsfrm = resultContainer.transform.position;
        switch(indexX)
        {
            case 0:
            //CreateRotCluster(indexX, 1 - indexY % 3)
                if(!side)       // up
                {
                    leftBGCurPoint.x = resContTrnsfrm.x - 1;
                                                        //
                    leftBGCurPoint.y = resContTrnsfrm.z + 0.5f + lastPlacedLeftBGBlock.sizeXZ.y;
                    rightBGCurPoint.x = resContTrnsfrm.x + 1;
                    rightBGCurPoint.y = resContTrnsfrm.z - 0.5f;
                }
                break;
            case 1:
                if(!side)       // left
                {
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
                    leftBGCurPoint.x = resContTrnsfrm.x - 1;
                    leftBGCurPoint.y = resContTrnsfrm.z - 0.5f;
                    rightBGCurPoint.x = resContTrnsfrm.x + 1;
                    rightBGCurPoint.y = resContTrnsfrm.z + 0.5f + (lastPlacedRightBGblock == null ? 0 : lastPlacedRightBGblock.sizeXZ.y);
                }
                break;
        }
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
            if(value > (int)RotModes.left)
            {
                _curRotMode = (int)RotModes.right;
            }
            else if(value < (int)RotModes.right)
            {
                _curRotMode = (int)RotModes.left;
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
    public int getSeed()
    {
        return rSeed;
    }

    private bool isLastTDeadEnd;
    void Awake()
    {
        isLastTDeadEnd = false;
        player = GameObject.FindWithTag("Player");
        track = new Queue<GameObject>();
        _isWaiting = false;
        NotRotChance = 2.8f;
        curRotMode = (int)RotModes.right;
        leftBGCurPoint.x = 0;
        leftBGCurPoint.y = 1;
        roadCurPoint.x = 1;
        roadCurPoint.y = 0;
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
        StartCoroutine("ControlDelete");
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
                    //> _|_
                    case 0:
                        curTrackCluster = Instantiate(TRotBlocks[temp].block, new Vector3(roadCurPoint.x, 0, roadCurPoint.y), Quaternion.Euler(0, 180, 0), transform);
                        tmpBGCurPoint = new Vector2(leftBGCurPoint.x, leftBGCurPoint.y + lastPlacedLeftBGBlock.sizeXZ.y);
                        tmpBGCurPoint.x-=0.5f;
                        tmpBGCurPoint.y-=0.5f;
                        temp = Random.Range(0, backgroundBlocks.Length);
                        Instantiate(backgroundBlocks[temp].block, new Vector3(tmpBGCurPoint.x, 0, tmpBGCurPoint.y + backgroundBlocks[temp].sizeXZ.x/2), Quaternion.Euler(0,-90,0), curTrackCluster.transform);
                        tmpBGCurPoint.y += backgroundBlocks[temp].sizeXZ.x;
                        TRotLeftBGCurPoint = new Vector2(tmpBGCurPoint.x, tmpBGCurPoint.y);


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
                        TRotRoadLeftCurPoint = new Vector2(roadCurPoint.x, Mathf.Ceil(rightOrLeft));
                        
                        roadCurPoint.x++;
                        CreateRoad();
                        roadCurPoint.x--;
                    break;
                    case 1:
                        temp = Random.Range(0, 2);
                        //temp = 0;
                        switch(temp)
                        {
                            //|-
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
                            TRotLeftBGCurPoint = new Vector2(tmpBGCurPoint.x, tmpBGCurPoint.y);
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
                            TRotRoadRightCurPoint = new Vector2(Mathf.Ceil(tmpBGCurPoint.x), roadCurPoint.y);
                            roadCurPoint.y++;
                            for(int i = (int)(roadCurPoint.y); i < tmpBGCurPoint.y - 0.5f + backgroundBlocks[temp].sizeXZ.y; i++)
                            {
                                temp = Random.Range(0, roadBlocks.Length);
                                Instantiate(roadBlocks[temp].block, new Vector3(roadCurPoint.x, 0, i), Quaternion.Euler(0, roadAngle, 0), curTrackCluster.transform);
                                if(Random.value < (2f/5))
                                {
                                    Instantiate(coins[Random.Range(0, coins.Length)], new Vector3(roadCurPoint.x, 0, i), Quaternion.Euler(0, roadAngle, 0), curTrackCluster.transform);
                                }
                                roadCurPoint.y++;
                            }
                            roadCurPoint.y--;
                          //  rightBGCurPoint.y = tmpBGCurPoint.y + 0.5f;

                            break;
                            // '|'
                            case 1:
                                // while(roadCurPoint.y < leftBGCurPoint.y || roadCurPoint.y < rightBGCurPoint.y)
                                // {
                                //     CreateRoadBlock();
                                // }
                                isLastTDeadEnd = true;
                                temp = Random.Range(0, TRotBlocks.Length);
                                curTrackCluster = Instantiate(TRotBlocks[temp].block, new Vector3(roadCurPoint.x, 0, roadCurPoint.y), Quaternion.identity, transform);
                                tmpBGCurPoint = new Vector2(roadCurPoint.x - 0.5f, roadCurPoint.y + 1);
                                temp = Random.Range(0, backgroundBlocks.Length);
                                Instantiate(backgroundBlocks[temp].block, new Vector3(tmpBGCurPoint.x += backgroundBlocks[temp].sizeXZ.x/2, 0, tmpBGCurPoint.y), Quaternion.identity, curTrackCluster.transform);
                                Vector2 scndTmpBGCurPoint = new Vector2(tmpBGCurPoint.x, tmpBGCurPoint.y);
                                tmpBGCurPoint.x += backgroundBlocks[temp].sizeXZ.x/2;
                                scndTmpBGCurPoint.x -= backgroundBlocks[temp].sizeXZ.x/2;
                                temp = Random.Range(0, backgroundBlocks.Length);
                                Instantiate(backgroundBlocks[temp].block, new Vector3(tmpBGCurPoint.x + backgroundBlocks[temp].sizeXZ.x/2, 0, tmpBGCurPoint.y), Quaternion.identity, curTrackCluster.transform);
                                tmpBGCurPoint.x += backgroundBlocks[temp].sizeXZ.x;
                                temp = Random.Range(0, backgroundBlocks.Length);
                                Instantiate(backgroundBlocks[temp].block, new Vector3(scndTmpBGCurPoint.x - backgroundBlocks[temp].sizeXZ.x/2, 0, tmpBGCurPoint.y), Quaternion.identity, curTrackCluster.transform);
                                scndTmpBGCurPoint.x -= backgroundBlocks[temp].sizeXZ.x;
                                TRotRightBGCurPoint = new Vector2(scndTmpBGCurPoint.x, scndTmpBGCurPoint.y);
                                TRotLeftBGCurPoint = new Vector2(tmpBGCurPoint.x, tmpBGCurPoint.y);

                                for(int i = (int)roadCurPoint.x + 1; i < tmpBGCurPoint.x; i++)
                                {
                                    temp = Random.Range(0, roadBlocks.Length);
                                    Instantiate(roadBlocks[temp].block, new Vector3(i, 0, roadCurPoint.y), Quaternion.identity, curTrackCluster.transform);
                                    if(Random.value < (2f/5))
                                    {
                                        Instantiate(coins[Random.Range(0, coins.Length)], new Vector3(i, 0, roadCurPoint.y), Quaternion.Euler(0, roadAngle, 0), curTrackCluster.transform);
                                    }
                                }
                                TRotRoadRightCurPoint = new Vector2(tmpBGCurPoint.x + 0.5f, roadCurPoint.y);
                                TRotRoadLeftCurPoint = new Vector2(scndTmpBGCurPoint.x - 0.5f, roadCurPoint.y);
                                for(int i = (int)roadCurPoint.x - 1; i > scndTmpBGCurPoint.x; i--)
                                {
                                    temp = Random.Range(0, roadBlocks.Length);
                                    Instantiate(roadBlocks[temp].block, new Vector3(i, 0, roadCurPoint.y), Quaternion.identity, curTrackCluster.transform);
                                    if(Random.value < (2f/5))
                                    {
                                        Instantiate(coins[Random.Range(0, coins.Length)], new Vector3(i, 0, roadCurPoint.y), Quaternion.Euler(0, roadAngle, 0), curTrackCluster.transform);
                                    }
                                }
                            break;
                            //-|
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
                                TRotRightBGCurPoint = new Vector2(tmpBGCurPoint.x, tmpBGCurPoint.y);
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
                                TRotRoadLeftCurPoint = new Vector2(Mathf.Ceil(tmpBGCurPoint.x), roadCurPoint.y);
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
                    //_|_<
                    case 2:
                        curTrackCluster = Instantiate(TRotBlocks[temp].block, new Vector3(roadCurPoint.x, 0, roadCurPoint.y), Quaternion.Euler(0, 180, 0), transform);
                        tmpBGCurPoint = new Vector2(rightBGCurPoint.x, rightBGCurPoint.y + lastPlacedRightBGblock.sizeXZ.y);
                        tmpBGCurPoint.x+=0.5f;
                        tmpBGCurPoint.y-=0.5f;
                        temp = Random.Range(0, backgroundBlocks.Length);
                        Instantiate(backgroundBlocks[temp].block, new Vector3(tmpBGCurPoint.x, 0, tmpBGCurPoint.y + backgroundBlocks[temp].sizeXZ.x/2), Quaternion.Euler(0, 90, 0), curTrackCluster.transform);
                        tmpBGCurPoint.y += backgroundBlocks[temp].sizeXZ.x;
                        TRotRightBGCurPoint = new Vector2(tmpBGCurPoint.x, tmpBGCurPoint.y);
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
                        TRotRoadRightCurPoint = new Vector2(roadCurPoint.x, Mathf.Ceil(rightOrLeft));
                        roadCurPoint.x--;
                        CreateRoad();
                        roadCurPoint.x++;
                    break;
                }
                IsWaiting = true;
                NotRotChance = 5.8f;
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
                        CreateSingleRot(curTrackCluster, _curRotMode, false);  
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

                        CreateSingleRot(curTrackCluster, _curRotMode, true);  
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
                        CreateSingleRot(curTrackCluster, _curRotMode, false);  
                        curRotMode++;
                    }
                    break;
                    case 2:
                        curTrackCluster.tag = "rotRight";
                        curTrackCluster.transform.Rotate(0,90, 0);
                        CreateSingleRot(curTrackCluster, _curRotMode, true);
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
    
    public void SetRotationMode(int side, GameObject rotTrigger)
    {
        if(side == -2)
        {
            rotTrigger.GetComponent<PlayerTrackRotation>().trigger.enabled = false;
            if(isLastTDeadEnd)
            {
                return;
            }
            side = 0;
        }
        int tmp = _curRotMode;
        curRotMode += side;
        if(tmp != _curRotMode)
        {
            switch(tmp)
            {
                case 0:
                    leftBGCurPoint = new Vector2(TRotLeftBGCurPoint.x, TRotLeftBGCurPoint.y);
                    roadCurPoint = new Vector2(TRotRoadLeftCurPoint.x, TRotRoadLeftCurPoint.y);
                    rotTrigger.tag = "rotLeft";
                break;
                case 1:
                    if(_curRotMode == 2)
                    {
                        rightBGCurPoint = new Vector2(TRotRightBGCurPoint.x, TRotRightBGCurPoint.y);
                        roadCurPoint = new Vector2(TRotRoadLeftCurPoint.x, TRotRoadLeftCurPoint.y);
                        rotTrigger.tag = "rotLeft";
                    }
                    else
                    {
                        leftBGCurPoint = new Vector2(TRotLeftBGCurPoint.x, TRotLeftBGCurPoint.y);
                        roadCurPoint = new Vector2(TRotRoadRightCurPoint.x, TRotRoadRightCurPoint.y);
                        rotTrigger.tag = "rotRight";
                    }
                break;
                case 2:
                    rightBGCurPoint = new Vector2(TRotRightBGCurPoint.x, TRotRightBGCurPoint.y);
                    roadCurPoint = new Vector2(TRotRoadRightCurPoint.x, TRotRoadRightCurPoint.y);
                    rotTrigger.tag = "rotRight";
                break;
            }
        }
        _isWaiting = false;
        isLastTDeadEnd = false;
    }
    private void CreateBackground()
    {
        int temp = Random.Range(0, backgroundBlocks.Length);
        switch(_curRotMode)
        {
            case 0:
                Instantiate(backgroundBlocks[temp].block, new Vector3 (leftBGCurPoint.x + backgroundBlocks[temp].sizeXZ.x/2, 0, leftBGCurPoint.y), Quaternion.Euler(0, backgroundAngle, 0), curTrackCluster.transform);
                leftBGCurPoint.x += backgroundBlocks[temp].sizeXZ.x;
                lastPlacedLeftBGBlock = backgroundBlocks[temp];
                break;
            case 1: 
                    while(rightBGCurPoint.y < roadCurPoint.y - 3)
                    { 
                        Instantiate(backgroundBlocks[temp].block, new Vector3 (rightBGCurPoint.x, 0, rightBGCurPoint.y + backgroundBlocks[temp].sizeXZ.x/2), Quaternion.Euler(0, backgroundAngle + 180, 0), curTrackCluster.transform);
                        rightBGCurPoint.y += backgroundBlocks[temp].sizeXZ.x;
                        lastPlacedRightBGblock = backgroundBlocks[temp];
                        temp = Random.Range(0, backgroundBlocks.Length);
                    }
                    while(leftBGCurPoint.y < roadCurPoint.y - 3)
                    {
                        Instantiate(backgroundBlocks[temp].block, new Vector3 (leftBGCurPoint.x, 0, leftBGCurPoint.y + backgroundBlocks[temp].sizeXZ.x/2), Quaternion.Euler(0, backgroundAngle, 0), curTrackCluster.transform);
                        leftBGCurPoint.y += backgroundBlocks[temp].sizeXZ.x;
                        lastPlacedLeftBGBlock = backgroundBlocks[temp];
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
                leftOrRight = leftBGCurPoint.y < rightBGCurPoint.y ? leftBGCurPoint.y : rightBGCurPoint.y;
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
        while(true)
        {
            if( trackClusterForDelete != null && Vector3.Distance(player.transform.position, trackClusterForDelete.transform.position) > 10f)
            {
                Destroy(trackClusterForDelete.gameObject);
                if(track.Count != 0) 
                {
                    trackClusterForDelete = track.Dequeue();
                }
            }
            yield return null;
        }
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
