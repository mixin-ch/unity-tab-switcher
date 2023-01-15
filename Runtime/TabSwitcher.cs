using Mixin.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Mixin.UI
{
    /// <summary>
    /// The main TabSwitcher class.
    /// Should be the parent of the TabSwitchButtons.
    /// </summary>
    [ExecuteInEditMode]
    public class TabSwitcher : MonoBehaviour
    {
        /// <summary>
        /// This will setup everything on awake.
        /// </summary>
        [Tooltip("This will setup everything on awake.")]
        [SerializeField] private bool _autoInit = true;

        /// <summary>
        /// Automatically add all buttons to the array when scene starts.
        /// </summary>
        [Tooltip("Automatically add all buttons to the array when scene starts.")]
        [SerializeField] private bool _autoAddButtons = true;

        /// <summary>
        /// Lets the user invoke a click on the page that he currently is.
        /// </summary>
        [Tooltip("Lets the user invoke a click on the page that he currently is.")]
        [SerializeField] private bool _allowClickActivePage = false;

        /// <summary>
        /// When enabled it will not change the page but still invokes a click.
        /// This is useful for dynamic content.
        /// </summary>
        [Tooltip("When enabled it will not change the page but still invokes a click. " +
            "This is useful for dynamic content.")]
        [SerializeField] private bool _ignorePageObjects = false;

        /// <summary>
        /// The colors of all tabs (does not overwrite the button's custom color).
        /// </summary>
        [Tooltip("The colors of all tabs (does not overwrite the button's custom color).")]
        [SerializeField] private TabColors _tabColors;

        /// <summary>
        /// Event when any tab is switched.
        /// </summary>
        [Tooltip("Event when any tab is switched.")]
        public UnityEvent<TabSwitchButton> OnAnyTabSwitched;

        /// <summary>
        /// List of all buttons. Fill this manually if you disabled 'AutoAddButtons'.
        /// </summary>
        [Header("Optional")]
        [Tooltip("List of all buttons. Fill this manually if you disabled 'AutoAddButtons'.")]
        [SerializeField] private List<TabSwitchButton> _tabSwitchButtonList;

        /// <summary>
        /// The page it should show when the game starts.
        /// </summary>
        [Tooltip("The page it should show when the game starts.")]
        [SerializeField] private TabSwitchButton _defaultActivePage;

        /// <summary>
        /// The current active page
        /// </summary>
        private TabSwitchButton _activePage;


        /********** API **********/

        /// <inheritdoc cref="_activePage"/>
        public TabSwitchButton ActivePage { get => _activePage; }


        private void Awake()
        {
            // if enabled, it will automatically setup on awake.
            if (_autoInit)
            {
                Setup();
            }
        }

        /// <summary>
        /// The main setup.
        /// Sets up everything.
        /// </summary>
        private void Setup()
        {
            if (_autoAddButtons)
            {
                // Clear the list
                _tabSwitchButtonList.Clear();

                // Add buttons to list
                TabSwitchButton[] foundButtons = transform.GetComponentsInChildren<TabSwitchButton>();
                foreach (TabSwitchButton button in foundButtons)
                {
                    _tabSwitchButtonList.Add(button);
                }
            }

            // If there are no Buttons, then return
            if (_tabSwitchButtonList.Count == 0)
            {
                "Setup failed: No Tab Switch Element added".LogWarning();
                return;
            }

            // Setup all buttons
            foreach (TabSwitchButton button in _tabSwitchButtonList)
            {
                button.Setup();

                // Add button listeners
                button.Button.onClick.AddListener(() =>
                {
                    SwitchToPage(button);
                });

                // Define the Colors
                button.DefineColors(_tabColors);
            }

            // Switch to the default page
            // If default is not set, it takes the first button
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

            // Disable all pages
            DeactivateAllPages();

            // Activate page
            if (!_ignorePageObjects)
                page.PageObject.SetActive(true);

            // Set current page active
            _activePage = page;
            page.IsActive = true;
            page.SetColorAuto();

            // Fire Event
            OnAnyTabSwitched?.Invoke(page);

            // Call methods from individual page
            page.OnThisTabSwitched?.Invoke();

            // Log
            $"Switched to page {page.Name}".Log(Color.yellow);
        }

        /// <summary>
        /// Deativates all pages.
        /// </summary>
        private void DeactivateAllPages()
        {
            foreach (TabSwitchButton page in _tabSwitchButtonList)
                DeactivatePage(page);
        }

        /// <summary>
        /// Deactivates a specific page.
        /// </summary>
        /// <param name="page">The page that should be deactivated.</param>
        private void DeactivatePage(TabSwitchButton page)
        {
            // Deactivate page
            if (!_ignorePageObjects)
                page.PageObject.SetActive(false);

            // Set color
            page.IsActive = false;
            page.SetColorAuto();
        }

        private void OnValidate()
        {
            RefreshEditor();
        }

        /// <summary>
        /// Will be executed every time a value is changed in the inspector.
        /// </summary>
        public void RefreshEditor()
        {
            Setup();
        }



    }
}