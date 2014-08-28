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
	public class SocialStoryGate : SocialActionGate
	{
		private const string TAG = "SOOMLA SocialStoryGate";

		public string Message;
		public string Name;
		public string Caption;
		public string Link;
		public string ImgUrl;

		public SocialStoryGate(string id, Provider provider, string message, string name, string caption, string link, string imgUrl)
			: base(id, provider)
		{
			Message = message;
			Name = name;
			Caption = caption;
			Link = link;
			ImgUrl = imgUrl;
		}
		
		/// <summary>
		/// see parent.
		/// </summary>
		public SocialStoryGate(JSONObject jsonGate)
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

				SoomlaProfile.UpdateStory(Provider,
				                          Message,
				                          Name,
				                          Caption,
				                          Link,
				                          ImgUrl,
				                          this.ID);

				return true;
			}
			
			return false;
		}
	}
}

