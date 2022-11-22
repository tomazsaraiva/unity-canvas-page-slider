#region Includes
using System;

using UnityEngine;
using UnityEngine.Events;
#endregion

namespace TS.PageSlider
{
    public class PageView : MonoBehaviour
    {
        #region Variables

        [Header("Events")]
        public UnityEvent OnChangingToActiveState;
        public UnityEvent OnChangingToInactiveState;
        public UnityEvent<bool> OnActiveStateChanged;

        #endregion

        public void ChangingToActiveState()
        {
            OnChangingToActiveState?.Invoke();
        }
        public void ChangingToInactiveState()
        {
            OnChangingToInactiveState?.Invoke();
        }
        public void ChangeActiveState(bool active)
        {
            OnActiveStateChanged?.Invoke(active);
        }
    }
}