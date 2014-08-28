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
using System.Runtime.InteropServices;

namespace Soomla.Levelup
{
	/// <summary>
	/// <c>GateStorage</c> for iOS.
	/// A utility class for persisting and querying the state of <c>Gate</c>s.
	/// Use this class to check if a certain <c>Gate</c> is open, or to open it.
	/// </summary>
	public class GateStorageIOS : GateStorage {
		
#if UNITY_IOS && !UNITY_EDITOR

		[DllImport ("__Internal")]
		private static extern void gateStorage_SetOpen(string gateId,
		                                               [MarshalAs(UnmanagedType.Bool)] bool open,
		                                               [MarshalAs(UnmanagedType.Bool)] bool notify);
		[DllImport ("__Internal")]
		[return:MarshalAs(UnmanagedType.I1)]
		private static extern bool gateStorage_IsOpen(string gateId);


		override protected void _setOpen(Gate gate, bool open, bool notify) {
			gateStorage_SetOpen(gate.ID, open, notify);
		}
		
		override protected bool _isOpen(Gate gate) {
			return gateStorage_IsOpen(gate.ID);
		}

#endif
	}
}

