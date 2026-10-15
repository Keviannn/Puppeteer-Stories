using UnityEngine;

public class Movimiento3D : MonoBehaviour
{
    private Rigidbody _rigid;
    public float speed = 5f;
    public float jump = 40f;
    public float mirarSpeed = 10f;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         _rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movimientoDir  = new Vector3(horizontal,0f,vertical);
        transform.position = transform.position + speed * movimientoDir * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(movimientoDir),mirarSpeed * Time.deltaTime);
    }

    private void OnCollisionStay(Collision collision){
        if(collision.gameObject.tag == "Untagged"){
            if(Input.GetKeyDown(KeyCode.Space)){
            Vector3 jumpDir = new Vector3(0,10f,0);
            _rigid.AddForce(jump * jumpDir);
            }
        }
    }
}
