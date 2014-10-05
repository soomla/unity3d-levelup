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
	/// A utility class for persisting and querying the state of <c>Gate</c>s.
	/// Use this class to check if a certain <c>Gate</c> is open, or to open it.
	/// </summary>
	public class GateStorage
	{

		protected const string TAG = "SOOMLA GateStorage";

		/// <summary>
		/// Holds an instance of <c>GateStorage</c> or <c>GateStorageAndroid</c> or <c>GateStorageIOS</c>.
		/// </summary>
		static GateStorage _instance = null;

		/// <summary>
		/// Determines which <c>GateStorage</c> to use according to the platform in use
		/// and if the Unity Editor is being used. 
		/// </summary>
		/// <value>The instance to use.</value>
		static GateStorage instance {
			get {
				if(_instance == null) {
					#if UNITY_ANDROID && !UNITY_EDITOR
					_instance = new GateStorageAndroid();
					#elif UNITY_IOS && !UNITY_EDITOR
					_instance = new GateStorageIOS();
					#else
					_instance = new GateStorage();
					#endif
				}
				return _instance;
			}
		}
			

		/** The following functions call the relevant instance-specific functions. **/

		public static void SetOpen(Gate gate, bool open) {
			instance._setOpen(gate, open, true);
		}
		public static void SetOpen(Gate gate, bool open, bool notify) {
			instance._setOpen(gate, open, notify);
		}

		public static bool IsOpen(Gate gate) {
			return instance._isOpen(gate);
		}


		/** Unity-Editor Functions **/

		/// <summary>
		/// Sets the given <c>Gate</c> as open if <c>open</c> is <c>true.</c>
		/// Otherwise sets as closed. 
		/// </summary>
		/// <param name="gate">The <c>Gate</c> to open/close.</param>
		/// <param name="open">If set to <c>true</c> set the <c>Gate</c> to open; 
		/// <param name="notify">If set to <c>true</c> trigger event.</param>
		protected virtual void _setOpen(Gate gate, bool open, bool notify) {
#if UNITY_EDITOR
			string key = keyGateOpen(gate.ID);
			
			if (open) {
				PlayerPrefs.SetString(key, "yes");

				if (notify) {
					LevelUpEvents.OnGateOpened(gate);
				}
			} else {
				PlayerPrefs.DeleteKey(key);
			}
#endif
		}

		/// <summary>
		/// Determines if the given <c>Gate</c> is open.
		/// </summary>
		/// <returns>If the given <c>Gate</c> is open returns <c>true</c>; 
		/// otherwise, <c>false</c>.</returns>
		/// <param name="gate"><c>Gate</c> to check if is open.</param>
		protected virtual bool _isOpen(Gate gate) {
#if UNITY_EDITOR
			string key = keyGateOpen(gate.ID);
			string val = PlayerPrefs.GetString (key);
			return !string.IsNullOrEmpty(val);
#else
			return false;
#endif
		}


		/** Keys (private helper functions if Unity Editor is being used.) **/

#if UNITY_EDITOR
		private static string keyGateOpen(string gateId) {
			return keyGates(gateId, "open");
		}

		private static string keyGates(string gateId, string postfix) {
			return SoomlaLevelUp.DB_KEY_PREFIX + "gates." + gateId + "." + postfix;
		}
#endif
	}
}

