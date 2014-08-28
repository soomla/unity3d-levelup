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
	/// of <c>Mission</c>s. The user is required to complete all these <c>Mission</c>s in order  
	/// to earn the <c>Reward</c> associated with the <c>Challenge</c>.
	/// </summary>
	public class Challenge : Mission
	{

		/// <summary>
		/// Used in log error messages.
		/// </summary>
		private const string TAG = "SOOMLA Challenge";

		/// <summary>
		/// The missions that belong to this <c>Challenge</c>.
		/// </summary>
		public List<Mission> Missions = new List<Mission>();


		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="id"><c>Challenge</c> ID.</param>
		/// <param name="name"><c>Challenge</c> name.</param>
		/// <param name="missions"><c>Mission</c>s that belong to this <c>Challenge</c>.</param>
		public Challenge(string id, string name, List<Mission> missions)
			: base(id, name)
		{
			Missions = missions;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="id"><c>Challenge</c> ID.</param>
		/// <param name="name"><c>Challenge</c> name.</param>
		/// <param name="missions"><c>Mission</c>s that belong to this <c>Challenge</c>.</param>
		/// <param name="rewards"><c>Reward</c>s associated with this <c>Challenge</c>.</param>
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
		/// Converts this <c>Challenge</c> to a JSONObject.
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
		/// Checks if this <c>Mission</c> has ever been completed - no matter how many times.
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
		/// Handles mission-completion events. Checks if all <c>Mission</c>s included in this 
		/// <c>Challenge</c> are completed, and if so, sets the <c>Challenge</c> as completed.
		/// </summary>
		/// <param name="completedMission">The <c>Mission</c> that triggered the event.</param>
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
		/// Handles mission-revoked events. If the <c>Challenge</c> was completed before, but now 
		/// one of its child <c>Mission</c>s is incomplete, the <c>Challenge</c> is revoked as well.
		/// </summary>
		/// <param name="mission">The <c>Mission</c> that triggered the event.</param>
		public void onMissionCompletionRevoked(Mission mission) {
			if (Missions.Contains(mission)) {
				if (MissionStorage.IsCompleted(this)) {
					setCompletedInner(false);
				}
			}
		}

		/// <summary>
		/// Registers relevant events: <c>OnMissionCompleted</c> and <c>OnMissionCompletionRevoked</c>.
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

