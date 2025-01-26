using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityHealHandler : MonoBehaviour
{
    [SerializeField] private List<string> interactionTags;
    [SerializeField] private int healAmount = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (interactionTags.Contains(collision.tag))
        {
            if (collision.GetComponent<EntityResources>())
            {
                EntityResources entityResources = collision.GetComponent<EntityResources>();
                EntityDamageHandler entityDamageHandler = collision.GetComponent<EntityDamageHandler>();

                if (entityResources.Health >= entityResources.MaxHealth) return;

                // TODO: this indeed feels too broken when you try to hand the hero a bubble
                //if (entityDamageHandler.InHurtState) return; //TODO this probably requires you to touch and untouch a bubble if it enters while you're hurt. Would like to fix this but no time. --ecarter

                entityResources.Heal(healAmount);
                Destroy(gameObject);
            }
        }
    }
}
