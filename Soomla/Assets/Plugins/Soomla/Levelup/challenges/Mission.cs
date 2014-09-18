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
	/// A <c>Mission</c> is a task your users need to complete in your game. Each <c>Mission</c>s is 
	/// associated with a <c>Gate</c> that defines the criteria for completing the <c>Mission</c>.
	/// You can optionally choose to give a <c>Reward</c> for users that complete your <c>Mission</c>.
	/// Create <c>Mission</c>s and use them as single, independent, entities OR you can create a 
	/// <c>Challenge</c> to handle several <c>Mission</c>s and monitor their completion.
	/// NOTE: <c>Mission</c>s cab be completed multiple times.
	/// </summary>
	public abstract class Mission : SoomlaEntity<Mission> {

		private const string TAG = "SOOMLA Mission";

		/// <summary>
		/// <c>Reward</c>s that can be earned when completing this mission.
		/// </summary>
		public List<Reward> Rewards;

		/// <summary>
		/// <c>Schedule</c> that defines the number of times this <c>Mission</c> can be played, how often, etc.
		/// </summary>
		public Schedule Schedule;

		/// <summary>
		/// The <c>Gate</c> that needs to be opened in order to complete this <c>Mission</c>.
		/// </summary>
		public Gate Gate;


		/// <summary>
		/// Generates a gate ID for this <c>Mission</c>.
		/// </summary>
		/// <value>"gate_" followed by this <c>Mission</c>'s ID.</value>
		public string AutoGateId {
			get { return "gate_" + this._id; }
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="name">Name.</param>
		protected Mission (String id, String name) 
			: this(id, name, null, null)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="name">Name.</param>
		/// <param name="rewards"><c>Reward</c>s for completing this <c>Mission</c>.</param>
		protected Mission (String id, String name, List<Reward> rewards) 
			: this(id, name, rewards, null, null)
		{
		}

		/// <summary>
		/// Constructor. 
		/// </summary>
		//// <param name="id">ID.</param>
		/// <param name="name">Name.</param>
		/// <param name="gateType"><c>Gate</c> to open to complete this <c>Mission</c></param>
		/// <param name="gateInitParams">Parameters to initialize <c>Gate</c>.</param>
		protected Mission (String id, String name, Type gateType, object[] gateInitParams)
			: this(id, name, new List<Reward>(), gateType, gateInitParams)
		{
		}

		/// <summary>
		/// Constructor. 
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="name">Name.</param>
		/// <param name="rewards"><c>Reward</c>s.</param>
		/// <param name="gateType"><c>Gate</c> to open to complete this <c>Mission</c></param>
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
			registerEvents();
		}

		/// <summary>
		/// Converts this <c>Mission</c> into a JSONObject.
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
		/// Converts the given JSONObject into a <c>Mission</c>. 
		/// </summary>
		/// <returns>The JSON object.</returns>
		/// <param name="missionObj">JSON object.</param>
		public static Mission fromJSONObject(JSONObject missionObj) {
			string className = missionObj[JSONConsts.SOOM_CLASSNAME].str;
			
			Mission mission = (Mission) Activator.CreateInstance(Type.GetType("Soomla.Levelup." + className), new object[] { missionObj });
			
			return mission;
		}

		/// <summary>
		/// Determines whether this <c>Mission</c> is available by checking the criteria 
		/// that makes the specific <c>Mission</c> available. 
		/// </summary>
		/// <returns>If this instance is available returns <c>true</c>; otherwise <c>false</c>.</returns>
		public virtual bool IsAvailable() {
			return Gate.CanOpen() && Schedule.Approve(MissionStorage.GetTimesCompleted(this));
		}

		/// <summary>
		/// Checks if this <c>Mission</c> has ever been completed - no matter how many times. 
		/// </summary>
		/// <returns>If this instance is completed returns <c>true</c>; otherwise <c>false</c>.</returns>
		public virtual bool IsCompleted() {
			return MissionStorage.IsCompleted (this);
		}

		/// <summary>
		/// Completes this <c>Mission</c>'s by opening its <c>Gate</c>.
		/// </summary>
		/// <returns>If <c>Schedule</c> doesn't approve, the mission cannot be completed
		/// and thus returns <c>false</c>; otherwise opens this <c>Mission</c>'s
		/// <c>Gate</c> and returns <c>true</c> if successful.</returns>
		public bool Complete() {
			if (!Schedule.Approve(MissionStorage.GetTimesCompleted(this))) {
				SoomlaUtils.LogDebug(TAG, "missions cannot be completed b/c Schedule doesn't approve.");
				return false;
			}
			SoomlaUtils.LogDebug(TAG, "trying opening gate to complete mission: " + ID);
			return Gate.Open();
		}

		/// <summary>
		/// Forces completion of this <c>Mission</c>.
		/// This function should not be used in standard scenarios.
		/// </summary>
		public void ForceComplete() {
			Gate.ForceOpen(true);
		}

		/// <summary>
		/// Clones this <c>Mission</c> and gives it the given ID.
		/// </summary>
		/// <param name="newMissionId">Cloned mission ID.</param>
		public override Mission Clone(string newMissionId) {
			return (Mission) base.Clone(newMissionId);
		}

		/// <summary>
		/// Sets this <c>Mission</c> as completed and gives or takes <c>Reward</c>s according
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
		/// Registers relevant events: <c>OnGateOpened</c>.
		/// </summary>
		protected virtual void registerEvents() {
			if (!IsCompleted() && this.Gate != null) {
				LevelUpEvents.OnGateOpened += onGateOpened;
			}
		}
		
		/// <summary>
		/// Sets this <c>Mission</c> as completed if the <c>Gate</c> that was opened is 
		/// this <c>Mission</c>'s gate.
		/// </summary>
		/// <param name="gate">The <c>Gate</c> that was opened.</param>
		private void onGateOpened(Gate gate) {
			if(this.Gate == gate) {
				Gate.ForceOpen(false);
				setCompletedInner(true);
			}
		}

		/// <summary>
		/// Takes this <c>Mission</c>'s <c>Reward</c>s.
		/// </summary>
		private void takeRewards() {
			foreach (Reward reward in Rewards) {
				reward.Take();
			}
		}

		/// <summary>
		/// Gives this <c>Mission</c>'s <c>Reward</c>s.
		/// </summary>
		private void giveRewards() {
			foreach (Reward reward in Rewards) {
				reward.Give();
			}
		}

	}
}

