using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Rotation : MonoBehaviour
{
    public static bool movingMode; // 0 - horizontal, 1 - vertical //
    private Quaternion qAngle;
    private Vector3 playerCurRot;
    private Vector3 playerRotPos;
    private bool stabilized;
    public bool inRotation;
    private Vector3 velocity;

    private  float rotStabilizeTime;

    void Start()
    {
        qAngle  = new Quaternion();
        playerCurRot = new Vector3(0,90,0);
        playerCurRot.z = 0;
        qAngle.z = 0;
        rotStabilizeTime = 0;
        playerRotPos = transform.position;
        inRotation = false;
        rotStabilize(0f);
        movingMode = false;
        stabilized = true;
        playerRotPos.x = Mathf.Round(playerRotPos.x);
        playerRotPos.z = Mathf.Round(playerRotPos.z);
        StopAllCoroutines();
        Moving.PlayerRB.constraints = RigidbodyConstraints.FreezeRotationZ |
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
        Debug.Log(velocity);
        playerCurRot.x = transform.rotation.eulerAngles.x;
    }
    void FixedUpdate()
    {
        if(!inRotation)
        {
            if(!stabilized)
            {
                Moving.PlayerRB.MoveRotation(Quaternion.Normalize(qAngle));
            }
            Moving.PlayerRB.MovePosition(Vector3.Lerp(transform.position, playerRotPos, rotStabilizeTime+0.01f));
        }
    }
    public void rotStabilize(float angle) 
    {
        playerCurRot.y += angle;
        Moving.PlayerRB.constraints = RigidbodyConstraints.None;
        StartCoroutine("rotTimer");
    }
    public void setRotJoint(byte rotMode)
    {
        Moving.PlayerRB.constraints = RigidbodyConstraints.FreezeRotationZ
                                    | RigidbodyConstraints.FreezeRotationX;
        gameObject.AddComponent(typeof(ConfigurableJoint));
        ConfigurableJoint CJ = GetComponent<ConfigurableJoint>();
        SoftJointLimit CJlimit = new SoftJointLimit();
        CJlimit.limit = 0.32f;
        CJlimit.bounciness = 0;
        CJlimit.contactDistance = 0;

        CJ.xMotion = ConfigurableJointMotion.Limited;
        CJ.zMotion = ConfigurableJointMotion.Limited;
        CJ.anchor = new Vector3(-0.32f * Mathf.Pow(-1, rotMode), 0, 0);
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
        playerCurRot.x = delta;
        if(playerCurRot.x >= 360 || playerCurRot.x <= -360)
        {
            playerCurRot.x = 0;
        }
    }

    IEnumerator rotTimer()
    {
        StartCoroutine("getX");
        velocity = Moving.PlayerRB.velocity;
        stabilized = false;
        Moving.PlayerRB.Sleep();
        yield return null;
        Moving.PlayerRB.WakeUp();
        Moving.PlayerRB.velocity = velocity;
        Moving.PlayerRB.constraints = RigidbodyConstraints.FreezeRotationX;
        yield return new WaitForSeconds(2f);
        Moving.PlayerRB.constraints = RigidbodyConstraints.FreezeRotationY;
        stabilized = true;
        StopCoroutine("getX");
        rotStabilizeTime = 0;
        yield return new WaitUntil(() => transform.rotation.eulerAngles.z <= 0.0001f && transform.rotation.eulerAngles.z >= -0.0001f);
        Moving.PlayerRB.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        StopCoroutine("rotTimer");
    }
    IEnumerator getX()
    {
        while(true)
        {
            rotStabilizeTime += Time.deltaTime/10;
            playerCurRot.x = transform.rotation.eulerAngles.x;
            qAngle = Quaternion.Euler(playerCurRot);
            yield return new WaitForEndOfFrame();
        }
    }
}
