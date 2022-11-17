using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler,
     IPointerMoveHandler
{
    public Image icon;
    TemporaryCard card;

    public Text stackNumber;
    public Text infoText; // Prefab
    public Text info;

    public void OnPointerClick(PointerEventData pointerEventData) // player clicks on slot
    {
        if (card != null) // slot filled
        {
            // move card to mouse
        }
    }
    public void OnPointerMove(PointerEventData pointerEventData)
    {
        Vector3 mousePos = Input.mousePosition;
        info.transform.position = mousePos; // test out
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (card != null)
        {
            info = Instantiate(infoText, this.gameObject.transform);
            info.transform.position += new Vector3(0, 50, 0);
            info.text = card.info;
            info.enabled = true;
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (card != null)
        {

            info.enabled = false;
            Destroy(info);
        }
    }

    public void AddCard(TemporaryCard newCard)
    {
        card = newCard;
        icon.sprite = newCard.cardIcon;
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        card = null;
        icon.sprite = null;
        icon.enabled = false;
    }
    
}
