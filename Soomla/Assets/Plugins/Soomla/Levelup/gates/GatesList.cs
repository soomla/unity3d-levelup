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

using System;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Levelup
{
	public abstract class GatesList : Gate
	{
		protected List<Gate> Gates = new List<Gate>();

		public GatesList(string id)
			: base(id)
		{
			Gates = new List<Gate>();
		}

		public GatesList(string id, Gate singleGate)
			: base(id)
		{
			Gates = new List<Gate>();
			Gates.Add(singleGate);
		}

		public GatesList(string id, List<Gate> gates)
			: base(id)
		{
			Gates = gates;
		}
		
		/// <summary>
		/// see parent.
		/// </summary>
		public GatesList(JSONObject jsonGate)
			: base(jsonGate)
		{
			Gates = new List<Gate>();
			List<JSONObject> gatesJSON = jsonGate[LUJSONConsts.LU_GATES].list;

			// Iterate over all gates in the JSON array and for each one create
			// an instance according to the gate type
			foreach (JSONObject gateJSON in gatesJSON) {
				Gate gate = Gate.fromJSONObject(gateJSON);
				if (gate != null) {
					Gates.Add(gate);
				}
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

		public new static GatesList fromJSONObject(JSONObject gateObj) {
			string className = gateObj[JSONConsts.SOOM_CLASSNAME].str;
			
			GatesList gatesList = (GatesList) Activator.CreateInstance(Type.GetType("Soomla.Levelup." + className), new object[] { gateObj });
			
			return gatesList;
		}

		public int Count {
			get {
				return Gates.Count;
			}
		}	

		public void Add(Gate gate) {
			Gates.Add(gate);
		}

		public void Remove(Gate gate) {
			Gates.Remove(gate);
		}

		public Gate this[string id] {
			get { 
				foreach(Gate g in Gates) {
					if (g.ID == id) {
						return g;
					}
				}

				return null;
			}
		}

		public Gate this[int idx] {
			get { return Gates[idx]; }
			set {  Gates[idx] = value; }
		}

		protected override void registerEvents() {
			if (!IsOpen ()) {
				LevelUpEvents.OnGateOpened += onGateOpened;
			}
		}
		
		protected override void unregisterEvents() {
			LevelUpEvents.OnGateOpened -= onGateOpened;
		}

		private void onGateOpened(Gate gate) {
			if(Gates.Contains(gate)) {
				if (CanOpen()) {
					ForceOpen(true);
				}
			}
		}

		protected override bool openInner() {
			if (CanOpen()) {

				// There's nothing to do here... If CanOpen returns true it means that the gates list meets the condition for being opened.

				ForceOpen(true);
				return true;
			}
			return false;
		}

	}
}

