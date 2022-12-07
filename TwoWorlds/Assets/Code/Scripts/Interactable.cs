using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{


    public bool isInRange;
    public KeyCode interactKey;
    public UnityEvent interactionAction;

    public GameObject NotificationPrefab;

    // Start is called before the first frame update
    void Start()
    {
            if (NotificationPrefab != null) NotificationPrefab.SetActive(false);      
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
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = true;
            Debug.Log("Player in Range");
            if (NotificationPrefab != null) NotificationPrefab.SetActive(true);
        }
    }
    //Is not interactable when exiting the trigger
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = false;
            Debug.Log("Player not in Range");
            if (NotificationPrefab != null) NotificationPrefab.SetActive(false);
            UIManager.instance.OpenTextBox(false);
        }
    }


    public void Interacted()
    {
            GetComponent<Collider2D>().enabled = false;
    }


    //public void ChestOpened(GameObject obj)
    //{
         
    //        TestControllerInteract manager = obj.GetComponent<TestControllerInteract>();
    //    if (manager)
    //    {

    //        if (manager.isOpen == true)
    //        {
    //            NotificationPrefab.SetActive(false);
    //        }
    //    }
    //}

}
