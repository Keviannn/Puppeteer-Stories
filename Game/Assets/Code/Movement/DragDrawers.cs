using UnityEngine;

public class DragDrawers : MonoBehaviour
{
    public float inputBuffer = 0.1f;     // tiempo hasta que se para de arrastrar
    public float stiffness = 60f;        // rigidez del muelle (más alto = sigue más rápido)
    [Range(0.5f, 1.5f)]
    public float dampingRatio = 1f;      // 1 = sin rebote; <1 rebota un poco
    public float maxForce = 200f;        // límite de fuerza (evita empujones violentos)
    public float maxSpeed = 20f;         // límite de velocidad

    float time;
    Camera cam;
    bool active = false;
    Rigidbody rb;
    float dragDepth;                     // profundidad fija del punto de agarre
    float damping;

    void Start()
    {
        cam = Camera.main;
        if (cam == null)
            Debug.LogError("DragDrawers no ha encontrado cámara");
    }

    void Update()
    {
        if (!active)
            rb = GetObject();

        if (rb != null && Input.GetMouseButton(0))
        {
            if (!active)
            {
                active = true;
                // Se calcula UNA vez, al agarrar el objeto
                dragDepth = cam.WorldToScreenPoint(rb.position).z;
                // Amortiguación crítica: sin rebotes ni oscilación
                damping = 2f * Mathf.Sqrt(stiffness * rb.mass) * dampingRatio;
            }
            time = inputBuffer;
        }
        else if (time >= 0 && active)
        {
            time -= Time.deltaTime;
        }
        else if (active) {
    active = false;
    if (rb != null){
        // Eliminar la velocidad acumulada al soltar
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
    rb = null;
}
    }

    void FixedUpdate()
    {
        if (active && rb != null)
            MoveToMouse();
    }

    void MoveToMouse()
    {
        Vector3 mouse = Input.mousePosition;
        mouse.z = dragDepth;
        Vector3 target = cam.ScreenToWorldPoint(mouse);

        Vector3 toTarget = target - rb.position;

        // Muelle (hacia el cursor) + amortiguador (contra la velocidad)
        Vector3 force = toTarget * stiffness - rb.linearVelocity * damping;
        force = Vector3.ClampMagnitude(force, maxForce);
        rb.AddForce(force, ForceMode.Force);

        // Límite de velocidad
        if (rb.linearVelocity.magnitude > maxSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
    }

    Rigidbody GetObject()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Rigidbody body = hit.collider.GetComponent<Rigidbody>();
            if (body != null && hit.collider.CompareTag("Drawer"))
                return body;
        }
        return null;
    }
}