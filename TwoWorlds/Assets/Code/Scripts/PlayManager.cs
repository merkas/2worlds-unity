using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayManager : MonoBehaviour
{

    public int keyCount;
    public Transform PickUpPoint;

    public void PickUpKey()
    {
        keyCount++;
        Debug.Log("Key found!");
    }

    public void UseKey()
    {
        keyCount--;
        Debug.Log("Key used!");
    }

    public void pickUPP()
    {
        //gameObject.transform.position = PickUpPoint.transform.position;

    }
   



}
