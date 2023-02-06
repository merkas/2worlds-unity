using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }





    void OnMouseEnter()
    {

        //Enable Panel mit Info

        transform.position += Vector3.up * 0.2f;
        //transform.position += Vector3.right * 0.1f;
    }

    void OnMouseExit()
    {
        transform.position += Vector3.up * -0.2f;
        //transform.position += Vector3.right * -0.1f;
    }
}
