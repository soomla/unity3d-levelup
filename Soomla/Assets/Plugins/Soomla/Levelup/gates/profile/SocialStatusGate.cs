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
	/// order to open the <c>Gate</c>. Currently, the social provider that's available 
	/// is Facebook, so the <c>Gates</c>s are FB-oriented. In the future, more social 
	/// providers will be added.
	/// 
	/// A specific type of <c>Gate</c> that has an associated status. The <c>Gate</c> 
	/// is opened once the player posts the status.   
	/// </summary>
	public class SocialStatusGate : SocialActionGate
	{
		private const string TAG = "SOOMLA SocialStatusGate";

		public string Status; // the status to post in order to open this Gate.

		/// <summary>
		/// Constructor. 
		/// </summary>
		/// <param name="id"><c>Gate</c> ID.</param>
		/// <param name="provider">Social provider.</param>
		/// <param name="status">Status to post in order to open this <c>Gate</c>.</param>
		public SocialStatusGate(string id, Provider provider, string status)
			: base(id, provider)
		{
			Status = status;
		}
		
		/// <summary>
		/// Constructor. 
		/// Generates an instance of <c>SocialStatusGate</c> from the given JSONObject.
		/// </summary>
		/// <param name="jsonGate">Json gate.</param>
		public SocialStatusGate(JSONObject jsonGate)
			: base(jsonGate)
		{
			this.Status = jsonGate[LUJSONConsts.LU_SOCIAL_STATUS].str;
		}
		
		/// <summary>
		/// Converts this <c>SocialStatusGate</c> to a JSONObject.
		/// </summary>
		/// <returns>The JSON object.</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();
			obj.AddField(LUJSONConsts.LU_SOCIAL_STATUS, this.Status);

			return obj;
		}

		/// <summary>
		/// Opens this <c>Gate</c> by posting the associated status.
		/// </summary>
		/// <returns>If the status was successfully posted returns <c>true</c>; otherwise 
		/// <c>false</c>.</returns>
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

