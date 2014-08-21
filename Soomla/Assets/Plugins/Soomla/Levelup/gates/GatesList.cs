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
	/// <summary>
	/// A representation of one or more <c>Gate</c>s which together define a 
	/// composite criteria for progressing between the game's <c>World</c>s 
	/// or <code>Level</code>s.
	/// </summary>
	public abstract class GatesList : Gate
	{
		/// <summary>
		/// The list of <c>Gate</c>s.
		/// </summary>
		protected List<Gate> Gates = new List<Gate>();

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="id">GatesList ID.</param>
		public GatesList(string id)
			: base(id)
		{
			Gates = new List<Gate>();
		}

		/// <summary>
		/// Constructor for GatesList with one gate.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="singleGate">Single gate in this gateslist.</param>
		public GatesList(string id, Gate singleGate)
			: base(id)
		{
			Gates = new List<Gate>();
			Gates.Add(singleGate);
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="gates">List of gates.</param>
		public GatesList(string id, List<Gate> gates)
			: base(id)
		{
			Gates = gates;
		}
		
		/// <summary>
		/// Constructor. 
		/// </summary>
		/// <param name="jsonGate">JSON gate.</param>
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
		/// Converts this gateslist into a JSONObject.
		/// </summary>
		/// <returns>The JSON object.</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();

			JSONObject gatesJSON = new JSONObject(JSONObject.Type.ARRAY);
			foreach(Gate gate in Gates) {
				gatesJSON.Add(gate.toJSONObject());
			}
			obj.AddField(LUJSONConsts.LU_GATES, gatesJSON);

			return obj;
		}

		/// <summary>
		/// Converts the given JSONObject into a gateslist.
		/// </summary>
		/// <returns>GatesList.</returns>
		/// <param name="gateObj">The JSON object to convert.</param>
		public new static GatesList fromJSONObject(JSONObject gateObj) {
			string className = gateObj[JSONConsts.SOOM_CLASSNAME].str;
			
			GatesList gatesList = (GatesList) Activator.CreateInstance(Type.GetType("Soomla.Levelup." + className), new object[] { gateObj });
			
			return gatesList;
		}

		/// <summary>
		/// Counts the number of gates in this gateslist.
		/// </summary>
		/// <value>The number of gates.</value>
		public int Count {
			get {
				return Gates.Count;
			}
		}	

		/// <summary>
		/// Adds the given gate to this gateslist. 
		/// </summary>
		/// <param name="gate">Gate to add.</param>
		public void Add(Gate gate) {
			Gates.Add(gate);
		}

		/// <summary>
		/// Removes the given gate from this gateslist.
		/// </summary>
		/// <param name="gate">Gate to remove.</param>
		public void Remove(Gate gate) {
			Gates.Remove(gate);
		}

		/// <summary>
		/// Retrieves from this gateslist the gate with the given ID.
		/// </summary>
		/// <param name="id">ID of gate to be retrieved.</param>
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

		/// <summary>
		/// get: Retrieves from this gateslist the gate at the given index.
		/// set: Sets this gateslist at the given index to be `value`.
		/// </summary>
		/// <param name="idx">Index.</param>
		public Gate this[int idx] {
			get { return Gates[idx]; }
			set {  Gates[idx] = value; }
		}

		/// <summary>
		/// Registers relevant events: gate-opened event.
		/// </summary>
		protected override void registerEvents() {
			if (!IsOpen ()) {
				LevelUpEvents.OnGateOpened += onGateOpened;
			}
		}

		/// <summary>
		/// Unregisters relevant events: gate-opened event.
		/// </summary>
		protected override void unregisterEvents() {
			LevelUpEvents.OnGateOpened -= onGateOpened;
		}

		/// <summary>
		/// Opens this gateslist if the gate-opened event causes the GatesList composite criteria to be met.
		/// </summary>
		/// <param name="gate">Gate that was opened.</param>
		/// @subscribe
		private void onGateOpened(Gate gate) {
			if(Gates.Contains(gate)) {
				if (CanOpen()) {
					ForceOpen(true);
				}
			}
		}

		/// <summary>
		/// Opens this gateslist if it can be opened (its criteria has been met).
		/// </summary>
		/// <returns>If the gate has been opened returns <c>true</c>; otherwise 
		/// <c>false</c>.</returns>
		protected override bool openInner() {
			if (CanOpen()) {
				// There's nothing to do here... If CanOpen returns true it means that the 
				// gates list meets the condition for being opened.
				ForceOpen(true);
				return true;
			}
			return false;
		}

	}
}

