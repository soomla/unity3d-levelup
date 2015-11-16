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

	/// <summary>
	/// A <c>Level</c> is a type of <c>World</c>, while a <c>World</c> contains a set of <c>Level</c>s. 
	/// Each <c>Level</c> always has a state that is one of: idle, running, paused, ended, or completed. 
	/// 
	/// Real Game Examples: "Candy Crush" and "Angry Birds" use <c>Level</c>s.
	/// </summary>
	public class Level : World {

		// The state of this Level. Every level must have one of the below states. 
		public enum LevelState {
			Idle,
			Running,
			Paused,
			Ended,
			Completed
		}

		private const string TAG = "SOOMLA Level";

		/// <summary>
		/// The start time of this <c>Level</c>.
		/// </summary>
		private long StartTime;
		       
		/// <summary>
		/// The elapsed time this <c>Level</c> is being played. 
		/// </summary>
		private long Elapsed;

		/// <summary>
		/// The state of this level. The initial state is idle, later in the game can be any of:
		/// running, paused, ended, or completed.
		/// </summary>
		public LevelState State = LevelState.Idle;

		/// <summary>
		/// Determines that should time scaling will be taken into account.
		/// </summary>
		public readonly bool UseTimeScaling;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="useTimeScaling">Take Time.timeScale value into account while duration will be calculated.</param>
		public Level(String id, bool useTimeScaling)
			: base(id) 
		{
			this.UseTimeScaling = useTimeScaling;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="id">ID.</param>
		public Level(String id)
			: this(id, false)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="gate">Gate to open this <c>Level</c>.</param>
		/// <param name="scores">Scores of this <c>Level</c>.</param>
		/// <param name="missions">Missions of this <c>Level</c>.</param>
		/// <param name="useTimeScaling">Take Time.timeScale value into account while duration will be calculated.</param>
		public Level(string id, Gate gate, Dictionary<string, Score> scores, List<Mission> missions, bool useTimeScaling)
			: base(id, gate, new Dictionary<string, World>(), scores, missions)
		{
			this.UseTimeScaling = useTimeScaling;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="gate">Gate to open this <c>Level</c>.</param>
		/// <param name="scores">Scores of this <c>Level</c>.</param>
		/// <param name="missions">Missions of this <c>Level</c>.</param>
		public Level(string id, Gate gate, Dictionary<string, Score> scores, List<Mission> missions)
			: this(id, gate, new Dictionary<string, World>(), scores, missions, false)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="gate">Gate to open this <c>Level</c>.</param>
		/// <param name="innerWorlds">Inner <c>Level</c>s of this <c>Level</c>.</param>
		/// <param name="scores">Scores of this <c>Level</c>.</param>
		/// <param name="missions">Missions of this <c>Level</c>.</param>
		/// <param name="useTimeScaling">Take Time.timeScale value into account while duration will be calculated.</param>
		public Level(string id, Gate gate, Dictionary<string, World> innerWorlds, Dictionary<string, Score> scores, List<Mission> missions, bool useTimeScaling)
			: base(id, gate, innerWorlds, scores, missions)
		{
			this.UseTimeScaling = useTimeScaling;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="gate">Gate to open this <c>Level</c>.</param>
		/// <param name="innerWorlds">Inner <c>Level</c>s of this <c>Level</c>.</param>
		/// <param name="scores">Scores of this <c>Level</c>.</param>
		/// <param name="missions">Missions of this <c>Level</c>.</param>
		public Level(string id, Gate gate, Dictionary<string, World> innerWorlds, Dictionary<string, Score> scores, List<Mission> missions)
			: this(id, gate, innerWorlds, scores, missions, false)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="jsonObj">Json object.</param>
		public Level(JSONObject jsonObj)
			: base(jsonObj) 
		{
		}

		/// <summary>
		/// Converts the given JSON object into a <c>Level</c>.
		/// </summary>
		/// <returns>Level constructed.</returns>
		/// <param name="levelObj">The JSON object to be converted.</param>
		public new static Level fromJSONObject(JSONObject levelObj) {
			string className = levelObj[JSONConsts.SOOM_CLASSNAME].str;

			Level level = (Level) Activator.CreateInstance(Type.GetType("Soomla.Levelup." + className), new object[] { levelObj });
			
			return level;
		}

		/// <summary>
		/// Gets the number of times this <c>Level</c> was started.
		/// </summary>
		/// <returns>The number of times started.</returns>
		public int GetTimesStarted() {
			return LevelStorage.GetTimesStarted(this);
		}

		/// <summary>
		/// Gets the number of times this <c>Level</c> was played. 
		/// </summary>
		/// <returns>The number of times played.</returns>
		public int GetTimesPlayed() {
			return LevelStorage.GetTimesPlayed(this);
		}

		/// <summary>
		/// Gets the slowest duration in millis that this <c>Level</c> was played.
		/// </summary>
		/// <returns>The slowest duration in millis.</returns>
		public long GetSlowestDurationMillis() {
			return LevelStorage.GetSlowestDurationMillis(this);
		}

		/// <summary>
		/// Gets the fastest duration in millis that this <c>Level</c> was played.
		/// </summary>
		/// <returns>The fastest duration in millis.</returns>
		public long GetFastestDurationMillis() {
			return LevelStorage.GetFastestDurationMillis(this);
		}

		/// <summary>
		/// Gets the play duration of this <c>Level</c> in millis.
		/// </summary>
		/// <returns>The play duration in millis.</returns>
		public long GetPlayDurationMillis() {
			long now = this.UseTimeScaling ? Mathf.RoundToInt(Time.time * 1000) : DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
			long duration = Elapsed;

			if (StartTime != 0 || UseTimeScaling) {
				duration += now - StartTime;
			}

			return duration;
		}

		/// <summary>
		/// Starts this <c>Level</c>.
		/// </summary>
		public bool Start() {
			if (State == LevelState.Running) {
				SoomlaUtils.LogError(TAG, "Can't start a level that is already running. state=" + State);
				return false;
			}

			SoomlaUtils.LogDebug(TAG, "Starting level with world id: " + _id);

			if (!CanStart()) {
				return false;
			}

			if (State != LevelState.Paused) {
				Elapsed = 0;
				LevelStorage.IncTimesStarted(this);
			}

			StartTime = this.UseTimeScaling ? Mathf.RoundToInt(Time.time * 1000) : DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
			State = LevelState.Running;
			return true;
		}

		/// <summary>
		/// Pauses this <c>Level</c>.
		/// </summary>
		public void Pause() {
			if (State != LevelState.Running) {
				SoomlaUtils.LogError(TAG, "Can't pause a level that is not running. state=" + State);
				return;
			}

			long now = this.UseTimeScaling ? Mathf.RoundToInt(Time.time * 1000) : DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

			Elapsed += now - StartTime;
			StartTime = 0;

			State = LevelState.Paused;
		}

		/// <summary>
		/// Ends this <c>Level</c>.
		/// </summary>
		/// <param name="completed">If set to <c>true</c> completed.</param>
		public void End(bool completed) {
			
			// check end() called without matching start(),
			// i.e, the level is not running nor paused
			if(State != LevelState.Running && State != LevelState.Paused) {
				SoomlaUtils.LogError(TAG, "end() called without prior start()! ignoring.");
				return;
			}

			State = LevelState.Ended;

			long duration = GetPlayDurationMillis ();
			LevelStorage.SetLastDurationMillis(this, duration);

			if (completed) {

				// Calculate the slowest \ fastest durations of level play
				
				if (duration > GetSlowestDurationMillis ()) {
					LevelStorage.SetSlowestDurationMillis (this, duration);
				}

				// We assume that levels' duration is never 0
				long fastest = GetFastestDurationMillis ();
				if (fastest == 0 || duration < fastest) {
					LevelStorage.SetFastestDurationMillis (this, duration);
				}
			}

			if (completed) {
				
				foreach (Score score in Scores.Values) {
					score.Reset(true); // resetting scores
				}

				SetCompleted(true);
			}

			// Count number of times this level was played
			LevelStorage.IncTimesPlayed(this);
		}

		/// <summary>
		/// Restarts this <c>Level</c>.
		/// </summary>
		/// <param name="completed">If set to <c>true</c> completed.</param>
		public void Restart(bool completed) {
			if (State == LevelState.Running || State == LevelState.Paused) {
				End(completed);
			}
			Start();
		}

		/// <summary>
		/// Sets this <c>Level</c> as completed. 
		/// </summary>
		/// <param name="completed">If set to <c>true</c> completed.</param>
		public override void SetCompleted(bool completed) {
			if (completed) {
				State = LevelState.Completed;
				LevelStorage.IncTimesCompleted(this);
			} else {
				State = LevelState.Idle;
			}
			base.SetCompleted(completed);
		}
	}
}

