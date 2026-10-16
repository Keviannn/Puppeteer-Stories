using UnityEngine;

public class DragableObjects3D : MonoBehaviour
{
    Rigidbody objectToMove;
    public float speed = 500f;
    float objectDepth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void FixedUpdate(){
        if(objectToMove){
            Vector3 cursorPosition = Input.mousePosition;
            cursorPosition.z = objectDepth;
            Vector3 dir= Camera.main.ScreenToWorldPoint(cursorPosition) - objectToMove.transform.position;
            objectToMove.linearVelocity= dir * speed * Time.fixedDeltaTime;
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
	    string tag = hit.collider.tag;
            if (rb != null && tag == "Drag")
            {
                // Guardamos la distancia a la cámara para el arrastre
                objectDepth = Vector3.Distance(Camera.main.transform.position, hit.point);
                return rb;
            }
        }
        return null;
    }
            
}
