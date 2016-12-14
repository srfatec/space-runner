using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class MenuController : MonoBehaviour {

    public GameObject m_MainMenu;
    public GameObject m_SelectionMenu;
    public GameObject[] m_PlayersIcons;
    private bool m_State = false;
    private bool[] m_PlayersActive;
    private int[] m_Characters;
    private GlobalVariables m_GlobalVariables;

    void Awake()
    {
        m_GlobalVariables = GameObject.FindGameObjectWithTag("GlobalVariables").GetComponent<GlobalVariables>();
        m_PlayersActive = new bool[4];
        m_Characters = new int[4];        
    }

	public void play()
    {
        m_SelectionMenu.SetActive(true);
        m_MainMenu.SetActive(false);
        m_State = true;
        m_Characters[0] = 0;
        m_Characters[1] = 0;
        m_Characters[2] = 0;
        m_Characters[3] = 0;
        m_PlayersActive[0] = false;
        m_PlayersActive[1] = false;
        m_PlayersActive[2] = false;
        m_PlayersActive[3] = false;
    }

    void Update()
    {
        if (m_State)
        {            
            if (Input.GetButtonDown("Escape"))
            {
                m_SelectionMenu.SetActive(false);
                m_MainMenu.SetActive(true);
                m_State = false;
            }
            PlayerSelectionHandler(1);
            PlayerSelectionHandler(2);
            PlayerSelectionHandler(3);
            PlayerSelectionHandler(4);
            if (Input.GetButtonDown("Confirm"))
            {
                m_GlobalVariables.m_NumberOfPlayers = 0;
                foreach (bool active in m_PlayersActive)
                {
                    if (active)
                        m_GlobalVariables.m_NumberOfPlayers += 1;
                }
                if (m_GlobalVariables.m_NumberOfPlayers > 1)
                {
                    if(m_PlayersActive[0])
                        m_GlobalVariables.m_PlayersActive[0] = CheckCharacter(1)+1;
                    if (m_PlayersActive[1])
                        m_GlobalVariables.m_PlayersActive[1] = CheckCharacter(2)+1;
                    if (m_PlayersActive[2])
                        m_GlobalVariables.m_PlayersActive[2] = CheckCharacter(3)+1;
                    if (m_PlayersActive[3])
                        m_GlobalVariables.m_PlayersActive[3] = CheckCharacter(4)+1;
                    SceneManager.LoadScene("Game", LoadSceneMode.Single);
                }                
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1Joy1") || Input.GetButtonDown("Fire1Joy2") || Input.GetButtonDown("Fire1Joy3") || Input.GetButtonDown("Fire1Joy4"))
                play();
        }
    }

    void PlayerSelectionHandler(int number)
    {
        int n = number - 1;
        if (Input.GetButtonDown("R1Joy" + number.ToString()))
        {
            if (m_PlayersActive[n])
            {
                int currentCharacter = CheckCharacter(number);
                int character = CheckCharacter(0, currentCharacter);
                if (character != -1)
                {
                    m_PlayersIcons[n].transform.Find("P1Model").gameObject.SetActive(false);
                    m_PlayersIcons[n].transform.Find("P2Model").gameObject.SetActive(false);
                    m_PlayersIcons[n].transform.Find("P3Model").gameObject.SetActive(false);
                    m_PlayersIcons[n].transform.Find("P4Model").gameObject.SetActive(false);                    
                    m_PlayersIcons[n].transform.Find("P" + (character+1) + "Model").gameObject.SetActive(true);
                                        
                    m_Characters[character] = number;
                    m_Characters[currentCharacter] = 0;                    
                }
                
            }
        }
        else if (Input.GetButtonDown("L1Joy" + number.ToString()))
        {
            if (m_PlayersActive[n])
            {
                int currentCharacter = CheckCharacter(number);
                int character = CheckCharacter(0, currentCharacter, true);
                if (character != -1)
                {
                    m_PlayersIcons[n].transform.Find("P1Model").gameObject.SetActive(false);
                    m_PlayersIcons[n].transform.Find("P2Model").gameObject.SetActive(false);
                    m_PlayersIcons[n].transform.Find("P3Model").gameObject.SetActive(false);
                    m_PlayersIcons[n].transform.Find("P4Model").gameObject.SetActive(false);
                    m_PlayersIcons[n].transform.Find("P" + (character + 1) + "Model").gameObject.SetActive(true);

                    m_Characters[character] = number;
                    m_Characters[currentCharacter] = 0;
                }

            }
        }

        if (Input.GetButtonDown("Fire1Joy"+number.ToString()))
        {
            if (m_PlayersActive[n])
            {
                m_PlayersActive[n] = false;
                int character = CheckCharacter(number);
                m_Characters[character] = 0;
                m_PlayersIcons[n].GetComponent<UnityEngine.UI.Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.19f);
                m_PlayersIcons[n].transform.Find("P1Model").gameObject.SetActive(false);
                m_PlayersIcons[n].transform.Find("P2Model").gameObject.SetActive(false);
                m_PlayersIcons[n].transform.Find("P3Model").gameObject.SetActive(false);
                m_PlayersIcons[n].transform.Find("P4Model").gameObject.SetActive(false);
            }
            else
            {
                m_PlayersActive[n] = true;
                m_PlayersIcons[n].GetComponent<UnityEngine.UI.Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.9f);
                m_PlayersIcons[n].transform.Find("SelectionEffect").GetComponent<ParticleSystem>().Play();
                int character = CheckCharacter(0);
                m_Characters[character] = number;
                m_PlayersIcons[n].transform.Find("P"+(character+1)+"Model").gameObject.SetActive(true);
            }
        }
    }

    private int CheckCharacter(int n, int from = 0, bool inverse = false)
    {
        int character = -1;
        if (inverse)
        {
            for (int i = from; i >= 0; i--)
            {
                if (m_Characters[i] == n)
                {
                    character = i;
                    break;
                }
            }
        }
        else
        {
            for (int i = from; i < m_Characters.Length; i++)
            {
                if (m_Characters[i] == n)
                {
                    character = i;
                    break;
                }
            }
        }
        
        return character;
    }
}
