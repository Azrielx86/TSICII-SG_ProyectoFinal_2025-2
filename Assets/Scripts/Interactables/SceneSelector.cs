using UnityEngine;

namespace Interactables
{
    public class SceneSelector : MonoBehaviour, IInteractable
    {
        public string sceneName;
        
        public void Interact()
        {
            Debug.Log("This is in the interactable zone.");
        }
    }
}