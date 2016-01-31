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
	/// <c>GateStorage</c> for Android.
	/// A utility class for persisting and querying the state of <c>Gate</c>s.
	/// Use this class to check if a certain <c>Gate</c> is open, or to open it.
	/// </summary>
	public class GateStorageAndroid : GateStorage {
		
#if UNITY_ANDROID && !UNITY_EDITOR

		override protected void _setOpen(Gate gate, bool open, bool notify) {
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniGateStorage = new AndroidJavaClass("com.soomla.levelup.data.GateStorage")) {
				jniGateStorage.CallStatic("setOpen", gate.ID, open, notify);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
		
		override protected bool _isOpen(Gate gate) {
			bool open = false;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniGateStorage = new AndroidJavaClass("com.soomla.levelup.data.GateStorage")) {
				open = jniGateStorage.CallStatic<bool>("isOpen", gate.ID);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			return open;
		}

#endif
	}
}

