using UnityEngine;

public class ClickToMoveObjects2D : MonoBehaviour
{
    public bool posicion_original;
    void Start()
    {
        posicion_original = true;
    }

    private void OnMouseDown()
    {
        if (posicion_original)
        {
            Vector3 aux1 = new Vector3(0f, 1f, 0f);   
            posicion_original = false;
        }
        else
        {
            Vector3 aux1 = new Vector3(0f, -1f, 0f);  
            posicion_original = true;
        }
    }
}