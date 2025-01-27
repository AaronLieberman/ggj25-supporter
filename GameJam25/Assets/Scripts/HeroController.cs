using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    [HideInInspector] public EntityResources EntityResources;

    enum HeroState { Idle, Running }

    // HeroState _heroState = HeroState.Idle;

    [SerializeField] List<AudioClip> FootstepClips;
    [SerializeField] float FootstepInterval = 0.15f;
    float _footstepTimer = 0f;

    Rigidbody2D _rigidBody;
    SpriteRenderer _spriteRenderer;
    Animator _animator;
    BoxCollider2D _boxCollider;
    AudioSource _audioSource;
    DialogBubbleController _dialogBubble;
    Shadow _shadow;

    private float lastSpeech;

    private void Awake()
    {
        EntityResources = GetComponent<EntityResources>();
    }

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        _boxCollider = GetComponentInChildren<BoxCollider2D>();
        _audioSource = GetComponent<AudioSource>();
        _dialogBubble = GetComponentInChildren<DialogBubbleController>();
        _shadow = GetComponentInChildren<Shadow>();
    }

    void Update()
    {
        var damageHandler = GetComponentInChildren<EntityDamageHandler>();
        var spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        bool isInHurtState = damageHandler != null && damageHandler.InHurtState;
        spriteRenderer.color = isInHurtState
            ? new Color(1, Mathf.PingPong(Time.time * 3, 1), Mathf.PingPong(Time.time * 3, 1))
            : new Color(1, 1, 1);
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

    public IEnumerator Say(string v)
    {
        lastSpeech = Time.time;
        Debug.Log("Hero says: \"" + v + "\"");
        return _dialogBubble.PopDialog(v);
    }

    public IEnumerator Throw(string itemName, Vector2 targetPos)
    {
        Debug.Log("Hero throws " + itemName);
        yield return Utilities.WaitForSeconds(1);
    }

    public IEnumerator Brag()
    {
        while (true)
        {
            yield return Utilities.WaitForSeconds(1);
            if (Time.time > lastSpeech + 15)
            {
                Say("Hero_Brag");
            }
        }
    }

    public void LoseDivingGear()
    {
        transform.Find("Root").Find("GFXHelmet").gameObject.SetActive(false);
        transform.Find("Root").Find("GFX").gameObject.SetActive(true);
    }

    public void SetMachineGunActive(bool v)
    {
        Debug.Log("TODO: Hero starts shooting machine gun");
        //TODO ugh
        var machineGun = transform.Find("Root").Find("MachineGun");
        if (machineGun)
            machineGun.gameObject.SetActive(v);
    }

    internal void StartBragging()
    {
        StartCoroutine(Brag());
    }
}
