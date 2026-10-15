using UnityEngine;

public class DraggableObjects2D : MonoBehaviour
{
    Rigidbody2D objectToMove;
    public float speed = 500f;
    Camera cam;
    public LayerMask draggableLayers = ~0;
    float objectDepth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void FixedUpdate(){
        if(objectToMove != null){
            Vector2 cursorPos = Input.mousePosition;
            Vector2 dir = cursorPos - objectToMove.position;            
            objectToMove.linearVelocity= dir * speed * Time.fixedDeltaTime;
        }
    }
    void Awake()
    {
    cam = Camera.main;

    // Fallback: si no tiene el tag MainCamera, buscamos cualquiera en la escena
    if (cam == null)
        cam = FindFirstObjectByType<Camera>();

    // Si aún así no hay cámara, avisamos claramente
    if (cam == null)
        Debug.LogError("DragableObjects2D: No se encontró ninguna cámara en la escena.");
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            objectToMove = GetObject();
        }
        if(Input.GetMouseButtonUp(0)){
            objectToMove = null;
        }
    }

    //Convierte la posición z del cursor siempre a 0 (Plano 2D)
    Vector2 GetMouseWorldPosition()
    {
        Vector3 mp = Input.mousePosition;
        mp.z = -cam.transform.position.z; 
        return cam.ScreenToWorldPoint(mp);
    }

    Rigidbody2D GetObject()
    {
        Vector2 worldPoint = GetMouseWorldPosition();

        // OverlapPoint es lo más simple y eficiente en 2D
        Collider2D hit = Physics2D.OverlapPoint(worldPoint, draggableLayers);
        if (hit != null)
            return hit.attachedRigidbody;

        return null;
    }
            
}
