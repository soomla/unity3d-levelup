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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Soomla;

namespace Soomla.Levelup {

	/// <summary>
	/// A game can have multiple <c>World</c>s or a single one, and <c>World</c>s can also contain other 
	/// <c>World</c>s in them. A <c>World</c> can contain a set of <c>Level</c>s, or multiple sets of  
	/// <c>Level</c>s. A <c>World</c> can also have a <c>Gate</c> that defines the criteria to enter it. 
	/// Games that don’t have the concept of <c>World</c>s can be modeled as single <c>World</c> games. 
	/// 
	/// Real Game Example: "Candy Crush" uses <c>World</c>s.
	/// </summary>
	public class World : SoomlaEntity<World> {

		/// <summary>
		/// Used in log error messages.
		/// </summary>
		private static string TAG = "SOOMLA World";

		/// <summary>
		/// <c>Gate</c> that defines the criteria to enter this <c>World</c>.
		/// </summary>
		public Gate Gate;

		/// <summary>
		/// A map of the <c>World</c>s included in this <c>World</c>.
		/// </summary>
		public Dictionary<string, World> InnerWorldsMap = new Dictionary<string, World>();

		/// <summary>
		/// The <c>Score</c>s associated with this <c>World</c>.
		/// </summary>
		public Dictionary<string, Score> Scores = new Dictionary<string, Score>();

		/// <summary>
		/// <c>Missions</c> in this <c>World</c>.
		/// </summary>
		public List<Mission> Missions = new List<Mission>();

		/// <summary>
		/// Gets the inner <c>World</c>s list.
		/// </summary>
		/// <value>The inner worlds list.</value>
		public List<World> InnerWorldsList {
			get { return InnerWorldsMap.Values.ToList(); }
		}  

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="id">ID.</param>
		public World(String id)
			: base(id)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="gate">A <c>Gate</c> that needs to be unlocked in order to enter this 
		/// <c>World</c>.</param>
		/// <param name="innerWorlds">A list of <c>World</c>s contained within this one.</param>
		/// <param name="scores">Scores of this <c>World</c>.</param>
		/// <param name="missions"><c>Missions</c>s that are available in this <c>World</c>.</param>
		public World(string id, Gate gate, Dictionary<string, World> innerWorlds, Dictionary<string, Score> scores, List<Mission> missions)
			: base(id)
		{
			this.InnerWorldsMap = (innerWorlds != null) ? innerWorlds : new Dictionary<string, World>();
			this.Scores = (scores != null) ? scores : new Dictionary<string, Score>();
			this.Gate = gate;
			this.Missions = (missions != null) ? missions : new List<Mission>();
		}

		/// <summary>
		/// Constructs a <c>World</c> from a JSON object. 
		/// </summary>
		/// <param name="jsonWorld">Json World.</param>
		public World(JSONObject jsonWorld)
			: base(jsonWorld)
		{	
			InnerWorldsMap = new Dictionary<string, World>();
			List<JSONObject> worldsJSON = jsonWorld[LUJSONConsts.LU_WORLDS].list;
			
			// Iterates over all inner worlds in the JSON array and for each one creates
			// an instance according to the world type.
			foreach (JSONObject worldJSON in worldsJSON) {
				World innerWorld = World.fromJSONObject(worldJSON);
				if (innerWorld != null) {
					InnerWorldsMap.Add(innerWorld._id, innerWorld);
				}
			}
			
			Scores = new Dictionary<String, Score>();
			List<JSONObject> scoresJSON = jsonWorld[LUJSONConsts.LU_SCORES].list;
			
			// Iterates over all scores in the JSON array and for each one creates
			// an instance according to the score type.
			foreach (JSONObject scoreJSON in scoresJSON) {
				Score score = Score.fromJSONObject(scoreJSON);
				if (score != null) {
					Scores.Add(score.ID, score);
				}
			}

			Missions = new List<Mission>();
			List<JSONObject> missionsJSON = jsonWorld[LUJSONConsts.LU_MISSIONS].list;
			
			// Iterates over all challenges in the JSON array and creates an instance for each one.
			foreach (JSONObject missionJSON in missionsJSON) {
				Missions.Add(Mission.fromJSONObject(missionJSON));
			}
			
			JSONObject gateJSON = jsonWorld[LUJSONConsts.LU_GATE];
			if (gateJSON != null && gateJSON.keys != null && gateJSON.keys.Count > 0) {
				Gate = Gate.fromJSONObject (gateJSON);
			}
		}

