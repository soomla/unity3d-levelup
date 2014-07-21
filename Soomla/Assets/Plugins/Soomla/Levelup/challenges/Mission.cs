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
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Levelup {
	
	public abstract class Mission : SoomlaEntity {

//#if UNITY_IOS && !UNITY_EDITOR
//		[DllImport ("__Internal")]
//		private static extern int storeAssets_Save(string type, string viJSON);
//#endif

		private const string TAG = "SOOMLA Mission";

		public List<Reward> Rewards;

		protected Mission (String id, String name)
			: this(id, name, new List<Reward>())
		{
		}

		protected Mission (String id, String name, List<Reward> rewards)
			: base(id, name, "")
		{
			this.Rewards = rewards;
			
			registerEvents();
		}

		public Mission(JSONObject jsonObj)
			: base(jsonObj)
		{
			this.Rewards = new List<Reward>();
			List<JSONObject> jsonRewardList = jsonObj [JSONConsts.SOOM_REWARDS].list;
			foreach (JSONObject jsonRewardObj in jsonRewardList) {
				this.Rewards.Add(Reward.fromJSONObject(jsonRewardObj));
			}
		}

		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();
			obj.AddField(JSONConsts.SOOM_CLASSNAME, GetType().Name);

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

#if UNITY_ANDROID 
		//&& !UNITY_EDITOR
		public AndroidJavaObject toJNIObject() {
			using(AndroidJavaClass jniClass = new AndroidJavaClass("com.soomla.levelup.challenges.Mission")) {
				string json = toJSONObject().print();
				SoomlaUtils.LogError(TAG, "json:"+json);
				return jniClass.CallStatic<AndroidJavaObject>("fromJSONString", json);
			}
		}
#endif

		protected abstract void registerEvents();

		protected abstract void unregisterEvents();

		public virtual bool IsCompleted() {
			// check if completed in Mission Storage
			return MissionStorage.IsCompleted (this);
		}

		public void SetCompleted(bool completed) {
			// set completed in Mission Storage
			MissionStorage.SetCompleted (this, completed);

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
			foreach (Reward reward in Rewards) {
				reward.Give();
			}
		}
	}
}

