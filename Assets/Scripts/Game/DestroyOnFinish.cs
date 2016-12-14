using UnityEngine;
using System.Collections;

public class DestroyOnFinish : MonoBehaviour {

    private ParticleSystem m_ParticleSystem;

	void Start () {
        m_ParticleSystem = transform.GetComponent<ParticleSystem>();
    }
	
	void Update () {
        if (m_ParticleSystem)
            if (!m_ParticleSystem.IsAlive())
                Destroy(this.gameObject);                   
    }
}
