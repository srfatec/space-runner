using UnityEngine;
using System.Collections;

public class FreezingMystAreaEffect : AreaEffect {

    public float m_InicialSlowValue;
    public float m_SlowPerSecondValue;
    public float m_MaxSlowValue;
    public float m_MinRadius;
    public float m_MaxRadius;
    private float m_TimeForMaximumRadius = 4.5f;
    private SphereCollider m_SphereCollider;

    public override void Awake () {
        base.Awake();
        m_SphereCollider = transform.GetComponent<SphereCollider>();
    }

    public override void Update()
    {
        float t = this.m_Timer / m_TimeForMaximumRadius;
        m_SphereCollider.radius = Mathf.Lerp(m_MinRadius, m_MaxRadius, t);
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
                Player.ApplyStatusEffect("Slow", m_InicialSlowValue, m_MaxSlowValue);
            }
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (this.m_Timer >= m_EffectDuration)
            return;

        if (col.transform.tag == "Player")
        {
            PlayerController Player = col.gameObject.GetComponentInParent<PlayerController>();
            if (Player.getIdNumber() != m_IdNumber)
            {
                Player.ApplyStatusEffect("Slow", m_SlowPerSecondValue * Time.deltaTime, m_MaxSlowValue);                
            }
        }
    }
}
