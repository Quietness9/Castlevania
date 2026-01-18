using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartUICtr : MonoBehaviour
{
    [SerializeField] string _gameScene = "Scene_1";
    [SerializeField] float _delay;

    [SerializeField] FadeScene _fadeScene;
    [SerializeField] Button _continueGameBt;
    [SerializeField] Button _newGameBt;
    [SerializeField] Button _exitGameBt;

    private void OnEnable()
    {
        _newGameBt.onClick.AddListener(NewGame);
        _continueGameBt.onClick.AddListener(ContinueGame);
        _exitGameBt.onClick.AddListener(ExitGame);
    }

    private void Start()
    {
        _fadeScene.gameObject.SetActive(true);

        if (!SaveAndLoadMgr.Instance.HaveGameData())
        {
            _continueGameBt.gameObject.SetActive(false);
        }
    }
    private void OnDisable()
    {
        _newGameBt.onClick.RemoveAllListeners();
        _exitGameBt.onClick.RemoveAllListeners();
        _continueGameBt.onClick.RemoveAllListeners();
    }

    /// <summary>
    /// 继续游戏
    /// </summary>
    private void ContinueGame()
    {
        StartCoroutine(LoadSceneFadeEffectCo(_delay));
    }

    /// <summary>
    /// 新游戏
    /// </summary>
    private void NewGame()
    {
        SaveAndLoadMgr.Instance.DeleteSaveGameData();
        StartCoroutine(LoadSceneFadeEffectCo(_delay));
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    private void ExitGame()
    {
        Debug.Log("exit game");
        //Application.Quit();
    }

    IEnumerator LoadSceneFadeEffectCo(float delay)
    {
        _fadeScene.FadeOutScene();
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(_gameScene);
    }
}
