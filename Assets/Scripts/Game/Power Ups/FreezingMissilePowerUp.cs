using UnityEngine;
using System.Collections;

public class FreezingMissilePowerUp : PowerUp {
        
    public GameObject m_Projectile;    
    public Sprite m_Icon2;
    public Sprite m_Icon3;
    private bool m_HasFired;
    private float m_Timer;
    private PlayerController m_PlayerController;

    void Update()
    {
        if (m_HasFired)
        {
            FreezingMissileController m_FreezingMissileController = m_Projectile.GetComponent<FreezingMissileController>();
            m_Timer += Time.deltaTime;
            if (m_Timer >= m_FreezingMissileController.m_TimeOut)
            {
                Destroy();
            }
        }
        
    }

    public override void PickUp()
    {
        this.m_Parent.PickUp();
        m_HasFired = false;
        m_Timer = 0.0f;
    }

    public override void Fire(Transform player)
    {
        this.m_Parent.Fire(player);
        m_PlayerController = player.GetComponent<PlayerController>();
        if (!m_HasFired)
        {            
            m_PlayerController.Fire(m_Projectile, player.GetComponent<PlayerController>().getPowerUpSpawnPoint(), true);
            this.m_Icon = m_Icon3;
            m_PlayerController.SetPowerUpIcon();
            m_HasFired = true;
        }
        else
        {
            m_PlayerController.TriggerMissile();
            Destroy();
        }        
    }

    public override void Destroy()
    {                
        m_PlayerController.emptyPowerUpSlot();
        m_HasFired = false;
        base.Destroy();
    }

    public override void setIcon()
    {
        this.m_Icon = m_Icon2;
    }

}
