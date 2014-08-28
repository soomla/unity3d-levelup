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
	public abstract class SocialActionGate : Gate
	{
		private const string TAG = "SOOMLA SocialActionGate";

		public Provider Provider;

		public SocialActionGate(string id, Provider provider)
			: base(id)
		{
			Provider = provider;
		}
		
		/// <summary>
		/// see parent.
		/// </summary>
		public SocialActionGate(JSONObject jsonGate)
			: base(jsonGate)
		{
			Provider = Provider.fromString(jsonGate[LUJSONConsts.LU_SOCIAL_PROVIDER].str);
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <returns>see parent</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();
			obj.AddField(LUJSONConsts.LU_SOCIAL_PROVIDER, Provider.ToString());

			return obj;
		}

		protected override bool canOpenInner() {
			return true;
		}

		protected void onSocialActionFinished(Provider provider, SocialActionType socialActionType, string payload) {
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

