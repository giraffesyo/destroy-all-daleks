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
    public DeathSounds deathsounds;

    public bool isDead = false;

    AudioSource[] exterminateSounds;


    [HideInInspector]
    public int health;

    bool canPlay = true;
 
    void Awake()
    {
        deathsounds = GameObject.FindObjectOfType<DeathSounds>();
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
        deathsounds.Play(exterminateSounds[0]);
        yield return new WaitForSeconds(3.0f);
        Destroy(this.gameObject);
    }

    public void Damage(int damage, Vector3 hitPoint)
    {
        this.health -= damage;
        Debug.Log($"Dalek {this.gameObject.name} was hit for {damage} and has {health} health remaining");
    }
}