		/// <summary>
		/// Converts this <c>World</c> into a JSON object. 
		/// </summary>
		/// <returns>The JSON object.</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();

			obj.AddField(LUJSONConsts.LU_GATE, (Gate==null ? new JSONObject(JSONObject.Type.OBJECT) : Gate.toJSONObject()));
			
			JSONObject worldsArr = new JSONObject(JSONObject.Type.ARRAY);
			foreach (World world in InnerWorldsMap.Values) {
				worldsArr.Add(world.toJSONObject());
			}
			obj.AddField(LUJSONConsts.LU_WORLDS, worldsArr);

			JSONObject scoresArr = new JSONObject(JSONObject.Type.ARRAY);
			foreach (Score score in Scores.Values) {
				scoresArr.Add(score.toJSONObject());
			}
			obj.AddField(LUJSONConsts.LU_SCORES, scoresArr);

			JSONObject missionsArr = new JSONObject(JSONObject.Type.ARRAY);
			foreach (Mission mission in Missions) {
				missionsArr.Add(mission.toJSONObject());
			}
			obj.AddField(LUJSONConsts.LU_MISSIONS, missionsArr);
			
			return obj;
		}

		/// <summary>
		/// Converts the given JSON object into a <c>World</c>.
		/// </summary>
		/// <returns>The JSON object to be converted.</returns>
		/// <param name="worldObj">World object.</param>
		public static World fromJSONObject(JSONObject worldObj) {
			string className = worldObj[JSONConsts.SOOM_CLASSNAME].str;
			
			World world = (World) Activator.CreateInstance(Type.GetType("Soomla.Levelup." + className), new object[] { worldObj });
			
			return world;
		}

#if UNITY_ANDROID 
		//&& !UNITY_EDITOR
		public AndroidJavaObject toJNIObject() {
			using(AndroidJavaClass jniWorldClass = new AndroidJavaClass("com.soomla.levelup.World")) {
				return jniWorldClass.CallStatic<AndroidJavaObject>("fromJSONString", toJSONObject().print());
			}
		}
#endif

		/** Add elements to world. **/

		/// <summary>
		/// Adds the given inner <c>World</c> to this <c>World</c>.
		/// </summary>
		/// <param name="world">World to be added.</param>
		public void AddInnerWorld(World world) {
			InnerWorldsMap.Add(world._id, world);
		}

		/// <summary>
		/// Adds the given <c>Mission</c> to this <c>World</c>.
		/// </summary>
		/// <param name="mission">Mission to be added.</param>
		public void AddMission(Mission mission) {
			Missions.Add(mission);
		}

		/// <summary>
		/// Adds the given <c>Score</c> to this <c>World</c>.
		/// </summary>
		/// <param name="score">Score to be added.</param>
		public void AddScore(Score score) {
			Scores.Add(score.ID, score);
		}


		/** Automatic generation of levels. **/

		/// <summary>
		/// Auto-generates an ID for a <c>Level</c>, according to the given ID and index.
		/// </summary>
		/// <returns>The auto-generated ID for the <c>Level</c>.</returns>
		/// <param name="id">World ID.</param>
		/// <param name="idx">Level index.</param>
		private string IdForAutoGeneratedLevel(string id, int idx) {
			return id + "_level" + idx;
		}

		/// <summary>
		/// Auto-generates an ID for a <c>Score</c>, according to the given ID and index.
		/// </summary>
		/// <returns>The auto-generated ID for the <c>Score</c>.</returns>
		/// <param name="id">World ID.</param>
		/// <param name="idx">Score index.</param>
		private string IdForAutoGeneratedScore(string id, int idx) {
			return id + "_score" + idx;
		}

		/// <summary>
		/// Auto-generates an ID for a <c>Gate</c>, according to the given ID.
		/// </summary>
		/// <returns>The auto-generated ID for the <c>Gate</c>.</returns>
		/// <param name="id">Level ID.</param>
		private string IdForAutoGeneratedGate(string id) {
			return id + "_gate";
		}

		/// <summary>
		/// Auto-generates an ID for a <c>Mission</c>, according to the given ID and index.
		/// </summary>
		/// <returns>The auto-generated ID for the <c>Mission</c>.</returns>
		/// <param name="id">World ID.</param>
		/// <param name="idx">Mission index.</param>
		private string IdForAutoGeneratedMission(string id, int idx) {
			return id + "_mission" + idx;
		}

