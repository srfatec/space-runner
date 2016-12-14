using UnityEngine;
using System.Collections;

public class CenterSphereController : MonoBehaviour {
    
    public void setSize(float radiusScale)
    {
        transform.localScale = new Vector3(radiusScale, radiusScale, radiusScale);
    }

}
