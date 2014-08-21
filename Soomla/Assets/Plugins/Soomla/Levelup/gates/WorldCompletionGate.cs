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
	/// A specific type of <c>Gate</c> that has an associated world. The <c>Gate</c> opens 
	/// once the <c>World</c> has been completed.
	/// </summary>
	public class WorldCompletionGate : Gate
	{
		/// <summary>
		/// ID of the <c>World</c> that needs to be completed. 
		/// </summary>
		public string AssociatedWorldId;

		/// <summary>
		/// Constructor. 
		/// </summary>
		/// <param name="id">ID.</param>
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
		/// Converts this <c>Gate</c> to a JSONObject.
		/// </summary>
		/// <returns>The JSON object.</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();

			return obj;
		}

		/// <summary>
		/// Checks if this <c>Gate</c> meets its criteria for opening, by checking that the 
		/// associated world is not null and has been completed. 
		/// </summary>
		/// <returns>If this <c>World</c> can be opened returns <c>true</c>; otherwise <c>false</c>.</returns>
		protected override bool canOpenInner() {
			World world = LevelUp.GetInstance().GetWorld(AssociatedWorldId);
			return world != null && world.IsCompleted();
		}

		/// <summary>
		/// Opens this <c>Gate</c> if it can be opened (its criteria has been met).
		/// </summary>
		/// <returns>Upon success of opening returns <c>true</c>; otherwise <c>false</c>.</returns>
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
		/// Opens this <c>Gate</c> if the world-completed event causes the <c>Gate</c>'s criteria to be met.
		/// </summary>
		/// <param name="world"><c>World</c> to be compared to the associated <c>World</c>.</param>
		/// @subscribe
		public void onWorldCompleted(World world) {
			if (world.ID == AssociatedWorldId) {
				ForceOpen(true);
			}
		}

	}
}

