using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using Soomla;
using Soomla.Levelup;
using Soomla.Store;
using Soomla.Store.Example;

namespace Soomla.Test {
	public class BasicTest : MonoBehaviour {

		private const string TAG = "SOOMLA-TEST BasicTest";

		private class Assert {
			public static void assertTrue(bool cond) {
				if (!cond) {
					throw new Exception("assertTrue");
				}
			}
			public static void assertFalse(bool cond) {
				if (cond) {
					throw new Exception("assertTrue");
				}
			}
			public static void assertEquals<T>(T expected, T actual) {
				if (!expected.Equals (actual)) {
					throw new Exception(expected + "!=" + actual);
				}
			}
			public static void assertEquals(double actual, double expected, double percision) {
				if (Math.Abs(actual-expected) > percision) {
					throw new Exception(expected + "!=" + actual);
				}
			}
		}

		private static BasicTest instance = null;
		void Awake(){
			if(instance == null){ 	//making sure we only initialize one instance.
				instance = this;
				GameObject.DontDestroyOnLoad(this.gameObject);
			} else {					//Destroying unused instances.
				GameObject.Destroy(this);
			}
		}

//		bool SoomlaInit(string secret) {
//	#if !UNITY_EDITOR
//	#if UNITY_IOS
//			[DllImport ("__Internal")]
//			private static extern void soomla_SetLogDebug(bool debug);
//			[DllImport ("__Internal")]
//			private static extern void soomla_Init(string secret);
//					
//			/// <summary>
//			/// Initializes the SOOMLA SDK.
//			/// </summary>
//			public static bool Initialize() {
//				soomla_SetLogDebug(true);
//				soomla_Init(secret);
//				return true;
//			}
//	#endif
//	#if UNITY_ANDROID
//			AndroidJNI.PushLocalFrame(100);
//			using(AndroidJavaClass jniSoomlaClass = new AndroidJavaClass("com.soomla.Soomla")) {
//				jniSoomlaClass.CallStatic("initialize", secret);
//			}
//			//init EventHandler
//			using(AndroidJavaClass jniEventHandler = new AndroidJavaClass("com.soomla.unity.SoomlaEventHandler")) {
//				jniEventHandler.CallStatic("initialize");
//			}
//			AndroidJNI.PopLocalFrame(IntPtr.Zero);
//			return true;
//
//	#endif
//	#else
//			return false;
//	#endif
//		}

		// Use this for initialization
		void Start () {
	//		SoomlaInit ("hansolo");
			UnityEngine.Debug.LogError("Start SOOMLA");
			StoreEvents.OnSoomlaStoreInitialized += onSoomlaStoreInitialized;
//			StoreEvents.OnSoomlaStoreInitialized += () => {
//				testScoreAsc ();
////				testLevel();
//			};
			UnityEngine.Debug.LogError("Start2 SOOMLA");
			SoomlaStore.Initialize (new MuffinRushAssets ());
		}

		public void onSoomlaStoreInitialized() {
			UnityEngine.Debug.LogError("onSoomlaStoreInitialized SOOMLA");
			printSomething();
			testScoreAsc();
			printSomething();
			UnityEngine.Debug.LogError("onSoomlaStoreInitialized2 SOOMLA");
		}

		private void printSomething() {
			UnityEngine.Debug.LogError("something SOOMLA");
		}

		void Update() {
			if (Application.platform == RuntimePlatform.Android) {
				if (Input.GetKeyUp(KeyCode.Escape)) {
					//quit application on back button
					Application.Quit();
					return;
				}
			}
		}