		/// <summary>
		/// Creates a batch of <c>Level</c>s and adds them to this <c>World</c>. This function will save you 
		/// a lot of time - instead of creating many levels one by one, you can create them all at once.
		/// </summary>
		/// <param name="numLevels">The number of <c>Level</c>s to be added to this <c>World</c>.</param>
		/// <param name="gateTemplate">The <c>Gate</c> for the levels.</param>
		/// <param name="scoreTemplate"><c>Score</c> template for the <c>Level</c>s.</param>
		/// <param name="missionTemplate"><c>Mission</c> template for the <c>Level</c>s.</param>
		public void BatchAddLevelsWithTemplates(int numLevels, Gate gateTemplate, Score scoreTemplate, Mission missionTemplate) {
			List<Score> scoreTemplates = new List<Score>();
			if (scoreTemplate != null) {
				scoreTemplates.Add(scoreTemplate);
			}
			List<Mission> missionTemplates = new List<Mission>();
			if (missionTemplate != null) {
				missionTemplates.Add(missionTemplate);
			}

			BatchAddLevelsWithTemplates(numLevels, gateTemplate, scoreTemplates, missionTemplates);
		}
		public void BatchAddLevelsWithTemplates(int numLevels, Gate gateTemplate, List<Score> scoreTemplates, List<Mission>missionTemplates) {
			for (int i=0; i<numLevels; i++) {
				string lvlId = IdForAutoGeneratedLevel(_id, i);
				Level aLvl = new Level(lvlId);
				
				aLvl.Gate = gateTemplate.Clone(IdForAutoGeneratedGate(lvlId));

				if (scoreTemplates != null) {
					for(int k=0; k<scoreTemplates.Count(); k++) {
						aLvl.AddScore(scoreTemplates[k].Clone(IdForAutoGeneratedScore(lvlId, k)));
					}
				}
				
				if (missionTemplates != null) {
					for(int k=0; i<missionTemplates.Count(); k++) {
						aLvl.AddMission(missionTemplates[k].Clone(IdForAutoGeneratedMission(lvlId, k)));
					}
				}
				
				this.InnerWorldsMap.Add(lvlId, aLvl);
			}
		}


		/** For Single Score **/

		/// <summary>
		/// Sets the single <c>Score</c> value to the given amount.
		/// </summary>
		/// <param name="amount">Amount to set.</param>
		public void SetSingleScoreValue(double amount) {
			if (Scores.Count() == 0) {
				return;
			}
			SetScoreValue(Scores.First().Value.ID, amount);
		}

		/// <summary>
		/// Decreases the single <c>Score</c> value by the given amount.
		/// </summary>
		/// <param name="amount">Amount to decrease by.</param>
		public void DecSingleScore(double amount) {
			if (Scores.Count() == 0) {
				return;
			}
			DecScore(Scores.First().Value.ID, amount);
		}

		/// <summary>
		/// Increases the single <c>Score</c> value by the given amount.
		/// </summary>
		/// <param name="amount">Amount to increase by.</param>
		public void IncSingleScore(double amount) {
			if (Scores.Count() == 0) {
				return;
			}
			IncScore(Scores.First().Value.ID, amount);
		}

		/// <summary>
		/// Retrieves the single <c>Score</c> value.
		/// </summary>
		/// <returns>The single score.</returns>
		public Score GetSingleScore() {
			if (Scores.Count() == 0) {
				return null;
			}
			return Scores.First().Value;
		}

		/// <summary>
		/// Sums the inner <c>World</c> records.
		/// </summary>
		/// <returns>The sum of inner <c>World</c> records.</returns>
		public double SumInnerWorldsRecords() {
			double ret = 0;
			foreach(World world in InnerWorldsList) {
				ret += world.GetSingleScore().Record;
			}
			return ret;
		}


		/** For more than one Score **/

		/// <summary>
		/// Resets the <c>Score</c>s for this <c>World</c>.
		/// </summary>
		/// <param name="save">If set to <c>true</c> save.</param>
		public void ResetScores(bool save) {
			if (Scores == null || Scores.Count == 0) {
				SoomlaUtils.LogError(TAG, "(ResetScores) You don't have any scores defined in this world. World id: " + _id);
				return;
			}
			
			foreach (Score score in Scores.Values) {
				score.Reset(save);
			}
		}

