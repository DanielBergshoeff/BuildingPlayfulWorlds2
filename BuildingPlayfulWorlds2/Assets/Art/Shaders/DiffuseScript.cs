using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiffuseScript : MonoBehaviour {

    public float sizeDissolve = 0f;
    public float sizeSlice = 0f;
    public float maxDissolve = 4.0f;
    public float timesFactor = 3.0f;
    public bool startDissolve;

    public Vector3 startPosition;

    public float speedDissolve = 0.01f;

    private Material myMaterial;

	// Use this for initialization
	void Start () {
        myMaterial = GetComponent<Renderer>().material;      
    }
	
	// Update is called once per frame
	void Update () {
        
        if (startDissolve && sizeSlice < maxDissolve)
        {
            sizeSlice += speedDissolve;                     
        }
        else if(!startDissolve && sizeSlice > 0.0f)
        {
            sizeSlice -= speedDissolve;
        }
        else
        {
            if(!startDissolve)
            {
                sizeSlice = 0.0f;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        sizeDissolve = (timesFactor * sizeSlice);
        myMaterial.SetFloat("_DissolveSize", sizeDissolve);
        myMaterial.SetFloat("_SliceAmount", sizeSlice);
    }

    public void StartDissolve(Vector3 startVector)
    {
        myMaterial.SetVector("_StartingVector", startVector);
        Debug.Log(startVector);
        startDissolve = true;
    }
}
