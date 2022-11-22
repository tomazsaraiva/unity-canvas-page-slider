#region Includes
using System.Collections;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#endregion

namespace TS.PageSlider
{
    public class PageScroller : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        #region Variables

        [Header("Configuration")]
        [SerializeField] private float _minDeltaDrag = 0.1f;
        [SerializeField] private float _snapDuration = 0.3f;

        [Header("Events")]
        public UnityEvent<int, int> OnPageChangeStarted;
        public UnityEvent<int, int> OnPageChangeEnded;

        public Rect Rect
        {
            get
            {
#if UNITY_EDITOR
                if (_scrollRect == null)
                {
                    _scrollRect = FindScrollRect();
                }
#endif
                return ((RectTransform)_scrollRect.transform).rect;
            }
        }
        public RectTransform Content
        {
            get
            {
#if UNITY_EDITOR
                if (_scrollRect == null)
                {
                    _scrollRect = FindScrollRect();
                }
#endif
                return _scrollRect.content;
            }
        }

        private ScrollRect _scrollRect;

        private int _currentPage;
        private int _targetPage;

        private float _startNormalizedPosition;
        private float _targetNormalizedPosition;
        private float _moveSpeed;

        #endregion

        private void Awake()
        {
            _scrollRect = FindScrollRect();
        }
        private void Update()
        {
            if (_moveSpeed == 0) { return; }

            var position = _scrollRect.horizontalNormalizedPosition;
            position += _moveSpeed * Time.deltaTime;

            var min = _moveSpeed > 0 ? position : _targetNormalizedPosition;
            var max = _moveSpeed > 0 ? _targetNormalizedPosition : position;
            position = Mathf.Clamp(position, min, max);

            _scrollRect.horizontalNormalizedPosition = position;

            if (Mathf.Abs(_targetNormalizedPosition - position) < Mathf.Epsilon)
            {
                _moveSpeed = 0;

                OnPageChangeEnded?.Invoke(_currentPage, _targetPage);

                _currentPage = _targetPage;
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _startNormalizedPosition = _scrollRect.horizontalNormalizedPosition;

            if (_targetPage != _currentPage)
            {
                OnPageChangeEnded?.Invoke(_currentPage, _targetPage);

                _currentPage = _targetPage;
            }
            _moveSpeed = 0;
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            // Width of a single page (normalized).
            var pageWidth = 1f / GetPageCount();

            // Position of the current page (normalized).
            // When snapping it should equal the start normaalized position.
            var pagePosition = _currentPage * pageWidth;

            // Current position (normalized).
            var currentPosition = _scrollRect.horizontalNormalizedPosition;

            // Min amount of drag to change page (normalized).
            var minPageDrag = pageWidth * _minDeltaDrag;

            // If it's a forward or backward drag.
            var isForwardDrag = _scrollRect.horizontalNormalizedPosition > _startNormalizedPosition;

            // Position to change page (normalized).
            var switchPageBreakpoint = pagePosition + (isForwardDrag ? minPageDrag : -minPageDrag);

            // Change page when the current position is greater or lesser than the breakpoint,
            // when dragging forward or backward.
            var page = _currentPage;
            if (isForwardDrag && currentPosition > switchPageBreakpoint)
            {
                page++;
            }
            else if (!isForwardDrag && currentPosition < switchPageBreakpoint)
            {
                page--;
            }

            ScrollToPage(page);
        }

        private void ScrollToPage(int page)
        {
            _targetNormalizedPosition = page * (1f / GetPageCount());
            _moveSpeed = (_targetNormalizedPosition - _scrollRect.horizontalNormalizedPosition) / _snapDuration;

            _targetPage = page;

            if (_targetPage != _currentPage)
            {
                OnPageChangeStarted?.Invoke(_currentPage, _targetPage);
            }
        }
        private int GetPageCount()
        {
            var contentWidth = _scrollRect.content.rect.width;
            var rectWidth = ((RectTransform)_scrollRect.transform).rect.size.x;
            return Mathf.RoundToInt(contentWidth / rectWidth) - 1;
        }
        private ScrollRect FindScrollRect()
        {
            var scrollRect = GetComponentInChildren<ScrollRect>();

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (scrollRect == null)
            {
                Debug.LogError("Missing ScrollRect in Children");
            }
#endif
            return scrollRect;
        }
    }
}