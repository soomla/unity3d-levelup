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

namespace Soomla
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
			

		public static void SetCompleted(World world, boolean completed, boolean notify) {
			_instance._setCompleted(world, completed, notify);
		}

		public static boolean IsCompleted(World world) {
			return _instance._isCompleted(world);
		}


		virtual protected void _setCompleted(World world, boolean open, boolean notify) {
			// TODO: WIE
		}

		virtual protected bool _isCompleted(World world) {
			// TODO: WIE
		}
	}
}

