using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityDamageHandler : MonoBehaviour
{
    float damageCooldown = 5f;
    float _timeSinceLastDamage;
    EntityResources _entityResources;

    readonly HashSet<Damager> _damagers = new();

    public bool InHurtState { get; private set; }

    void AttachEntityResources()
    {
        _entityResources = GetComponent<EntityResources>();
    }

    private void Start()
    {
        AttachEntityResources();
    }

    private void FixedUpdate()
    {
        if (!_entityResources)
        {
            AttachEntityResources();
        }

        if (_damagers.Count > 0 && _timeSinceLastDamage <= Time.time)
        {
            StartCoroutine(ApplyHurt());

            var damagers = _damagers;
            _entityResources.Damage((int)damagers.Sum(d => d.DamageAmount));

            var damagersToDestroy = damagers.Where(d => d.DestroyOnContact).ToList();
            foreach (var damager in damagersToDestroy)
            {
                Destroy(damager.gameObject);
                _damagers.Remove(damager);
            }

            _timeSinceLastDamage = Time.time + damageCooldown;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var damager = collision.GetComponent<Damager>();
        if (damager == null)
            return;
        _damagers.Add(damager);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var damager = collision.GetComponent<Damager>();
        if (damager == null)
            return;
        _damagers.Remove(damager);
    }

    private IEnumerator ApplyHurt()
    {
        InHurtState = true;

        yield return Utilities.WaitForSeconds(3f);

        InHurtState = false;
    }
}
