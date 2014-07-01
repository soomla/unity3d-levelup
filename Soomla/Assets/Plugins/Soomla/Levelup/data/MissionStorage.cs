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

namespace Soomla
{
	public class MissionStorage
	{

		protected const string TAG = "SOOMLA MissionStorage"; // used for Log error messages

		static MissionStorage _instance = null;
		static MissionStorage instance {
			get {
				if(_instance == null) {
					#if UNITY_ANDROID && !UNITY_EDITOR
					_instance = new MissionStorageAndroid();
					#elif UNITY_IOS && !UNITY_EDITOR
					_instance = new MissionStorageIOS();
					#else
					_instance = new MissionStorage();
					#endif
				}
				return _instance;
			}
		}
			

		public static void SetCompleted(Mission mission, boolean completed, boolean notify) {
			_instance._setCompleted(mission, completed, notify);
		}

		public static boolean IsCompleted(Mission mission) {
			return _instance._isCompleted(mission);
		}


		virtual protected void _setCompleted(Mission mission, boolean open, boolean notify) {
			// TODO: WIE
		}

		virtual protected bool _isCompleted(Mission mission) {
			// TODO: WIE
		}
	}
}

