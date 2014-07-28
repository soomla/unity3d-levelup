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
	
	public class LevelUp {

		public static readonly string DB_KEY_PREFIX = "soomla.levelup.";

		public World InitialWorld;
		public Dictionary<string, Reward> Rewards;

		public void Initialize(World initialWorld, List<Reward> rewards) {
			InitialWorld = initialWorld;
//			save();

			if (rewards != null) {
				Dictionary<string, Reward> rewardMap = new Dictionary<string, Reward> ();
				foreach (Reward reward in rewards) {
						rewardMap.Add (reward.ID, reward);
				}
				Rewards = rewardMap;
			}
		}

		public Reward GetReward(string rewardId) {
			Reward reward = null;
			Rewards.TryGetValue(rewardId, out reward);
			return reward;
		}

		public Score GetScore(string scoreId) {
			Score retScore = null;
			InitialWorld.Scores.TryGetValue(scoreId, out retScore);
			if (retScore == null) {
				retScore = fetchScoreFromWorlds(scoreId, InitialWorld.InnerWorldsMap);
			}
			
			return retScore;
		}

		public World GetWorld(string worldId) {
			if (InitialWorld.ID == worldId) {
				return InitialWorld;
			}

			return fetchWorld(worldId, InitialWorld.InnerWorldsMap);
		}

		/// <summary>
		/// Counts all levels in all worlds and inner worlds.
		/// </summary>
		/// <returns>The number of levels in all worlds and their inner worlds</returns>
		public int GetLevelCount() {
			return GetLevelCountInWorld(InitialWorld);
		}

		/// <summary>
		/// Counts all levels in the given world and its inner worlds.
		/// </summary>
		/// <param name="world">The world to examine</param>
		/// <returns>The number of levels in the given world and its inner worlds</returns>
		public int GetLevelCountInWorld(World world) {
			int count = 0;
			foreach (World initialWorld in world.InnerWorldsMap.Values) {
				count += getRecursiveCount(initialWorld, (World innerWorld) => {
					return innerWorld.GetType() == typeof(Level);
				});
			}
			return count;
		}

		/// <summary>
		/// Counts all worlds and their inner worlds with or without their levels.
		/// </summary>
		/// <param name="withLevels">Indicates whether to count also levels</param>
		/// <returns>The number of worlds and their inner worlds, and optionally their inner levels</returns>
		public int GetWorldCount(bool withLevels) {
			return getRecursiveCount(InitialWorld, (World innerWorld) => {
				return withLevels ?
					(innerWorld.GetType() == typeof(World) || innerWorld.GetType() == typeof(Level)) :
						(innerWorld.GetType() == typeof(World));
			});
		}

		/// <summary>
		/// Counts all completed levels.
		/// </summary>
		/// <returns>The number of completed levels and their inner completed levels</returns>
		public int GetCompletedLevelCount() {
			return getRecursiveCount(InitialWorld, (World innerWorld) => {
				return innerWorld.GetType() == typeof(Level) && innerWorld.IsCompleted();
			});
		}

		/// <summary>
		/// Counts the number of completed worlds.
		/// </summary>
		/// <returns>The number of completed worlds and their inner completed worlds</returns>
		public int GetCompletedWorldCount() {
			return getRecursiveCount(InitialWorld, (World innerWorld) => {
				return innerWorld.GetType() == typeof(World) && innerWorld.IsCompleted();
			});
		}


		private static LevelUp instance = null;
		public static LevelUp GetInstance() {
			if (instance == null) {
				instance = new LevelUp();
			}
			return instance;
		}

		private LevelUp() {}



		// NOTE: Not sure we need a save function.

//		private void save() {
//			string lu_json = toJSONObject().print();
//			SoomlaUtils.LogDebug(TAG, "saving LevelUp to DB. json is: " + lu_json);
//			string key = DB_KEY_PREFIX + "model";
//
//			// TODO: save on Android and iOS with KeyValueStorage
//			// KeyValueStorage.setValue(key, lu_json);
//		}

		private JSONObject toJSONObject() {
			JSONObject jsonObject = new JSONObject(JSONObject.Type.OBJECT);

			jsonObject.AddField(LUJSONConsts.LU_MAIN_WORLD, InitialWorld.toJSONObject());
			
			return jsonObject;
		}

		private const string TAG = "SOOMLA LevelUp";

		private Score fetchScoreFromWorlds(string scoreId, Dictionary<string, World> worlds) {
			Score retScore = null;
			foreach (World world in worlds.Values) {
				world.Scores.TryGetValue(scoreId, out retScore);
				if (retScore == null) {
					retScore = fetchScoreFromWorlds(scoreId, world.InnerWorldsMap);
				}
				if (retScore != null) {
					break;
				}
			}
			
			return retScore;
		}
		
		private World fetchWorld(string worldId, Dictionary<string, World> worlds) {
			World retWorld;
			worlds.TryGetValue(worldId, out retWorld);
			if (retWorld == null) {
				foreach (World world in worlds.Values) {
					retWorld = fetchWorld(worldId, world.InnerWorldsMap);
				}
			}
			
			return retWorld;
		}

		private int getRecursiveCount(World world, Func<World, bool> isAccepted) {
			int count = 0;
			
			// If the predicate is true, increment
			if (isAccepted(world)) {
				count++;
			}
			
			foreach (World innerWorld in world.InnerWorldsMap.Values) {
				
				// Recursively count for inner world
				count += getRecursiveCount(innerWorld, isAccepted);
			}
			return count;
		}

	}
}

