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
	/// This is the top level container for the unity-levelup model and definitions.
	/// It stores the configurations of the game's world-hierarchy and provides
	/// lookup functions for levelup model elements.
	/// </summary>
	public class LevelUp {

		/// <summary>
		/// Used in log messages.
		/// </summary>
		public static readonly string DB_KEY_PREFIX = "soomla.levelup.";

		/// <summary>
		/// Used in log error messages.
		/// </summary>
		private const string TAG = "SOOMLA LevelUp";

		/// <summary>
		/// The instance of <c>LevelUp</c> for this game.
		/// </summary>
		private static LevelUp instance = null;

		/// <summary>
		/// Initial <c>World</c> to begin the game.
		/// </summary>
		public World InitialWorld;

		/// <summary>
		/// Potential rewards of the <c>InitialWorld</c>.
		/// </summary>
		public Dictionary<string, Reward> Rewards;

		/// <summary>
		/// Initializes the specified <c>InitialWorld</c> and rewards.
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
		/// Retrieves the reward with the given ID.
		/// </summary>
		/// <returns>The reward that was fetched.</returns>
		/// <param name="rewardId">Reward identifier.</param>
		public Reward GetReward(string rewardId) {
			Reward reward = null;
			Rewards.TryGetValue(rewardId, out reward);
			return reward;
		}

		/// <summary>
		/// Retrieves the <c>Score</c> with the given score ID.
		/// </summary>
		/// <returns>The score.</returns>
		/// <param name="scoreId">ID of the <c>Score</c> to be fetched.</param>
		public Score GetScore(string scoreId) {
			Score retScore = null;
			InitialWorld.Scores.TryGetValue(scoreId, out retScore);
			if (retScore == null) {
				retScore = fetchScoreFromWorlds(scoreId, InitialWorld.InnerWorldsMap);
			}
			
			return retScore;
		}

		/// <summary>
		/// Retrieves the <c>World</c> with the given world ID.
		/// </summary>
		/// <returns>The world.</returns>
		/// <param name="worldId">World ID of the <c>Score</c> to be fetched.</param>
		public World GetWorld(string worldId) {
			if (InitialWorld.ID == worldId) {
				return InitialWorld;
			}

			return fetchWorld(worldId, InitialWorld.InnerWorldsMap);
		}

		/// <summary>
		/// Counts all the <c>Level</c>s in all <c>World</c>s and inner <c>World</c>s 
		/// starting from the <c>InitialWorld</c>.
		/// </summary>
		/// <returns>The number of levels in all worlds and their inner worlds.</returns>
		public int GetLevelCount() {
			return GetLevelCountInWorld(InitialWorld);
		}

		/// <summary>
		/// Counts all the <c>Level</c>s in all <c>World</c>s and inner <c>World</c>s 
		/// starting from the given <c>World</c>.
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
		/// Counts all <c>World</c>s and their inner <c>World</c>s with or without their  
		/// <c>Level</c>s according to the given <c>withLevels</c>.
		/// </summary>
		/// <param name="withLevels">Indicates whether to count <c>Level</c>s also.</param>
		/// <returns>The number of <c>World</c>s, and optionally their inner <c>Level</c>s.</returns>
		public int GetWorldCount(bool withLevels) {
			return getRecursiveCount(InitialWorld, (World innerWorld) => {
				return withLevels ?
					(innerWorld.GetType() == typeof(World) || innerWorld.GetType() == typeof(Level)) :
						(innerWorld.GetType() == typeof(World));
			});
		}

		/// <summary>
		/// Counts all completed <c>Level</c>s.
		/// </summary>
		/// <returns>The number of completed <c>Level</c>s and their inner completed 
		/// <c>Level</c>s.</returns>
		public int GetCompletedLevelCount() {
			return getRecursiveCount(InitialWorld, (World innerWorld) => {
				return innerWorld.GetType() == typeof(Level) && innerWorld.IsCompleted();
			});
		}

		/// <summary>
		/// Counts the number of completed <c>World</c>s.
		/// </summary>
		/// <returns>The number of completed <c>World</c>s and their inner completed 
		/// <c>World</c>s.</returns>
		public int GetCompletedWorldCount() {
			return getRecursiveCount(InitialWorld, (World innerWorld) => {
				return innerWorld.GetType() == typeof(World) && innerWorld.IsCompleted();
			});
		}

		/// <summary>
		/// Retrieves this instance of <c>LevelUp</c>. Used when initializing LevelUp.
		/// </summary>
		/// <returns>This instance of <c>LevelUp</c>.</returns>
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
		/// Converts this instance of <c>LevelUp</c> to JSONobject
		/// </summary>
		/// <returns>The JSON object.</returns>
		private JSONObject toJSONObject() {
			JSONObject jsonObject = new JSONObject(JSONObject.Type.OBJECT);

			jsonObject.AddField(LUJSONConsts.LU_MAIN_WORLD, InitialWorld.toJSONObject());
			
			return jsonObject;
		}

		/// <summary>
		/// Fetches the <c>Score</c> with the given scoreID from the given <c>World</c>s. 
		/// </summary>
		/// <returns>The <c>Score</c> with the given score ID.</returns>
		/// <param name="scoreId">ID of the <c>Score</c> to fetch.</param>
		/// <param name="worlds">Worlds to search through.</param>
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
		/// Fetches the <c>World</c> with the given ID from the given dictionary of <c>World</c>s.
		/// </summary>
		/// <returns>The <c>World</c> with the given ID.</returns>
		/// <param name="worldId">ID of <c>World</c> to fetch.</param>
		/// <param name="worlds">Worlds to search through.</param>
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
		/// Counts the number of worlds starting from the given <c>World</c> and according to the 
		/// given function that determines which <c>World</c>s to count in the sum.
		/// </summary>
		/// <returns>The recursive count.</returns>
		/// <param name="world"><c>World</c> to begin counting from.</param>
		/// <param name="isAccepted">Function that determines if the <c>World</c> accepted.</param>
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

