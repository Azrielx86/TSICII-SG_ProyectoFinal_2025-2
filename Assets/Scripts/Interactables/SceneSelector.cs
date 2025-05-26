using UnityEngine;
using UnityEngine.SceneManagement;

namespace Interactables
{
    public class SceneSelector : MonoBehaviour, IInteractable
    {
        public string sceneName;
        
        public void Interact()
        {
            Debug.Log("Changing scene");
            SceneManager.LoadScene(sceneName);
        }

        public void ShowHint()
        {
            Debug.Log("Show hint triggered");
        }
    }
}