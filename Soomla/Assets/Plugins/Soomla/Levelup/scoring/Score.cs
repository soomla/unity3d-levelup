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

namespace Soomla.Levelup {
	
	public class Score {

//#if UNITY_IOS && !UNITY_EDITOR
//		[DllImport ("__Internal")]
//		private static extern int storeAssets_Save(string type, string viJSON);
//#endif

		private const string TAG = "SOOMLA Score";
		
		public string Name;
		public string ScoreId;
		public double StartValue;
		public bool HigherBetter;
		protected double _tempScore;

		public Score (string scoreId)
		{
			this.ScoreId = scoreId;
			this.Name = "temp_score_name";
			this.StartValue = 0;
			this.HigherBetter = true;
		}

		public Score (string scoreId, string name, bool higherBetter)
		{
			this.ScoreId = scoreId;
			this.Name = name;
			this.StartValue = 0;
			this.HigherBetter = higherBetter;
		}
		
//#if UNITY_ANDROID && !UNITY_EDITOR
//		protected Mission(AndroidJavaObject jniVirtualItem) {
//			this.Name = jniVirtualItem.Call<string>("getName");
//			this.Description = jniVirtualItem.Call<string>("getDescription");
//			this.ItemId = jniVirtualItem.Call<string>("getItemId");
//		}
//#endif

		public Score(JSONObject jsonObj) {
			this.ScoreId = jsonObj[LUJSONConsts.LU_SCORE_SCOREID].str;
			if (jsonObj[JSONConsts.SOOM_NAME]) {
				this.Name = jsonObj[JSONConsts.SOOM_NAME].str;
			} else {
				this.Name = "";
			}

			this.StartValue = jsonObj[LUJSONConsts.LU_SCORE_STARTVAL].n;
			this.HigherBetter = jsonObj[LUJSONConsts.LU_SCORE_HIGHBETTER].b;
		}

		public virtual JSONObject toJSONObject() {
			JSONObject obj = new JSONObject(JSONObject.Type.OBJECT);
			obj.AddField(JSONConsts.SOOM_NAME, this.Name);
			obj.AddField(LUJSONConsts.LU_SCORE_SCOREID, this.ScoreId);
			obj.AddField(LUJSONConsts.LU_SCORE_STARTVAL, Convert.ToInt32(this.StartValue));
			obj.AddField(LUJSONConsts.LU_SCORE_HIGHBETTER, this.HigherBetter);
			obj.AddField(JSONConsts.SOOM_CLASSNAME, GetType().Name);
			
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



		// Equality
		
		public override bool Equals(System.Object obj)
		{
			// If parameter is null return false.
			if (obj == null)
			{
				return false;
			}
			
			// If parameter cannot be cast to Point return false.
			Score g = obj as Score;
			if ((System.Object)g == null)
			{
				return false;
			}
			
			// Return true if the fields match:
			return (ScoreId == g.ScoreId);
		}
		
		public bool Equals(Score g)
		{
			// If parameter is null return false:
			if ((object)g == null)
			{
				return false;
			}
			
			// Return true if the fields match:
			return (ScoreId == g.ScoreId);
		}
		
		public override int GetHashCode()
		{
			return ScoreId.GetHashCode();
		}
		
		public static bool operator ==(Score a, Score b)
		{
			// If both are null, or both are same instance, return true.
			if (System.Object.ReferenceEquals(a, b))
			{
				return true;
			}
			
			// If one is null, but not both, return false.
			if (((object)a == null) || ((object)b == null))
			{
				return false;
			}
			
			// Return true if the fields match:
			return a.ScoreId == b.ScoreId;
		}
		
		public static bool operator !=(Score a, Score b)
		{
			return !(a == b);
		}

	}
}

