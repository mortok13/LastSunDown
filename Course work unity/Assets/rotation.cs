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

    public bool inRotation;

    private float lerpTimer;

    void Start()
    {
        playerCurRot = transform.rotation.eulerAngles;
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
    }
    void FixedUpdate()
    {
        //Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0), 0.1f);
        if(!inRotation)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, qAngle,  Time.fixedDeltaTime * 5);
            transform.position = Vector3.Lerp(transform.position, playerRotPos, 2 * Time.deltaTime);
        }
        /*else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0f), Time.fixedDeltaTime * 5 );
        }*/
    }
    public void rotStabilize(float angle)
    {
        playerCurRot.y += angle;
        qAngle = Quaternion.Euler(0,playerCurRot.y, 0);
        lerpTimer = 0f;
    }

    public void setRotJoint(byte rotMode)
    {
       // movingMode = !movingMode;
        gameObject.AddComponent(typeof(ConfigurableJoint));
        ConfigurableJoint CJ = GetComponent<ConfigurableJoint>();
        SoftJointLimit CJlimit = new SoftJointLimit();
        CJlimit.limit = 0.1f;
        CJlimit.bounciness = 0f;
        CJlimit.contactDistance = 0f;

        CJ.xMotion = ConfigurableJointMotion.Limited;
        CJ.zMotion = ConfigurableJointMotion.Limited;
        CJ.anchor = new Vector3(-0.1f * Mathf.Pow(-1, rotMode), 0, 0);
        CJ.linearLimit = CJlimit;

    }

    public void resetRotJoint()
    {
        Destroy(GetComponent<ConfigurableJoint>());
    }
}
