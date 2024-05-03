#region Includes
using UnityEngine;
#endregion

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TS.PageSlider
{
    /// <summary>
    /// This class represents a container for a page in a paginated view.
    /// It handles assigning content to the container and manages the active state of the contained page.
    /// </summary>
    public class PageContainer : MonoBehaviour
    {
        #region Variables

        [Header("Children")]

        // <summary>
        /// The PageView component representing the content of this page container.
        /// </summary>
        [Tooltip("The PageView component representing the content of this page container")]
        [SerializeField] private PageView _page;

        #endregion

        /// <summary>
        /// Assigns content (RectTransform) to this container.
        /// If no content is provided, it creates a new GameObject with a RectTransform and a PageView component.
        /// The assigned content is then parented to this container and its properties are set to ensure proper positioning and scaling.
        /// </summary>
        /// <param name="content">The RectTransform representing the content to be assigned.</param>
        public void AssignContent(RectTransform content)
        {
            if (content == null)
            {
                // Create a new GameObject with required components if content is not provided.
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

        /// <summary>
        /// Calls the ChangingToActiveState method on the contained PageView component,
        /// to signal a transition to an active state.
        /// </summary>
        public void ChangingToActiveState()
        {
            _page.ChangingToActiveState();
        }

        /// <summary>
        /// Calls the ChangingToInactiveState method on the contained PageView component,
        /// to signal a transition to an inactive state.
        /// </summary>
        public void ChangingToInactiveState()
        {
            _page.ChangingToInactiveState();
        }

        /// <summary>
        /// Calls the ChangeActiveState method on the contained PageView component with the provided active state.
        /// </summary>
        /// <param name="active">True to set the page to active, False to set it to inactive.</param>
        public void ChangeActiveState(bool active)
        {
            _page.ChangeActiveState(active);
        }
    }

}