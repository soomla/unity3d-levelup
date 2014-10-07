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
	/// <summary>
	/// A specific type of <c>Score</c> that has an associated virtual item. 
	/// The score is related to the specific item ID. For example: a game that  
	/// has an "energy" virtual item can have energy points.
	/// </summary>
	public class VirtualItemScore : Score
	{
		private static string TAG = "SOOMLA VirtualItemScore"; 

		public string AssociatedItemId; // ID of the item associated with the Score. 

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="associatedItemId">Associated virtual item ID.</param>
		public VirtualItemScore(string id, string associatedItemId)
			: base(id)
		{
			AssociatedItemId = associatedItemId;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="name">Name.</param>
		/// <param name="higherBetter">If set to <c>true</c> higher is better.</param>
		/// <param name="associatedItemId">Associated virtual item ID.</param>
		public VirtualItemScore(string id, string name, bool higherBetter, string associatedItemId)
			: base(id, name, higherBetter)
		{
			AssociatedItemId = associatedItemId;
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="jsonScore">JSON score.</param>
		public VirtualItemScore(JSONObject jsonScore)
			: base(jsonScore)
		{
			AssociatedItemId = jsonScore[LUJSONConsts.LU_ASSOCITEMID].str;
		}
		
		/// <summary>
		/// Converts this <c>Score</c> to JSONObject.
		/// </summary>
		/// <returns>see parent</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();
			obj.AddField(LUJSONConsts.LU_ASSOCITEMID, AssociatedItemId);

			return obj;
		}

		/// <summary>
		/// Gives your user the temp-score amount of the associated item.
		/// </summary>
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

