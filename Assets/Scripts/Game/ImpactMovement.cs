using UnityEngine;
using System.Collections;

public class ImpactMovement {

    private Vector3 m_Force;
    private float m_DampCoefficient;
    private float m_Time = 1.0f;
    private float m_Timer = 0.0f;

    public ImpactMovement(Vector3 Force, float ImpactTime)
    {
        m_Time = ImpactTime;
        m_Force = Force / ImpactTime;
    }

    public Vector3 CalculateMovement()
    {
        m_Timer += Time.deltaTime;
        m_DampCoefficient = Mathf.Lerp(0.0f, 2.0f, 1.0f - m_Timer/m_Time);
        return m_Force * Time.deltaTime * m_DampCoefficient;
    }

    public bool CheckIfFinished()
    {
        return m_Timer >= m_Time;         
    }
}
