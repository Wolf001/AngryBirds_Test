using UnityEngine;

public class ParallaxScrolling : MonoBehaviour 
{
    private new Camera  camera;
    public float ParallaxFactor;
    Vector3 previous_CameraTransform;
    //se asigna la camara principal a la variable camara
    void Start () {
        camera = Camera.main;
        previous_CameraTransform = camera.transform.position;
	}    
	void Update () {
        Vector3 delta = camera.transform.position - previous_CameraTransform;
        delta.y = 0; delta.z = 0;
        transform.position += delta / ParallaxFactor;
        previous_CameraTransform = camera.transform.position;
	}
    
}
