using UnityEngine;
using System.Collections;

public class RotateModel : MonoBehaviour {    

	void Update () {
        transform.Rotate(new Vector3(0.0f, 20.0f * Time.deltaTime, 0.0f));
    }
}
