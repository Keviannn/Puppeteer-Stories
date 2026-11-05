using UnityEngine;

public class FollowHandle : MonoBehaviour
{
    Rigidbody rb;
    public Rigidbody objetivo;
    Vector3 desfase;

    [SerializeField] float maxSpeed = 20f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        desfase = rb.position - objetivo.position;
    }

    void FixedUpdate()
    {
        Vector3 destino = objetivo.position + desfase;
        Vector3 error = destino - rb.position;

        // Velocidad necesaria para llegar al destino en este paso de física
        Vector3 vel = error / Time.fixedDeltaTime;
        vel = Vector3.ClampMagnitude(vel, maxSpeed);

        rb.linearVelocity = vel;
    }
}