using UnityEngine;
using System.Collections;

public class BigMissileAreaEffect : AreaEffect
{
    private SphereCollider m_SphereCollider;
    public float m_Damage;
    public float m_ImpactForce;

    public override void Awake()
    {
        base.Awake();
        m_SphereCollider = transform.GetComponent<SphereCollider>();
    }

    public override void Update()
    {
        base.Update();
    }

    void OnTriggerEnter(Collider col)
    {
        if (this.m_Timer >= m_EffectDuration)
            return;

        if (col.transform.tag == "Player")
        {
            PlayerController Player = col.gameObject.GetComponentInParent<PlayerController>();
            if (Player.getIdNumber() != m_IdNumber)
            {
                Player.Hit(m_Damage, m_ImpactForce, transform.position);
            }
        }
    }
}

