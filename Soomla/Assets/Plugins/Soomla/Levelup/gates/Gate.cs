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
	/// A <c>Gate</c> is an object that defines certain criteria, and is opened when this criteria is met.
	/// <c>Gate</c>s are usually associated with <c>Mission</c>s - each <c>Mission</c> has a <c>Gate</c>
	/// that needs to be opened in order for the <c>Mission</c> to be complete.
	/// </summary>
	public abstract class Gate : SoomlaEntity<Gate> {

		private const string TAG = "SOOMLA Gate";
		private bool eventsRegistered = false;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="id">ID.</param>
		protected Gate (string id)
			: this(id, "")
		{
		}

		/// <summary>
		/// Contructor.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="name">Name.</param>
		protected Gate (string id, string name)
			: base(id, name, "")
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="jsonObj">JSON object.</param>
		public Gate(JSONObject jsonObj)
			: base(jsonObj)
		{
		}

		/// <summary>
		/// Converts this <c>Gate</c> to a JSONObject.
		/// </summary>
		/// <returns>The JSON object.</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();

			return obj;
		}

		/// <summary>
		/// Converts the given JSONObject into a <c>Gate</c>.
		/// </summary>
		/// <returns>The JSON object.</returns>
		/// <param name="gateObj">Gate object.</param>
		public static Gate fromJSONObject(JSONObject gateObj) {
			if (gateObj == null) {
				// Challenge allows its gate undefined, and uses corresponding gates from missions.
				return null;
			}

			string className = gateObj[JSONConsts.SOOM_CLASSNAME].str;

			Gate gate = (Gate) Activator.CreateInstance(Type.GetType("Soomla.Levelup." + className), new object[] { gateObj });

			return gate;
		}

		/// <summary>
		/// Attempts to open this <c>Gate</c>, if it has not been opened aready.
		/// </summary>
		public bool Open() {
			//  check in gate storage if it's already open.
			if (GateStorage.IsOpen(this)) {
				return true;
			}
			return openInner();
		}

		/// <summary>
		/// Sets the <c>Gate</c> to open without checking if the <c>Gate</c> meets its criteria.
		/// </summary>
		/// <param name="open">If set to <c>true</c> open the <c>Gate</c>.</param>
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
		/// Determines whether this <c>Gate</c> is open.
		/// </summary>
		/// <returns>If this <c>Gate</c> is open returns <c>true</c>; otherwise, <c>false</c>.</returns>
		public bool IsOpen() {
			return GateStorage.IsOpen(this);
		}

		/// <summary>
		/// Checks if this <c>Gate</c> meets its criteria for opening.
		/// </summary>
		/// <returns>If this <c>Gate</c> can be opened returns <c>true</c>; otherwise, <c>false</c>.</returns>
		public bool CanOpen() {
			// check in gate storage if the gate is open.
			// gates are only opened once
			if (GateStorage.IsOpen(this)) {
				return false;
			}

			return canOpenInner();
		}

		/// <summary>
		/// Clones this <c>Gate</c> and gives it the given ID.
		/// </summary>
		/// <param name="newGateId">Cloned gate ID.</param>
		public override Gate Clone(string newGateId) {
			return (Gate) base.Clone(newGateId);
		}

		internal void OnAttached() {
			if (eventsRegistered) {
				return;
			}

			registerEvents();
			eventsRegistered = true;
		}

		internal void OnDetached() {
			if (!eventsRegistered) {
				return;
			}

			unregisterEvents();
			eventsRegistered = false;
		}

		private void onInitialize() {
			OnAttached();
		}

		/// <summary>
		/// Registers relevant events. Each specific type of <c>Gate</c> must implement this method.
		/// </summary>
		protected abstract void registerEvents();

		/// <summary>
		/// Unregisters relevant events. Each specific type of <c>Gate</c> must implement this method.
		/// </summary>
		protected abstract void unregisterEvents();

		/// <summary>
		/// Checks if this <c>Gate</c> meets its criteria for opening.
		/// Each specific type of <c>Gate</c> must implement this method to
		/// add specific <c>Gate</c> criteria.
		/// </summary>
		/// <returns>If the criteria is met for opening this <c>Gate</c> returns <c>true</c>;
		/// otherwise <c>false</c>.</returns>
		protected abstract bool canOpenInner();

		/// <summary>
		/// Opens this <c>Gate</c>.
		/// </summary>
		/// <returns>If this <c>Gate</c> was opened returns <c>true</c>; otherwise <c>false</c>.</returns>
		protected abstract bool openInner();

		//	public abstract void OnInitialize();
	}
}
