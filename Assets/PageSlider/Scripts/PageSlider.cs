#region Includes
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
#endregion

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TS.PageSlider
{
    /// <summary>
    /// The PageSlider class manages a collection of pages within a PageScroller component. 
    /// It provides functionality for adding, removing, and keeping track of pages, 
    /// as well as handling page change events.
    /// </summary>
    public class PageSlider : MonoBehaviour
    {
        #region Variables

        [Header("References")]

        /// <summary>
        /// "An optional reference to a PageDotsIndicator component used to display dots for each page."
        /// </summary>
        [Tooltip("Optional reference to a PageDotsIndicator to display dots for each page")]
        [SerializeField] private PageDotsIndicator _dotsIndicator;

        [Header("Children")]

        /// <summary>
        /// A list of PageContainer components representing the pages managed by the PageSlider.
        /// </summary>
        [Tooltip("A list of PageContainer components representing the pages managed by the PageSlider")]
        [SerializeField] private List<PageContainer> _pages;

        [Header("Events")]

        /// <summary>
        /// Invoked whenever the active page changes. 
        /// The event argument is a reference to the new active page.
        /// </summary>
        public UnityEvent<PageContainer> OnPageChanged;

        /// <summary>
        /// Gets the rectangle of the PageSlider component.
        /// </summary>
        public Rect Rect { get { return ((RectTransform)transform).rect; } }

        private PageScroller _scroller;

        #endregion

        private void Awake()
        {
            _scroller = FindScroller();
        }
        private void Start()
        {
            _scroller.OnPageChangeStarted.AddListener(PageScroller_PageChangeStarted);
            _scroller.OnPageChangeEnded.AddListener(PageScroller_PageChangeEnded);
        }


        /// <summary>
        /// Adds a new page to the PageSlider. 
        /// The content argument specifies the RectTransform of the content to be displayed on the new page.
        /// </summary>
        /// <param name="content">The RectTransform of the content to be displayed on the new page.</param>
        public void AddPage(RectTransform content)
        {
            if (_scroller == null)
            {
                _scroller = FindScroller();
            }

            _pages ??= new List<PageContainer>();

            var page = new GameObject(string.Format("Page_{0}", _pages.Count), typeof(RectTransform), typeof(PageContainer));
            page.transform.SetParent(_scroller.Content);

            var rectTransform = page.GetComponent<RectTransform>();
            rectTransform.sizeDelta = _scroller.Rect.size;
            rectTransform.localScale = Vector3.one;

            var pageView = page.GetComponent<PageContainer>();
            pageView.AssignContent(content);

            if (_pages.Count == 0)
            {
                pageView.ChangingToActiveState();
                pageView.ChangeActiveState(true);
            }

            _pages.Add(pageView);

            if (_dotsIndicator != null)
            {
                _dotsIndicator.Add();
                _dotsIndicator.IsVisible = _pages.Count > 1;
            }

#if UNITY_EDITOR
            if (Application.isPlaying) { return; }
            EditorUtility.SetDirty(this);
#endif
        }

        /// <summary>
        /// Removes all pages from the PageSlider and clears the associated PageDotsIndicator (if exists).
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < _pages.Count; i++)
            {
                if (_pages[i] == null) { continue; }
#if UNITY_EDITOR
                DestroyImmediate(_pages[i].gameObject);
#else
                Destroy(_pages[i].gameObject);
#endif
            }
            _pages.Clear();

            if (_dotsIndicator != null)
            {
                _dotsIndicator.Clear();
            }
        }


        /// <summary>
        /// Called by the PageScroller component when a page change starts. 
        /// Deactivates the page at the fromIndex and activates the page at the toIndex.
        /// </summary>
        /// <param name="fromIndex">The index of the page that is being deactivated.</param>
        /// <param name="toIndex">The index of the page that is being activated.</param>
        private void PageScroller_PageChangeStarted(int fromIndex, int toIndex)
        {
            _pages[fromIndex].ChangingToInactiveState();
            _pages[toIndex].ChangingToActiveState();
        }

        /// <summary>
        /// Called by the PageScroller component when a page change ends. Sets the page at the fromIndex to inactive and the page at the toIndex to active. Updates the PageDotsIndicator and invokes the OnPageChanged event.
        /// </summary>
        /// <param name="fromIndex">The index of the page that is being deactivated.</param>
        /// <param name="toIndex">The index of the page that is being activated.</param>
        private void PageScroller_PageChangeEnded(int fromIndex, int toIndex)
        {
            _pages[fromIndex].ChangeActiveState(false);
            _pages[toIndex].ChangeActiveState(true);

            if (_dotsIndicator != null)
            {
                _dotsIndicator.ChangeActiveDot(fromIndex, toIndex);
            }

            OnPageChanged?.Invoke(_pages[toIndex]);
        }

        /// <summary>
        /// Finds the PageScroller component within the children of the gameobject this script is attached to. 
        /// </summary>
        /// <returns>The PageScroller component found in the children, or null if not found.</returns>
        private PageScroller FindScroller()
        {
            var scroller = GetComponentInChildren<PageScroller>();

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (scroller == null)
            {
                Debug.LogError("Missing PageScroller in Children");
            }
#endif
            return scroller;
        }

#if UNITY_EDITOR

        [CustomEditor(typeof(PageSlider))]
        public class PageControllerEditor : Editor
        {
            #region Variables

            private PageSlider _target;
            private RectTransform _contentPrefab;

            #endregion

            private void OnEnable()
            {
                _target = (PageSlider)target;
            }
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Editor");

                _contentPrefab = (RectTransform)EditorGUILayout.ObjectField(_contentPrefab, typeof(RectTransform), false);
                if (GUILayout.Button("Add Page"))
                {
                    _target.AddPage((RectTransform)PrefabUtility.InstantiatePrefab(_contentPrefab));
                }
                if (GUILayout.Button("Clear"))
                {
                    _target.Clear();
                }
            }
        }
#endif
    }

}