using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardDeckSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler,
     IPointerMoveHandler, IPointerDownHandler
{
    public Image icon;

    TemporaryCard card;

    public Text infoText; // Prefab
    public Text info;

    public GameObject miniMenu; // Prefab
    GameObject existingMiniMenu;

    bool cardActive;

    InventoryUI inventoryUI;

    void Start()
    {
        inventoryUI = gameObject.transform.GetComponentInParent<InventoryUI>();
        InventoryUI.inventoryClosed += DeactivateCard;
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (card != null)
            {
                existingMiniMenu = Instantiate(miniMenu, this.gameObject.transform);
                existingMiniMenu.transform.position += new Vector3(0, 50, 0);
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            if (card != null)
            {

                inventoryUI.ShowActiveCard(card, gameObject);

                cardActive = true;
            }
        }
    }

    public void OnPointerClick(PointerEventData pointerEventData) // player clicks on inventory slot
    {
        
        if (card != null)
        {
            //if (pointerEventData.clickTime <= 1)
            //inventoryUI.ShowActiveCard(card, gameObject, true);
            /*else */
            inventoryUI.ShowActiveCard(card, gameObject);

            cardActive = true;
        }
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
