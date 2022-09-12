using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Mixin
{
    [ExecuteInEditMode]
    public class TabSwitcher : MonoBehaviour
    {
        [Tooltip("This will setup everything on awake.")]
        [SerializeField] private bool _autoInit = true;

        [Tooltip("Automatically add all buttons to the array when scene starts.")]
        [SerializeField] private bool _autoAddButtons = true;

        [Tooltip("Lets the user invoke a click on the page that he currently is.")]
        [SerializeField] private bool _allowClickActivePage = true;

        [Tooltip("When enabled it will not change the page but still invokes a click. " +
            "This is useful for dynamic content.")]
        [SerializeField] private bool _ignorePageObjects = false;

        [Tooltip("The colors of all tabs (does not overwrite the button's custom color)")]
        [SerializeField] private TabColors _tabColors;

        [Tooltip("Event when the tab is switched")]
        public UnityEvent<TabSwitchButton> OnAnyTabSwitched;

        [Header("Optional")]
        [Tooltip("List of all buttons. Fill this manually if you disabled 'AutoAddButtons'.")]
        [SerializeField] private List<TabSwitchButton> _tabSwitchButtonList;

        [Tooltip("The page it should show when the game starts")]
        [SerializeField] private TabSwitchButton _defaultActivePage;

        private TabSwitchButton _activePage;

        public TabSwitchButton ActivePage { get => _activePage; }


        private void Awake()
        {
            if (_autoInit)
            {
                Setup();
            }
        }

        private void Setup()
        {
            if (_autoAddButtons)
            {
                _tabSwitchButtonList.Clear();

                TabSwitchButton[] foundButtons = transform.GetComponentsInChildren<TabSwitchButton>();
                foreach (TabSwitchButton button in foundButtons)
                {
                    _tabSwitchButtonList.Add(button);
                }
            }

            // If there are no Buttons, then return
            if (_tabSwitchButtonList.Count == 0)
            {
                "Setup failed: No Tab Switch Element added".Log();
                return;
            }

            foreach (TabSwitchButton button in _tabSwitchButtonList)
            {
                button.Setup();

                // add button listeners
                button.Button.onClick.AddListener(() =>
                {
                    SwitchToPage(button);
                });

                // Define the Colors
                button.DefineColors(_tabColors);
            }

            // switch to the default page
            // if default is not set, it takes the first button
            SwitchToPage(_defaultActivePage ?? _tabSwitchButtonList[0]);

        }

        public void SwitchToPage(TabSwitchButton page)
        {
            if (!_allowClickActivePage)
            {
                // If already on this Page, then return
                if (page == _activePage)
                    return;
            }

            $"Switching Page...".Log(Color.yellow);

            // disable all pages
            DeactivateAllPages();

            // activate page
            if (!_ignorePageObjects)
                page.PageObject.SetActive(true);

            // set current active page
            _activePage = page;
            page.IsActive = true;
            page.SetColorAuto();

            // Fire Event
            OnAnyTabSwitched?.Invoke(page);

            // call methods from individual page
            page.OnThisTabSwitched?.Invoke();
        }

        private void DeactivateAllPages()
        {
            foreach (TabSwitchButton page in _tabSwitchButtonList)
                DeactivatePage(page);
        }

        private void DeactivatePage(TabSwitchButton page)
        {
            //deactivate page
            if (!_ignorePageObjects)
                page.PageObject.SetActive(false);

            //set color
            page.IsActive = false;
            page.SetColorAuto();
        }

        private void OnValidate()
        {
            RefreshEditor();
        }

        public void RefreshEditor()
        {
            Setup();

            /*foreach (TabSwitchButton button in _tabSwitchButtonList)
            {
                button.Setup();

                // Define the Colors
                button.DefineColors(_tabColors);
            }

            foreach (TabSwitchButton page in _tabSwitchButtonList)
                page.SetColorAuto();*/
        }



    }
}