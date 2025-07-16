using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attached to MainCamera
public class CameraController : MonoBehaviour
{
    public Transform player; // These field holds player transform to follow it
    public float threshold = 2.0f; // Distance in Y before camera moves
    public float smoothSpeed = 1.0f; // Higher = faster smoothing

    private float yOffset; // Initial offset between camera & player

    void Start()
    {
        yOffset = transform.position.y - player.position.y;
    }

    void LateUpdate()
    {
        float cameraY = transform.position.y;
        float playerY = player.position.y + yOffset;

        // Only move camera if player has crossed the treshold.
        if (Mathf.Abs(cameraY - playerY) > threshold)
        {
            // Target position with offset
            float targetY = playerY;

            // Smooth damp or Lerp towards the target
            float newY = Mathf.Lerp(cameraY, targetY, Time.deltaTime * smoothSpeed);

            // Apply new Y while keeping X & Z the same
            transform.position = new(transform.position.x, newY, transform.position.z);
        }
    }
}
