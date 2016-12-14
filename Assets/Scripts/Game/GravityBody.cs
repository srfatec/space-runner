using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour
{
    public float m_GravityMultiplier = 1.0f;
    private GravityAttractor centerSphere;
    private Rigidbody rigidBody;
    private Transform t;

    void Awake()
    {
        centerSphere = GameObject.FindGameObjectWithTag("CenterSphere").GetComponent<GravityAttractor>();
        rigidBody = GetComponent<Rigidbody>();
        t = GetComponent<Transform>();
        
        rigidBody.useGravity = false;
        rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void FixedUpdate()
    {        
        centerSphere.Attract(rigidBody, t, m_GravityMultiplier);
    }
}