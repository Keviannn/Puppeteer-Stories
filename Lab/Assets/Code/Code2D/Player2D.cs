using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

public class Player2D : MonoBehaviour
{
    private Rigidbody2D rigid2D;

    [Header("Movimiento")]

    private float movimientoHorizontal = 0f;
    [SerializeField] private float velocidadDeMovimiento;
    [SerializeField] private float suavizadoDeMovimiento = 0f;
    
    private Vector3 velocidad = Vector3.zero;
    private bool mirandoDerecha = true;

    [Header("Salto")]

    [SerializeField] private float fuerzaDeSalto;
    //Identifica qué es suelo
    [SerializeField] private LayerMask queEsSuelo;
    //Comprueba si tiene suelo debajo para saltar (Crea el Controlador)
    [SerializeField] private Transform controladorSuelo;
    //Dimensiones del controlador
    [SerializeField] private Vector3 dimensionesCaja;
    //Está en el suelo?
    [SerializeField] private bool enSuelo;
    //Está saltando?
    private bool salto = false;


    void Start()
    {
        rigid2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //Input para moverse Horizontalmente
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

        //Input para salto
        if(Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            salto = true;
        }
    }

    void FixedUpdate()
    {
        //Comprobar si está en suelo
        enSuelo = Physics2D.OverlapBox(controladorSuelo.position, dimensionesCaja, 0f, queEsSuelo);
        //Mover
        Mover(movimientoHorizontal * Time.fixedDeltaTime, salto);
        //Tras saltar, hace que el salto sea false para que no sea infinito
        salto = false;
    }

    //Función que permite al personaje moverse
    private void Mover(float mover, bool saltar)
    {
        //Movimiento Horizontal
        Vector3 velocidadObjetivo = new Vector2(mover, rigid2D.linearVelocity.y);
        rigid2D.linearVelocity = Vector3.SmoothDamp(rigid2D.linearVelocity, velocidadObjetivo, ref velocidad, suavizadoDeMovimiento);
        //Gira al cambiar de dirección
        if(mover > 0 && !mirandoDerecha)
        {
            Girar();
        }
        else if(mover < 0 && mirandoDerecha)
        {
            Girar();
        }
        //Salto
        if(enSuelo && saltar)
        {
            enSuelo = false;
            rigid2D.AddForce(new Vector2(0f, fuerzaDeSalto));
        }
    }

    //Gira el sprite al cambiar de dirección (Con cápsula no se nota pero con el sprite sí)
    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    //Permite ver la caja creada para ver si toca el suelo o no (Perfecto para testing)
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(controladorSuelo.position, dimensionesCaja);
    }
}