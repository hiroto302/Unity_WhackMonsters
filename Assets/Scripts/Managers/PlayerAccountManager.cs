using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Text;
using UnityEditor;

// PlayerAccount を管理するクラス
public class PlayerAccountManager : MonoSingleton<PlayerAccountManager>
{
    [SerializeField] PlayerAccountData _accountData;

    // string _customID;
    bool _hasCreatedAccount = true;  // 既にアカウントが作成されているか

    protected override void Awake()
    {
        base.Awake();
        Login();
    }

#region Login処理

    public void Login()
    {
        // カスタムID が作成されていなければ作成
        if(string.IsNullOrEmpty(_accountData.CustomID))
        {
            _hasCreatedAccount = false;
            GenerateCustomID();
            return;
        }

        PlayFabClientAPI.LoginWithCustomID(
            new LoginWithCustomIDRequest { CustomId = _accountData.CustomID, CreateAccount = !_hasCreatedAccount },
            result => OnLoginSuccess(result),
            error => Debug.LogError(error.GenerateErrorReport())
        );
    }

    void OnLoginSuccess(LoginResult result)
    {
        // アカウントを作成しようとしたのに既に、生成したCustomID に対応した アカウントが生成されている場合
        if(!_hasCreatedAccount && !result.NewlyCreated)
        {
            _accountData.CustomID = "";
            Login();
            return;
        }
    }

    // PlayerAccount の カスタムID を生成
    // ID に使用する文字
    private static readonly string ID_CHARACTERS = "0123456789abcdefghijklmnopqrstuvwxyz";
    void GenerateCustomID()
    {
        // ID の長さ
        int idLength = 32;
        StringBuilder stringBuilder = new StringBuilder(idLength);
        var random = new System.Random();

        // ランダムにID を生成
        for(int i = 0; i < idLength; i++)
        {
            stringBuilder.Append(ID_CHARACTERS[random.Next(ID_CHARACTERS.Length)]);
        }

        _accountData.CustomID =  stringBuilder.ToString();

        // 値が変更されたことが正しく保存されるようにする(今まで他のスクリプタブルオブジェクトの値を更新した時は大丈夫であった)
        // ダーティとしてマークする(変更があった事を記録する)
        // EditorUtility.SetDirty(_accountData);
        // //保存する
        // AssetDatabase.SaveAssets();

        // 再びログイン処理実行
        Login();
    }
#endregion

}
