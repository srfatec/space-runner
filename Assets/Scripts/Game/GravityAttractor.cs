using UnityEngine;
using System.Collections;

public class GravityAttractor : MonoBehaviour
{
    public float gravity = -9.8f;

    public void Attract(Rigidbody body, Transform bodyTransform, float multiplier)
    {
        Vector3 gravityUp = (bodyTransform.position - transform.position).normalized;
        Vector3 localUp = bodyTransform.up;

        // Apply downwards gravity to body
        body.AddForce(gravityUp * gravity * multiplier);
        // Allign bodies up axis with the centre of planet
        bodyTransform.rotation = Quaternion.FromToRotation(localUp, gravityUp) * body.rotation;
    }
}