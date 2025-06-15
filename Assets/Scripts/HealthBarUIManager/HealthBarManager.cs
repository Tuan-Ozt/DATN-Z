using UnityEngine;
using UnityEngine.UI;
using Fusion;

public class HealthBarManager : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    private HealthSystem target;
    private float localHealth;

    public void Init(HealthSystem health)
    {
        target = health;
        localHealth = target.CurrentHealth;
    }

    public void DecreaseLocalHealth(float amount)
    {
        localHealth = Mathf.Max(localHealth - amount, 0);
    }

    private void Update()
    {
        if (target != null)
        {
            healthSlider.value = localHealth / target.MaxHealth;
        }
    }
}
