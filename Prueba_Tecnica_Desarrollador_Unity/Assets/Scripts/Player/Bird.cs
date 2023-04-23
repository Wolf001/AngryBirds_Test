using UnityEngine;
using System.Collections;
using Assets.Scripts;

[RequireComponent(typeof(Rigidbody2D))]
public class Bird : MonoBehaviour
{
    #region Bird State
    public BirdState State { get; private set; }
    public GameObject holderHabilitie;
    #endregion
    #region main & fixedUpdate Methods
    void Start()
    {
        //trail renderer se mantiene oculto hasta que el gamestate cambie a throw
        GetComponent<TrailRenderer>().enabled = false;
        GetComponent<TrailRenderer>().sortingLayerName = "Foreground";
        //mantenemos el rigidbody sin gravedad en modo de espera
        GetComponent<Rigidbody2D>().isKinematic = true;
        //aumentamos el collider2D para poder tener contacto con el
        GetComponent<CircleCollider2D>().radius = Constants.Bird_Collider_Radius_Big;
        State = BirdState.BeforeThrown;
    }
    void FixedUpdate()
    {
        //condicion si se ha lanzado el ave a poca velocidad 
        if (State == BirdState.Thrown && GetComponent<Rigidbody2D>().velocity.sqrMagnitude <= Constants.Min_Velocity)
            //iniciar corrutina y con dos segundos de delay
            StartCoroutine(DestroyAfter(2));
    }
    #endregion
    #region Methods
    IEnumerator DestroyAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
    public void AlDispararPajaro()
    {
        //inicializa sonido
        GetComponent<AudioSource>().Play();
        //mostramos train render
        GetComponent<TrailRenderer>().enabled = true;
        //Activamos la habilidad del ave
        holderHabilitie.SetActive(true);
        //agregamos gravedad al rigidbody
        GetComponent<Rigidbody2D>().isKinematic = false; 
        //se ajusta a escala normal el radio del ave
        GetComponent<CircleCollider2D>().radius = Constants.BirdColliderRadiusNormal; 
        State = BirdState.Thrown;
    }
    #endregion
}
