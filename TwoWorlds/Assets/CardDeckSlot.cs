using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardDeckSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler,
     IPointerMoveHandler
{
    public Image icon;

    TemporaryCard card;

    public Text infoText; // Prefab
    public Text info;

    public void OnPointerClick(PointerEventData pointerEventData) // player clicks on inventory slot
    {
        if (card != null) // slot is filled
        {
            // get card to mouse

            //ClearSlot();
        }
    }
    public void OnPointerMove(PointerEventData pointerEventData)
    {

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
