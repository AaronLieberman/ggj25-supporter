using UnityEngine;

public class PeriodicSpawner : MonoBehaviour
{
    public float RateInSeconds = 1;
    public GameObject SpawnPrefab;
    
    float _lastSpawn = 0;

    void Start()
    {
        _lastSpawn = Time.time;
    }

    void Update()
    {
        if (SpawnPrefab == null)
            return;

        if (Time.time - _lastSpawn > RateInSeconds / Time.timeScale)
        {
            Instantiate(SpawnPrefab, transform);
        }
    }
}
