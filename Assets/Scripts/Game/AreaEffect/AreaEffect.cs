using UnityEngine;
using System.Collections;

public class AreaEffect : MonoBehaviour {

    protected GameObject m_GameController;
    public float m_EffectDuration;
    public float m_AnimationDuration;   
    protected float m_Timer;
    protected int m_IdNumber;

    public virtual void Awake()
    {
        m_Timer = 0.0f;
        m_GameController = GameObject.FindGameObjectWithTag("GameController");
        Vector3 pointingVector = m_GameController.GetComponent<GameController>().centerSphere.transform.position - transform.position;
        transform.rotation = Quaternion.FromToRotation(Vector3.up, pointingVector);
    }

    public virtual void Update()
    {
        m_Timer += Time.deltaTime;
        if (m_Timer >= m_AnimationDuration)
            Destroy(this.gameObject);
    }    

    public void setIdNumber(int number)
    {
        m_IdNumber = number;
    }
}
