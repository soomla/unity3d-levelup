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
using Soomla.Store;

namespace Soomla.Levelup
{
	public class VirtualItemScore : Score
	{
		private static string TAG = "SOOMLA VirtualItemScore";
		public string AssociatedItemId;

		public VirtualItemScore(string scoreId, string associatedItemId)
			: base(scoreId)
		{
			AssociatedItemId = associatedItemId;
		}

		public VirtualItemScore(string scoreId, string name, bool higherBetter, string associatedItemId)
			: base(scoreId, name, higherBetter)
		{
			AssociatedItemId = associatedItemId;
		}
		
		/// <summary>
		/// see parent.
		/// </summary>
		public VirtualItemScore(JSONObject jsonScore)
			: base(jsonScore)
		{
			AssociatedItemId = jsonScore[JSONConsts.SOOM_ASSOCITEMID].str;
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <returns>see parent</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();
			obj.AddField(JSONConsts.SOOM_ASSOCITEMID, AssociatedItemId);

			return obj;
		}

		protected override void performSaveActions() {
			base.performSaveActions();
			try {
				int amount = (int)_tempScore;
				StoreInventory.GiveItem(AssociatedItemId, amount);
			} catch (VirtualItemNotFoundException e) {
				SoomlaUtils.LogError(TAG, "Couldn't find item associated with a given " +
				                     "VirtualItemScore. itemId: " + AssociatedItemId);
				SoomlaUtils.LogError(TAG, e.Message);
			}
		}

	}
}

