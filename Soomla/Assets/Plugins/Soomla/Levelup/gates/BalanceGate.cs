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
using Soomla.Store;

namespace Soomla.Levelup
{
	public class BalanceGate : Gate
	{
		private const string TAG = "SOOMLA BalanceGate";

		public string AssociatedItemId;
		public int DesiredBalance;

		public BalanceGate(string gateId, string associatedItemId, int desiredBalance)
			: base(gateId)
		{
			AssociatedItemId = associatedItemId;
			DesiredBalance = desiredBalance;

			registerEvents();
		}
		
		/// <summary>
		/// see parent.
		/// </summary>
		public BalanceGate(JSONObject jsonGate)
			: base(jsonGate)
		{
			this.AssociatedItemId = jsonGate[JSONConsts.SOOM_ASSOCITEMID].str;
			this.DesiredBalance = Convert.ToInt32(jsonGate[JSONConsts.SOOM_DESIRED_BALANCE].n);

			registerEvents();
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

		public override bool CanOpen() {
			// check in gate storage if the gate is open
			if (GateStorage.IsOpen(this)) {
				return true;
			}

			try {
				if (StoreInventory.GetItemBalance(AssociatedItemId) < DesiredBalance) {
					return false;
				}
			} catch (VirtualItemNotFoundException e) {
				SoomlaUtils.LogError(TAG, "(canPass) Couldn't find itemId. itemId: " + AssociatedItemId);
				SoomlaUtils.LogError(TAG, e.Message);
				return false;
			}
			return true;
		}

		protected override bool TryOpenInner() {
			if (CanOpen()) {

				try {
					StoreInventory.TakeItem(AssociatedItemId, DesiredBalance);
				} catch (VirtualItemNotFoundException e) {
					SoomlaUtils.LogError(TAG, "(open) Couldn't find itemId. itemId: " + AssociatedItemId);
					SoomlaUtils.LogError(TAG, e.Message);
					return false;
				}
				
				ForceOpen(true);
				return true;
			}
			
			return false;
		}

		/// <summary>
		/// Handles a currency balance changed event.
		/// </summary>
		/// <param name="virtualCurrency">Virtual currency whose balance has changed.</param>
		/// <param name="balance">Balance of the given virtual currency.</param>
		/// <param name="amountAdded">Amount added to the balance.</param>
		/// @Subscribe
		public void onCurrencyBalanceChanged(VirtualCurrency virtualCurrency, int balance, int amountAdded) {
			checkItemIdBalance (virtualCurrency.ItemId, balance);
		}
		
		/// <summary>
		/// Handles a good balance changed event.
		/// </summary>
		/// <param name="good">Virtual good whose balance has changed.</param>
		/// <param name="balance">Balance.</param>
		/// <param name="amountAdded">Amount added.</param>
		/// @Subscribe
		public void onGoodBalanceChanged(VirtualGood good, int balance, int amountAdded) {
			checkItemIdBalance (good.ItemId, balance);
		}


		protected virtual void registerEvents() {
			if (!IsOpen()) {
				StoreEvents.OnCurrencyBalanceChanged += onCurrencyBalanceChanged;
				StoreEvents.OnGoodBalanceChanged += onGoodBalanceChanged;
			}
		}

		private void checkItemIdBalance(String itemId, int balance) {
			if (itemId.Equals(AssociatedItemId) && balance >= DesiredBalance) {
				StoreEvents.OnCurrencyBalanceChanged -= onCurrencyBalanceChanged;
				StoreEvents.OnGoodBalanceChanged -= onGoodBalanceChanged;
				// gate can open now
			}
		}
	}
}

