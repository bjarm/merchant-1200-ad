using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Events
{
	public class PlayerHasArrivedInTownEvent 
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
}