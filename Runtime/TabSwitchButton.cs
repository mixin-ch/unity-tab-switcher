using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using Mixin.Utils;

namespace Mixin.UI
{
    /// <summary>
    /// The child object(s) of the TabSwitcher.
    /// </summary>
    [Serializable]
    [RequireComponent(typeof(Button))]
    [ExecuteInEditMode]
    public class TabSwitchButton : MonoBehaviour
    {
        /// <summary>
        /// Automatically set the button text.
        /// </summary>
        [Tooltip("Automatically set the button text.")]
        [SerializeField] public bool _setButtonText = true;

        /// <summary>
        /// The button text field.
        /// </summary>
        [Tooltip("The button text field.")]
        [ConditionalField("_setButtonText", true)]
        [SerializeField] private TMP_Text _buttonText;

        /// <summary>
        /// This uses the gameobject's name for the button text.
        /// </summary>
        [Tooltip("This uses the gameobject's name for the button text.")]
        [ConditionalField("_setButtonText", true)]
        [SerializeField] private bool _useGameobjectName = true;

        /// <summary>
        /// Set custom name. This is only used when useGameobjectName is false.
        /// </summary>
        [Tooltip("Set custom name. This is only used when useGameobjectName is false.")]
        [ConditionalField("_useGameobjectName", false)]
        [ConditionalField("_setButtonText", true)]
        [Multiline]
        [SerializeField] private string _customName;

        /*****************************************/

        /// <summary>
        /// The gameobject which should be shown when tab is active.
        /// </summary>
        [Space]
        [Tooltip("The gameobject which should be shown when tab is active.")]
        [SerializeField] private GameObject _pageObject;

        /// <summary>
        /// Overwrites the TabSwitcher's colors.
        /// </summary>
        [Space]
        [Tooltip("Overwrites the TabSwitcher's colors.")]
        [SerializeField] private bool _customColor = false;

        /// <inheritdoc cref="Mixin.UI.TabColors"/>
        [SerializeField] private TabColors _tabColors;

        /*****************************************/

        /// <summary>
        /// The background of the button.
        /// </summary>
        [Header("Optional")]
        [SerializeField] private Image _buttonBackground;

        /// <summary>
        /// This is executed when it switches on this tab.
        /// </summary>
        public UnityEvent OnThisTabSwitched;

        /*****************************************/

        /// <summary>
        /// The button.
        /// </summary>
        private Button _button;

        /// <summary>
        /// Tells if the page is active or not.
        /// </summary>
        private bool _isActive;

        /// <summary>
        /// The parent TabSwitcher.
        /// </summary>
        private TabSwitcher _tabSwitcher;

        /// <inheritdoc cref="TabColors._activeColor"/>
        private Color _activeColor;

        /// <inheritdoc cref="TabColors._inactiveColor"/>
        private Color _inactiveColor;


        /********* API *********/

        /// <summary>
        /// The name that is used for the text field.
        /// </summary>
        public string Name
        {
            get
            {
                if (_useGameobjectName)
                {
                    return gameObject.name;
                }
                else
                {
                    return _customName;
                }
            }
        }

        /// <inheritdoc cref="_button"/>
        public Button Button { get => _button; }

        /// <inheritdoc cref="_buttonText"/>
        public TMP_Text ButtonText { get => _buttonText; set => _buttonText = value; }

        /// <inheritdoc cref="_buttonBackground"/>
        public Image ButtonBackground { get => _buttonBackground; }

        /// <inheritdoc cref="_pageObject"/>
        public GameObject PageObject { get => _pageObject; }

        /// <inheritdoc cref="_customColor"/>
        public bool CustomColor { get => _customColor; }

        /// <inheritdoc cref="_tabColors"/>
        public TabColors TabColors { get => _tabColors; set => _tabColors = value; }

        /// <inheritdoc cref="_isActive"/>
        public bool IsActive { get => _isActive; set => _isActive = value; }

        /// <summary>
        /// Sets up this button.
        /// </summary>
        public void Setup()
        {
            try
            {
                // Try to set the button text
                if (_setButtonText)
                    _buttonText.text = Name;
            }
            catch (Exception e)
            {
                $"{Name}: {e}".LogWarning();
            }

            // Get TabSwitcher and Button
            _tabSwitcher = transform.parent.GetComponent<TabSwitcher>();
            _button = GetComponent<Button>();

            // If background is not set, try to get image component
            try
            {
                if (_buttonBackground == null)
                    _buttonBackground = GetComponent<Image>();
            }
            catch (Exception e)
            {
                $"{Name}: {e}".LogWarning();
            }
        }

        /// <summary>
        /// Defines the color
        /// </summary>
        /// <param name="parentColors">The defined color in the TabSwitcher parent</param>
        public void DefineColors(TabColors parentColors)
        {
            TabColors colors;

            if (_customColor)
                colors = _tabColors;
            else
                colors = parentColors;

            _activeColor = colors.ActiveColor;
            _inactiveColor = colors.InactiveColor;
        }

        /// <summary>
        /// Applies the background color
        /// </summary>
        public void SetColorAuto()
        {
            if (_isActive)
                ButtonBackground.color = _activeColor;
            else
                ButtonBackground.color = _inactiveColor;
        }

        /// <summary>
        /// Execute setup when something changed
        /// </summary>
        private void OnValidate()
        {
            _tabSwitcher.RefreshEditor();
        }
    }
}