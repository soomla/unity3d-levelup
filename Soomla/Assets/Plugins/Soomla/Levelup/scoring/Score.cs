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
using System.Collections.Generic;
using Soomla;

namespace Soomla.Levelup {

	/// <summary>
	/// Represents a score in the game. A simple game usually has one generic numeric score
	/// which grows as the user progresses in the game. A game can also have multiple
	/// <c>Score</c>s for different aspects such as time, speed, points etc.
	/// A score can be ascending in nature such as regular points (higher is better) or can
	/// be descending such as time-to-complete level (lower is better).
	/// </summary>
	public class Score : SoomlaEntity<Score> {

//#if UNITY_IOS && !UNITY_EDITOR
//		[DllImport ("__Internal")]
//		private static extern int storeAssets_Save(string type, string viJSON);
//#endif

		private const string TAG = "SOOMLA Score";

		public double StartValue = 0;

		/// <summary>
		/// In many games a high score is better than a low score, but in some games it's
		/// the opposite - for example if a point represents a monster that attacked your 
		/// character then you want to have as least points as possible (lower is better!)
		/// If this value is set to <c>true</c> then higher is better. 
		/// </summary>
		public bool HigherBetter;

		protected double _tempScore;

		private bool _scoreRecordReachedSent = false;


		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="id">Score ID.</param>
		public Score (string id)
			: this(id, "", true)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="id">Score ID.</param>
		/// <param name="name">Score name.</param>
		/// <param name="higherBetter">If <c>true</c> the higher the score the better.</param>
		public Score (string id, string name, bool higherBetter)
			: base(id, name, "")
		{
			this.HigherBetter = higherBetter;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="jsonObj">Json object.</param>
		public Score(JSONObjectnonCon jsonObj) 
			: base(jsonObj)
		{
			this.StartValue = jsonObj[LUJSONConsts.LU_SCORE_STARTVAL].n;
			this.HigherBetter = jsonObj[LUJSONConsts.LU_SCORE_HIGHBETTER].b;
		}

		/// <summary>
		/// Converts this score into a JSONObject
		/// </summary>
		/// <returns>The JSON object.</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();
			obj.AddField(LUJSONConsts.LU_SCORE_STARTVAL, Convert.ToInt32(this.StartValue));
			obj.AddField(LUJSONConsts.LU_SCORE_HIGHBETTER, this.HigherBetter);
			
			return obj;
		}

		/// <summary>
		/// Converts the given JSONObject into a score. 
		/// </summary>
		/// <returns>The JSON object.</returns>
		/// <param name="scoreObj">Score object.</param>
		public static Score fromJSONObject(JSONObject scoreObj) {
			string className = scoreObj[JSONConsts.SOOM_CLASSNAME].str;
			
			Score score = (Score) Activator.CreateInstance(Type.GetType("Soomla.Levelup." + className), new object[] { scoreObj });
			
			return score;
		}

#if UNITY_ANDROID 
		//&& !UNITY_EDITOR
		public AndroidJavaObject toJNIObject() {
			using(AndroidJavaClass jniScoreClass = new AndroidJavaClass("com.soomla.levelup.scoring.Score")) {
				return jniScoreClass.CallStatic<AndroidJavaObject>("fromJSONString", toJSONObject().print());
			}
		}
#endif

		/// <summary>
		/// Increases this score by the given amount. 
		/// </summary>
		/// <param name="amount">Amount to increase by.</param>
		public virtual void Inc(double amount) {
			SetTempScore(_tempScore + amount);
		}

		/// <summary>
		/// Decreases this score by the given amount. 
		/// </summary>
		/// <param name="amount">Amount to decrease by.</param>
		public virtual void Dec(double amount) {
			SetTempScore(_tempScore - amount);
		}

		/// <summary>
		/// Saves the current score (and record if reached) and resets the score to 
		/// its initial value.  Use this method for example when a user restarts a
		/// level with a fresh score of 0.
		/// </summary>
		/// <param name="save">If set to <c>true</c> save.</param>
		public void Reset(bool save) {
			if (save) {
				double record = ScoreStorage.GetRecordScore(this);
				if (HasTempReached(record)) {
					ScoreStorage.SetRecordScore(this, _tempScore);
					_scoreRecordReachedSent = false;
				}
				
				performSaveActions();
				
				ScoreStorage.SetLatestScore(this, _tempScore);
			}

			SetTempScore(StartValue);
		}
	
		/// <summary>
		/// Checks if the score in the current game session has reached the given value.
		/// </summary>
		/// <returns><c>true</c> if this score has reached the given scoreVal; otherwise, 
		/// <c>false</c>.</returns>
		/// <param name="scoreVal">Score value.</param>
		public bool HasTempReached(double scoreVal) {
			return HasScoreReached(_tempScore, scoreVal);
		}

		/// <summary>
		/// Determines if this score has reached a record value of the given scoreVal.
		/// </summary>
		/// <returns><c>true</c> if this score has reached the given record; otherwise, 
		/// <c>false</c>.</returns>
		/// <param name="scoreVal">Score value.</param>
		public bool HasRecordReached(double scoreVal) {
			double record = ScoreStorage.GetRecordScore(this); 
			return HasScoreReached(record, scoreVal);
		}

		/// <summary>
		/// Score can sometimes have additional actions associated with reaching/saving it.
		/// Override this method to add specific score behavior.
		/// </summary>
		protected virtual void performSaveActions() {}

		/// <summary>
		/// Determines which of the two given scores is better (based on this score's definition
		/// of better - higher or lower).
		/// </summary>
		/// <returns>If score1 is higher than score2 returns <c>true</c>; otherwise <c>false</c>.
		/// <c>false</c>.</returns>
		/// <param name="score1">Score1 to compare.</param>
		/// <param name="score2">Score2 to compare.</param>
		private bool HasScoreReached(double score1, double score2) {
			return this.HigherBetter ?
				(score1 >= score2) :
					(score1 <= score2);
		}


		public virtual void SetTempScore(double score) {
			SetTempScore(score, false);
		}
		public virtual void SetTempScore(double score, bool onlyIfBetter) {
			if (onlyIfBetter && !HasScoreReached(score, _tempScore)) {
				return;
			}
			if (!_scoreRecordReachedSent && HasScoreReached(score, _tempScore)) {
				LevelUpEvents.OnScoreRecordReached(this);
				_scoreRecordReachedSent = true;
			}
			_tempScore = score;
		}

		public virtual double GetTempScore() {
			return _tempScore;
		}

		/// <summary>
		/// Retrieves the record of this score.
		/// </summary>
		/// <returns>The record.</returns>
		public double Record {
			get {
				return ScoreStorage.GetRecordScore(this);
			}
		}

		/// <summary>
		/// Retrieves the most recently saved value of this score.
		/// </summary>
		/// <returns>The latest score.</returns>
		public double Latest {
			get {
				return ScoreStorage.GetLatestScore(this);
			}
		}

		/// /// <summary>
		/// Clones this score and gives it the given ID.
		/// </summary>
		/// <param name="newScoreId">Cloned score ID.</param>
		public override Score Clone(string newScoreId) {
			return (Score) base.Clone(newScoreId);
		}
	}
}

