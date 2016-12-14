using UnityEngine;
using System.Collections;

public class PUMissile : PowerUp {

    public override void PickUp()
    {
        base.PickUp();
        
    }

    public override void Fire(Transform player)
    {
        base.Fire(player);

        PlayerController m_PlayerController = player.GetComponent<PlayerController>();
        m_PlayerController.Fire(this.m_Projectile, player.GetComponent<PlayerController>().getPowerUpSpawnPoint());
        m_PlayerController.emptyPowerUpSlot();
        Destroy(this.gameObject);
    }
}
