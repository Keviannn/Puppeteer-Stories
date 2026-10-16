using UnityEngine;

public class AltDragableObjects : MonoBehaviour
{
    Rigidbody objectToMove;
    public float speed = 50f;
    float objectDepth;
    string tag;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void FixedUpdate(){
        if(objectToMove){
            Vector3 cursorPosition = Input.mousePosition;
            cursorPosition.z = objectDepth;
            Vector3 dir= Camera.main.ScreenToWorldPoint(cursorPosition) - objectToMove.transform.position;
	    if(dir.y < 2f && dir.y>-2f){
	    	objectToMove.linearVelocity= Vector3.Scale(dir, new Vector3(1.0f,0f,1.0f))* speed * Time.fixedDeltaTime;
	    } else {
		objectToMove.linearVelocity= Vector3.Scale(dir, new Vector3(1.0f,1.0f,1.0f))* speed * Time.fixedDeltaTime;
	    }
	    Debug.Log(Vector2.Angle(Vector2.up,new Vector2(dir.x,dir.y))+"---"+Vector2.Angle(Vector2.up,new Vector2(dir.y,dir.z)));
	    //if(Vector3.Angle(Vector3.UP,dir){
	   // ultDir = 0.5f;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            objectToMove = GetObject();
        }
        if(Input.GetMouseButtonUp(0)){
            objectToMove = null;
        }
    }

    Rigidbody GetObject(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
	    tag = hit.collider.tag;
            if (rb != null && tag == "AltDrag")
            {
                // Guardamos la distancia a la cámara para el arrastre
                objectDepth = Vector3.Distance(Camera.main.transform.position, hit.point);
                return rb;
            }
        }
        return null;
    }
            
}
