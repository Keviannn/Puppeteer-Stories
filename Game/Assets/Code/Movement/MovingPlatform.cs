using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 1f;
    Vector3 targetVector = Vector3.back;
    
    void Start()
    {
    }

    // Mueve la plataforma de un lado a otro
    void Update()
    {
	transform.Translate(targetVector*speed);
	if(transform.position.z > 3 || transform.position.z < -3){
	    speed = -speed;
	}
    }
}
