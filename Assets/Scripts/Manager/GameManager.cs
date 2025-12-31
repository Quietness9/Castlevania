using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    public void RestartScene()
    {
        Scene scene=SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
