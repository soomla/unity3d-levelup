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
	
	public abstract class Gate {

		private const string TAG = "SOOMLA Gate";

		public string GateId;

		protected Gate (string gateId)
		{
			this.GateId = gateId;
		}
		
//#if UNITY_ANDROID && !UNITY_EDITOR
//		protected Mission(AndroidJavaObject jniVirtualItem) {
//			this.Name = jniVirtualItem.Call<string>("getName");
//			this.Description = jniVirtualItem.Call<string>("getDescription");
//			this.ItemId = jniVirtualItem.Call<string>("getItemId");
//		}
//#endif

		public Gate(JSONObject jsonObj) {
			this.GateId = jsonObj[LUJSONConsts.LU_GATE_GATEID].str;
		}

		public virtual JSONObject toJSONObject() {
			JSONObject obj = new JSONObject(JSONObject.Type.OBJECT);
			obj.AddField(LUJSONConsts.LU_GATE_GATEID, this.GateId);
			obj.AddField(JSONConsts.SOOM_CLASSNAME, GetType().Name);
			
			return obj;
		}

		public static Gate fromJSONObject(JSONObject gateObj) {
			string className = gateObj[JSONConsts.SOOM_CLASSNAME].str;
			
			Gate gate = (Gate) Activator.CreateInstance(Type.GetType("Soomla.Levelup." + className), new object[] { gateObj });
			
			return gate;
		}

		// Equality
		
		public override bool Equals(System.Object obj)
		{
			// If parameter is null return false.
			if (obj == null)
			{
				return false;
			}
			
			// If parameter cannot be cast to Point return false.
			Gate g = obj as Gate;
			if ((System.Object)g == null)
			{
				return false;
			}
			
			// Return true if the fields match:
			return (GateId == g.GateId);
		}
		
		public bool Equals(Gate g)
		{
			// If parameter is null return false:
			if ((object)g == null)
			{
				return false;
			}
			
			// Return true if the fields match:
			return (GateId == g.GateId);
		}
		
		public override int GetHashCode()
		{
			return GateId.GetHashCode();
		}

#if UNITY_ANDROID 
//&& !UNITY_EDITOR
		public AndroidJavaObject toJNIObject() {
			using(AndroidJavaClass jniGateClass = new AndroidJavaClass("com.soomla.levelup.gates.Gate")) {
				return jniGateClass.CallStatic<AndroidJavaObject>("fromJSONString", toJSONObject().print());
			}
		}
#endif

		public bool TryOpen() {
			//  check in gate storage if it's already open
			if (GateStorage.IsOpen(this)) {
				return true;
			}
			return TryOpenInner();
		}

		protected abstract bool TryOpenInner();

		public void ForceOpen(bool open) {
			GateStorage.SetOpen(this, open);
		}

		public virtual bool IsOpen() {
			return GateStorage.IsOpen(this);
		}

		public abstract bool CanOpen();

	}
}

