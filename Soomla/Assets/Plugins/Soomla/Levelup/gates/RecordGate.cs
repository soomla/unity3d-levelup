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
	public class RecordGate : Gate
	{
		public String AssociatedItemId;
		public double DesiredRecord;

		public RecordGate(string gateId, string associatedItemId, double desiredRecord)
			: base(gateId)
		{
			AssociatedItemId = associatedItemId;
			DesiredRecord = desiredRecord;
		}
		
		/// <summary>
		/// see parent.
		/// </summary>
		public RecordGate(JSONObject jsonGate)
			: base(jsonGate)
		{
			this.AssociatedItemId = jsonGate[JSONConsts.SOOM_ASSOCITEMID].str;
			this.DesiredRecord = jsonGate[JSONConsts.SOOM_DESIRED_RECORD].n;
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <returns>see parent</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();
			obj.AddField(JSONConsts.SOOM_ASSOCITEMID, this.AssociatedItemId);
			obj.AddField(JSONConsts.SOOM_DESIRED_RECORD, this.DesiredRecord);

			return obj;
		}

		// TODO: register for events and handle them

		public override bool CanOpen() {
			// TODO: remove THIS !
			Score score = null;
//			Score score = LevelUp.getInstance().getScore(mAssociatedScoreId); // TODO: get the associated score from LevelUp 
			if (score == null) {
				SoomlaUtils.LogError(TAG, "(canOpen) couldn't find score with scoreId: " + AssociatedScoreId);
				return false;
			}
			
			//        return score.hasTempReached(mDesiredRecord);
			return score.HasRecordReached(DesiredRecord);
		}

		public override bool TryOpenInner() {
			if (canOpen()) {
				ForceOpen(true);
				return true;
			}
			
			return false;
		}


	}
}

