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
	/// A specific type of <c>Mission</c> that has an associated score and a desired
	/// record. The <c>Mission</c> is complete once the player achieves the desired record 
	/// for the given score.
	/// </summary>
	public class RecordMission : Mission
	{

		/// <summary>
		/// Constructor. 
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="name">Name.</param>
		/// <param name="associatedScoreId">ID of the score examined.</param>
		/// <param name="desiredRecord">Desired record.</param>
		public RecordMission(string id, string name, string associatedScoreId, double desiredRecord)
			: base(id, name, typeof(RecordGate), new object[] { associatedScoreId, desiredRecord })
		{
		}

		/// <summary>
		/// Constructor for <c>Mission</c> with rewards. 
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="name">Name.</param>
		/// <param name="rewards">Rewards.</param>
		/// <param name="associatedScoreId">ID of the score examined.</param>
		/// <param name="desiredRecord">Desired record.</param>
		public RecordMission(string id, string name, List<Reward> rewards, string associatedScoreId, double desiredRecord)
			: base(id, name, rewards, typeof(RecordGate), new object[] { associatedScoreId, desiredRecord })
		{
		}

		/// <summary>
		/// Constructor.
		/// Generates an instance of <c>RecordMission</c> from the given JSONObject. 
		/// </summary>
		/// <param name="jsonMission">JSON mission.</param>
		public RecordMission(JSONObject jsonMission)
			: base(jsonMission)
		{
			// TODO: implement this when needed. It's irrelevant now.
		}

		/// <summary>
		/// Converts this <c>RecordMission</c> to a JSONObject.
		/// </summary>
		/// <returns>The JSON object.</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();
			
			// TODO: implement this when needed. It's irrelevant now.
			
			return obj;
		}
	}
}

