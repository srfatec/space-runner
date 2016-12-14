using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour {

    public GameObject m_Projectile;
    public Sprite m_Icon;    
    private Transform m_Transform;
    private Rigidbody m_Rigidbody;
    private MeshRenderer m_MeshRenderer;
    private GravityBody m_GravityBody;
    private AudioSource m_AudioSource;

    private void Awake()
    {
        m_Transform = GetComponent<Transform>();
        m_MeshRenderer = m_Transform.Find("Mesh").GetComponent<MeshRenderer>();
        m_GravityBody = GetComponent<GravityBody>();
        m_AudioSource = GetComponent<AudioSource>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.useGravity = false;
        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public virtual void PickUp () {
        //O que acontece quando o power up é pego
        //Envia o Power Up para o centro e retira todas as colisões o mesh, mantendo apenas o script
        transform.position = Vector3.zero;
        m_MeshRenderer.enabled = false;
        m_AudioSource.PlayOneShot(m_AudioSource.clip,1.0f);
        Destroy(m_GravityBody);
        Destroy(m_Rigidbody);
    }

    public virtual void Fire(Transform player)
    {
        //O que acontece quando o power up eh usado
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Player")
        {
            PlayerController Player = col.gameObject.GetComponentInParent<PlayerController>();
            Player.PowerUpCollision(this);
        }
    }
}
