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
/// limitations under the License.

using System;

namespace Soomla.Levelup {

	/// <summary>
	/// This class contains all string names of the keys/vals in the JSON being parsed all around the SDK.
	/// </summary>
	public static class LUJSONConsts
	{
		/** LevelUp **/
		public const string LU_MAIN_WORLD        = "mainWorld";

		/** Score **/
		public const string LU_SCORES            = "scores";
		public const string LU_SCORE_STARTVAL    = "startValue";
		public const string LU_SCORE_HIGHBETTER  = "higherBetter";
		public const string LU_SCORE_RANGE       = "range";
		public const string LU_SCORE_RANGE_LOW   = "low";
		public const string LU_SCORE_RANGE_HIGH  = "high";

		/** Gate **/
		public const string LU_GATE              = "gate";
		public const string LU_GATES             = "gates";

		/** Challenge **/
		public const string LU_CHALLENGES        = "challenges";
		public const string LU_MISSIONS          = "missions";

		/** World **/
		public const string LU_WORLDS            = "worlds";

	}
}

