using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    Transform player;
    NavMeshAgent nav;
    float timeSinceLastShot = 0.0f;
    public float rotationSpeed = .01f;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        nav.SetDestination(player.position);
        timeSinceLastShot += Time.deltaTime;
        // if we're within 8 units of the player, and its been 2 seconds since our last shot, shoot the player
        if (nav.remainingDistance <= 8.2f)
        {
            LookAtPlayer();

            if (timeSinceLastShot > 2.0f)
            {
                Debug.Log("We are within range, shooting player");
                timeSinceLastShot = 0.0f;
            }
        }


    }

    void LookAtPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

    }

}
