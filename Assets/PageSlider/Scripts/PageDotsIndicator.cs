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
    /// This class manages a collection of page dots used for navigation in a paginated view.
    /// It provides methods to add, clear, and change the active dot.
    /// </summary>
    public class PageDotsIndicator : MonoBehaviour
    {
        #region Variables

        [Header("References")]

        /// <summary>
        /// Prefab reference for the PageDot component representing a single dot indicator.
        /// </summary>
        [Tooltip("Prefab reference for the PageDot component representing a single dot indicator")]
        [SerializeField] private PageDot _prefab;

        [Header("Children")]

        /// <summary>
        /// List containing references to all currently displayed PageDot instances.
        /// </summary>
        [Tooltip("List containing references to all currently displayed PageDot instances")]
        [SerializeField] private List<PageDot> _dots;

        [Header("Events")]

        /// <summary>
        /// UnityEvent that is invoked when a page dot is pressed, passing the index of the pressed dot.
        /// </summary>
        [Tooltip("Invoked when a page dot is pressed, passing the index of the pressed dot")]
        public UnityEvent<int> OnDotPressed;

        /// <summary>
        /// Gets or sets the visibility of the PageDotsIndicator game object.
        /// </summary>
        public bool IsVisible
        {
            get { return gameObject.activeInHierarchy; }
            set { gameObject.SetActive(value); }
        }

        #endregion

        private void Awake()
        {
            if (_dots.Count == 0) return;
            for (int i = 0; i < _dots.Count; i++)
            {
                _dots[i].ChangeActiveState(i == 0);
            }
        }

        /// <summary>
        /// Adds a new page dot indicator to the collection.
        /// </summary>
        public void Add()
        {
            PageDot dot = null;

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                // In editor mode, use PrefabUtility for non-destructive instantiation.
                dot = (PageDot)PrefabUtility.InstantiatePrefab(_prefab, transform);
            }
#endif

            // If no dot was instantiated in editor mode, use regular Instantiate in play mode.
            if (dot == null)
            {
                dot = Instantiate(_prefab, transform);
            }

            dot.Index = _dots.Count;
            dot.ChangeActiveState(_dots.Count == 0); // Activate the first dot.

            _dots.Add(dot);

#if UNITY_EDITOR
            if (Application.isPlaying) { return; }

            // In editor mode, mark the scene as dirty to save changes.
            EditorUtility.SetDirty(this);
#endif
        }

        /// <summary>
        /// Clears all the page dot indicators from the collection and destroys their game objects.
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < _dots.Count; i++)
            {
                if (_dots[i] == null) { continue; }
#if UNITY_EDITOR
                // In editor mode, use DestroyImmediate for immediate object removal.
                DestroyImmediate(_dots[i].gameObject);
#else

                // In play mode, use Destroy for object removal during gameplay.
                Destroy(_dots[i].gameObject);
#endif
            }

            _dots.Clear();

#if UNITY_EDITOR

            // In editor mode, mark the scene as dirty to save changes.
            EditorUtility.SetDirty(this);
#endif
        }

        /// <summary>
        /// Changes the active state of the page dots.
        /// It deactivates the dot at the 'fromIndex' and activates the dot at the 'toIndex'.
        /// </summary>
        /// <param name="fromIndex">The index of the dot to deactivate.</param>
        /// <param name="toIndex">The index of the dot to activate.</param>
        public void ChangeActiveDot(int fromIndex, int toIndex)
        {
            _dots[fromIndex].ChangeActiveState(false);
            _dots[toIndex].ChangeActiveState(true);
        }

#if UNITY_EDITOR

        [CustomEditor(typeof(PageDotsIndicator))]
        public class PageDotsIndicatorEditor : Editor
        {
            #region Variables

            private PageDotsIndicator _target;

            #endregion

            private void OnEnable()
            {
                _target = (PageDotsIndicator)target;
            }
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Editor");

                if (GUILayout.Button("Clear"))
                {
                    _target.Clear();
                }
            }
        }
#endif
    }

}