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
		public string AssociatedScoreId;
		public double DesiredRecord;

		public RecordMission(string id, string name, string associatedScoreId, double desiredRecord)
			: base(id, name)
		{
			AssociatedScoreId = associatedScoreId;
			DesiredRecord = desiredRecord;
		}

		public RecordMission(string id, string name, List<Reward> rewards, string associatedScoreId, double desiredRecord)
			: base(id, name, rewards)
		{
			AssociatedScoreId = associatedScoreId;
			DesiredRecord = desiredRecord;
		}
		
		/// <summary>
		/// see parent.
		/// </summary>
		public RecordMission(JSONObject jsonMission)
			: base(jsonMission)
		{
			this.AssociatedScoreId = jsonMission[JSONConsts.SOOM_ASSOCSCOREID].str;
			this.DesiredRecord = jsonMission[JSONConsts.SOOM_DESIRED_RECORD].n;
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <returns>see parent</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();
			obj.AddField(JSONConsts.SOOM_ASSOCSCOREID, this.AssociatedScoreId);
			obj.AddField(JSONConsts.SOOM_DESIRED_RECORD, Convert.ToInt32(this.DesiredRecord));

			return obj;
		}

		/// <summary>
		/// Ons the score record changed.
		/// </summary>
		/// <param name="score">Score.</param>
		/// @Subscribe
		public void onScoreRecordChanged(Score score) {
			if (score.ID == AssociatedScoreId &&
			    score.HasRecordReached(DesiredRecord)) {
					SetCompleted(true);
			}
		}

		protected override void registerEvents() {
			if (!IsCompleted ()) {
				LevelUpEvents.OnScoreRecordChanged += onScoreRecordChanged;
			}
		}

		protected override void unregisterEvents() {
			LevelUpEvents.OnScoreRecordChanged -= onScoreRecordChanged;
		}
	}
}

