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
	/// The top level container for the unity-levelup model and definitions.
	/// It stores the configurations of all the game's worlds hierarchy and
	/// provides lookup for levelup model elements.
	/// </summary>
	public class LevelUp {

		public static readonly string DB_KEY_PREFIX = "soomla.levelup.";

		public World InitialWorld;
		public Dictionary<string, Reward> Rewards;

		/// <summary>
		/// Initializes the specified initialWorld and rewards.
		/// </summary>
		/// <param name="initialWorld">Initial world.</param>
		/// <param name="rewards">Rewards.</param>
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

		/// <summary>
		/// Gets the reward with the given ID.
		/// </summary>
		/// <returns>The reward that was fetched.</returns>
		/// <param name="rewardId">Reward identifier.</param>
		public Reward GetReward(string rewardId) {
			Reward reward = null;
			Rewards.TryGetValue(rewardId, out reward);
			return reward;
		}

		/// <summary>
		/// Gets the score with the given score ID.
		/// </summary>
		/// <returns>The score.</returns>
		/// <param name="scoreId">Score ID of the score to be fetched.</param>
		public Score GetScore(string scoreId) {
			Score retScore = null;
			InitialWorld.Scores.TryGetValue(scoreId, out retScore);
			if (retScore == null) {
				retScore = fetchScoreFromWorlds(scoreId, InitialWorld.InnerWorldsMap);
			}
			
			return retScore;
		}

		/// <summary>
		/// Gets the world with the given world ID.
		/// </summary>
		/// <returns>The world.</returns>
		/// <param name="worldId">World ID of the score to be fetched.</param>
		public World GetWorld(string worldId) {
			if (InitialWorld.ID == worldId) {
				return InitialWorld;
			}

			return fetchWorld(worldId, InitialWorld.InnerWorldsMap);
		}

		/// <summary>
		/// Counts all the levels in all worlds and inner worlds.
		/// </summary>
		/// <returns>The number of levels in all worlds and their inner worlds.</returns>
		public int GetLevelCount() {
			return GetLevelCountInWorld(InitialWorld);
		}

		/// <summary>
		/// Counts all levels in the given world and its inner worlds.
		/// </summary>
		/// <param name="world">The world to examine.</param>
		/// <returns>The number of levels in the given world and its inner worlds.</returns>
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
		/// <param name="withLevels">Indicates whether to count levels also.</param>
		/// <returns>The number of worlds and their inner worlds, and optionally their inner levels.</returns>
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
		/// <returns>The number of completed levels and their inner completed levels.</returns>
		public int GetCompletedLevelCount() {
			return getRecursiveCount(InitialWorld, (World innerWorld) => {
				return innerWorld.GetType() == typeof(Level) && innerWorld.IsCompleted();
			});
		}

		/// <summary>
		/// Counts the number of completed worlds.
		/// </summary>
		/// <returns>The number of completed worlds and their inner completed worlds.</returns>
		public int GetCompletedWorldCount() {
			return getRecursiveCount(InitialWorld, (World innerWorld) => {
				return innerWorld.GetType() == typeof(World) && innerWorld.IsCompleted();
			});
		}


		private static LevelUp instance = null;

		/// <summary>
		/// Gets this instance of LevelUp.
		/// Use this function when initializing LevelUp.
		/// </summary>
		/// <returns>This instance of LevelUp.</returns>
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

		/// <summary>
		/// Converts this instance of LevelUp to JSONobject
		/// </summary>
		/// <returns>The JSON object.</returns>
		private JSONObject toJSONObject() {
			JSONObject jsonObject = new JSONObject(JSONObject.Type.OBJECT);

			jsonObject.AddField(LUJSONConsts.LU_MAIN_WORLD, InitialWorld.toJSONObject());
			
			return jsonObject;
		}

		private const string TAG = "SOOMLA LevelUp";

		/// <summary>
		/// Looks for and returns the score with the given scoreID from the given worlds. 
		/// </summary>
		/// <returns>The score with the given score ID.</returns>
		/// <param name="scoreId">Score identifier.</param>
		/// <param name="worlds">Worlds.</param>
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

		/// <summary>
		/// Fetches the world with the given world ID from the given dictionary of worlds.
		/// </summary>
		/// <returns>The world with the given ID.</returns>
		/// <param name="worldId">World ID.</param>
		/// <param name="worlds">Dictionary of Worlds.</param>
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

		/// <summary>
		/// Counts the number of worlds starting from the given world and according to the given function
		/// that determines which worlds to count in the sum.
		/// </summary>
		/// <returns>The recursive count.</returns>
		/// <param name="world">World to begin counting from.</param>
		/// <param name="isAccepted">Function that determines if the world accepted.</param>
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

