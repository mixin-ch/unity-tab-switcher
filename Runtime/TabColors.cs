using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Mixin
{
    [Serializable]
    public class TabColors
    {
        [SerializeField] private Color _activeColor = Color.white;
        [SerializeField] private Color _inactiveColor = new Color(1f, 1f, 1f, 0.7f);

        public Color ActiveColor { get => _activeColor; }
        public Color InactiveColor { get => _inactiveColor; }
    }
}