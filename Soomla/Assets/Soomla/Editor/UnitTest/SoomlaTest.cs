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
	public abstract class SoomlaTest
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
			UnsubscribeFromEvents ();
		}

		public abstract void SubscribeToEvents();
		public abstract void UnsubscribeFromEvents();

	    Queue<Dictionary<string, object>> _eventQueue; 
	}
}
