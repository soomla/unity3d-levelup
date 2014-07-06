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
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Soomla.Levelup {

	/// <summary>
	/// This class provides functions for event handling.
	/// </summary>
	public class LevelUpEvents : MonoBehaviour {
		private const string TAG = "SOOMLA LevelUpEvents";

#if UNITY_IOS && !UNITY_EDITOR
		[DllImport ("__Internal")]
		private static extern void soomlaLevelup_Init();
#endif
		private static LevelUpEvents instance = null;

		/// <summary>
		/// Initializes game state before the game starts.
		/// </summary>
		void Awake(){
			if(instance == null){ 	// making sure we only initialize one instance.
				instance = this;
				GameObject.DontDestroyOnLoad(this.gameObject);
				Initialize();
			} else {				// Destroying unused instances.
				GameObject.Destroy(this.gameObject);
			}
		}

		public static void Initialize() {
			SoomlaUtils.LogDebug (TAG, "Initialize");
#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniEventHandler = new AndroidJavaClass("com.soomla.unity.LevelUpEventHandler")) {
				jniEventHandler.CallStatic("initialize");
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
#elif UNITY_IOS && !UNITY_EDITOR
			soomlaLevelup_Init();
#endif
		}

		public void onGateOpened(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onGateOpened with message: " + message);

			// message is Gate as JSON
			JSONObject json = new JSONObject (message);
			Gate gate = Gate.fromJSONObject (json);

			LevelUpEvents.OnGateOpened(gate);
		}

		public void onLevelEnded(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onLevelEnded with message: " + message);
			
			// message is Level as Level

			JSONObject json = new JSONObject (message);
			Level level = Level.fromJSONObject(json);

			LevelUpEvents.OnLevelEnded(level);
		}

		public void onLevelStarted(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onLevelStarted with message: " + message);
			
			// message is Level as Level

			JSONObject json = new JSONObject (message);
			Level level = Level.fromJSONObject(json);
			
			LevelUpEvents.OnLevelStarted(level);
		}

		public void onMissionCompleted(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onMissionCompleted with message: " + message);
			
			// message is Mission AND isChallenge as JSON

			JSONObject json = new JSONObject (message);
			Mission mission = Mission.fromJSONObject (json);

			LevelUpEvents.OnMissionCompleted(mission);
		}

		public void onMissionCompletionRevoked(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onMissionCompletionRevoked with message: " + message);
			
			// message is Mission AND isChallenge as JSON

			JSONObject json = new JSONObject (message);
			Mission mission = Mission.fromJSONObject (json);

			LevelUpEvents.OnMissionCompletionRevoked(mission);
		}

		public void onScoreRecordChanged(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onScoreRecordChanged with message: " + message);
			
			// message is Score as JSON

			JSONObject json = new JSONObject (message);
			Score score = Score.fromJSONObject (json);

			LevelUpEvents.OnScoreRecordChanged(score);
		}

		public void onWorldCompleted(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onWorldCompleted with message: " + message);
			
			// message is World as JSON

			JSONObject json = new JSONObject (message);
			World world = World.fromJSONObject (json);

			LevelUpEvents.OnWorldCompleted(world);
		}

		public void onWorldAssignedReward(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onWorldAssignedReward with message: " + message);
			
			// message is World as JSON
			
			JSONObject json = new JSONObject (message);
			World world = World.fromJSONObject (json);
			
			LevelUpEvents.OnWorldAssignedReward(world);
		}



		public delegate void Action();

		public static Action<Gate> OnGateOpened = delegate {};

		public static Action<Level> OnLevelEnded = delegate {};

		public static Action<Level> OnLevelStarted = delegate {};

		public static Action<Mission> OnMissionCompleted = delegate {};

		public static Action<Mission> OnMissionCompletionRevoked = delegate {};

		public static Action<Score> OnScoreRecordChanged = delegate {};

		public static Action<World> OnWorldCompleted = delegate {};

		public static Action<World> OnWorldAssignedReward = delegate {};

	}
}
