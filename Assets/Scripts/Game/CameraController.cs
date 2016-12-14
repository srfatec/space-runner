using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    private Vector3 m_DefaultPosition;
    private Quaternion m_DefaultRotation;
    private Vector3 m_SavedPosition;
    private Quaternion m_SavedRotation;
    private Vector3 m_GoingToInicialPosition;
    private Vector3 m_GoingToPosition;
    private float m_GoingToTime;
    private float m_GoingToInicialTime;
    private Quaternion m_RotatingToInicialPosition;
    private Quaternion m_RotatingToPosition;
    private float m_RotatingToTime;
    private float m_RotatingToInicialTime;

    void Start()
    {
        m_GoingToPosition = transform.position;
        m_GoingToTime = 0.0f;
        m_GoingToInicialTime = 0.0f;
    }

    void FixedUpdate()
    {
        if (m_GoingToTime != 0.0f)
        {
            float step = (Time.time - m_GoingToInicialTime) / m_GoingToTime;
            transform.localPosition = Vector3.Lerp(m_GoingToInicialPosition, m_GoingToPosition, step);
            if (step >= 1.0f)
            {
                m_GoingToTime = 0.0f;
            }
        }

        if (m_RotatingToTime != 0.0f)
        {
            float step = (Time.time - m_RotatingToInicialTime) / m_RotatingToTime;
            transform.localRotation = Quaternion.Lerp(m_RotatingToInicialPosition, m_RotatingToPosition, step);
            if (step >= 1.0f)
            {
                m_RotatingToTime = 0.0f;
            }
        }
    }

    public void GoTo(Vector3 position, float time)
    {
        if (m_GoingToTime == 0.0f)
        {
            m_GoingToInicialPosition = transform.localPosition;
            m_GoingToPosition = position;
            m_GoingToTime = time;
            m_GoingToInicialTime = Time.time;
        }        
    }

    public void RotateTo(Quaternion rotation, float time)
    {
        if (m_RotatingToTime == 0.0f)
        {
            m_RotatingToInicialPosition = transform.localRotation;
            m_RotatingToPosition = rotation;
            m_RotatingToTime = time;
            m_RotatingToInicialTime = Time.time;
        }
    }

    public void SetDefaultPosition(Vector3 position)
    {
        m_DefaultPosition = position;
    }

    public Vector3 GetDefaultPosition()
    {
        return m_DefaultPosition;
    }

    public void SetDefaultRotation(Quaternion rotation)
    {
        m_DefaultRotation = rotation;
    }

    public Quaternion GetDefaultRotation()
    {
        return m_DefaultRotation;
    }

    public void SetSavedPosition(Vector3 position)
    {
        m_SavedPosition = position;
    }

    public Vector3 GetSavedPosition()
    {
        return m_SavedPosition;
    }

    public void SetSavedRotation(Quaternion rotation)
    {
        m_SavedRotation = rotation;
    }

    public Quaternion GetSavedRotation()
    {
        return m_SavedRotation;
    }
}

