using UnityEngine;

public class Brick : MonoBehaviour
{
    public float Health = 70f;
    //cuando colisiona con un objeto realiza el calculo de daño
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<Rigidbody2D>() == null) return;

        float damage = col.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 10;
        if (damage >= 10)
            GetComponent<AudioSource>().Play();
        Health -= damage;
        //si el daño es igual o menor a cero destruye el gameobject
        if (Health <= 0) Destroy(this.gameObject);
    }

    
}
