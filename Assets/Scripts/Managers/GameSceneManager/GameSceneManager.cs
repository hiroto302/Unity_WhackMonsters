using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.SceneManagement.SceneManager;
using UnityEngine.SceneManagement;
using UniRx;
using DG.Tweening;
using Cysharp.Threading.Tasks;

/* ゲームシーン 管理を担うクラス

実装したいこと
- 引数に渡されたシーンをロード、アンロードすること

出来ること
- シーンをロード・アンロードすること
*/
public class GameSceneManager : MonoSingleton<GameSceneManager>, ISceneLoader
{
    [SerializeField]  ScenesData _scenesData = null;
    public ScenesData SD => _scenesData;

    async void Start()
    {
    #if UNITY_EDITOR

        // 初期に読み込まれているシーンを取得
        string initialLoadedScene = GetActiveScene().name;
        // 初期に読み込まれているシーンが 「基底シーンである Managers」である時
        if(initialLoadedScene == "GameCore")
        {
            // メインニューをロード
            await LoadSceneAsync(SD.Menus[0].SceneName, LoadSceneMode.Additive);
            SetActiveScene(GetSceneByName(_scenesData.Menus[0].SceneName));
        }
        // それ以外の時 (開発時の時、他のシーンが最初に呼び込まれている可能性があるため)
        else
        {
            // 初期に呼び込まれているシーンを アクティブシーンに指定
            // メインメニューをロードしない
            SetActiveScene(GetSceneByName(initialLoadedScene));
        }

    #elif UNITY_ANDROID

        // 画面起動時の処理 メインメニューシーンを追加
        await LoadSceneAsync(SD.Menus[0].SceneName, LoadSceneMode.Additive);
        SetActiveScene(GetSceneByName(_scenesData.Menus[0].SceneName));

    #endif
    }

    /// <summary>
    /// ロードに必要な一連の処理を実行
    /// </summary>
    /// <param name="sceneName">ロードするシーン名</param>
    /// <param name="gameState">ロード後のGameState</param>
    async public void LoadScene(string sceneName, GameState gameState)
    {
        // ロード前に行う処理
        await UniTask.WhenAll(UIManager.Instance.FadeInAlphaUniTask(1.0f));
        // DummyCamera ON : ロード画面を表示
        UIManager.Instance.SetDummyCamera(true);
        // 現在のアクティブシーンをアンロード
        // await UnloadSceneAsync(SceneManager.GetActiveScene()).ToUniTask();
        await UnloadSceneAsync(SceneManager.GetActiveScene());
        // 新たなシーンをロード
        await LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        // Active シーンを ロードしたシーン に変更
        SetActiveScene(GetSceneByName(sceneName));
        // DummyCamera OFF
        UIManager.Instance.SetDummyCamera(false);
        // ロード完了後行う処理
        await UniTask.WhenAll(UIManager.Instance.FadeOutAlphaUniTask(1.0f));
        // ゲーム開始状態に変更
        GameStateManager.Instance.ChangeGameState(gameState);
    }

    /// <summary>
    /// 現在のActiveScene をリロードする
    /// </summary>
    /// <param name="gameState">リロード先のGameState</param>
    async public void ReloadScene(GameState gameState)
    {
        // ロード前に行う処理
        await UniTask.WhenAll(UIManager.Instance.FadeInAlphaUniTask(1.0f));
        // DummyCamera ON : ロード画面を表示
        UIManager.Instance.SetDummyCamera(true);
        // リロードするシーン(現在のアクティブシーン)を取得
        string reloadSceneName = GetActiveScene().name;
        // GameState を変更
        GameStateManager.Instance.ChangeGameState(GameState.PreGame);
        // 現在のアクティブシーンをアンロード
        // await UnloadSceneAsync(SceneManager.GetActiveScene()).ToUniTask();
        await UnloadSceneAsync(SceneManager.GetActiveScene());
        // シーンをリロード
        await LoadSceneAsync(reloadSceneName, LoadSceneMode.Additive);
        // Active シーンを リロードしたシーン に変更
        SetActiveScene(GetSceneByName(reloadSceneName));
        // DummyCamera OFF
        UIManager.Instance.SetDummyCamera(false);
        // ロード完了後行う処理
        await UniTask.WhenAll(UIManager.Instance.FadeOutAlphaUniTask(1.0f));
        // ゲーム開始状態に変更
        GameStateManager.Instance.ChangeGameState(gameState);
    }

}
