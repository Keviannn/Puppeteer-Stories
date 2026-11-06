using UnityEngine;
using UnityEngine.InputSystem; 
using System.Collections;
using UnityEngine.InputSystem.LowLevel;

/*
 * Este script controla la funcion de arrastrar objetos con el raton
 * Este script solo debería estar una vez por escena, puede estar en cualquier objeto
 * Solo funciona sobre objetos con la tag "Drag"
 */

public class DragableObjects2_5D : MonoBehaviour
{

    public float inputBuffer = 0.1f; // tiempo hasta que se pare de arrastrar
    public float dragForce = 15.0f; // Fuerza del arrastre
    public float minDistanceToMouse = 0.03f; // Minima distancia para detectar el movimieto

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
	// Detección del input del ratón
        if(rb != null && Mouse.current.leftButton.isPressed){
		if(!isActiv){
			isActiv = true;
		}
		time = inputBuffer;
	} 
	// Implementación de un buffer de detección del raton
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
		
		if(distanceToMouse>=minDistanceToMouse){
			// Corrige la dirección si lo puedes levantar
			if(dragForce>=9.81f*rb.mass){
				rb.AddForce(9.81f *rb.mass* Vector3.up);
				rb.AddForce((dragForce-9.81f*rb.mass)*direction,ForceMode.Force);
			}
			// Mueve directamente hacia el mouse sin corregir por gravar
			else {
				rb.AddForce(dragForce*direction,ForceMode.Force);
			}
			//decelera el objeto antes de que llegue al raton
			Vector3 stoppingForce = -(rb.mass*rb.linearVelocity)/(2*distanceToMouse/rb.linearVelocity.magnitude);
			rb.AddForce(stoppingForce, ForceMode.Force);
		} 
		// Para el objeto si esta muy cerca y va muy lento
		else if (rb.linearVelocity.magnitude<=0.6f){
			rb.linearVelocity = Vector3.zero;
			rb.AddForce(9.81f *rb.mass* Vector3.up);
		}
	}

    // Devuelve el rigidbody del objeto que tiene encima el raton si tiene la tag "Drag"
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
