#region Includes
using System;

using TMPro;

using UnityEngine;
using UnityEngine.UI;
#endregion

namespace TS.PageSlider.Demo
{
    public class SliderPage : MonoBehaviour
    {
        #region Variables

        [Header("Children")]
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _label;

        public string Text
        {
            get { return _label.text; }
            set { _label.text = value; }
        }

        public Sprite Image
        {
            get { return _image.sprite; }
            set { _image.sprite = value; }
        }

        #endregion
    }
}