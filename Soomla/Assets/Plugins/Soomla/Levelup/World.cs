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
	
	public class World {

		public string WorldId;
		public GatesList Gates;
		public Dictionary<string, World> InnerWorlds;
		public Dictionary<string, Score> Scores;
		public List<Challenge> Challenges;

		public World(String worldId) {
			this.WorldId = worldId;
			this.Gates = null;
			this.InnerWorlds = new Dictionary<string, World>();
			this.Scores = new Dictionary<string, Score>();
			this.Challenges = new List<Challenge>();
		}

		public World(string worldId, GatesList gates, Dictionary<string, World> innerWorlds, Dictionary<string, Score> scores, List<Challenge> challenges) {
			this.WorldId = worldId;
			this.InnerWorlds = innerWorlds;
			this.Scores = scores;
			this.Gates = gates;
			this.Challenges = challenges;
		}

		public World(JSONObject jsonWorld) {
			
			WorldId = jsonWorld[LUJSONConsts.LU_WORLD_WORLDID].str;
			
			InnerWorlds = new Dictionary<string, World>();
			List<JSONObject> worldsJSON = jsonWorld[LUJSONConsts.LU_WORLDS].list;
			
			// Iterate over all inner worlds in the JSON array and for each one create
			// an instance according to the world type
			foreach (JSONObject worldJSON in worldsJSON) {
				World innerWorld = World.fromJSONObject(worldJSON);
				if (innerWorld != null) {
					InnerWorlds.Add(innerWorld.WorldId, innerWorld);
				}
			}
			
			Scores = new Dictionary<String, Score>();
			List<JSONObject> scoresJSON = jsonWorld[LUJSONConsts.LU_SCORES].list;
			
			// Iterate over all scores in the JSON array and for each one create
			// an instance according to the score type
			foreach (JSONObject scoreJSON in scoresJSON) {
				Score score = Score.fromJSONObject(scoreJSON);
				if (score != null) {
					Scores.put(score.ScoreId, score);
				}
			}
			
			Challenges = new List<Challenge>();
			List<JSONObject> challengesJSON = jsonWorld[LUJSONConsts.LU_CHALLENGES].list;
			
			// Iterate over all challenges in the JSON array and create an instance for each one
			foreach (JSONObject challengeJSON in challengesJSON) {
				Challenges.add(new Challenge(challengeJSON));
			}
			
			JSONObject gateListJSON = jsonWorld[LUJSONConsts.LU_GATES];
			Gates = GatesList.fromJSONObject(gateListJSON);
		}

		public virtual JSONObject toJSONObject() {
			JSONObject obj = new JSONObject();

			obj.AddField(JSONConsts.SOOM_CLASSNAME, GetType().Name);
			obj.AddField(LUJSONConsts.LU_WORLD_WORLDID, WorldId);
			obj.AddField(LUJSONConsts.LU_GATES, (Gates==null ? new JSONObject(JSONObject.Type.OBJECT) : Gates.toJSONObject()));
			
			JSONObject worldsArr = new JSONObject(JSONObject.Type.ARRAY);
			foreach (World world in InnerWorlds.Values) {
				worldsArr.Add(world.toJSONObject());
			}
			obj.AddField(LUJSONConsts.LU_WORLDS, worldsArr);

			JSONObject scoresArr = new JSONObject(JSONObject.Type.ARRAY);
			foreach (Score score in Scores.Values) {
				scoresArr.Add(score.toJSONObject());
			}
			obj.AddField(LUJSONConsts.LU_SCORES, scoresArr);

			JSONObject challengesArr = new JSONObject(JSONObject.Type.ARRAY);
			foreach (Challenge challenge in Challenges) {
				challengesArr.Add(challenge.toJSONObject());
			}
			obj.AddField(LUJSONConsts.LU_CHALLENGES, challengesArr);
			
			return obj;
		}

		public static World fromJSONObject(JSONObject worldObj) {
			string className = worldObj[JSONConsts.SOOM_CLASSNAME].str;
			
			World world = (World) Activator.CreateInstance(Type.GetType("Soomla.Levelup." + className), new object[] { worldObj });
			
			return world;
		}


		public Dictionary<string, double> GetRecordScores() {
			Dictionary<string, double> records = new Dictionary<string, double>();
			foreach(Score score in Scores.Values) {
				records.Add(score.ScoreId, score.Record);
			}
			
			return records;
		}

		public Dictionary<string, double> GetLatestScores() {
			Dictionary<string, double> latest = new Dictionary<string, double>();
			foreach (Score score in Scores.Values) {
				latest.Add(score.ScoreId, score.Latest);
			}
			
			return latest;
		}

		public void SetScoreValue(string scoreId, double scoreVal) {
			Score score = Scores[scoreId];
			if (score == null) {
				SoomlaUtils.LogError(TAG, "(setScore) Can't find scoreId: " + scoreId + "  worldId: " + WorldId);
				return;
			}
			score.SetTempScore(scoreVal);
		}

		public bool IsCompleted() {
			return WorldStorage.IsCompleted(this);
		}

		public virtual void SetCompleted(bool completed) {
			SetCompleted(completed, false);
		}
		public void SetCompleted(bool completed, bool recursive) {
			if (recursive) {
				foreach (World world in InnerWorlds.Values) {
					world.SetCompleted(completed, true);
				}
			}
			WorldStorage.SetCompleted(this, completed);
		}

		public bool CanStart() {
			return Gates == null || Gates.IsOpen();
		}

	}
}

