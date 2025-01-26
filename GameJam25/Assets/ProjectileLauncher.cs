using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class ProjectileLauncher : MonoBehaviour
{
    public float ShotsPerSecond;
    public List<GameObject> Projectiles = new();
    public bool ShootImmediately = true;
    public Vector3 ShootDirection;
    public float ShootConeDegrees;

    float _timeLastShot;
    int _lastProjIndex = 0;

    void Start()
    {
        _timeLastShot = ShootImmediately ? 0 : Time.time;
    }

    void Update()
    {
        if (Time.time - _timeLastShot > 1 / ShotsPerSecond)
        {
            Shoot();
            _timeLastShot = Time.time;
        }
    }

    private void Shoot()
    {
        _lastProjIndex = (_lastProjIndex + 1) % Projectiles.Count;

        GameObject thisProj = Projectiles[_lastProjIndex];
        var go = Instantiate(thisProj, transform.position, Quaternion.identity);
        if (ShootDirection.sqrMagnitude > 0.0001f)
        {
            var projectileMovement = go.GetComponent<ProjectileMovement>();
            if (projectileMovement != null)
            {
                float randomAngle = Random.Range(-ShootConeDegrees / 2, ShootConeDegrees / 2);
                Quaternion rotation = Quaternion.Euler(0, 0, randomAngle);
                Vector3 randomDirection = rotation * ShootDirection;
                projectileMovement.GoInDirection(randomDirection);
            }
        }
    }
}
