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

namespace Soomla.Levelup
{
	/// <summary>
	/// A utility class for persisting and querying the state of <c>Mission</c>s.
	/// Use this class to check if a certain <c>Mission</c> is complete, or to
	/// set its completion status.
	/// </summary>
	public class MissionStorage
	{

		protected const string TAG = "SOOMLA MissionStorage";

        public static readonly string DB_MISSION_KEY_PREFIX = "soomla.levelup.missions.";

		/// <summary>
		/// Holds an instance of <c>MissionStorage</c> or <c>MissionStorageAndroid</c> or <c>MissionStorageIOS</c>.
		/// </summary>
		static MissionStorage _instance = null;

		/// <summary>
		/// Determines which <c>MissionStorage</c> to use according to the platform in use
		/// and if the Unity Editor is being used. 
		/// </summary>
		/// <value>The instance to use.</value>
		static MissionStorage instance {
			get {
				if(_instance == null) {
					#if UNITY_ANDROID && !UNITY_EDITOR
					_instance = new MissionStorageAndroid();
					#elif UNITY_IOS && !UNITY_EDITOR
					_instance = new MissionStorageIOS();
					#else
					_instance = new MissionStorage();
					#endif
				}
				return _instance;
			}
		}
			

		/** The following functions call the relevant instance-specific functions. **/

		public static void SetCompleted(Mission mission, bool completed) {
			SetCompleted (mission, completed, true);
		}
		public static void SetCompleted(Mission mission, bool completed, bool notify) {
			instance._setCompleted(mission, completed, notify);
		}

		public static int GetTimesCompleted(Mission mission) {
			return instance._getTimesCompleted(mission);
		}

		/// <summary>
		/// Determines if the given <c>Mission</c> is complete.
		/// </summary>
		/// <returns>If the given <c>Mission</c> is completed returns <c>true</c>;
		/// otherwise <c>false</c>.</returns>
		/// <param name="mission"><c>Mission</c> to determine if complete.</param>
		public static bool IsCompleted(Mission mission) {
			return GetTimesCompleted(mission) > 0;
		}


		/** Unity-Editor Functions **/
	
		/// <summary>
		/// Increases the number of times the given <c>Mission</c> has been
		/// completed if the given <c>up</c> is <c>true</c>; otherwise decreases 
		/// the number of times completed. 
		/// </summary>
		/// <param name="mission"><c>Mission</c> to set as completed.</param>
		/// <param name="up">If set to <c>true</c> add 1 to the number of times
		/// completed, otherwise subtract 1.</param>
		/// <param name="notify">If set to <c>true</c> trigger the relevant
		/// event according to the value of <c>up</c>.</param>
		

		protected virtual void _setCompleted(Mission mission, bool up, bool notify) {
#if UNITY_EDITOR
			int total = _getTimesCompleted(mission) + (up ? 1 : -1);
			if(total<0) total = 0;

			string key = keyMissionTimesCompleted(mission.ID);
			PlayerPrefs.SetString(key, total.ToString());
			
			if (notify) {
				if (up) {
					LevelUpEvents.OnMissionCompleted(mission);
				} else {
					LevelUpEvents.OnMissionCompletionRevoked(mission);
				}
			}
#endif
		}

		/// <summary>
		/// Retrieves the number of times the given <c>Mission</c> has been completed.
		/// </summary>
		/// <returns>The number of times the given mission has been completed.</returns>
		/// <param name="mission">Mission.</param> 
		protected virtual int _getTimesCompleted(Mission mission) {
#if UNITY_EDITOR
			string key = keyMissionTimesCompleted(mission.ID);
			string val = PlayerPrefs.GetString (key);
			if (string.IsNullOrEmpty(val)) {
				return 0;
			}
			return int.Parse(val);
#else
			return 0;
#endif
		}


		/** Keys (private helper functions if Unity Editor is being used.) **/

#if UNITY_EDITOR
		private static string keyMissions(string missionId, string postfix) {
			return SoomlaLevelUp.DB_KEY_PREFIX + "missions." + missionId + "." + postfix;
		}
		
		private static string keyMissionTimesCompleted(string missionId) {
			return keyMissions(missionId, "timesCompleted");
		}

#endif

        public static string getKeyMissionPrefix() {
            return DB_MISSION_KEY_PREFIX;
        }
	}
}

