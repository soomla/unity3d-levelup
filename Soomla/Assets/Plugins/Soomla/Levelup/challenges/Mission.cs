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

		public Mission(JSONObject jsonObj) {
			this.MissionId = jsonObj[LUJSONConsts.LU_MISSION_MISSIONID].str;
			if (jsonObj[JSONConsts.SOOM_NAME]) {
				this.Name = jsonObj[JSONConsts.SOOM_NAME].str;
			} else {
				this.Name = "";
			}

			this.Rewards = new List<Reward>();
			List<JSONObject> jsonRewardList = jsonObj [JSONConsts.SOOM_REWARDS].list;
			foreach (JSONObject jsonRewardObj in jsonRewardList) {
				this.Rewards.Add(Reward.fromJSONObject(jsonRewardObj));
			}
		}

		public virtual JSONObject toJSONObject() {
			JSONObject obj = new JSONObject(JSONObject.Type.OBJECT);
			obj.AddField(JSONConsts.SOOM_NAME, this.Name);
			obj.AddField(LUJSONConsts.LU_MISSION_MISSIONID, this.MissionId);
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

		// Equality
		
		public override bool Equals(System.Object obj)
		{
			// If parameter is null return false.
			if (obj == null)
			{
				return false;
			}
			
			// If parameter cannot be cast to Point return false.
			Mission m = obj as Mission;
			if ((System.Object)m == null)
			{
				return false;
			}
			
			// Return true if the fields match:
			return (MissionId == m.MissionId);
		}
		
		public bool Equals(Mission m)
		{
			// If parameter is null return false:
			if ((object)m == null)
			{
				return false;
			}
			
			// Return true if the fields match:
			return (MissionId == m.MissionId);
		}
		
		public override int GetHashCode()
		{
			return MissionId.GetHashCode();
		}

		public static bool operator ==(Mission a, Mission b)
		{
			// If both are null, or both are same instance, return true.
			if (System.Object.ReferenceEquals(a, b))
			{
				return true;
			}
			
			// If one is null, but not both, return false.
			if (((object)a == null) || ((object)b == null))
			{
				return false;
			}
			
			// Return true if the fields match:
			return a.MissionId == b.MissionId;
		}
		
		public static bool operator !=(Mission a, Mission b)
		{
			return !(a == b);
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

		protected virtual void registerEvents() {

		}

		protected virtual void unregisterEvents() {

		}

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

