using UnityEngine;
using UnityEngine.SceneManagement;

public class Return : MonoBehaviour
{
    public string MainScene;

    public void CargarEscena()
    {
        SceneManager.LoadScene(MainScene);
    }
}
