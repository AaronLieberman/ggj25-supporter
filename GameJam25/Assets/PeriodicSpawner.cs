using UnityEngine;

public class PeriodicSpawner : MonoBehaviour
{
    public float RateInSeconds = 1;
    public GameObject SpawnPrefab;
    public Transform SpawnPosition;
    
    bool _enabled;
    float _lastSpawn;

    void Start()
    {
        _lastSpawn = Time.time;
    }

    public void SetEnabled(bool v)
    {
        _enabled = v;
        _lastSpawn = Time.time;
    }

    void Update()
    {
        if (SpawnPrefab == null || SpawnPosition == null || !_enabled)
            return;

        if (Time.time - _lastSpawn > RateInSeconds / Time.timeScale)
        {
            Instantiate(SpawnPrefab, SpawnPosition);
            _lastSpawn = Time.time;
        }
    }
}
