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
	public class MissionStorage
	{

		protected const string TAG = "SOOMLA MissionStorage"; // used for Log error messages

		static MissionStorage _instance = null;
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
			

		public static void SetCompleted(Mission mission, bool completed) {
			instance._setCompleted (mission, completed, true);
		}

		public static void SetCompleted(Mission mission, bool completed, bool notify) {
			instance._setCompleted(mission, completed, notify);
		}

		public static bool IsCompleted(Mission mission) {
			return instance._isCompleted(mission);
		}


		protected void _setCompleted(Mission mission, bool completed, bool notify) {
#if UNITY_EDITOR
			string key = keyMissionCompleted (mission.MissionId);
			if (completed) {
				PlayerPrefs.SetString(key, "yes");

				if (notify) {
					LevelUpEvents.OnMissionCompleted(mission);
				}
			} else {
				PlayerPrefs.DeleteKey(key);

				if (notify) {
					LevelUpEvents.OnMissionCompletionRevoked(mission);
				}
			}
#endif
		}

		protected bool _isCompleted(Mission mission) {
#if UNITY_EDITOR
			string key = keyMissionCompleted (mission.MissionId);
			string val = PlayerPrefs.GetString (key);
			return !string.IsNullOrEmpty(val);
#else
			return false;
#endif
		}



		/** keys **/

		private static string keyMissions(string missionId, string postfix) {
			return LevelUp.DB_KEY_PREFIX + "missions." + missionId + "." + postfix;
		}
		
		private static string keyMissionCompleted(string missionId) {
			return keyMissions(missionId, "completed");
		}


	}
}

