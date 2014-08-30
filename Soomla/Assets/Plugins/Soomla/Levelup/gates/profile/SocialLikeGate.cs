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
using Soomla.Profile;

namespace Soomla.Levelup
{
	/// <summary>
	/// NOTE: Social <c>Gate</c>s require the user to perform a specific social action in
	/// order to unlock the <c>Gate</c>. Currently, the social provider that's available 
	/// is Facebook, so the <c>Gates</c>s are FB-oriented. In the future, more social 
	/// providers will be added.
	/// 
	/// A specific type of <c>Gate</c> that has an associated page name.
	/// The <c>Gate</c> is unlocked once the player "Likes" the associated page.  
	/// </summary>
	public class SocialLikeGate : SocialActionGate
	{
		private const string TAG = "SOOMLA SocialLikeGate"; // used in error log messages. 

		/// <summary>
		/// The name of the page that needs to be liked. 
		/// </summary>
		public string PageName;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="id"><c>Gate</c> name.</param>
		/// <param name="provider">Social provider.</param>
		/// <param name="pageName">Name of the page to "Like" in order to unlock this <c>Gate</c>.</param>
		public SocialLikeGate(string id, Provider provider, string pageName)
			: base(id, provider)
		{
			PageName = pageName;
		}
		
		/// <summary>
		/// Constructor.
		/// Generates an instance of <c>SocialLikeGate</c> from the given JSONObject.
		/// </summary>
		/// <param name="jsonGate">Json gate.</param>
		public SocialLikeGate(JSONObject jsonGate)
			: base(jsonGate)
		{
			// TODO: implement this when needed. It's irrelevant now.
		}
		
		/// <summary>
		/// Converts this <c>SocialLikeGate</c> to a JSONObject.
		/// </summary>
		/// <returns>The JSON object.</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();

			// TODO: implement this when needed. It's irrelevant now.

			return obj;
		}

		/// <summary>
		/// Opens this <c>Gate</c> by "liking" the associated page.
		/// </summary>
		/// <returns>If the page was successfully "liked" returns <c>true</c>; otherwise 
		/// <c>false</c>.</returns>
		protected override bool openInner() {
			if (CanOpen()) {

				SoomlaProfile.Like(Provider, PageName);

				return true;
			}
			
			return false;
		}
	}
}

