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
using Soomla.Profile;

namespace Soomla.Levelup
{
	/// <summary>
	/// NOTE: Social <c>Mission</c>s require the user to perform a specific social action in
	/// order to receive a <c>Reward</c>. Currently, the social provider that's available 
	/// is Facebook, so the <c>Mission</c>s are FB-oriented. In the future, more social 
	/// providers will be added. 
	/// 
	/// A specific type of <c>Mission</c> that has an associated page name. The <c>Mission</c> 
	/// is complete once the player "Likes" the page.  
	/// </summary>
	public class SocialLikeMission : Mission
	{

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="id"><c>Mission</c> ID.</param>
		/// <param name="name"><c>Mission</c> name.</param>
		/// <param name="provider">Social provider.</param>
		/// <param name="pageName">Name of the page to "Like" in order to complete this <c>Mission</c>.</param>
		public SocialLikeMission(string id, string name, Provider provider, string pageName)
			: base(id, name, typeof(SocialLikeGate), new object[] { provider, pageName })
		{
		}

		/// <summary>
		/// Constructor with rewards.
		/// </summary>
		/// <param name="id"><c>Mission</c> ID.</param>
		/// <param name="name"><c>Mission</c> name.</param>
		/// <param name="rewards">Rewards.</param>
		/// <param name="provider">Social provider.</param>
		/// <param name="pageName">Name of the page to "Like".</param>
		public SocialLikeMission(string id, string name, List<Reward> rewards, Provider provider, string pageName)
			: base(id, name, rewards, typeof(SocialLikeGate), new object[] { provider, pageName })
		{
		}

		/// <summary>
		/// Constructor.
		/// Generates an instance of <c>SocialLikeMission</c> from the given JSONObject.
		/// </summary>
		/// <param name="jsonMission">Json mission.</param>
		public SocialLikeMission(JSONObject jsonMission)
			: base(jsonMission)
		{
			// TODO: implement this when needed. It's irrelevant now.
		}

		/// <summary>
		/// Converts this <c>SocialLikeMission</c> to a JSONObject.
		/// </summary>
		/// <returns>The JSON object.</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();

			// TODO: implement this when needed. It's irrelevant now.

			return obj;
		}

	}
}

