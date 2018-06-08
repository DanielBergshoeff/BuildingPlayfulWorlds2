using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementGain : MonoBehaviour {

    public Vector3 previousPos;
    public GameObject ovrPlayerController;

	// Use this for initialization
	void Start () {
        previousPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 moveDirection = previousPos - transform.position;
        ovrPlayerController.transform.position = ovrPlayerController.transform.position + moveDirection;
	}
}
