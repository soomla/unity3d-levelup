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
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Soomla.Levelup
{

	#if UNITY_EDITOR
	[InitializeOnLoad]
	#endif
	/// <summary>
	/// This class holds the levelup's configurations.
	/// </summary>
	public class LevelUpSettings : ISoomlaSettings
	{

		#if UNITY_EDITOR

		static LevelUpSettings instance = new LevelUpSettings();

		static string currentModuleVersion = "1.1.2";

		static LevelUpSettings()
		{
			SoomlaEditorScript.addSettings(instance);

			SoomlaEditorScript.addFileList("LevelUp", "Assets/Soomla/levelup_file_list", new string[] {});
		}

//		BuildTargetGroup[] supportedPlatforms = { BuildTargetGroup.Android, BuildTargetGroup.iPhone,
//			BuildTargetGroup.WebPlayer, BuildTargetGroup.Standalone};

		GUIContent levelUpVersion = new GUIContent("LevelUp Version [?]", "The SOOMLA LevelUp version. ");

		private LevelUpSettings()
		{

        }

		public void OnEnable() {
			// Generating AndroidManifest.xml
			//			ManifestTools.GenerateManifest();
		}

		public void OnModuleGUI() {
//			AndroidGUI();
//			EditorGUILayout.Space();
//			IOSGUI();

		}

		public void OnAndroidGUI() {
			
		}
		
		public void OnIOSGUI(){

		}
		
		public void OnWP8GUI(){
			
		}

		public void OnInfoGUI() {
			SoomlaEditorScript.RemoveSoomlaModuleButton(levelUpVersion, currentModuleVersion, "LevelUp");
			SoomlaEditorScript.LatestVersionField ("unity3d-levelup", currentModuleVersion, "New version available!", "http://library.soom.la/fetch/unity3d-levelup-only/latest?cf=unity");
			EditorGUILayout.Space();
		}

		public void OnSoomlaGUI() {
		}



		#endif




		/** LevelUp Specific Variables **/

	}
}
