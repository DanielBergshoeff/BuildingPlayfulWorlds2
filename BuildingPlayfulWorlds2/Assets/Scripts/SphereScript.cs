using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereScript : MonoBehaviour {

    public float force = 10f;

    public float forceMoveSphere = 1000.0f;

    public AudioClip[] audioClips;

    private bool btnPressedLast;
    private bool pull;

    public float pullBackSpeed = 5.0f;

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

        if(OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch) > 0.1 && OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.LTouch) > 0.1) {
            StartCoroutine(Teleport());
        }
        else if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch) > 0.1)
        {
            FlyRight();
            if (!btnPressedLast)
            {
                int audioClipToPlay = Random.Range(0, audioClips.Length - 1);
                GetComponent<AudioSource>().PlayOneShot(audioClips[audioClipToPlay]);
            }
            btnPressedLast = true;            
        }
        else if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) > 0.1)
        {
            //pullBackObject.GetComponent<Rigidbody>().useGravity = false;
            rigidBody.isKinematic = true;
            rigidBody.GetComponent<Rigidbody>().velocity = Vector3.zero;
            rigidBody.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            pull = true;
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

        if (pull == true)
        {
            if (Vector3.Distance(transform.position, rightCube.transform.position) < 0.01f)
            {
                //pullBackObject.GetComponent<Rigidbody>().useGravity = true;
                GetComponent<Rigidbody>().isKinematic = false;
                pull = false;
                GetComponent<Renderer>().material.SetColor("_Color", Color.white);
            }
            else
            {
                PullToHand();
            }
        }
    }

    void OnColliderEnter (Collision col)
    {
        if(col.gameObject.tag == "Enemy")
        {
            if(col.gameObject.GetComponent<DiffuseScript>() != null)
            {
                if (col.gameObject.GetComponent<DiffuseScript>().startDissolve != true)
                {
                    col.gameObject.GetComponent<DiffuseScript>().StartDissolve(transform.position);
                }
            }
            else
            {
                Destroy(col.gameObject);
            }            
        }
    }

    void PullToHand()
    {
        transform.position = Vector3.MoveTowards(transform.position, rightCube.transform.position, Time.deltaTime * pullBackSpeed);
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

    IEnumerator Teleport()
    {
        Vector3 currentPosOrb = transform.position;
        Vector3 currentPosPlayer = GameObject.Find("OVRPlayerController").transform.position;
        GameObject.Find("CenterEyeAnchor").GetComponent<OVRScreenFade>().FadeOut();
        yield return new WaitForSeconds(0.25f);
        transform.position = currentPosPlayer;
        GameObject.Find("OVRPlayerController").transform.position = currentPosOrb;
        GameObject.Find("CenterEyeAnchor").GetComponent<OVRScreenFade>().FadeIn();
    }
}
