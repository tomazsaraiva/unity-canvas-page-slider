#region Includes
using UnityEditor;

using UnityEngine;
using UnityEngine.Events;
#endregion

namespace TS.PageSlider
{
    public class PageContainer : MonoBehaviour
    {
        #region Variables

        [Header("Children")]
        [SerializeField] private PageView _page;

        #endregion

        public void AssignContent(RectTransform content)
        {
            if (content == null)
            {
                var contentObject = new GameObject("Content", typeof(RectTransform), typeof(PageView));
                content = contentObject.GetComponent<RectTransform>();
            }

            content.SetParent(transform);

            content.anchorMin = Vector2.zero;
            content.anchorMax = Vector2.one;
            content.offsetMin = Vector2.zero;
            content.offsetMax = Vector2.zero;
            content.anchoredPosition = Vector2.zero;

            content.localScale = Vector3.one;

            _page = content.GetComponent<PageView>();

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

        public void ChangingToActiveState()
        {
            _page.ChangingToActiveState();
        }
        public void ChangingToInactiveState()
        {
            _page.ChangingToInactiveState();
        }
        public void ChangeActiveState(bool active)
        {
            _page.ChangeActiveState(active);
        }
    }

}