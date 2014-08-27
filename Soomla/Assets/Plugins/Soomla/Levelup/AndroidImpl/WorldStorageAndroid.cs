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
	public class WorldStorageAndroid : WorldStorage {
#if UNITY_ANDROID && !UNITY_EDITOR
	
		override protected void _setCompleted(World world, bool completed, bool notify) {
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniWorldStorage = new AndroidJavaClass("com.soomla.levelup.data.WorldStorage")) {
				jniWorldStorage.CallStatic("setCompleted", world.ID, completed, notify);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
		
		override protected bool _isCompleted(World world) {
			bool completed = false;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniWorldStorage = new AndroidJavaClass("com.soomla.levelup.data.WorldStorage")) {
				completed = jniWorldStorage.CallStatic<bool>("isCompleted", world.ID);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			return completed;
		}

		override protected void _setReward(World world, string rewardId) {
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniWorldStorage = new AndroidJavaClass("com.soomla.levelup.data.WorldStorage")) {
				jniWorldStorage.CallStatic("setReward", world.ID, rewardId);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
		
		override protected string _getAssignedReward(World world) {
			string rewardId;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniWorldStorage = new AndroidJavaClass("com.soomla.levelup.data.WorldStorage")) {
				rewardId = jniWorldStorage.CallStatic<string>("getAssignedReward", world.ID);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			return rewardId;
		}
	
#endif
	}
}

