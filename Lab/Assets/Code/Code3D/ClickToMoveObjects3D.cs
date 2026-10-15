using UnityEngine;

public class ClickToMoveObjects3D : MonoBehaviour
{
    public bool posicion_original;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        posicion_original = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown(){
        if (posicion_original){
            Vector3 aux1 = new Vector3(0f,0f,1f);
            transform.position=transform.position + 10f * aux1;
            posicion_original = false;
            
        }
        else{
            Vector3 aux1 = new Vector3(0f,0f,-1f);
            transform.position=transform.position + 10f * aux1;
            posicion_original = true;
            
        }
    }
}