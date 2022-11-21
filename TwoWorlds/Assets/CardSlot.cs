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

    bool cardOnMove;
    bool cardActive;
    bool doubleClickedCard;

    InventoryUI inventoryUI;

    void Start()
    {
        inventoryUI = gameObject.transform.GetComponentInParent<InventoryUI>();
        InventoryUI.movedObject += ResendActiveCardData;
    }

    public bool CardActive(bool active)
    {
        cardActive = active;
        return cardActive;
    }

    void ResendActiveCardData()
    {
        //if (cardActive == true) inventoryUI.ShowActiveCard(card, gameObject);
    }

    public void OnPointerClick(PointerEventData pointerEventData) // player clicks on slot
    {
        //Debug.Log(pointerEventData.clickTime + ", " + Time.time);
        
        if (card != null)
        {
            //if (pointerEventData.clickTime <= 1)
                //inventoryUI.ShowActiveCard(card, gameObject, true);
            /*else */
            inventoryUI.ShowActiveCard(card, gameObject);

            cardActive = true;
        }





        //if (card != null) // slot filled
        //{
        //    // move card to mouse
        //    cardOnMove = true;
        //    cardActive = true;
        //}
        //if (cardOnMove == true) // slot empty and card at mouse
        //{
        //    Debug.Log("Card on mouse, clicked on other object");
        //    if (pointerEventData.selectedObject.GetComponent<CardSlot>() != null)
        //    {
        //        pointerEventData.selectedObject.GetComponent<CardSlot>().AddCard(card);

        //    }
        //    else if (pointerEventData.selectedObject.GetComponent<CardDeckSlot>() != null)
        //    {
        //        pointerEventData.selectedObject.GetComponent<CardDeckSlot>().AddCard(card);

        //    }
        //    cardOnMove = false;
        //    ClearSlot();
        //}
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
            info.text = card.cardName;
            info.enabled = true;
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (card != null)
        {
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
        CardActive(false);
    }
    
}
