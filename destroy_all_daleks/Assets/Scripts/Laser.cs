using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    LineRenderer line;
    public int shotDistance = 10;
    bool canShoot = false;
    bool reloaded = true;
    public float reloadTime = 2.0f;
    public float fadeTime = 0.1f;
    Transform player;

    void Awake()
    {
        line = GetComponentInChildren<LineRenderer>();
        line.enabled = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        var distance = Vector3.Distance(player.transform.position, this.transform.position);

        if (distance > 7 || reloaded == false)
        {
            canShoot = false;
        }
        else { canShoot = true;  }

        if (canShoot)
        {
            canShoot = false;
            reloaded = false;
            StopCoroutine("Shoot");
            StartCoroutine("Shoot");
        }

        Ray ray = new Ray(transform.position, transform.forward);
        line.SetPosition(0, ray.origin);
        line.SetPosition(1, ray.GetPoint(shotDistance));
    }

    IEnumerator Shoot()
    {
        line.enabled = true;
        //damagePlayer();
        yield return new WaitForSeconds(fadeTime);
        line.enabled = false;

        yield return new WaitForSeconds(reloadTime);
        reloaded = true;
    }
}
