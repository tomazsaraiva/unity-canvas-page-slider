#region Includes
using System.Collections;

using TMPro;

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
#endregion

namespace TS.PageSlider.Demo
{
    public class SliderPageLazy : MonoBehaviour
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

        public string ImageUrl { get; set; }

        private PageView _pageView;

        #endregion

        private void Awake()
        {
            _pageView = GetComponent<PageView>();
            _pageView.OnChangingToActiveState.AddListener(PageView_ChangingToActiveState);
        }

        private void PageView_ChangingToActiveState()
        {
            if(_image.sprite != null) { return; }

            StartCoroutine(GetImageRoutine(ImageUrl));
        }

        private IEnumerator GetImageRoutine(string uri)
        {
            var request = UnityWebRequestTexture.GetTexture(uri);
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
                yield break;
            }

            var texture = ((DownloadHandlerTexture)request.downloadHandler).texture;

            _image.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
            _image.color = Color.white;
            _image.preserveAspect = true;
        }
    }
}