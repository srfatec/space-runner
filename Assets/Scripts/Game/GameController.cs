using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public GameObject centerSphere;
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;
    public GameObject pu;
    public GameObject m_WayPoint;
    public Canvas m_Canvas;    
    public float centerSphereSize = 1000.0f;
    public int m_WayPointQuantity = 1;
    private int m_WayPointActualQuantity = 0;
    private GlobalVariables m_GlobalVariables;
    private string m_GameState = "Start";
    private float m_FinalCountDown = 0.0f;    
    
    void Awake () {
        m_GlobalVariables = GameObject.FindGameObjectWithTag("GlobalVariables").GetComponent<GlobalVariables>();        

        centerSphere = (GameObject)Instantiate(centerSphere, Vector3.zero,Quaternion.identity);
        centerSphere.GetComponent<CenterSphereController>().setSize(centerSphereSize);     
                   
        player1 = (GameObject)Instantiate(player1, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        player1.transform.position = LongitudeLatitudeToVector3(0, 0);
        player1.transform.Find("Camera").gameObject.GetComponent<Camera>().rect = new Rect(0.0f, 0.5f, 0.5f, 0.5f);
        player1.GetComponent<PlayerController>().setIdNumber(1);        

        player2 = (GameObject)Instantiate(player2, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        player2.transform.position = LongitudeLatitudeToVector3(4, 0);
        player2.transform.Find("Camera").gameObject.GetComponent<Camera>().rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
        player2.GetComponent<PlayerController>().setIdNumber(2);
        
        player3 = (GameObject)Instantiate(player3, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        player3.transform.position = LongitudeLatitudeToVector3(8, 0);
        player3.transform.Find("Camera").gameObject.GetComponent<Camera>().rect = new Rect(0.0f, 0.0f, 0.5f, 0.5f);
        player3.GetComponent<PlayerController>().setIdNumber(3);

        player4 = (GameObject)Instantiate(player4, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        player4.transform.position = LongitudeLatitudeToVector3(12, 0);
        player4.transform.Find("Camera").gameObject.GetComponent<Camera>().rect = new Rect(0.5f, 0.0f, 0.5f, 0.5f);
        player4.GetComponent<PlayerController>().setIdNumber(4);
        
        int pus = 0;
        while (pus < 16)
        {
            CreateRandomPowerUp();
            pus += 1;
        }
        
        CreateWayPoint(Random.Range(0, 359) - 180, Random.Range(26, 154) - 90);

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
            if (player1.GetComponent<PlayerController>().getIdNumber() == 1)
                player1.GetComponent<PlayerController>().EndMatch();
            if (player2.GetComponent<PlayerController>().getIdNumber() == 2)
                player2.GetComponent<PlayerController>().EndMatch();
            if (player3.GetComponent<PlayerController>().getIdNumber() == 3)
                player3.GetComponent<PlayerController>().EndMatch();
            if (player4.GetComponent<PlayerController>().getIdNumber() == 4)
                player4.GetComponent<PlayerController>().EndMatch();
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
        GameObject PowerUp = (GameObject)Instantiate(pu, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        PowerUp.transform.position = LongitudeLatitudeToVector3(longitude, latitude);
        PowerUp.transform.rotation = Quaternion.LookRotation(PowerUp.transform.position - Vector3.zero);
    }

    public Vector3 LongitudeLatitudeToVector3(int longitude, int latitude)
    {
        //Longitude: -180 até 180 (positivo = leste, negativo = oeste)
        //Latitude: -90 até 90 (positivo = norte, negativo = sul)
        Vector3 position = new Vector3();
        float radius = centerSphereSize / 2 + 5.0f;
        position = Quaternion.AngleAxis(longitude, -Vector3.up) * Quaternion.AngleAxis(latitude, -Vector3.right) * new Vector3(0, 0, 1);   
        position = position * radius;

        return position;
    }

    public string getGameState()
    {
        return m_GameState;
    }
}
