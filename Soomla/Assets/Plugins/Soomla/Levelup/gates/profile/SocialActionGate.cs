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
using System.Collections.Generic;
using UnityEngine;

namespace Soomla.Levelup
{
	public class SocialActionGate : Gate
	{
		private const string TAG = "SOOMLA SocialActionGate";

		public SocialActionType RequiredSocialActionType;
		Dictionary<string, object> SocialActionParams;

		public SocialActionGate(string id, SocialActionType socialActionType, Dictionary<string, object> socialActionParams)
			: base(id)
		{
			RequiredSocialActionType = socialActionType;
			SocialActionParams = socialActionParams;
		}
		
		/// <summary>
		/// see parent.
		/// </summary>
		public SocialActionGate(JSONObject jsonGate)
			: base(jsonGate)
		{
			RequiredSocialActionType = SocialActionType.fromString(jsonGate[LUJSONConsts.LU_SOCIAL_ACTION_TYPE].str);
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <returns>see parent</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();
			obj.AddField(LUJSONConsts.LU_SOCIAL_ACTION_TYPE, RequiredSocialActionType.ToString());

			return obj;
		}

		protected override bool canOpenInner() {
			return true;
		}

		protected override bool openInner() {
			if (CanOpen()) {

				if (RequiredSocialActionType == SocialActionType.UPLOAD_IMAGE) {
					SoomlaProfile.UploadImage(Provider.FACEBOOK,
					                          (Texture2D)SocialActionParams["texture"],
					                          SocialActionParams["fileName"].ToString(),
					                          SocialActionParams["message"].ToString(),
					                          this.ID);
				} else if (RequiredSocialActionType == SocialActionType.UPDATE_STATUS) {
					SoomlaProfile.UpdateStatus(Provider.FACEBOOK,
					                           SocialActionParams["status"].ToString(),
					                           this.ID);
				} else if (RequiredSocialActionType == SocialActionType.UPDATE_STORY) {
					SoomlaProfile.UpdateStory(Provider.FACEBOOK,
					                          SocialActionParams["message"].ToString(),
					                          SocialActionParams["name"].ToString(),
					                          SocialActionParams["caption"].ToString(),
					                          SocialActionParams["link"].ToString(),
					                          SocialActionParams["picture"].ToString(),
					                          this.ID);
				}

				return true;
			}
			
			return false;
		}

		public void onSocialActionFinished(Provider provider, SocialActionType socialActionType, string payload) {
			if (payload == this.ID) {
				ForceOpen(true);
			}
		}

		protected override void registerEvents() {
			if (!IsOpen()) {
				ProfileEvents.OnSocialActionFinished += onSocialActionFinished;
			}
		}

		protected override void unregisterEvents() {
			ProfileEvents.OnSocialActionFinished -= onSocialActionFinished;
		}

	}
}

