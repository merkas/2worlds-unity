using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardDeckSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image icon;

    CompleteCard comCard;
    TemporaryCard card;

    public Text infoText; // Prefab
    public Text info;

    bool cardActive;

    bool doubleClickedCard;
    int clicked = 0;
    float clickTime;
    float clickDelay = 0.5f;

    InventoryUI inventoryUI;

    void Start()
    {
        inventoryUI = gameObject.transform.GetComponentInParent<InventoryUI>();
        InventoryUI.inventoryClosed += DeactivateCard;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (card != null)
        {
            if (Time.time - clickTime > clickDelay)
            {
                clicked = 0;
                doubleClickedCard = false;
            }
            clicked++;
            if (clicked == 1) clickTime = Time.time;
            else if (clicked > 1 && Time.time - clickTime < clickDelay)
            {
                clicked = 0;
                clickTime = 0;
                doubleClickedCard = true;
                // highlight slot somehow
            }

            if (doubleClickedCard != true) inventoryUI.ShowActiveCard(comCard, gameObject);
            else inventoryUI.ShowActiveCard(comCard, gameObject, true);
            //doubleClickedCard = false;
            cardActive = true;
        }
        else inventoryUI.ResetDoubleClickedObjects();
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

    public void AddCard(CompleteCard newCard)
    {
        comCard = newCard;
        card = newCard.card;
        icon.sprite = card.cardIcon;
        icon.enabled = true;
        if (info != null) info.text = card.cardName;
    }

    public void ClearSlot()
    {
        CardActive(false);

        comCard = null;
        card = null;
        icon.sprite = null;
        icon.enabled = false;
    }
}
