#region Includes
using UnityEngine;
using UnityEngine.Events;
#endregion

namespace TS.PageViewer
{
    public class PageView : MonoBehaviour
    {
        #region Variables

        private PageViewContent _content;

        #endregion

        public void AssignContent(RectTransform content)
        {
            if (content == null) { return; }

            content.SetParent(transform);

            content.anchorMin = Vector2.zero;
            content.anchorMax = Vector2.one;
            content.offsetMin = Vector2.zero;
            content.offsetMax = Vector2.zero;
            content.anchoredPosition = Vector2.zero;

            content.localScale = Vector3.one;

            _content = content.GetComponent<PageViewContent>();
        }

        public void ChangingToActiveState()
        {
            _content?.ChangingToActiveState();
        }
        public void ChangingToInactiveState()
        {
            _content?.ChangingToInactiveState();
        }
        public void ChangeActiveState(bool active)
        {
            _content?.ChangeActiveState(active);
        }
    }

}