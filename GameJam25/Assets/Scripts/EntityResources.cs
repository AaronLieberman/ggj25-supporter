using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityResources : MonoBehaviour
{ 
    public bool isAlive => _health > 0;

    public int MaxHealth = 6;
    private int _health;
    public int Health
    {
        get { return _health; }
        set
        {
            _health = Mathf.Clamp(value, 0, MaxHealth);

            HealthChanged?.Invoke(this, null);
            if (_health <= 0)
            {
                Death?.Invoke();
            }
        }
    }

    public event EventHandler HealthChanged;
    public UnityAction Death;

    [SerializeField] List<AudioClip> TakeDamageClips;
    AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        Health = MaxHealth;
    }

    public void Damage(int amount=1)
    {
        Health -= amount;
        _audioSource.PlayOneShot(TakeDamageClips[UnityEngine.Random.Range(0, TakeDamageClips.Count - 1)]);
    }
}
