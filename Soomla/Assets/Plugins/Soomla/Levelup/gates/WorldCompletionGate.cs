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
	/// <summary>
	/// A specific type of <c>Gate</c> that has an associated world. The gate opens 
	/// once the world has been completed.
	/// </summary>
	public class WorldCompletionGate : Gate
	{
		public string AssociatedWorldId;

		/// <summary>
		/// Constructor. 
		/// </summary>
		/// <param name="id">Gate ID.</param>
		/// <param name="associatedWorldId">Associated world ID.</param>
		public WorldCompletionGate(string id, string associatedWorldId)
			: base(id)
		{
			AssociatedWorldId = associatedWorldId;
		}
		
		/// <summary>
		/// Contructor.
		/// </summary>
		/// <param name="jsonGate">JSON gate.</param>
		public WorldCompletionGate(JSONObject jsonGate)
			: base(jsonGate)
		{
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
		/// Checks if this gate meets its criteria for opening, by checking that the 
		/// associated world is not null and has been completed. 
		/// </summary>
		/// <returns>If this world can be opened returns <c>true</c>; otherwise <c>false</c>.</returns>
		protected override bool canOpenInner() {
			World world = LevelUp.GetInstance().GetWorld(AssociatedWorldId);
			return world != null && world.IsCompleted();
		}

		/// <summary>
		/// Opens this gate if it can be opened (its criteria has been met).
		/// </summary>
		/// <returns>If the gate has been opened returns <c>true</c>; otherwise <c>false</c>.</returns>
		protected override bool openInner() {
			if (CanOpen()) {
				ForceOpen(true);
				return true;
			}
			
			return false;
		}

		/// <summary>
		/// Registers relevant events: world-completed event. 
		/// </summary>
		protected override void registerEvents() {
			if (!IsOpen ()) {
				LevelUpEvents.OnWorldCompleted += onWorldCompleted;
			}
		}

		/// <summary>
		/// Unregisters relevant events: world-completed event. 
		/// </summary>
		protected override void unregisterEvents() {
			LevelUpEvents.OnWorldCompleted -= onWorldCompleted;
		}

		/// <summary>
		/// Opens this gate if the world-completed event causes the gate's criteria to be met.
		/// </summary>
		/// <param name="world">World to be compared to the associated world.</param>
		/// @subscribe
		public void onWorldCompleted(World world) {
			if (world.ID == AssociatedWorldId) {
				ForceOpen(true);
			}
		}

	}
}

