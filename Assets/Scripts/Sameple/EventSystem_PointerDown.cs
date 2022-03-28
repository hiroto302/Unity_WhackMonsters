using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// オブジェクトが重なった時に処理を行うことができるのかテスト
public class EventSystem_PointerDown : MonoBehaviour,
    IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        // Debug.Log($"タップされた位置 {eventData.position}");
        Debug.Log($"タップされたオブジェクト {this.gameObject.name}");
    }
}
