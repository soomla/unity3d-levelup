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
	public class RecordGate : Gate
	{
		private const string TAG = "SOOMLA RecordGate";

		public string AssociatedScoreId;
		public double DesiredRecord;

		public RecordGate(string id, string associatedScoreId, double desiredRecord)
			: base(id)
		{
			AssociatedScoreId = associatedScoreId;
			DesiredRecord = desiredRecord;
		}
		
		/// <summary>
		/// see parent.
		/// </summary>
		public RecordGate(JSONObject jsonGate)
			: base(jsonGate)
		{
			this.AssociatedScoreId = jsonGate[JSONConsts.SOOM_ASSOCSCOREID].str;
			this.DesiredRecord = jsonGate[JSONConsts.SOOM_DESIRED_RECORD].n;
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

		protected override void registerEvents() {
			if (!IsOpen ()) {
				LevelUpEvents.OnScoreRecordChanged += onScoreRecordChanged;
			}
		}

		protected override void unregisterEvents() {
			LevelUpEvents.OnScoreRecordChanged -= onScoreRecordChanged;
		}

		public void onScoreRecordChanged(Score score) {
			if (score.ID == AssociatedScoreId) {
				// We were thinking what will happen if the score's record will be broken over and over again.
				// It might have made this function being called over and over again.
				// It won't be called b/c ForceOpen(true) calls 'unregisterEvents' inside.
				ForceOpen(true);
			}
		}

		protected override bool canOpenInner() {
			Score score = LevelUp.GetInstance().GetScore(AssociatedScoreId);
			if (score == null) {
				SoomlaUtils.LogError(TAG, "(canOpenInner) couldn't find score with scoreId: " + AssociatedScoreId);
				return false;
			}

			return score.HasRecordReached(DesiredRecord);
		}

		protected override bool openInner() {
			if (CanOpen()) {

				// There's nothing to do here... If the DesiredRecord was reached then the gate is just open.

				ForceOpen(true);
				return true;
			}
			
			return false;
		}


	}
}

