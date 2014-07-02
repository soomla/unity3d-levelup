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
	public class PurchasableGate : Gate
	{
		public String AssociatedItemId;

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
			this.AssociatedItemId = jsonItem[JSONConsts.SOOM_ASSOCITEMID].str;
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

		public override boolean CanOpen() {
			return true;
		}

		public override boolean tryOpenInner() {
				// TODO: move this object to Store module. the following code will not work.

//			try {
//				PurchasableVirtualItem pvi = (PurchasableVirtualItem) StoreInfo.getVirtualItem(mAssociatedItemId);
//				PurchaseWithMarket ptype = (PurchaseWithMarket) pvi.getPurchaseType();
//				SoomlaStore.getInstance().buyWithMarket(ptype.getMarketItem(), getGateId());
//				return true;
//			} catch (VirtualItemNotFoundException e) {
//				SoomlaUtils.LogError(TAG, "The item needed for purchase doesn't exist. itemId: " +
//				                     mAssociatedItemId);
//			} catch (ClassCastException e) {
//				SoomlaUtils.LogError(TAG, "The associated item is not a purchasable item. itemId: " +
//				                     mAssociatedItemId);
//			}
//				
//				forceOpen(true);
//				return true;
//			}
			
			return false;
		}
	}
}

