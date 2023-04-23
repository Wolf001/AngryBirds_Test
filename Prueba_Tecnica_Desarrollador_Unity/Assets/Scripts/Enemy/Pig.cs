using UnityEngine;
public class Pig : MonoBehaviour
{
    #region Variables
    public float Health = 150f;
    public Sprite SpriteShownWhenHurt;
    private float ChangeSpriteHealth;
    #endregion
    #region Main Methods
    void Start()
    {
        ChangeSpriteHealth = Health - 30f;
    }
    #endregion
    #region Collision Enter
    //comportamiento del puerco 
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<Rigidbody2D>() == null) return;
        //si colisiona con un ave
        if (col.gameObject.tag == "Bird")
        {
            GetComponent<AudioSource>().Play();
            Destroy(gameObject);
        }
        else
        {
            //si colisiona con algo mas 
            float damage = col.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 10;
            Health -= damage;
            if (damage >= 10)
                GetComponent<AudioSource>().Play();
            if (Health < ChangeSpriteHealth)
                //cambiamos el diseño del sprite por el de dañado
                GetComponent<SpriteRenderer>().sprite = SpriteShownWhenHurt;
            //si la vida es menor o igual a cero entonces destruimos el puerco
            if (Health <= 0) Destroy(this.gameObject);
        }
    }
    #endregion
}
