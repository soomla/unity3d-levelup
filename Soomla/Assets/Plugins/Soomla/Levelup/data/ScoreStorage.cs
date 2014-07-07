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
	public class ScoreStorage
	{

		protected const string TAG = "SOOMLA ScoreStorage"; // used for Log error messages

		static ScoreStorage _instance = null;
		static ScoreStorage instance {
			get {
				if(_instance == null) {
					#if UNITY_ANDROID && !UNITY_EDITOR
					_instance = new ScoreStorageAndroid();
					#elif UNITY_IOS && !UNITY_EDITOR
					_instance = new ScoreStorageIOS();
					#else
					_instance = new ScoreStorage();
					#endif
				}
				return _instance;
			}
		}
			

		public static void SetLatestScore(Score score, double latest) {
			instance._setLatestScore (score, latest);
		}

		public static double GetLatestScore(Score score) {
			return instance._getLatestScore (score);
		}

		public static void SetRecordScore(Score score, double record) {
			instance._setRecordScore (score, record);
		}

		public static double GetRecordScore(Score score) {
			return instance._getRecordScore (score);
		}



		protected void _setLatestScore(Score score, double latest) {
#if UNITY_EDITOR
			string key = keyLatestScore (score.ScoreId);
			string val = latest.ToString ();
			PlayerPrefs.SetString (key, val);
#endif
		}
		
		protected double _getLatestScore(Score score) {
#if UNITY_EDITOR
			string key = keyLatestScore (score.ScoreId);
			string val = PlayerPrefs.GetString (key);
			return val == null ? score.StartValue : double.Parse (val);
#else
			return score.StartValue;
#endif
		}
		
		protected void _setRecordScore(Score score, double record) {
#if UNITY_EDITOR
			string key = keyRecordScore (score.ScoreId);
			string val = record.ToString ();
			PlayerPrefs.SetString (key, val);

			LevelUpEvents.OnScoreRecordChanged (score);
#endif
		}
		
		protected double _getRecordScore(Score score) {
#if UNITY_EDITOR
			string key = keyRecordScore (score.ScoreId);
			string val = PlayerPrefs.GetString (key);
			return val == null ? score.StartValue : double.Parse (val);
#else
			return score.StartValue;
#endif
		}



		/** keys **/

		private static string keyScores(string scoreId, string postfix) {
			return LevelUp.DB_KEY_PREFIX + "scores." + scoreId + "." + postfix;
		}
		
		private static string keyLatestScore(string scoreId) {
			return keyScores(scoreId, "latest");
		}
		
		private static string keyRecordScore(string scoreId) {
			return keyScores(scoreId, "record");
		}

	}
}

