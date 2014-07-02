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
	public class BalanceMission : Mission
	{
		public string AssociatedItemId;
		public int DesiredBalance;

		public BalanceMission(string name, string missionId, string associatedItemId, int desiredBalance)
			: base(missionId, name)
		{
			AssociatedItemId = associatedItemId;
			DesiredBalance = desiredBalance;
		}

		public BalanceMission(string missionId, string name, List<Reward> rewards, string associatedItemId, int desiredBalance)
			: base(missionId, name, rewards)
		{
			AssociatedItemId = associatedItemId;
			DesiredBalance = desiredBalance;
		}
		
		/// <summary>
		/// see parent.
		/// </summary>
		public BalanceMission(JSONObject jsonMission)
			: base(jsonMission)
		{
			this.AssociatedItemId = jsonMission[JSONConsts.SOOM_ASSOCITEMID].str;
			this.DesiredBalance = jsonMission[JSONConsts.SOOM_DESIRED_BALANCE].n;
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <returns>see parent</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();
			obj.AddField(JSONConsts.SOOM_ASSOCITEMID, this.AssociatedItemId);
			obj.AddField(JSONConsts.SOOM_DESIRED_BALANCE, this.DesiredBalance);

			return obj;
		}

		// TODO: register for events and handle them
	}
}

