using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cargo : MonoBehaviour
{
    const string emptySlotImage = "empty_icon";

    public GameObject dragObj;
    public List<GameObject> cargoSlotsObj = new List<GameObject>();  
    public Item draggedObject;

    private RectTransform canvasRect;
    private RectTransform dragObjRect;
    private RectTransform tipObjRect;

    private Vector3 mousePosition = Vector3.zero;
    private Vector3 mouseToolPosition = Vector3.zero;

    private bool dragItem;
    private bool tipObj;

    private string tooltipText;

    public bool DragItem
    {
        get { return dragItem; }
    }
    public Vector3 MousePosition
    {
        get
        {
            return new Vector3(mousePosition.x + 35, mousePosition.y - 35, mousePosition.z);
        }
    }

    public bool TipObj
    {
        get { return tipObj; }
        set { tipObj = value; }
    }

    public Vector3 MouseToolPosition
    {
        get
        {
            return new Vector3(mousePosition.x + 65, mousePosition.y - 75, mousePosition.z); ;
        }

    }

    void Awake()
    {
        canvasRect = GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>();
        dragObjRect = dragObj.GetComponent<RectTransform>();

        cargoSlotsObj.AddRange(GameObject.FindGameObjectsWithTag("CargoSlot"));
    }

    // Start is called before the first frame update
    void Start()
    {
        Player._Cargo.Add(0, ItemGenerator._ItemGenerator.ItemGen(0));
        Player._Cargo.Add(1, ItemGenerator._ItemGenerator.ItemGen(1));
        Player._Cargo.Add(2, ItemGenerator._ItemGenerator.ItemGen(2));
        Player._Cargo.Add(3, ItemGenerator._ItemGenerator.ItemGen(3));
        Player._Cargo.Add(4, ItemGenerator._ItemGenerator.ItemGen(4));

        createCargoSlots();
    }

    // Update is called once per frame
    void Update()
    {
        if (dragItem)
        {
            // ќтображение иконки перетаскиваемого слота
            mousePosition = (Input.mousePosition - canvasRect.localPosition);
            dragObjRect.localPosition = MousePosition;
        }
    }

    private void createCargoSlots()
    {
        int slotNumber = 0;

        for (int i = 0; i < cargoSlotsObj.Count; i++)
        {
            cargoSlotsObj[i].GetComponent<SlotEvent>().slotNumber = slotNumber;
            slotNumber++;

            // ќтображение иконки €чейки
            if (Player._Cargo.ContainsKey((i)))
            {
                cargoSlotsObj[(i)].GetComponent<Image>().sprite = Resources.Load<Sprite>(Player._Cargo[(i)].iconPath);
            }
            else
            {
                cargoSlotsObj[(i)].GetComponent<Image>().sprite = Resources.Load<Sprite>(emptySlotImage);
            }
        }
    }

    /// <summary>
    /// »змен€ем свойства €чейки (добавл€ем и удал€ем предметы инвентар€)
    /// </summary>
    /// <param name="key">номер слота</param>
    /// <param name="drag">перемещение €чейки</param>
    /// <param name="type">тип добавлени€ предмета(добавление в инвентарь или в сундук)</param>
    private void slotsProperties(int key, bool drag, string type)
    {
        if (type == "cargo")
        {
            if (!drag)
            {
                dragItem = false;
                draggedObject = null;
                dragObj.SetActive(false);
            }

            //измен€ем изображение иконки €чейки при добавление предмета в нее
            if (Player._Cargo.ContainsKey(key))
            {
                cargoSlotsObj[key].GetComponent<Image>().sprite = Resources.Load<Sprite>(Player._Cargo[key].iconPath);
            }
            else
            {
                cargoSlotsObj[key].GetComponent<Image>().sprite = Resources.Load<Sprite>(emptySlotImage);
            }
        }
        /*else if (type == "chest")
        {
            if (!drag)
            {
                dragItem = false;
                DraggedObject = null;
                DragObj.SetActive(false);
            }

            GameObject.FindGameObjectWithTag("Canvas").GetComponent<LootChest>().ChangeSlotIcon(key);

        }*/
        else
        {
            Debug.LogError("Ќе верно передан аргумент (√руз)");
        }
    }

    //ѕубличный метод дл€ изменени€ свойст €чеек
    public void ConditionSlots(int key, bool drag, string type)
    {
        slotsProperties(key, drag, type);
    }

    //¬ключаем возможность перетаскивать предмет
    public void DragObject(Item item)
    {
        dragItem = true;
        draggedObject = item;
        dragObj.SetActive(true);

        dragObj.GetComponent<Image>().sprite = Resources.Load<Sprite>(item.iconPath);
    }
}
