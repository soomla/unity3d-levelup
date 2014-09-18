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
	/// <summary>
	/// NOTE: Social <c>Gate</c>s require the user to perform a specific social action in
	/// order to open the <c>Gate</c>. Currently, the social provider that's available 
	/// is Facebook, so the <c>Gates</c>s are FB-oriented. In the future, more social 
	/// providers will be added.
	/// 
	/// A specific type of <c>Gate</c> that has an associated image. The <c>Gate</c> 
	/// is opened once the player uploads the image.   
	/// </summary>
	public class SocialUploadGate : SocialActionGate
	{
		private const string TAG = "SOOMLA SocialUploadGate"; 

		/** Components of a social Upload: **/
		public string FileName;
		public string Message;
		public Texture2D ImgTexture;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="id"><c>Gate</c> ID.</param>
		/// <param name="provider">Social provider.</param>
		/// <param name="fileName">Name of file to upload.</param>
		/// <param name="message">Message.</param>
		/// <param name="texture">Texture.</param>
		public SocialUploadGate(string id, Provider provider, string fileName, string message, Texture2D texture)
			: base(id, provider)
		{
			FileName = fileName;
			Message = message;
			ImgTexture = texture;
		}
		
		/// <summary>
		/// Constructor.
		/// Generates an instance of <c>SocialUploadGate</c> from the given JSONObject. 
		/// </summary>
		/// <param name="jsonGate">Json gate.</param>
		public SocialUploadGate(JSONObject jsonGate)
			: base(jsonGate)
		{
			// TODO: implement this when needed. It's irrelevant now.
		}
		
		/// <summary>
		/// Converts this <c>SocialUploadGate</c> to a JSONObject.
		/// </summary>
		/// <returns>The JSON object.</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();

			// TODO: implement this when needed. It's irrelevant now.

			return obj;
		}

		/// <summary>
		/// Opens this <c>Gate</c> by uploading the associated image.
		/// </summary>
		/// <returns>If the image was successfully uploaded returns <c>true</c>; otherwise 
		/// <c>false</c>.</returns>
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

