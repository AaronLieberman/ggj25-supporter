using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    public static HeroController _instance;
    public static HeroController Instance => _instance;

    [HideInInspector] public EntityResources EntityResources;

    enum HeroState { Idle, Running }

    //[SerializeField] float MovementSpeed = 6;

    //HeroState _heroState = HeroState.Idle;

    [SerializeField] List<AudioClip> FootstepClips;
    [SerializeField] float FootstepInterval = 0.15f;
    float _footstepTimer = 0f;

    Rigidbody2D _rigidBody;
    SpriteRenderer _spriteRenderer;
    Animator _animator;
    BoxCollider2D _boxCollider;
    AudioSource _audioSource;

    void Start()
    {
        EntityResources = GetComponent<EntityResources>();

        _rigidBody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        _boxCollider = GetComponentInChildren<BoxCollider2D>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
       
    }

    void PlayFootsteps()
    {
        _footstepTimer += Time.deltaTime;
        if (_footstepTimer >= FootstepInterval)
        {
            _footstepTimer = 0f;
            _audioSource.PlayOneShot(FootstepClips[UnityEngine.Random.Range(0, FootstepClips.Count - 1)], 0.5f);
        }
    }

    public void DashComplete()
    {
        // _heroState = HeroState.Idle;
        _rigidBody.linearVelocity = new Vector2(0.0f, 0.0f);
    }

    public IEnumerator FloatTo(float x, float y, float seconds)
    {
        Debug.Log("Hero floating down");
        yield return Utilities.WaitForSeconds(seconds);
        Debug.Log("Hero landed");
    }

    public IEnumerator WalkTo(float x, float y, float seconds)
    {
        Debug.Log("Hero walking");
        yield return Utilities.WaitForSeconds(seconds);
        Debug.Log("Hero stopped");
    }

    public IEnumerator Say(string v, float seconds)
    {
        Debug.Log("Hero says: \"" + v + "\"");
        yield return Utilities.WaitForSeconds(seconds);
    }

    public IEnumerator Throw(string itemName, float x, float y, float seconds)
    {
        Debug.Log("Hero throws " + itemName);
        yield return Utilities.WaitForSeconds(seconds);
    }
}
