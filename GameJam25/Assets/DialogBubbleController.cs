using UnityEngine;

public class DialogBubbleController : MonoBehaviour
{

    [SerializeField] AnimationCurve SpawnCurve = new AnimationCurve();
    [SerializeField] float SpawnDuration = 1;
    [SerializeField] AnimationCurve DespawnCurve = new AnimationCurve();
    [SerializeField] float DespawnDuration = 1;

    [SerializeField] public float Duration = 4.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private float startTime = 0;
    private Vector3 startingScale;


    void Start()
    {
        startingScale = transform.localScale;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float timeSinceSpawn = Time.time - startTime;
        if (timeSinceSpawn < SpawnDuration)
        {
            float currentScaleMultiplyer = DespawnCurve.Evaluate(timeSinceSpawn);
            transform.localScale = startingScale * currentScaleMultiplyer;
        }

        if (SpawnDuration < timeSinceSpawn && timeSinceSpawn < SpawnDuration + Duration)
        {
            float currentScaleMultiplyer = DespawnCurve.Evaluate(timeSinceSpawn);
            transform.localScale = startingScale * currentScaleMultiplyer;
        }
        else
        {

            if (Time.time - startTime < SpawnDuration + Duration + DespawnDuration)
            {
                float currentScaleMultiplyer = DespawnCurve.Evaluate(timeSinceSpawn);
                transform.localScale = startingScale * currentScaleMultiplyer;
            }
        }
    }
}
