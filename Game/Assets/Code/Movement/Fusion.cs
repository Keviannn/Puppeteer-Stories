using UnityEngine;

public class Fusion : MonoBehaviour
{
    private FixedJoint jointVertical;
    private Rigidbody rb;
    private bool siendoArrastrado = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
        if (Input.GetMouseButtonDown(0) && Camera.main != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.rigidbody == rb)
                {
                    siendoArrastrado = true;
                }
            }
        }

        //It only creates the joint if the object is selected with the mouse.
        if (!Input.GetMouseButton(0))
        {
            siendoArrastrado = false;
            if (jointVertical != null)
            {
                Destroy(jointVertical);
                jointVertical = null;
            }
        }

        //If the player moves, the joint disappears
        if (jointVertical != null && jointVertical.connectedBody != null)
        {
            if (jointVertical.connectedBody.CompareTag("Player"))
            {
                if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || 
                    Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
                {
                    Destroy(jointVertical);
                    jointVertical = null;
                }
            }
        }
    }


    //the joint is created
    void OnCollisionStay(Collision collision)
    {
        
        if (!siendoArrastrado || jointVertical != null || collision.rigidbody == null) 
        return;

        
        if (collision.transform.position.y > transform.position.y + 0.4f)
        {
            
            if (collision.gameObject.CompareTag("Player"))
            {
                if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || 
                    Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
                {
                    return;
                }
            }

            
            jointVertical = gameObject.AddComponent<FixedJoint>();
            jointVertical.connectedBody = collision.rigidbody;
        }
    }
}
