﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    LineRenderer line;
    public int shotDistance = 20;
    bool canShoot = false;
    bool reloaded = true;
    public float reloadTime = 2.0f;
    public float fadeTime = 0.1f;
    Transform player;

    EnemyController ec;
    bool isDead;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.enabled = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        line.endWidth = 0.1f;
        line.startWidth = 0.07f;
        ec = GetComponentInParent<EnemyController>();
    }

    void Update()
    {
        isDead = ec.isDead;
        if (!isDead)
        {
            var distance = Vector3.Distance(player.transform.position, this.transform.position);

            if (distance > 7 || reloaded == false)
            {
                canShoot = false;
            }
            else { canShoot = true; }

            if (canShoot)
            {
                canShoot = false;
                reloaded = false;
                StopCoroutine("Shoot");
                StartCoroutine("Shoot");
            }
        }
    }

    IEnumerator Shoot()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        line.SetPosition(0, ray.origin);

        if (Physics.Raycast(ray, out hit, shotDistance))
        {
            line.SetPosition(1, hit.point);
            if (hit.transform == player)
            {
                //damagePlayer();
                Debug.Log("Hit the player");
            }
        }
        else
        {
            line.SetPosition(1, ray.GetPoint(shotDistance));
        }

        line.enabled = true;
        yield return new WaitForSeconds(fadeTime);
        line.enabled = false;

        yield return new WaitForSeconds(reloadTime);
        reloaded = true;
    }
}