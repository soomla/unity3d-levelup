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
	public class LevelStorage
	{

		protected const string TAG = "SOOMLA LevelStorage"; // used for Log error messages

		static LevelStorage _instance = null;
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
			

		public static void SetSlowestDuration(Level level, double duration) {
			instance._setSlowestDuration (level, duration);	
		}
		
		public static double GetSlowestDuration(Level level) {
			return instance._getSlowestDuration (level);
		}
		
		public static void SetFastestDuration(Level level, double duration) {
			instance._setFastestDuration (level, duration);
		}
		
		public static double GetFastestDuration(Level level) {
			return instance._getFastestDuration (level);
		}
		
		
		
		/** Level Times Started **/
		
		public static int IncTimesStarted(Level level) {
			return instance._incTimesStarted (level);
		}
		
		public static int DecTimesStarted(Level level) {
			return instance._decTimesStarted (level);
		}
		
		public static int GetTimesStarted(Level level) {
			return instance._getTimesStarted (level);
		}
		
		
		/** Level Times Played **/
		
		public static int IncTimesPlayed(Level level) {
			return instance._incTimesPlayed (level);
		}
		
		public static int DecTimesPlayed(Level level){
			return instance._decTimesPlayed (level);
		} 
		
		public static int GetTimesPlayed(Level level) {
			return instance._getTimesPlayed (level);
		}



		protected void _setSlowestDuration(Level level, double duration) {
			string key = keySlowestDuration (level.WorldId);
			string val = duration.ToString ();
			PlayerPrefs.SetString (key, val);
		}
		
		protected double _getSlowestDuration(Level level) {
			string key = keySlowestDuration (level.WorldId);
			string val = PlayerPrefs.GetString (key);
			return val == null ? double.MinValue : double.Parse (val);
		}
		
		protected void _setFastestDuration(Level level, double duration) {
			string key = keyFastestDuration (level.WorldId);
			string val = duration.ToString ();
		}
		
		protected double _getFastestDuration(Level level) {
			string key = keyFastestDuration (level.WorldId);
			string val = PlayerPrefs.GetString (key);
			return val == null ? double.MaxValue : double.Parse (val);
		}
		
		
		
		/** Level Times Started **/
		
		protected int _incTimesStarted(Level level) {
			int started = _getTimesStarted(level);
			if (started < 0) { /* can't be negative */
				started = 0;
			}
			string startedStr = (started + 1).ToString();
			string key = keyTimesStarted(level.WorldId);
			PlayerPrefs.SetString (key, startedStr);

			// Notify level has started
			LevelUpEvents.OnLevelStarted (level);

			return started + 1;
		}
		
		protected int _decTimesStarted(Level level) {
			int started = _getTimesStarted(level);
			if (started <= 0) { /* can't be negative or zero */
				return 0;
			}
			string startedStr = (started - 1).ToString();
			string key = keyTimesStarted(level.WorldId);
			PlayerPrefs.SetString (key, startedStr);

			return started - 1;
		}
		
		protected int _getTimesStarted(Level level) {
			string key = keyTimesStarted(level.WorldId);
			string val = PlayerPrefs.GetString (key);
			
			int started = 0;
			if (val != null) {
				started = int.Parse(val);
			}
			
			return started;
		}
		
		
		/** Level Times Played **/
		
		protected int _incTimesPlayed(Level level) {
			int played = _getTimesPlayed(level);
			if (played < 0) { /* can't be negative */
				played = 0;
			}
			string playedStr = (played + 1).ToString();
			string key = keyTimesPlayed(level.WorldId);
			PlayerPrefs.SetString (key, playedStr);
			
			// Notify level has ended
			LevelUpEvents.OnLevelEnded (level);
			
			return played + 1;
		}
		
		protected int _decTimesPlayed(Level level){
			int played = _getTimesPlayed(level);
			if (played <= 0) { /* can't be negative or zero */
				return 0;
			}
			string playedStr = (played - 1).ToString();
			string key = keyTimesPlayed(level.WorldId);
			PlayerPrefs.SetString (key, playedStr);
			
			return played - 1;
		} 
		
		protected int _getTimesPlayed(Level level) {
			string key = keyTimesPlayed(level.WorldId);
			string val = PlayerPrefs.GetString (key);
			
			int played = 0;
			if (val != null) {
				played = int.Parse(val);
			}
			
			return played;
		}


		/** Keys **/

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

	}
}

