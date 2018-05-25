using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyStatus
{
    WALK,
    CHASE
}

public class EnemyScript : MonoBehaviour {

    public float detectDistance = 3f;
    public float speed = 2f;

    private GameObject target;
    private EnemyStatus currentStatus;

    NavMeshAgent agent;

	// Use this for initialization
	void Start () {
        target = GameObject.Find("CenterEyeAnchor");
        agent = GetComponent<NavMeshAgent>();
        currentStatus = EnemyStatus.WALK;
	}
	
	// Update is called once per frame
	void Update () {
        if (Vector3.Distance(target.transform.position, transform.position) < detectDistance)
        {
            currentStatus = EnemyStatus.CHASE;
        }
        else
        {
            currentStatus = EnemyStatus.WALK;
        }
        if (currentStatus == EnemyStatus.WALK)
        {
            //bool wallClose = Physics.Raycast(transform.position, transform.forward, 0.2f);

            //if(Vector3.Distance(agent.destination, transform.position) < 0.3f || wallClose)
            //{
                agent.SetDestination(transform.position + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5)));
            //}
        }   
        else if(currentStatus == EnemyStatus.CHASE)
        {
            agent.destination = target.transform.position;
        }     
	}
}
