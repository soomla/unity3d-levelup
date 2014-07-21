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
	
	public class Score : SoomlaEntity {

//#if UNITY_IOS && !UNITY_EDITOR
//		[DllImport ("__Internal")]
//		private static extern int storeAssets_Save(string type, string viJSON);
//#endif

		private const string TAG = "SOOMLA Score";

		public double StartValue = 0;
		public bool HigherBetter;
		protected double _tempScore;

		public Score (string id)
			: this(id, "", true)
		{
		}

		public Score (string id, string name, bool higherBetter)
			: base(id, name, "")
		{
			this.HigherBetter = higherBetter;
		}
		
//#if UNITY_ANDROID && !UNITY_EDITOR
//		protected Mission(AndroidJavaObject jniVirtualItem) {
//			this.Name = jniVirtualItem.Call<string>("getName");
//			this.Description = jniVirtualItem.Call<string>("getDescription");
//			this.ItemId = jniVirtualItem.Call<string>("getItemId");
//		}
//#endif

		public Score(JSONObject jsonObj) 
			: base(jsonObj)
		{
			this.StartValue = jsonObj[LUJSONConsts.LU_SCORE_STARTVAL].n;
			this.HigherBetter = jsonObj[LUJSONConsts.LU_SCORE_HIGHBETTER].b;
		}

		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();
			obj.AddField(LUJSONConsts.LU_SCORE_STARTVAL, Convert.ToInt32(this.StartValue));
			obj.AddField(LUJSONConsts.LU_SCORE_HIGHBETTER, this.HigherBetter);
			
			return obj;
		}

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

		public virtual void Inc(double amount) {
			SetTempScore(_tempScore + amount);
		}

		public virtual void Dec(double amount) {
			SetTempScore(_tempScore - amount);
		}

		public void SaveAndReset() {
			double record = ScoreStorage.GetRecordScore(this);
			if (HasTempReached(record)) {
				ScoreStorage.SetRecordScore(this, _tempScore);
			}
			
			performSaveActions();
			
			ScoreStorage.SetLatestScore(this, _tempScore);
			SetTempScore(StartValue);
		}

		public void Reset() {
			SetTempScore(StartValue);
			// 0 doesn't work well (confusing) for descending score
			// if someone set higherBetter(false) and a start value of 100
			// I think they expect reset to go back to 100, otherwise
			// 0 is the best and current record and can't be beat
			ScoreStorage.GetRecordScore(this);
			ScoreStorage.SetLatestScore(this, /*0*/StartValue);
		}

		public bool HasTempReached(double scoreVal) {
			return HasScoreReached(_tempScore, scoreVal);
		}

		public bool HasRecordReached(double scoreVal) {
			double record = ScoreStorage.GetRecordScore(this); 
			return HasScoreReached(record, scoreVal);
		}

		protected virtual void performSaveActions() {}

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
			_tempScore = score;
		}

		public virtual double GetTempScore() {
			return _tempScore;
		}

		public double Record {
			get {
				return ScoreStorage.GetRecordScore(this);
			}
		}

		public double Latest {
			get {
				return ScoreStorage.GetLatestScore(this);
			}
		}
	}
}

