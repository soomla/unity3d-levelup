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
	/// This is an abstract class that all social <c>Gate</c>s must implement.
	/// </summary>
	public abstract class SocialActionGate : Gate
	{
		private const string TAG = "SOOMLA SocialActionGate"; // used for Log error messages

		public Provider Provider; // The related social provider.

		/// <summary>
		/// Constructor. 
		/// </summary>
		/// <param name="id"><c>Gate</c> ID.</param>
		/// <param name="provider">Social provider.</param>
		public SocialActionGate(string id, Provider provider)
			: base(id)
		{
			Provider = provider;
		}
		
		/// <summary>
		/// Constructor. 
		/// Generates an instance of <c>SocialActionGate</c> from the given JSONObject. 
		/// </summary>
		/// <param name="jsonGate">Json gate.</param>
		public SocialActionGate(JSONObject jsonGate)
			: base(jsonGate)
		{
			Provider = Provider.fromString(jsonGate[LUJSONConsts.LU_SOCIAL_PROVIDER].str);
		}
		
		/// <summary>
		/// Converts this <c>SocialActionGate</c> to a JSONObject.
		/// </summary>
		/// <returns>The JSON object.</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();
			obj.AddField(LUJSONConsts.LU_SOCIAL_PROVIDER, Provider.ToString());

			return obj;
		}

		/// <summary>
		/// Checks if this <c>Gate</c> meets its criteria for opening. For <c>SocialActionGate</c>, 
		/// it is always true because at any time the user may perform a social action to open the gate.
		/// </summary>
		/// <returns>Always <c>true</c>.</returns>
		protected override bool canOpenInner() {
			return true;
		}

		/// <summary>
		/// Opens this <c>Gate</c> if the social action that was finished causes the 
		/// <c>Gate</c>'s criteria to be met.
		/// </summary>
		/// <param name="provider">Social provider related to the action that was finished.</param>
		/// <param name="socialActionType">The type of the social action that was finished.</param>
		/// <param name="payload">Payload to compare with this <c>Gate</c>'s ID.</param>
		protected void onSocialActionFinished(Provider provider, SocialActionType socialActionType, string payload) {
			if (payload == this.ID) {
				ForceOpen(true);
			}
		}

		/// <summary>
		/// Registers relevant events: social-action-finished event.
		/// </summary>
		protected override void registerEvents() {
			if (!IsOpen()) {
				ProfileEvents.OnSocialActionFinished += onSocialActionFinished;
			}
		}

		/// <summary>
		/// Unregisters relevant events: social-action-finished event.
		/// </summary>
		protected override void unregisterEvents() {
			ProfileEvents.OnSocialActionFinished -= onSocialActionFinished;
		}

	}
}

