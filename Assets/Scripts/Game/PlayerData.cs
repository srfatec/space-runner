using UnityEngine;
using System.Collections;

public class PlayerData : MonoBehaviour {

    public float m_MaxSpeed = 100.0f;
    public float m_MinSpeed = 40.0f;
    public float m_Acceleration = 20.0f;
    public float m_Deceleration = 40.0f;
    public float m_NormalDeceleration = 20.0f;
    public float m_Angle = 20.0f;
    public float m_AttackSpeed = 0.6f;
    public float m_Energy = 10.0f;
    public float m_EnergyRecoveryRate = 1.0f;
    public float m_EnergyRecoveryDelay = 2.0f;
    public float m_SpinningDelay = 0.6f;
    public float m_LoopDelay = 1.4f;
    public float m_DamageDelay = 2.0f;
    public GameObject m_Missile;
    public GameObject m_RadarCamera;
    public Transform m_SpawnPoint1;
    public ParticleSystem[] m_Turbine;
}
