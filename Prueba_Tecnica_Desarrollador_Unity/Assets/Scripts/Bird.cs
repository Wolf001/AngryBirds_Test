using UnityEngine;
using System.Collections;
using Assets.Scripts;

[RequireComponent(typeof(Rigidbody2D))]
public class Bird : MonoBehaviour
{

    public void AlDispararPajaro()
    {
        GetComponent<AudioSource>().Play(); GetComponent<TrailRenderer>().enabled = true; GetComponent<Rigidbody2D>().isKinematic = false; GetComponent<CircleCollider2D>().radius = Constants.BirdColliderRadiusNormal; State = BirdState.Thrown;
    }

    IEnumerator DestroyAfter(float seconds)  {  yield return new WaitForSeconds(seconds); Destroy(gameObject); }
    public BirdState State {  get; private set; }

    void FixedUpdate()
    {
        if (State == BirdState.Thrown && GetComponent<Rigidbody2D>().velocity.sqrMagnitude <= Constants.Min_Velocity)
            StartCoroutine(DestroyAfter(2));
    }
    
    void Start()
    {
        GetComponent<TrailRenderer>().enabled = false;
        GetComponent<TrailRenderer>().sortingLayerName = "Foreground";
        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<CircleCollider2D>().radius = Constants.Bird_Collider_Radius_Big;
        State = BirdState.BeforeThrown;
    }
}
