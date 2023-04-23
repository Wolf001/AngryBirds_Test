using UnityEngine;
using System.Collections;

public class Destroyer : MonoBehaviour {
    //trigger si tiene contacto ave o puerco o estructura, desrtuir game object
    void OnTriggerEnter2D(Collider2D col)
    {
        string tag = col.gameObject.tag;
        if(tag == "Bird" || tag == "Pig" || tag == "Brick")
        {
            Destroy(col.gameObject);
        }
    }
}
