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
/// See the License for the specific language governing perworlds and
/// limitations under the License.

using UnityEngine;
using System;

namespace Soomla.Levelup
{

	/// <summary>
	/// A utility class for persisting and querying <c>World</c>s.
	/// Use this class to get or set the completion of <c>World</c>s and assign rewards.
	/// </summary>
	public class WorldStorage
	{

		protected const string TAG = "SOOMLA WorldStorage";

		/// <summary>
		/// Holds an instance of <c>WorldStorage</c> or <c>WorldStorageAndroid</c> or <c>WorldStorageIOS</c>.
		/// </summary>
		static WorldStorage _instance = null;

		/// <summary>
		/// Determines which <c>Worldtorage</c> to use according to the platform in use
		/// and if the Unity Editor is being used. 
		/// </summary>
		/// <value>The instance to use.</value>
		static WorldStorage instance {
			get {
				if(_instance == null) {
					#if UNITY_ANDROID && !UNITY_EDITOR
					_instance = new WorldStorageAndroid();
					#elif UNITY_IOS && !UNITY_EDITOR
					_instance = new WorldStorageIOS();
					#else
					_instance = new WorldStorage();
					#endif
				}
				return _instance;
			}
		}

		public static void InitLevelUp()
		{
			instance._initLevelUp();
		}

		/** The following functions call the relevant instance-specific functions. **/

		/** WORLD COMPLETION **/ 

		public static void SetCompleted(World world, bool completed) {
			SetCompleted(world, completed, true);
		}
		public static void SetCompleted(World world, bool completed, bool notify) {
			bool currentStatus = IsCompleted(world);
			if (currentStatus == completed) {
				// we don't need to set the status of a world to the same status over and over again.
				return;
			}

			instance._setCompleted(world, completed, notify);
		}

		public static bool IsCompleted(World world) {
			return instance._isCompleted(world);
		}

		/** WORLD REWARDS **/

		public static void SetReward(World world, string rewardId) {
			instance._setReward(world, rewardId);
		}

		public static string GetAssignedReward(World world) {
			return instance._getAssignedReward(world);
		}


		/** Unity-Editor Functions **/
	
		/// <summary>
		/// Initializes <c>SoomlaLevelUp</c>
		/// </summary>
		protected virtual void _initLevelUp()
		{
			#if UNITY_EDITOR
			LevelUpEvents.OnLevelUpInitialized();
			#endif
		}

		/// <summary>
		/// Sets the given <c>World</c> as completed if <c>completed</c> is <c>true</c>.
		/// </summary>
		/// <param name="world"><c>World</c> to set as completed.</param>
		/// <param name="completed">If set to <c>true</c> the <c>World</c> will be set 
		/// as completed.</param>
		/// <param name="notify">If set to <c>true</c> trigger events.</param>
		protected virtual void _setCompleted(World world, bool completed, bool notify) {
#if UNITY_EDITOR
			string key = keyWorldCompleted(world.ID);
			
			if (completed) {
				PlayerPrefs.SetString(key, "yes");
				
				if (notify) {
					LevelUpEvents.OnWorldCompleted(world);
				}
			} else {
				PlayerPrefs.DeleteKey(key);
			}
#endif
		}

		/// <summary>
		/// Determines if the given <c>World</c> is completed.
		/// </summary>
		/// <returns>If the given <c>World</c> is completed returns <c>true</c>; 
		/// otherwise <c>false</c>.</returns>
		/// <param name="world"><c>World</c> to determine if completed.</param>
		protected virtual bool _isCompleted(World world) {
#if UNITY_EDITOR
			string key = keyWorldCompleted(world.ID);
			string val = PlayerPrefs.GetString (key);
			return !string.IsNullOrEmpty(val);
#else
			return false;
#endif
		}

		
		/// <summary>
		/// Assigns the reward with the given reward ID to the given <c>World</c>. 
		/// </summary>
		/// <param name="world"><c>World</c> to assign a reward to.</param>
		/// <param name="rewardId">ID of reward to assign.</param>
		protected virtual void _setReward(World world, string rewardId) {
#if UNITY_EDITOR
			string key = keyReward (world.ID);
			if (!string.IsNullOrEmpty(rewardId)) {
				PlayerPrefs.SetString(key, rewardId);
			} else {
				PlayerPrefs.DeleteKey(key);
			}

			// Notify world was assigned a reward
			LevelUpEvents.OnWorldAssignedReward(world);
#endif
		}
		
		/// <summary>
		/// Retrieves the given <c>World</c>'s assigned reward.
		/// </summary>
		/// <returns>The assigned reward to retrieve.</returns>
		/// <param name="world"><c>World</c> whose reward is to be retrieved.</param>
		protected virtual string _getAssignedReward(World world) {
#if UNITY_EDITOR
			string key = keyReward (world.ID);
			return PlayerPrefs.GetString (key);
#else
			return null;
#endif
		}


		/** Keys (private helper functions if Unity Editor is being used.) **/

#if UNITY_EDITOR
		private static string keyWorlds(string worldId, string postfix) {
			return SoomlaLevelUp.DB_KEY_PREFIX + "worlds." + worldId + "." + postfix;
		}
		
		private static string keyWorldCompleted(string worldId) {
			return keyWorlds(worldId, "completed");
		}
		
		private static string keyReward(string worldId) {
			return keyWorlds(worldId, "assignedReward");
		}
#endif
	}
}

