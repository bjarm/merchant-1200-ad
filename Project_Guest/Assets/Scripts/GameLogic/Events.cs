using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;

namespace Events
{
	public abstract class AbsractEvent
	{
		private readonly List<Action> callbacks = new List<Action>(); 
		public void Subscribe(Action callback)
		{ 
			callbacks.Add(callback);
		}
	
		public void Publish()
		{
			foreach (Action callback in callbacks)
				callback();
		}
	}

	public class PlayerHasArrivedInTownEvent : AbsractEvent
	{
	}

	public class DateChangedEvent: AbsractEvent
	{
		private readonly List<Action<int>> callbacks = new List<Action<int>>();
		
		public void Subscribe(Action<int> callback)
		{ 
			callbacks.Add(callback);
		}
	
		public void Publish(int amountOfDaysPassed)
		{
			foreach (Action<int> callback in callbacks)
				callback(amountOfDaysPassed);
		}
	}
}