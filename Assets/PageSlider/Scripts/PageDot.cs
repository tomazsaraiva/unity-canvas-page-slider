#region Includes
using UnityEngine;
using UnityEngine.Events;
#endregion

namespace TS.PageSlider
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

        private void Start()
        {
            ChangeActiveState(Index == 0);
        }

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