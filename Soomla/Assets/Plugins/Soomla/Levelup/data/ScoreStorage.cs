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
	/// A utility class for persisting and querying scores and records.
	/// Use this class to get or set the values of scores and records.
	/// </summary>
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
			
		/// <summary>
		/// Sets the given value for the given score. 
		/// </summary>
		/// <param name="score">Score whose value is to be set.</param>
		/// <param name="latest">New value.</param>
		public static void SetLatestScore(Score score, double latest) {
			instance._setLatestScore (score, latest);
		}

		/// <summary>
		/// Retrieves the most recently saved value of the given score.
		/// </summary>
		/// <returns>The latest score.</returns>
		/// <param name="score">Score whose most recent value it to be retrieved.</param>
		public static double GetLatestScore(Score score) {
			return instance._getLatestScore (score);
		}

		/// <summary>
		/// Sets the given record for the given score.
		/// </summary>
		/// <param name="score">Score whose record is to change.</param>
		/// <param name="record">The new record.</param>
		public static void SetRecordScore(Score score, double record) {
			instance._setRecordScore (score, record);
		}

		/// <summary>
		/// Retrieves the record of the given score.
		/// </summary>
		/// <returns>The record value of the given score.</returns>
		/// <param name="score">Score whose record is to be retrieved.</param>
		public static double GetRecordScore(Score score) {
			return instance._getRecordScore (score);
		}

		/// <summary>
		/// Sets the given value for the given score.
		/// </summary>
		/// <param name="score">Score.</param>
		/// <param name="latest">Latest.</param>
		protected virtual void _setLatestScore(Score score, double latest) {
#if UNITY_EDITOR
			string key = keyLatestScore (score.ID);
			string val = latest.ToString ();
			PlayerPrefs.SetString (key, val);
#endif
		}

		/// <summary>
		/// Retrieves the most recently saved value of the given score.
		/// </summary>
		/// <returns>The latest score.</returns>
		/// <param name="score">Score whose most recent value it to be retrieved.</param>
		protected virtual double _getLatestScore(Score score) {
#if UNITY_EDITOR
			string key = keyLatestScore (score.ID);
			string val = PlayerPrefs.GetString (key);
			return (string.IsNullOrEmpty(val)) ? score.StartValue : double.Parse (val);
#else
			return score.StartValue;
#endif
		}

		/// <summary>
		/// Sets the given record for the given score.
		/// </summary>
		/// <param name="score">Score whose record is to change.</param>
		/// <param name="record">The new record.</param>
		protected virtual void _setRecordScore(Score score, double record) {
#if UNITY_EDITOR
			string key = keyRecordScore (score.ID);
			string val = record.ToString ();
			PlayerPrefs.SetString (key, val);

			LevelUpEvents.OnScoreRecordChanged (score);
#endif
		}

		/// <summary>
		/// Retrieves the record of the given score.
		/// </summary>
		/// <returns>The record value of the given score.</returns>
		/// <param name="score">Score whose record is to be retrieved.</param>
		protected virtual double _getRecordScore(Score score) {
#if UNITY_EDITOR
			string key = keyRecordScore (score.ID);
			string val = PlayerPrefs.GetString (key);
			return (string.IsNullOrEmpty(val)) ? score.StartValue : double.Parse (val);
#else
			return score.StartValue;
#endif
		}


		/** keys **/

		/// <summary>
		/// Private helper functions if Unity Editor is being used. 
		/// </summary>
#if UNITY_EDITOR
		private static string keyScores(string scoreId, string postfix) {
			return LevelUp.DB_KEY_PREFIX + "scores." + scoreId + "." + postfix;
		}
		
		private static string keyLatestScore(string scoreId) {
			return keyScores(scoreId, "latest");
		}
		
		private static string keyRecordScore(string scoreId) {
			return keyScores(scoreId, "record");
		}
#endif
	}
}

