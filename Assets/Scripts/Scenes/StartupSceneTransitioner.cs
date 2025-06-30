using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupSceneTransitioner : MonoBehaviour {
    public string sceneToLoad;

    void Start() {
        SceneManager.LoadScene(sceneToLoad);
    }
}
