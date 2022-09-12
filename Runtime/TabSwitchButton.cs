using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

namespace Mixin
{
    [Serializable]
    [RequireComponent(typeof(Button))]
    [ExecuteInEditMode]
    public class TabSwitchButton : MonoBehaviour
    {
        [Tooltip("Automatically set the button text")]
        [SerializeField] public bool _setButtonText = true;

        [Tooltip("The button text field")]
        [ConditionalField("_setButtonText", true)]
        [SerializeField] private TMP_Text _buttonText;

        [Tooltip("This uses the gameobject's name for the button text")]
        [ConditionalField("_setButtonText", true)]
        [SerializeField] private bool _useGameobjectName = true;

        [Tooltip("Set custom name. This is only used when useGameobjectName is false")]
        [ConditionalField("_useGameobjectName", false)]
        [ConditionalField("_setButtonText", true)]
        [Multiline]
        [SerializeField] private string _customName;

        /*****************************************/

        [Space]
        [Tooltip("The gameobject which should be shown when tab is active")]
        [SerializeField] private GameObject _pageObject;

        [Space]
        [Tooltip("Overwrites the TabSwitcher's colors")]
        [SerializeField] private bool _customColor = false;

        [SerializeField] private TabColors _tabColors;

        /*****************************************/

        [Header("Optional")]
        [SerializeField] private Image _buttonBackground;
        public UnityEvent OnThisTabSwitched;

        /*****************************************/

        private Button _button;
        private bool _isActive;

        private TabSwitcher _tabSwitcher;
        private Color _activeColor;
        private Color _inactiveColor;


        //public string Name { get => _name; }
        public GameObject PageObject { get => _pageObject; }
        public Button Button { get => _button; }
        public Image ButtonBackground { get => _buttonBackground; }

        public bool CustomColor { get => _customColor; }
        public TabColors TabColors { get => _tabColors; }
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
        public bool IsActive { get => _isActive; set => _isActive = value; }

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