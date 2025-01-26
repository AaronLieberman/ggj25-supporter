using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] List<AudioClip> HealClips;
    AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        Health = MaxHealth;
    }

    AudioClip GetRandomAudioClip(List<AudioClip> audioClips)
    {
        if (!audioClips.Any())
            return null;

        return audioClips[UnityEngine.Random.Range(0, audioClips.Count - 1)];
    }

    public void Damage(int amount = 1)
    {
        if (!Utilities.GetRootComponent<PhaseManager>().Invincible)
        {
            Health -= amount;
        }

        foreach (var carriable in GetComponentsInChildren<Carriable>())
        {
            Destroy(carriable.gameObject);
        }

        AudioClip damageClip = GetRandomAudioClip(TakeDamageClips);
        if (damageClip)
        {
            _audioSource.PlayOneShot(damageClip);
        }
    }

    public void Heal(int amount = 1)
    {
        Health += amount;
        AudioClip healClip = GetRandomAudioClip(HealClips);
        if (healClip)
        {
            _audioSource.PlayOneShot(healClip);
        }
    }
}
