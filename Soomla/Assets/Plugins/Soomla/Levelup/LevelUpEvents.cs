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

namespace Soomla.Levelup {

	/// <summary>
	/// This class provides functions for event handling.
	/// </summary>
	public class LevelupEvents : MonoBehaviour {

		private const string TAG = "SOOMLA LevelupEvents";

		private static LevelupEvents instance = null;

		/// <summary>
		/// Initializes game state before the game starts.
		/// </summary>
		void Awake(){
			if(instance == null){ 	// making sure we only initialize one instance.
				instance = this;
				GameObject.DontDestroyOnLoad(this.gameObject);
			} else {				// Destroying unused instances.
				GameObject.Destroy(this.gameObject);
			}
		}

		public void onGateOpened(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onGateOpened with message: " + message);

			// TODO: message is Gate as JSON
			JSONObject json = new JSONObject (message);
			//Gate gate = new Gate(json);

			LevelupEvents.OnGateOpened(/*gate*/);
		}

		public void onLevelEnded(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onLevelEnded with message: " + message);
			
			// TODO: message is Level as Level

			JSONObject json = new JSONObject (message);
			//Level level = new Level(json);
			
			LevelupEvents.OnLevelEnded(/*level*/);
		}

		public void onLevelStarted(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onLevelStarted with message: " + message);
			
			// TODO: message is Level as Level

			JSONObject json = new JSONObject (message);
			//Level level = new Level(json);
			
			LevelupEvents.OnLevelEnded(/*level*/);
		}

		public void onMissionCompleted(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onMissionCompleted with message: " + message);
			
			// TODO: message is Mission AND isChallenge as JSON

			JSONObject json = new JSONObject (message);
			//Mission mission = new Mission(json);
			LevelupEvents.OnMissionCompleted(/*mission*/);
		}

		public void onMissionCompletionRevoked(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onMissionCompletionRevoked with message: " + message);
			
			// TODO: message is Mission AND isChallenge as JSON

			JSONObject json = new JSONObject (message);
			//Mission mission = new Mission(json);
			LevelupEvents.OnMissionCompletionRevoked(/*mission*/);
		}

		public void onScoreRecordChanged(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onScoreRecordChanged with message: " + message);
			
			// TODO: message is Score as JSON

			JSONObject json = new JSONObject (message);
			// Score score = new Score(json);
			LevelupEvents.OnScoreRecordChanged(/*score*/);
		}

		public void onWorldCompleted(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onWorldCompleted with message: " + message);
			
			// TODO: message is World as JSON

			JSONObject json = new JSONObject (message);
			// World world = new World(json);

			LevelupEvents.OnWorldCompleted(/*world*/);
		}



		public delegate void Action();

		public static Action<Gate> OnGateOpened = delegate {}; /* TODO: Gate here */
		public static Action<Level> OnLevelEnded = delegate {}; /* TODO: Level here */
		public static Action<Level> OnLevelStarted = delegate {}; /* TODO: Level here */
		public static Action<Mission, bool> OnMissionCompleted = delegate {}; /* TODO: Mission and bool here */
		public static Action<Mission, bool> OnMissionCompletionRevoked = delegate {}; /* TODO: Mission and bool here */
		public static Action<Score> OnScoreRecordChanged = delegate {}; /* TODO: Score here */
		public static Action<World> OnWorldCompleted = delegate {}; /* TODO: World here */

	}
}
