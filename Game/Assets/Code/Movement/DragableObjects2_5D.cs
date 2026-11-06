using UnityEngine;
using UnityEngine.InputSystem; 
using System.Collections;
using UnityEngine.InputSystem.LowLevel;

/*
 * Este script solo debería estar una vez por escena, puede estar en cualquier objeto
 * Solo funciona sobre objetos con la tag "Drag"
 * Falta por implementar deceleracion cuando se acerca al mouse y probablemente desactivar la gravedad
 * y programar un equivalente, para que vaya el linea recta al mouse pero sigas moviendo diferente cosas por su peso
 */

public class DragableObjects2_5D : MonoBehaviour
{
    public float inputBuffer = 0.1f; // tiempo hasta que se pare de arrastrar
   // public float maxSpeed = 20.0f;
    public float dragForce = 15.0f;
    public float minDistanceToMouse = 0.03f;

    float time;
    Camera cam; // Camara usada para calcular pos del mouse
    public bool isActiv = false; // true si se esta arrastrando algo 
    Rigidbody rb;

    void Start()
    {
        cam = Camera.main;
	if(cam == null){
		Debug.LogError("DragableObjects2_5D no ha encontrado camara");
	}
    }

    void Update()
    {
	if(isActiv == false){
		rb = GetObject();
	}
        if(rb != null && Mouse.current.leftButton.isPressed){
		if(!isActiv){
			isActiv = true;
		}
		time = inputBuffer;
	} //estas funciones están solo para que puedas seguir arrastrando aunque no todos los frames te detecte el click
	  else if(time >= 0 && isActiv){
		time = time - Time.deltaTime;
	} else if (isActiv) {
		isActiv = false;
		rb = null;
	}
    }

    void FixedUpdate(){
	if(isActiv){
		MoveToMouse();
	}
    }

    // Controla todo el movimiento de los objetos
    void MoveToMouse()
	{
		var mousePosition = Mouse.current.position.ReadValue();

		float distance_to_screen = cam.WorldToScreenPoint(rb.position).z;
		Vector3 pclick = cam.ScreenToWorldPoint( new Vector3(mousePosition.x, mousePosition.y, distance_to_screen));
		Vector3 direction = pclick - rb.position;
		float distanceToMouse = new Vector3(direction.x,0,direction.z).magnitude;
		direction.Normalize();
		Debug.Log(rb.position);

		if(distanceToMouse>=minDistanceToMouse){
			if(dragForce>=9.81f*rb.mass){
				rb.AddForce(9.81f *rb.mass* Vector3.up);
				rb.AddForce((dragForce-9.81f*rb.mass)*direction,ForceMode.Force);
			} else {
				rb.AddForce(dragForce*direction,ForceMode.Force);
			}
			
			Vector3 stoppingForce = -(rb.mass*rb.linearVelocity)/(2*distanceToMouse/rb.linearVelocity.magnitude); //esto intenta minimizar el overshooting
			rb.AddForce(stoppingForce, ForceMode.Force);
		} else if (rb.linearVelocity.magnitude<=0.6f){
			rb.linearVelocity = Vector3.zero;
			rb.AddForce(9.81f *rb.mass* Vector3.up);
		}
	}
    // Devuelve el rigidbody del objeto que tiene encima el raton
    public Rigidbody GetObject(){
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
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
