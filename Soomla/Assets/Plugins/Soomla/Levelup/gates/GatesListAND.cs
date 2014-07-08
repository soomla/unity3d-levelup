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
/// limitations under the License.using System;

using System.Collections;
using System.Collections.Generic;

namespace Soomla.Levelup
{
	public class GatesListAND : GatesList
	{

		public GatesListAND(string gateId)
			: base(gateId)
		{
			Gates = new List<Gate>();
		}

		public GatesListAND(string gateId, Gate singleGate)
			: base(gateId, singleGate)
		{
		}

		public GatesListAND(string gateId, List<Gate> gates)
			: base(gateId, gates)
		{
		}
		
		/// <summary>
		/// see parent.
		/// </summary>
		public GatesListAND(JSONObject jsonGate)
			: base(jsonGate)
		{
		}

		public override bool IsOpen() {
			// this flag is required since World/Level
			// actually creates a fake AND gate (list) even for a single gate
			// it means that it should answer true when the (only) child subgate is open
			// without being required to open the (anonymous) AND parent
			if(AutoOpenBehavior) {
				foreach (Gate gate in Gates) {
					if (!gate.IsOpen()) {
						return false;
					}
				}
				return true;
			}
			else {
				return base.IsOpen();
			}
		}

		public override bool CanOpen() {
			foreach (Gate gate in Gates) {
				if (!gate.IsOpen()) {
					return false;
				}
			}
			return true;
		}

	}
}

