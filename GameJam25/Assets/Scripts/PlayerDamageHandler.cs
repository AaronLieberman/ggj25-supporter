using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageHandler : MonoBehaviour
{
    [SerializeField]
    private List<string> interactionTags;
    [SerializeField]
    private float damageCooldown = 5f;
    private float timeSinceLastDamage;
    private bool damageable = false;
    EntityResources playerResources;

    public bool InHurtState { get; private set; }

    void AttachPlayerResources()
    {
        playerResources = Utilities.GetRootComponent<PlayerController>().GetComponent<EntityResources>();
    }

    private void Start()
    {
        AttachPlayerResources();
    }

    private void FixedUpdate()
    {
        if (!playerResources)
        {
            AttachPlayerResources();
        }

        if (damageable && timeSinceLastDamage <= Time.time)
        {
            StartCoroutine(ApplyHurt());
            playerResources.Damage();
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
