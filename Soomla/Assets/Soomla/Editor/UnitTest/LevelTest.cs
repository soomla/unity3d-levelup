/// <summary>
/// Test Level Class functionality
/// </summary>
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
	[TestFixture]
	[Category ("Level Tests")]
	internal class LevelTest
	{
		/// <summary>
		/// Run before each test
		/// </summary>
		[SetUp] 
		public void Init()
		{
			PlayerPrefs.DeleteAll();
			_level = new Level(cDummyLevelID);
			_eventQueue = new Queue<Dictionary<string, object>> ();
		}

		/// <summary>
		/// Run after each test
		/// </summary>
		[TearDown] 
		public void Cleanup()
		{
			PlayerPrefs.DeleteAll();

			//Unsubscribe from events
			LevelUpEvents.OnLevelStarted -= onLevelStarted;
			LevelUpEvents.OnLevelEnded -= onLevelEnded;
		}

		/// <summary>
		/// Check if newly instantiated level has is Idle
		/// </summary>
		[Test]
		[Category ("States")]
		public void CheckInitialLevelState()
		{	
			Assert.AreEqual (_level.State, Level.LevelState.Idle);
		}

		/// <summary>
		/// Check if a newly instantiated level can start
		/// </summary>
		[Test]
		[Category ("States")]
		public void CanLevelStart()
		{	
			Assert.IsTrue(_level.CanStart());
		}

		/// <summary>
		/// Check level state after Start() is Running
		/// </summary>
		[Test]
		[Category ("States")]
		public void DidLevelStart()
		{
			_level.Start();
			
			Assert.AreEqual(_level.State, Level.LevelState.Running);

			_level.End (true);
		}

		/// <summary>
		/// Check level state after End(false) is NOT completed
		/// </summary>
		[Test]
		[Category ("States")]
		public void LevelEndedNotCompleted()
		{	
			_level.Start ();
	
			_level.End(false);

			Assert.True(_level.State == Level.LevelState.Ended);
			Assert.False(_level.IsCompleted());
		}

		/// <summary>
		/// Check level state after End(false) is NOT completed
		/// </summary>
		[Test]
		[Category ("States")]
		public void LevelCompleted()
		{	
			_level.Start();
			
			_level.SetCompleted(true);

			Assert.True(_level.IsCompleted());
		}

		/// <summary>
		/// Check level state after Start() is Running
		/// </summary>
		[Test]
		[Category ("States")]
		public void DidLevelPause()
		{	
			_level.Start();

			_level.Pause ();

			Assert.AreEqual(_level.State, Level.LevelState.Paused);
		}

		/// <summary>
		/// Check is Score was added to Level
		/// </summary>
		[Test]
		[Category ("Scores")]
		public void LevelScoreAddition()
		{
			_level.AddScore(new Score(cDummyScoreID));

			Assert.IsTrue(_level.Scores.ContainsKey(cDummyScoreID));
		}
			
		/// <summary>
		/// Level playing duration
		/// Start level, sleep for 1 sec, check that 1 <= playing duration < 2
		/// </summary>
		[Test]
		[Category ("Playing")]
		public void LevelPlayTime()
		{
			_level.Start();

			System.Threading.Thread.Sleep(1000); 

			double playDuration = _level.GetPlayDurationMillis();

			Assert.LessOrEqual (1000, playDuration);  

			Assert.Less(playDuration, 2000);
		}

		/// <summary>
		/// Level pause
		/// Start level, sleep for 1 sec, check that 1 <= playing duration < 2
		/// </summary>
		[Test]
		[Category ("Playing")]
		public void LevelPlayTimeWithPause()
		{
			//TODO
		}

		/// <summary>
		/// Test onLevelStarted event is getting raised
		/// </summary>
		/// 
		[Test]
		[Category ("Events")]
		public void CheckOnLevelStartedEvent()
		{
			_eventQueue.Clear ();

			LevelUpEvents.OnLevelStarted += onLevelStarted;

			Dictionary<string, object> evtLvlStarted = new Dictionary<string, object> {
				{ "handler", "onLevelStarted" },
				{ "id", cDummyLevelID }
			};

			_eventQueue.Enqueue(evtLvlStarted);

			_level.Start();

			//TODO: validate this is a sync call
		}

		/// <summary>
		/// Test onLevelStarted event is getting raised
		/// </summary>
		/// 
		[Test]
		[Category ("Events")]
		public void CheckOnLevelEndedEvent()
		{
			_eventQueue.Clear ();

			LevelUpEvents.OnLevelEnded += onLevelEnded;
			
			Dictionary<string, object> evtLvlEnded = new Dictionary<string, object> {
				{ "handler", "onLevelEnded" },
				{ "id", cDummyLevelID }
			};
			
			_eventQueue.Enqueue(evtLvlEnded);
			
			_level.Start();

			System.Threading.Thread.Sleep(1000); 

			_level.End (true);	
		}

		private void onLevelStarted(Level level)
		{
			onEventFired (level, System.Reflection.MethodBase.GetCurrentMethod ().Name);
		}

		private void onLevelEnded(Level level)
		{
			onEventFired (level, System.Reflection.MethodBase.GetCurrentMethod ().Name);
		}

		private void onEventFired(Level level, string eventName)
		{
			Dictionary<string, object> expected = _eventQueue.Dequeue ();
			
			Assert.AreEqual(expected["handler"], eventName);
			
			Assert.AreEqual(expected["id"], level.ID);
		}

		/// <summary>
		/// Constants
		/// </summary>
		const string cDummyLevelID = "TestLevel";
		const string cDummyScoreID = "TestScore";

		/// <summary>
		/// Members
		/// </summary>
		Level _level;
		static Queue<Dictionary<string, object>> _eventQueue; 
	}
}

