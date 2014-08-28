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
using UnityEngine;

namespace Soomla.Levelup
{
	public class SocialUploadGate : SocialActionGate
	{
		private const string TAG = "SOOMLA SocialUploadGate";

		public string FileName;
		public string Message;
		public Texture2D ImgTexture;

		public SocialUploadGate(string id, Provider provider, string fileName, string message, Texture2D texture)
			: base(id, provider)
		{
			FileName = fileName;
			Message = message;
			ImgTexture = texture;
		}
		
		/// <summary>
		/// see parent.
		/// </summary>
		public SocialUploadGate(JSONObject jsonGate)
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

				SoomlaProfile.UploadImage(Provider.FACEBOOK,
				                          ImgTexture,
				                          FileName,
				                          Message,
				                          this.ID);

				return true;
			}
			
			return false;
		}
	}
}

