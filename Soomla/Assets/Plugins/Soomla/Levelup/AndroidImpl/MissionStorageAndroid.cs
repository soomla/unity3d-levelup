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

namespace Soomla.Levelup
{
	/// <summary>
	/// <c>MissionStorage</c> for Android.
	/// A utility class for persisting and querying the state of <c>Mission</c>s.
	/// Use this class to check if a certain <c>Mission</c> is complete, or to
	/// set its completion status.
	/// </summary>
	public class MissionStorageAndroid : MissionStorage {
#if UNITY_ANDROID && !UNITY_EDITOR
	
	override protected void _setCompleted(Mission mission, bool up, bool notify) {
		AndroidJNI.PushLocalFrame(100);
		using(AndroidJavaClass jniMissionStorage = new AndroidJavaClass("com.soomla.levelup.data.MissionStorage")) {
			jniMissionStorage.CallStatic("setCompleted", mission.ID, up, notify);
		}
		AndroidJNI.PopLocalFrame(IntPtr.Zero);
	}
	
	override protected bool _getTimesCompleted(Mission mission) {
		int times = 0;
		AndroidJNI.PushLocalFrame(100);
		using(AndroidJavaClass jniMissionStorage = new AndroidJavaClass("com.soomla.levelup.data.MissionStorage")) {
			times = jniMissionStorage.CallStatic<int>("getTimesCompleted", mission.ID);
		}
		AndroidJNI.PopLocalFrame(IntPtr.Zero);
		return times;
	}
	
#endif
	}
}

