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
	public class BalanceMission : Mission
	{
		public string AssociatedItemId;
		public int DesiredBalance;

		public BalanceMission(string id, string name, string associatedItemId, int desiredBalance)
			: base(id, name)
		{
			AssociatedItemId = associatedItemId;
			DesiredBalance = desiredBalance;
		}

		public BalanceMission(string id, string name, List<Reward> rewards, string associatedItemId, int desiredBalance)
			: base(id, name, rewards)
		{
			AssociatedItemId = associatedItemId;
			DesiredBalance = desiredBalance;
		}
		
		/// <summary>
		/// see parent.
		/// </summary>
		public BalanceMission(JSONObject jsonMission)
			: base(jsonMission)
		{
			this.AssociatedItemId = jsonMission[JSONConsts.SOOM_ASSOCITEMID].str;
			this.DesiredBalance = Convert.ToInt32(jsonMission[JSONConsts.SOOM_DESIRED_BALANCE].n);
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

		protected override void registerEvents() {
			StoreEvents.OnCurrencyBalanceChanged += onCurrencyBalanceChanged;
			StoreEvents.OnGoodBalanceChanged += onGoodBalanceChanged;
		}

		protected override void unregisterEvents() {
			StoreEvents.OnCurrencyBalanceChanged -= onCurrencyBalanceChanged;
			StoreEvents.OnGoodBalanceChanged -= onGoodBalanceChanged;
		}
		
		private void checkItemIdBalance(String itemId, int balance) {
			if (itemId.Equals(AssociatedItemId) && balance >= DesiredBalance) {
				SetCompleted(true);
			}
		}
	}
}

