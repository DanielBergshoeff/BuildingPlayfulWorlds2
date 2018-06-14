using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotScript : MonoBehaviour {

    public float shotDuration = 0.3f;
    public float shotRange = 50f;

    public Transform target;
    public AudioClip shotClip;

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

            GetComponent<AudioSource>().clip = shotClip;
            GetComponent<AudioSource>().Play();

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

            target.GetComponent<Health>().TakeDamage(1);
        }
    }

    private IEnumerator ShootLeftEffect()
    {
        lineRendererLeft.enabled = true;
        yield return new WaitForSeconds(shotDuration);
        lineRendererLeft.enabled = false;
    }
}
