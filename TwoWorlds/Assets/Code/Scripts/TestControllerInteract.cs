using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestControllerInteract : MonoBehaviour
{

    public bool isOpen;
    public bool isDoorOpen;

    public void OpenDoors(GameObject obj)
    {
        if (!isDoorOpen)
        {
            PlayManager manager = obj.GetComponent<PlayManager>();
            if (manager)
            {
                if (manager.keyCount > 0)
                {
                    isDoorOpen = true;
                    manager.UseKey();
                    Debug.Log("Door opened");
                }
            }
        }
    }


    public void OpenChest()
    {
        if (!isOpen)
        {
            isOpen = true;
            Debug.Log("Chest is open");


        }
    }

    public void destroyObstacle()
    {
        if (this.gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
