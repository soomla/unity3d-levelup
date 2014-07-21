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

		public GatesListAND(string id)
			: base(id)
		{
			Gates = new List<Gate>();
		}

		public GatesListAND(string id, Gate singleGate)
			: base(id, singleGate)
		{
		}

		public GatesListAND(string id, List<Gate> gates)
			: base(id, gates)
		{
		}
		
		/// <summary>
		/// see parent.
		/// </summary>
		public GatesListAND(JSONObject jsonGate)
			: base(jsonGate)
		{
		}

		protected override bool canOpenInner() {
			foreach (Gate gate in Gates) {
				if (!gate.IsOpen()) {
					return false;
				}
			}
			return true;
		}

	}
}

