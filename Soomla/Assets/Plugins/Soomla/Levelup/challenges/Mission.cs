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
using System.Linq;

namespace Soomla.Levelup {

	/// <summary>
	/// A mission is a task your users need to complete in your game. Missions are usually 
	/// associated with rewards meaning that you can give your users something for completing 
	/// missions. Create missions and use them as single, independent, entities OR you can  
	/// create a <c>Challenge</c> to handle several missions and monitor their completion.
	/// NOTE: We are allowing missions to be completed multiple times.
	/// </summary>
	public abstract class Mission : SoomlaEntity<Mission> {

		private const string TAG = "SOOMLA Mission";

		public List<Reward> Rewards;
		public Schedule Schedule;
		protected Gate Gate;

		/// <summary>
		/// Generates a gate ID for this mission.
		/// </summary>
		/// <value>"gate_" followed by this mission's ID.</value>
		public string AutoGateId {
			get { return "gate_" + this._id; }
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="id">Mission ID.</param>
		/// <param name="name">Mission name.</param>
		protected Mission (String id, String name) 
			: this(id, name, null, null)
		{
		}

		/// <summary>
		/// Constructor for mission with rewards.
		/// </summary>
		/// <param name="id">Mission ID.</param>
		/// <param name="name">Mission name.</param>
		/// <param name="rewards">Rewards for completing this mission.</param>
		protected Mission (String id, String name, List<Reward> rewards) 
			: this(id, name, rewards, null, null)
		{
		}

		/// <summary>
		/// Constructor for mission with a gate. 
		/// </summary>
		//// <param name="id">Mission ID.</param>
		/// <param name="name">Mission name.</param>
		/// <param name="gateType">Gate type.</param>
		/// <param name="gateInitParams">Parameters to initialize gate.</param>
		protected Mission (String id, String name, Type gateType, object[] gateInitParams)
			: this(id, name, new List<Reward>(), gateType, gateInitParams)
		{
		}

		/// <summary>
		/// Constructor for mission with a gate and rewards. 
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="name">Name.</param>
		/// <param name="rewards">Rewards.</param>
		/// <param name="gateType">Gate type.</param>
		/// <param name="gateInitParams">Gate init parameters.</param>
		protected Mission (String id, String name, List<Reward> rewards, Type gateType, object[] gateInitParams)
			: base(id, name, "")
		{
			this.Rewards = rewards;
			if (gateType != null) {
				this.Gate = (Soomla.Levelup.Gate) Activator.CreateInstance(gateType, new object[] { AutoGateId }.Concat(gateInitParams).ToArray());
			}

			Schedule = Schedule.AnyTimeOnce();

			registerEvents();
		}

		/// <summary>
		/// Constructor. 
		/// Generates an instance of <c>Mission</c> from the given JSONObject.
		/// </summary>
		/// <param name="jsonObj">JSON object.</param>
		public Mission(JSONObject jsonObj)
			: base(jsonObj)
		{
			this.Rewards = new List<Reward>();
			List<JSONObject> jsonRewardList = jsonObj [JSONConsts.SOOM_REWARDS].list;
			foreach (JSONObject jsonRewardObj in jsonRewardList) {
				this.Rewards.Add(Reward.fromJSONObject(jsonRewardObj));
			}

			this.Gate = Gate.fromJSONObject(jsonObj[LUJSONConsts.LU_GATE]);
			if (jsonObj[JSONConsts.SOOM_SCHEDULE]) {
				this.Schedule = new Schedule(jsonObj[JSONConsts.SOOM_SCHEDULE]);
			}
		}

		/// <summary>
		/// Converts this mission into a JSONObject.
		/// </summary>
		/// <returns>The JSON object.</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();

			JSONObject rewardsArr = new JSONObject(JSONObject.Type.ARRAY);
			foreach(Reward reward in this.Rewards) {
				rewardsArr.Add(reward.toJSONObject());
			}
			obj.AddField(JSONConsts.SOOM_REWARDS, rewardsArr);

			obj.AddField(LUJSONConsts.LU_GATE, Gate.toJSONObject());
			obj.AddField(JSONConsts.SOOM_SCHEDULE, Schedule.toJSONObject());

			return obj;
		}

