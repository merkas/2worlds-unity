using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{


    public bool isInRange;
    public KeyCode interactKey;
    public UnityEvent interactionAction;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isInRange)
        {   //Event System triggers event on chosen Keycode
            if(Input.GetKeyDown(interactKey))
            {
                interactionAction.Invoke();
            }
        }
    }

    //Is interactable when inside the trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            isInRange = true;
            Debug.Log("Player in Range");
        }
    }
    //Is not interactable when exiting the trigger
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = false;
            Debug.Log("Player not in Range");
        }
    }
}
