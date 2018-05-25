using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereScript : MonoBehaviour {

    public float force = 10f;

    public float forceMoveSphere = 1000.0f;

    public AudioClip[] audioClips;

    private bool btnPressedLast;

    private Transform leftCube;
    private Transform rightCube;
    private GameObject cam;

    private Vector3 localPosLeftCube;
    private Vector3 localPosRightCube;

    private int amtTestedRight = 0;
    private int amtTestedLeft = 0;

    private float[] currentPosRight = new float[3];
    private float[] currentPosLeft = new float[3];

    private float[] previousYPosRight = new float[3];
    private float[] previousYPosLeft = new float[3];
    private float testRange = 3.0f;
    private float[] totalTestRangeLeft = new float[3];
    private float[] totalTestRangeRight = new float[3];
    private float[] averageRight = new float[3];
    private float[] averageLeft = new float[3];

    private Rigidbody rigidBody;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();

        rightCube = GameObject.Find("RightHandAnchor").transform;
        leftCube = GameObject.Find("LeftHandAnchor").transform;
    }
	
	// Update is called once per frame
	void Update () {
        localPosLeftCube = leftCube.localPosition;
        localPosRightCube = rightCube.localPosition;

        if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) > 0.1)
        {
            FlyRight();
            if (!btnPressedLast)
            {
                Debug.Log("Play sound!");
                int audioClipToPlay = Random.Range(0, audioClips.Length - 1);
                GetComponent<AudioSource>().PlayOneShot(audioClips[audioClipToPlay]);
            }
            btnPressedLast = true;            
        }
        else if (btnPressedLast)
        {
            for (int i = 0; i < 3; i++)
            {
                totalTestRangeRight[i] = 0;
                amtTestedRight = 0;
            }
            btnPressedLast = false;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "BounceObject")
        {
            Vector3 contactNormal = col.contacts[0].normal;
            rigidBody.AddForce(contactNormal * force, ForceMode.Impulse);
        }
        else if(col.gameObject.tag == "Enemy")
        {
            if(col.gameObject.GetComponent<DiffuseScript>() != null)
            {
                col.gameObject.GetComponent<DiffuseScript>().StartDissolve(col.contacts[0].point);
            }
            else
            {
                Destroy(col.gameObject);
            }            
        }
    }

    void FlyRight()
    {
        if (amtTestedRight < testRange)
        {
            if (amtTestedRight > 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    totalTestRangeRight[i] += (localPosRightCube[i] - previousYPosRight[i]);
                }
            }

            for (int i = 0; i < 3; i++)
            {
                previousYPosRight[i] = localPosRightCube[i];
            }

            amtTestedRight++;
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                averageRight[i] = totalTestRangeRight[i] / amtTestedRight;
                totalTestRangeRight[i] = 0;
            }

            amtTestedRight = 0;

            rigidBody.AddForce(new Vector3(averageRight[0], averageRight[1], averageRight[2]) * forceMoveSphere);
        }
    }
}
