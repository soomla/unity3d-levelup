using UnityEngine;
using System.Collections;
using NUnit.Framework;
using Soomla.Levelup;
using Soomla.Profile;
using System.Collections.Generic;
using System;

namespace Soomla.Test
{
	[TestFixture]
	[Category ("JSON OBject Tests")]
	internal class JSONObjectTest : SoomlaTest
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
		
		public override void SubscribeToEvents ()
		{
		}
		
		public override void UnsubscribeFromEvents ()
		{
		}

		[Test]
		[Category ("Reward fromJSONObject")]
		public void Reward_fromJSONObject()
		{
			{
				var expect =
					@"{
	""name"":""Test_BadgeReward"",
	""description"":"""",
	""itemId"":""Test_BadgeReward_Id"",
	""className"":""BadgeReward"",
	""schedule"":{
		""className"":""Schedule"",
		""schedRecurrence"":4,
		""schedApprovals"":1,
		""schedTimeRanges"":[]
	},
	""iconUrl"":""http://iconUrl""
}";
				var reward = Reward.fromJSONObject(JSONObject.Create(expect)) as BadgeReward;
				Assert.IsNotNull(reward);
				Assert.AreEqual("Test_BadgeReward", reward.Name);
				Assert.AreEqual("Test_BadgeReward_Id", reward.ID);
				Assert.AreEqual("http://iconUrl", reward.IconUrl);
				Assert.IsNotNull(reward.Schedule);
				Assert.AreEqual(Schedule.Recurrence.NONE, reward.Schedule.RequiredRecurrence);
				Assert.AreEqual(1, reward.Schedule.ActivationLimit);
			}
			{
				var expect =
					@"{
	""name"":""Test_VirtualItemReward"",
	""description"":"""",
	""itemId"":""Test_VirtualItemReward_Id"",
	""className"":""VirtualItemReward"",
	""schedule"":{
		""className"":""Schedule"",
		""schedRecurrence"":4,
		""schedApprovals"":1,
		""schedTimeRanges"":[]
	},
	""associatedItemId"":""Test_Item_Id"",
	""amount"":100
}";
				var reward = Reward.fromJSONObject(JSONObject.Create(expect)) as VirtualItemReward;
				Assert.IsNotNull(reward);
				Assert.AreEqual("Test_VirtualItemReward", reward.Name);
				Assert.AreEqual("Test_VirtualItemReward_Id", reward.ID);
				Assert.AreEqual("Test_Item_Id", reward.AssociatedItemId);
				Assert.AreEqual(100, reward.Amount);
			}
			{
				var expect =
					@"{
	""name"":""Test_SequenceReward"",
	""description"":"""",
	""itemId"":""Test_SequenceReward_Id"",
	""className"":""SequenceReward"",
	""schedule"":{
		""className"":""Schedule"",
		""schedRecurrence"":4,
		""schedApprovals"":1,
		""schedTimeRanges"":[]
	},
	""rewards"":[
		{
			""name"":""Test_BadgeReward"",
			""description"":"""",
			""itemId"":""Test_BadgeReward_Id"",
			""className"":""BadgeReward"",
			""schedule"":{
				""className"":""Schedule"",
				""schedRecurrence"":4,
				""schedApprovals"":1,
				""schedTimeRanges"":[]
			},
			""iconUrl"":""http://iconUrl""
		},
		{
			""name"":""Test_VirtualItemReward"",
			""description"":"""",
			""itemId"":""Test_VirtualItemReward_Id"",
			""className"":""VirtualItemReward"",
			""schedule"":{
				""className"":""Schedule"",
				""schedRecurrence"":4,
				""schedApprovals"":1,
				""schedTimeRanges"":[]
			},
			""associatedItemId"":""Test_Item_Id"",
			""amount"":100
		}
	]
}";

				var reward = Reward.fromJSONObject(JSONObject.Create(expect)) as SequenceReward;
				Assert.IsNotNull(reward);
				var rewards = reward.Rewards;
				Assert.IsNotNull(rewards);
				Assert.AreEqual(2, rewards.Count);
			}
		}

		[Test]
		[Category ("World fromJSONObject")]
		public void World_fromJSONObject()
		{
			var expect =
				@"{
	""name"":"""",
	""description"":"""",
	""itemId"":""Test_World_Id"",
	""className"":""World"",
	""gate"":{},
	""worlds"":[],
	""scores"":[
		{
			""name"":"""",
			""description"":"""",
			""itemId"":""Test_RangeScore_Id"",
			""className"":""RangeScore"",
			""startValue"":0,
			""higherBetter"":true,
			""range"":{
				""low"":0,
				""high"":1
			}
		},
		{
			""name"":"""",
			""description"":"""",
			""itemId"":""Test_VirtualItemScore_Id"",
			""className"":""VirtualItemScore"",
			""startValue"":0,
			""higherBetter"":true,
			""associatedItemId"":""Test_Item_Id""
		}
	],
	""missions"":[
		{
			""name"":""Test_WorldCompletionMission"",
			""description"":"""",
			""itemId"":""Test_WorldCompletionMission_Id"",
			""className"":""WorldCompletionMission"",
			""rewards"":[],
			""gate"":{
				""name"":"""",
				""description"":"""",
				""itemId"":""gate_Test_WorldCompletionMission_Id"",
				""className"":""WorldCompletionGate"",
				""associatedWorldId"":""Test_World_Id""
			},
			""schedule"":{
				""className"":""Schedule"",
				""schedRecurrence"":4,
				""schedApprovals"":1,
				""schedTimeRanges"":[]
			}
		},
		{
			""name"":""Test_BalanceMission"",
			""description"":"""",
			""itemId"":""Test_BalanceMission_Id"",
			""className"":""BalanceMission"",
			""rewards"":[],
			""gate"":{
				""name"":"""",
				""description"":"""",
				""itemId"":""gate_Test_BalanceMission_Id"",
				""className"":""BalanceGate"",
				""associatedItemId"":""Test_Item_Id"",
				""desiredBalance"":100
			},
			""schedule"":{
				""className"":""Schedule"",
				""schedRecurrence"":4,
				""schedApprovals"":1,
				""schedTimeRanges"":[]
			}
		},
		{
			""name"":""Test_PurchasingMission"",
			""description"":"""",
			""itemId"":""Test_PurchasingMission_Id"",
			""className"":""PurchasingMission"",
			""rewards"":[],
			""gate"":{
				""name"":"""",
				""description"":"""",
				""itemId"":""gate_Test_PurchasingMission_Id"",
				""className"":""PurchasableGate"",
				""associatedItemId"":""Test_Item_Id""
			},
			""schedule"":{
				""className"":""Schedule"",
				""schedRecurrence"":4,
				""schedApprovals"":1,
				""schedTimeRanges"":[]
			}
		},
		{
			""name"":""Test_SocialStatusMission"",
			""description"":"""",
			""itemId"":""Test_SocialStatusMission_Id"",
			""className"":""SocialStatusMission"",
			""rewards"":[],
			""gate"":{
				""name"":"""",
				""description"":"""",
				""itemId"":""gate_Test_SocialStatusMission_Id"",
				""className"":""SocialStatusGate"",
				""provider"":""google"",
				""status"":""Test_Status""
			},
			""schedule"":{
				""className"":""Schedule"",
				""schedRecurrence"":4,
				""schedApprovals"":1,
				""schedTimeRanges"":[]
			}
		},
		{
			""name"":""Test_Challenge"",
			""description"":"""",
			""itemId"":""Test_Challenge_Id"",
			""className"":""Challenge"",
			""rewards"":[],
			""schedule"":{
				""className"":""Schedule"",
				""schedRecurrence"":4,
				""schedApprovals"":1,
				""schedTimeRanges"":[]
			},
			""missions"":[
				{
					""name"":""Test_RecordMission"",
					""description"":"""",
					""itemId"":""Test_RecordMission_Id"",
					""className"":""RecordMission"",
					""rewards"":[],
					""gate"":{
						""name"":"""",
						""description"":"""",
						""itemId"":""gate_Test_RecordMission_Id"",
						""className"":""RecordGate"",
						""associatedScoreId"":""Test_Score_Id"",
						""desiredBalance"":100
					},
					""schedule"":{
						""className"":""Schedule"",
						""schedRecurrence"":4,
						""schedApprovals"":1,
						""schedTimeRanges"":[]
					}
				},
				{
					""name"":""Test_SocialLikeMission"",
					""description"":"""",
					""itemId"":""Test_SocialLikeMission_Id"",
					""className"":""SocialLikeMission"",
					""rewards"":[],
					""gate"":{
						""name"":"""",
						""description"":"""",
						""itemId"":""gate_Test_SocialLikeMission_Id"",
						""className"":""SocialLikeGate"",
						""provider"":""facebook"",
						""pageName"":""Test_PageName""
					},
					""schedule"":{
						""className"":""Schedule"",
						""schedRecurrence"":4,
						""schedApprovals"":1,
						""schedTimeRanges"":[]
					}
				}
			]
		}
	]
}";
			var world = World.fromJSONObject(JSONObject.Create(expect));
			Assert.IsNotNull(world);
			var innerWorlds = world.InnerWorldsMap;
			Assert.IsNotNull(innerWorlds);
			Assert.IsEmpty(innerWorlds);
			var scores = world.Scores;
			Assert.IsNotNull(scores);
			Assert.AreEqual(2, scores.Count);
			var missions = world.Missions;
			Assert.IsNotNull(missions);
			Assert.AreEqual(5, missions.Count);
		}
		
		[Test]
		[Category ("Mission fromJSONObject")]
		public void Mission_fromJSONObject()
		{
			{
				var expect =
					@"{
	""name"":""Test_RecordMission"",
	""description"":"""",
	""itemId"":""Test_RecordMission_Id"",
	""className"":""RecordMission"",
	""rewards"":[],
	""gate"":{
		""name"":"""",
		""description"":"""",
		""itemId"":""gate_Test_RecordMission_Id"",
		""className"":""RecordGate"",
		""associatedScoreId"":""Test_Score_Id"",
		""desiredBalance"":100
	},
	""schedule"":{
		""className"":""Schedule"",
		""schedRecurrence"":4,
		""schedApprovals"":1,
		""schedTimeRanges"":[]
	}
}";
				var mission = Mission.fromJSONObject(JSONObject.Create(expect)) as RecordMission;
				Assert.IsNotNull(mission);
				Assert.AreEqual("Test_RecordMission_Id", mission.ID);
				Assert.AreEqual("Test_RecordMission", mission.Name);
				var gate = mission.Gate as RecordGate;
				Assert.IsNotNull(gate);
				Assert.AreEqual("Test_Score_Id", gate.AssociatedScoreId);
				Assert.AreEqual(100, gate.DesiredRecord);
			}
			{
				var expect =
					@"{
	""name"":""Test_WorldCompletionMission"",
	""description"":"""",
	""itemId"":""Test_WorldCompletionMission_Id"",
	""className"":""WorldCompletionMission"",
	""rewards"":[],
	""gate"":{
		""name"":"""",
		""description"":"""",
		""itemId"":""gate_Test_WorldCompletionMission_Id"",
		""className"":""WorldCompletionGate"",
		""associatedWorldId"":""Test_World_Id""
	},
	""schedule"":{
		""className"":""Schedule"",
		""schedRecurrence"":4,
		""schedApprovals"":1,
		""schedTimeRanges"":[]
	}
}";
				var mission = Mission.fromJSONObject(JSONObject.Create(expect)) as WorldCompletionMission;
				Assert.IsNotNull(mission);
				Assert.AreEqual("Test_WorldCompletionMission_Id", mission.ID);
				Assert.AreEqual("Test_WorldCompletionMission", mission.Name);
				var gate = mission.Gate as WorldCompletionGate;
				Assert.IsNotNull(gate);
				Assert.AreEqual("Test_World_Id", gate.AssociatedWorldId);
			}
			{
				var expect =
					@"{
	""name"":""Test_BalanceMission"",
	""description"":"""",
	""itemId"":""Test_BalanceMission_Id"",
	""className"":""BalanceMission"",
	""rewards"":[],
	""gate"":{
		""name"":"""",
		""description"":"""",
		""itemId"":""gate_Test_BalanceMission_Id"",
		""className"":""BalanceGate"",
		""associatedItemId"":""Test_Item_Id"",
		""desiredBalance"":100
	},
	""schedule"":{
		""className"":""Schedule"",
		""schedRecurrence"":4,
		""schedApprovals"":1,
		""schedTimeRanges"":[]
	}
}";
				var mission = Mission.fromJSONObject(JSONObject.Create(expect)) as BalanceMission;
				Assert.IsNotNull(mission);
				Assert.AreEqual("Test_BalanceMission_Id", mission.ID);
				Assert.AreEqual("Test_BalanceMission", mission.Name);
				var gate = mission.Gate as BalanceGate;
				Assert.IsNotNull(gate);
				Assert.AreEqual("Test_Item_Id", gate.AssociatedItemId);
				Assert.AreEqual(100, gate.DesiredBalance);
			}
			{
				var expect =
					@"{
	""name"":""Test_PurchasingMission"",
	""description"":"""",
	""itemId"":""Test_PurchasingMission_Id"",
	""className"":""PurchasingMission"",
	""rewards"":[],
	""gate"":{
		""name"":"""",
		""description"":"""",
		""itemId"":""gate_Test_PurchasingMission_Id"",
		""className"":""PurchasableGate"",
		""associatedItemId"":""Test_Item_Id""
	},
	""schedule"":{
		""className"":""Schedule"",
		""schedRecurrence"":4,
		""schedApprovals"":1,
		""schedTimeRanges"":[]
	}
}";
				var mission = Mission.fromJSONObject(JSONObject.Create(expect)) as PurchasingMission;
				Assert.IsNotNull(mission);
				Assert.AreEqual("Test_PurchasingMission_Id", mission.ID);
				Assert.AreEqual("Test_PurchasingMission", mission.Name);
				var gate = mission.Gate as PurchasableGate;
				Assert.IsNotNull(gate);
				Assert.AreEqual("Test_Item_Id", gate.AssociatedItemId);
			}
			{
				var expect =
					@"{
	""name"":""Test_SocialLikeMission"",
	""description"":"""",
	""itemId"":""Test_SocialLikeMission_Id"",
	""className"":""SocialLikeMission"",
	""rewards"":[],
	""gate"":{
		""name"":"""",
		""description"":"""",
		""itemId"":""gate_Test_SocialLikeMission_Id"",
		""className"":""SocialLikeGate"",
		""provider"":""facebook"",
		""pageName"":""Test_PageName""
	},
	""schedule"":{
		""className"":""Schedule"",
		""schedRecurrence"":4,
		""schedApprovals"":1,
		""schedTimeRanges"":[]
	}
}";
				var mission = Mission.fromJSONObject(JSONObject.Create(expect)) as SocialLikeMission;
				Assert.IsNotNull(mission);
				Assert.AreEqual("Test_SocialLikeMission_Id", mission.ID);
				Assert.AreEqual("Test_SocialLikeMission", mission.Name);
				var gate = mission.Gate as SocialLikeGate;
				Assert.IsNotNull(gate);
				Assert.AreEqual(Provider.FACEBOOK, gate.Provider);
				Assert.AreEqual("Test_PageName", gate.PageName);
			}
			{
				var expect =
					@"{
	""name"":""Test_SocialStatusMission"",
	""description"":"""",
	""itemId"":""Test_SocialStatusMission_Id"",
	""className"":""SocialStatusMission"",
	""rewards"":[],
	""gate"":{
		""name"":"""",
		""description"":"""",
		""itemId"":""gate_Test_SocialStatusMission_Id"",
		""className"":""SocialStatusGate"",
		""provider"":""google"",
		""status"":""Test_Status""
	},
	""schedule"":{
		""className"":""Schedule"",
		""schedRecurrence"":4,
		""schedApprovals"":1,
		""schedTimeRanges"":[]
	}
}";
				var mission = Mission.fromJSONObject(JSONObject.Create(expect)) as SocialStatusMission;
				Assert.IsNotNull(mission);
				Assert.AreEqual("Test_SocialStatusMission_Id", mission.ID);
				Assert.AreEqual("Test_SocialStatusMission", mission.Name);
				var gate = mission.Gate as SocialStatusGate;
				Assert.IsNotNull(gate);
				Assert.AreEqual(Provider.GOOGLE, gate.Provider);
				Assert.AreEqual("Test_Status", gate.Status);
			}
			{
				var expect =
					@"{
	""name"":""Test_Challenge"",
	""description"":"""",
	""itemId"":""Test_Challenge_Id"",
	""className"":""Challenge"",
	""rewards"":[],
	""schedule"":{
		""className"":""Schedule"",
		""schedRecurrence"":4,
		""schedApprovals"":1,
		""schedTimeRanges"":[]
	},
	""missions"":[
		{
			""name"":""Test_RecordMission"",
			""description"":"""",
			""itemId"":""Test_RecordMission_Id"",
			""className"":""RecordMission"",
			""rewards"":[],
			""gate"":{
				""name"":"""",
				""description"":"""",
				""itemId"":""gate_Test_RecordMission_Id"",
				""className"":""RecordGate"",
				""associatedScoreId"":""Test_Score_Id"",
				""desiredBalance"":100
			},
			""schedule"":{
				""className"":""Schedule"",
				""schedRecurrence"":4,
				""schedApprovals"":1,
				""schedTimeRanges"":[]
			}
		},
		{
			""name"":""Test_SocialLikeMission"",
			""description"":"""",
			""itemId"":""Test_SocialLikeMission_Id"",
			""className"":""SocialLikeMission"",
			""rewards"":[],
			""gate"":{
				""name"":"""",
				""description"":"""",
				""itemId"":""gate_Test_SocialLikeMission_Id"",
				""className"":""SocialLikeGate"",
				""provider"":""facebook"",
				""pageName"":""Test_PageName""
			},
			""schedule"":{
				""className"":""Schedule"",
				""schedRecurrence"":4,
				""schedApprovals"":1,
				""schedTimeRanges"":[]
			}
		}
	]
}";
				var challenge = Mission.fromJSONObject(JSONObject.Create(expect)) as Challenge;
				Assert.IsNotNull(challenge);
				Assert.AreEqual("Test_Challenge_Id", challenge.ID);
				Assert.AreEqual("Test_Challenge", challenge.Name);
				var missions = challenge.Missions;
				Assert.IsNotNull(missions);
				Assert.AreEqual(2, missions.Count);
			}
		}
		
		[Test]
		[Category ("Gate fromJSONObject")]
		public void Gate_fromJSONObject()
		{
			{
				var expect =
					@"{
	""name"":"""",
	""description"":"""",
	""itemId"":""Test_RecordGate_Id"",
	""className"":""RecordGate"",
	""associatedScoreId"":""Test_Score_Id"",
	""desiredBalance"":100
}";
				var gate = Gate.fromJSONObject(JSONObject.Create(expect)) as RecordGate;
				Assert.IsNotNull(gate);
				Assert.AreEqual("Test_RecordGate_Id", gate.ID);
				Assert.AreEqual("Test_Score_Id", gate.AssociatedScoreId);
				Assert.AreEqual(100, gate.DesiredRecord);
			}
			{
				var expect =
					@"{
	""name"":"""",
	""description"":"""",
	""itemId"":""Test_ScheduleGate_Id"",
	""className"":""ScheduleGate"",
	""schedule"":{
		""className"":""Schedule"",
		""schedRecurrence"":3,
		""schedApprovals"":1,
		""schedTimeRanges"":[
			{
				""className"":""DateTimeRange"",
				""schedTimeRangeStart"":63397899010048,
				""schedTimeRangeEnd"":63429435981824
			},
			{
				""className"":""DateTimeRange"",
				""schedTimeRangeStart"":63429435981824,
				""schedTimeRangeEnd"":63460972953600
			}
		]
	}
}";
				var gate = Gate.fromJSONObject(JSONObject.Create(expect)) as ScheduleGate;
				Assert.IsNotNull(gate);
				Assert.AreEqual("Test_ScheduleGate_Id", gate.ID);
				Assert.IsNotNull(gate.Schedule);
				var schedule = gate.Schedule;
				Assert.AreEqual(Schedule.Recurrence.EVERY_HOUR, schedule.RequiredRecurrence);
				Assert.AreEqual(1, schedule.ActivationLimit);
				Assert.IsNotNull(schedule.TimeRanges);
				var timeRanges = schedule.TimeRanges;
				Assert.AreEqual(2, timeRanges.Count);
			}
			{
				var expect =
					@"{
	""name"":"""",
	""description"":"""",
	""itemId"":""Test_WorldCompletionGate_Id"",
	""className"":""WorldCompletionGate"",
	""associatedWorldId"":""Test_World_Id""
}";
				var gate = Gate.fromJSONObject(JSONObject.Create(expect)) as WorldCompletionGate;
				Assert.IsNotNull(gate);
				Assert.AreEqual("Test_WorldCompletionGate_Id", gate.ID);
				Assert.AreEqual("Test_World_Id", gate.AssociatedWorldId);
			}
			{
				var expect =
					@"{
	""name"":"""",
	""description"":"""",
	""itemId"":""Test_BalanceGate_Id"",
	""className"":""BalanceGate"",
	""associatedItemId"":""Test_Item_Id"",
	""desiredBalance"":100
}";
				var gate = Gate.fromJSONObject(JSONObject.Create(expect)) as BalanceGate;
				Assert.IsNotNull(gate);
				Assert.AreEqual("Test_BalanceGate_Id", gate.ID);
				Assert.AreEqual("Test_Item_Id", gate.AssociatedItemId);
				Assert.AreEqual(100, gate.DesiredBalance);
			}
			{
				var expect =
					@"{
	""name"":"""",
	""description"":"""",
	""itemId"":""Test_PurchasableGate_Id"",
	""className"":""PurchasableGate"",
	""associatedItemId"":""Test_Item_Id""
}";
				var gate = Gate.fromJSONObject(JSONObject.Create(expect)) as PurchasableGate;
				Assert.IsNotNull(gate);
				Assert.AreEqual("Test_PurchasableGate_Id", gate.ID);
				Assert.AreEqual("Test_Item_Id", gate.AssociatedItemId);
			}
			{
				var expect =
					@"{
	""name"":"""",
	""description"":"""",
	""itemId"":""Test_SocialLikeGate_Id"",
	""className"":""SocialLikeGate"",
	""provider"":""facebook"",
	""pageName"":""Test_PageName""
}";
				var gate = Gate.fromJSONObject(JSONObject.Create(expect)) as SocialLikeGate;
				Assert.IsNotNull(gate);
				Assert.AreEqual("Test_SocialLikeGate_Id", gate.ID);
				Assert.AreEqual(Provider.FACEBOOK, gate.Provider);
				Assert.AreEqual("Test_PageName", gate.PageName);
			}
			{
				var expect =
					@"{
	""name"":"""",
	""description"":"""",
	""itemId"":""Test_SocialStatusGate_Id"",
	""className"":""SocialStatusGate"",
	""provider"":""google"",
	""status"":""Test_Status""
}";
				var gate = Gate.fromJSONObject(JSONObject.Create(expect)) as SocialStatusGate;
				Assert.IsNotNull(gate);
				Assert.AreEqual("Test_SocialStatusGate_Id", gate.ID);
				Assert.AreEqual(Provider.GOOGLE, gate.Provider);
				Assert.AreEqual("Test_Status", gate.Status);
			}
		}
		
		[Test]
		[Category ("GateList fromJSONObject")]
		public void GateList_fromJSONObject()
		{
			{
				var expect =
					@"{
	""name"":"""",
	""description"":"""",
	""itemId"":""Test_GatesList_Id"",
	""className"":""GatesListOR"",
	""gates"":[
		{
			""name"":"""",
			""description"":"""",
			""itemId"":""Test_RecordGate_Id"",
			""className"":""RecordGate"",
			""associatedScoreId"":""Test_Score_Id"",
			""desiredBalance"":100
		},
		{
			""name"":"""",
			""description"":"""",
			""itemId"":""Test_SocialLikeGate_Id"",
			""className"":""SocialLikeGate"",
			""provider"":""facebook"",
			""pageName"":""Test_PageName""
		}
	]
}";
				var gatesList = Gate.fromJSONObject(JSONObject.Create(expect)) as GatesListOR;
				Assert.IsNotNull(gatesList);
				Assert.AreEqual("Test_GatesList_Id", gatesList.ID);
				Assert.AreEqual(2, gatesList.Count);
			}
			{
				var expect =
					@"{
	""name"":"""",
	""description"":"""",
	""itemId"":""Test_GatesList_Id"",
	""className"":""GatesListAND"",
	""gates"":[
		{
			""name"":"""",
			""description"":"""",
			""itemId"":""Test_RecordGate_Id"",
			""className"":""RecordGate"",
			""associatedScoreId"":""Test_Score_Id"",
			""desiredBalance"":100
		},
		{
			""name"":"""",
			""description"":"""",
			""itemId"":""Test_SocialLikeGate_Id"",
			""className"":""SocialLikeGate"",
			""provider"":""facebook"",
			""pageName"":""Test_PageName""
		}
	]
}";
				var gatesList = Gate.fromJSONObject(JSONObject.Create(expect)) as GatesListAND;
				Assert.IsNotNull(gatesList);
				Assert.AreEqual("Test_GatesList_Id", gatesList.ID);
				Assert.AreEqual(2, gatesList.Count);
			}
		}
		
		[Test]
		[Category ("Score fromJSONObject")]
		public void Score_fromJSONObject()
		{
			{
				var expect =
					@"{
	""name"":"""",
	""description"":"""",
	""itemId"":""Test_RangeScore_Id"",
	""className"":""RangeScore"",
	""startValue"":0,
	""higherBetter"":true,
	""range"":{
		""low"":0,
		""high"":1
	}
}";
				var score = Soomla.Levelup.Score.fromJSONObject(JSONObject.Create(expect)) as RangeScore;
				Assert.IsNotNull(score);
				Assert.AreEqual("Test_RangeScore_Id", score.ID);
				Assert.AreEqual(0, score.StartValue);
				Assert.IsTrue (score.HigherBetter);
				Assert.AreEqual(0, score.Range.Low);
				Assert.AreEqual(1, score.Range.High);
			}
			{
				var expect =
					@"{
	""name"":"""",
	""description"":"""",
	""itemId"":""Test_VirtualItemScore_Id"",
	""className"":""VirtualItemScore"",
	""startValue"":0,
	""higherBetter"":true,
	""associatedItemId"":""Test_Item_Id""
}";
				var score = Soomla.Levelup.Score.fromJSONObject(JSONObject.Create(expect)) as VirtualItemScore;
				Assert.IsNotNull(score);
				Assert.AreEqual("Test_VirtualItemScore_Id", score.ID);
				Assert.AreEqual(0, score.StartValue);
				Assert.IsTrue (score.HigherBetter);
				Assert.AreEqual("Test_Item_Id", score.AssociatedItemId);
			}
		}
		
		[Test]
		[Category ("Reward toJSONObject")]
		public void Reward_toJSONObject()
		{
			{
				var reward = new BadgeReward("Test_BadgeReward_Id", "Test_BadgeReward", "http://iconUrl");
				var json = reward.toJSONObject();
				var expect =
					@"{
	""name"":""Test_BadgeReward"",
	""description"":"""",
	""itemId"":""Test_BadgeReward_Id"",
	""className"":""BadgeReward"",
	""schedule"":{
		""className"":""Schedule"",
		""schedRecurrence"":4,
		""schedApprovals"":1,
		""schedTimeRanges"":[]
	},
	""iconUrl"":""http://iconUrl""
}";
				Assert.AreEqual(expect, json.print(true));
			}
			{
				var reward = new VirtualItemReward("Test_VirtualItemReward_Id", "Test_VirtualItemReward", "Test_Item_Id", 100);
				var json = reward.toJSONObject();
				var expect =
					@"{
	""name"":""Test_VirtualItemReward"",
	""description"":"""",
	""itemId"":""Test_VirtualItemReward_Id"",
	""className"":""VirtualItemReward"",
	""schedule"":{
		""className"":""Schedule"",
		""schedRecurrence"":4,
		""schedApprovals"":1,
		""schedTimeRanges"":[]
	},
	""associatedItemId"":""Test_Item_Id"",
	""amount"":100,
	""className"":""VirtualItemReward""
}";
				Assert.AreEqual(expect, json.print(true));
			}
			{
				var rewards = new List<Reward>();
				rewards.Add(new BadgeReward("Test_BadgeReward_Id", "Test_BadgeReward", "http://iconUrl"));
				rewards.Add(new VirtualItemReward("Test_VirtualItemReward_Id", "Test_VirtualItemReward", "Test_Item_Id", 100));
				var reward = new SequenceReward("Test_SequenceReward_Id", "Test_SequenceReward", rewards);
				var json = reward.toJSONObject();
				var expect =
					@"{
	""name"":""Test_SequenceReward"",
	""description"":"""",
	""itemId"":""Test_SequenceReward_Id"",
	""className"":""SequenceReward"",
	""schedule"":{
		""className"":""Schedule"",
		""schedRecurrence"":4,
		""schedApprovals"":1,
		""schedTimeRanges"":[]
	},
	""rewards"":[
		{
			""name"":""Test_BadgeReward"",
			""description"":"""",
			""itemId"":""Test_BadgeReward_Id"",
			""className"":""BadgeReward"",
			""schedule"":{
				""className"":""Schedule"",
				""schedRecurrence"":4,
				""schedApprovals"":1,
				""schedTimeRanges"":[]
			},
			""iconUrl"":""http://iconUrl""
		},
		{
			""name"":""Test_VirtualItemReward"",
			""description"":"""",
			""itemId"":""Test_VirtualItemReward_Id"",
			""className"":""VirtualItemReward"",
			""schedule"":{
				""className"":""Schedule"",
				""schedRecurrence"":4,
				""schedApprovals"":1,
				""schedTimeRanges"":[]
			},
			""associatedItemId"":""Test_Item_Id"",
			""amount"":100,
			""className"":""VirtualItemReward""
		}
	]
}";
				Assert.AreEqual(expect, json.print(true));
			}
		}

		[Test]
		[Category ("World toJSONObject")]
		public void World_toJSONObject()
		{
			var innerWorlds = new Dictionary<string, World>();
			var scores = new Dictionary<string, Soomla.Levelup.Score>();
			scores.Add("Test_RangeScore_Id", new RangeScore("Test_RangeScore_Id", new RangeScore.SRange(0.0, 1.0)));
			scores.Add("Test_VirtualItemScore_Id", new VirtualItemScore("Test_VirtualItemScore_Id", "Test_Item_Id"));
			var missions = new List<Mission>();
			missions.Add(new WorldCompletionMission("Test_WorldCompletionMission_Id", "Test_WorldCompletionMission", "Test_World_Id"));
			missions.Add(new BalanceMission("Test_BalanceMission_Id", "Test_BalanceMission", "Test_Item_Id", 100));
			missions.Add(new PurchasingMission("Test_PurchasingMission_Id", "Test_PurchasingMission", "Test_Item_Id"));
			missions.Add(new SocialStatusMission("Test_SocialStatusMission_Id", "Test_SocialStatusMission", Provider.GOOGLE, "Test_Status"));
			missions.Add(new Challenge("Test_Challenge_Id", "Test_Challenge", new List<Mission>() {
				new RecordMission("Test_RecordMission_Id", "Test_RecordMission", "Test_Score_Id", 100.0),
				new SocialLikeMission("Test_SocialLikeMission_Id", "Test_SocialLikeMission", Provider.FACEBOOK, "Test_PageName")
			}));
			var world = new World("Test_World_Id", null, innerWorlds, scores, missions);
			var json = world.toJSONObject();
			var expect =
				@"{
	""name"":"""",
	""description"":"""",
	""itemId"":""Test_World_Id"",
	""className"":""World"",
	""gate"":{},
	""worlds"":[],
	""scores"":[
		{
			""name"":"""",
			""description"":"""",
			""itemId"":""Test_RangeScore_Id"",
			""className"":""RangeScore"",
			""startValue"":0,
			""higherBetter"":true,
			""range"":{
				""low"":0,
				""high"":1
			}
		},
		{
			""name"":"""",
			""description"":"""",
			""itemId"":""Test_VirtualItemScore_Id"",
			""className"":""VirtualItemScore"",
			""startValue"":0,
			""higherBetter"":true,
			""associatedItemId"":""Test_Item_Id""
		}
	],
	""missions"":[
		{
			""name"":""Test_WorldCompletionMission"",
			""description"":"""",
			""itemId"":""Test_WorldCompletionMission_Id"",
			""className"":""WorldCompletionMission"",
			""rewards"":[],
			""gate"":{
				""name"":"""",
				""description"":"""",
				""itemId"":""gate_Test_WorldCompletionMission_Id"",
				""className"":""WorldCompletionGate"",
				""associatedWorldId"":""Test_World_Id""
			},
			""schedule"":{
				""className"":""Schedule"",
				""schedRecurrence"":4,
				""schedApprovals"":1,
				""schedTimeRanges"":[]
			}
		},
		{
			""name"":""Test_BalanceMission"",
			""description"":"""",
			""itemId"":""Test_BalanceMission_Id"",
			""className"":""BalanceMission"",
			""rewards"":[],
			""gate"":{
				""name"":"""",
				""description"":"""",
				""itemId"":""gate_Test_BalanceMission_Id"",
				""className"":""BalanceGate"",
				""associatedItemId"":""Test_Item_Id"",
				""desiredBalance"":100
			},
			""schedule"":{
				""className"":""Schedule"",
				""schedRecurrence"":4,
				""schedApprovals"":1,
				""schedTimeRanges"":[]
			}
		},
		{
			""name"":""Test_PurchasingMission"",
			""description"":"""",
			""itemId"":""Test_PurchasingMission_Id"",
			""className"":""PurchasingMission"",
			""rewards"":[],
			""gate"":{
				""name"":"""",
				""description"":"""",
				""itemId"":""gate_Test_PurchasingMission_Id"",
				""className"":""PurchasableGate"",
				""associatedItemId"":""Test_Item_Id""
			},
			""schedule"":{
				""className"":""Schedule"",
				""schedRecurrence"":4,
				""schedApprovals"":1,
				""schedTimeRanges"":[]
			}
		},
		{
			""name"":""Test_SocialStatusMission"",
			""description"":"""",
			""itemId"":""Test_SocialStatusMission_Id"",
			""className"":""SocialStatusMission"",
			""rewards"":[],
			""gate"":{
				""name"":"""",
				""description"":"""",
				""itemId"":""gate_Test_SocialStatusMission_Id"",
				""className"":""SocialStatusGate"",
				""provider"":""google"",
				""status"":""Test_Status""
			},
			""schedule"":{
				""className"":""Schedule"",
				""schedRecurrence"":4,
				""schedApprovals"":1,
				""schedTimeRanges"":[]
			}
		},
		{
			""name"":""Test_Challenge"",
			""description"":"""",
			""itemId"":""Test_Challenge_Id"",
			""className"":""Challenge"",
			""rewards"":[],
			""schedule"":{
				""className"":""Schedule"",
				""schedRecurrence"":4,
				""schedApprovals"":1,
				""schedTimeRanges"":[]
			},
			""missions"":[
				{
					""name"":""Test_RecordMission"",
					""description"":"""",
					""itemId"":""Test_RecordMission_Id"",
					""className"":""RecordMission"",
					""rewards"":[],
					""gate"":{
						""name"":"""",
						""description"":"""",
						""itemId"":""gate_Test_RecordMission_Id"",
						""className"":""RecordGate"",
						""associatedScoreId"":""Test_Score_Id"",
						""desiredBalance"":100
					},
					""schedule"":{
						""className"":""Schedule"",
						""schedRecurrence"":4,
						""schedApprovals"":1,
						""schedTimeRanges"":[]
					}
				},
				{
					""name"":""Test_SocialLikeMission"",
					""description"":"""",
					""itemId"":""Test_SocialLikeMission_Id"",
					""className"":""SocialLikeMission"",
					""rewards"":[],
					""gate"":{
						""name"":"""",
						""description"":"""",
						""itemId"":""gate_Test_SocialLikeMission_Id"",
						""className"":""SocialLikeGate"",
						""provider"":""facebook"",
						""pageName"":""Test_PageName""
					},
					""schedule"":{
						""className"":""Schedule"",
						""schedRecurrence"":4,
						""schedApprovals"":1,
						""schedTimeRanges"":[]
					}
				}
			]
		}
	]
}";
			Assert.AreEqual(expect, json.print(true));
		}
		
		[Test]
		[Category ("Mission toJSONObject")]
		public void Mission_toJSONObject()
		{
			{
				var mission = new RecordMission("Test_RecordMission_Id", "Test_RecordMission", "Test_Score_Id", 100.0);
				var json = mission.toJSONObject();
				var expect =
					@"{
	""name"":""Test_RecordMission"",
	""description"":"""",
	""itemId"":""Test_RecordMission_Id"",
	""className"":""RecordMission"",
	""rewards"":[],
	""gate"":{
		""name"":"""",
		""description"":"""",
		""itemId"":""gate_Test_RecordMission_Id"",
		""className"":""RecordGate"",
		""associatedScoreId"":""Test_Score_Id"",
		""desiredBalance"":100
	},
	""schedule"":{
		""className"":""Schedule"",
		""schedRecurrence"":4,
		""schedApprovals"":1,
		""schedTimeRanges"":[]
	}
}";
				Assert.AreEqual(expect, json.print(true));
			}
			{
				var mission = new WorldCompletionMission("Test_WorldCompletionMission_Id", "Test_WorldCompletionMission", "Test_World_Id");
				var json = mission.toJSONObject();
				var expect =
					@"{
	""name"":""Test_WorldCompletionMission"",
	""description"":"""",
	""itemId"":""Test_WorldCompletionMission_Id"",
	""className"":""WorldCompletionMission"",
	""rewards"":[],
	""gate"":{
		""name"":"""",
		""description"":"""",
		""itemId"":""gate_Test_WorldCompletionMission_Id"",
		""className"":""WorldCompletionGate"",
		""associatedWorldId"":""Test_World_Id""
	},
	""schedule"":{
		""className"":""Schedule"",
		""schedRecurrence"":4,
		""schedApprovals"":1,
		""schedTimeRanges"":[]
	}
}";
				Assert.AreEqual(expect, json.print(true));
			}
			{
				var mission = new BalanceMission("Test_BalanceMission_Id", "Test_BalanceMission", "Test_Item_Id", 100);
				var json = mission.toJSONObject();
				var expect =
					@"{
	""name"":""Test_BalanceMission"",
	""description"":"""",
	""itemId"":""Test_BalanceMission_Id"",
	""className"":""BalanceMission"",
	""rewards"":[],
	""gate"":{
		""name"":"""",
		""description"":"""",
		""itemId"":""gate_Test_BalanceMission_Id"",
		""className"":""BalanceGate"",
		""associatedItemId"":""Test_Item_Id"",
		""desiredBalance"":100
	},
	""schedule"":{
		""className"":""Schedule"",
		""schedRecurrence"":4,
		""schedApprovals"":1,
		""schedTimeRanges"":[]
	}
}";
				Assert.AreEqual(expect, json.print(true));
			}
			{
				var mission = new PurchasingMission("Test_PurchasingMission_Id", "Test_PurchasingMission", "Test_Item_Id");
				var json = mission.toJSONObject();
				var expect =
					@"{
	""name"":""Test_PurchasingMission"",
	""description"":"""",
	""itemId"":""Test_PurchasingMission_Id"",
	""className"":""PurchasingMission"",
	""rewards"":[],
	""gate"":{
		""name"":"""",
		""description"":"""",
		""itemId"":""gate_Test_PurchasingMission_Id"",
		""className"":""PurchasableGate"",
		""associatedItemId"":""Test_Item_Id""
	},
	""schedule"":{
		""className"":""Schedule"",
		""schedRecurrence"":4,
		""schedApprovals"":1,
		""schedTimeRanges"":[]
	}
}";
				Assert.AreEqual(expect, json.print(true));
			}
			{
				var mission = new SocialLikeMission("Test_SocialLikeMission_Id", "Test_SocialLikeMission", Provider.FACEBOOK, "Test_PageName");
				var json = mission.toJSONObject();
				var expect =
					@"{
	""name"":""Test_SocialLikeMission"",
	""description"":"""",
	""itemId"":""Test_SocialLikeMission_Id"",
	""className"":""SocialLikeMission"",
	""rewards"":[],
	""gate"":{
		""name"":"""",
		""description"":"""",
		""itemId"":""gate_Test_SocialLikeMission_Id"",
		""className"":""SocialLikeGate"",
		""provider"":""facebook"",
		""pageName"":""Test_PageName""
	},
	""schedule"":{
		""className"":""Schedule"",
		""schedRecurrence"":4,
		""schedApprovals"":1,
		""schedTimeRanges"":[]
	}
}";
				Assert.AreEqual(expect, json.print(true));
			}
			{
				var mission = new SocialStatusMission("Test_SocialStatusMission_Id", "Test_SocialStatusMission", Provider.GOOGLE, "Test_Status");
				var json = mission.toJSONObject();
				var expect =
					@"{
	""name"":""Test_SocialStatusMission"",
	""description"":"""",
	""itemId"":""Test_SocialStatusMission_Id"",
	""className"":""SocialStatusMission"",
	""rewards"":[],
	""gate"":{
		""name"":"""",
		""description"":"""",
		""itemId"":""gate_Test_SocialStatusMission_Id"",
		""className"":""SocialStatusGate"",
		""provider"":""google"",
		""status"":""Test_Status""
	},
	""schedule"":{
		""className"":""Schedule"",
		""schedRecurrence"":4,
		""schedApprovals"":1,
		""schedTimeRanges"":[]
	}
}";
				Assert.AreEqual(expect, json.print(true));
			}
			{
				var missions = new List<Mission>();
				missions.Add(new RecordMission("Test_RecordMission_Id", "Test_RecordMission", "Test_Score_Id", 100.0));
				missions.Add(new SocialLikeMission("Test_SocialLikeMission_Id", "Test_SocialLikeMission", Provider.FACEBOOK, "Test_PageName"));
				var challenge = new Challenge("Test_Challenge_Id", "Test_Challenge", missions);
				var json = challenge.toJSONObject();
				var expect =
					@"{
	""name"":""Test_Challenge"",
	""description"":"""",
	""itemId"":""Test_Challenge_Id"",
	""className"":""Challenge"",
	""rewards"":[],
	""schedule"":{
		""className"":""Schedule"",
		""schedRecurrence"":4,
		""schedApprovals"":1,
		""schedTimeRanges"":[]
	},
	""missions"":[
		{
			""name"":""Test_RecordMission"",
			""description"":"""",
			""itemId"":""Test_RecordMission_Id"",
			""className"":""RecordMission"",
			""rewards"":[],
			""gate"":{
				""name"":"""",
				""description"":"""",
				""itemId"":""gate_Test_RecordMission_Id"",
				""className"":""RecordGate"",
				""associatedScoreId"":""Test_Score_Id"",
				""desiredBalance"":100
			},
			""schedule"":{
				""className"":""Schedule"",
				""schedRecurrence"":4,
				""schedApprovals"":1,
				""schedTimeRanges"":[]
			}
		},
		{
			""name"":""Test_SocialLikeMission"",
			""description"":"""",
			""itemId"":""Test_SocialLikeMission_Id"",
			""className"":""SocialLikeMission"",
			""rewards"":[],
			""gate"":{
				""name"":"""",
				""description"":"""",
				""itemId"":""gate_Test_SocialLikeMission_Id"",
				""className"":""SocialLikeGate"",
				""provider"":""facebook"",
				""pageName"":""Test_PageName""
			},
			""schedule"":{
				""className"":""Schedule"",
				""schedRecurrence"":4,
				""schedApprovals"":1,
				""schedTimeRanges"":[]
			}
		}
	]
}";
				Assert.AreEqual(expect, json.print(true));
			}
		}
		
		[Test]
		[Category ("Gate toJSONObject")]
		public void Gate_toJSONObject()
		{
			{
				var gate = new RecordGate("Test_RecordGate_Id", "Test_Score_Id", 100.0);
				var json = gate.toJSONObject();
				var expect =
					@"{
	""name"":"""",
	""description"":"""",
	""itemId"":""Test_RecordGate_Id"",
	""className"":""RecordGate"",
	""associatedScoreId"":""Test_Score_Id"",
	""desiredBalance"":100
}";
				Assert.AreEqual(expect, json.print(true));
			}
			{
				List<Schedule.DateTimeRange> timeRanges = new List<Schedule.DateTimeRange>();
				timeRanges.Add(new Schedule.DateTimeRange(DateTime.Parse("01/01/2010"), DateTime.Parse("01/01/2011")));
				timeRanges.Add(new Schedule.DateTimeRange(DateTime.Parse("01/01/2011"), DateTime.Parse("01/01/2012")));
				var schedule = new Schedule(timeRanges, Schedule.Recurrence.EVERY_HOUR, 1);
				var gate = new ScheduleGate("Test_ScheduleGate_Id", schedule);
				var json = gate.toJSONObject();
				var expect =
					@"{
	""name"":"""",
	""description"":"""",
	""itemId"":""Test_ScheduleGate_Id"",
	""className"":""ScheduleGate"",
	""schedule"":{
		""className"":""Schedule"",
		""schedRecurrence"":3,
		""schedApprovals"":1,
		""schedTimeRanges"":[
			{
				""className"":""DateTimeRange"",
				""schedTimeRangeStart"":63397899010048,
				""schedTimeRangeEnd"":63429435981824
			},
			{
				""className"":""DateTimeRange"",
				""schedTimeRangeStart"":63429435981824,
				""schedTimeRangeEnd"":63460972953600
			}
		]
	}
}";
				Assert.AreEqual(expect, json.print(true));
			}
			{
				var gate = new WorldCompletionGate("Test_WorldCompletionGate_Id", "Test_World_Id");
				var json = gate.toJSONObject();
				var expect =
					@"{
	""name"":"""",
	""description"":"""",
	""itemId"":""Test_WorldCompletionGate_Id"",
	""className"":""WorldCompletionGate"",
	""associatedWorldId"":""Test_World_Id""
}";
				Assert.AreEqual(expect, json.print(true));
			}
			{
				var gate = new BalanceGate("Test_BalanceGate_Id", "Test_Item_Id", 100);
				var json = gate.toJSONObject();
				var expect =
					@"{
	""name"":"""",
	""description"":"""",
	""itemId"":""Test_BalanceGate_Id"",
	""className"":""BalanceGate"",
	""associatedItemId"":""Test_Item_Id"",
	""desiredBalance"":100
}";
				Assert.AreEqual(expect, json.print(true));
			}
			{
				var gate = new PurchasableGate("Test_PurchasableGate_Id", "Test_Item_Id");
				var json = gate.toJSONObject();
				var expect =
					@"{
	""name"":"""",
	""description"":"""",
	""itemId"":""Test_PurchasableGate_Id"",
	""className"":""PurchasableGate"",
	""associatedItemId"":""Test_Item_Id""
}";
				Assert.AreEqual(expect, json.print(true));
			}
			{
				var gate = new SocialLikeGate("Test_SocialLikeGate_Id", Provider.FACEBOOK, "Test_PageName");
				var json = gate.toJSONObject();
				var expect =
					@"{
	""name"":"""",
	""description"":"""",
	""itemId"":""Test_SocialLikeGate_Id"",
	""className"":""SocialLikeGate"",
	""provider"":""facebook"",
	""pageName"":""Test_PageName""
}";
				Assert.AreEqual(expect, json.print(true));
			}
			{
				var gate = new SocialStatusGate("Test_SocialStatusGate_Id", Provider.GOOGLE, "Test_Status");
				var json = gate.toJSONObject();
				var expect =
					@"{
	""name"":"""",
	""description"":"""",
	""itemId"":""Test_SocialStatusGate_Id"",
	""className"":""SocialStatusGate"",
	""provider"":""google"",
	""status"":""Test_Status""
}";
				Assert.AreEqual(expect, json.print(true));
			}
		}
		
		[Test]
		[Category ("GatesList toJSONObject")]
		public void GatesList_toJSONObject()
		{
			{
				List<Gate> gates = new List<Gate>();
				gates.Add(new RecordGate("Test_RecordGate_Id", "Test_Score_Id", 100.0));
				gates.Add(new SocialLikeGate("Test_SocialLikeGate_Id", Provider.FACEBOOK, "Test_PageName"));
				var gatesList = new GatesListOR("Test_GatesList_Id", gates);
				var json = gatesList.toJSONObject();
				var expect =
					@"{
	""name"":"""",
	""description"":"""",
	""itemId"":""Test_GatesList_Id"",
	""className"":""GatesListOR"",
	""gates"":[
		{
			""name"":"""",
			""description"":"""",
			""itemId"":""Test_RecordGate_Id"",
			""className"":""RecordGate"",
			""associatedScoreId"":""Test_Score_Id"",
			""desiredBalance"":100
		},
		{
			""name"":"""",
			""description"":"""",
			""itemId"":""Test_SocialLikeGate_Id"",
			""className"":""SocialLikeGate"",
			""provider"":""facebook"",
			""pageName"":""Test_PageName""
		}
	]
}";
				Assert.AreEqual(expect, json.print(true));
			}
			{
				List<Gate> gates = new List<Gate>();
				gates.Add(new RecordGate("Test_RecordGate_Id", "Test_Score_Id", 100.0));
				gates.Add(new SocialLikeGate("Test_SocialLikeGate_Id", Provider.FACEBOOK, "Test_PageName"));
				var gatesList = new GatesListAND("Test_GatesList_Id", gates);
				var json = gatesList.toJSONObject();
				var expect =
					@"{
	""name"":"""",
	""description"":"""",
	""itemId"":""Test_GatesList_Id"",
	""className"":""GatesListAND"",
	""gates"":[
		{
			""name"":"""",
			""description"":"""",
			""itemId"":""Test_RecordGate_Id"",
			""className"":""RecordGate"",
			""associatedScoreId"":""Test_Score_Id"",
			""desiredBalance"":100
		},
		{
			""name"":"""",
			""description"":"""",
			""itemId"":""Test_SocialLikeGate_Id"",
			""className"":""SocialLikeGate"",
			""provider"":""facebook"",
			""pageName"":""Test_PageName""
		}
	]
}";
				Assert.AreEqual(expect, json.print(true));
			}
		}
		
		[Test]
		[Category ("Score toJSONObject")]
		public void Score_toJSONObject()
		{
			{
				var score = new RangeScore("Test_RangeScore_Id", new RangeScore.SRange(0.0, 1.0));
				var json = score.toJSONObject();
				var expect =
					@"{
	""name"":"""",
	""description"":"""",
	""itemId"":""Test_RangeScore_Id"",
	""className"":""RangeScore"",
	""startValue"":0,
	""higherBetter"":true,
	""range"":{
		""low"":0,
		""high"":1
	}
}";
				Assert.AreEqual(expect, json.print(true));
			}
			{
				var score = new VirtualItemScore("Test_VirtualItemScore_Id", "Test_Item_Id");
				var json = score.toJSONObject();
				var expect =
					@"{
	""name"":"""",
	""description"":"""",
	""itemId"":""Test_VirtualItemScore_Id"",
	""className"":""VirtualItemScore"",
	""startValue"":0,
	""higherBetter"":true,
	""associatedItemId"":""Test_Item_Id""
}";
				Assert.AreEqual(expect, json.print(true));
			}
		}
	}
}
