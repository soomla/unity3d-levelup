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
	/// A specific type of <c>Gate</c> that has an associated score and a desired record. 
	/// The gate opens once the player achieves the desired record for the given score.
	/// </summary>
	public class RecordGate : Gate
	{
		private const string TAG = "SOOMLA RecordGate";

		public string AssociatedScoreId;
		public double DesiredRecord;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="id">Gate ID.</param>
		/// <param name="associatedScoreId">Associated score ID.</param>
		/// <param name="desiredRecord">Desired record.</param>
		public RecordGate(string id, string associatedScoreId, double desiredRecord)
			: base(id)
		{
			AssociatedScoreId = associatedScoreId;
			DesiredRecord = desiredRecord;
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="jsonGate">JSON gate.</param>
		public RecordGate(JSONObject jsonGate)
			: base(jsonGate)
		{
			this.AssociatedScoreId = jsonGate[JSONConsts.SOOM_ASSOCSCOREID].str;
			this.DesiredRecord = jsonGate[JSONConsts.SOOM_DESIRED_RECORD].n;
		}
		
		/// <summary>
		/// Converts this gate to a JSONObject.
		/// </summary>
		/// <returns>The JSON object.</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();
			obj.AddField(JSONConsts.SOOM_ASSOCSCOREID, this.AssociatedScoreId);
			obj.AddField(JSONConsts.SOOM_DESIRED_RECORD, Convert.ToInt32(this.DesiredRecord));

			return obj;
		}

		/// <summary>
		/// Registers relevant events: score-record changed event.
		/// </summary>
		protected override void registerEvents() {
			if (!IsOpen ()) {
				LevelUpEvents.OnScoreRecordChanged += onScoreRecordChanged;
			}
		}

		/// <summary>
		/// Unregisters relevant events: score-record changed event.
		/// </summary>
		protected override void unregisterEvents() {
			LevelUpEvents.OnScoreRecordChanged -= onScoreRecordChanged;
		}

		/// <summary>
		/// Opens this gate if the score-record-changed event causes the gate's criteria to be met.
		/// </summary>
		/// <param name="score">The score whose record has changed.</param>
		/// @subscribe
		public void onScoreRecordChanged(Score score) {
			if (score.ID == AssociatedScoreId &&
			    score.HasRecordReached(DesiredRecord)) {
				// If the score's record is reached mutiple times, don't worry about this function 
				// being called over and over again - that won't happen because `ForceOpen(true)` 
				// calls`unregisterEvents` inside.
				ForceOpen(true);
			}
		}

		/// <summary>
		/// Checks if this gate meets its criteria for opening, by checking if this gate's
		/// associated score has reached the desired record. 
		/// </summary>
		/// <returns>If the gate can be opened returns <c>true</c>; otherwise <c>false</c>.</returns>
		protected override bool canOpenInner() {
			Score score = LevelUp.GetInstance().GetScore(AssociatedScoreId);
			if (score == null) {
				SoomlaUtils.LogError(TAG, "(canOpenInner) couldn't find score with scoreId: " + AssociatedScoreId);
				return false;
			}

			return score.HasRecordReached(DesiredRecord);
		}

		/// <summary>
		/// Opens this gate if it can be opened (its criteria has been met).
		/// </summary>
		/// <returns>If the gate has been opened returns <c>true</c>; otherwise <c>false</c>.</returns>
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

