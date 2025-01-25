using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class DialogBubbleController : MonoBehaviour
{

    [SerializeField] AnimationCurve SpawnCurve = new AnimationCurve();
    [SerializeField] float SpawnDuration = 1;
    [SerializeField] AnimationCurve DespawnCurve = new AnimationCurve();
    [SerializeField] float DespawnDuration = 1;

    [SerializeField] public float Duration = 4.5f;

    [SerializeField] TextMeshPro dialogText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private float startTime = 0;
    private Vector3 startingScale;


    void Start()
    {
        startingScale = transform.localScale;
        startTime = Time.time;
    }

    public void PopDialog(string dialogKey)
    {

    }

    // Update is called once per frame
    void Update()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();

        float timeSinceSpawn = Time.time - startTime;

        // this is useful for debugging the animation:
        // timeSinceSpawn = timeSinceSpawn % (SpawnDuration + Duration + DespawnDuration);

        if (timeSinceSpawn < SpawnDuration)
        {
            float currentScaleMultiplyer = SpawnCurve.Evaluate(timeSinceSpawn);
            rectTransform.localScale = startingScale * currentScaleMultiplyer;
        }

        if (SpawnDuration < timeSinceSpawn && timeSinceSpawn < SpawnDuration + Duration)
        {
            float currentScaleMultiplyer = 1.0f;
            rectTransform.localScale = startingScale * currentScaleMultiplyer;
        }

        if (SpawnDuration + Duration < timeSinceSpawn && timeSinceSpawn < SpawnDuration + Duration + DespawnDuration)
        {
            float currentScaleMultiplyer = DespawnCurve.Evaluate(timeSinceSpawn - SpawnDuration - Duration);
            rectTransform.localScale = startingScale * currentScaleMultiplyer;
        }
    }
}
