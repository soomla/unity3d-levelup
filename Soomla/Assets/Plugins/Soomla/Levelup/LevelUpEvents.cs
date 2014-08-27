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

			Gate gate = LevelUp.GetInstance().GetGate(message);

			LevelUpEvents.OnGateOpened(gate);
		}

		public void onLevelEnded(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onLevelEnded with message: " + message);
			
			Level level = (Level) LevelUp.GetInstance().GetWorld(message);

			LevelUpEvents.OnLevelEnded(level);
		}

		public void onLevelStarted(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onLevelStarted with message: " + message);

			Level level = (Level) LevelUp.GetInstance().GetWorld(message);
			
			LevelUpEvents.OnLevelStarted(level);
		}

		public void onMissionCompleted(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onMissionCompleted with message: " + message);

			Mission mission = LevelUp.GetInstance().GetMission(message);

			LevelUpEvents.OnMissionCompleted(mission);
		}

		public void onMissionCompletionRevoked(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onMissionCompletionRevoked with message: " + message);
			
			Mission mission = LevelUp.GetInstance().GetMission(message);

			LevelUpEvents.OnMissionCompletionRevoked(mission);
		}

		public void onScoreRecordChanged(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onScoreRecordChanged with message: " + message);
			
			Score score = LevelUp.GetInstance().GetScore(message);

			LevelUpEvents.OnScoreRecordChanged(score);
		}

		public void onWorldCompleted(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onWorldCompleted with message: " + message);
			
			World world = LevelUp.GetInstance().GetWorld(message);

			LevelUpEvents.OnWorldCompleted(world);
		}

		public void onWorldAssignedReward(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onWorldAssignedReward with message: " + message);
			
			World world = LevelUp.GetInstance().GetWorld(message);
			
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

		public static Action<Score> OnScoreRecordReached = delegate {}; 

	}
}
