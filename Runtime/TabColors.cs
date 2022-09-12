using System;
using UnityEngine;

namespace Mixin.UI
{
    /// <summary>
    /// Color Manager for the Tabs
    /// </summary>
    [Serializable]
    public class TabColors
    {
        /// <summary>
        /// Color when the tab is active
        /// </summary>
        [SerializeField] private Color _activeColor = Color.white;

        /// <summary>
        /// Color when the tab is inactive
        /// </summary>
        [SerializeField] private Color _inactiveColor = new Color(1f, 1f, 1f, 0.7f);

        /// <inheritdoc cref="_activeColor"/>
        public Color ActiveColor { get => _activeColor; }

        /// <inheritdoc cref="_inactiveColor"/>
        public Color InactiveColor { get => _inactiveColor; }
    }
}