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
using System.Collections.Generic;

namespace Soomla.Levelup {
	
	public abstract class Gate : SoomlaEntity<Gate> {

		private const string TAG = "SOOMLA Gate";

		protected Gate (string id)
			: this(id, "")
		{
		}

		protected Gate (string id, string name)
			: base(id, name, "")
		{
			registerEvents();
		}

		public Gate(JSONObject jsonObj) 
			: base(jsonObj)
		{
			registerEvents();
		}

		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();
			
			return obj;
		}

		public static Gate fromJSONObject(JSONObject gateObj) {
			string className = gateObj[JSONConsts.SOOM_CLASSNAME].str;
			
			Gate gate = (Gate) Activator.CreateInstance(Type.GetType("Soomla.Levelup." + className), new object[] { gateObj });
			
			return gate;
		}

#if UNITY_ANDROID && !UNITY_EDITOR
		public AndroidJavaObject toJNIObject() {
			using(AndroidJavaClass jniGateClass = new AndroidJavaClass("com.soomla.levelup.gates.Gate")) {
				return jniGateClass.CallStatic<AndroidJavaObject>("fromJSONString", toJSONObject().print());
			}
		}
#endif

		public bool Open() {
			//  check in gate storage if it's already open
			if (GateStorage.IsOpen(this)) {
				return true;
			}
			return openInner();
		}

		public void ForceOpen(bool open) {
			bool isOpen = IsOpen();
			if (isOpen == open) {
				// if it's already open why open it again?
				return;
			}

			GateStorage.SetOpen(this, open);
			if (open) {
				unregisterEvents();
			} else {
				// we can do this here ONLY becasue we check 'isOpen == open' a few lines above.
				registerEvents();
			}
		}

		public bool IsOpen() {
			return GateStorage.IsOpen(this);
		}

		public bool CanOpen() {
			// check in gate storage if the gate is open
			if (GateStorage.IsOpen(this)) {
				return true;
			}

			return canOpenInner();
		}

		protected abstract void registerEvents();
		protected abstract void unregisterEvents();

		protected abstract bool canOpenInner();
		protected abstract bool openInner();

		public override Gate Clone(string newGateId) {
			return (Gate) base.Clone(newGateId);
		}
	}
}

