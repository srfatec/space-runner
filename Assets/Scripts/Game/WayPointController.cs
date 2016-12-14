using UnityEngine;
using System.Collections;

public class WayPointController : MonoBehaviour {

    private int m_IdNumber = 0;
    private int m_PlayerCount = 0;
    private int m_Longitude;
    private int m_Latitude;

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Player")
        {
            PlayerController Player = col.gameObject.GetComponentInParent<PlayerController>();
            Player.PassedWayPoint(this);
            transform.Find("CursorP" + Player.getIdNumber()).GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 1f);
        }
    }

    public void PlayerPassed()
    {
        m_PlayerCount += 1;
    }

    public void setLongitude(int longitude)
    {
        m_Longitude = longitude;
    }

    public int getLongitude()
    {
        return m_Longitude;
    }

    public void setLatitude(int latitude)
    {
        m_Latitude = latitude;
    }

    public int getLatitude()
    {
        return m_Latitude;
    }

    public void setIdNumber(int IdNumber)
    {
        m_IdNumber = IdNumber;
    }

    public int getIdNumber()
    {
        return m_IdNumber;
    }

    public int getPlayerCount()
    {
        return m_PlayerCount;
    }

}
