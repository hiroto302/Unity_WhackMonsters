using UnityEngine;
using UniRx;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 継承クラス ボタンをクリックした時に実行することを実装
/// </summary>
[RequireComponent(typeof(Button))]
public abstract class AButton :MonoBehaviour
{
    [SerializeField] protected Button _button;

    void Reset()
    {
        _button =  GetComponent<Button>();
    }

    virtual public void Start()
    {
        _button.OnClickAsObservable()
            .Subscribe(_ => Execute())
            .AddTo(this);
    }

    /// <summary>
    /// クリックした時の実行処理
    /// </summary>
    public abstract void Execute();
}
