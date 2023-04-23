﻿using UnityEngine;
using Assets.Scripts;
using System;

public class SlingShot : MonoBehaviour
{
    #region Variables
    //vector que medira la media entre las partes derecha e izquierda
    private Vector3 SlingshotMiddleVector;

    [HideInInspector]
    public SlingshotState slingshotState;
    //parte derecha e izquierda del charpe
    public Transform LeftSlingshotOrigin, RightSlingshotOrigin;
    //resorte del charpe
    public LineRenderer SlingshotLineRenderer1;
    public LineRenderer SlingshotLineRenderer2;
    //Dibuja la trayectoria del ave
    public LineRenderer TrajectoryLineRenderer;
    //ave a lanzar
    [HideInInspector]
    public GameObject BirdToThrow;
    //posicion del ave en el charpe
    public Transform BirdWaitPosition;
    //velocidad de disparo
    public float ThrowSpeed;
    //tiempo despues del disparo
    [HideInInspector]
    public float TimeSinceThrown;
    #endregion
    #region Main Methods
    void Start()
    {
        SlingshotLineRenderer1.sortingLayerName = "Foreground";
        SlingshotLineRenderer2.sortingLayerName = "Foreground";
        TrajectoryLineRenderer.sortingLayerName = "Foreground";

        slingshotState = SlingshotState.Idle;
        SlingshotLineRenderer1.SetPosition(0, LeftSlingshotOrigin.position);
        SlingshotLineRenderer2.SetPosition(0, RightSlingshotOrigin.position);

        SlingshotMiddleVector = new Vector3((LeftSlingshotOrigin.position.x + RightSlingshotOrigin.position.x) / 2,
            (LeftSlingshotOrigin.position.y + RightSlingshotOrigin.position.y) / 2, 0);
    }

    void Update()
    {
        switch (slingshotState)
        {
            case SlingshotState.Idle:
                InitializeBird();
                DisplaySlingshotLineRenderers();
                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 location = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    if (BirdToThrow.GetComponent<CircleCollider2D>() == Physics2D.OverlapPoint(location))
                    {
                        slingshotState = SlingshotState.UserPulling;
                    }
                }
                break;
            //calcula la distancia entre el ave y el punto medio del charpe
            case SlingshotState.UserPulling:
                DisplaySlingshotLineRenderers();

                if (Input.GetMouseButton(0))
                {
                    Vector3 location = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    location.z = 0;
                    if (Vector3.Distance(location, SlingshotMiddleVector) > 1.5f)
                    {
                        var maxPosition = (location - SlingshotMiddleVector).normalized * 1.5f + SlingshotMiddleVector;
                        BirdToThrow.transform.position = maxPosition;
                    }
                    else
                    {
                        BirdToThrow.transform.position = location;
                    }
                    float distance = Vector3.Distance(SlingshotMiddleVector, BirdToThrow.transform.position);
                    //muestra la trayectoria del disparo
                    MostrarTrayectoria(distance);
                }
                else
                {
                    Set_TrajectoryLineRenderesActive(false);
                    TimeSinceThrown = Time.time;
                    float distance = Vector3.Distance(SlingshotMiddleVector, BirdToThrow.transform.position);
                    if (distance > 1)
                    {
                        SetSlingshot_LineRenderersActive(false);
                        slingshotState = SlingshotState.BirdFlying;
                        TirarPajaro(distance);
                    }
                    else
                    {
                        BirdToThrow.transform.positionTo(distance / 10,
                            BirdWaitPosition.transform.position).
                            setOnCompleteHandler((x) =>
                            {
                                x.complete();
                                x.destroy();
                                InitializeBird();
                            });
                    }
                }
                break;
            case SlingshotState.BirdFlying:
                break;
            default:
                break;
        }
    }
    #endregion
    #region methods
    
    private void TirarPajaro(float distance)
    {
        Vector3 velocity = SlingshotMiddleVector - BirdToThrow.transform.position;
        BirdToThrow.GetComponent<Bird>().AlDispararPajaro();
        BirdToThrow.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x, velocity.y) * ThrowSpeed * distance;
        if (BirdThrown != null)
            BirdThrown(this, EventArgs.Empty);
    }

    public event EventHandler BirdThrown;

    private void InitializeBird()
    {
        BirdToThrow.transform.position = BirdWaitPosition.position;
        slingshotState = SlingshotState.Idle;
        SetSlingshot_LineRenderersActive(true);
    }

    void DisplaySlingshotLineRenderers()
    {
        SlingshotLineRenderer1.SetPosition(1, BirdToThrow.transform.position);
        SlingshotLineRenderer2.SetPosition(1, BirdToThrow.transform.position);
    }

    void SetSlingshot_LineRenderersActive(bool active)
    {
        SlingshotLineRenderer1.enabled = active;
        SlingshotLineRenderer2.enabled = active;
    }

    void Set_TrajectoryLineRenderesActive(bool active)
    {
        TrajectoryLineRenderer.enabled = active;
    }
    //Calcula la trayectoria de previsualizacion del disparo
    void MostrarTrayectoria(float distance)
    {
        Set_TrajectoryLineRenderesActive(true);
        Vector3 v2 = SlingshotMiddleVector - BirdToThrow.transform.position;
        int segmentCount = 15;
        float segmentScale = 2;
        Vector2[] segments = new Vector2[segmentCount];

        segments[0] = BirdToThrow.transform.position;
        Vector2 segVelocity = new Vector2(v2.x, v2.y) * ThrowSpeed * distance;

        float angle = Vector2.Angle(segVelocity, new Vector2(1, 0));
        float time = segmentScale / segVelocity.magnitude;
        for (int i = 1; i < segmentCount; i++)
        {
            float time2 = i * Time.fixedDeltaTime * 5;
            segments[i] = segments[0] + segVelocity * time2 + 0.5f * Physics2D.gravity * Mathf.Pow(time2, 2);
        }

        TrajectoryLineRenderer.positionCount = segments.Length;
        for (int i = 0; i < segmentCount; i++)
            TrajectoryLineRenderer.SetPosition(i, segments[i]);
    }
    #endregion
}