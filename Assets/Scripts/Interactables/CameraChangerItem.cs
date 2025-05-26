using System;
using UnityEngine;

namespace Interactables
{
    public class CameraChangerItem : MonoBehaviour, IInteractable
    {
        public Canvas hintCanvas;
        public GameObject playerObject;
        public GameObject controllableObject;

        private void Start()
        {
            hintCanvas.enabled = false;
        }

        public void Interact()
        {
            Debug.Log("Changing Camera!");
            var playerCamera = playerObject.GetComponentInChildren<Camera>();
            var controllableCamera = controllableObject.GetComponentInChildren<Camera>();

            playerCamera.enabled = false;
            playerObject.SetActive(false);
            
            controllableCamera.enabled = true;
            controllableObject.SetActive(true);
        }

        public void ShowHint()
        {
            hintCanvas.enabled = true;
        }
    }
}
