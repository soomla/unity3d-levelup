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

using UnityEngine;
using System;
using System.Runtime.InteropServices;

namespace Soomla.Levelup
{
	/// <summary>
	/// <c>LevelStorage</c> for iOS.
	/// A utility class for persisting and querying the state of <c>Level</c>s.
	/// Use this class to get or set information about a <c>Level</c>, such as 
	/// the play duration, start or end time, etc.
	/// </summary>
	public class LevelStorageIOS : LevelStorage {
#if UNITY_IOS && !UNITY_EDITOR

	[DllImport ("__Internal")]
	private static extern void levelStorage_SetSlowestDurationMillis(string levelId, long duration);
	[DllImport ("__Internal")]
	private static extern long levelStorage_GetSlowestDurationMillis(string levelId);
	[DllImport ("__Internal")]
	private static extern void levelStorage_SetFastestDurationMillis(string levelId, long duration);
	[DllImport ("__Internal")]
	private static extern long levelStorage_GetFastestDurationMillis(string levelId);
	[DllImport ("__Internal")]
	private static extern int levelStorage_IncTimesStarted(string levelId);
	[DllImport ("__Internal")]
	private static extern int levelStorage_DecTimesStarted(string levelId);
	[DllImport ("__Internal")]
	private static extern int levelStorage_GetTimesStarted(string levelId);
	[DllImport ("__Internal")]
	private static extern int levelStorage_IncTimesPlayed(string levelId);
	[DllImport ("__Internal")]
	private static extern int levelStorage_DecTimesPlayed(string levelId);
	[DllImport ("__Internal")]
	private static extern int levelStorage_GetTimesPlayed(string levelId);


	protected override void _setSlowestDurationMillis(Level level, long duration) {
		levelStorage_SetSlowestDurationMillis(level.ID, duration);
	}
	
	protected override long _getSlowestDurationMillis(Level level) {
		return levelStorage_GetSlowestDurationMillis(level.ID);
	}
	
	protected override void _setFastestDurationMillis(Level level, long duration) {
		levelStorage_SetFastestDurationMillis(level.ID, duration);
	}
	
	protected override long _getFastestDurationMillis(Level level) {
		return levelStorage_GetFastestDurationMillis(level.ID);
	}
	
		
	/** Level Times Started **/
	
	protected override int _incTimesStarted(Level level) {
		return levelStorage_IncTimesStarted(level.ID);
	}
	
	protected override int _decTimesStarted(Level level) {
		return levelStorage_DecTimesStarted(level.ID);
	}
	
	protected override int _getTimesStarted(Level level) {
		return levelStorage_GetTimesStarted(level.ID);
	}
	
	
	/** Level Times Played **/
	
	protected override int _incTimesPlayed(Level level) {
		return levelStorage_IncTimesPlayed(level.ID);
	}
	
	protected override int _decTimesPlayed(Level level) {
		return levelStorage_DecTimesPlayed(level.ID);
	} 
	
	protected override int _getTimesPlayed(Level level) {
		return levelStorage_GetTimesPlayed(level.ID);
	}


#endif
	}
}

