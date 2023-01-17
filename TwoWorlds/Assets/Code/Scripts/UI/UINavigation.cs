using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UINavigation : MonoBehaviour
{
    public Button[] tabButtons;
    public GameObject[] tabs;

    void Start()
    {
        foreach (GameObject tab in tabs)
        {
            tab.SetActive(false);
        }
        TabButtons(false);
    }
    public void ChangeSceneButton() // for testing
    {
        SceneManager.LoadScene("DialogueTest");
    }

    void Update() // add to input manager?
    {
        if (Input.GetKeyDown(KeyCode.I)) // inventory window
        {
            if (tabs[0].activeSelf == false)
            {
                ActivateTab1();
            }
            else
            {
                tabs[0].SetActive(false);
                TabButtons(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.K)) // card inventory window
        {
            if (tabs[1].activeSelf == false)
            {
                ActivateTab2();
            }
            else
            {
                tabs[1].SetActive(false);
                TabButtons(false);
            }
        }
    }

    void HideTabs() // always call before activating a tab, hides all tabs, but not buttons
    {
        foreach (GameObject tab in tabs)
        {
            tab.SetActive(false);
        }
        foreach (Button button in tabButtons)
        {
            button.GetComponent<Image>().color = new Color32(0, 50, 80, 255); // dark blueish grey, change component if needed
            button.enabled = true;
        }
    }

    void TabButtons(bool activate)
    {
        if (activate == true)
        {
            foreach (Button tabButton in tabButtons)
            {
                tabButton.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (Button tabButton in tabButtons)
            {
                tabButton.gameObject.SetActive(false);
            }
        }
    }

    void ActivateNewTab(int index)
    {
        if (tabs[index] == null)
        {
            Debug.Log("Index of tab not existing");
            return;
        }

        HideTabs();
        TabButtons(true);
        tabs[index].SetActive(true);
        tabButtons[index].GetComponent<Image>().color = new Color32(0, 75, 120, 255);
        tabButtons[index].enabled = false;
    }

    public void ActivateTab1() // Button
    {
        ActivateNewTab(0);
    }
    public void ActivateTab2() // Button
    {
        ActivateNewTab(1);
    }

    public void CloseAllTabs() // Button
    {
        foreach (GameObject tab in tabs)
        {
            tab.SetActive(false);
        }
        TabButtons(false);
        gameObject.GetComponent<InventoryUI>().CloseInventory();
    }
}
