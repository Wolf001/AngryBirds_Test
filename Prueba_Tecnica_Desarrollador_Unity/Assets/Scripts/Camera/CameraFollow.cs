using UnityEngine;
public class CameraFollow : MonoBehaviour
{
    #region
    [HideInInspector]
    public Vector3 StartingPosition;
    //posicion minima y maxima de la camara respecto al eje X
    private const float minCameraX = 0;
    private const float maxCameraX = 13;
    [HideInInspector]
    public bool IsFollowing;
    [HideInInspector]
    public Transform BirdToFollow;
    #endregion
    #region
    //posicion inicial de la camara
    void Start()
    {
        StartingPosition = transform.position;
    }
    //seguimiento del ave manteniendolo en su objetivo de la vista
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
    #endregion    
}
