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
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Levelup
{
	public class ScheduleGate : Gate
	{
		private const string TAG = "SOOMLA ScheduleGate";

		public Schedule Schedule;

		public ScheduleGate(string id, Schedule schedule)
			: base(id)
		{
			Schedule = schedule;
		}
		
		/// <summary>
		/// see parent.
		/// </summary>
		public ScheduleGate(JSONObject jsonGate)
			: base(jsonGate)
		{
			this.Schedule = new Schedule(jsonGate[JSONConsts.SOOM_SCHEDULE]);
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <returns>see parent</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();
			obj.AddField(JSONConsts.SOOM_SCHEDULE, Schedule.toJSONObject());

			return obj;
		}

		protected override void registerEvents() {
			// Not listening to any events
		}

		protected override void unregisterEvents() {
			// Not listening to any events
		}

		protected override bool canOpenInner() {
			// gates don't have activation times. they can only be activated once. 
			// We kind of ignoring the activation limit of Schedule here.
			return Schedule.Approve(GateStorage.IsOpen(this) ? 1 : 0);
		}

		protected override bool openInner() {
			if (CanOpen()) {

				// There's nothing to do here... If the DesiredRecord was reached then the gate is just open.

				ForceOpen(true);
				return true;
			}
			
			return false;
		}


	}
}

