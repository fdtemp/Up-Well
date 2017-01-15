using UnityEngine;
using UnityEngine.SceneManagement;

class NewGame : MonoBehaviour {
    void OnClick() {
        SceneManager.LoadScene("Main");
        
    }
}