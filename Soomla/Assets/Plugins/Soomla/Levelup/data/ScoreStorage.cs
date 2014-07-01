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

namespace Soomla
{
	public class ScoreStorage
	{

		protected const string TAG = "SOOMLA ScoreStorage"; // used for Log error messages

		static ScoreStorage _instance = null;
		static ScoreStorage instance {
			get {
				if(_instance == null) {
					#if UNITY_ANDROID && !UNITY_EDITOR
					_instance = new ScoreStorageAndroid();
					#elif UNITY_IOS && !UNITY_EDITOR
					_instance = new ScoreStorageIOS();
					#else
					_instance = new ScoreStorage();
					#endif
				}
				return _instance;
			}
		}
			

		public static void setLatestScore(Score score, double latest) {
			_instance._setLatestScore (score, latest);
		}

		public static double getLatestScore(Score score) {
			_instance._getLatestScore (score);
		}

		public static void setRecordScore(Score score, double record) {
			_instance._setRecordScore (score, record);
		}

		public static double getRecordScore(Score score) {
			_instance._getRecordScore (score);
		}



		virtual protected void _setLatestScore(Score score, double latest) {
			// TODO: WIE	
		}
		
		virtual protected double _getLatestScore(Score score) {
			// TODO: WIE
		}
		
		virtual protected void _setRecordScore(Score score, double record) {
			// TODO: WIE
		}
		
		virtual protected double _getRecordScore(Score score) {
			// TODO: WIE
		}
	}
}

