using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image icon;
    TemporaryCard card;
    CompleteCard comCard;

    public Text stackNumber;
    public Text infoText; // Prefab
    public Text info;

    bool cardActive;

    bool doubleClickedCard;
    int clicked = 0;
    float clickTime;
    float clickDelay = 0.5f;

    int numberOfCards;

    InventoryUI inventoryUI;

    void Start()
    {
        inventoryUI = gameObject.transform.GetComponentInParent<InventoryUI>();
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

    public void OnPointerClick(PointerEventData pointerEventData) // player clicks on slot
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

            cardActive = true;
        }
        else inventoryUI.ResetDoubleClickedObjects();
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
        numberOfCards = newCard.amount;
        icon.sprite = card.cardIcon;
        icon.enabled = true;
        if (info != null) info.text = card.cardName;
    }

    public void ClearSlot()
    {
        CardActive(false);

        comCard = null;
        card = null;
        numberOfCards = 0;
        icon.sprite = null;
        icon.enabled = false;
        stackNumber.enabled = false;
    }

    public void UpdateStack(int newAmount)
    {
        numberOfCards = newAmount;
        if (numberOfCards > 1)
        {
            stackNumber.text = numberOfCards.ToString();
            stackNumber.enabled = true;
        }
        else stackNumber.enabled = false;
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
