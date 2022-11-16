#region Includes
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System;
using UnityEngine.Events;
using System.Collections.Generic;
#endregion

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TS.PageViewer
{
    public class PageViewer : MonoBehaviour
    {
        #region Variables

        [Header("References")]
        [SerializeField] private PageDotsIndicator _dotsIndicator;

        [Header("Children")]
        [SerializeField] private List<PageView> _pages;

        [Header("Events")]
        public UnityEvent<PageView> OnPageChanged;

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

        public void AddPage(RectTransform content)
        {
#if UNITY_EDITOR
            if (_scroller == null)
            {
                _scroller = FindScroller();
            }
#endif

            var page = new GameObject(string.Format("Page_{0}", _pages.Count), typeof(RectTransform), typeof(PageView));
            page.transform.SetParent(_scroller.Content);

            var rectTransform = page.GetComponent<RectTransform>();
            rectTransform.sizeDelta = _scroller.Rect.size;
            rectTransform.localScale = Vector3.one;

            var pageView = page.GetComponent<PageView>();
            pageView.AssignContent(content);

            _pages.Add(pageView);

            if (_dotsIndicator != null)
            {
                _dotsIndicator.Add();
                _dotsIndicator.IsVisible = _pages.Count > 1;
            }

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
        public void Clear()
        {
            for (int i = 0; i < _pages.Count; i++)
            {
#if UNITY_EDITOR
                DestroyImmediate(_pages[i].gameObject);
#else
            Destroy(_pages[i].gameObject);
#endif
            }
            _pages.Clear();

            _dotsIndicator?.Clear();
        }

        private void PageScroller_PageChangeStarted(int fromIndex, int toIndex)
        {
            _pages[fromIndex].ChangingToInactiveState();
            _pages[toIndex].ChangingToActiveState();
        }
        private void PageScroller_PageChangeEnded(int fromIndex, int toIndex)
        {
            _pages[fromIndex].ChangeActiveState(false);
            _pages[toIndex].ChangeActiveState(true);

            _dotsIndicator?.ChangeActiveDot(fromIndex, toIndex);

            OnPageChanged?.Invoke(_pages[toIndex]);
        }

        private PageScroller FindScroller()
        {
            var scroller = GetComponentInChildren<PageScroller>();

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (scroller == null)
            {
                Debug.LogError("Missing PageScroller in Children");
            }

            return scroller;
#endif
        }

#if UNITY_EDITOR

        [CustomEditor(typeof(PageViewer))]
        public class PageControllerEditor : Editor
        {
            #region Variables

            private PageViewer _target;

            #endregion

            private void OnEnable()
            {
                _target = (PageViewer)target;
            }
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Editor");

                if (GUILayout.Button("Add Page"))
                {
                    _target.AddPage(null);
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