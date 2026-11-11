using UnityEngine;
using UnityEngine.InputSystem; 
using System.Collections;

public class ReturnPlatform : MonoBehaviour
{

    public GameObject platform;
    public GameObject positionobject;

    public float tiempoDeEspera = 3f;

    private Vector3 posicionOriginal;
    
    [Range(0f,5f)]
    public float velocityplatform=2f;

    private float temporizador;

    void Start()
    {
        
        posicionOriginal = platform.transform.position;
        temporizador = tiempoDeEspera;
    }


    // Update is called once per frame
    void Update()
    {

        if (Mouse.current.leftButton.isPressed)
        {
            temporizador = tiempoDeEspera;
        }else{
            if (temporizador > 0)
            {
                temporizador -= Time.deltaTime;
            }else{

                platform.transform.position = Vector3.MoveTowards(platform.transform.position, posicionOriginal, velocityplatform * Time.deltaTime);

                Rigidbody rb = platform.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }

            }
        }
        

    }
}
