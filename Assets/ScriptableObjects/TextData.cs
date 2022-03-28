using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTextData", menuName = "TextData")]
public class TextData : ScriptableObject
{
    [Header("Infomation")]
    [TextArea(1, 5)]
    public string Text;
}
