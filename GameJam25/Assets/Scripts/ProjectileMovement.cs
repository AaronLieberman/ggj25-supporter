using UnityEditor.UI;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    [SerializeField] public Vector3 MovePerSecond = new Vector3 (-1, 0, 0);

    [Tooltip("TODO: Make this actually work right. The number of degrees that a projectile can wobble up or down from the perfect angle. ")]
    [SerializeField] public float RandomSpreadInHalfDegrees = 0;
    [SerializeField] public float MaxLifetime = -1;
    [SerializeField] public float startTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //TODO: I am sure this is wrong
        float  selectedSpread = Random.Range(-RandomSpreadInHalfDegrees, RandomSpreadInHalfDegrees);
        MovePerSecond += new Vector3(0, selectedSpread, 0);
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Carriable>() && GetComponent<Carriable>().BeingCarried) { return; }

        transform.position += (MovePerSecond * Time.deltaTime);
        if(MaxLifetime > 0 && Time.time > startTime + MaxLifetime)
        {
            Destroy(gameObject);
        }
    }
}
