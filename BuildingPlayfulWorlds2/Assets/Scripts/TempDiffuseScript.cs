using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempDiffuseScript : MonoBehaviour {

    private Material mat;

    public float startingY, dissolveY, sizeDissolve, sliceAmount;

	// Use this for initialization
	void Start () {
        mat = GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
        mat.SetFloat("_StartingY", startingY);
        mat.SetFloat("_DissolveY", dissolveY);
        mat.SetFloat("_SliceAmount", sliceAmount);
        mat.SetFloat("_DissolveSize", sizeDissolve);
        Debug.Log(sliceAmount);
	}
}
