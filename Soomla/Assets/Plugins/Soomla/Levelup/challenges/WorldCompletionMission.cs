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
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Levelup
{
	/// <summary>
	/// A specific type of <c>Mission</c> that has an associated world. The mission 
	/// is completed once the world has been completed.
	/// </summary>
	public class WorldCompletionMission : Mission
	{
		/// <summary>
		/// Constructor. 
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="name">Name.</param>
		/// <param name="associatedWorldId">World that needs to be completed.</param>
		public WorldCompletionMission(string id, string name, string associatedWorldId)
			: base(id, name, typeof(WorldCompletionGate), new object[] { associatedWorldId })
		{
		}

		/// <summary>
		/// Constructor. 
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="name">Name.</param>
		/// <param name="rewards">Rewards for this mission.</param>
		/// <param name="associatedWorldId">World that needs to be completed.</param>
		public WorldCompletionMission(string id, string name, List<Reward> rewards, string associatedWorldId)
			: base(id, name, rewards, typeof(WorldCompletionGate), new object[] { associatedWorldId })
		{
		}

		/// <summary>
		/// Constructor.
		/// Generates a <c>WorldCompletionMission</c> from the given JSONObject.
		/// </summary>
		/// <param name="jsonMission">JSON mission.</param>
		public WorldCompletionMission(JSONObject jsonMission)
			: base(jsonMission)
		{
		}
	}
}

