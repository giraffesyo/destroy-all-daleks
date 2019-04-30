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

    PlayerUIScript HUD;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        source = GetComponent<AudioSource>();
        fpsCam = GetComponentInParent<Camera>();
        lineRenderer.startWidth = 0.025f;
        HUD = GameObject.FindGameObjectWithTag("HUD").GetComponent<PlayerUIScript>();
    }

    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        if (Input.GetButtonDown("Fire1") && Time.time > nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            HUD.Fire();

            lineRenderer.SetPosition(0, ray.origin);
            if (Physics.Raycast(ray, out hit, range))
            {
                lineRenderer.SetPosition(1, hit.point);
                IDamageable dmgScript = hit.collider.gameObject.GetComponent<IDamageable>();
                if (dmgScript != null)
                {
                    dmgScript.Damage(damage, hit.point);
                }
                /*
                if (hit.rigidbody != null)
                {
                              
                    hit.rigidbody.AddForce(-hit.normal * 100f);
                }*/
            }
            else
            {
                lineRenderer.SetPosition(1, ray.GetPoint(range));
            }
            //Instantiate(hitParticles, hit.point, Quaternion.identity);
            StartCoroutine(ShotEffect());
        }
    }

    private IEnumerator ShotEffect()
    {
        lineRenderer.enabled = true;
        source.Play();
        //smokeParticles.Play();
        shootFlare.SetActive(true);
        yield return shotLength;
        lineRenderer.enabled = false;
        shootFlare.SetActive(false);
    }
}
