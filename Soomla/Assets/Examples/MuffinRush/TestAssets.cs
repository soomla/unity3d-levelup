using UnityEngine;
using System.Collections;

using Soomla.Store;

namespace Soomla.Test {
public class TestAssets : IStoreAssets {

	public const string ITEM_ID_BALANCE_GATE = "item_balance_gate";
	public const string ITEM_ID_BALANCE_MISSION = "balance_mission_item_id";
	public const string ITEM_ID_BALANCE_MISSION_REWARD = "balance_mission_reward_item_id";
	public const string ITEM_ID_PURCHASE_GATE_VI = "item_purchase_gate_vi";
	public const string ITEM_ID_PURCHASE_GATE_MARKET = "item_purchase_gate_market";
	public const string ITEM_ID_VI_SCORE = "item_vi_score";
	public const string ITEM_ID_VI_REWARD = "item_vi_reward";

	public int GetVersion() {
		return 0;
	}
	
	public VirtualCurrency[] GetCurrencies() {
		return new VirtualCurrency[0];
	}
	
	public VirtualGood[] GetGoods() {
		int i = 5;
		VirtualGood[] virtualGoods = new VirtualGood[i];
		virtualGoods[--i] = new SingleUseVG("ItemBalanceGate",
		                                    "", ITEM_ID_BALANCE_GATE,
		                                    new PurchaseWithMarket(ITEM_ID_BALANCE_GATE, 1));
		virtualGoods[--i] = new SingleUseVG("ItemBalanceMission",
		                                    "", ITEM_ID_BALANCE_MISSION,
		                                    new PurchaseWithMarket(ITEM_ID_BALANCE_MISSION, 1));
		virtualGoods[--i] = new SingleUseVG("ItemBalanceMissionReward",
		                                    "", ITEM_ID_BALANCE_MISSION_REWARD,
		                                    new PurchaseWithMarket(ITEM_ID_BALANCE_MISSION_REWARD, 1));
		virtualGoods[--i] = new SingleUseVG("ItemVIScore",
		                                    "", ITEM_ID_VI_SCORE,
		                                    new PurchaseWithMarket(ITEM_ID_VI_SCORE, 1));
		virtualGoods[--i] = new SingleUseVG("ItemVIReward",
		                                    "", ITEM_ID_VI_REWARD,
		                                    new PurchaseWithMarket(ITEM_ID_VI_REWARD, 1));
		
		return virtualGoods;
	}

	public VirtualCurrencyPack[] GetCurrencyPacks() {
		return new VirtualCurrencyPack[0];
	}
	
	public VirtualCategory[] GetCategories() {
		return new VirtualCategory[0];
	}
	
	public NonConsumableItem[] GetNonConsumableItems() {
		int i = 2;
		NonConsumableItem[] nonConsumableItems = new NonConsumableItem[i];
		nonConsumableItems[--i] = new NonConsumableItem("ItemPurchaseGateWithMarket",
		                                                "", ITEM_ID_PURCHASE_GATE_VI,
		                                                new PurchaseWithMarket(ITEM_ID_PURCHASE_GATE_VI, 10));
		nonConsumableItems[--i] = new NonConsumableItem("ItemPurchaseGateWithVI",
		                                                "", ITEM_ID_PURCHASE_GATE_MARKET,
		                                                new PurchaseWithMarket(ITEM_ID_PURCHASE_GATE_MARKET, 2));
		return nonConsumableItems;
	}
}
}