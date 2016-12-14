using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuidedMissileController : MissileController {
    private GameObject m_TargetPlayer = null;
    private float m_Timer = 0.0f;
    public float m_MinimumDistance;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update () {
        base.Update();                                        
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        GameObject ClosestPlayer = GetClosestPlayer();
        float distance = Vector3.Distance(transform.position, ClosestPlayer.transform.position);

        if (m_Timer >= 1.0f)
        {            
            if (distance < m_MinimumDistance)
            {
                m_TargetPlayer = ClosestPlayer;
                m_Timer = 0.0f;
            }
        }
        if (m_TargetPlayer != null)
        {            
            Vector3 direction = m_TargetPlayer.transform.position - transform.position;            
            Quaternion lookRotation = Quaternion.LookRotation(direction, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 20.0f);  
        }
        m_Timer += Time.deltaTime;
    }

    protected GameObject GetClosestPlayer()
    {
        GameObject player = null;
        float ShortestDistance = Mathf.Infinity;

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            PlayerController playerController = obj.GetComponent<PlayerController>();
            if (m_IdNumber == playerController.getIdNumber())
                continue;
            else
            {
                float Distance = Vector3.Distance(transform.position, obj.transform.position);
                if (Distance < ShortestDistance)
                {
                    ShortestDistance = Distance;
                    player = obj;
                }
            }
        }

        return player;
    }

    public override void Trigger()
    {
        base.Trigger();
    }

    public override void BeforeDestroy()
    {
        base.BeforeDestroy();
    }
}
