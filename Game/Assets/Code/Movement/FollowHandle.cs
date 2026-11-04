using UnityEngine;

public class FollowHandle : MonoBehaviour
{
Rigidbody rb;
public Rigidbody objetivo;
Vector3 desfase;

void Start()
{
    rb = GetComponent<Rigidbody>();
    desfase = rb.position - objetivo.position;
}

void FixedUpdate()
{
    rb.MovePosition(objetivo.position + desfase);
}
}
