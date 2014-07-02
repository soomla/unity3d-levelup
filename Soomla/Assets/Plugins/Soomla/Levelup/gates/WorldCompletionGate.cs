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


namespace Soomla.Levelup
{
	public class WorldCompletionGate : Gate
	{
		public String AssociatedWorldId;

		public WorldCompletionGate(string gateId, string associatedWorldId)
			: base(gateId)
		{
			AssociatedWorldId = associatedWorldId;
		}
		
		/// <summary>
		/// see parent.
		/// </summary>
		public WorldCompletionGate(JSONObject jsonGate)
			: base(jsonGate)
		{
			this.AssociatedWorldId = jsonItem[LUJSONConsts.LU_GATE_ASSOCWORLDID].str;
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <returns>see parent</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();
			obj.AddField(LUJSONConsts.LU_GATE_ASSOCWORLDID, this.AssociatedWorldId);

			return obj;
		}

		// TODO: register for events and handle them

		public override boolean CanOpen() {
			// TODO: remove THIS
			World world = null;
//			World world = LevelUp.getInstance().getWorld(mAssociatedWorldId); // TODO: take a world from LevelUp
			return world != null && world.IsCompleted();
		}

		public override boolean tryOpenInner() {
				// TODO: move this object to Store module. the following code will not work.

			if (canOpen()) {
				ForceOpen(true);
				return true;
			}
			
			return false;
		}
	}
}

