using UnityEngine;
using System.Collections;

public class FreezingMissileController : MissileController {

    public float m_SlowOnImpactValue;
    private GameObject m_TargetPlayer = null;
    private float m_Timer = 0.0f;    

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
    }

    public override void Trigger()
    {
        this.Destroy();        
    }

    protected override void OnCollidePlayer(PlayerController Player)
    {        
        base.OnCollidePlayer(Player);
        DestroyPowerUp();
        Player.ApplyStatusEffect("Slow", m_SlowOnImpactValue, 100.0f);
    }

    protected override void OnCollideMissile()
    {
        base.OnCollideMissile();
    }

    public override void BeforeDestroy()
    {
        DestroyPowerUp();
        base.BeforeDestroy();
        this.m_BeforeDestroyObject.GetComponent<AreaEffect>().setIdNumber(this.m_IdNumber);        
    }

    private void DestroyPowerUp()
    {
        if (m_PowerUp != null)
        {
            this.m_PowerUp.Destroy();
            m_PowerUp = null;
        }        
    }
}
