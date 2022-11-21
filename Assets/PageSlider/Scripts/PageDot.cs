#region Includes
using UnityEngine;
using UnityEngine.Events;
#endregion

namespace TS.PageViewer
{
    public class PageDot : MonoBehaviour
    {
        #region Variables

        [Header("Events")]
        public UnityEvent<bool> OnActiveStateChanged;
        public UnityEvent<int> OnPressed;

        public bool IsActive { get; private set; }
        public int Index { get; set; }

        #endregion

        public virtual void ChangeActiveState(bool active)
        {
            IsActive = active;

            OnActiveStateChanged?.Invoke(active);
        }
        public void Press()
        {
            OnPressed?.Invoke(Index);
        }
    }
}