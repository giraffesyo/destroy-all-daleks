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

    [HideInInspector]
    public int health;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nav = GetComponent<NavMeshAgent>();
        if (this.gameObject.name == "Dalek")
        {
            health = 5;
        }
        else if (this.gameObject.name == "Dalek - Blue")
        {
            health = 10;
        }
        else if (this.gameObject.name == "Dalek - Gold")
        {
            health = 15;
        }
    }

    void Update()
    {
        if (health <= 0)
        {
            isDead = true;
        }
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
        else
        {
            StartCoroutine("DestroyDalek");
        }
    }

    IEnumerator DestroyDalek()
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(this.gameObject);
    }
}