		/// <summary>
		/// Decreases the <c>Score</c> with the given ID by the given amount.
		/// </summary>
		/// <param name="scoreId">ID of the <c>Score</c> to be decreased.</param>
		/// <param name="amount">Amount to decrease by.</param>
		public void DecScore(string scoreId, double amount) {
			Scores[scoreId].Dec(amount);
		}

		/// <summary>
		/// Increases the <c>Score</c> with the given ID by the given amount.
		/// </summary>
		/// <param name="scoreId">ID of the <c>Score</c> to be increased.</param>
		/// <param name="amount">Amount.</param>
		public void IncScore(string scoreId, double amount) {
			Scores[scoreId].Inc(amount);
		}

		/// <summary>
		/// Retrieves the record <c>Score</c>s.
		/// </summary>
		/// <returns>The record <c>Score</c>s - each <c>Score</c> ID with its record.</returns>
		public Dictionary<string, double> GetRecordScores() {
			Dictionary<string, double> records = new Dictionary<string, double>();
			foreach(Score score in Scores.Values) {
				records.Add(score.ID, score.Record);
			}
			
			return records;
		}

		/// <summary>
		/// Retrieves the latest <c>Score</c>s.
		/// </summary>
		/// <returns>The latest <c>Score</c>s - each <c>Score</c> ID with its record.</returns>
		public Dictionary<string, double> GetLatestScores() {
			Dictionary<string, double> latest = new Dictionary<string, double>();
			foreach (Score score in Scores.Values) {
				latest.Add(score.ID, score.Latest);
			}
			
			return latest;
		}

		/// <summary>
		/// Sets the <c>Score</c> with the given ID to have the given value.
		/// </summary>
		/// <param name="id"><c>Score</c> whose value is to be set.</param>
		/// <param name="scoreVal">Value to set.</param>
		public void SetScoreValue(string id, double scoreVal) {
			SetScoreValue(id, scoreVal, false);
		}
		public void SetScoreValue(string id, double scoreVal, bool onlyIfBetter) {
			Score score = Scores[id];
			if (score == null) {
				SoomlaUtils.LogError(TAG, "(setScore) Can't find score id: " + id + "  world id: " + this._id);
				return;
			}
			score.SetTempScore(scoreVal, onlyIfBetter);
		}


		/** Completion **/

		/// <summary>
		/// Determines whether this <c>World</c> is completed.
		/// </summary>
		/// <returns><c>true</c> if this instance is completed; otherwise, <c>false</c>.</returns>
		public bool IsCompleted() {
			return WorldStorage.IsCompleted(this);
		}

		/// <summary>
		/// Sets this <c>World</c> as completed.
		/// </summary>
		/// <param name="completed">If set to <c>true</c> completed.</param>
		public virtual void SetCompleted(bool completed) {
			SetCompleted(completed, false);
		}
		public void SetCompleted(bool completed, bool recursive) {
			if (recursive) {
				foreach (World world in InnerWorldsMap.Values) {
					world.SetCompleted(completed, true);
				}
			}
			WorldStorage.SetCompleted(this, completed);
		}


		/** Reward Association **/

		/// <summary>
		/// Assigns the given reward to this <c>World</c>.
		/// </summary>
		/// <param name="reward">Reward to assign.</param>
		public void AssignReward(Reward reward) {
			String olderReward = GetAssignedRewardId();
			if (!string.IsNullOrEmpty(olderReward)) {
				Reward oldReward = LevelUp.GetInstance().GetReward(olderReward);
				if (oldReward != null) {
					oldReward.Take();
				}
			}

			// We have to make sure the assigned reward can be assigned unlimited times.
			// There's no real reason why it won't be.
			if (reward.Schedule.ActivationLimit > 0) {
				reward.Schedule.ActivationLimit = 0;
			}

			reward.Give();
			WorldStorage.SetReward(this, reward.ID);
		}

		/// <summary>
		/// Retrieves the assigned reward ID.
		/// </summary>
		/// <returns>The assigned reward ID.</returns>
		public String GetAssignedRewardId() {
			return WorldStorage.GetAssignedReward(this);
		}

		/// <summary>
		/// Determines if this world is available for starting, based on either if there 
		/// is no <c>Gate</c> for this <c>World</c>, or if the <c>Gate</c> is open.
		/// </summary>
		/// <returns><c>true</c> if this instance can start; otherwise, <c>false</c>.</returns>
		public bool CanStart() {
			return Gate == null || Gate.IsOpen();
		}

	}
}

