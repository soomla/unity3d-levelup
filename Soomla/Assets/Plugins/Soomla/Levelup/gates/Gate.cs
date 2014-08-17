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
using Soomla;

namespace Soomla.Levelup {

	/// <summary>
	/// A gate is an object which defines certain criteria for progressing
	/// between the game's <c>World</c>s or <c>Level</c>s. The gate is a 
	/// criteria or a list of rules which which must be met to pass. The 
	/// rules are based on components of the previous world or level: scores 
	/// achieved, missions completed, etc. The gate is opened once the logical 
	/// conditions are met. In some games, gates can be opened with a payment 
	/// or social task.
	/// </summary>
	public abstract class Gate : SoomlaEntity<Gate> {

		private const string TAG = "SOOMLA Gate";

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="id">Gate ID.</param>
		protected Gate (string id)
			: this(id, "")
		{
		}

		/// <summary>
		/// Contructor.
		/// </summary>
		/// <param name="id">Gate ID.</param>
		/// <param name="name">Gate name.</param>
		protected Gate (string id, string name)
			: base(id, name, "")
		{
			registerEvents();
		}

		/// <summary>
		/// Constructor. 
		/// </summary>
		/// <param name="jsonObj">JSON object.</param>
		public Gate(JSONObject jsonObj) 
			: base(jsonObj)
		{
			registerEvents();
		}

		/// <summary>
		/// Converts this gate to a JSONObject.
		/// </summary>
		/// <returns>The JSON object.</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();
			
			return obj;
		}

		/// <summary>
		/// Converts the given JSONObject into a Gate. 
		/// </summary>
		/// <returns>The JSON object.</returns>
		/// <param name="gateObj">Gate object.</param>
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

		/// <summary>
		/// Attempts to open this gate. 
		/// </summary>
		public bool Open() {
			//  check in gate storage if it's already open.
			if (GateStorage.IsOpen(this)) {
				return true;
			}
			return openInner();
		}

		/// <summary>
		/// Sets the gate to open without checking if the gate meets its criteria.
		/// </summary>
		/// <param name="open">If set to <c>true</c> open the gate.</param>
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
				// we can do this here ONLY because we check 'isOpen == open' a few lines above.
				registerEvents();
			}
		}

		/// <summary>
		/// Determines whether this gate is open.
		/// </summary>
		/// <returns>If this gate is open returns <c>true</c>; otherwise, <c>false</c>.</returns>
		public bool IsOpen() {
			return GateStorage.IsOpen(this);
		}

		/// <summary>
		/// Checks if this gate meets its criteria for opening.
		/// </summary>
		/// <returnsIif this gate can be opened returns <c>true</c>; otherwise, <c>false</c>.</returns>
		public bool CanOpen() {
			// check in gate storage if the gate is open
			if (GateStorage.IsOpen(this)) {
				return true;
			}

			return canOpenInner();
		}

		/// <summary>
		/// Registers relevant events. Each specific type of Gate must implement this method. 
		/// </summary>
		protected abstract void registerEvents();

		/// <summary>
		/// Unregisters relevant events. Each specific type of Gate must implement this method. 
		/// </summary>
		protected abstract void unregisterEvents();

		/// <summary>
		/// Checks if this gate meets its criteria for opening.
		/// Each specific type of gate must implement this method to 
		/// add specific gate criteria.
		/// </summary>
		/// <returns>If if criteria met for opening this gate returns <c>true</c>, 
		/// otherwise <c>false</c>.</returns>
		protected abstract bool canOpenInner();

		/// <summary>
		/// Opens the inner.
		/// </summary>
		/// <returns><c>true</c>, if inner was opened, <c>false</c> otherwise.</returns>
		protected abstract bool openInner();

		/// <summary>
		/// Clones this gate and gives it the given ID.
		/// </summary>
		/// <param name="newGateId">Cloned gate ID.</param>
		public override Gate Clone(string newGateId) {
			return (Gate) base.Clone(newGateId);
		}
	}
}

