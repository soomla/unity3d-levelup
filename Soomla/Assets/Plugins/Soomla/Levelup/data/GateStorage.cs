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
	public class GateStorage
	{

		protected const string TAG = "SOOMLA GateStorage"; // used for Log error messages

		static GateStorage _instance = null;
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
			

		public static void SetOpen(Gate gate, bool open, bool notify) {
			_instance._setOpen(gate, give, notify);
		}

		public static bool IsOpen(Gate gate) {
			return _instance._isOpen(gate);
		}


		virtual protected void _setOpen(Gate gate, bool open, bool notify) {
			// TODO: WIE
		}

		virtual protected bool _isOpen(Gate gate) {
			// TODO: WIE
		}
	}
}

