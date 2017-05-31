using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMaskRadius : MonoBehaviour {

    public GameObject leftEyeMask;
    public GameObject rightEyeMask;
    public float speed = 1f;
    private float lastTickTime;
    public float tickDelay = 0.25f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time - lastTickTime > tickDelay)
        {

            if (Input.GetKey(KeyCode.P))
            {
                lastTickTime = Time.time;
                if (leftEyeMask)
                {
                    leftEyeMask.transform.localScale += new Vector3(speed * Time.deltaTime, speed * Time.deltaTime, speed * Time.deltaTime);
                }
                if (rightEyeMask)
                {
                    rightEyeMask.transform.localScale += new Vector3(speed * Time.deltaTime, speed * Time.deltaTime, speed * Time.deltaTime);
                }
            }
            if (Input.GetKey(KeyCode.O))
            {
                lastTickTime = Time.time;
                if (leftEyeMask)
                {
                    leftEyeMask.transform.localScale -= new Vector3(speed * Time.deltaTime, speed * Time.deltaTime, speed * Time.deltaTime);
                }
                if (rightEyeMask)
                {
                    rightEyeMask.transform.localScale -= new Vector3(speed * Time.deltaTime, speed * Time.deltaTime, speed * Time.deltaTime);
                }
            }
        }
    }
}
