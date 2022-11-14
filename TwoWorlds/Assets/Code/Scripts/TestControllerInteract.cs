using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestControllerInteract : MonoBehaviour
{

    public bool isOpen;
    public bool isDoorOpen;

    Collider2D m_Collider;



    private void Start()
    {
        m_Collider = GetComponent<Collider2D>();
    }


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
                    m_Collider.enabled = !m_Collider.enabled;

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
