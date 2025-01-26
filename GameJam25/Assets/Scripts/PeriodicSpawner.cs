using UnityEngine;

public class PeriodicSpawner : MonoBehaviour
{
    [SerializeField] bool startEnabled = false;
    public float RateInSeconds = 1;
    public float InitialWait = 1;
    public GameObject SpawnPrefab;
    public Transform SpawnPosition;
    
    bool _enabled;
    float _lastSpawn;

    void Start()
    {
        _enabled = startEnabled;
        _lastSpawn = Time.time;
    }

    public void SetEnabled(bool v)
    {
        _enabled = v;
        _lastSpawn = Time.time - (RateInSeconds - InitialWait);
    }

    void Update()
    {
        if (SpawnPrefab == null || SpawnPosition == null || !_enabled)
            return;

        if (Time.time - _lastSpawn > RateInSeconds)
        {
            Instantiate(SpawnPrefab, SpawnPosition);
            _lastSpawn = Time.time;
        }
    }
}
