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
    [SerializeField] private float aceleracion = 0f;
    [SerializeField] private float deceleracion = 0f;
    private float velocidadHorizontal = 0f;

    [SerializeField] private float jumpForce;
    
    private Vector3 velocidad = Vector3.zero;
    private bool mirandoDerecha = true;
    private bool isGrounded = false;

    private float coyoteTime=0.15f;
    private float coyoteTimeCounter;

    private float jumpBuffer = 0.25f;
    private float jumpBufferCounter;


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
        if(isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        if(Keyboard.current.spaceKey.isPressed){
            jumpBufferCounter = jumpBuffer;
        }
        else
        {
            jumpBufferCounter-= Time.deltaTime;
        }
        if(jumpBufferCounter > 0  && coyoteTimeCounter > 0f)
        {
            Vector3 v = rigid.linearVelocity;
            if (v.y > 0f)
            {
                v.y *= 0f;
                rigid.linearVelocity = v;
            }
            coyoteTimeCounter = 0f;
            jumpBufferCounter = 0f;
            jumpIntention=1f;
	    }
        if(Keyboard.current.spaceKey.wasReleasedThisFrame && rigid.linearVelocity.y > 0)
        {
            Vector3 v = rigid.linearVelocity;
            if (v.y > 0f)
            {
                v.y *= 0.5f;
                rigid.linearVelocity = v;
            }
        }
    }

    void FixedUpdate()
    {
        Mover(movimientoHorizontal * Time.fixedDeltaTime);
    }

    private void Mover(float mover)
    {
        //Tasa de Aceleración/Deceleración
        float tasa;
        if(mover != 0f)
        {
            tasa = aceleracion;
        }
        else
        {
            tasa = deceleracion;
        }

        //Función de velocidad afectada por la aceleración
        velocidadHorizontal = Mathf.MoveTowards(velocidadHorizontal, mover, tasa * Time.fixedDeltaTime);

        Vector3 velocidadObjetivo = new Vector2(velocidadHorizontal, rigid.linearVelocity.y);
        rigid.linearVelocity = Vector3.SmoothDamp(rigid.linearVelocity, velocidadObjetivo, ref velocidad, suavizadoDeMovimiento);
	    rigid.AddForce(Vector3.up * jumpForce * jumpIntention * 3f, ForceMode.Impulse);
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
    void OnTriggerStay()
    {
        isGrounded = true;
    }
    void OnTriggerExit()
    {
	isGrounded = false;
    }
}
