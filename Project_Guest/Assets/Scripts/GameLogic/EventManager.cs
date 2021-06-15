using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public class EventManager
{
    public static readonly PlayerHasArrivedInTownEvent PlayerHasArrivedInTown = new PlayerHasArrivedInTownEvent();
    public static readonly DateChangedEvent DateChanged = new DateChangedEvent();
}
