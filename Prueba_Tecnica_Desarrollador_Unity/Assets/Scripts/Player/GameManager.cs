using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Variables
    public CameraFollow cameraFollow;
    int currentBirdIndex;
    public SlingShot slingshot;
    [HideInInspector]
    public static GameState CurrentGameState = GameState.Start;
    private List<GameObject> Bricks;
    private List<GameObject> Birds;
    private List<GameObject> Pigs;
    public int indexLvl;
    //[HideInInspector]
    [SerializeField]private GameObject fScore;
    [SerializeField] private GameObject uiWin;
    [SerializeField] private GameObject uiLost;
    [SerializeField] private GameObject tapStart;
    [SerializeField] private GameObject ScoreBar;
    [SerializeField] private GameObject finishClip;
    #endregion
    #region Main methods
    void Start()
    {
        CurrentGameState = GameState.Start;
        slingshot.enabled = false;
        //encontramos y asignamos la lista de elementos clave del juego como son aves, estructuras y puercos
        Bricks = new List<GameObject>(GameObject.FindGameObjectsWithTag("Brick"));
        Birds = new List<GameObject>(GameObject.FindGameObjectsWithTag("Bird"));
        Pigs = new List<GameObject>(GameObject.FindGameObjectsWithTag("Pig"));        
        //desasignamos el lanzamiento 
        slingshot.BirdThrown -= Slingshot_BirdThrown; 
        //reasignamos lanzamiento
        slingshot.BirdThrown += Slingshot_BirdThrown;
    }

    void Update()
    {
        switch (CurrentGameState)
        {
            //si es player hace tap sobre pantalla entramos a el game state start
            case GameState.Start:
                if (Input.GetMouseButtonUp(0)) 
                {
                    AnimateBirdToSlingshot();
                    tapStart.SetActive(false);
                }                                      
                break;
            case GameState.BirdMovingToSlingshot:
                break;
            //si hemos lanzado el ave ya sea que no exista actividad o hayan pasado 5 segundos desde que lanzamos el ave camara regresa a posicion de inicio
            case GameState.Playing:
                if (slingshot.slingshotState == SlingshotState.BirdFlying &&
                    (BricksBirdsPigsStoppedMoving() || Time.time - slingshot.TimeSinceThrown > 5f))
                {
                    slingshot.enabled = false;
                    AnimateCamera_ToStartPosition();
                    CurrentGameState = GameState.BirdMovingToSlingshot;
                }
                break;
            //caso si hemos ganado o perdido, en ambos se carga la escena index 0
            case GameState.Won:
                ScoreBar.SetActive(false);
                uiWin.SetActive(true);
                finishClip.SetActive(true);
                break;
            case GameState.Lost:
                uiLost.SetActive(true);
                ScoreBar.SetActive(false);
                finishClip.SetActive(true);
                break;
            default:
                break;
        }
    }
    #endregion
    #region Methods
    //se verifica si los cerdos han sido destruidos o no
    private bool TodosLosCerdosDestruidos()
    {
        return Pigs.All(x => x == null);
    }
    //
    private void AnimateCamera_ToStartPosition()
    {
        float duration = Vector2.Distance(Camera.main.transform.position, cameraFollow.StartingPosition) / 10f;
        if (duration == 0.0f) duration = 0.1f;
        //anima la camara al inicio
        Camera.main.transform.positionTo
            (duration,
            cameraFollow.StartingPosition). //posicion final
            setOnCompleteHandler((x) =>
            {
                cameraFollow.IsFollowing = false;
                if (TodosLosCerdosDestruidos())
                    //condicion de ganar si aun hay aves y no cerdos
                    CurrentGameState = GameState.Won;
                //anima la siguiente ave si aun no han sido destruidos los cerdos
                else if (currentBirdIndex == Birds.Count - 1)
                    //condicion de perdida si no hay aves y aun hay cerds
                    CurrentGameState = GameState.Lost;
                else
                {
                    slingshot.slingshotState = SlingshotState.Idle;
                    //prepara el siguiente ave para ser disparada
                    currentBirdIndex++;
                    AnimateBirdToSlingshot();
                }
            });
    }
    //este metodo toma la siguiente ave en lista para ser disparada y ubica en posicion correcta
    void AnimateBirdToSlingshot()
    {
        CurrentGameState = GameState.BirdMovingToSlingshot;
        Birds[currentBirdIndex].transform.positionTo
            (Vector2.Distance(Birds[currentBirdIndex].transform.position / 10,
            slingshot.BirdWaitPosition.transform.position) / 10, 
            slingshot.BirdWaitPosition.transform.position).
                setOnCompleteHandler((x) =>
                {
                    x.complete();
                    x.destroy();
                    CurrentGameState = GameState.Playing;
                    slingshot.enabled = true;
                    slingshot.BirdToThrow = Birds[currentBirdIndex];
                });
    }
    //permite a la camara hacer seguimiento del ave lanzada
    private void Slingshot_BirdThrown(object sender, System.EventArgs e)
    {
        cameraFollow.BirdToFollow = Birds[currentBirdIndex].transform;
        cameraFollow.IsFollowing = true;
    }
    //verifica si los elementos clave del juego se han dejado de mover (aven, puercos, estructuras)
    bool BricksBirdsPigsStoppedMoving()
    {
        foreach (var item in Bricks.Union(Birds).Union(Pigs))
        {
            if (item != null && item.GetComponent<Rigidbody2D>().velocity.sqrMagnitude > Constants.Min_Velocity)
            {
                return false;
            }
        }

        return true;
    }

    public static void AutoResize(int screenWidth, int screenHeight)
    {
        Vector2 resizeRatio = new Vector2((float)Screen.width / screenWidth, (float)Screen.height / screenHeight);
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(resizeRatio.x, resizeRatio.y, 1.0f));
    }
    
    public void Reset()
    {
        SceneManager.LoadScene(indexLvl);
    }
    #endregion
}
