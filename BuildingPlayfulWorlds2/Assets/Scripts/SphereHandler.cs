using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereHandler : MonoBehaviour {

    public GameObject pullBackObject;
    public float speed = 10.0f;

    private bool pull = false;

    public GameObject leftHand;
    public GameObject rightHand;

    public float timeBetweenPulls = 0.5f;

    private float currentTime = 0.0f;
    private float timeToPull = 0.0f;

	// Use this for initialization
	void Start () {
        pullBackObject.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
    }
	
	// Update is called once per frame
	void Update () {
        currentTime += Time.deltaTime;

		if(OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch) > 0 && currentTime >= timeToPull)
        {
            //pullBackObject.GetComponent<Rigidbody>().useGravity = false;
            pullBackObject.GetComponent<Rigidbody>().isKinematic = true;
            pullBackObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            pullBackObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            pull = true;

            timeToPull = currentTime + timeBetweenPulls;

            pullBackObject.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
        }

        if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) > 0)
        {

        }

        if(pull == true)
        {           
            if(Vector3.Distance(pullBackObject.transform.position, leftHand.transform.position) < 0.1f)
            {
                //pullBackObject.GetComponent<Rigidbody>().useGravity = true;
                pullBackObject.GetComponent<Rigidbody>().isKinematic = false;
                pull = false;
                pullBackObject.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
            }
            else
            {
                PullObject();
            }
        }
    }

    void PullObject()
    {
        /* Vector3 direction = leftHand.transform.position - pullBackObject.transform.position;
        pullBackObject.transform.Translate(direction * Time.deltaTime * speed); */

        pullBackObject.transform.position = Vector3.MoveTowards(pullBackObject.transform.position, leftHand.transform.position, Time.deltaTime * speed);
    }
}
