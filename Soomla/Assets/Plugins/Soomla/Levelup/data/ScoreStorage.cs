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
	/// A utility class for persisting and querying <c>Score</c>s and records.
	/// Use this class to get or set the values of <c>Score</c>s and records.
	/// </summary>
	public class ScoreStorage
	{

		protected const string TAG = "SOOMLA ScoreStorage";

		/// <summary>
		/// Holds an instance of <c>ScoreStorage</c> or <c>ScoreStorageAndroid</c> or <c>ScoreStorageIOS</c>.
		/// </summary>
		static ScoreStorage _instance = null;

		/// <summary>
		/// Determines which <c>ScoreStorage</c> to use according to the platform in use
		/// and if the Unity Editor is being used. 
		/// </summary>
		/// <value>The instance to use.</value>
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


		/** The following functions call the relevant instance-specific functions. **/
			
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


		/** Unity-Editor Functions **/

		/// <summary>
		/// Sets the given <c>Score</c> to the given value.
		/// </summary>
		/// <param name="score"><c>Score</c> to set.</param>
		/// <param name="latest">The value to set for the <c>Score</c>.</param>
		protected virtual void _setLatestScore(Score score, double latest) {
#if UNITY_EDITOR
			string key = keyLatestScore (score.ID);
			string val = latest.ToString ();
			PlayerPrefs.SetString (key, val);
#endif
		}

		/// <summary>
		/// Retrieves the most recently saved value of the given <c>Score</c>.
		/// </summary>
		/// <returns>The latest <c>Score</c>.</returns>
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
		/// Sets the given record for the given <c>Score</c>.
		/// </summary>
		/// <param name="score"><c>Score</c> whose record is to change.</param>
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
		/// Retrieves the record of the given <c>Score</c>.
		/// </summary>
		/// <returns>The record value of the given <c>Score</c>.</returns>
		/// <param name="score"><c>Score</c> whose record is to be retrieved.</param>
		protected virtual double _getRecordScore(Score score) {
#if UNITY_EDITOR
			string key = keyRecordScore (score.ID);
			string val = PlayerPrefs.GetString (key);
			return (string.IsNullOrEmpty(val)) ? score.StartValue : double.Parse (val);
#else
			return score.StartValue;
#endif
		}


		/** Keys (private helper functions if Unity Editor is being used.) **/

		/// <summary>
		/// Private helper functions if Unity Editor is being used. 
		/// </summary>
#if UNITY_EDITOR
		private static string keyScores(string scoreId, string postfix) {
			return SoomlaLevelUp.DB_KEY_PREFIX + "scores." + scoreId + "." + postfix;
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

