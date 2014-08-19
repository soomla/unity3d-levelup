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
	/// A specific type of <c>Mission</c> that has an associated virtual item and a desired  
	/// balance. The mission is completed once the item's balance reaches the desired balance.
	/// </summary>
	public class BalanceMission : Mission
	{

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="id">Mission ID.</param>
		/// <param name="name">Mission name.</param>
		/// <param name="associatedItemId">ID of the item who's balance is examined.</param>
		/// <param name="desiredBalance">Desired balance.</param>
		public BalanceMission(string id, string name, string associatedItemId, int desiredBalance)
			: base(id, name, typeof(BalanceGate), new object[] { associatedItemId, desiredBalance })
		{
		}

		/// <summary>
		/// Constructor for mission with rewards.
		/// </summary>
		/// <param name="id">Mission ID.</param>
		/// <param name="name">Mission name.</param>
		/// <param name="rewards">Rewards.</param>
		/// <param name="associatedItemId">ID of the item who's balance is examined.</param>
		/// <param name="desiredBalance">Desired balance.</param>
		public BalanceMission(string id, string name, List<Reward> rewards, string associatedItemId, int desiredBalance)
			: base(id, name, rewards, typeof(BalanceGate), new object[] { associatedItemId, desiredBalance })
		{
		}

		/// <summary>
		/// Constructor.
		/// Generates an instance of <c>BalanceMission</c> from the given JSONObject.
		/// </summary>
		/// <param name="jsonMission">JSON mission.</param>
		public BalanceMission(JSONObject jsonMission)
			: base(jsonMission)
		{
		}

	}
}

