using UnityEngine;
using Fusion;

public class LocalHealthTester : NetworkBehaviour
{
    private HealthBarManager ui;

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            ui = FindObjectOfType<HealthBarManager>();
        }
    }

    private void Update()
    {
        if (Object.HasInputAuthority && Input.GetKeyDown(KeyCode.P))
        {
            if (ui != null)
            {
                ui.DecreaseLocalHealth(10f);
                Debug.Log("Đã trừ 10 máu (UI Local)");
            }
        }
    }
}

