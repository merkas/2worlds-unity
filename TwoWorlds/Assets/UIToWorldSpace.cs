using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIToWorldSpace : MonoBehaviour
{
    public GameObject worldObject;
    //public Text chatterText;
    RectTransform rectTransform;

    RectTransform canvasRect;
    public Canvas usedCanvas;
    public Camera cam;
    Vector2 uiOffset;

    private TextMeshProUGUI chatterText;

    void Start()
    {
        


    //canvasRect = usedCanvas.GetComponent<RectTransform>();
    //this.rectTransform = GetComponent<RectTransform>();

    //this.uiOffset = new Vector2((float)Canvas.sizeDelta.x / 2f, (float)Canvas.sizeDelta.y / 2f);

    //Vector2 pos = worldObject.transform.position;  // get the game object position
    //Vector2 viewportPoint = Camera.main.WorldToViewportPoint(pos);  //convert game object position to VievportPoint

    // set MIN and MAX Anchor values(positions) to the same position (ViewportPoint)
    //rectTransform.anchorMin = viewportPoint;
    //rectTransform.anchorMax = viewportPoint;

}

    public void ObjectToClickPoint(Vector3 objectTransformPosition)
    {
        // Get the position on the canvas
        //Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(objectTransformPosition);
        //Vector2 proportionalPosition = new Vector2(ViewportPosition.x * Canvas.sizeDelta.x, ViewportPosition.y * Canvas.sizeDelta.y);

        // Set the position and remove the screen offset
        //this.rectTransform.localPosition = proportionalPosition - uiOffset;
    }


    void Update()
    {
        
    }
}
