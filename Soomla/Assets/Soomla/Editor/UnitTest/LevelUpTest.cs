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
			_levelUp = SoomlaLevelUp.GetInstance ();
		}

		/// <summary>
		/// Run after each test
		/// </summary>
		[TearDown] 
		public override void Cleanup()
		{
			base.Cleanup ();
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

			_levelUp.Initialize (mainWorld, new List<Reward> () { bronzeMedal, silverMedal, goldMedal, perfectMedal });
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
			
			_levelUp.Initialize (mainWorld, new List<Reward> () { bronzeMedal });

			string json = KeyValueStorage.GetValue ("soomla.levelup.model");

			Assert.IsNotEmpty (json); 

			Assert.AreEqual ("Dummy", json); //should fail
		}

		public override void onLevelUpInitialized()
		{
			Dictionary<string, object> expected = EventQueue.Dequeue ();
			
			Assert.AreEqual(expected["handler"], "onLevelUpInitialized");
		}
	
		//Private test components
		SoomlaLevelUp _levelUp;
	}
}

