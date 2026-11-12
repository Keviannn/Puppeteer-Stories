using UnityEngine;

public class Key : MonoBehaviour
{
    public static int keyCount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        keyCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider){
        if(collider.gameObject.tag == "Player"){
            keyCount++;
            Destroy(gameObject);
        }
    }
    
}
