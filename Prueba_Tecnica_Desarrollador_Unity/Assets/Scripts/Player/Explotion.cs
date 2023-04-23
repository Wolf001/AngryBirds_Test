using Assets.Scripts;
using System.Collections;
using UnityEngine;

public class Explotion : MonoBehaviour
{
    #region Variables
    public BirdState State { get; private set; }
    public SlingShot charpe;
    public GameObject BirdExplosive;
    [SerializeField] private float radioDamage;
    [SerializeField] private float explotionForce;
    [SerializeField] GameObject explotion;
    public AddScore addScor;
    #endregion
    #region Update
    void Update()
    {
        if (charpe.slingshotState == SlingshotState.BirdFlying)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ExplotionVFX();
            }
        }
    }
    #endregion
    #region Method
    void ExplotionVFX()
    {
        //instancia del VFX explosion
        Instantiate(explotion, transform.position, Quaternion.identity);
        //se asigna la fisica circular acorde al tamaño del radio para la colision
        Collider2D[] Damage = Physics2D.OverlapCircleAll(transform.position, radioDamage);

        foreach (Collider2D damageZone in Damage)
        {
            Rigidbody2D rb2D = damageZone.GetComponent<Rigidbody2D>();
            if (rb2D != null)
            {
                //calculo de la fuerza ejercida por la explosion
                Vector2 dirDam = damageZone.transform.position - transform.position;
                float rango = 1 + dirDam.magnitude;
                float finalDam = explotionForce / rango;
                rb2D.AddForce(dirDam * finalDam);
                if (damageZone.tag == "Brick" || damageZone.tag == "Pig") 
                {
                    addScor.addScore = addScor.addScore + 500;
                }
            }
        }
        StartCoroutine(DestroyAfter(0.1f));
    }
    //destruccion de el gameobject
    IEnumerator DestroyAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(BirdExplosive);
    }
    #endregion
}
