#region Includes
using UnityEngine;
using UnityEngine.Events;
#endregion

namespace TS.PageSlider
{
    /// <summary>
    /// This class represents a view or page within a paginated view system.
    /// It provides events to signal changes in the active state of the page.
    /// </summary>
    public class PageView : MonoBehaviour
    {
        #region Variables

        [Header("Events")]

        /// <summary>
        /// UnityEvent that is invoked when the page is about to transition to the active state.
        /// </summary>
        [Tooltip("Invoked when the page is about to transition to the active state")]
        public UnityEvent OnChangingToActiveState;

        /// <summary>
        /// UnityEvent that is invoked when the page is about to transition to the inactive state.
        /// </summary>
        [Tooltip("Invoked when the page is about to transition to the inactive state")]
        public UnityEvent OnChangingToInactiveState;

        /// <summary>
        /// UnityEvent with a boolean parameter that is invoked when the active state of the page changes.
        /// The parameter is True if the page becomes active, False if it becomes inactive.
        /// </summary>
        [Tooltip("Invoked when the active state of the page changes: True when active and False when inactive")]
        public UnityEvent<bool> OnActiveStateChanged;

        #endregion

        /// <summary>
        /// Invokes the OnChangingToActiveState event to signal that the page is about to become active.
        /// </summary>
        public void ChangingToActiveState()
        {
            OnChangingToActiveState?.Invoke();
        }

        /// <summary>
        /// Invokes the OnChangingToInactiveState event to signal that the page is about to become inactive.
        /// </summary>
        public void ChangingToInactiveState()
        {
            OnChangingToInactiveState?.Invoke();
        }

        /// <summary>
        /// Invokes the OnActiveStateChanged event with the provided active state.
        /// </summary>
        /// <param name="active">True to signal the page becoming active, False for inactive.</param>
        public void ChangeActiveState(bool active)
        {
            OnActiveStateChanged?.Invoke(active);
        }
    }
}