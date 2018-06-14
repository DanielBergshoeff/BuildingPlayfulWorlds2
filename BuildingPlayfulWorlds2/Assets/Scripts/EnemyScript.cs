using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum State { Idle, Move, Attack }

public class EnemyScript : MonoBehaviour
{
    public State currentState;
    public int damage = 20;
    public float attackRange;
    public float maxCooldown = 1;
    public float senseRange = 10;

    public float rotationSpeed = 3.0f;

    private int layerMask;

    private NavMeshAgent agent;
    private Health target;
    private float coolDown;
    private float distanceToTarget;

    public Animator animator;
    // Use this for initialization
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        //player = GameObject.FindGameObjectWithTag("Player");

        layerMask = 1 << 10;
        layerMask = ~layerMask;

    }

    // Update is called once per frame
    void Update()
    {

        CheckState();

    }

    void CheckForHealth()
    {
        distanceToTarget = float.MaxValue;
            Collider[] cols = Physics.OverlapSphere(transform.position, senseRange);
            foreach (Collider c in cols)
            {
                if (c.gameObject == gameObject) { continue; }
                Health hp = c.gameObject.GetComponent<Health>();
                if (hp != null)
                {
                    float distToHealthScript = Vector3.Distance(transform.position, hp.transform.position);
                    if (distToHealthScript < distanceToTarget)
                    {
                        target = hp;
                        distanceToTarget = distToHealthScript;
                    }
                }
            }
    }

    void CheckState()
    {
        //Sensing
        if (target == null)
        {
            CheckForHealth();

            if (target == null)
            {
                currentState = State.Idle;
            }

        }
        else
        {
            distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
            if (distanceToTarget > senseRange)
            {
                target = null;
            }
        }

        //States
        switch (currentState)
        {
            case State.Attack:
                //Action
                if (coolDown > 0)
                {
                    coolDown -= Time.deltaTime;
                }

                if (target != null)
                {
                    Vector3 newDir = Vector3.RotateTowards(transform.forward, (target.transform.position - transform.position), rotationSpeed * Time.deltaTime, 0.0f);
                    transform.rotation = Quaternion.LookRotation(newDir);

                    GameObject hitObject = null;
                    RaycastHit hit;
                    Physics.Raycast(transform.position, newDir, out hit, attackRange * 2);
                    if (hit.collider != null)
                    {
                        hitObject = hit.collider.gameObject;
                        Debug.Log(hitObject.name);
                        Debug.Log(target.gameObject.name);
                    }

                    //Do Damage
                    if (distanceToTarget < attackRange && coolDown <= 0 && hitObject == target.gameObject)
                    {
                        GetComponentInChildren<BotScript>().target = target.gameObject.transform;
                        animator.SetBool("ShootLeft", true);
                        coolDown = maxCooldown;
                    }
                    //Transition
                    else if (distanceToTarget > 2 * attackRange)
                    {
                        animator.SetBool("ShootLeft", false);
                        currentState = State.Move;
                        GetComponentInChildren<BotScript>().target = null;
                    }
                }

                break;

            case State.Idle:

                //if we are close pick a new position to walk to
                if (agent.remainingDistance > agent.stoppingDistance)
                {
                    break;
                }
                else
                {
                    agent.SetDestination(transform.position + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5)));
                }
                if (distanceToTarget < senseRange)
                {
                    currentState = State.Move;
                }


                break;
            case State.Move:
                //Move to the target      
                if (distanceToTarget < attackRange)
                {
                    currentState = State.Attack;
                }
                else if(target != null)
                {
                    agent.SetDestination(transform.position + (target.transform.position - transform.position) / 2);
                }

                break;
        }
    }
}
