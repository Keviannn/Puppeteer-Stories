using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

public class Player2_5D : MonoBehaviour
{
    private Rigidbody rigid;
    [Header("Movimiento")]
    private float movimientoHorizontal = 0f;
    private float jumpIntention = 0f;
    [SerializeField] private float velocidadDeMovimiento;
    [SerializeField] private float suavizadoDeMovimiento = 0f;
    [SerializeField] private float jumpForce;
    
    private Vector3 velocidad = Vector3.zero;
    private bool mirandoDerecha = true;
    private bool isGrounded = false;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float inputH = 0f;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) 
        {
            inputH = -1f;
        }
        else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) 
        {
            inputH = 1f;
        }
        movimientoHorizontal = inputH * velocidadDeMovimiento;
	if(Keyboard.current.spaceKey.isPressed  && isGrounded){
		jumpIntention=1f;
	}
    }

    void FixedUpdate()
    {
        Mover(movimientoHorizontal * Time.fixedDeltaTime);
    }

    private void Mover(float mover)
    {
        Vector3 velocidadObjetivo = new Vector2(mover, rigid.linearVelocity.y);
        rigid.linearVelocity = Vector3.SmoothDamp(rigid.linearVelocity, velocidadObjetivo, ref velocidad, suavizadoDeMovimiento);
	rigid.AddForce(Vector3.up * jumpForce * jumpIntention, ForceMode.Impulse);
	jumpIntention = 0f;
        if(mover > 0 && !mirandoDerecha)
        {
            Girar();
        }
        else if(mover < 0 && mirandoDerecha)
        {
            Girar();
        }
    }

    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }
    void OnCollisionStay()
    {
        isGrounded = true;
    }
    void OnCollisionExit()
    {
	isGrounded = false;
    }
}
