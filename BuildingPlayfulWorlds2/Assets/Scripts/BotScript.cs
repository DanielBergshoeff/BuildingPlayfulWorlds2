using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotScript : MonoBehaviour {

    public float shotDuration = 0.3f;
    public float shotRange = 50f;

    public Transform target;

    public GameObject armLeft;
    public GameObject armLeftEnd;
    private LineRenderer lineRendererLeft;

	// Use this for initialization
	void Start () {
        lineRendererLeft = armLeft.GetComponent<LineRenderer>();
        lineRendererLeft.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShootLeft()
    {
        if (target != null)
        {
            StartCoroutine(ShootLeftEffect());

            RaycastHit hit;
            lineRendererLeft.SetPosition(0, armLeftEnd.transform.position);

            Vector3 direction = (armLeftEnd.transform.position - target.transform.position).normalized;

            if (Physics.Raycast(armLeftEnd.transform.position, -direction, out hit, shotRange))
            {
                lineRendererLeft.SetPosition(1, hit.point);
            }
            else
            {
                lineRendererLeft.SetPosition(1, armLeftEnd.transform.position + (-direction * shotRange));
            }
        }
    }

    private IEnumerator ShootLeftEffect()
    {
        lineRendererLeft.enabled = true;
        yield return new WaitForSeconds(shotDuration);
        lineRendererLeft.enabled = false;
    }
}
