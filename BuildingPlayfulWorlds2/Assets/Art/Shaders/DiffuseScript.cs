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

	// Use this for initialization
	void Start () {
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
                if (gameObject.tag == "Enemy")
                {
                    GameManager.Points += 1;
                }
                if(gameObject.tag == "HealthShrine")
                {
                    GameManager.HealthShrines -= 1;
                }
            }
        }

        sizeDissolve = (timesFactor * sizeSlice);

        foreach (Renderer ren in GetComponentsInChildren<Renderer>())
        {
            foreach (Material mat in ren.materials)
            {
                mat.SetFloat("_DissolveSize", sizeDissolve);
                mat.SetFloat("_SliceAmount", sizeSlice);
            }
        }
    }

    public void StartDissolve(Vector3 startVector)
    {
        foreach (Renderer ren in GetComponentsInChildren<Renderer>())
        {
            foreach (Material mat in ren.materials)
            {
                mat.SetVector("_StartingVector", startVector);
            }
        }        
        startDissolve = true;
    }
}
