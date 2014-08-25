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
	public class WorldCompletionGate : Gate
	{
		public string AssociatedWorldId;

		public WorldCompletionGate(string id, string associatedWorldId)
			: base(id)
		{
			AssociatedWorldId = associatedWorldId;
		}
		
		/// <summary>
		/// see parent.
		/// </summary>
		public WorldCompletionGate(JSONObject jsonGate)
			: base(jsonGate)
		{
			this.AssociatedWorldId = jsonGate[LUJSONConsts.LU_ASSOCWORLDID].str;
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <returns>see parent</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();
			obj.AddField(LUJSONConsts.LU_ASSOCWORLDID, this.AssociatedWorldId);
			return obj;
		}

		protected override bool canOpenInner() {
			World world = LevelUp.GetInstance().GetWorld(AssociatedWorldId);
			return world != null && world.IsCompleted();
		}

		protected override bool openInner() {

			if (CanOpen()) {
				ForceOpen(true);
				return true;
			}
			
			return false;
		}

		protected override void registerEvents() {
			if (!IsOpen ()) {
				LevelUpEvents.OnWorldCompleted += onWorldCompleted;
			}
		}
		
		protected override void unregisterEvents() {
			LevelUpEvents.OnWorldCompleted -= onWorldCompleted;
		}

		public void onWorldCompleted(World world) {
			if (world.ID == AssociatedWorldId) {
				ForceOpen(true);
			}
		}

	}
}

