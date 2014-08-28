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
	public class SocialLikeGate : SocialActionGate
	{
		private const string TAG = "SOOMLA SocialLikeGate";

		public string PageName;

		public SocialLikeGate(string id, Provider provider, string pageName)
			: base(id, provider)
		{
			PageName = pageName;
		}
		
		/// <summary>
		/// see parent.
		/// </summary>
		public SocialLikeGate(JSONObject jsonGate)
			: base(jsonGate)
		{
			// TODO: implement this when needed. It's irrelevant now.
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <returns>see parent</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();

			// TODO: implement this when needed. It's irrelevant now.

			return obj;
		}

		protected override bool openInner() {
			if (CanOpen()) {

				SoomlaProfile.Like(Provider, PageName);

				return true;
			}
			
			return false;
		}
	}
}

