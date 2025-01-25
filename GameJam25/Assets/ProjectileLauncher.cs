using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class ProjectileLauncher : MonoBehaviour
{

    [SerializeField] private float ShotsPerSecond;
    [SerializeField] private List<GameObject> Projectiles = new List<GameObject>();
    [SerializeField] private bool shootImmediately = true;

    private float timeSinceLastShot;
    private int lastProjIndex = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (shootImmediately)
        {
            timeSinceLastShot = float.MaxValue;
        }
        else
        {
            timeSinceLastShot = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        if(timeSinceLastShot > (1 / ShotsPerSecond ))
        {
            Shoot();
            timeSinceLastShot = 0;
        }
    }

    private void Shoot()
    {
        lastProjIndex = Utilities.IncrementIndex(lastProjIndex, Projectiles.Count - 1);

        GameObject thisProj = Projectiles[lastProjIndex % Projectiles.Count];
        Instantiate(thisProj, transform.position, Quaternion.identity);
    }


}
