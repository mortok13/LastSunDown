using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotation : MonoBehaviour
{
    //private float rotationTime = 1f;
    public bool movingMode;        /////// 0 - horizontal, 1 - vertical ///////
    private Quaternion qAngle;
    private Vector3 playerCurRot;
    private Vector3 playerRotPos;
    private bool Stabilized;
    public bool inRotation;

    private float lerpTimer;

    void Start()
    {
        Stabilized = true;
        playerCurRot = transform.rotation.eulerAngles;
        playerCurRot.z = 0;

        playerRotPos = transform.position;
        inRotation = false;
        rotStabilize(0f);
        movingMode = false;
        lerpTimer = 0f;
        //qAngle = Quaternion.Euler(transform.rotation.eulerAngles);
        playerRotPos.x = Mathf.Round(playerRotPos.x);
        playerRotPos.z = Mathf.Round(playerRotPos.z);
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
        //playerCurRot.x = transform.rotation.x;
        Debug.Log(playerCurRot);
        Debug.Log(transform.rotation);
    }
    void FixedUpdate()
    {
        //Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0), 0.1f);
        if(!inRotation)
        {
            if(!(controls.rotLeft || controls.rotRight))
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(playerCurRot),  Time.fixedDeltaTime * 5);
            }
            //transform.rotation =  Quaternion.Lerp(transform.rotation, playerCurRot, 5 * Time.fixedDeltaTime);
            
            transform.position = Vector3.Lerp(transform.position, playerRotPos, 5 * Time.deltaTime);
        }
    }
    public void rotStabilize(float angle)
    {
        playerCurRot += new Vector3(0,angle,0);
       // qAngle = Quaternion.Euler(0,playerCurRot.y, 0);
        //lerpTimer = 0f;
       // Stabilized = false;
    }

    public void setRotJoint(byte rotMode)
    {
       // movingMode = !movingMode;
        gameObject.AddComponent(typeof(ConfigurableJoint));
        ConfigurableJoint CJ = GetComponent<ConfigurableJoint>();
        SoftJointLimit CJlimit = new SoftJointLimit();
        CJlimit.limit = 0.1f;
        CJlimit.bounciness = 0;
        CJlimit.contactDistance = 0;

        CJ.xMotion = ConfigurableJointMotion.Limited;
        CJ.zMotion = ConfigurableJointMotion.Limited;
        CJ.anchor = new Vector3(-0.1f * Mathf.Pow(-1, rotMode), 0, 0);
        CJ.linearLimit = CJlimit;
    }
    public void resetRotJoint()
    {
        Destroy(GetComponent<ConfigurableJoint>());
    }
    
    public Vector3 getCurRot()
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
}
