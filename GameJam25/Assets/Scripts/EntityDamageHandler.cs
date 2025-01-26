using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityDamageHandler : MonoBehaviour
{
    [SerializeField] public float PostDamageInvicibilitySeconds = 0.1f;
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

            _entityResources.Damage((int)_damagers.Sum(d => d.DamageAmount));

            foreach (var damager in _damagers.Where(d => d.DestroyOnContact).ToList())
            {
                Destroy(damager.gameObject);
                _damagers.Remove(damager);
            }

            _timeSinceLastDamage = Time.time + PostDamageInvicibilitySeconds;
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

        yield return Utilities.WaitForSeconds(PostDamageInvicibilitySeconds);

        InHurtState = false;
    }
}
