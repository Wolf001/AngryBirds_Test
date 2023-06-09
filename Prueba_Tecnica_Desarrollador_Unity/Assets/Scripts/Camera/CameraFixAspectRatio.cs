﻿using UnityEngine;
public class CameraFixAspectRatio : MonoBehaviour
{
    //se ajusta el aspecto de la camara
    void Start()
    {
        Camera camera = GetComponent<Camera>();
        float aspect = Mathf.Round(camera.aspect * 100f) / 100f;

        if (aspect == 0.6f)
            camera.orthographicSize = 5;
        else if (aspect == 0.56f) //720p
            camera.orthographicSize = 4.6f;
    }
}
