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
	/// A specific type of <c>Gate</c> that has an associated story. The <c>Gate</c> 
	/// is opened once the player posts the story.   
	/// </summary>
	public class SocialStoryGate : SocialActionGate
	{
		private const string TAG = "SOOMLA SocialStoryGate";

		/** Components of a social Story: **/
		public string Message; 
		public string StoreName;
		public string Caption;
		public string Link;
		public string ImgUrl;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="id"><c>Gate</c> ID.</param>
		/// <param name="provider">Social provider.</param>
		/// <param name="message">Message for the story.</param>
		/// <param name="name">Story name.</param>
		/// <param name="caption">Caption for the story image.</param>
		/// <param name="link">Link for the story.</param>
		/// <param name="imgUrl">Image URL for the story.</param>
		public SocialStoryGate(string id, Provider provider, string message, string storyName, string caption, string link, string imgUrl)
			: base(id, provider)
		{
			Message = message;
			StoreName = storyName;
			Caption = caption;
			Link = link;
			ImgUrl = imgUrl;
		}
		
		/// <summary>
		/// Constructor.
		/// Generates an instance of <c>SocialStoryGate</c> from the given JSONObject.
		/// </summary>
		/// <param name="jsonGate">Json gate.</param>
		public SocialStoryGate(JSONObject jsonGate)
			: base(jsonGate)
		{
			// TODO: implement this when needed. It's irrelevant now.
		}
		
		//// <summary>
		/// Converts this <c>SocialStoryGate</c> to a JSONObject.
		/// </summary>
		/// <returns>The JSON object.</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();

			// TODO: implement this when needed. It's irrelevant now.

			return obj;
		}

		/// <summary>
		/// Opens this <c>Gate</c> by posting the associated story.
		/// </summary>
		/// <returns>If the story was successfully posted returns <c>true</c>; otherwise 
		/// <c>false</c>.</returns>
		protected override bool openInner() {
			if (CanOpen()) {

				SoomlaProfile.UpdateStory(Provider,
				                          Message,
				                          StoreName,
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

