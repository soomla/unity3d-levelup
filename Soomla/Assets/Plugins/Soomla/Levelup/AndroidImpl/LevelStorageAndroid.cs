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
	#if UNITY_ANDROID && !UNITY_EDITOR

	protected override void _setSlowestDuration(Level level, double duration) {
		AndroidJNI.PushLocalFrame(100);
		using(AndroidJavaClass jniLevelStorage = new AndroidJavaClass("com.soomla.levelup.data.LevelStorage")) {
			jniLevelStorage.CallStatic("setSlowestDuration", level.toJNIObject(), duration);
		}
		AndroidJNI.PopLocalFrame(IntPtr.Zero);
	}
	
	protected override double _getSlowestDuration(Level level) {
		double duration = 0;
		AndroidJNI.PushLocalFrame(100);
		using(AndroidJavaClass jniLevelStorage = new AndroidJavaClass("com.soomla.levelup.data.LevelStorage")) {
			duration = jniLevelStorage.CallStatic<double>("getSlowestDuration", level.toJNIObject());
		}
		AndroidJNI.PopLocalFrame(IntPtr.Zero);
		return duration;
	}
	
	protected override void _setFastestDuration(Level level, double duration) {
		AndroidJNI.PushLocalFrame(100);
		using(AndroidJavaClass jniLevelStorage = new AndroidJavaClass("com.soomla.levelup.data.LevelStorage")) {
			jniLevelStorage.CallStatic("setFastestDuration", level.toJNIObject(), duration);
		}
		AndroidJNI.PopLocalFrame(IntPtr.Zero);
	}
	
	protected override double _getFastestDuration(Level level) {
		double duration = 0;
		AndroidJNI.PushLocalFrame(100);
		using(AndroidJavaClass jniLevelStorage = new AndroidJavaClass("com.soomla.levelup.data.LevelStorage")) {
			duration = jniLevelStorage.CallStatic<double>("getSlowestDuration", level.toJNIObject());
		}
		AndroidJNI.PopLocalFrame(IntPtr.Zero);
		return duration;
	}
	
	
	
	/** Level Times Started **/
	
	protected override int _incTimesStarted(Level level) {
		int timesStarted = 0;
		AndroidJNI.PushLocalFrame(100);
		using(AndroidJavaClass jniLevelStorage = new AndroidJavaClass("com.soomla.levelup.data.LevelStorage")) {
			timesStarted = jniLevelStorage.CallStatic<int>("incTimesStarted", level.toJNIObject());
		}
		AndroidJNI.PopLocalFrame(IntPtr.Zero);
		return timesStarted;
	}
	
	protected override int _decTimesStarted(Level level) {
		int timesStarted = 0;
		AndroidJNI.PushLocalFrame(100);
		using(AndroidJavaClass jniLevelStorage = new AndroidJavaClass("com.soomla.levelup.data.LevelStorage")) {
			timesStarted = jniLevelStorage.CallStatic<int>("decTimesStarted", level.toJNIObject());
		}
		AndroidJNI.PopLocalFrame(IntPtr.Zero);
		return timesStarted;
	}
	
	protected override int _getTimesStarted(Level level) {
		int timesStarted = 0;
		AndroidJNI.PushLocalFrame(100);
		using(AndroidJavaClass jniLevelStorage = new AndroidJavaClass("com.soomla.levelup.data.LevelStorage")) {
			timesStarted = jniLevelStorage.CallStatic<int>("getTimesStarted", level.toJNIObject());
		}
		AndroidJNI.PopLocalFrame(IntPtr.Zero);
		return timesStarted;
	}
	
	
	/** Level Times Played **/
	
	protected override int _incTimesPlayed(Level level) {
		int timesPlayed = 0;
		AndroidJNI.PushLocalFrame(100);
		using(AndroidJavaClass jniLevelStorage = new AndroidJavaClass("com.soomla.levelup.data.LevelStorage")) {
			timesPlayed = jniLevelStorage.CallStatic<int>("incTimesPlayed", level.toJNIObject());
		}
		AndroidJNI.PopLocalFrame(IntPtr.Zero);
		return timesPlayed;
	}
	
	protected override int _decTimesPlayed(Level level){
		int timesPlayed = 0;
		AndroidJNI.PushLocalFrame(100);
		using(AndroidJavaClass jniLevelStorage = new AndroidJavaClass("com.soomla.levelup.data.LevelStorage")) {
			timesPlayed = jniLevelStorage.CallStatic<int>("decTimesPlayed", level.toJNIObject());
		}
		AndroidJNI.PopLocalFrame(IntPtr.Zero);
		return timesPlayed;
	} 
	
	protected override int _getTimesPlayed(Level level) {
		int timesPlayed = 0;
		AndroidJNI.PushLocalFrame(100);
		using(AndroidJavaClass jniLevelStorage = new AndroidJavaClass("com.soomla.levelup.data.LevelStorage")) {
			timesPlayed = jniLevelStorage.CallStatic<int>("getTimesPlayed", level.toJNIObject());
		}
		AndroidJNI.PopLocalFrame(IntPtr.Zero);
		return timesPlayed;
	}

	override protected void _setLatestLevel(Level level, double latest) {
		AndroidJNI.PushLocalFrame(100);
		using(AndroidJavaClass jniLevelStorage = new AndroidJavaClass("com.soomla.levelup.data.LevelStorage")) {
			jniLevelStorage.CallStatic("setLatestLevel", level.toJNIObject(), latest);
		}
		AndroidJNI.PopLocalFrame(IntPtr.Zero);
	}

	#endif
}

