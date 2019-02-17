using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotation : MonoBehaviour
{
    //private float rotationTime = 1f;

    private Quaternion qAngle;
    private Vector3 playerCurRot;
    public bool inRotation;

    void Start()
    {
        playerCurRot = transform.rotation.eulerAngles;
        inRotation = false;
        rotStabilize(0f);
        //qAngle = Quaternion.Euler(transform.rotation.eulerAngles);
    }

    void Update()
    {
    }
    void FixedUpdate()
    {
        //Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0), 0.1f);
        if(!inRotation)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, qAngle,  Time.fixedDeltaTime * 5);
          //  transform.tran
        }
    }
    public void rotStabilize(float angle)
    {
        playerCurRot.y += angle;
        qAngle = Quaternion.Euler(0,playerCurRot.y, 0);

    }

    public void setRotJoint(byte rotMode)
    {
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
