#region Includes
using System;

using UnityEngine;
#endregion

namespace TS.PageSlider.Demo
{
    public class PageSliderDemoLazy : MonoBehaviour
    {
        #region Variables

        [Header("References")]
        [SerializeField] private PageSlider _slider;
        [SerializeField] private SliderPageLazy _pagePrefab;

        [Header("Configuration")]
        [SerializeField] private SliderItemLazy[] _items;

        #endregion

        private void Start()
        {
            for (int i = 0; i < _items.Length; i++)
            {
                var page = Instantiate(_pagePrefab);
                page.Text = _items[i].Text;
                page.ImageUrl = _items[i].ImageUrl;

                _slider.AddPage((RectTransform)page.transform);
            }
        }
    }

    [Serializable]
    public class SliderItemLazy
    {
        [SerializeField] private string _text;
        [SerializeField] private string _imageUrl;

        public string Text { get { return _text; } }
        public string ImageUrl { get { return _imageUrl; } }
    }
}
