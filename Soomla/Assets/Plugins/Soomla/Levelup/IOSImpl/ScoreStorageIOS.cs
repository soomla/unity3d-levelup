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
	public class ScoreStorageIOS : ScoreStorage {
#if UNITY_IOS && !UNITY_EDITOR

	[DllImport ("__Internal")]
	private static extern void scoreStorage_SetLatestScore(string scoreId, double latest);
	[DllImport ("__Internal")]
	private static extern double scoreStorage_GetLatestScore(string scoreId);
	[DllImport ("__Internal")]
	private static extern void scoreStorage_SetRecordScore(string scoreId, double record);
	[DllImport ("__Internal")]
	private static extern double scoreStorage_GetRecordScore(string scoreId);


	override protected void _setLatestScore(Score score, double latest) {
		scoreStorage_SetLatestScore(score.ID, latest);
	}
	
	override protected double _getLatestScore(Score score) {
		return scoreStorage_GetLatestScore(score.ID);
	}
	
	override protected void _setRecordScore(Score score, double record) {
		scoreStorage_SetRecordScore(score.ID, record);
	}
	
	override protected double _getRecordScore(Score score) {
		return scoreStorage_GetRecordScore(score.ID);
	}

#endif
	}
}

