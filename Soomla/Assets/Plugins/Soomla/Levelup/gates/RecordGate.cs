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

		public RecordGate(string gateId, string associatedScoreId, double desiredRecord)
			: base(gateId)
		{
			AssociatedScoreId = associatedScoreId;
			DesiredRecord = desiredRecord;

			registerEvents();
		}
		
		/// <summary>
		/// see parent.
		/// </summary>
		public RecordGate(JSONObject jsonGate)
			: base(jsonGate)
		{
			this.AssociatedScoreId = jsonGate[JSONConsts.SOOM_ASSOCSCOREID].str;
			this.DesiredRecord = jsonGate[JSONConsts.SOOM_DESIRED_RECORD].n;

			registerEvents();
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <returns>see parent</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();
			obj.AddField(JSONConsts.SOOM_ASSOCITEMID, this.AssociatedScoreId);
			obj.AddField(JSONConsts.SOOM_DESIRED_RECORD, Convert.ToInt32(this.DesiredRecord));

			return obj;
		}

		protected virtual void registerEvents() {
			LevelupEvents.OnScoreRecordChanged += onScoreRecordChanged;
		}

		protected virtual void unregisterEvents() {
			LevelupEvents.OnScoreRecordChanged -= onScoreRecordChanged;
		}

		public void onScoreRecordChanged(Score score) {
			if (score.ScoreId.Equals (AssociatedScoreId)) {
				unregisterEvents();
				ForceOpen(true);
			}
		}

		public override bool CanOpen() {
			Score score = LevelUp.GetScore(AssociatedScoreId);
			if (score == null) {
				SoomlaUtils.LogError(TAG, "(canOpen) couldn't find score with scoreId: " + AssociatedScoreId);
				return false;
			}

			return score.HasRecordReached(DesiredRecord);
		}

		protected override bool TryOpenInner() {
			if (CanOpen()) {
				ForceOpen(true);
				return true;
			}
			
			return false;
		}


	}
}

