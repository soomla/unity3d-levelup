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
	public class SocialStatusGate : SocialActionGate
	{
		private const string TAG = "SOOMLA SocialStatusGate";

		public string Status;

		public SocialStatusGate(string id, Provider provider, string status)
			: base(id, provider)
		{
			Status = status;
		}
		
		/// <summary>
		/// see parent.
		/// </summary>
		public SocialStatusGate(JSONObject jsonGate)
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

//				if (RequiredSocialActionType == SocialActionType.UPLOAD_IMAGE) {
//					SoomlaProfile.UploadImage(Provider.FACEBOOK,
//					                          (Texture2D)SocialActionParams["texture"],
//					                          SocialActionParams["fileName"].ToString(),
//					                          SocialActionParams["message"].ToString(),
//					                          this.ID);
//				} else if (RequiredSocialActionType == SocialActionType.UPDATE_STATUS) {
				SoomlaProfile.UpdateStatus(Provider, Status, this.ID);
//				} else if (RequiredSocialActionType == SocialActionType.UPDATE_STORY) {
//					SoomlaProfile.UpdateStory(Provider.FACEBOOK,
//					                          SocialActionParams["message"].ToString(),
//					                          SocialActionParams["name"].ToString(),
//					                          SocialActionParams["caption"].ToString(),
//					                          SocialActionParams["link"].ToString(),
//					                          SocialActionParams["picture"].ToString(),
//					                          this.ID);
//				}

				return true;
			}
			
			return false;
		}
	}
}

