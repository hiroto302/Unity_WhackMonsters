using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// ボタンがクリックされた時に音を鳴らすクラス
/// </summary>
[RequireComponent(typeof(Button))]
[RequireComponent(typeof(AudioSource))]
public class ButtonClickSounder : MonoBehaviour
{
    [SerializeField] protected Button _button;
    [SerializeField] protected AudioSource _audioSource;
    [SerializeField] protected AudioClip _clickSound;

    void Reset()
    {
        _button =  GetComponent<Button>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        _button.OnClickAsObservable()
            .Subscribe(_ => EmitClickSound())
            .AddTo(this);
    }

    /// <summary>
    /// クリックした時、音を鳴らす
    /// </summary>
    public virtual void EmitClickSound()
    {
        if(_clickSound != null)
            _audioSource?.PlayOneShot(_clickSound);
    }
}
