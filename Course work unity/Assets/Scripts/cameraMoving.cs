using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMoving : MonoBehaviour
{
    [SerializeField,Range(0, 1)]
    private float startDelay;

    [SerializeField, Range(0.001f,1)]
    private float lerpIntensity;
    public GameObject player;
    private Vector3 cameraOffset;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cameraOffset = transform.position - player.transform.position; 
    }
    void Start()
    {
       transform.position = new Vector3(player.transform.position.x + 1.5f, 0.75f, -4f);
       transform.Rotate(new Vector3(-15.5f, 0, 0)); 
       StartCoroutine("CameraMainControl");
    }
    void Update()
    {
       //Debug.Log("camera player: " + player.transform.position);
    }

    private IEnumerator CameraMainControl()
    {
        yield return new WaitUntil(() => player.transform.position.x >= transform.position.x);
        Debug.Log("Camera Control has started");
        yield return new WaitForSeconds(startDelay);
        Debug.Log("Camera Delay ended, Position Lerp to player body started");
        while(true)
        {
            SetTransformPosX(Mathf.Lerp(transform.position.x, player.transform.position.x, lerpIntensity));
            SetTransformPosZ(Mathf.Lerp(transform.position.z, player.transform.position.z - 4, lerpIntensity));
            yield return null;
        }
    }

    private void SetTransformPosX(float newPos)
    {
        transform.position = new Vector3(newPos, transform.position.y, transform.position.z);
    }

    private void SetTransformPosZ(float newPos)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, newPos);
    }
    

}
