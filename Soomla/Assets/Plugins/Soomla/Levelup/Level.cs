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
	
	public class Level : World {

		public enum LevelState {
			Idle,
			Running,
			Paused,
			Ended,
			Completed
		}

		private const string TAG = "SOOMLA Level";
		private long StartTime;
		private long Elapsed;
		
		public LevelState State = LevelState.Idle;

		public Level(String worldId)
			: base(worldId) 
		{
		}
		
		public Level(string worldId, GatesList gates, Dictionary<string, Score> scores, List<Challenge> challenges)
			: base(worldId, gates, new Dictionary<string, World>(), scores, challenges)
		{
		}

		public Level(string worldId, GatesList gates, Dictionary<string, World> innerWorlds, Dictionary<string, Score> scores, List<Challenge> challenges)
			: base(worldId, gates, innerWorlds, scores, challenges)
		{
		}

		protected Level(JSONObject jsonObj)
			: base(jsonObj) 
		{
		}

		public new static Level fromJSONObject(JSONObject levelObj) {
			string className = levelObj[JSONConsts.SOOM_CLASSNAME].str;
			
			Level level = (Level) Activator.CreateInstance(Type.GetType("Soomla.Levelup." + className), new object[] { levelObj });
			
			return level;
		}

		public int GetTimesStarted() {
			return LevelStorage.GetTimesStarted(this);
		}
		
		public int GetTimesPlayed() {
			return LevelStorage.GetTimesPlayed(this);
		}
		
		public double GetSlowestDuration() {
			return LevelStorage.GetSlowestDuration(this);
		}
		
		public double GetFastestDuration() {
			return LevelStorage.GetFastestDuration(this);
		}
		
		public void DecScore(string scoreId, double amount) {
			Scores[scoreId].Dec(amount);
		}
		
		public void IncScore(string scoreId, double amount) {
			Scores[scoreId].Inc(amount);
		}


		public bool Start() {
			SoomlaUtils.LogDebug(TAG, "Starting level with worldId: " + WorldId);
			
			if (!CanStart()) {
				return false;
			}

			StartTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
			Elapsed = 0;
			State = LevelState.Running;
			LevelStorage.IncTimesStarted(this);
			
			return true;
		}

		public void Pause() {
			if (State != LevelState.Running) {
				return;
			}
			
			long now = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
			Elapsed += now - StartTime;
			StartTime = 0;
			
			State = LevelState.Paused;
		}

		public void Resume() {
			if (State != LevelState.Paused) {
				return;
			}
			
			StartTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
			State = LevelState.Running;
		}
		
		public double GetPlayDuration() {
			
			long now = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
			long duration = Elapsed;
			if (StartTime != 0) {
				duration += now - StartTime;
			}
			
			return duration / 1000.0;
		}


		public void End(bool completed) {
			
			// check end() called without matching start()
			if(StartTime == 0) {
				SoomlaUtils.LogError(TAG, "end() called without prior start()! ignoring.");
				return;
			}
			
			double duration = GetPlayDuration();
			
			State = LevelState.Ended;
			
			// Calculate the slowest \ fastest durations of level play
			
			if (duration > GetSlowestDuration()) {
				LevelStorage.SetSlowestDuration(this, duration);
			}
			
			if (duration < GetFastestDuration()) {
				LevelStorage.SetFastestDuration(this, duration);
			}
			
			foreach (Score score in Scores.Values) {
				score.SaveAndReset(); // resetting scores
			}

			// Count number of times this level was played
			LevelStorage.IncTimesPlayed(this);
			
			// reset timers
			StartTime = 0;
			Elapsed = 0;
			
			if(completed) {
				SetCompleted(true);
			}
		}

		public override void SetCompleted(bool completed) {
			State = LevelState.Completed;
			base.SetCompleted(completed);
		}
	}
}

