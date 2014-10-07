using System;
using System.Threading;
using NUnit.Framework;
using UnityTest;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Soomla;
using Soomla.Levelup;
using Soomla.Store;

namespace Soomla.Test
{
	public class SoomlaTest
	{
		public Queue<Dictionary<string, object>> EventQueue
		{
			get { return this._eventQueue; }
		}

		public virtual void Init()
		{
			PlayerPrefs.DeleteAll();
			_eventQueue = new Queue<Dictionary<string, object>> ();
		}

		public virtual void Cleanup()
		{
			PlayerPrefs.DeleteAll();

			//Unsubscribe from events
			LevelUpEvents.OnLevelStarted -= onLevelStarted;
			LevelUpEvents.OnLevelEnded -= onLevelEnded;
			LevelUpEvents.OnLevelUpInitialized -= onLevelUpInitialized;
		}

		public virtual void onLevelStarted(Level level){}
		public virtual void onLevelEnded(Level level){}
		public virtual void onLevelUpInitialized(){}

	    Queue<Dictionary<string, object>> _eventQueue; 
	}
}
