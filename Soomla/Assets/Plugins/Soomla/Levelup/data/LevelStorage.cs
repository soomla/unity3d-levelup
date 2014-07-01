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

namespace Soomla.Levelup
{
	public class LevelStorage
	{

		protected const string TAG = "SOOMLA LevelStorage"; // used for Log error messages

		static LevelStorage _instance = null;
		static LevelStorage instance {
			get {
				if(_instance == null) {
					#if UNITY_ANDROID && !UNITY_EDITOR
					_instance = new LevelStorageAndroid();
					#elif UNITY_IOS && !UNITY_EDITOR
					_instance = new LevelStorageIOS();
					#else
					_instance = new LevelStorage();
					#endif
				}
				return _instance;
			}
		}
			

		public static void SetSlowestDuration(Level level, double duration) {
			_instance._setSlowestDuration (level, duration);	
		}
		
		public static double GetSlowestDuration(Level level) {
			_instance._getSlowestDuration (level);
		}
		
		public static void SetFastestDuration(Level level, double duration) {
			_instance._setFastestDuration (level, duration);
		}
		
		public static double GetFastestDuration(Level level) {
			_instance._getFastestDuration (level);
		}
		
		
		
		/** Level Times Started **/
		
		public static int IncTimesStarted(Level level) {
			_instance._incTimesStarted (level);
		}
		
		public static int DecTimesStarted(Level level) {
			_instance._decTimesStarted (level);
		}
		
		public static int GetTimesStarted(Level level) {
			_instance._getTimesStarted (level);
		}
		
		
		/** Level Times Played **/
		
		public static int IncTimesPlayed(Level level) {
			_instance._incTimesPlayed (level);
		}
		
		public static int DecTimesPlayed(Level level){
			_instance._decTimesPlayed (level);
		} 
		
		public static int GetTimesPlayed(Level level) {
			_instance._getTimesPlayed (level);
		}



		protected override void _setSlowestDuration(Level level, double duration) {
			// TODO: WIE
		}
		
		protected override double _getSlowestDuration(Level level) {
			// TODO: WIE
		}
		
		protected override void _setFastestDuration(Level level, double duration) {
			// TODO: WIE
		}
		
		protected override double _getFastestDuration(Level level) {
			// TODO: WIE
		}
		
		
		
		/** Level Times Started **/
		
		protected override int _incTimesStarted(Level level) {
			// TODO: WIE
		}
		
		protected override int _decTimesStarted(Level level) {
			// TODO: WIE
		}
		
		protected override int _getTimesStarted(Level level) {
			// TODO: WIE
		}
		
		
		/** Level Times Played **/
		
		protected override int _incTimesPlayed(Level level) {
			// TODO: WIE
		}
		
		protected override int _decTimesPlayed(Level level){
			// TODO: WIE
		} 
		
		protected override int _getTimesPlayed(Level level) {
			// TODO: WIE
		}
	}
}

