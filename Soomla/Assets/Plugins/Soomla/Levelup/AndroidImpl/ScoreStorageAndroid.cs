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
	/// <c>ScoreStorage</c> for Android.
	/// A utility class for persisting and querying scores and records.
	/// Use this class to get or set the values of scores and records.
	/// </summary>
	public class ScoreStorageAndroid : ScoreStorage {
#if UNITY_ANDROID && !UNITY_EDITOR

	override protected void _setLatestScore(Score score, double latest) {
		AndroidJNI.PushLocalFrame(100);
		using(AndroidJavaClass jniScoreStorage = new AndroidJavaClass("com.soomla.levelup.data.ScoreStorage")) {
			jniScoreStorage.CallStatic("setLatestScore", score.toJNIObject(), latest);
		}
		AndroidJNI.PopLocalFrame(IntPtr.Zero);
	}
	
	override protected double _getLatestScore(Score score) {
		double latestScore = 0;
		AndroidJNI.PushLocalFrame(100);
		using(AndroidJavaClass jniScoreStorage = new AndroidJavaClass("com.soomla.levelup.data.ScoreStorage")) {
			latestScore = jniScoreStorage.CallStatic<double>("getLatestScore", score.toJNIObject());
		}
		AndroidJNI.PopLocalFrame(IntPtr.Zero);
		return latestScore;
	}
	
	override protected void _setRecordScore(Score score, double record) {
		AndroidJNI.PushLocalFrame(100);
		using(AndroidJavaClass jniScoreStorage = new AndroidJavaClass("com.soomla.levelup.data.ScoreStorage")) {
			jniScoreStorage.CallStatic("setRecordScore", score.toJNIObject(), record);
		}
		AndroidJNI.PopLocalFrame(IntPtr.Zero);
	}
	
	override protected double _getRecordScore(Score score) {
		double recordScore = 0;
		AndroidJNI.PushLocalFrame(100);
		using(AndroidJavaClass jniScoreStorage = new AndroidJavaClass("com.soomla.levelup.data.ScoreStorage")) {
			recordScore = jniScoreStorage.CallStatic<double>("getRecordScore", score.toJNIObject());
		}
		AndroidJNI.PopLocalFrame(IntPtr.Zero);
		return recordScore;
	}

#endif
	}
}

