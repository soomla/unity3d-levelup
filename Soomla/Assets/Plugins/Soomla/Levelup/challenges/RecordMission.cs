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
	public class RecordMission : Mission
	{
		public string AssociatedItemId;
		public double DesiredRecord;

		public RecordMission(string name, string missionId, string associatedItemId, double desiredRecord)
			: base(missionId, name)
		{
			AssociatedItemId = associatedItemId;
			DesiredRecord = desiredRecord;
		}

		public RecordMission(string missionId, string name, List<Reward> rewards, string associatedItemId, double desiredRecord)
			: base(missionId, name, rewards)
		{
			AssociatedItemId = associatedItemId;
			DesiredRecord = desiredRecord;
		}
		
		/// <summary>
		/// see parent.
		/// </summary>
		public RecordMission(JSONObject jsonMission)
			: base(jsonMission)
		{
			this.AssociatedItemId = jsonMission[JSONConsts.SOOM_ASSOCITEMID].str;
			this.DesiredRecord = jsonMission[JSONConsts.SOOM_DESIRED_RECORD].n;
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <returns>see parent</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();
			obj.AddField(JSONConsts.SOOM_ASSOCITEMID, this.AssociatedItemId);
			obj.AddField(JSONConsts.SOOM_DESIRED_RECORD, Convert.ToInt32(this.DesiredRecord));

			return obj;
		}

		// TODO: register for events and handle them
	}
}

