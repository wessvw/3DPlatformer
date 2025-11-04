using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class enemyScript : MonoBehaviour
{

    public float viewRadius;

    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;
    private NavMeshAgent agent;
    private bool isChasing = false;
    private bool waschasing = false;
    private float rotateTimer = 0f;
    public float rotateInterval = 2f;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();

    void Start()
    {
        StartCoroutine("FindTargetsWithDelay", .2f);
        StartCoroutine("RotateRandomly", 2f);
        agent = this.GetComponent<NavMeshAgent>();
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    IEnumerator activateConfusedLookAround()
    {
        for (int i = 0; i < 4; i++)
        {
            if (!isChasing)
            {
                transform.Rotate(new Vector3(0, 90, 0));
                yield return new WaitForSeconds(1f);
            }
        }
    }

    void Update()
    {
        if (!isChasing && !waschasing)
        {
            rotateTimer += Time.deltaTime;

            if (rotateTimer >= rotateInterval)
            {
                RotateRandomly();
                rotateTimer = 0f;
            }
        }
    }

    void RotateRandomly()
    {
        if(!waschasing)
        {
            transform.Rotate(new Vector3(0, 20, 0));
        }
    }


    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                }
            }
        }
        moveAgent();
    }

    void moveAgent()
    {
        if (visibleTargets.Count > 0)
        {
            // Quaternion.LookRotation(visibleTargets[0].position);
            isChasing = true;
            waschasing = true;
            agent.SetDestination(visibleTargets[0].position);
            Quaternion.LookRotation(visibleTargets[0].position);
        }
        else if (isChasing == false && waschasing == true)
        {
            StartCoroutine("activateConfusedLookAround");
            waschasing = false;
        }
        else
        {
            isChasing = false;
        }
    }
}

