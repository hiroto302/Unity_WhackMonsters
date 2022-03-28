using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

// ステージの制限時間
[CreateAssetMenu(fileName = "NewTimelimitData", menuName = "TimelimitData")]
public class TimelimitData : ScriptableObject,  ISerializationCallbackReceiver
{
    [Header("Initial Value")]
    [SerializeField] public float InitialTimelimit;
    [NonSerialized] public FloatReactiveProperty RuntimeTimelimit = new FloatReactiveProperty(0);

    public void OnAfterDeserialize()
    {
        RuntimeTimelimit.Value = InitialTimelimit;
    }
    public void OnBeforeSerialize(){}

    [TextArea(1, 2)]
    public string ShortDescription;
}
