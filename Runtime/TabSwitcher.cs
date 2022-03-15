using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Mixin
{
    public class TabSwitcher : MonoBehaviour
    {
        [Header("Colors")]
        [SerializeField] private Color ActiveColor = Color.white;
        [SerializeField] private Color InactiveColor = new Color(1f, 1f, 1f, 0.7f);

        [SerializeField] private bool IgnorePageObjects = false;

        [Space]
        [SerializeField] private List<TabSwitchElement> TabSwitchElementList;

        [Space]
        private List<Page> PageList;

        // Obsolete
        public UnityEvent<Page> OnTabSwitch;

        public static event UnityAction<Page> OnTabSwitched;

        //[ReadOnly]
        private Page ActivePage;

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

            //$"Switching to Page {page.Name}".Log(Color.yellow);

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
        [SerializeField] private string _name;
        [SerializeField] private GameObject _pageObject;

        [Space]
        [SerializeField] private Button _buttonForSwitch;

        [Space]
        [SerializeField] private Image _backgroundToHighlight;

        [Header("Colors")]
        [SerializeField] private bool _customColor = false;
        [SerializeField] private Color _activeColor = Color.white;
        [SerializeField] private Color _inactiveColor = new Color(1f, 1f, 1f, 0.3f);

        [Header("Optional")]
        public UnityEvent OnTabSwitch;


        public string Name { get => _name; }
        public GameObject PageObject { get => _pageObject; }
        public Button ButtonForSwitch { get => _buttonForSwitch; }
        public Image BackgroundToHighlight { get => _backgroundToHighlight; }

        public bool CustomColor { get => _customColor; }
        public Color ActiveColor { get => _activeColor; }
        public Color InactiveColor { get => _inactiveColor; }
    }
}