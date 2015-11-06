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
	/// <c>LevelStorage</c> for Android.
	/// A utility class for persisting and querying the state of <c>Level</c>s.
	/// Use this class to get or set information about a <c>Level</c>, such as 
	/// the play duration, start or end time, etc.
	/// </summary>
	public class LevelStorageAndroid : LevelStorage {
	#if UNITY_ANDROID && !UNITY_EDITOR

		protected override void _setLastDurationMillis(Level level, long duration) {
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniLevelStorage = new AndroidJavaClass("com.soomla.levelup.data.LevelStorage")) {
				jniLevelStorage.CallStatic("setLastDurationMillis", level.ID, duration);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
		
		protected override long _getLastDurationMillis(Level level) {
			long duration = 0;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniLevelStorage = new AndroidJavaClass("com.soomla.levelup.data.LevelStorage")) {
				duration = jniLevelStorage.CallStatic<long>("getLastDurationMillis", level.ID);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			return duration;
		}

		protected override void _setSlowestDurationMillis(Level level, long duration) {
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniLevelStorage = new AndroidJavaClass("com.soomla.levelup.data.LevelStorage")) {
				jniLevelStorage.CallStatic("setSlowestDurationMillis", level.ID, duration);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
		
		protected override long _getSlowestDurationMillis(Level level) {
			long duration = 0;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniLevelStorage = new AndroidJavaClass("com.soomla.levelup.data.LevelStorage")) {
				duration = jniLevelStorage.CallStatic<long>("getSlowestDurationMillis", level.ID);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			return duration;
		}
		
		protected override void _setFastestDurationMillis(Level level, long duration) {
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniLevelStorage = new AndroidJavaClass("com.soomla.levelup.data.LevelStorage")) {
				jniLevelStorage.CallStatic("setFastestDurationMillis", level.ID, duration);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
		
		protected override long _getFastestDurationMillis(Level level) {
			long duration = 0;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniLevelStorage = new AndroidJavaClass("com.soomla.levelup.data.LevelStorage")) {
				duration = jniLevelStorage.CallStatic<long>("getFastestDurationMillis", level.ID);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			return duration;
		}
		
		
		
		/** Level Times Started **/
		
		protected override int _incTimesStarted(Level level) {
			int timesStarted = 0;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniLevelStorage = new AndroidJavaClass("com.soomla.levelup.data.LevelStorage")) {
				timesStarted = jniLevelStorage.CallStatic<int>("incTimesStarted", level.ID);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			return timesStarted;
		}
		
		protected override int _decTimesStarted(Level level) {
			int timesStarted = 0;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniLevelStorage = new AndroidJavaClass("com.soomla.levelup.data.LevelStorage")) {
				timesStarted = jniLevelStorage.CallStatic<int>("decTimesStarted", level.ID);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			return timesStarted;
		}
		
		protected override int _getTimesStarted(Level level) {
			int timesStarted = 0;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniLevelStorage = new AndroidJavaClass("com.soomla.levelup.data.LevelStorage")) {
				timesStarted = jniLevelStorage.CallStatic<int>("getTimesStarted", level.ID);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			return timesStarted;
		}
		
		
		/** Level Times Played **/
		
		protected override int _incTimesPlayed(Level level) {
			int timesPlayed = 0;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniLevelStorage = new AndroidJavaClass("com.soomla.levelup.data.LevelStorage")) {
				timesPlayed = jniLevelStorage.CallStatic<int>("incTimesPlayed", level.ID);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			return timesPlayed;
		}
		
		protected override int _decTimesPlayed(Level level){
			int timesPlayed = 0;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniLevelStorage = new AndroidJavaClass("com.soomla.levelup.data.LevelStorage")) {
				timesPlayed = jniLevelStorage.CallStatic<int>("decTimesPlayed", level.ID);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			return timesPlayed;
		} 
		
		protected override int _getTimesPlayed(Level level) {
			int timesPlayed = 0;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniLevelStorage = new AndroidJavaClass("com.soomla.levelup.data.LevelStorage")) {
				timesPlayed = jniLevelStorage.CallStatic<int>("getTimesPlayed", level.ID);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			return timesPlayed;
		}

		/** Level Times Completed **/
		
		protected override int _incTimesCompleted(Level level) {
			int timesCompleted = 0;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniLevelStorage = new AndroidJavaClass("com.soomla.levelup.data.LevelStorage")) {
				timesCompleted = jniLevelStorage.CallStatic<int>("incTimesCompleted", level.ID);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			return timesCompleted;
		}
		
		protected override int _decTimesCompleted(Level level){
			int timesCompleted = 0;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniLevelStorage = new AndroidJavaClass("com.soomla.levelup.data.LevelStorage")) {
				timesCompleted = jniLevelStorage.CallStatic<int>("decTimesCompleted", level.ID);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			return timesCompleted;
		} 
		
		protected override int _getTimesCompleted(Level level) {
			int timesCompleted = 0;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniLevelStorage = new AndroidJavaClass("com.soomla.levelup.data.LevelStorage")) {
				timesCompleted = jniLevelStorage.CallStatic<int>("getTimesCompleted", level.ID);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			return timesCompleted;
		}

	//TODO: what's this? error?
//	override protected void _setLatestLevel(Level level, long latest) {
//		AndroidJNI.PushLocalFrame(100);
//		using(AndroidJavaClass jniLevelStorage = new AndroidJavaClass("com.soomla.levelup.data.LevelStorage")) {
//			jniLevelStorage.CallStatic("setLatestLevel", level.ID, latest);
//		}
//		AndroidJNI.PopLocalFrame(IntPtr.Zero);
//	}

	#endif
	}
}

