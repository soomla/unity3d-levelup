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
/// limitations under the License.using System;

using System.Collections;
using System.Collections.Generic;

namespace Soomla.Levelup
{
	/// <summary>
	/// A challenge is a specific type of <c>Mission</c> which holds a collection
	/// of missions. The user is required to complete all these missions in order  
	/// to earn the reward associated with the challenge.
	/// </summary>
	public class Challenge : Mission
	{
		private const string TAG = "SOOMLA Challenge";

		public List<Mission> Missions = new List<Mission>();

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="id">Challenge ID.</param>
		/// <param name="name">Challenge name.</param>
		/// <param name="missions">Missions that belong to this Challenge.</param>
		public Challenge(string id, string name, List<Mission> missions)
			: base(id, name)
		{
			Missions = missions;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="id">Challenge ID.</param>
		/// <param name="name">Challenge name.</param>
		/// <param name="missions">Missions that belong to this Challenge.</param>
		/// <param name="rewards">Rewards associated with this Challenge.</param>
		public Challenge(string id, string name, List<Mission> missions, List<Reward> rewards)
			: base(id, name, rewards)
		{
			Missions = missions;
		}
		
		/// <summary>
		/// Constructor. 
		/// Generates an instance of <c>Challenge</c> from the given JSONObject.
		/// </summary>
		/// <param name="jsonMission">JSON mission.</param>
		public Challenge(JSONObject jsonMission)
			: base(jsonMission)
		{
			Missions = new List<Mission>();
			List<JSONObject> missionsJSON = jsonMission[LUJSONConsts.LU_MISSIONS].list;
			foreach(JSONObject missionJSON in missionsJSON) {
				Missions.Add(Mission.fromJSONObject(missionJSON));
			}
		}
		
		/// <summary>
		/// Converts this challenge to a JSONObject.
		/// </summary>
		/// <returns>The JSON object.</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();

			JSONObject missionsJSON = new JSONObject(JSONObject.Type.ARRAY);
			foreach (Mission mission in Missions) {			
				missionsJSON.Add(mission.toJSONObject());
			}
			obj.AddField(LUJSONConsts.LU_MISSIONS, missionsJSON);		

			return obj;
		}

		/// <summary>
		/// Checks if this mission has ever been completed - no matter how many times.
		/// </summary>
		/// <returns>If this instance is completed returns <c>true</c>; 
		/// otherwise <c>false</c>.</returns>
		public override bool IsCompleted() {
			// Scenario that could happen in construction - need to return false 
			// in order to register for child events.
			if(Missions == null || Missions.Count == 0) {
				return false;
			}
			
			foreach (Mission mission in Missions) {
				if (!mission.IsCompleted()) {
					return false;
				}
			}
			
			return true;
		}

		/// <summary>
		/// Handles mission completion events. Checks if all missions included
		/// in the challenge are completed, and if so, sets the challenge as completed.
		/// </summary>
		/// <param name="completedMission">Completed mission.</param>
		/// @Subscribe
		public void onMissionCompleted(Mission completedMission) {
			SoomlaUtils.LogDebug (TAG, "onMissionCompleted");
			if (Missions.Contains(completedMission)) {
				SoomlaUtils.LogDebug (TAG, string.Format ("Mission {0} is part of challenge {1} ({2}) total", completedMission.ID, _id, Missions.Count));
				bool completed = true;
				foreach (Mission mission in Missions) {
					if (!mission.IsCompleted()) {
						SoomlaUtils.LogDebug (TAG, "challenge mission not completed?=" + mission.ID);
						completed = false;
						break;
					}
				}
				
				if(completed) {
					SoomlaUtils.LogDebug (TAG, string.Format ("Challenge {0} completed!", _id));
					setCompletedInner(true);
				}
			}
		}

		/// <summary>
		/// Handles mission revoked events. If the challenge was completed before, but
		/// now one of its child missions is incomplete, the challenge is revoked as well.
		/// </summary>
		/// <param name="mission">Mission.</param>
		/// @Subscribe
		public void onMissionCompletionRevoked(Mission mission) {
			if (Missions.Contains(mission)) {
				if (MissionStorage.IsCompleted(this)) {
					setCompletedInner(false);
				}
			}
		}

		/// <summary>
		/// Registers relevant events: onMissionCompleted and onMissionCompletionRevoked.
		/// </summary>
		protected override void registerEvents() {
			SoomlaUtils.LogDebug (TAG, "registerEvents called");
			if (!IsCompleted()) {
				SoomlaUtils.LogDebug (TAG, "registering!");
				// register for events
				LevelUpEvents.OnMissionCompleted += onMissionCompleted; 
				LevelUpEvents.OnMissionCompletionRevoked += onMissionCompletionRevoked;
			}
		}

		// this is irrelevant for now
//		protected override void unregisterEvents() {
//			SoomlaUtils.LogDebug(TAG, "ignore unregisterEvents() since challenge can be revoked by child missions revoked");
//		}
	}
}

