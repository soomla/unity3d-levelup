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
using System.Runtime.InteropServices;
using Soomla;

namespace Soomla.Levelup
{
	public class MissionStorageIOS : MissionStorage {
#if UNITY_IOS && !UNITY_EDITOR
	
	[DllImport ("__Internal")]
	private static extern void missionStorage_SetCompleted(string missionJson,
	                                               [MarshalAs(UnmanagedType.Bool)] bool completed,
	                                               [MarshalAs(UnmanagedType.Bool)] bool notify);
	[DllImport ("__Internal")]
	[return:MarshalAs(UnmanagedType.I1)]
	private static extern bool missionStorage_IsCompleted(string missionJson);

	
	override protected void _setCompleted(Mission mission, bool completed, bool notify) {
		string missionJson = mission.toJSONObject().ToString();
		missionStorage_SetCompleted(missionJson, completed, notify);
	}
	
	override protected bool _isCompleted(Mission mission) {
		string missionJson = mission.toJSONObject().ToString();
		bool completed = missionStorage_IsCompleted(missionJson);
		SoomlaUtils.LogDebug("SOOMLA/UNITY MissionStorageIOS", string.Format("mission {0} completed={1}", mission.MissionId, completed));
		return completed;
	}
	
#endif
	}
}

