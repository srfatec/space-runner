using UnityEngine;
using System.Collections;

public class MissileController : MonoBehaviour {

    public float m_Speed = 500.0f;
    public float m_TimeOut = 5.0f;
    public float m_Damage = 1.0f;
    public float m_ImpactForce = 10.0f;
    private int m_IdNumber;
    private float timer;
    private Transform m_Transform;
    private Rigidbody m_Rigidbody;

    private void Awake()
    {
        timer = 0.0f;
        m_Transform = GetComponent<Transform>();
        m_Rigidbody = GetComponent<Rigidbody>();        
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer >= m_TimeOut)
            Destroy(this.gameObject);
    }

    void FixedUpdate()
    {
        Vector3 m_Movement = m_Transform.forward * m_Speed * Time.deltaTime;
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Player")
        {
            PlayerController Player = col.gameObject.GetComponentInParent<PlayerController>();
            if (Player.getIdNumber() != m_IdNumber)
            {
                Player.Hit(m_Damage, m_ImpactForce, this.gameObject);
            }
        }
        else if (col.transform.tag == "Missile")
        {            
            Destroy(col.transform.gameObject);
            Destroy(this.gameObject);
        }

    }

    public void InvertMovement(int id = 0)
    {       
        m_Transform.Rotate(m_Transform.up, 180.0f, Space.World);
        if (id != 0)
            m_IdNumber = id;
    }

    public void setIdNumber(int id)
    {
        m_IdNumber = id;
    }

    public int getIdNumber()
    {
        return m_IdNumber;
    }
}
