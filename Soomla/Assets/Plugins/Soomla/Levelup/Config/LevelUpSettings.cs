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
		static LevelUpSettings()
		{
			SoomlaEditorScript.addSettings(instance);
		}

//		BuildTargetGroup[] supportedPlatforms = { BuildTargetGroup.Android, BuildTargetGroup.iPhone,
//			BuildTargetGroup.WebPlayer, BuildTargetGroup.Standalone};

		GUIContent profileVersion = new GUIContent("LevelUp Version [?]", "The SOOMLA LevelUp version. ");
		GUIContent profileBuildVersion = new GUIContent("LevelUp Build [?]", "The SOOMLA LevelUp build.");

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

		public void OnInfoGUI() {
			SoomlaEditorScript.SelectableLabelField(profileVersion, "1.0.14");
			SoomlaEditorScript.SelectableLabelField(profileBuildVersion, "1");
			EditorGUILayout.Space();
		}

		public void OnSoomlaGUI() {
		}



		#endif




		/** LevelUp Specific Variables **/

	}
}
