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
	public class GatesList : Gate
	{
		public List<Gate> Gates;
		protected bool AutoOpenBehavior = false;

		public GatesList(string gateId)
			: base(gateId)
		{
			Gates = new GatesList<Gate>();
		}

		public GatesList(string gateId, Gate singleGate)
			: base(gateId)
		{
			Gates = new ArrayList<Gate>();
			Gates.add(singleGate);
			
			// "fake" gates with 1 sub-gate are auto open
			AutoOpenBehavior = true;
		}

		public GatesList(string gateId, List<Gate> gates)
			: base(gateId)
		{
			Gates = gates;
		}
		
		/// <summary>
		/// see parent.
		/// </summary>
		public GatesList(JSONObject jsonGate)
			: base(jsonGate)
		{
			Gates = new GatesList<Gate>();
			List<JSONObject> gatesJSON = jsonGate[LUJSONConsts.LU_GATES].list;

			// Iterate over all gates in the JSON array and for each one create
			// an instance according to the gate type
			foreach (JSONObject gateJSON in gatesJSON) {
				Gate gate = Gate.fromJSONObject(gateJSON);
				if (gate != null) {
					Gates.add(gate);
				}
			}
			
			if (Gates.Count < 2) {
				// "fake" gates with 1 sub-gate are auto open
				AutoOpenBehavior = true;
			}
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <returns>see parent</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();

			JSONObject gatesJSON = new JSONObject(JSONObject.Type.ARRAY);
			foreach(Gate gate in Gates) {
				gatesJSON.Add(gate.toJSONObject());
			}
			obj.AddField(LUJSONConsts.LU_GATES, gatesJSON);

			return obj;
		}

		public int Count {
			get {
				return Gates.Count;
			}
		}

		public override bool TryOpenInner() {
			if(AutoOpenBehavior) {
				foreach (Gate gate in Gates) {
					gate.TryOpen();
				}
				
				return IsOpen();
			}
			else {
				if (CanOpen()) {
					ForceOpen(true);
					return true;
				}
				
				return false;
			}
		}

		// TODO: register for events and handle them

	}
}

