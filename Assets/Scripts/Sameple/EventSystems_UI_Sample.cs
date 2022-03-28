using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/* スマホ画面にタッチした時、インタクトな機能を実装するするために作成する サンプルスクリプト

EventSystem を利用する方法
3D オブジェクトにインタラクトする場合
- Camera に Raycaster をアタッチ
- クリックされる側に、このスクリプトをアタッチ
- クリックされる側は、Collider があること

UI(Imageなど) にインタラクトする場合
- このスクリプトがアタッチされてるのみ

- 検証 1. スマホの複数の同時入力に対応出来るか
同時入力に対応していた。

- Class PointerEventData
タッチイベントが発生するたびに、関連するすべての情報を含むこれらの1つが作成される.
スマホ操作だと PointerEventDataを引数にとるメソッドを実装するインターフェースが使用用途がありそう


*/
public class EventSystems_UI_Sample : MonoBehaviour,
    IPointerClickHandler,
    IPointerDownHandler,
    IPointerUpHandler,
    IDragHandler,
    IBeginDragHandler,
    IEndDragHandler,
    IInitializePotentialDragHandler,
    IPointerEnterHandler,
    IPointerExitHandler,
    IScrollHandler,
    IMoveHandler

{
    // クリックした時
    public void OnPointerClick(PointerEventData eventData)
    {
        // Debug.Log(name + " click されたよ");
        Debug.Log(eventData.clickTime);
    }

    // これもクリックした時、だが上記と動作が異なる
    // こちらの方が理想の動作している
    public void OnPointerDown(PointerEventData eventData)
    {
        // Debug.Log("OnPointerDownが実行され続けてるよ");
        transform.localScale = Vector3.one * 2.2f;
        GetComponent<Image>().color = Color.red;

    }

    // クリックした指が離れる時
    // クリックを検出するために、このコールバックを使用
    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = Vector3.one * 2.0f;
        GetComponent<Image>().color = Color.white;
    }

    // ドラッグした時
    public void OnDrag(PointerEventData eventData)
    {
        // Debug.Log("ドラッグしてるよ");
        transform.position = eventData.position;
        // Debug.Log(eventData.hovered);
        // Debug.Log(eventData.dragging);                        // ドラッグされているか
        // Debug.Log(eventData.delta);
        // Debug.Log(eventData.pointerDrag);                        // 現在ドラッグしている object
        // Debug.Log(eventData.pointerId + " : PointerID");
        // Debug.Log(eventData.position);                           // 現在の Pointer 位置
        // Debug.Log(eventData.pressPosition + " : pressPosition"); // 押された位置
        Debug.Log(eventData.IsPointerMoving() + " : IsPoiterMoving");
    }

    // ドラッグ開始時
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log(eventData.delta);
    }
    // ドラッグ終了時
    public void OnEndDrag(PointerEventData eventData)
    {
    }

    // ドラッグが感知されたときではなく、ドラッグ開始が有効になる前に BaseInputModule は呼び出される
    // 実際は、OnPointerDown と同じタイミングで実行されてるように見える
    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        // Debug.Log("ドラッグ検知");
    }

    // 実行タイミングがよく分からない
    // 今のタイミングだと、クリックした時とに Enter、そして話した時に Exit が実行される
    // このイベントの基準は、実装に依存する。例えば、StandAloneInputModuleを参照。
    // 実機で確認するべし
    // 指をスワイプ中にオブジェクトに触れたことを検出
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Debug.Log("Enter");
    }
    // 指がオブジェクトから離れたことを検出
    public void OnPointerExit(PointerEventData eventData)
    {
        // Debug.Log("Exit");
    }

    public void OnScroll(PointerEventData eventData)
    {
        Debug.Log("スクロール");
    }




    // 軸イベント（コントローラ／キーボード）に関連するイベントデータです。
    // 注意 : 引数が AxisEventData である
    public void OnMove(AxisEventData eventData)
    {
        // Debug.Log("OnMove");
    }
}
