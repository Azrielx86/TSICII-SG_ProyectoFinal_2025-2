using UnityEngine;
using UnityEngine.SceneManagement;

public class Leave : MonoBehaviour
{
    public string MainScene;

    public void CargarEscena()
    {
        SceneManager.LoadScene(MainScene);
    }
}