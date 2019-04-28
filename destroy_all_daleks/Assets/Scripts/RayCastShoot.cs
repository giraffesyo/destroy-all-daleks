using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastShoot : MonoBehaviour
{

    public float fireRate = .25f;
    public float range = 50;
    public ParticleSystem smokeParticles;
    //public GameObject hitParticles; // not using yet
    public GameObject shootFlare;
    public int damage = 1;
    public Transform gunEnd;

    private Camera fpsCam;
    private LineRenderer lineRenderer;
    private WaitForSeconds shotLength = new WaitForSeconds(.07f);
    private AudioSource source;
    private float nextFireTime;
    private int damageToEnemy = 0;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        source = GetComponent<AudioSource>();
        fpsCam = GetComponentInParent<Camera>();
    }

    void Update()
    {
        RaycastHit hit;
        //Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(.5f, .5f, 0));
        Ray ray = new Ray(transform.position, transform.forward);

        if (Input.GetButtonDown("Fire1") && Time.time > nextFireTime)
        {
            nextFireTime = Time.time + fireRate;

            if (Physics.Raycast(ray, out hit, range))
            {
                //IDamageable dmgScript = hit.collider.gameObject.GetComponent<IDamageable>();
                //if (dmgScript != null)
                //{
                //    dmgScript.Damage(damage, hit.point);
                //}

                //if (hit.rigidbody != null)
                //{
                //    hit.rigidbody.AddForce(-hit.normal * 100f);
                //}

                lineRenderer.SetPosition(0, ray.origin);
                if (Physics.Raycast(ray, out hit, range))
                {
                    lineRenderer.SetPosition(1, hit.point);
                    if (hit.collider.gameObject.CompareTag("enemy"))
                    {
                        //damageDalek();
                        this.damageToEnemy++;
                        if (this.damageToEnemy >= 10)
                        {
                            EnemyController ec = hit.rigidbody.GetComponent<EnemyController>();
                            ec.isDead = true;
                        }
                    }
                }
                else
                {
                    lineRenderer.SetPosition(1, ray.GetPoint(range));
                }
                //lineRenderer.SetPosition(0, gunEnd.position);
                //lineRenderer.SetPosition(1, hit.point);
                //Instantiate(hitParticles, hit.point, Quaternion.identity);

                // code to kill dalek   // need to work out how to keep track of damage seperately (or just assume that they won't
                // fight more than one at a time and just use a counter
            }
            StartCoroutine(ShotEffect());
        }

    }

    private IEnumerator ShotEffect()
    {
        lineRenderer.enabled = true;
        source.Play();
        smokeParticles.Play();
        shootFlare.SetActive(true);
        yield return shotLength;
        lineRenderer.enabled = false;
        shootFlare.SetActive(false);
    }
}
