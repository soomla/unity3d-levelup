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
	public class LevelStorageIOS : LevelStorage {
#if UNITY_IOS 
		//&& !UNITY_EDITOR

	[DllImport ("__Internal")]
	private static extern void levelStorage_SetSlowestDurationMillis(string levelJson, long duration);
	[DllImport ("__Internal")]
	private static extern long levelStorage_GetSlowestDurationMillis(string levelJson);
	[DllImport ("__Internal")]
	private static extern void levelStorage_SetFastestDurationMillis(string levelJson, long duration);
	[DllImport ("__Internal")]
	private static extern long levelStorage_GetFastestDurationMillis(string levelJson);
	[DllImport ("__Internal")]
	private static extern int levelStorage_IncTimesStarted(string levelJson);
	[DllImport ("__Internal")]
	private static extern int levelStorage_DecTimesStarted(string levelJson);
	[DllImport ("__Internal")]
	private static extern int levelStorage_GetTimesStarted(string levelJson);
	[DllImport ("__Internal")]
	private static extern int levelStorage_IncTimesPlayed(string levelJson);
	[DllImport ("__Internal")]
	private static extern int levelStorage_DecTimesPlayed(string levelJson);
	[DllImport ("__Internal")]
	private static extern int levelStorage_GetTimesPlayed(string levelJson);


	protected override void _setSlowestDurationMillis(Level level, long duration) {
		string levelJson = level.toJSONObject().ToString();
		levelStorage_SetSlowestDurationMillis(levelJson, duration);
	}
	
	protected override long _getSlowestDurationMillis(Level level) {
		string levelJson = level.toJSONObject().ToString();
		return levelStorage_GetSlowestDurationMillis(levelJson);
	}
	
	protected override void _setFastestDurationMillis(Level level, long duration) {
		string levelJson = level.toJSONObject().ToString();
		levelStorage_SetFastestDurationMillis(levelJson, duration);
	}
	
	protected override long _getFastestDurationMillis(Level level) {
		string levelJson = level.toJSONObject().ToString();
		return levelStorage_GetFastestDurationMillis(levelJson);
	}
	
		
	/** Level Times Started **/
	
	protected override int _incTimesStarted(Level level) {
		string levelJson = level.toJSONObject().ToString();
		return levelStorage_IncTimesStarted(levelJson);
	}
	
	protected override int _decTimesStarted(Level level) {
		string levelJson = level.toJSONObject().ToString();
		return levelStorage_DecTimesStarted(levelJson);
	}
	
	protected override int _getTimesStarted(Level level) {
		string levelJson = level.toJSONObject().ToString();
		return levelStorage_GetTimesStarted(levelJson);
	}
	
	
	/** Level Times Played **/
	
	protected override int _incTimesPlayed(Level level) {
		string levelJson = level.toJSONObject().ToString();
		return levelStorage_IncTimesPlayed(levelJson);
	}
	
	protected override int _decTimesPlayed(Level level) {
		string levelJson = level.toJSONObject().ToString();
		return levelStorage_DecTimesPlayed(levelJson);
	} 
	
	protected override int _getTimesPlayed(Level level) {
		string levelJson = level.toJSONObject().ToString();
		return levelStorage_GetTimesPlayed(levelJson);
	}


#endif
	}
}

