using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler,
     IPointerMoveHandler, IPointerDownHandler
{
    public Image icon;
    TemporaryCard card;

    public Text stackNumber;
    public Text infoText; // Prefab
    public Text info;

    public GameObject miniMenu; // Prefab
    GameObject existingMiniMenu;

    int clicked = 0;

    //bool cardOnMove;
    bool cardActive;
    bool doubleClickedCard;

    int numberOfCards;

    InventoryUI inventoryUI;

    void Start()
    {
        inventoryUI = gameObject.transform.GetComponentInParent<InventoryUI>();
        InventoryUI.movedObject += ResendActiveCardData;
        InventoryUI.inventoryClosed += DeactivateCard;
    }

    public bool CardActive(bool active)
    {
        cardActive = active;
        return cardActive;
    }

    void DeactivateCard()
    {
        cardActive = false;
    }

    void ResendActiveCardData()
    {
        //if (cardActive == true) inventoryUI.ShowActiveCard(card, gameObject);
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (card != null)
            {
                Debug.Log("Pointer Down");
                existingMiniMenu = Instantiate(miniMenu, this.gameObject.transform);
                existingMiniMenu.transform.position += new Vector3(0, 50, 0);
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            if (card != null)
            {
                Debug.Log("Pointer Click");
                
                inventoryUI.ShowActiveCard(card, gameObject);

                cardActive = true;
            }
        }
    }

    public void OnPointerClick(PointerEventData pointerEventData) // player clicks on slot
    {
        //Debug.Log(pointerEventData.clickTime + ", " + Time.time);
        //if (existingMiniMenu != null) Destroy(existingMiniMenu);

        //if (card != null)
        //{
        //    Debug.Log("Pointer Click");
        //    //if (pointerEventData.clickTime <= 1)
        //        //inventoryUI.ShowActiveCard(card, gameObject, true);
        //    /*else */
        //    inventoryUI.ShowActiveCard(card, gameObject);

        //    cardActive = true;
        //}





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
        stackNumber.enabled = false;
        //if (existingMiniMenu != null) Destroy(existingMiniMenu);
        CardActive(false);
    }

    public void AddToStack(int added)
    {
        numberOfCards += added;
        if (numberOfCards > 1)
        {
            stackNumber.text = numberOfCards.ToString();
            stackNumber.enabled = true;
        }
    }
}