		/// <summary>
		/// Converts the given JSONObject into a Mission. 
		/// </summary>
		/// <returns>The JSON object.</returns>
		/// <param name="missionObj">Mission object.</param>
		public static Mission fromJSONObject(JSONObject missionObj) {
			string className = missionObj[JSONConsts.SOOM_CLASSNAME].str;
			
			Mission mission = (Mission) Activator.CreateInstance(Type.GetType("Soomla.Levelup." + className), new object[] { missionObj });
			
			return mission;
		}

#if UNITY_ANDROID && !UNITY_EDITOR
		public AndroidJavaObject toJNIObject() {
			using(AndroidJavaClass jniClass = new AndroidJavaClass("com.soomla.levelup.challenges.Mission")) {
				string json = toJSONObject().print();
				SoomlaUtils.LogError(TAG, "json:"+json);
				return jniClass.CallStatic<AndroidJavaObject>("fromJSONString", json);
			}
		}
#endif

		/// <summary>
		/// Registers relevant events. Each specific type of Mission must implement this method. 
		/// </summary>
		protected virtual void registerEvents() {
			if (!IsCompleted() && this.Gate != null) {
				LevelUpEvents.OnGateOpened += onGateOpened;
			}
		}

		/// <summary>
		/// Sets this mission as completed if the gate that was opened in the gate-opened
		/// event is this mission's gate.
		/// </summary>
		/// <param name="gate">The gate that was opened.</param>
		private void onGateOpened(Gate gate) {
			if(this.Gate == gate) {
				Gate.ForceOpen(false);
				setCompletedInner(true);
			}
		}

		/// <summary>
		/// Determines whether this mission is available by checking that its gate can be 
		/// opened and also that its schedule is approved.
		/// </summary>
		/// <returns>If this instance is available returns <c>true</c>; otherwise <c>false</c>.</returns>
		public virtual bool IsAvailable() {
			return Gate.CanOpen() && Schedule.Approve(MissionStorage.GetTimesCompleted(this));
		}

		/// <summary>
		/// Checks if this mission has ever been completed - no matter how many times. 
		/// </summary>
		/// <returns>If this instance is completed returns <c>true</c>; otherwise <c>false</c>.</returns>
		public virtual bool IsCompleted() {
			return MissionStorage.IsCompleted (this);
		}

		/// <summary>
		/// Completes this mission be opening its gate. 
		/// </summary>
		/// <returns>If the schedule doesn't approve the mission cannot be completed
		/// and thus returns <c>false</c>; otherwise <c>true</c>.</returns>
		public bool Complete() {
			if (!Schedule.Approve(MissionStorage.GetTimesCompleted(this))) {
				SoomlaUtils.LogDebug(TAG, "missions cannot be completed b/c Schedule doesn't approve.");
				return false;
			}
			SoomlaUtils.LogDebug(TAG, "trying opening gate to complete mission: " + ID);
			return Gate.Open();
		}

		/// <summary>
		/// Forces completion of this mission without checking the schedule.
		/// This function should not be used in standard scenarios.
		/// </summary>
		public void ForceComplete() {
			Gate.ForceOpen(true);
		}

		/// <summary>
		/// Sets this mission as completed and gives or takes rewards according
		/// to the given <c>completed</c> value.
		/// </summary>
		/// <param name="completed">If set to <c>true</c> gives rewards.</param>
		protected void setCompletedInner(bool completed) {
			// set completed in Mission Storage
			MissionStorage.SetCompleted (this, completed);

			if (completed) {
				giveRewards();
			} else {
				takeRewards();
			}
		}

		/// <summary>
		/// Takes this mission's rewards.
		/// </summary>
		private void takeRewards() {
			foreach (Reward reward in Rewards) {
				reward.Take();
			}
		}

		/// <summary>
		/// Gives this mission's rewards.
		/// </summary>
		private void giveRewards() {
			foreach (Reward reward in Rewards) {
				reward.Give();
			}
		}

		/// <summary>
		/// Clones this mission and gives it the given ID.
		/// </summary>
		/// <param name="newMissionId">Cloned mission ID.</param>
		public override Mission Clone(string newMissionId) {
			return (Mission) base.Clone(newMissionId);
		}
	}
}

