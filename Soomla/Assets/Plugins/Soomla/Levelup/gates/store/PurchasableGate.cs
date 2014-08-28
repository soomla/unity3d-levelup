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
using System.Collections.Generic;

using Soomla.Store;

namespace Soomla.Levelup
{
	/// <summary>
	/// A specific type of <c>Gate</c> that has an associated market item. The <c>Gate</c> 
	/// opens once the item has been purchased. This <c>Gate</c> is useful when you want to 
	/// allow unlocking of certain <c>Level</c>s or <c>World</c>s only if they are purchased.
	/// </summary>
	public class PurchasableGate : Gate
	{
		/// <summary>
		/// Used in log error messages.
		/// </summary>
		private const string TAG = "SOOMLA PurchasableGate";

		/// <summary>
		/// ID of the item who needs to be purchased.
		/// </summary>
		public string AssociatedItemId;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="associatedItemId">Associated item ID.</param>
		public PurchasableGate(string id, string associatedItemId)
			: base(id)
		{
			AssociatedItemId = associatedItemId;
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="jsonGate">JSON gate.</param>
		public PurchasableGate(JSONObject jsonGate)
			: base(jsonGate)
		{
			this.AssociatedItemId = jsonGate[LUJSONConsts.LU_ASSOCITEMID].str;
		}
		
		/// <summary>
		/// Converts this <c>Gate</c> to JSONObject.
		/// </summary>
		/// <returns>The JSON object.</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();
			obj.AddField(LUJSONConsts.LU_ASSOCITEMID, this.AssociatedItemId);

			return obj;
		}

		/// <summary>
		/// Registers relevant events: item-purchased event.
		/// </summary>
		protected override void registerEvents() {
			if (!IsOpen()) {
				StoreEvents.OnItemPurchased += onItemPurchased;
			}
		}

		/// <summary>
		/// Unregisters relevant events: item-purchased event.
		/// </summary>
		protected override void unregisterEvents() {
			StoreEvents.OnItemPurchased -= onItemPurchased;
		}

		/// <summary>
		/// Opens this <c>Gate</c> if the item-purchased event causes the <c>Gate</c>'s criteria to be met.
		/// </summary>
		/// <param name="pvi">The item that was purchased.</param>
		/// <param name="payload">Payment ID of the item purchased.</param>
		/// @subscribe
		public void onItemPurchased(PurchasableVirtualItem pvi, string payload) {
			if (pvi.ItemId == AssociatedItemId && payload == this._id) {
				ForceOpen(true);
			}
		}

		/// <summary>
		/// Checks if this <c>Gate</c> meets its criteria for opening. For this type of <c>Gate</c>, 
		/// it is always true because at any time the user may purchase the item associated with the
		/// opening of this gate. 
		/// </summary>
		/// <returns>Always <c>true</c>.</returns>
		protected override bool canOpenInner() {
			return true;
		}

		/// <summary>
		/// Opens this <c>Gate</c> by buying its associated item.
		/// </summary>
		/// <returns>If purchase was successfully made returns <c>true</c>; otherwise 
		/// <c>false</c>.</returns>
		protected override bool openInner() {
			try {
				StoreInventory.BuyItem(AssociatedItemId, this._id);
				return true;
			} catch (VirtualItemNotFoundException e) {
				SoomlaUtils.LogError(TAG, "The item needed for purchase doesn't exist. itemId: " +
				                     AssociatedItemId);
				SoomlaUtils.LogError(TAG, e.Message);
			} catch (InsufficientFundsException e) {
				SoomlaUtils.LogError(TAG, "There's not enough funds to purchase this item. itemId: " +
				                     AssociatedItemId);
				SoomlaUtils.LogError(TAG, e.Message);
			}

			return false;
		}
	}
}

