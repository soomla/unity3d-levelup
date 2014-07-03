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

		public Dictionary<string, World> InitialWorlds;

		public void Initialize(List<World> initialWorlds) {
			Dictionary<string, World> worldMap = new Dictionary<string, World>();
			foreach (World world in initialWorlds) {
				worldMap.Add(world.WorldId, world);
			}
			InitialWorlds = worldMap;
//			save();
		}

		public Score GetScore(string scoreId) {
			return fetchScoreFromWorlds(scoreId, InitialWorlds);
		}

		public World GetWorld(string worldId) {
			return fetchWorld(worldId, InitialWorlds);
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
			
			JSONObject worldsJSON = new JSONObject(JSONObject.Type.ARRAY);
			foreach (World world in InitialWorlds.Values) {
				worldsJSON.Add(world.toJSONObject());
			}
			jsonObject.AddField(LUJSONConsts.LU_WORLDS, worldsJSON);
			
			return jsonObject;
		}

		private const string TAG = "SOOMLA LevelUp";

		private Score fetchScoreFromWorlds(string scoreId, Dictionary<string, World> worlds) {
			Score retScore = null;
			foreach (World world in worlds.Values) {
				retScore = world.Scores[scoreId];
				if (retScore == null) {
					retScore = fetchScoreFromWorlds(scoreId, world.InnerWorlds);
				}
				if (retScore != null) {
					break;
				}
			}
			
			return retScore;
		}
		
		private World fetchWorld(string worldId, Dictionary<string, World> worlds) {
			World retWorld = worlds[worldId];
			if (retWorld == null) {
				foreach (World world in worlds.Values) {
					retWorld = fetchWorld(worldId, world.InnerWorlds);
				}
			}
			
			return retWorld;
		}

	}
}

