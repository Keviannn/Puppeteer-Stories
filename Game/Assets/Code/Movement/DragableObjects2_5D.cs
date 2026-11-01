using UnityEngine;
using UnityEngine.InputSystem; 

/*
 * Este script solo debería estar una vez por escena, puede estar en cualquier objeto
 * Solo funciona sobre objetos con la tag "Drag"
 * Falta por implementar deceleracion cuando se acerca al mouse y probablemente desactivar la gravedad
 * y programar un equivalente, para que vaya el linea recta al mouse pero sigas moviendo diferente cosas por su peso
 */

public class DragableObjects2_5D : MonoBehaviour
{
    public float inputBuffer = 0.1f; // tiempo hasta que se pare de arrastrar
    public float maxSpeed = 20.0f;
    public float dragForce = 15.0f;
    public float maxDistanceToMouse = 5.0f; //distancias a partir de las cual el linearDamp no cambia
    public float minDistanceToMouse = 1.0f;

    float linearDamping;
    float time;
    Camera cam; // Camara usada para calcular pos del mouse
    bool active = false; // true si se esta arrastrando algo 
    Rigidbody rb;
    float originalDamp; // Valor del LinearDamp antes de arrastre

    void Start()
    {
        cam = Camera.main;
	if(cam == null){
		Debug.LogError("DragableObjects2_5D no ha encontrado camara");
	}
    }

    void Update()
    {
	if(active == false){
		rb = GetObject();
	}
        if(rb != null && Input.GetMouseButton(0)){
		if(!active){
			active = true;
			originalDamp = rb.linearDamping;
			linearDamping = dragForce/rb.mass/maxSpeed;
			// rb.useGravity = false;
		}
		time = inputBuffer;
	} //estas funciones están solo para que puedas seguir arrastrando aunque no todos los frames te detecte el click
	  else if(time >= 0 && active){
		time = time - Time.deltaTime;
	} else if (active) {
		active = false;
		if(rb != null){
			//rb.useddGravity = true;
			rb.linearDamping = originalDamp;
		}
		rb = null;
	}
    }

    void FixedUpdate(){
	if(active){
		MoveToMouse();
	}
    }
    // Controla todo el movimiento de los objetos
    void MoveToMouse()
	{
		float distance_to_screen = cam.WorldToScreenPoint(transform.position).z;
		Vector3 pclick = cam.ScreenToWorldPoint( new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance_to_screen));
		Vector3 direction = pclick - rb.position;
		float distanceToMouse = new Vector3(direction.x,0,direction.z).magnitude;
		direction.Normalize();
		//esto regula la velocidad maxima
		if(distanceToMouse >= maxDistanceToMouse){
			rb.linearDamping = 0f;
		} else if(distanceToMouse <= minDistanceToMouse){
			rb.linearDamping = linearDamping;
		} else {
			rb.linearDamping = (distanceToMouse-minDistanceToMouse)/(maxDistanceToMouse-minDistanceToMouse) * linearDamping;
		}
		rb.AddForce(dragForce*direction,ForceMode.Force);
	}
    // Devuelve el rigidbody del objeto que tiene encima el raton
    Rigidbody GetObject(){
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if (rb != null && hit.collider.tag == "Drag")
            {
                return rb;
            }
        }
        return null;
    }

}
