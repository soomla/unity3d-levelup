using System;
using System.Threading;
using NUnit.Framework;
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

			SoomlaLevelUp.Initialize (mainWorld);

			//basic asserts
			Assert.AreEqual("main_world", SoomlaLevelUp.GetWorld ("main_world").ID);
			Assert.AreEqual("badge_bronzeMedal", SoomlaLevelUp.GetReward ("badge_bronzeMedal").ID);
			Assert.AreEqual("badge_silverMedal", SoomlaLevelUp.GetReward ("badge_silverMedal").ID);
			Assert.AreEqual("main_world", SoomlaLevelUp.InitialWorld.ID);
			Assert.AreEqual(0, SoomlaLevelUp.GetLevelCount());
			Assert.AreEqual(bronzeMedal, Reward.GetReward("badge_bronzeMedal"));
			Assert.AreEqual(silverMedal, Reward.GetReward("badge_silverMedal"));
			Assert.AreEqual(goldMedal, Reward.GetReward("badge_goldMedal"));
			Assert.AreEqual(perfectMedal, Reward.GetReward("item_perfectMedal"));
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

			mainWorld.AssignReward(bronzeMedal);
			
			SoomlaLevelUp.Initialize (mainWorld);

			string json = KeyValueStorage.GetValue ("soomla.levelup.model");

			Assert.IsNotEmpty(json); 

			Assert.AreNotEqual("Dummy", json); //should fail
		}

		void onLevelUpInitialized()
		{
			Dictionary<string, object> expected = EventQueue.Dequeue ();
			
			Assert.AreEqual(expected["handler"], "onLevelUpInitialized");
		}
	}
}

