using UnityEngine;
using System.Collections;

public class MissileController : MonoBehaviour {

    public float m_Speed = 500.0f;
    public float m_TimeOut = 5.0f;
    public float m_Damage = 1.0f;
    public float m_ImpactForce = 10.0f;
    public GameObject m_CreateBeforeDestroy = null;
    protected int m_IdNumber;
    protected PowerUp m_PowerUp = null;
    private float timer;
    private Transform m_Transform;
    private Rigidbody m_Rigidbody;
    protected GameObject m_BeforeDestroyObject;

    protected virtual void Awake()
    {
        timer = 0.0f;
        m_Transform = GetComponent<Transform>();
        m_Rigidbody = GetComponent<Rigidbody>();        
    }

    protected virtual void Update()
    {        
        timer += Time.deltaTime;
        if(timer >= m_TimeOut)
            this.Destroy();
    }

    protected virtual void FixedUpdate()
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
                OnCollidePlayer(Player);            
        }
        else if (col.transform.tag == "Missile")
        {
            MissileController missileController = col.transform.GetComponent<MissileController>();
            if (missileController.getIdNumber() != m_IdNumber)
                OnCollideMissile();                      
        }

    }

    protected virtual void OnCollidePlayer(PlayerController Player)
    {
        Player.Hit(m_Damage, m_ImpactForce, this.gameObject);
    }

    protected virtual void OnCollideMissile()
    {
        this.Destroy();
    }

    public void InvertMovement(int id = 0)
    {       
        m_Transform.Rotate(m_Transform.up, 180.0f, Space.World);
        if (id != 0)
            m_IdNumber = id;
    }

    public void Destroy() {
        this.BeforeDestroy();
        Destroy(this.gameObject);
    }

    public virtual void BeforeDestroy()
    {
        if (m_CreateBeforeDestroy == null)
        {
            return;
        }
        m_BeforeDestroyObject = (GameObject)Instantiate(m_CreateBeforeDestroy, m_Transform.position, m_Transform.rotation);
    }

    public virtual void Trigger() { }

    public void setIdNumber(int id)
    {
        m_IdNumber = id;
    }

    public int getIdNumber()
    {
        return m_IdNumber;
    }

    public void setPowerUp(PowerUp powerUp)
    {
        m_PowerUp = powerUp;
    }
}
