using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    private int m_IdNumber = 0;    
    private float m_MaxSpeed;
    private float m_MinSpeed;
    private float m_Acceleration;
    private float m_Deceleration;
    private float m_NormalDeceleration;
    private float m_Angle;
    private float m_AttackSpeed;
    private float m_Energy;
    private float m_EnergyRecoveryRate;
    private float m_EnergyRecoveryDelay = 0.0f;
    private float m_AttackSpeedTimer = 0.0f;
    private float m_ActualSpeed = 0.0f;
    private float m_SpinningTimer = 0.0f;
    private float m_LoopTimer = 0.0f;
    private float m_DamagedTimer = 0.0f;
    private string m_State = "Default";
    private List<int> m_WayPoints = new List<int>();
    private CameraController m_Camera;
    private CameraController m_RadarCamera;
    private GameObject m_Missile;
    private Transform m_SpawnPoint1;
    private GameObject m_PlayerMesh;
    private GameObject m_PlayerCursor;
    private GameObject m_PlayerWayPointArrow;
    private PowerUp m_PowerUpSlot;
    private Transform m_CanvasPanel;
    private Transform m_CanvasEnergyBar;
    private Transform m_CanvasEnergyBarDamaged;
    private Transform m_CanvasCentralMessage;
    private Transform m_CanvasPowerUpIcon;
    private Rigidbody m_Rigidbody;
    private Transform m_Transform;    
    private GravityAttractor m_CenterSphere;
    private GameController m_GameController;
    private PlayerData m_PlayerData;
    private float m_DistanceFromSurface = 0.0f;
    private float m_TurnAngle = 0.0f;
    private List<ImpactMovement> m_ImpactMovementList = new List<ImpactMovement>();

    private void Awake()
    {
        m_Transform = GetComponent<Transform>();        
        m_Rigidbody = GetComponent<Rigidbody>();
        m_CenterSphere = GameObject.FindGameObjectWithTag("CenterSphere").GetComponent<GravityAttractor>();
        m_GameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        m_Camera = m_Transform.Find("Camera").gameObject.GetComponent<CameraController>();
        m_RadarCamera = m_Transform.Find("RadarCamera").gameObject.GetComponent<CameraController>();

        m_Rigidbody.useGravity = false;
        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        m_PowerUpSlot = null;
    }

    private void Start()
    {
        m_Camera.SetDefaultPosition(m_Camera.transform.localPosition);
        m_Camera.SetDefaultRotation(m_Camera.transform.localRotation);
        m_RadarCamera.SetDefaultPosition(m_RadarCamera.transform.localPosition);
        m_RadarCamera.SetDefaultRotation(m_RadarCamera.transform.localRotation);

        if (m_IdNumber == 1)
            m_RadarCamera.GetComponent<Camera>().rect = new Rect(0.397f, 0.514f, 0.1f, 0.2f);
        else if (m_IdNumber == 2)
            m_RadarCamera.GetComponent<Camera>().rect = new Rect(0.9f, 0.514f, 0.1f, 0.2f);
        else if (m_IdNumber == 3)
            m_RadarCamera.GetComponent<Camera>().rect = new Rect(0.397f, 0.0f, 0.1f, 0.2f);
        else if (m_IdNumber == 4)
            m_RadarCamera.GetComponent<Camera>().rect = new Rect(0.9f, 0.0f, 0.1f, 0.2f);
        m_RadarCamera.GetComponent<Camera>().cullingMask = (1 << LayerMask.NameToLayer("Radar")) | (1 << LayerMask.NameToLayer("Radar" + m_IdNumber));
        m_RadarCamera.transform.Find("radarFormato").gameObject.layer = LayerMask.NameToLayer("Radar" + m_IdNumber);

        string PlayerName = "Prefabs/Player" + m_IdNumber;
        m_PlayerMesh = (GameObject)Instantiate(Resources.Load(PlayerName), m_Transform.position, m_Transform.rotation);        
        m_PlayerData = m_PlayerMesh.GetComponent<PlayerData>();
        m_PlayerCursor = m_Transform.Find("Cursor").gameObject;
        m_PlayerWayPointArrow = m_Transform.Find("WayPointArrow").gameObject;
        m_PlayerWayPointArrow.layer = LayerMask.NameToLayer("Radar" + m_IdNumber);
        m_PlayerWayPointArrow.SetActive(false);

        m_MaxSpeed = m_PlayerData.m_MaxSpeed;
        m_MinSpeed = m_PlayerData.m_MinSpeed;
        m_Acceleration = m_PlayerData.m_Acceleration;
        m_Deceleration = m_PlayerData.m_Deceleration;
        m_NormalDeceleration = m_PlayerData.m_NormalDeceleration;
        m_Angle = m_PlayerData.m_Angle;
        m_AttackSpeed = m_PlayerData.m_AttackSpeed;
        m_Energy = m_PlayerData.m_Energy;
        m_EnergyRecoveryRate = m_PlayerData.m_EnergyRecoveryRate;
        m_Missile = m_PlayerData.m_Missile;
        m_SpawnPoint1 = m_PlayerData.m_SpawnPoint1;

        m_CanvasPanel = m_GameController.m_Canvas.transform.Find("PanelP" + m_IdNumber);
        m_CanvasPowerUpIcon = m_CanvasPanel.Find("PowerUp/Icon");        
        m_CanvasPowerUpIcon.gameObject.SetActive(false);
        m_CanvasEnergyBar = m_CanvasPanel.Find("EnergyBar");
        m_CanvasEnergyBarDamaged = m_CanvasPanel.Find("Damaged");
        m_CanvasEnergyBarDamaged.gameObject.SetActive(false);
        m_CanvasCentralMessage = m_CanvasPanel.Find("CentralMessage");
        m_CanvasCentralMessage.gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        ImpactMovementRoutine();
        switch (m_State)
        {
            case "Default":
                m_CenterSphere.Attract(m_Rigidbody, m_Transform, 1.0f);
                VerticalInputHandler();
                HorizontalInputHandler();
                PrimaryFireInputHandler();
                SecondaryFireInputHandler();
                SpinningInputHandler();
                LoopInputHandler();
                EnergyRecoveryRoutine();
                m_PlayerMesh.transform.position = m_Transform.position;        
                m_PlayerMesh.transform.rotation = m_Transform.rotation;
                m_PlayerMesh.transform.Rotate(m_PlayerMesh.transform.forward, 180.0f, Space.World);
                m_PlayerMesh.transform.Rotate(m_PlayerMesh.transform.forward, m_TurnAngle, Space.World);
                break;
		case "Spinning":
                m_CenterSphere.Attract(m_Rigidbody, m_Transform, 1.0f);
                VerticalInputHandler();
                HorizontalInputHandler();
                m_PlayerMesh.transform.position = m_Transform.position;
                m_PlayerMesh.transform.rotation = m_Transform.rotation;
                m_PlayerMesh.transform.Rotate(m_PlayerMesh.transform.forward, 180.0f, Space.World);
                SpinningRoutine();                
                break;
            case "Loop":                                
                LoopRoutine();
                m_PlayerMesh.transform.position = m_Transform.position;
                m_PlayerMesh.transform.rotation = m_Transform.rotation;
                m_PlayerMesh.transform.Rotate(m_PlayerMesh.transform.forward, 180.0f, Space.World);
                break;
            case "Damaged":
                m_CenterSphere.Attract(m_Rigidbody, m_Transform, 1.0f);
                m_PlayerMesh.transform.position = m_Transform.position;
                m_PlayerMesh.transform.rotation = m_Transform.rotation;
                m_PlayerMesh.transform.Rotate(m_PlayerMesh.transform.forward, 180.0f, Space.World);
                DamagedRoutine();
                break;
            default:
                break;
        }
        EnergyBarRoutine();
        //RadarArrowRoutine();
    }

    void EnergyBarRoutine()
    {
        RectTransform rectTransform = m_CanvasEnergyBar.GetComponent<UnityEngine.RectTransform>();
        float CurrentWidth = rectTransform.sizeDelta.x;
        float CurrentBarEnergy = CurrentWidth * m_PlayerData.m_Energy / 200;
        float t = CurrentBarEnergy / m_PlayerData.m_Energy;
        float Increment = 0.01f;
        if (CurrentBarEnergy > m_Energy)
        {
            t -= Increment;
            if (Mathf.Lerp(0.0f, m_PlayerData.m_Energy, t) < m_Energy)          
                t = m_Energy / m_PlayerData.m_Energy;            
        }
        else if (CurrentBarEnergy < m_Energy)
        {
            t += Increment;
            if (Mathf.Lerp(0.0f, m_PlayerData.m_Energy, t) > m_Energy)
                t = m_Energy / m_PlayerData.m_Energy;
        }            
        
        rectTransform.sizeDelta = new Vector2(Mathf.Lerp(0.0f, 200.0f, t), rectTransform.sizeDelta.y);
    }

    void EnergyRecoveryRoutine()
    {
        if (m_EnergyRecoveryDelay <= 0.0f)
        {
            m_CanvasEnergyBarDamaged.gameObject.SetActive(false);
            m_Energy += m_EnergyRecoveryRate * Time.deltaTime;
            if (m_Energy > m_PlayerData.m_Energy)
                m_Energy = m_PlayerData.m_Energy;
        }
        else        
            m_CanvasEnergyBarDamaged.gameObject.SetActive(true);
        
        m_EnergyRecoveryDelay -= Time.deltaTime;
    }

    void VerticalInputHandler()
    {
        if (Input.GetAxisRaw("VerticalJoy" + m_IdNumber.ToString()) > 0)
            m_ActualSpeed += m_Acceleration * Time.deltaTime;
        else if (Input.GetAxisRaw("VerticalJoy" + m_IdNumber.ToString()) < 0)
            m_ActualSpeed -= m_Deceleration * Time.deltaTime;
            if (m_ActualSpeed < m_MinSpeed)            
                m_ActualSpeed = m_MinSpeed;            
        else
        {            
            if (m_ActualSpeed < m_MinSpeed)
            {                
                m_ActualSpeed += m_Acceleration * Time.deltaTime;
                if (m_ActualSpeed > m_MinSpeed)
                    m_ActualSpeed = m_MinSpeed;
            }
            else if (m_ActualSpeed > m_MinSpeed)
            {
                m_ActualSpeed -= m_NormalDeceleration * Time.deltaTime;
            }
        }
            
        if (m_ActualSpeed >= m_MaxSpeed)
            m_ActualSpeed = m_MaxSpeed;
        else if (m_ActualSpeed <= 0.0f)
            m_ActualSpeed = 0.0f;

        Vector3 m_Movement = m_Transform.forward * m_ActualSpeed * Time.deltaTime;
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement);
    }

    void HorizontalInputHandler()
    {
        float maxTurnAngle = 45.0f;
        float angle = m_Angle * Time.deltaTime;

        if (Input.GetAxisRaw("HorizontalJoy" + m_IdNumber.ToString()) != 0)
        {
            m_TurnAngle += Input.GetAxisRaw("HorizontalJoy" + m_IdNumber.ToString()) * angle * -1;
            if (m_TurnAngle >= maxTurnAngle)
                m_TurnAngle = maxTurnAngle;
            else if (m_TurnAngle <= -maxTurnAngle)
                m_TurnAngle = -maxTurnAngle;
        }
        else if (m_TurnAngle > 0.0f)
        {
            m_TurnAngle += m_Angle * Time.deltaTime * -1;
            if (m_TurnAngle < 0.0f)
                m_TurnAngle = 0.0f;
        }
        else if (m_TurnAngle < 0.0f)
        {
            m_TurnAngle -= m_Angle * Time.deltaTime * -1;            
            if (m_TurnAngle > 0.0f)
                m_TurnAngle = 0.0f;
        }

        m_Transform.Rotate(m_Transform.up, angle * m_TurnAngle / maxTurnAngle * -1, Space.World);
    }

    public void Fire(GameObject missile, Transform spawnPoint)
    {
        GameObject FiredMissile = (GameObject)Instantiate(missile, m_SpawnPoint1.position, m_Transform.rotation);
        MissileController missileController = FiredMissile.GetComponent<MissileController>();
        missileController.m_Speed += m_ActualSpeed;
        missileController.setIdNumber(m_IdNumber);
        m_Energy -= missileController.m_Damage / 10;
        if (m_Energy <= 0.0f)        
            m_Energy = 0.0f;        
    }

    void PrimaryFireInputHandler()
    {
        m_AttackSpeedTimer += Time.deltaTime;

        //if (Input.GetButton("Fire1") && m_AttackSpeedTimer >= m_AttackSpeed && m_Energy > 0.0f)
        if (Input.GetButton("Fire1Joy" + m_IdNumber.ToString()) && m_AttackSpeedTimer >= m_AttackSpeed)
        {
            Fire(m_Missile, m_SpawnPoint1);
            m_AttackSpeedTimer = 0.0f;
        }
    }

    void SecondaryFireInputHandler()
    {
        //if (Input.GetButton("Fire2") && m_PowerUpSlot != null)
        if (Input.GetButton("Fire2Joy" + m_IdNumber.ToString()) && m_PowerUpSlot != null)
        {            
            m_PowerUpSlot.Fire(m_Transform);
        }        
    }

    void SpinningInputHandler()
    {
        m_SpinningTimer -= Time.deltaTime;
        if (Input.GetButton("SpinningJoy" + m_IdNumber.ToString()) && m_SpinningTimer <= 0)
        {
            m_State = "Spinning";
            m_SpinningTimer = 0;
        }
    }

    void SpinningRoutine()
    {
        m_SpinningTimer += Time.deltaTime;
        if (m_SpinningTimer >= 0.4f)
        {
            m_State = "Default";
            m_SpinningTimer = m_PlayerData.m_SpinningDelay;
        }
        else
        {
            m_PlayerMesh.transform.Rotate(m_PlayerMesh.transform.forward, m_SpinningTimer * 1800, Space.World);            
        }            
    }

    void LoopInputHandler()
    {
        m_LoopTimer -= Time.deltaTime;
        if (Input.GetButton("LoopJoy" + m_IdNumber.ToString()) && m_LoopTimer <= 0)
        {
            m_State = "Loop";
            m_TurnAngle = 0.0f;
            m_LoopTimer = 0;            
            m_Camera.SetSavedPosition(m_Camera.transform.position);
            m_Camera.SetSavedRotation(m_Camera.transform.rotation);
            m_RadarCamera.SetSavedPosition(m_RadarCamera.transform.position);
            m_RadarCamera.SetSavedRotation(m_RadarCamera.transform.rotation);            
        }
    }

    void LoopRoutine()
    {
        float rotationSpeed = 180.0f;        
        float timeLimit = 180 / rotationSpeed;
        Vector3 antiGravityVector = (m_Transform.position - m_CenterSphere.transform.position).normalized;
        Vector3 pointOnSurface = m_CenterSphere.transform.position + (antiGravityVector * m_CenterSphere.transform.localScale.z/2);

        m_Rigidbody.detectCollisions = false;
        m_LoopTimer += Time.deltaTime;
        Vector3 m_Movement = m_Transform.forward * m_ActualSpeed * Time.deltaTime;
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement);
        if (m_LoopTimer >= timeLimit * 2 + 0.3f)
        {
            m_State = "Default";
            m_LoopTimer = m_PlayerData.m_LoopDelay;
            m_Rigidbody.detectCollisions = true;
        }
        else if (m_LoopTimer >= timeLimit * 2)
        {
            //Corrige o alinhamento com a superfície da esfera
            Quaternion fromToRotation = Quaternion.FromToRotation(m_Transform.up, antiGravityVector) * m_Transform.rotation;
            m_Transform.rotation = Quaternion.RotateTowards(transform.rotation, fromToRotation, 50.0f * Time.deltaTime);

            m_Camera.GoTo(m_Camera.GetDefaultPosition(), 0.8f);
            m_Camera.RotateTo(m_Camera.GetDefaultRotation(), 0.8f);
            m_RadarCamera.RotateTo(m_RadarCamera.GetDefaultRotation(), 0.8f);
        }
        else if (m_LoopTimer >= timeLimit)
        {
            m_Transform.Rotate(m_Transform.forward, Time.deltaTime * rotationSpeed, Space.World);
            m_Rigidbody.MovePosition(m_Rigidbody.position - (antiGravityVector * m_DistanceFromSurface * Time.deltaTime) / timeLimit);

            m_Camera.transform.position = m_Camera.GetSavedPosition() + m_Movement;
            m_Camera.transform.rotation = m_Camera.GetSavedRotation();
            m_Camera.transform.Rotate(m_Camera.transform.forward, Time.deltaTime * rotationSpeed, Space.World);
            m_RadarCamera.GoTo(m_RadarCamera.GetDefaultPosition(), timeLimit);
            m_RadarCamera.transform.rotation = m_RadarCamera.GetSavedRotation();            
        }
        else
        { 
            m_ActualSpeed = m_MaxSpeed;
            m_Transform.Rotate(-m_Transform.right, Time.deltaTime * rotationSpeed, Space.World);
            m_DistanceFromSurface = Vector3.Distance(m_Transform.position, pointOnSurface) + 9.0f;

            m_Camera.transform.position = m_Camera.GetSavedPosition();                      
            m_Camera.transform.rotation = m_Camera.GetSavedRotation();
            m_RadarCamera.transform.position = m_RadarCamera.GetSavedPosition();
            m_RadarCamera.transform.rotation = m_RadarCamera.GetSavedRotation();
        }

        m_Camera.SetSavedPosition(m_Camera.transform.position);
        m_Camera.SetSavedRotation(m_Camera.transform.rotation);         
    }

    void DamagedRoutine()
    {
        m_DamagedTimer += Time.deltaTime;
        if (m_DamagedTimer >= m_PlayerData.m_DamageDelay)
        {
            m_State = "Default";
            m_Energy += m_PlayerData.m_Energy / 2;
            m_DamagedTimer = 0.0f;
        }
        else
        {
            m_PlayerMesh.transform.Rotate(m_PlayerMesh.transform.up, m_DamagedTimer * 600, Space.World);            
        }            
    }

    public void Hit(float damage, float impactForce, GameObject missile)
    {
        Vector3 force = (m_Transform.position - missile.transform.position).normalized * impactForce;
        float impactMovimentTime = 0.5f;
        switch (m_State)
        {
            case "Default":
                m_Energy -= damage;
                m_EnergyRecoveryDelay = m_PlayerData.m_EnergyRecoveryDelay;
                if (m_Energy <= 0.0f)
                {
                    m_State = "Damaged";
                    m_Energy = 0.0f;
                }
                m_ImpactMovementList.Add(new ImpactMovement(force, impactMovimentTime));                                                
                Destroy(missile);
                break;
            case "Spinning":
                missile.GetComponent<MissileController>().InvertMovement(m_IdNumber);
                break;
            case "Loop":
                break;
            case "Damaged":
                //m_DamagedTimer -= damage / 20;
                m_ImpactMovementList.Add(new ImpactMovement(force, impactMovimentTime));
                Destroy(missile);
                break;
            default:
                break;
        }        
    }

    //Faz a animação da movimentação após o impacto com algum Missile
    private void ImpactMovementRoutine()
    {
        List<ImpactMovement> List = new List<ImpactMovement> (m_ImpactMovementList);
        int i = 0;
        foreach (ImpactMovement movement in List)
        {
            Vector3 m_Movement = movement.CalculateMovement();
            m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement);
            if (movement.CheckIfFinished())
                m_ImpactMovementList.RemoveAt(i);
            i++;
        }        
    }

    public void PowerUpCollision(PowerUp powerUp)
    {
        m_PowerUpSlot = powerUp;
        m_CanvasPowerUpIcon.GetComponent<UnityEngine.UI.Image>().sprite = powerUp.m_Icon;
        m_CanvasPowerUpIcon.gameObject.SetActive(true);
        m_PowerUpSlot.PickUp();
    }

    public void PassedWayPoint(WayPointController Waypoint)
    {
        int IdNumber = Waypoint.getIdNumber();

        if (!m_WayPoints.Contains(IdNumber) && m_GameController.getGameState() != "Final")
        {
            m_WayPoints.Add(IdNumber);
            m_CanvasPanel.Find("Waypoints").GetComponent<UnityEngine.UI.Text>().text = "Waypoints: " + m_WayPoints.Count;
            m_GameController.PlayerPassedThroughWaypoint(this, Waypoint);
        }
    }

    void RadarArrowRoutine()
    {
        GameObject WayPoint = GetShortestDistanceWayPoint();
        
        if(WayPoint != null)
        {
            Vector3 PointingVector = WayPoint.transform.position - m_PlayerWayPointArrow.transform.position;
            m_PlayerWayPointArrow.transform.LookAt(WayPoint.transform);
            //m_PlayerWayPointArrow.transform.rotation = Quaternion.FromToRotation(m_Transform.up, m_PlayerWayPointArrow.transform.forward);
        }
        else
            m_PlayerWayPointArrow.SetActive(false);

    }

    public GameObject GetShortestDistanceWayPoint()
    {
        GameObject WayPoint = null;
        float ShortestDistance = Mathf.Infinity;

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("WayPoint"))
        {
            WayPointController waypointController = obj.GetComponent<WayPointController>();
            if (m_WayPoints.Contains(waypointController.getIdNumber()))
                continue;
            else
            {
                float Distance = Vector3.Distance(m_Transform.position, obj.transform.position);
                if (Distance < ShortestDistance)
                {
                    ShortestDistance = Distance;
                    WayPoint = obj;
                }
            }
        }

        return WayPoint;
    }

    public void EndMatch()
    {
        if (m_WayPoints.Count == m_GameController.m_WayPointQuantity)
        {
            m_CanvasCentralMessage.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Sprites/YouWin");
        }
        else
        {
            m_CanvasCentralMessage.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Sprites/YouLose");
        }

        m_CanvasCentralMessage.gameObject.SetActive(true);
    }

    public void setIdNumber(int id)
    {
        m_IdNumber = id;
    }

    public int getIdNumber()
    {
        return m_IdNumber;
    }

    public void emptyPowerUpSlot()
    {
        m_PowerUpSlot = null;
        m_CanvasPowerUpIcon.gameObject.SetActive(false);
    }
    
    public Transform getPowerUpSpawnPoint()
    {
        return m_SpawnPoint1;
    }

    public List<int> getWayPoints()
    {
        return m_WayPoints;
    }
}
