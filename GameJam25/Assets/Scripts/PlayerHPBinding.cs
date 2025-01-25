using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class PlayerHPBinding : MonoBehaviour
{
    [SerializeField] EntityResources EntityResources;

    public GameObject HeartPrefab;

    public int HeartSize = 100;

    private List<HeartSpriteSwitch> hearts = new List<HeartSpriteSwitch>();

    private void Start()
    {
        EntityResources.HealthChanged += (_, __) => UpdateHealth();

        for (int i = 0; i < EntityResources.MaxHealth; i++)
        {
            var HeartObj = Instantiate(HeartPrefab, this.transform);
            hearts.Add(HeartObj.GetComponent<HeartSpriteSwitch>());
            var rectTransform = HeartObj.transform as RectTransform;
            rectTransform.anchoredPosition = new Vector2(0 + HeartSize * i, 0);
        }
    }

    void UpdateHealth()
    {
        for (int i = 0; i < EntityResources.MaxHealth; i++)
        {
            var heart = hearts[i];
            heart.Filled = (i + 1) <= EntityResources.Health;
        }
    }
}