		private IEnumerator testLevel() {
			List<World> worlds = new List<World>();
			Level lvl1 = new Level("lvl1");
			worlds.Add(lvl1);
			
			//LevelUp.Initialize(worlds);
			
			// no gates
			Assert.assertTrue(lvl1.CanStart());
			Assert.assertTrue(lvl1.State == Level.LevelState.Idle);
			
	//		mExpectedWorldEventId = "lvl1";
			
			lvl1.Start();
			Assert.assertTrue(lvl1.State == Level.LevelState.Running);
			
			yield return new WaitForSeconds(1);
			// check level time measure
			double playDuration = lvl1.GetPlayDuration();
			SoomlaUtils.LogDebug(TAG, "playDuration = " + playDuration);
			Assert.assertTrue(playDuration >= 1);
			Assert.assertFalse(playDuration > 2);
			
			lvl1.Pause();
			yield return new WaitForSeconds(1);
			// make sure no changes after pause
			playDuration = lvl1.GetPlayDuration();
			SoomlaUtils.LogDebug(TAG, "playDuration = " + playDuration);
			Assert.assertTrue(playDuration >= 1);
			Assert.assertFalse(playDuration > 2);
			Assert.assertTrue(lvl1.State == Level.LevelState.Paused);
			
			lvl1.Resume();
			yield return new WaitForSeconds(1);
			// make sure working after resume
			playDuration = lvl1.GetPlayDuration();
			SoomlaUtils.LogDebug(TAG, "playDuration = " + playDuration);
			Assert.assertTrue(playDuration >= 2);
			Assert.assertFalse(playDuration > 3);
			Assert.assertTrue(lvl1.State == Level.LevelState.Running);
			
			lvl1.End(false);
			Assert.assertTrue(lvl1.State == Level.LevelState.Ended);
			Assert.assertFalse(lvl1.IsCompleted());
			
			lvl1.SetCompleted(true);
			Assert.assertTrue(lvl1.IsCompleted());
			
			Assert.assertEquals(playDuration, lvl1.GetSlowestDuration(), 0.1);
			Assert.assertEquals(playDuration, lvl1.GetFastestDuration(), 0.1);
			Assert.assertEquals(1, lvl1.GetTimesPlayed());
			Assert.assertEquals(1, lvl1.GetTimesStarted());

			yield return null;
		}

		public void testScoreAsc() {
			UnityEngine.Debug.LogError("testScoreAsc SOOMLA");
			bool higherIsBetter = true;
			string scoreId = "score_asc";
			Score scoreAsc = new Score(scoreId, "ScoreAsc", higherIsBetter);
			
			//mExpectedScoreEventId = scoreId;
			
			Assert.assertEquals(0, scoreAsc.GetTempScore(), 0.01);
			scoreAsc.StartValue = 0;
			scoreAsc.Inc(1);
			Assert.assertEquals(1, scoreAsc.GetTempScore(), 0.01);
			scoreAsc.Dec(1);
			Assert.assertEquals(0, scoreAsc.GetTempScore(), 0.01);
			scoreAsc.Inc(10);
			Assert.assertEquals(10, scoreAsc.GetTempScore(), 0.01);
	//		mExpectedRecordValue = 10;
			scoreAsc.SaveAndReset();
			Assert.assertEquals(10, scoreAsc.Latest, 0.01);
			Assert.assertEquals(0, scoreAsc.GetTempScore(), 0.01);
			scoreAsc.SetTempScore(20);
	//		mExpectedRecordValue = 0;
			scoreAsc.Reset();
			Assert.assertEquals(0, scoreAsc.Latest, 0.01);
			Assert.assertEquals(0, scoreAsc.GetTempScore(), 0.01);
			scoreAsc.SetTempScore(30);
			Assert.assertTrue(scoreAsc.HasTempReached(30));
			Assert.assertFalse(scoreAsc.HasTempReached(31));
	//		mExpectedRecordValue = 30;
			scoreAsc.SaveAndReset();
			Assert.assertEquals(30, scoreAsc.Latest, 0.01);
			Assert.assertEquals(30, scoreAsc.Record, 0.01);
			scoreAsc.SetTempScore(15);
	//		mExpectedRecordValue = 30;
			scoreAsc.SaveAndReset();
			Assert.assertEquals(15, scoreAsc.Latest, 0.01);
			Assert.assertEquals(30, scoreAsc.Record, 0.01);
			Assert.assertTrue(scoreAsc.HasRecordReached(30));
			Assert.assertFalse(scoreAsc.HasRecordReached(31));

			UnityEngine.Debug.LogError("Done! SOOMLA");

//			yield return null;
		}
	}
}
