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
	
	public abstract class Score {

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

		protected Score (string scoreId, string name)
		{
			this.ScoreId = scoreId;
			this.Name = name;
			this.StartValue = 0;
			this.HigherBetter = true;
		}

		protected Score (string scoreId, string name, bool higherBetter)
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

		protected Score(JSONObject jsonObj) {
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


		public virtual void Inc(double amount) {
			SetTempScore(_tempScore + amount);
		}

		public virtual void Dec(double amount) {
			SetTempScore(_tempScore - amount);
		}

		public void SaveAndReset() {
			double record = ScoreStorage.GetRecordScore(this); // TODO: get the record score from storage
			if (HasTempReached(record)) {
				ScoreStorage.GetRecordScore(this, _tempScore); // TODO: set the record score in storage
			}
			
			performSaveActions();
			
			ScoreStorage.SetLatestScore(this, _tempScore);  // TODO: set the latest score in storage
			SetTempScore(StartValue);
		}

		public void Reset() {
			SetTempScore(StartValue);
			// 0 doesn't work well (confusing) for descending score
			// if someone set higherBetter(false) and a start value of 100
			// I think they expect reset to go back to 100, otherwise
			// 0 is the best and current record and can't be beat
			ScoreStorage.GetRecordScore(this, /*0*/StartValue);
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

