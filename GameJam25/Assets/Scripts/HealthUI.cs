using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] EntityResources EntityResources;
    [SerializeField] Slider HealthSlider;

    void Start()
    {
        if (!EntityResources) { return; }
        
        EntityResources.HealthChanged += (_, __) => RefreshHealthUI();
    }

    void RefreshHealthUI()
    {
        if (!HealthSlider) { return; }

        HealthSlider.value = (float)EntityResources.Health / (float)EntityResources.MaxHealth;
    }
}
