#region Includes
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

        public Sprite Image
        {
            get { return _image.sprite; }
            set { _image.sprite = value; }
        }

        #endregion
    }
}