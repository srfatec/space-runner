using UnityEngine;
using System.Collections;

public class GlobalVariables : MonoBehaviour {

    public int m_NumberOfPlayers = 4;

	void Awake () {
        DontDestroyOnLoad(gameObject);
	}
}
