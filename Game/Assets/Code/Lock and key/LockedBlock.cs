using UnityEngine;

public class LockedBlock : MonoBehaviour
{
    private bool cerrado;
    public GameObject KeyPrefab,KeySpawner;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cerrado = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider collider){
        if(collider.gameObject.tag == "Player" && Key.keyCount > 0 && cerrado){
            Key.keyCount--;
            cerrado = false;
            Destroy(gameObject, 2f);


        }
    }
}
