using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    void Update()
    {
        if (IsFollowing)
            if (BirdToFollow != null)
            {
                var birdPosition = BirdToFollow.transform.position;
                float x = Mathf.Clamp(birdPosition.x, minCameraX, maxCameraX);
                transform.position = new Vector3(x, StartingPosition.y, StartingPosition.z);
            }
            else
                IsFollowing = false;
    }
    
    void Start()
    {
        StartingPosition = transform.position;
    }

    [HideInInspector]
    public Vector3 StartingPosition;

    private const float minCameraX = 0;
    private const float maxCameraX = 13;
    [HideInInspector]
    public bool IsFollowing;
    [HideInInspector]
    public Transform BirdToFollow;
}
