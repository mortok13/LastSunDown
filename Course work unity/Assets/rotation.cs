using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class rotation : MonoBehaviour
{
    //private float rotationTime = 1f;
    public bool movingMode;        /////// 0 - horizontal, 1 - vertical ///////
    private Quaternion qAngle = new Quaternion();
    private Quaternion playerCurRot;
    private Vector3 playerRotPos;
    private bool stabilized;
    public bool inRotation;

    private Rigidbody playerRB;

    //private float lerpTimer;

    private  float rotStabilizeTime;

    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
       // Stabilized = true;
        playerCurRot = Quaternion.Euler(0,90,0);
        playerCurRot.z = 0;

        rotStabilizeTime = 5*Time.deltaTime;
        playerRotPos = transform.position;
        inRotation = false;
        rotStabilize(0f);
        movingMode = false;
        stabilized = true;
        //lerpTimer = 0f;
        //qAngle = Quaternion.Euler(transform.rotation.eulerAngles);
        playerRotPos.x = Mathf.Round(playerRotPos.x);
        playerRotPos.z = Mathf.Round(playerRotPos.z);
        //StartCoroutine("rotTimer");
        StopAllCoroutines();
        playerRB.constraints = RigidbodyConstraints.FreezeRotationZ |
                               RigidbodyConstraints.FreezePositionZ |
                               RigidbodyConstraints.FreezeRotationY;
    }

    void Update()
    {       
        playerRotPos = transform.position;

        if(movingMode)
        {
            playerRotPos.x = Mathf.Round(playerRotPos.x);
        }
        else
        {
            playerRotPos.z = Mathf.Round(playerRotPos.z);
        }
        Debug.Log(rotStabilizeTime);
        Debug.Log(playerCurRot);
        Debug.Log(transform.rotation);
        playerCurRot.z = 0;
        playerCurRot.x = transform.rotation.eulerAngles.x;


        qAngle.x = transform.rotation.x;
        qAngle.y = Mathf.LerpUnclamped(transform.rotation.y, playerCurRot.y, rotStabilizeTime);
        qAngle.z = Mathf.LerpUnclamped(transform.rotation.z, 0f, rotStabilizeTime);
        qAngle.w = transform.rotation.w;
    }
    void FixedUpdate()
    {
        if(!inRotation)
        {
           // playerRB.MoveRotation(Quaternion.Lerp(transform.rotation, Quaternion.Euler(playerCurRot) ,  rotStabilizeTime));
            if(!stabilized)
            {
                playerRB.MoveRotation(Quaternion.Normalize(qAngle));
            }
            else
            {

            }
           // playerRB.MoveRotation(new Quaternion(transform.rotation.x, ));
            playerRB.MovePosition(Vector3.Lerp(transform.position, playerRotPos, 5*Time.deltaTime));
        }
    }
    public void rotStabilize(float angle) 
    {
        playerCurRot *= Quaternion.Euler(0,angle,0);
        playerRB.constraints = RigidbodyConstraints.None;
        StartCoroutine("rotTimer");
    }
    public void setRotJoint(byte rotMode)
    {
        playerRB.constraints = RigidbodyConstraints.FreezeRotationZ;
        gameObject.AddComponent(typeof(ConfigurableJoint));
        ConfigurableJoint CJ = GetComponent<ConfigurableJoint>();
        SoftJointLimit CJlimit = new SoftJointLimit();
        CJlimit.limit = 0.33f;
        CJlimit.bounciness = 0;
        CJlimit.contactDistance = 0;

        CJ.xMotion = ConfigurableJointMotion.Limited;
        CJ.zMotion = ConfigurableJointMotion.Limited;
        CJ.anchor = new Vector3(-0.33f * Mathf.Pow(-1, rotMode), 0, 0);
        CJ.linearLimit = CJlimit;
    }
    public void resetRotJoint()
    {
        //playerRB.constraints = RigidbodyConstraints.FreezeRotationZ;
        Destroy(GetComponent<ConfigurableJoint>());
    }
    
    public Quaternion getCurRot()
    {
        return playerCurRot;
    }
    public void deltaCurRotX(float delta)
    {
        if(delta == 0)
        {
            playerCurRot.x = 0;
            return;
        }
        //playerCurRot.x = transform.rotation.;
        playerCurRot.x = delta;
        if(playerCurRot.x >= 360 || playerCurRot.x <= -360)
        {
            playerCurRot.x = 0;
        }
    }

    IEnumerator rotTimer()
    {
        stabilized = false;
        yield return new WaitForSeconds(1.5f);
        if(!movingMode)
        {
        playerRB.constraints = RigidbodyConstraints.FreezeRotationZ |
                               RigidbodyConstraints.FreezeRotationY |
                               RigidbodyConstraints.FreezePositionX;
        }
        else
        {
        playerRB.constraints = RigidbodyConstraints.FreezeRotationZ |
                               RigidbodyConstraints.FreezeRotationY |
                               RigidbodyConstraints.FreezePositionX;
        }
        stabilized = true;
        StopCoroutine("rotTimer");
    }
}
