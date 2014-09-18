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
	/// A utility class for persisting and querying the state of <c>Level</c>s.
	/// Use this class to get or set information about a <c>Level</c>, such as 
	/// the play duration, start or end time, etc.
	/// </summary>
	public class LevelStorage
	{

		protected const string TAG = "SOOMLA LevelStorage"; 

		/// <summary>
		/// Holds an instance of <c>LevelStorage</c> or <c>LevelStorageAndroid</c> or <c>LevelStorageIOS</c>.
		/// </summary>
		static LevelStorage _instance = null;

		/// <summary>
		/// Determines which <c>LevelStorage</c> to use according to the platform in use
		/// and if the Unity Editor is being used. 
		/// </summary>
		/// <value>The instance to use.</value>
		static LevelStorage instance {
			get {
				if(_instance == null) {
					#if UNITY_ANDROID && !UNITY_EDITOR
					_instance = new LevelStorageAndroid();
					#elif UNITY_IOS && !UNITY_EDITOR
					_instance = new LevelStorageIOS();
					#else
					_instance = new LevelStorage();
					#endif
				}
				return _instance;
			}
		}


		/** The following functions call the relevant instance-specific functions. **/
			
		/** LEVEL DURATIONS **/
	
		public static void SetSlowestDurationMillis(Level level, long duration) {
			instance._setSlowestDurationMillis(level, duration);	
		}

		public static long GetSlowestDurationMillis(Level level) {
			return instance._getSlowestDurationMillis(level);
		}

		public static void SetFastestDurationMillis(Level level, long duration) {
			instance._setFastestDurationMillis(level, duration);
		}

		public static long GetFastestDurationMillis(Level level) {
			return instance._getFastestDurationMillis(level);
		}

		/** LEVEL TIMES STARTED **/

		public static int IncTimesStarted(Level level) {
			return instance._incTimesStarted (level);
		}

		public static int DecTimesStarted(Level level) {
			return instance._decTimesStarted (level);
		}

		public static int GetTimesStarted(Level level) {
			return instance._getTimesStarted (level);
		}

		/** LEVEL TIMES PLAYED **/

		public static int IncTimesPlayed(Level level) {
			return instance._incTimesPlayed (level);
		}

		public static int DecTimesPlayed(Level level){
			return instance._decTimesPlayed (level);
		} 

		public static int GetTimesPlayed(Level level) {
			return instance._getTimesPlayed (level);
		}


		/** Unity-Editor Functions **/

		/// <summary>
		/// Sets the slowest (given) duration for the given level.
		/// </summary>
		/// <param name="level"><c>Level</c> to set slowest duration.</param>
		/// <param name="duration">Duration to set.</param>
		protected virtual void _setSlowestDurationMillis(Level level, long duration) {
#if UNITY_EDITOR
			string key = keySlowestDuration (level.ID);
			string val = duration.ToString ();
			PlayerPrefs.SetString (key, val);
#endif
		}

		/// <summary>
		/// Retrieves the slowest duration for the given level.
		/// </summary>
		/// <returns>The slowest duration of the given <c>Level</c>.</returns>
		/// <param name="level"><c>Level</c> to get slowest duration.</param>
		protected virtual long _getSlowestDurationMillis(Level level) {
#if UNITY_EDITOR
			string key = keySlowestDuration (level.ID);
			string val = PlayerPrefs.GetString (key);
			return (string.IsNullOrEmpty(val)) ? 0 : long.Parse (val);
#else
			return 0;
#endif
		}

		/// <summary>
		/// Sets the fastest (given) duration for the given <c>Level</c>.
		/// </summary>
		/// <param name="level"><c>Level</c> to set fastest duration.</param>
		/// <param name="duration">Duration to set.</param>
		protected virtual void _setFastestDurationMillis(Level level, long duration) {
#if UNITY_EDITOR
			string key = keyFastestDuration (level.ID);
			string val = duration.ToString ();
			PlayerPrefs.SetString (key, val);
#endif
		}
		
		/// <summary>
		/// Gets the fastest duration for the given <c>Level</c>.
		/// </summary>
		/// <returns>The fastest duration of the given <c>Level</c>.</returns>
		/// <param name="level"><c>Level</c> to get fastest duration.</param>
		protected virtual long _getFastestDurationMillis(Level level) {
#if UNITY_EDITOR
			string key = keyFastestDuration (level.ID);
			string val = PlayerPrefs.GetString (key);
			return (string.IsNullOrEmpty(val)) ? 0 : long.Parse (val);
#else
			return 0;
#endif
		}
		
		/// <summary>
		/// Increases by 1 the number of times the given <c>Level</c> has been started. 
		/// </summary>
		/// <returns>The number of times started after increasing.</returns>
		/// <param name="level"><c>Level</c> to increase its times started.</param>
		protected virtual int _incTimesStarted(Level level) {
#if UNITY_EDITOR
			int started = _getTimesStarted(level);
			if (started < 0) { /* can't be negative */
				started = 0;
			}
			string startedStr = (started + 1).ToString();
			string key = keyTimesStarted(level.ID);
			PlayerPrefs.SetString (key, startedStr);

			// Notify level has started
			LevelUpEvents.OnLevelStarted (level);

			return started + 1;
#else
			return 0;
#endif
		}
		
		/// <summary>
		/// Decreases by 1 the number of times the given <c>Level</c> has been started. 
		/// </summary>
		/// <returns>The number of times started after decreasing.</returns>
		/// <param name="level"><c>Level</c> to decrease its times started.</param>
		protected virtual int _decTimesStarted(Level level) {
#if UNITY_EDITOR
			int started = _getTimesStarted(level);
			if (started <= 0) { /* can't be negative or zero */
				return 0;
			}
			string startedStr = (started - 1).ToString();
			string key = keyTimesStarted(level.ID);
			PlayerPrefs.SetString (key, startedStr);

			return started - 1;
#else
			return 0;
#endif
		}
		
		/// <summary>
		/// Retrieves the number of times this <c>Level</c> has been started. 
		/// </summary>
		/// <returns>The number of times started.</returns>
		/// <param name="level"><c>Level</c> whose times started is to be retrieved.</param>
		protected virtual int _getTimesStarted(Level level) {
#if UNITY_EDITOR
			string key = keyTimesStarted(level.ID);
			string val = PlayerPrefs.GetString (key);
			
			int started = 0;
			if (!string.IsNullOrEmpty(val)) {
				started = int.Parse(val);
			}
			
			return started;
#else
			return 0;
#endif
		}

		/// <summary>
		/// Increases by 1 the number of times the given <c>Level</c> has been played. 
		/// </summary>
		/// <returns>The number of times played after increasing.</returns>
		/// <param name="level"><c>Level</c> to increase its times played.</param>
		protected virtual int _incTimesPlayed(Level level) {
#if UNITY_EDITOR
			int played = _getTimesPlayed(level);
			if (played < 0) { /* can't be negative */
				played = 0;
			}
			string playedStr = (played + 1).ToString();
			string key = keyTimesPlayed(level.ID);
			PlayerPrefs.SetString (key, playedStr);
			
			// Notify level has ended
			LevelUpEvents.OnLevelEnded (level);
			
			return played + 1;
#else
			return 0;
#endif
		}
		
		/// <summary>
		/// Decreases by 1 the number of times the given <c>Level</c> has been played. 
		/// </summary>
		/// <returns>The number of times played after decreasing.</returns>
		/// <param name="level"><c>Level</c> to decrease its times played.</param>
		protected virtual int _decTimesPlayed(Level level){
#if UNITY_EDITOR
			int played = _getTimesPlayed(level);
			if (played <= 0) { /* can't be negative or zero */
				return 0;
			}
			string playedStr = (played - 1).ToString();
			string key = keyTimesPlayed(level.ID);
			PlayerPrefs.SetString (key, playedStr);
			
			return played - 1;
#else
			return 0;
#endif
		} 
		
		/// <summary>
		/// Retrieves the number of times this <c>Level</c> has been played. 
		/// </summary>
		/// <returns>The number of times played.</returns>
		/// <param name="level"><c>Level</c> whose times played is to be retrieved.</param>
		protected virtual int _getTimesPlayed(Level level) {
#if UNITY_EDITOR
			string key = keyTimesPlayed(level.ID);
			string val = PlayerPrefs.GetString (key);
			
			int played = 0;
			if (!string.IsNullOrEmpty(val)) {
				played = int.Parse(val);
			}
			
			return played;
#else
			return 0;
#endif
		}


		/** Keys (private helper functions if Unity Editor is being used.) **/

#if UNITY_EDITOR
		private static string keyLevels(string levelId, string postfix) {
			return LevelUp.DB_KEY_PREFIX + "levels." + levelId + "." + postfix;
		}
		
		private static string keyTimesStarted(string levelId) {
			return keyLevels(levelId, "started");
		}
		
		private static string keyTimesPlayed(string levelId) {
			return keyLevels(levelId, "played");
		}
		
		private static string keySlowestDuration(string levelId) {
			return keyLevels(levelId, "slowest");
		}
		
		private static string keyFastestDuration(string levelId) {
			return keyLevels(levelId, "fastest");
		}
#endif
	}
}

