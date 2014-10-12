using System;
using System.Threading;
using NUnit.Framework;
using UnityTest;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Soomla.Levelup;

namespace Soomla.Test
{
	[TestFixture]
	[Category ("Level Tests")]
	internal class LevelUpTest:SoomlaTest
	{
		/// <summary>
		/// Run before each test
		/// </summary>
		[SetUp] 
		public override void Init()
		{
			base.Init ();
		}

		/// <summary>
		/// Run after each test
		/// </summary>
		[TearDown] 
		public override void Cleanup()
		{
			base.Cleanup ();
		}

		public override void SubscribeToEvents()
		{
			LevelUpEvents.OnLevelUpInitialized += onLevelUpInitialized;
		}

		public override void UnsubscribeFromEvents()
		{
			LevelUpEvents.OnLevelUpInitialized -= onLevelUpInitialized;
		}

		/// <summary>
		/// Adding batch levels
		/// Creates multiple levels, checks whether they were properly created
		/// </summary>
		[Test]
		[Category ("Init SoomlaLevelUp")]
		public void SoomlaLevelUpInitTest()
		{
			LevelUpEvents.OnLevelUpInitialized += onLevelUpInitialized;

			Dictionary<string, object> evtLvlUpInitialized = new Dictionary<string, object> {
				{ "handler", "onLevelUpInitialized" }
			};

			EventQueue.Enqueue(evtLvlUpInitialized);

			World mainWorld = new World("main_world");

			BadgeReward bronzeMedal = new BadgeReward("badge_bronzeMedal", "Bronze Medal");
			BadgeReward silverMedal = new BadgeReward("badge_silverMedal", "Silver Medal");
			BadgeReward goldMedal = new BadgeReward("badge_goldMedal", "Gold Medal");
			VirtualItemReward perfectMedal = new VirtualItemReward("item_perfectMedal", "Perfect Medal", "perfect_medal", 1);

			SoomlaLevelUp.Initialize (mainWorld, new List<Reward> () { bronzeMedal, silverMedal, goldMedal, perfectMedal });

			//basic asserts
			Assert.AreEqual (SoomlaLevelUp.GetWorld ("main_world").ID, "main_world");
			Assert.AreEqual (SoomlaLevelUp.GetReward ("badge_bronzeMedal").ID, "badge_bronzeMedal");
			Assert.AreEqual (SoomlaLevelUp.InitialWorld.ID, "main_world");
			Assert.AreEqual (Convert.ToString (SoomlaLevelUp.GetLevelCount ()), "0");
			Assert.AreEqual (Convert.ToString (SoomlaLevelUp.Rewards.Count), "4");
		}

		/// <summary>
		/// Adding batch levels
		/// Creates multiple levels, checks whether they were properly created
		/// </summary>
		[Test]
		[Category ("Init SoomlaLevelUp")]
		public void SoomlaLevelUpDBSaveTest()
		{	
			World mainWorld = new World("main_world");
			
			BadgeReward bronzeMedal = new BadgeReward("badge_bronzeMedal", "Bronze Medal");
			
			SoomlaLevelUp.Initialize (mainWorld, new List<Reward> () { bronzeMedal });

			string json = KeyValueStorage.GetValue ("soomla.levelup.model");

			Assert.IsNotEmpty (json); 

			Assert.AreEqual ("Dummy", json); //should fail
		}

		void onLevelUpInitialized()
		{
			Dictionary<string, object> expected = EventQueue.Dequeue ();
			
			Assert.AreEqual(expected["handler"], "onLevelUpInitialized");
		}
	}
}

