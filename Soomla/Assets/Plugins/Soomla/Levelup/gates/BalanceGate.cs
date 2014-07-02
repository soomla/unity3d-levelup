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
	public class BalanceGate : Gate
	{
		public string AssociatedItemId;
		public int DesiredBalance;

		public BalanceGate(string gateId, string associatedItemId, int desiredBalance)
			: base(gateId)
		{
			AssociatedItemId = associatedItemId;
			DesiredBalance = desiredBalance;
		}
		
		/// <summary>
		/// see parent.
		/// </summary>
		public BalanceGate(JSONObject jsonGate)
			: base(jsonGate)
		{
			this.AssociatedItemId = jsonItem[JSONConsts.SOOM_ASSOCITEMID].str;
			this.DesiredBalance = jsonItem[JSONConsts.SOOM_DESIRED_BALANCE].n;
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <returns>see parent</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();
			obj.AddField(JSONConsts.SOOM_ASSOCITEMID, this.AssociatedItemId);
			obj.AddField(JSONConsts.SOOM_DESIRED_BALANCE, this.DesiredBalance);

			return obj;
		}

		// TODO: register for events and handle them

		public override bool CanOpen() {
			// TODO: check in gate storage if the gate is open
//			if (GateStorage.IsOpen(this)) {
//				return true;
//			}

			// TODO: move this object to Store module. the following code will not work.
//			try {
//				if (StoreInventory.getVirtualItemBalance(mAssociatedItemId) < mDesiredBalance) {
//					return false;
//				}
//			} catch (VirtualItemNotFoundException e) {
//				SoomlaUtils.LogError(TAG, "(canPass) Couldn't find itemId. itemId: " + mAssociatedItemId);
//				return false;
//			}
			return true;
		}

		public override bool tryOpenInner() {
			if (CanOpen()) {

				// TODO: move this object to Store module. the following code will not work.

//				try {
//					StoreInventory.takeVirtualItem(mAssociatedItemId, mDesiredBalance);
//				} catch (VirtualItemNotFoundException e) {
//					SoomlaUtils.LogError(TAG, "(open) Couldn't find itemId. itemId: " + mAssociatedItemId);
//					return false;
//				}
				
				forceOpen(true);
				return true;
			}
			
			return false;
		}
	}
}

