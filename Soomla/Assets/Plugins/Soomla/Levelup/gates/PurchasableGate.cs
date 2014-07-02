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
	public class PurchasableGate : Gate
	{
		private const string TAG = "SOOMLA PurchasableGate";

		public string AssociatedItemId;

		public PurchasableGate(string gateId, string associatedItemId)
			: base(gateId)
		{
			AssociatedItemId = associatedItemId;
		}
		
		/// <summary>
		/// see parent.
		/// </summary>
		public PurchasableGate(JSONObject jsonGate)
			: base(jsonGate)
		{
			this.AssociatedItemId = jsonGate[JSONConsts.SOOM_ASSOCITEMID].str;
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <returns>see parent</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();
			obj.AddField(JSONConsts.SOOM_ASSOCITEMID, this.AssociatedItemId);

			return obj;
		}

		// TODO: register for events and handle them

		public override bool CanOpen() {
			return true;
		}

		protected override bool TryOpenInner() {
			try {
				PurchasableVirtualItem pvi = (PurchasableVirtualItem) StoreInfo.GetItemByItemId(AssociatedItemId);
				PurchaseWithMarket ptype = (PurchaseWithMarket) pvi.PurchaseType;
				SoomlaStore.BuyMarketItem(ptype.MarketItem.ProductId, GateId);
				ForceOpen(true);
				return true;
			} catch (VirtualItemNotFoundException e) {
				SoomlaUtils.LogError(TAG, "The item needed for purchase doesn't exist. itemId: " +
				                     AssociatedItemId);
			} catch (InvalidCastException e) {
				SoomlaUtils.LogError(TAG, "The associated item is not a purchasable item. itemId: " +
				                     AssociatedItemId);
			}

			return false;
		}
	}
}

