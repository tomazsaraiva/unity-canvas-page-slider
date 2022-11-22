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
    public class PageDotsIndicator : MonoBehaviour
    {
        #region Variables

        [Header("References")]
        [SerializeField] private PageDot _prefab;

        [Header("Children")]
        [SerializeField] private List<PageDot> _dots;

        [Header("Events")]
        public UnityEvent<int> OnDotPressed;

        public bool IsVisible
        {
            get { return gameObject.activeInHierarchy; }
            set { gameObject.SetActive(value); }
        }

        #endregion

        public void Add()
        {
            PageDot dot = null;

#if UNITY_EDITOR
            if(!Application.isPlaying)
            {
                dot = (PageDot)PrefabUtility.InstantiatePrefab(_prefab, transform);
            }
#endif
            if(dot == null)
            {
                dot = Instantiate(_prefab, transform);
            }

            dot.Index = _dots.Count;
            dot.ChangeActiveState(_dots.Count == 0);

            _dots.Add(dot);

#if UNITY_EDITOR
            if (Application.isPlaying) { return; }
            EditorUtility.SetDirty(this);
#endif
        }
        public void Clear()
        {
            for (int i = 0; i < _dots.Count; i++)
            {
                if (_dots[i] == null) { continue; }
#if UNITY_EDITOR
                DestroyImmediate(_dots[i].gameObject);
#else
                Destroy(_dots[i].gameObject);
#endif
            }

            _dots.Clear();

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

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