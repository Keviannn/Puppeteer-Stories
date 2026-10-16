using UnityEngine;
using UnityEngine.InputSystem; 

public class DragableObjects2D : MonoBehaviour
{
    Vector3 mousePos;
    Camera cam;
    bool active = false;
    public float timeToStopDrag = 1.0f;
    float time;
    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
	if(cam == null){
		Debug.LogError("DragableObjects2D no ha encontrado camara");
	}
    }

    // Update is called once per frame
    void Update()
    {
	if(active == false){
		rb = GetObject().Rigidbody2D;
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
        //Se cambia a vector2
		rb.MovePosition(Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y)));
	 
	}
    Rigidbody2D GetObject(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Rigidbody2D rb = hit.collider.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Guardamos la distancia a la cámara para el arrastre
                return rb;
            }
        }
        return null;
    }

}
