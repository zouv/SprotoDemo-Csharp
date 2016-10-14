/***
* Author: zouv
* Date: 2015-08-11
* Doc: UGUI 时间监听接口
***/

using UnityEngine;
using UnityEngine.EventSystems;

class UIEventTriggerListener : EventTrigger
{
    public delegate void PointerEventDataDelegate(GameObject go, PointerEventData ed);
    public delegate void BaseEventDataDelegate(GameObject go, BaseEventData ed);
    public delegate void AxisEventDataDelegate(GameObject go, AxisEventData ed);

    public PointerEventDataDelegate onBeginDrag;
    public BaseEventDataDelegate onCancel;
    public BaseEventDataDelegate onDeselect;
    public PointerEventDataDelegate onDrag;
    public PointerEventDataDelegate onDrop;
    public PointerEventDataDelegate onEndDrag;
    public PointerEventDataDelegate onInitializePotentialDrag;
    public AxisEventDataDelegate onMove;
    public PointerEventDataDelegate onClick;
    public PointerEventDataDelegate onDown;
    public PointerEventDataDelegate onUp;
    public PointerEventDataDelegate onEnter;
    public PointerEventDataDelegate onExit;
    public PointerEventDataDelegate onScroll;
    public BaseEventDataDelegate onSelect;
    public BaseEventDataDelegate onSubmit;
    public BaseEventDataDelegate onUpdateSelected;

    public override void OnBeginDrag(PointerEventData eventData) { if (onBeginDrag == null) return; onBeginDrag(gameObject, eventData); }
    public override void OnCancel(BaseEventData eventData) { if (onCancel == null) return; onCancel(gameObject, eventData); }
    public override void OnDeselect(BaseEventData eventData) { if (onDeselect == null) return; onDeselect(gameObject, eventData); }
    public override void OnDrag(PointerEventData eventData) { if (onDrag == null) return; onDrag(gameObject, eventData); }
    public override void OnDrop(PointerEventData eventData) { if (onDrop == null) return; onDrop(gameObject, eventData); }
    public override void OnEndDrag(PointerEventData eventData) { if (onEndDrag == null) return; onEndDrag(gameObject, eventData); }
    public override void OnInitializePotentialDrag(PointerEventData eventData) { if (onInitializePotentialDrag == null) return; onInitializePotentialDrag(gameObject, eventData); }
    public override void OnMove(AxisEventData eventData) { if (onMove == null) return; onMove(gameObject, eventData); }
    public override void OnPointerClick(PointerEventData eventData) { if (onClick == null) return; onClick(gameObject, eventData); }
    public override void OnPointerDown(PointerEventData eventData) { if (onDown == null) return; onDown(gameObject, eventData); }
    public override void OnPointerUp(PointerEventData eventData) { if (onUp == null) return; onUp(gameObject, eventData); }
    public override void OnPointerEnter(PointerEventData eventData) { if (onEnter == null) return; onEnter(gameObject, eventData); }
    public override void OnPointerExit(PointerEventData eventData) { if (onExit == null) return; onExit(gameObject, eventData); }
    public override void OnScroll(PointerEventData eventData) { if (onScroll == null) return; onScroll(gameObject, eventData); }
    public override void OnSelect(BaseEventData eventData) { if (onSelect == null) return; onSelect(gameObject, eventData); }
    public override void OnSubmit(BaseEventData eventData) { if (onSubmit == null) return; onSubmit(gameObject, eventData); }
    public override void OnUpdateSelected(BaseEventData eventData) { if (onUpdateSelected == null) return; onUpdateSelected(gameObject, eventData); }

    static public UIEventTriggerListener Get(GameObject go)
    {
        UIEventTriggerListener listener = go.GetComponent<UIEventTriggerListener>();
        if (!listener)
            listener = go.AddComponent<UIEventTriggerListener>();

        return listener;
    }
}

