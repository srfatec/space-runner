using UnityEngine;
using System.Collections;

public class PUTurbo : PowerUp
{
    public GameObject m_Projectile;
    public Sprite m_Icon2;

    public override void PickUp()
    {
        this.m_Parent.PickUp();

    }

    public override void Fire(Transform player)
    {
        this.m_Parent.Fire(player);

        PlayerController m_PlayerController = player.GetComponent<PlayerController>();
        m_PlayerController.Fire(m_Projectile, player.GetComponent<PlayerController>().getPowerUpSpawnPoint());
        m_PlayerController.emptyPowerUpSlot();
        this.Destroy();
    }

    public override void Destroy()
    {
        base.Destroy();
    }

    public override void setIcon()
    {
        this.m_Icon = m_Icon2;
    }
}
