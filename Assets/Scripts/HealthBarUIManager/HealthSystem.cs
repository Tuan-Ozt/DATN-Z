using Fusion;
using UnityEngine;

public class HealthSystem : NetworkBehaviour
{
    [Networked] public float CurrentHealth { get; set; }
    public float MaxHealth = 100f;

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            CurrentHealth = MaxHealth;
        }
    }

    public void TakeDamage(float amount)
    {
        if (HasStateAuthority)
        {
            CurrentHealth = Mathf.Max(CurrentHealth - amount, 0);
        }
    }
}

