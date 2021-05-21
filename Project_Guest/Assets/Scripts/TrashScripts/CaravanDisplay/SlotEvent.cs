using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotEvent : MonoBehaviour, IPointerClickHandler, IDragHandler,
  IPointerEnterHandler, IPointerExitHandler,
  IEndDragHandler, IDropHandler
{
    public int slotNumber;

    private Cargo cargo;
    private Text debugText;

    private bool isCursorOverSlot;

    // Start is called before the first frame update
    void Start()
    {
        cargo = GameObject.FindGameObjectWithTag("Cargo").GetComponent<Cargo>();
        debugText = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Player._Cargo.ContainsKey(slotNumber) && !cargo.DragItem)
            Debug.Log(Player._Cargo[slotNumber].name);
        else
            Debug.Log(debugText.text);
    }

    public void OnDrag(PointerEventData eventData)
    {
        cargo.DragObject(Player._Cargo[slotNumber]);
        Player._Cargo.Remove(slotNumber);
        cargo.ConditionSlots(slotNumber, true, "cargo");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!Player._Cargo.ContainsKey(slotNumber) && cargo.DragItem)
        {
            isCursorOverSlot = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!Player._Cargo.ContainsKey(slotNumber) && cargo.DragItem)
        {
            isCursorOverSlot = false;
            cargo.TipObj = false;
        }
        if (Player._Cargo.ContainsKey(slotNumber))
        {
            cargo.TipObj = false;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (cargo.draggedObject != null && cargo.DragItem)
        {
            Player._Cargo.Add(slotNumber, cargo.draggedObject);
            cargo.ConditionSlots(slotNumber, false, "cargo");
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!Player._Cargo.ContainsKey(slotNumber) && cargo.DragItem && isCursorOverSlot)
        {
            // Помещаем предмет в ячейку если она пустая
            Player._Cargo.Add(slotNumber, cargo.draggedObject);
            cargo.ConditionSlots(slotNumber, false, "cargo");
        }
    }
}
