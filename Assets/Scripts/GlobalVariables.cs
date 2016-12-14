using UnityEngine;
using System.Collections;

public class GlobalVariables : MonoBehaviour {

    public int m_NumberOfPlayers = 4;
    public int[] m_PlayersActive;

    void Awake () {
        m_PlayersActive = new int[4];
        m_PlayersActive[0] = -1;
        m_PlayersActive[1] = -1;
        m_PlayersActive[2] = -1;
        m_PlayersActive[3] = -1;
        DontDestroyOnLoad(gameObject);
	}
}
