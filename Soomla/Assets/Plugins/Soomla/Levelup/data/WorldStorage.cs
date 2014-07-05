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
	public class WorldStorage
	{

		protected const string TAG = "SOOMLA WorldStorage"; // used for Log error messages

		static WorldStorage _instance = null;
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
			

		public static void SetCompleted(World world, bool completed) {
			SetCompleted(world, completed, true);
		}
		public static void SetCompleted(World world, bool completed, bool notify) {
			instance._setCompleted(world, completed, notify);
		}

		public static bool IsCompleted(World world) {
			return instance._isCompleted(world);
		}

		public static void SetBadge(World world, string badgeRewardId) {
			instance._setBadge(world, badgeRewardId);
		}

		public static string GetAssignedBadge(World world) {
			return instance._getAssignedBadge(world);
		}


		virtual protected void _setCompleted(World world, bool open, bool notify) {
			// TODO: WIE
		}

		virtual protected bool _isCompleted(World world) {
			// TODO: WIE
			return false;
		}

		virtual protected void _setBadge(World world, string badgeRewardId) {
			// TODO: WIE
		}

		virtual protected string _getAssignedBadge(World world) {
			// TODO: WIE
			return null;
		}
	}
}

