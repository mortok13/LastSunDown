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


    //main track GO array
    private GameObject[] track = new GameObject[10];
    // track[1] is always road and crossroad, 0 and 2 - background


    private int rSeed;

                        // x, z
    private static Vector2 leftBGCurPoint;
                        // x, z
    private static Vector2 roadCurPoint;

    private static int deleteIndex;
    private static short curIndex;


    private static float roadAngle;

    private static float backgroundAngle;


    // 0 - right, 1 - up, 2 - left, 3 - down;
    private static int _curRotMode;
    private int curRotMode {
        get
        {
            return _curRotMode;
        }
        
        set
        {
            if(value > (int)RotMode.down)
            {
                _curRotMode = (int)RotMode.right;
            }
            else if(value < (int)RotMode.right)
            {
                _curRotMode = (int)RotMode.down;
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
        curRotMode = (int)RotMode.right;
        leftBGCurPoint.x = 0;
        leftBGCurPoint.y = 1;
        roadCurPoint.x = 1;
        roadCurPoint.y = 0;
        deleteIndex = 0;
        curIndex = 1;
        rSeed = Mathf.FloorToInt(System.DateTime.Now.Millisecond * System.DateTime.Now.Second * System.DateTime.Now.Minute * System.DateTime.Now.Hour);
        // ~86 313 600 variations
        
        Random.InitState(rSeed);
        rSeed = Random.Range(int.MinValue, int.MaxValue);
        Random.InitState(rSeed);
        Debug.Log($"seed is {rSeed.ToString("X")}");
    }

    void Start()
    {
        track[0] = Instantiate(roadBlocks[Random.Range(0, roadBlocks.Length)].block ,new Vector3(0,0,0), Quaternion.identity, transform);
        Instantiate(backgroundBlocks[5].block, new Vector3 (leftBGCurPoint.x + backgroundBlocks[5].sizeXZ.x/2 - 0.5f, 0, leftBGCurPoint.y), Quaternion.identity, track[0].transform);
        leftBGCurPoint.x += backgroundBlocks[5].sizeXZ.x/2 + 1;
        Debug.Log (leftBGCurPoint);
        Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();Create();
       // Destroy(track[1]);
    }

    private void Create()
    {
        Debug.Log($"curIndex is {curIndex}");

        //Create road + background
        if(Random.value < 0.8f)     
        {
            int temp = Random.Range(0, roadBlocks.Length);
            track[curIndex] = Instantiate(roadBlocks[temp].block, new Vector3(roadCurPoint.x, 0, roadCurPoint.y), Quaternion.Euler(0, roadAngle - 180 * Random.Range(0, 2), 0), transform);
            roadCurPoint.x++;


            temp = Random.Range(0, backgroundBlocks.Length);
            Instantiate(backgroundBlocks[temp].block, new Vector3 (leftBGCurPoint.x + backgroundBlocks[temp].sizeXZ.x/2, 0, leftBGCurPoint.y), Quaternion.Euler(0, backgroundAngle, 0), track[curIndex].transform);
            leftBGCurPoint.x += backgroundBlocks[temp].sizeXZ.x;
            Debug.Log("leftBGCurPoint: " + leftBGCurPoint);


            for(int i = (int)roadCurPoint.x; i < leftBGCurPoint.x; i++)
            {
                temp = Random.Range(0, roadBlocks.Length);
                Instantiate(roadBlocks[temp].block, new Vector3(roadCurPoint.x, 0, roadCurPoint.y), Quaternion.Euler(0, roadAngle - 180 * Random.Range(0, 2), 0), track[curIndex].transform);
                roadCurPoint.x++;
            }
            Debug.Log("roadCurPoint: " + roadCurPoint);
        }
        //Create rotation
        else
        {
            int temp = Random.Range(0, singleRotateBlocks.Length);
            track[curIndex] = Instantiate(singleRotateBlocks[temp].block, new Vector3(roadCurPoint.x, 0, roadCurPoint.y), Quaternion.identity, transform);
            switch(Random.Range(0, 2))
            {
                //case left
                case 0:
                track[curIndex].tag = "rotLeft";
                curRotMode++;           
                break;
                //case right
                case 1:
                track[curIndex].tag = "rotRight";
                curRotMode--;
                break;
            }
          //  roadCurPoint.y++;
        }
        curIndex++;
        if(curIndex > 9)
        {
            curIndex = 0;
        }
    }
}
