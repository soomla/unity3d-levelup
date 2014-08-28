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
	/// <summary>
	/// A specific type of <c>Gate</c> that has an associated virtual item and a desired
	/// balance. The <c>Gate</c> opens once the item's balance reaches the desired balance.
	/// </summary>
	public class BalanceGate : Gate
	{
		/// <summary>
		/// Used in log error messages.
		/// </summary>
		private const string TAG = "SOOMLA BalanceGate";

		/// <summary>
		/// ID of the item whose balance is examined.
		/// </summary>
		public string AssociatedItemId;

		/// <summary>
		/// The desired balance of the associated item.
		/// </summary>
		public int DesiredBalance;

		/// <summary>
		/// Constructor. 
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="associatedItemId">Associated item ID.</param>
		/// <param name="desiredBalance">Desired balance.</param>
		public BalanceGate(string id, string associatedItemId, int desiredBalance)
			: base(id)
		{
			AssociatedItemId = associatedItemId;
			DesiredBalance = desiredBalance;
		}
		
		/// <summary>
		/// Constructor. 
		/// </summary>
		/// <param name="jsonGate">JSON gate.</param>
		public BalanceGate(JSONObject jsonGate)
			: base(jsonGate)
		{
			this.AssociatedItemId = jsonGate[LUJSONConsts.LU_ASSOCITEMID].str;
			this.DesiredBalance = Convert.ToInt32(jsonGate[LUJSONConsts.LU_DESIRED_BALANCE].n);
		}
		
		/// <summary>
		/// Converts this gate to a JSONObject.
		/// </summary>
		/// <returns>The JSON object.</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();
			obj.AddField(LUJSONConsts.LU_ASSOCITEMID, this.AssociatedItemId);
			obj.AddField(LUJSONConsts.LU_DESIRED_BALANCE, this.DesiredBalance);

			return obj;
		}

		/// <summary>
		/// Opens this <c>Gate</c> if the currency-balance changed event causes the <c>Gate</c>'s 
		/// criteria to be met. 
		/// </summary>
		/// <param name="virtualCurrency">Virtual currency whose balance changed.</param>
		/// <param name="balance">New balance.</param>
		/// <param name="amountAdded">Amount added.</param>
		public void onCurrencyBalanceChanged(VirtualCurrency virtualCurrency, int balance, int amountAdded) {
			checkItemIdBalance (virtualCurrency.ItemId, balance);
		}
		
		/// <summary>
		/// Opens this <c>Gate</c> if the good-balance changed event causes the <c>Gate</c>'s  
		/// criteria to be met. 
		/// </summary>
		/// <param name="good">Virtual good whose balance has changed.</param>
		/// <param name="balance">New balance.</param>
		/// <param name="amountAdded">Amount added.</param>
		public void onGoodBalanceChanged(VirtualGood good, int balance, int amountAdded) {
			checkItemIdBalance (good.ItemId, balance);
		}

		/// <summary>
		/// Checks if this <c>Gate</c> meets its criteria for opening, by checking if the
		/// item's balance has reached the desired balance.
		/// </summary>
		/// <returns>If the <c>Gate</c> meets the criteria to be opened returns <c>true</c>; 
		/// otherwise <c>false</c>.</returns>
		protected override bool canOpenInner() {
			try {
				return (StoreInventory.GetItemBalance(AssociatedItemId) >= DesiredBalance);
			} catch (VirtualItemNotFoundException e) {
				SoomlaUtils.LogError(TAG, "(canOpenInner) Couldn't find itemId. itemId: " + AssociatedItemId);
				SoomlaUtils.LogError(TAG, e.Message);
				return false;
			}
		}
		
		/// <summary>
		/// Opens the <c>Gate</c> if the criteria has been met. 
		/// </summary>
		/// <returns>If the <c>Gate</c> is opened returns <c>true</c>; otherwise <c>false</c>.</returns>
		protected override bool openInner() {
			if (CanOpen()) {
				
				// There's nothing to do here... If the DesiredBalance was reached then the gate is just open.
				
				ForceOpen(true);
				return true;
			}
			
			return false;
		}

		/// <summary>
		/// Registers relevant events: currency-balance and good-balance changed events. 
		/// </summary>
		protected override void registerEvents() {
			if (!IsOpen()) {
				StoreEvents.OnCurrencyBalanceChanged += onCurrencyBalanceChanged;
				StoreEvents.OnGoodBalanceChanged += onGoodBalanceChanged;
			}
		}

		/// <summary>
		/// Unregisters relevant events: currency-balance and good-balance changed events. 
		/// </summary>
		protected override void unregisterEvents() {
			StoreEvents.OnCurrencyBalanceChanged -= onCurrencyBalanceChanged;
			StoreEvents.OnGoodBalanceChanged -= onGoodBalanceChanged;
		}

		/// <summary>
		/// Opens this <c>Gate</c> if the given item ID is the same as the ID of the associated item 
		/// of this <c>Gate</c> AND if the given balance is greater or equal to the desired balance.
		/// </summary>
		/// <param name="itemId">Item ID to compare with associated ID.</param>
		/// <param name="balance">Balance to compare with the desired balance.</param>
		private void checkItemIdBalance(String itemId, int balance) {
			if (itemId == AssociatedItemId && balance >= DesiredBalance) {
				ForceOpen(true);
			}
		}
	}
}

