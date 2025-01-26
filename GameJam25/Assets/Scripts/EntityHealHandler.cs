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

                if (entityResources.Health >= entityResources.MaxHealth) return;

                entityResources.Heal(healAmount);
                Destroy(gameObject);
            }
        }
    }
}
