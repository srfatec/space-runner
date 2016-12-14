using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public GameObject centerSphere;
    public GameObject PlayerObject;
    private GameObject[] m_Players;
    private GameObject Player1;
    private GameObject Player2;
    private GameObject Player3;
    private GameObject Player4;
    public GameObject PowerUpItem;
    public GameObject m_WayPoint;
    public Canvas m_Canvas;    
    public float centerSphereSize = 1000.0f;
    public int m_WayPointQuantity = 1;
    private int m_WayPointActualQuantity = 0;
    public GlobalVariables m_GlobalVariables;
    private string m_GameState = "Start";
    private float m_FinalCountDown = 0.0f;    
    
    void Awake () {
        m_GlobalVariables = GameObject.FindGameObjectWithTag("GlobalVariables").GetComponent<GlobalVariables>();        

        centerSphere = (GameObject)Instantiate(centerSphere, Vector3.zero,Quaternion.identity);
        centerSphere.GetComponent<CenterSphereController>().setSize(centerSphereSize);

        m_Players = new GameObject[m_GlobalVariables.m_NumberOfPlayers];
        int count = 0;              
        for (int i = 0; i < 4; i++)
        {
            if(m_GlobalVariables.m_PlayersActive[i] != -1)
            {
                m_Players[count] = (GameObject)Instantiate(PlayerObject, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
                m_Players[count].transform.position = LongitudeLatitudeToVector3(count * 4, 0);
                m_Players[count].transform.Find("Camera").gameObject.GetComponent<Camera>().rect = GetCameraRectSize(count+1);
                setCanvasSize(count + 1, i+1);
                m_Players[count].GetComponent<PlayerController>().setIdNumber(i + 1);
                m_Players[count].GetComponent<PlayerController>().setCharacterNumber(m_GlobalVariables.m_PlayersActive[i]);
                m_Players[count].GetComponent<PlayerController>().setScreenPosition(count + 1);
                count++;
            }            
        }

        int pus = 0;
        while (pus < 16)
        {
            CreateRandomPowerUp();
            pus += 1;
        }

        CreateRandomWayPoint();
        m_GameState = "Running";
    }

    void Update()
    {
        switch (m_GameState)
        {
            case "Start":
                break;
            case "Running":
                break;
            case "Final":
                FinalRoutine();
                break;
            default:
                break;
        }
    }

    Rect GetCameraRectSize(int current)
    {
        Rect rect = new Rect();

        if (m_GlobalVariables.m_NumberOfPlayers == 2)
        {
            if (current == 1)
                rect = new Rect(0.0f, 0.5f, 1.0f, 0.5f);
            else if (current == 2)
                rect = new Rect(0.0f, 0.0f, 1.0f, 0.5f);
        }
        else
        {
            if (current == 1)
                rect = new Rect(0.0f, 0.5f, 0.5f, 0.5f);
            else if (current == 2)
                rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
            else if (current == 3)
                rect = new Rect(0.0f, 0.0f, 0.5f, 0.5f);
            else if (current == 4)
                rect = new Rect(0.5f, 0.0f, 0.5f, 0.5f);
        }
        return rect;
    }

    void setCanvasSize(int current, int id)
    {        
        GameObject Panel = m_Canvas.transform.Find("PanelP" + id).gameObject;
        Panel.SetActive(true);
        if (m_GlobalVariables.m_NumberOfPlayers == 2)
        {
            m_Canvas.transform.Find("Border2").gameObject.SetActive(true);
            if (current == 1)
            {
                Panel.GetComponent<RectTransform>().anchorMin = new Vector2(0.0f, 0.5f);
                Panel.GetComponent<RectTransform>().anchorMax = new Vector2(1.0f, 1.0f);
            }
            else if (current == 2)
            {
                Panel.GetComponent<RectTransform>().anchorMin = new Vector2(0.0f, 0.0f);
                Panel.GetComponent<RectTransform>().anchorMax = new Vector2(1.0f, 0.5f);
            }
        }
        else
        {
            m_Canvas.transform.Find("Border").gameObject.SetActive(true);
        }
    }

    void FinalRoutine()
    {
        m_FinalCountDown += Time.deltaTime;
        if (m_FinalCountDown >= 10.0f)
        {
            SceneManager.LoadScene("Main_Menu", LoadSceneMode.Single);
        }       
    }

    public void PlayerPassedThroughWaypoint(PlayerController Player,WayPointController Waypoint)
    {
        Waypoint.PlayerPassed();
        if (Player.getWayPoints().Count == m_WayPointQuantity && m_GameState == "Running")
        {
            m_GameState = "Final";
            foreach (GameObject player in m_Players)
            {
                player.GetComponent<PlayerController>().EndMatch();
            }
        }
        else
        {
            if (Waypoint.getPlayerCount() == 1 && m_WayPointActualQuantity < m_WayPointQuantity)
                CreateRandomWayPoint();

            if (Waypoint.getPlayerCount() >= m_GlobalVariables.m_NumberOfPlayers)
                Destroy(Waypoint.gameObject);
        }                        
    }

    private void CreateRandomWayPoint()
    {
        bool loop = true;
        while (loop)
        {
            int RandomLongitude = Random.Range(0, 359) - 180;
            int RandomLatitude = Random.Range(26, 154) - 90;
            if(CheckWayPointsProximity(LongitudeLatitudeToVector3(RandomLongitude, RandomLatitude), centerSphereSize / 2.5f))
            {
                CreateWayPoint(RandomLongitude, RandomLatitude);
                loop = false;
            }

        }        
    }

    private bool CheckWayPointsProximity(Vector3 NewWayPoint, float MinDistance)
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("WayPoint"))
        {
            WayPointController wc = obj.GetComponent<WayPointController>();
            float Distance = Vector3.Distance(NewWayPoint, wc.transform.position);
            if(Distance < MinDistance)
                return false;
        }

        return true;
    }

    private void CreateWayPoint(int longitude, int latitude)
    {
        GameObject waypoint = (GameObject)Instantiate(m_WayPoint, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        waypoint.transform.position = LongitudeLatitudeToVector3(longitude, latitude);
        waypoint.transform.rotation = Quaternion.LookRotation(waypoint.transform.position - Vector3.zero);
        m_WayPointActualQuantity += 1;
        waypoint.GetComponent<WayPointController>().setIdNumber(m_WayPointActualQuantity);
        waypoint.GetComponent<WayPointController>().setLongitude(longitude);
        waypoint.GetComponent<WayPointController>().setLatitude(latitude);
    }

    private void CreateRandomPowerUp()
    {
        bool loop = true;
        while (loop)
        {
            int RandomLongitude = Random.Range(0, 359) - 180;
            int RandomLatitude = Random.Range(26, 154) - 90;
            if (CheckPowerUpsProximity(LongitudeLatitudeToVector3(RandomLongitude, RandomLatitude), centerSphereSize / 4.0f))
            {
                CreatePowerUp(RandomLongitude, RandomLatitude);
                loop = false;
            }

        }
    }

    private bool CheckPowerUpsProximity(Vector3 NewPowerUp, float MinDistance)
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("PowerUp"))
        {
            PowerUp wc = obj.GetComponent<PowerUp>();
            float Distance = Vector3.Distance(NewPowerUp, wc.transform.position);
            if (Distance < MinDistance)
                return false;
        }

        return true;
    }

    private void CreatePowerUp(int longitude, int latitude)
    {
        GameObject PowerUp = (GameObject)Instantiate(PowerUpItem, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        PowerUp.transform.position = LongitudeLatitudeToVector3(longitude, latitude);
        PowerUp.transform.rotation = Quaternion.LookRotation(PowerUp.transform.position - Vector3.zero);
    }

    public Vector3 LongitudeLatitudeToVector3(int longitude, int latitude)
    {
        //Longitude: -180 até 180 (positivo = leste, negativo = oeste)
        //Latitude: -90 até 90 (positivo = norte, negativo = sul)
        Vector3 position = new Vector3();
        float radius = centerSphereSize * 0.535f + 5.0f;
        position = Quaternion.AngleAxis(longitude, -Vector3.up) * Quaternion.AngleAxis(latitude, -Vector3.right) * new Vector3(0, 0, 1);   
        position = position * radius;

        return position;
    }

    public string getGameState()
    {
        return m_GameState;
    }
}
