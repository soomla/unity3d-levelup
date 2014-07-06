using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using Soomla;
using Soomla.Levelup;
using Soomla.Store;
using Soomla.Store.Example;

namespace Soomla.Test {
	public class BasicTest : MonoBehaviour {

		private const string TAG = "SOOMLA-TEST BasicTest";

		private static string sTestLog;

		private Queue<Dictionary<string, object>> _eventQueue;

		GUIStyle _textStyle = new GUIStyle();

		private class Assert {

			private static string getStackTrace() {
				string stacktrace = "";
				StackTrace stackTrace = new StackTrace ();           // get call stack
				StackFrame[] stackFrames = stackTrace.GetFrames ();  // get method calls (frames)
		
				// write call stack method names
				foreach (StackFrame stackFrame in stackFrames) {
						stacktrace += stackFrame.ToString ();
				}

				return stacktrace;
			}

			public static void assertTrue(bool cond) {
				if (!cond) {
					sTestLog += "<color=red>FAIL!</color>\n";
//					sTestLog += UnityEngine.StackTraceUtility.ExtractStackTrace () + "\n";
					sTestLog += getStackTrace() + "\n";
					UnityEngine.Debug.LogException(new Exception("assertTrue"));
					throw new Exception("assertTrue");
				}
			}
			public static void assertFalse(bool cond) {
				if (cond) {
					sTestLog += "<color=red>FAIL!</color>\n";
//					sTestLog += UnityEngine.StackTraceUtility.ExtractStackTrace () + "\n";
					sTestLog += getStackTrace() + "\n";
					UnityEngine.Debug.LogException(new Exception("assertTrue"));
					throw new Exception("assertTrue");
				}
			}
			public static void assertEquals<T>(T expected, T actual) {
				if (!expected.Equals (actual)) {
					sTestLog += string.Format("<color=red>FAIL! expected:{0} actual:{1}</color>\n", expected, actual);
//					sTestLog += UnityEngine.StackTraceUtility.ExtractStackTrace () + "\n";
					sTestLog += getStackTrace() + "\n";
					UnityEngine.Debug.LogException(new Exception(expected + "!=" + actual));
					throw new Exception(expected + "!=" + actual);
				}
			}
			public static void assertEquals(double actual, double expected, double percision) {
				if (Math.Abs(actual-expected) > percision) {
					sTestLog += string.Format("<color=red>FAIL! expected:{0} actual:{1} percision:{2}</color>\n", expected, actual, percision);
					sTestLog += UnityEngine.StackTraceUtility.ExtractStackTrace () + "\n";
//					sTestLog += getStackTrace() + "\n";
					UnityEngine.Debug.LogException(new Exception(expected + "!=" + actual + "(percision:" + percision + ")"));
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

			sTestLog = "";
			_eventQueue = new Queue<Dictionary<string, object>> ();

//			JSONObject jsonObject = new JSONObject (@"{
//  ""scores"" : [
//
//  ],
//  ""className"" : ""Level"",
//""challenges"" : [
//
//  ],
//  ""worldId"" : ""lvl1"",
//  ""worlds"" : [
//
//  ]
//}");
//			Level.fromJSONObject (jsonObject);
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

			_textStyle.wordWrap = true;
			_textStyle.richText = true;	
			_textStyle.normal.textColor = Color.white;
			_textStyle.fontSize = 14;

			// clear last DB storage so test run anew
			string dbPath = null;
#if UNITY_ANDROID
			dbPath = "/private" + Application.persistentDataPath + "/store.kv.db";
#elif UNITY_IOS
			dbPath = "/private" + Application.persistentDataPath + "/../Library/Application Support/store.kv.db";
#endif
			if (dbPath != null) {
				UnityEngine.Debug.LogWarning ("TESTING-> db file exists?=" + System.IO.File.Exists(dbPath));
				UnityEngine.Debug.LogWarning ("TESTING-> delete db file at:" + dbPath);
				System.IO.File.Delete (dbPath);
			}


	//		SoomlaInit ("hansolo");
			StoreEvents.OnSoomlaStoreInitialized += onSoomlaStoreInitialized;
			LevelUpEvents.Initialize();
			SoomlaStore.Initialize (new MuffinRushAssets ());
		}

		public void onSoomlaStoreInitialized() {
//			Coroutine<bool> testScoreAscCR = Coroutine<bool>(testScoreAsc());
//			yield return testScoreAscCR.coroutine;
//			try {
//				if (testScoreAscCR.Value) {
//				}
//
//			}
//			catch (Exception e) {
//				//and handle any exceptions here
//			}

			LevelUpEvents.OnGateOpened += onGateOpen;
			LevelUpEvents.OnLevelEnded += onLevelEnded;
			LevelUpEvents.OnLevelStarted += onLevelStarted;
			LevelUpEvents.OnMissionCompleted += onMissionCompleted;
			LevelUpEvents.OnMissionCompletionRevoked += onMissionCompletedRevoked;
			LevelUpEvents.OnScoreRecordChanged += onScoreRecordChanged;
			LevelUpEvents.OnWorldCompleted += onWorldCompleted;

			StartCoroutine(testLevel());
			Assert.assertTrue (_eventQueue.Count == 0);
//			StartCoroutine (testScoreAsc());
//			Assert.assertTrue (_eventQueue.Count == 0);
		}

		private void onGateOpen(Gate gate) {

		}

		private void onLevelEnded(Level level) {
		}

		private void onLevelStarted(Level level) {
		}

		private void onMissionCompleted(Mission mission) {
		}

		private void onMissionCompletedRevoked(Mission mission) {
		}

		private void onScoreRecordChanged(Score score) {
			string scoreId = score.ScoreId;
			double record = score.Record;
			string msg = "<color=yellow>onEvent/onScoreRecordChanged:</color>" + score + "->" + record;
			sTestLog += msg + "\n";
			SoomlaUtils.LogDebug(TAG, msg);
			Dictionary<string, object> expected = _eventQueue.Dequeue ();
			Assert.assertEquals(expected["id"], scoreId);
			Assert.assertEquals((double)expected["val"], record, 0.1);
		}

		private void onWorldCompleted(World world) {
			string worldId = world.WorldId;
			string msg = "<color=yellow>onEvent/onWorldCompleted:</color>" + worldId;
			sTestLog += msg + "\n";
			SoomlaUtils.LogDebug(TAG, msg);
			Dictionary<string, object> expected = _eventQueue.Dequeue ();
			Assert.assertEquals(expected["id"], worldId);
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

		void OnGUI() {
			GUI.TextArea (new Rect (10, 10, Screen.width-10, Screen.height-10), sTestLog, _textStyle);
		}

		private void createFruitsGoblins() {
			World mainWorld = new World("main_world");

			World machineA = new World("machine_a", 20, true);
			World machineB = new World("machine_b", 20, true);
			World machineC = new World("machine_c", 20, true);
			World machineD = new World("machine_d", 20, true);

			BadgeReward bronzeMedal = new BadgeReward("badge_bronzeMedal", "Bronze Medal");
			BadgeReward silverMedal = new BadgeReward("badge_silverMedal", "Silver Medal");
			BadgeReward goldMedal = new BadgeReward("badge_goldMedal", "Gold Medal");
			VirtualItemReward perfectMedal = new VirtualItemReward("item_perfectMedal", "Perfect Medal", "perfect_medal", 1);


			/** Testing **/

			Level lvl1 = (Level)machineA.InnerWorldsList[0];
			lvl1.AssignReward(goldMedal);
		}
			
		private IEnumerator testLevel() {
			sTestLog += "testLevel...";		

			List<World> worlds = new List<World>();
			Level lvl1 = new Level("lvl1", false);
			worlds.Add(lvl1);
			
			//LevelUp.Initialize(worlds);
			
			// no gates
			Assert.assertTrue(lvl1.CanStart());
			Assert.assertTrue(lvl1.State == Level.LevelState.Idle);

			_eventQueue.Clear ();

			_eventQueue.Enqueue (new Dictionary<string, object> {
				{ "id", "lvl1" }
			});
			
			lvl1.Start();
			Assert.assertTrue(lvl1.State == Level.LevelState.Running);
			
			yield return new WaitForSeconds(1);
			// check level time measure
			double playDuration = lvl1.GetPlayDuration();
			SoomlaUtils.LogDebug(TAG, "playDuration = " + playDuration);
			sTestLog += "playDuration = " + playDuration + "\n";
			Assert.assertTrue(playDuration >= 1);
			Assert.assertFalse(playDuration > 2);
			
			lvl1.Pause();
			yield return new WaitForSeconds(1);
			// make sure no changes after pause
			playDuration = lvl1.GetPlayDuration();
			SoomlaUtils.LogDebug(TAG, "playDuration = " + playDuration);
			sTestLog += "playDuration = " + playDuration + "\n";;
			Assert.assertTrue(playDuration >= 1);
			Assert.assertFalse(playDuration > 2);
			Assert.assertTrue(lvl1.State == Level.LevelState.Paused);
			
			lvl1.Resume();
			yield return new WaitForSeconds(1);
			// make sure working after resume
			playDuration = lvl1.GetPlayDuration();
			SoomlaUtils.LogDebug(TAG, "playDuration = " + playDuration);
			sTestLog += "playDuration = " + playDuration + "\n";;
			Assert.assertTrue(playDuration >= 2);
			Assert.assertFalse(playDuration > 3);
			Assert.assertTrue(lvl1.State == Level.LevelState.Running);
			
			lvl1.End(false);
			Assert.assertTrue(lvl1.State == Level.LevelState.Ended);
			Assert.assertFalse(lvl1.IsCompleted());
			
			lvl1.SetCompleted(true);
			Assert.assertTrue(lvl1.IsCompleted());

			// it seems there is a delay of ~0.5-1.0 seconds of saving to storage
			Assert.assertEquals(playDuration, lvl1.GetSlowestDuration(), 0.9);
			Assert.assertEquals(playDuration, lvl1.GetFastestDuration(), 0.9);
			Assert.assertEquals(1, lvl1.GetTimesPlayed());
			Assert.assertEquals(1, lvl1.GetTimesStarted());		

			UnityEngine.Debug.LogError("Done! SOOMLA");

			sTestLog += "<color=green>SUCCESS</color>\n";

			yield return null;
		}

		public IEnumerator testScoreAsc() {
			sTestLog += "testScoreAsc...";
			UnityEngine.Debug.LogError("testScoreAsc SOOMLA");
			bool higherIsBetter = true;
			string scoreId = "score_asc";
			Score scoreAsc = new Score(scoreId, "ScoreAsc", higherIsBetter);		

			_eventQueue.Clear ();

			_eventQueue.Enqueue (new Dictionary<string, object> {
				{ "id", scoreId }, 
				{ "val", 0 }
			});
			
			Assert.assertEquals(0, scoreAsc.GetTempScore(), 0.01);
			scoreAsc.StartValue = 0;
			scoreAsc.Inc(1);
			Assert.assertEquals(1, scoreAsc.GetTempScore(), 0.01);
			scoreAsc.Dec(1);
			Assert.assertEquals(0, scoreAsc.GetTempScore(), 0.01);
			scoreAsc.Inc(10);
			Assert.assertEquals(10, scoreAsc.GetTempScore(), 0.01);
	//		mExpectedRecordValue = 10;
			_eventQueue.Enqueue (new Dictionary<string, object> {
				{ "id", scoreId }, 
				{ "val", 10 }
			});
			scoreAsc.SaveAndReset();
			Assert.assertEquals(10, scoreAsc.Latest, 0.01);
			Assert.assertEquals(0, scoreAsc.GetTempScore(), 0.01);
			scoreAsc.SetTempScore(20);
	//		mExpectedRecordValue = 0;
			_eventQueue.Enqueue (new Dictionary<string, object> {
				{ "id", scoreId }, 
				{ "val", 0 }
			});
			scoreAsc.Reset();
			Assert.assertEquals(0, scoreAsc.Latest, 0.01);
			Assert.assertEquals(0, scoreAsc.GetTempScore(), 0.01);
			scoreAsc.SetTempScore(30);
			Assert.assertTrue(scoreAsc.HasTempReached(30));
			Assert.assertFalse(scoreAsc.HasTempReached(31));
	//		mExpectedRecordValue = 30;
			_eventQueue.Enqueue (new Dictionary<string, object> {
				{ "id", scoreId }, 
				{ "val", 30 }
			});
			scoreAsc.SaveAndReset();
			Assert.assertEquals(30, scoreAsc.Latest, 0.01);
			Assert.assertEquals(30, scoreAsc.Record, 0.01);
			scoreAsc.SetTempScore(15);
	//		mExpectedRecordValue = 30;
			_eventQueue.Enqueue (new Dictionary<string, object> {
				{ "id", scoreId }, 
				{ "val", 30 }
			});
			scoreAsc.SaveAndReset();
			Assert.assertEquals(15, scoreAsc.Latest, 0.01);
			Assert.assertEquals(30, scoreAsc.Record, 0.01);
			Assert.assertTrue(scoreAsc.HasRecordReached(30));
			Assert.assertFalse(scoreAsc.HasRecordReached(31));

			UnityEngine.Debug.LogError("Done! SOOMLA");

			sTestLog += "<color=green>SUCCESS</color>\n";					

			yield return null;
		}
	}

	public class Coroutine<T>{
		public T Value {
			get{
				if(e != null){
					throw e;
				}
				return returnVal;
			}
		}
		private T returnVal;
		private Exception e;
		public Coroutine coroutine;
		
		public IEnumerator InternalRoutine(IEnumerator coroutine){
			while(true){
				try{
					if(!coroutine.MoveNext()){
						yield break;
					}
				}
				catch(Exception e){
					this.e = e;
					yield break;
				}
				object yielded = coroutine.Current;
				if(yielded != null && yielded.GetType() == typeof(T)){
					returnVal = (T)yielded;
					yield break;
				}
				else{
					yield return coroutine.Current;
				}
			}
		}
	}
}
