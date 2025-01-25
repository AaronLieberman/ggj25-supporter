using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    [SerializeField] Vector3 MovePerSecond = new Vector3 (-1, 0, 0);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (MovePerSecond * Time.deltaTime);
    }
}
