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
	public class VirtualItemScore : Score
	{
//		private static string TAG = "SOOMLA VirtualItemScore";
		public string AssociatedItemId;

		public VirtualItemScore(string scoreId, string name, string associatedItemId)
			: base(scoreId, name)
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


		// TODO: this function cannot be run b/c there won't always be a connection with Store module. maybe move this whole object to Store.
//		protected override void performSaveActions() {
//			base.performSaveActions();
//			try {
//				int amount = _tempScore;
//				StoreInventory.GiveVirtualItem(AssociatedItemId, amount);
//			} catch (VirtualItemNotFoundException e) {
//				SoomlaUtils.LogError(TAG, "Couldn't find item associated with a given " +
//				                     "VirtualItemScore. itemId: " + AssociatedItemId);
//			}
//		}

		// TODO: register for events and handle them

	}
}

