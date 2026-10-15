using UnityEngine;
using UnityEngine.InputSystem; 

public class DragableObjects2_5D : MonoBehaviour
{
    Vector3 mousePos;
    Camera cam;
    bool active = false;
    public float timeToStopDrag = 1.0f;
    float time;
    Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
	if(cam == null){
		Debug.LogError("DragableObjects2_5D no ha encontrado camara");
	}
    }

    // Update is called once per frame
    void Update()
    {
	if(active == false){
		rb = GetObject();
	}
        if(rb != null && Input.GetMouseButton(0)){
		active = true;
		time = timeToStopDrag;
	} else if(time >= 0){
		time = time - Time.deltaTime;
	} else {
		active = false;
		rb =null;
	}
    }

    void FixedUpdate(){
	if(active){
		OnMouseDrag();
	}

    }
    void OnMouseDrag()
	{
		float distance_to_screen = Camera.main.WorldToScreenPoint(transform.position).z;
		rb.MovePosition(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance_to_screen)));
	 
	}
    Rigidbody GetObject(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Guardamos la distancia a la cámara para el arrastre
                return rb;
            }
        }
        return null;
    }

}
