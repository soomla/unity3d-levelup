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
using System.Collections.Generic;

using Soomla.Store;
using Soomla.Profile;

namespace Soomla.Levelup
{
	public class SocialStoryMission : Mission
	{

		public SocialStoryMission(string id, string name, Provider provider, string message, string storyName, string caption, string link, string imgUrl)
			: base(id, name, typeof(SocialStoryGate), new object[] { provider, message, storyName, caption, link, imgUrl })
		{
		}
		
		public SocialStoryMission(string id, string name, List<Reward> rewards, Provider provider, string message, 
		                          string storyName, string caption, string link, string imgUrl)
			: base(id, name, rewards, typeof(SocialStoryGate), new object[] { provider, message, storyName, caption, link, imgUrl })
		{
		}
		
		public SocialStoryMission(JSONObject jsonMission)
			: base(jsonMission)
		{
			// TODO: implement this when needed. It's irrelevant now.
		}

		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();

			// TODO: implement this when needed. It's irrelevant now.

			return obj;
		}

	}
}

