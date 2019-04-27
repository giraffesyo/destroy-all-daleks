using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    Transform player;
    NavMeshAgent nav;
    public float rotationSpeed = .01f;
    public float startChaseRange = 10.0f;

    public bool isDead = false;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nav = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (!isDead)
        {
            var distance = Vector3.Distance(player.transform.position, this.transform.position);
            if (distance <= startChaseRange)
            {
                nav.SetDestination(player.position);
            }
            if (distance <= 4)
            {
                Vector3 direction = (player.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            }
        }
    }
}
