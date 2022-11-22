#region Includes
using UnityEngine;
using UnityEngine.UI;
#endregion

namespace TS.PageSlider.Demo
{
    public class SliderDot : MonoBehaviour
    {
        #region Variables

        [Header("Configuration")]
        [SerializeField] private Color _colorDefault;
        [SerializeField] private Color _colorSelected;

        private Image _image;
        private PageDot _dot;

        #endregion

        private void Awake()
        {
            _image = GetComponent<Image>();

            _dot = GetComponent<PageDot>();
            _dot.OnActiveStateChanged.AddListener(PageDot_ActiveStateChanged);
        }

        private void PageDot_ActiveStateChanged(bool active)
        {
            _image.color = active ? _colorSelected : _colorDefault;
        }
    }
}