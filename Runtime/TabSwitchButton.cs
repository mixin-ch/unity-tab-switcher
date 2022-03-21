using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Mixin
{

    [Serializable]
    [RequireComponent(typeof(Button))]
    [ExecuteInEditMode]
    public class TabSwitchButton : MonoBehaviour
    {
        //[SerializeField] private string _name;
        [SerializeField] private GameObject _pageObject;

        [Space]
        [SerializeField] private bool _customColor = false;
        [SerializeField] private TabColors _tabColors;

        private Button _button;
        private bool _isActive;

        private TabSwitcher _tabSwitcher;
        private Color _activeColor;
        private Color _inactiveColor;

        [Header("Optional")]
        [SerializeField] private Image _buttonBackground;
        public UnityEvent OnTabSwitch;


        //public string Name { get => _name; }
        public GameObject PageObject { get => _pageObject; }
        public Button Button { get => _button; }
        public Image ButtonBackground { get => _buttonBackground; }

        public bool CustomColor { get => _customColor; }
        public TabColors TabColors { get => _tabColors; }
        public bool IsActive { get => _isActive; set => _isActive = value; }

        public void Setup()
        {
            _tabSwitcher = transform.parent.GetComponent<TabSwitcher>();

            _button = GetComponent<Button>();

            if (_buttonBackground == null)
                _buttonBackground = GetComponent<Image>();
        }

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

        public void SetColorAuto()
        {
            if (_isActive)
                ButtonBackground.color = _activeColor;
            else
                ButtonBackground.color = _inactiveColor;
        }

        private void OnValidate()
        {
            RefreshEditor();
        }

        private void RefreshEditor()
        {
            Setup();
            _tabSwitcher.RefreshEditor();
        }
    }
}