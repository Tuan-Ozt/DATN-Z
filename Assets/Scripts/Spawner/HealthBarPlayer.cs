using UnityEngine;
using Fusion;

public class HealthbarPlayer : NetworkBehaviour
{
    private HealthSystem health;

    public void Init(HealthSystem healthSystem)
    {
        health = healthSystem;
    }

    public HealthSystem GetHealthSystem()
    {
        return health;
    }

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            Debug.Log("Spawned local player → gán UI máu");
            var ui = FindObjectOfType<HealthBarManager>();
            if (ui != null && health != null)
            {
                ui.Init(health);
            }
        }
    }
}

