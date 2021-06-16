using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    RectTransform rectT;

    void Start()
    {
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Drag;
        entry.callback.AddListener((data) => { OnDragDelegate((PointerEventData)data); });
        trigger.triggers.Add(entry);
        rectT = GetComponent<RectTransform>();
    }

    public void OnDragDelegate(PointerEventData data)
    {
        rectT.anchoredPosition = data.position;

    }
}
