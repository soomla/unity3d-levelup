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
/// limitations under the License.

using System;
using System.Collections.Generic;
using UnityEngine;
using Soomla;

namespace Soomla.Store.Example {

	/// <summary>
	/// This class contains currenciy and goods balances. 
	/// We keep these balances so we won't have to make too many calls to the native (Android/iOS) code.
	/// </summary>
	public static class ExampleLocalStoreInfo {
		
		// In this example we have a single currency so we can just save its balance. 
		// If you have more than one currency then you'll have to save a dictionary here.
		public static int CurrencyBalance = 0;
		
		public static Dictionary<string, int> GoodsBalances = new Dictionary<string, int>();
		public static List<VirtualCurrency> VirtualCurrencies = null;
		public static List<VirtualGood> VirtualGoods = null;
		public static List<VirtualCurrencyPack> VirtualCurrencyPacks = null;
		
		public static void UpdateBalances() {
			if (VirtualCurrencies.Count > 0) {
				CurrencyBalance = StoreInventory.GetItemBalance(VirtualCurrencies[0].ItemId);
			}
			foreach(VirtualGood vg in VirtualGoods){
				GoodsBalances[vg.ItemId] = StoreInventory.GetItemBalance(vg.ItemId);
			}
		}
		
		public static void Init() {
			VirtualCurrencies = StoreInfo.GetVirtualCurrencies();
			VirtualGoods = StoreInfo.GetVirtualGoods();
			VirtualCurrencyPacks = StoreInfo.GetVirtualCurrencyPacks();	
			UpdateBalances();
		}
	}
}

