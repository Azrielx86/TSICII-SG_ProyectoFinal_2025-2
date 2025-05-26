using System;
using UnityEngine;

namespace Interactables
{
    public class CameraChangerItem : MonoBehaviour, IInteractable
    {
        public Canvas hintCanvas;

        private void Start()
        {
            hintCanvas.enabled = false;
        }

        public void Interact()
        {
            Debug.Log("Changing Camera!");
        }

        public void ShowHint()
        {
            hintCanvas.enabled = true;
        }
    }
}
