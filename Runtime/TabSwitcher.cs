using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TabSwitcher : MonoBehaviour
{
    [Header("Colors")]
    public Color ActiveColor = new Color(1f, 1f, 1f, 1f);
    public Color InactiveColor = new Color(1f, 1f, 1f, 0.7f);

    public bool IgnorePageObjects = false;

    [Space]
    public List<TabSwitchElement> TabSwitchElementList;

    [Space]
    public List<Page> PageList;

    // Obsolete
    public UnityEvent<Page> OnTabSwitch;

    public static event UnityAction<Page> OnTabSwitched;

    [ReadOnly]
    public Page ActivePage;
 
    private void Awake()
    {
        Setup();
    }

    private void Setup()
    {
        // if he uses the TabSwitchElement, then add these pages to PageList
        if (TabSwitchElementList != null)
            foreach (TabSwitchElement tabSwitchElement in TabSwitchElementList)
                PageList.Add(tabSwitchElement.Page);

        // check if user wants to use the all in one feature
        if (PageList != null)
        {
            foreach (Page page in PageList)
            {
                // add button listeners
                page.ButtonForSwitch.onClick.AddListener(() =>
                {
                    SwitchToPage(page);
                });

            }
            // activate first page
            SwitchToPage(PageList[0]);
        }
    }

    public void SwitchToPage(Page page)
    {
        //if (page == ActivePage)
        //    return;

        $"Switching to Page {page.Name}".Log(Color.yellow);

        // disable all pages
        DeactivateAllPages();

        // activate page
        if (!IgnorePageObjects)
            page.PageObject.SetActive(true);

        // set color
        page.BackgroundToHighlight.color = ActiveColor;
        if (page.CustomColor)
            page.BackgroundToHighlight.color = page.ActiveColor;

        // set current active page
        ActivePage = page;

        // call general on tab switch methods
        OnTabSwitch?.Invoke(page);

        // Fire Event
        OnTabSwitched?.Invoke(page);

        // call methods from individual page
        page.OnTabSwitch?.Invoke();
    }

    private void DeactivatePage(Page page)
    {
        //deactivate page
        if (!IgnorePageObjects)
            page.PageObject.SetActive(false);

        //set color
        page.BackgroundToHighlight.color = InactiveColor;
        if (page.CustomColor)
            page.BackgroundToHighlight.color = page.InactiveColor;
    }

    private void DeactivateAllPages()
    {
        foreach (Page page in PageList)
            DeactivatePage(page);
    }

    private void OnDestroy()
    {
        foreach (Page page in PageList)
            page.ButtonForSwitch.onClick.RemoveAllListeners();
    }

}

[System.Serializable]
public class Page
{
    public string Name;
    public GameObject PageObject;
    [Space]
    public Button ButtonForSwitch;
    [Space]
    public Image BackgroundToHighlight;

    [Header("Optional")]
    public UnityEvent OnTabSwitch;
    //public TMP_Text ButtonText;

    [Header("Colors")]
    public bool CustomColor = false;
    public UnityEngine.Color ActiveColor = new UnityEngine.Color(1f, 1f, 1f, 1f);
    public UnityEngine.Color InactiveColor = new UnityEngine.Color(1f, 1f, 1f, 0.3f);
}