using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// DK wt.
/// </summary>
public class TurnToNewScene : MonoBehaviour 
{
    public void LoadScene(string sceneName) {
        Debug.Log("Load");
        SceneManager.LoadScene(sceneName);
    }

}
