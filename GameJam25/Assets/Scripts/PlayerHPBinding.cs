using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBinding : MonoBehaviour
{
    PlayerController _player;

    public GameObject HeartPrefab;

    public int HeartSize = 100;

    private List<HeartSpriteSwitch> hearts = new List<HeartSpriteSwitch>();

    void Awake()
    {
        _player = Utilities.GetRootComponent<PlayerController>();
    }

    private void Start()
    {
        var playerHealth = _player.EntityResources;
        playerHealth.HealthChanged += UpdateHealth;

        for (int i = 0; i < playerHealth.MaxHealth; i++)
        {
            var HeartObj = Instantiate(HeartPrefab, this.transform);
            hearts.Add(HeartObj.GetComponent<HeartSpriteSwitch>());
            var rectTransform = HeartObj.transform as RectTransform;
            rectTransform.anchoredPosition = new Vector2(0 + HeartSize * i, 0);
        }
    }
    private void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            //PlayerResources.Instance.Damage(1);
        }
    }

    void UpdateHealth()
    {
        var playerHealth = _player.EntityResources;
        for (int i = 0; i < playerHealth.MaxHealth; i++)
        {
            var heart = hearts[i];
            heart.Filled = (i + 1) <= playerHealth.Health;
        }
    }
}
