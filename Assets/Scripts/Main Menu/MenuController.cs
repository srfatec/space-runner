using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	public void play()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
}
