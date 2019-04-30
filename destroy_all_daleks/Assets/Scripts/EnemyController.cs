using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IDamageable
{
    Transform player;
    NavMeshAgent nav;
    public float rotationSpeed = .01f;
    public float startChaseRange = 10.0f;

    public bool isDead = false;

    AudioSource[] exterminateSounds;
    AudioSource ex1;
    AudioSource ex2;
    AudioSource ex3;
    AudioSource ex4;
    AudioSource ex5;
    AudioSource ex6;
    AudioSource ex7;
    AudioSource ex8;
    AudioSource ex9;
    AudioSource ex10;
    AudioSource ex11;

    [HideInInspector]
    public int health;

    bool canPlay = true;
 
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

        exterminateSounds = this.gameObject.GetComponents<AudioSource>();
        ex1 = exterminateSounds[0];
        ex2 = exterminateSounds[1];
        ex3 = exterminateSounds[2];
        ex4 = exterminateSounds[3];
        ex5 = exterminateSounds[4];
        ex6 = exterminateSounds[5];
        ex7 = exterminateSounds[6];
        ex8 = exterminateSounds[7];
        ex9 = exterminateSounds[8];
        ex10 = exterminateSounds[9];
        ex11 = exterminateSounds[10];
    }

    void Update()
    {
        int random = Random.Range(0, exterminateSounds.Length);
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
                if (canPlay)
                {
                    canPlay = false;
                    exterminateSounds[random].Play();
                }
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

    public void Damage(int damage, Vector3 hitPoint)
    {
        this.health -= damage;
        Debug.Log($"Dalek {this.gameObject.name} was hit for {damage} and has {health} health remaining");
    }
}
