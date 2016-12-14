using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour {

    public float minimum = 0.0f;
    public float maximum = 1f;
    public float duration = 5.0f;
    private float startTime;
    private SpriteRenderer sprite;

    void Start()
    {
        startTime = Time.time;
        sprite = GetComponent<SpriteRenderer>();
    }
	
	void Update () {
        float deltaTime = Time.time - startTime;
        float t = deltaTime / duration;
        sprite.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(minimum, maximum, t));

        if (deltaTime >= 10.0f)
        {
            SceneManager.LoadScene("Main_Menu", LoadSceneMode.Single);
        }
    }
}
