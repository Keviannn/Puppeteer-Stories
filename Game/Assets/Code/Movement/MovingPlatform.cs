using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 1f;
    Vector3 targetVector = Vector3.back;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
	transform.Translate(targetVector*speed);
	if(transform.position.z > 3 || transform.position.z < -3){
	    speed = -speed;
	}
    }
}
