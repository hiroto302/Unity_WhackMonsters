using UnityEngine;
using UniRx;

using static BatBoss;

public class BatBossAnimator : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] BatBoss _batBoss;

    public readonly int _hasApplyDamage = Animator.StringToHash("ApplyDamage");
    public readonly int _hashReturn = Animator.StringToHash("Return");

    void Start()
    {
        _batBoss.OnApplyDamage
            // .Subscribe(_ => _animator.SetTrigger(_hasApplyDamage))
            .Subscribe(_ => PlayDamageAnim())
            .AddTo(this);

        _batBoss.CurrentState
            .Where(state => state == State.Return)
            .Subscribe(_ => _animator.SetTrigger(_hashReturn))
            .AddTo(this);
    }

    // ダメージアニメーション
    // SetTrigger が連続的に呼ばれると バグるので回避する
    void PlayDamageAnim()
    {
        _batBoss.transform.Translate(0, -0.2f, 0);
    }

    public void OnFinishSpawnAnimation()
    {
        _batBoss.HasFinishedSpawnAnim = true;
    }
    public void OnFinishRetrunAnimation()
    {
        _batBoss.HasFinishedReturnAnim = true;
    }
}
