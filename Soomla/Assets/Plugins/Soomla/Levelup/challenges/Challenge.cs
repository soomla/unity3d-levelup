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
	public class Challenge : Mission
	{
		private const string TAG = "SOOMLA Challenge";

		public List<Mission> Missions;

		public Challenge(string missionId, string name, List<Mission> missions)
			: base(missionId, name)
		{
			Missions = missions;
		}

		public Challenge(string missionId, string name, List<Mission> missions, List<Reward> rewards)
			: base(missionId, name, rewards)
		{
			Missions = missions;
		}
		
		/// <summary>
		/// see parent.
		/// </summary>
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
		/// Constructor.
		/// </summary>
		/// <returns>see parent</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();

			JSONObject missionsJSON = new JSONObject(JSONObject.Type.ARRAY);
			foreach (Mission mission in Missions) {			
				missionsJSON.Add(mission.toJSONObject());
			}
			obj.AddField(LUJSONConsts.LU_MISSIONS, missionsJSON);		

			return obj;
		}

		public override bool IsCompleted() {
			// could happen in construction
			// need to return false in order to register for child events
			if(Missions == null) {
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
				SoomlaUtils.LogDebug (TAG, string.Format ("Mission {0} is part of challenge {1} ({2}) total", completedMission.MissionId, MissionId, Missions.Count));
				bool completed = true;
				foreach (Mission mission in Missions) {
					if (!mission.IsCompleted()) {
						SoomlaUtils.LogDebug (TAG, "challenge mission not completed?=" + mission.MissionId);
						completed = false;
						break;
					}
				}
				
				if(completed) {
					SoomlaUtils.LogDebug (TAG, string.Format ("Challenge {0} completed!", MissionId));
					SetCompleted(true);
				}
			}
		}

		/// <summary>
		/// handle child mission revoked.
		/// </summary>
		/// <param name="mission">Mission.</param>
		/// @Subscribe
		public void onMissionCompletionRevoked(Mission mission) {
			if (Missions.Contains(mission)) {
				// if the challenge was completed before, but now one of its child missions
				// was uncompleted - the challenge is revoked as well
				if (MissionStorage.IsCompleted(this)) {
					SetCompleted(false);
				}
			}
		}

		protected override void registerEvents() {
			SoomlaUtils.LogDebug (TAG, "registerEvents called");
			if (!IsCompleted()) {
				SoomlaUtils.LogDebug (TAG, "registering!");
				// register for events
				LevelUpEvents.OnMissionCompleted += onMissionCompleted; 
				LevelUpEvents.OnMissionCompletionRevoked += onMissionCompletionRevoked;
			}
		}

		protected override void unregisterEvents() {
			SoomlaUtils.LogDebug(TAG, "ignore unregisterEvents() since challenge can be revoked by child missions revoked");
		}
	}
}

