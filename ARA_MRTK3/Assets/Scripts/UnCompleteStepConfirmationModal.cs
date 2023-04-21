using Microsoft.MixedReality.Toolkit.UX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class UnCompleteStepConfirmationModal : MonoBehaviour
    {
        [SerializeField] private PressableButton cancelButton;
        [SerializeField] private PressableButton confirmButton;

        private Action CancellationCallback;
        private Action ConfirmationCallback;

        public void Show(Action cancellationCallback, Action confirmationCallback)
        {
            CancellationCallback = cancellationCallback;
            ConfirmationCallback = confirmationCallback;
            this.gameObject.SetActive(true);
        }

        public void Hide()
        {
            this.gameObject.SetActive(false);
        }

        public void OnCancelled()
        {
            CancellationCallback.Invoke();
            this.gameObject.SetActive(false);
        }

        public void OnConfirmed()
        {
            ConfirmationCallback.Invoke();
            this.gameObject.SetActive(false);
        }

    }
}
