/// Copyright (C) 2012-2014 Soomla Inc.
///
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///      http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.

using UnityEngine;
using System;
using System.Collections.Generic;

namespace Soomla.Levelup {
	
	public abstract class Mission {

//#if UNITY_IOS && !UNITY_EDITOR
//		[DllImport ("__Internal")]
//		private static extern int storeAssets_Save(string type, string viJSON);
//#endif

		private const string TAG = "SOOMLA Mission";
		
		public string Name;
		public string MissionId;
		public List<Reward> Rewards;

		protected Mission (String missionId, String name)
		{
			this.Name = name;
			this.MissionId = missionId;
			this.Rewards = new List<Reward>();

			registerEvents();
		}

		protected Mission (String missionId, String name, List<Reward> rewards)
		{
			this.Name = name;
			this.MissionId = missionId;
			this.Rewards = rewards;
			
			registerEvents();
		}
		
//#if UNITY_ANDROID && !UNITY_EDITOR
//		protected Mission(AndroidJavaObject jniVirtualItem) {
//			this.Name = jniVirtualItem.Call<string>("getName");
//			this.Description = jniVirtualItem.Call<string>("getDescription");
//			this.ItemId = jniVirtualItem.Call<string>("getItemId");
//		}
//#endif

		protected Mission(JSONObject jsonObj) {
			this.MissionId = jsonObj[LUJSONConsts.LU_MISSION_MISSIONID];
			if (jsonObj[JSONConsts.SOOM_NAME]) {
				this.Name = jsonItem[JSONConsts.SOOM_NAME].str;
			} else {
				this.Name = "";
			}

			List<JSONObject> rewardsObjs = jsonObj[JSONConsts.SOOM_REWARDS].list;
			this.Rewards = new List<Reward>();
			foreach(JSONObject rewardJson in rewardsObjs) {
				this.Rewards.Add(Reward.fromJSONObject(rewardJson));
			}
		}

		public virtual JSONObject toJSONObject() {
			JSONObject obj = new JSONObject(JSONObject.Type.OBJECT);
			obj.AddField(JSONConsts.ITEM_NAME, this.Name);
			obj.AddField(LUJSONConsts.LU_MISSION_MISSIONID, this.MissionId);

			JSONObject rewardsArr = new JSONObject(JSONObject.Type.ARRAY);
			foreach(Reward reward in this.Rewards) {
				rewardsArr.Add(reward.toJSONObject());
			}
			obj.AddField(JSONConsts.SOOM_REWARDS, rewardsArr);
			
			return obj;
		}

		public static Mission fromJSONObject(JSONObject missionObj) {
			string className = missionObj[JSONConsts.SOOM_CLASSNAME].str;
			
			Mission mission = (Mission) Activator.CreateInstance(Type.GetType("Soomla.Levelup." + className), new object[] { missionObj });
			
			return mission;
		}

		protected void registerEvents() {
			if (!IsCompleted()) {
				// TODO: register for events
			}
		}

		protected void unregisterEvents() {
			// TODO: unregister for events
		}

		public bool IsCompleted() {
			// TODO: check if completed on iOS and Android
		}

		public void SetCompleted(bool completed) {
			// TODO: set comleted on iOS and Android

			if (completed) {
				// events not interesting until revoked
				unregisterEvents();
				giveRewards();
			} else {
				takeRewards();
				// listen again for chance to be completed
				registerEvents();
			}
		}

		protected void takeRewards() {
			foreach (Reward reward in Rewards) {
				reward.Take();
			}
		}
		
		protected void giveRewards() {
			// The mission is completed, giving the rewards.
			for(Reward reward : mRewards) {
				reward.give();
			}
		}
	}
}

