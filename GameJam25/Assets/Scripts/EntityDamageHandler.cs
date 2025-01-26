using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDamageHandler : MonoBehaviour
{
    [SerializeField]
    private List<string> interactionTags;
    [SerializeField]
    private int damageAmount = 1;
    [SerializeField]
    private float damageCooldown = 5f;
    private float timeSinceLastDamage;
    private bool damageable = false;
    EntityResources entityResources;

    public bool InHurtState { get; private set; }

    void AttachPlayerResources()
    {
        entityResources = transform.parent.parent.GetComponent<EntityResources>();
    }

    private void Start()
    {
        AttachPlayerResources();
    }

    private void FixedUpdate()
    {
        if (!entityResources)
        {
            AttachPlayerResources();
        }

        if (damageable && timeSinceLastDamage <= Time.time)
        {
            StartCoroutine(ApplyHurt());
            entityResources.Damage(damageAmount);
            timeSinceLastDamage = Time.time + damageCooldown;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (interactionTags.Contains(collision.tag))
            damageable = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (interactionTags.Contains(collision.tag))
            damageable = false;
    }

    private IEnumerator ApplyHurt()
    {
        InHurtState = true;

        yield return Utilities.WaitForSeconds(3f);

        InHurtState = false;
    }
}
