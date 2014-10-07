*This project is a part of The [SOOMLA](http://www.soom.la) Framework which is a series of open source initiatives with a joint goal to help mobile game developers do more together. SOOMLA encourages better game designing, economy modeling and faster development.*

unity3d-levelup is the implementation of the LevelUp module for Unity3d.

## Contents

- [Model Overview](#model-overview)
    - [World / Level](#world--level)
    - [Score](#score)
    - [Gate](#gate)
    - [Mission/Challenge](#missionchallenge)
    - [Reward](#reward)
- [Getting Started](#getting-started)
    - [Cloning](#cloning)
    - [Integration with SOOMLA unity3d-store](#integration-with-soomla-unity3d-store)
    - [Integration with SOOMLA unity3d-profile](#integration-with-soomla-unity3d-profile)
- [Debugging](#debugging)
- [Example Usages](#example-usages)

<!-- end toc -->

<!-- Check out our [Wiki] (https://github.com/soomla/android-store/wiki) for more information about the project and how to use it better. -->

## Model Overview

<!-- attach UML style simple diagram -->

Generally, the SOOMLA sources contain detailed documentation on the different entities and how to use them, but here's a quick glance:

![SOOMLA's LevelUp Model](http://know.soom.la/img/tutorial_img/soomla_diagrams/levelup.png)

### World / Level

A _Level_ is pretty clear, and most games have them.
A simple example is an Angry Birds single level, where you need to knock out all the pigs.
It measures specific things, such as duration it takes to complete, and can be started and ended.

A _World_ is a more general concept than a Level (a Level **Is-a** World), and can have `innerWorlds` to create hierarchies. Another example from Angry Birds is level pages and episodes, which contain the actual levels.

The _Initial World_ is a container world for all worlds and levels in the game. We use the _Initial World_ to intialize the LevelUp module.

### Score

A _Score_ is something which can be accumulated or measured within a _World_ (or _Level_ of course).
It can be incremented or decremented based on user actions, and recorded at the completion of the _World / Level_.

This, in turn, can later be applied to high scores or best times, or treated as collectibles that can be awarded upon completion.

### Gate

A _Gate_ is a closed portal from one _World_ to the next. It can be unlocked in many different ways (according to _Gate_ type), and can also be combined into a _GatesList_ to build more complex _Gates_.

### Mission/Challenge

A _Mission_ is a single task a player can complete in a game, usually for a _Reward_.

A _Challenge_ is a set of _Missions_ that need to be completed, so it's a big _Mission_ built out of several smaller _Missions_.

### Reward

A _Reward_ is some kind of perk or status a player can achieve in the game.
This can be either a badge, a virtual item from the game's economy (sword, coins etc.) or anything you can think of, really (unlocking game content or levels comes to mind).

## Getting Started
> NOTE: LevelUp depends on SOOMLA's other modules: Core, Store, and Profile. This document assumes that you are new to SOOMLA and have not worked with any of the other SOOMLA modules. If this is not the case, and you already *have* some or all of the other modules, please follow these directions only for the modules you are missing and of course, for the **LevelUp** module.

1. Download the following unitypackages:
    - [soomla-unity3d-core](https://github.com/soomla/unity3d-levelup/raw/master/soomla-unity3d-core.unitypackage)
    - [unity3d-store](https://github.com/soomla/unity3d-levelup/raw/master/soomla-unity3d-store.unitypackage)
    - [unity3d-profile](https://github.com/soomla/unity3d-levelup/raw/master/soomla-unity3d-profile.unitypackage)
    - [unity3d-levelup](https://github.com/soomla/unity3d-levelup/raw/master/soomla-unity3d-levelup.unitypackage)
2. Double-click on them following the order they appear above. It'll import all the necessary files into your project.
> If you are completely new to SOOMLA you can use the [All-in-One](https://github.com/soomla/unity3d-levelup/raw/master/soomla-unity3d-all-in-one.unitypackage) unitypackage which contains all the above packages in one package. Importing it is equivalent to preforming the above steps.

3. Drag the "CoreEvents", "StoreEvents", "ProfileEvents" and "LevelUpEvents" Prefabs from `../Assets/Soomla/Prefabs` into your scene. You should see it listed in the "Hierarchy" panel. [This step MUST be done for unity3d-levelup to work properly]
4. On the menu bar click "Window -> Soomla -> Edit Settings" and change the value for "Soomla Secret" (also setup Public Key if you're building for Google Play):
    - _Soomla Secret_ - is an encryption secret you provide that will be used to secure your data. (If you used versions before v1.5.2 this secret MUST be the same as Custom Secret)  
    **Choose the secret wisely. You can't change it after you launch your game!**
    - _Public Key_ - is the public key given to you from Google. (iOS doesn't have a public key).
5. Create your own _Initial World_ which should contain all the 'blueprint' of the game (see [Model Overview](#model-overview)). Initialize _LevelUp_ with the class you just created:

    ```cs
    LevelUp.GetInstance ().Initialize (initialWorld);
    ```

    > Initialize _LevelUp_ ONLY ONCE when your application loads.

    > Initialize _LevelUp_ in the "Start()" function of a 'MonoBehaviour' and **NOT** in the "Awake()" function. SOOMLA has its own 'MonoBehaviour' and it needs to be "Awakened" before you initialize.

6. You'll need an event handler in order to be notified about _LevelUp_ related events. refer to the [Event Handling](#event-handling) section for more information.

And that's it ! You have game architecture capabilities at your fingertips.

### Cloning

There are some necessary files in submodules lined with symbolic links. If you're cloning the project make sure you clone it with the `--recursive` flag.

```
$ git clone --recursive git@github.com:soomla/unity3d-levelup.git
```

### Integration with SOOMLA unity3d-store

Please follow the steps in [unity3d-store](https://github.com/soomla/unity3d-store) for the _Store_ part of the setup.
Then, you can use the **store-related _LevelUp_ classes**, such as _VirtualItemScore_ or _VirtualItemReward_ or _BalanceGate_.

### Integration with SOOMLA unity3d-profile

Please follow the steps in [unity3d-profile](https://github.com/soomla/unity3d-profile) for the _Profile_ part of the setup.
Then, you can use the **profile-related _LevelUp_ classes**, such as _SocialLikeMission_.

Event Handling
---

SOOMLA lets you subscribe to _LevelUp_ events, get notified and implement your own application specific behavior to those events.

> Your behavior is an addition to the default behavior implemented by SOOMLA. You don't replace SOOMLA's behavior.

The _Events_ class is where all event go through. To handle various events, just add your specific behavior to the delegates in the _Events_ class.

For example, if you want to 'listen' to a WorldCompleted event:

```cs
LevelUpEvents.OnWorldCompleted += onWorldCompleted;

public void onWorldCompleted(World world) {
    Debug.Log("The world " + world.ID + " was COMPLETED!");
}
```

One thing you need to make sure is that you instantiate your `EventHandler` before LevelUp.  
So if you have:
````
private static Soomla.Example.ExampleEventHandler handler;
````
you'll need to do:
````
handler = new Soomla.Example.ExampleEventHandler();
````
before
````
Soomla.Levelup.LevelUp.GetInstance ().Initialize (initialWorld);
````

## Debugging

If you want to see full debug messages from android-levelup and ios-levelup you just need to check the box that says "Debug Messages" in the SOOMLA Settings.
Unity debug messages will only be printed out if you build the project with _Development Build_ checked.

## Example Usages

  > Examples using virtual items are dependent on unity3d-store module, with proper `SoomlaStore` initialization and `IStoreAssets` definitions. See the unity3d-store integration section for more details.

* Mission with Reward (collect 5 stars to get 1 mega star)

	```cs
  VirtualItemReward virtualItemReward = new VirtualItemReward("mega_star_reward_id",
      "MegaStarReward", megaStarItemId, 1);

  List<Reward> rewards = new List<Reward>();
  rewards.Add(virtualItemReward);

  BalanceMission balanceMission = new BalanceMission("star_balance_mission_id",
      "StarBalanceMission", rewards, starItemId, 5);

  // use the store to give the items out, usually this will be called from in-game events
  // such as player collecting the stars
  StoreInventory.GiveItem(starItemId, 5);

  // events posted:
  // 1. OnGoodBalanceChanged (Store events)
  // 2. OnMissionCompleted (LevelUp events)
  // 3. OnRewardGivenEvent (Core events)

  // now the mission is complete, and reward given
  balanceMission.IsCompleted(); // true
  virtualItemReward.Owned; // true
	```

* RecordGate with RangeScore

	```cs
  Level lvl1 = new Level("lvl1_recordgate_rangescore");
  Level lvl2 = new Level("lvl2_recordgate_rangescore");

  string scoreId = "range_score";
  RangeScore rangeScore = new RangeScore(scoreId, new RangeScore.SRange(0.0, 100.0));

  string recordGateId = "record_gate";
  RecordGate recordGate = new RecordGate(recordGateId, scoreId, 100.0);

  lvl1.AddScore(rangeScore);

  // Lock level 2 with record gate
  lvl2.Gate = recordGate;

  // the initial world
  world.AddInnerWorld(lvl1);
  world.AddInnerWorld(lvl2);

  LevelUp.GetInstance().Initialize(world);

  lvl1.Start();

  // events posted:
  // OnLevelStarted (LevelUp events)

  rangeScore.Inc(100.0);

  lvl1.End(true);

  // events posted:
  // OnLevelEnded (LevelUp events)
  // OnWorldCompleted (lvl1) (LevelUp events)
  // OnGateOpened (LevelUp events)
  // [OnScoreRecordReached] - if record was broken (LevelUp events)

  recordGate.IsOpen(); // true

  lvl2.CanStart(); // true
  lvl2.Start();
  lvl2.End(true);

  // events posted:
  // OnWorldCompleted (lvl2) (LevelUp events)

  lvl2.IsCompleted(); // true
	```

* VirtualItemScore

	```cs
  Level lvl1 = new Level("lvl1_viscore");
  string itemId = ITEM_ID_VI_SCORE;
  string scoreId = "vi_score";
  VirtualItemScore virtualItemScore = new VirtualItemScore(scoreId, itemId);
  lvl1.AddScore(virtualItemScore);

  world.AddInnerWorld(lvl1);

  LevelUp.GetInstance().Initialize(world);

  lvl1.Start();
  // events posted:
  // OnLevelStarted (LevelUp events)

  virtualItemScore.Inc(2.0);
  // events posted:
  // OnGoodBalanceChanged (Store events)

  lvl1.End(true);
  // events posted:
  // OnLevelEnded (LevelUp events)
  // OnWorldCompleted (lvl1) (LevelUp events)
  // [OnScoreRecordChanged] - if record was broken (LevelUp events)

  int currentBalance = StoreInventory.GetItemBalance(ITEM_ID_VI_SCORE);
  // currentBalance == 2
	```

* Challenge (Multi-Mission)

  ```cs
  string scoreId = "main_score";
	Score score = new Score(scoreId);

	Mission mission1 = new RecordMission("record1_mission", "Record 1 mission",
	                                     scoreId, 10.0);
	Mission mission2 = new RecordMission("record2_mission", "Record 2 mission",
	                                     scoreId, 100.0);
	List<Mission> missions = new List<Mission>() { mission1, mission2 };

	BadgeReward badgeReward = new BadgeReward("challenge_badge_reward_id",
	                                          "ChallengeBadgeRewardId");
	List<Reward> rewards = new List<Reward>() { badgeReward };

	Challenge challenge = new Challenge("challenge_id", "Challenge", missions, rewards);

	challenge.IsCompleted(); //false

	World world = new World("initial_world");
	world.AddMission(challenge);
	world.AddScore(score);

	LevelUp.GetInstance().Initialize(world);

	score.SetTempScore(20.0);
	score.Reset(true);

	// events:
	// OnMissionCompleted (mission1) (LevelUp events)
	// [OnScoreRecordReached] - if record is broken

	score.SetTempScore(120.0);
	score.Reset(true);

	// events:
	// OnMissionCompleted (mission2) (LevelUp events)
	// OnMissionCompleted (challenge) (LevelUp events)
	// OnRewardGivenEvent (badgeReward) (Core events)

	challenge.IsCompleted(); // true
	badgeReward.Owned; // true
  ```

* GatesList
> Note that currently a `GatesList` gate is automatically opened when sub-gates fulfill the `GatesList` requirement.

  ```cs
  string recordGateId1 = "gates_list_record_gate_id1";
  string scoreId1 = "gates_list_score_id1";
  double desiredRecord1 = 10.0;
  string recordGateId2 = "gates_list_record_gate_id2";
  string scoreId2 = "gates_list_score_id2";
  double desiredRecord2 = 20.0;

  Score score1 = new Score(scoreId1);
  Score score2 = new Score(scoreId2);

  World world = new World("initial_world");
  Level lvl1 = new Level("level1_id");
  lvl1.AddScore(score1);
  lvl1.AddScore(score2);
  world.AddInnerWorld(lvl1);

  RecordGate recordGate1 = new RecordGate(recordGateId1, scoreId1, desiredRecord1);
  RecordGate recordGate2 = new RecordGate(recordGateId2, scoreId2, desiredRecord2);

  List<Gate> gates = new List<Gate>() { recordGate1, recordGate2 };

  GatesListOR gatesListOR = new GatesListOR("gate_list_OR_id", gates);

  GatesListAND gatesListAND = new GatesListAND("gate_list_AND_id", gates);

  LevelUp.GetInstance().Initialize(world);

  score1.SetTempScore(desiredRecord1);
  score1.Reset(true);

  recordGate1.IsOpen(); // true
  gatesListOR.IsOpen(); // true

  gatesListAND.CanOpen(); // false (all sub-gates need to be open for AND)
  gatesListAND.IsOpen(); // false

  score2.SetTempScore(desiredRecord2);
  score2.Reset(true);

  recordGate2.IsOpen(); // true
  gatesListOR.IsOpen(); // still true
  gatesListAND.IsOpen(); // true
  ```

**Working with Unity Test Tools**:
- Install Unity Test Tools from assets store or from https://www.assetstore.unity3d.com/en/#!/content/13802
- Use NUnity to create tests (See example to Level class: Soomla/Editor/UnitTest/LevelTest.cs)
- Run tests from Unity GUI: Open Unit Test Tool (menu bar, should be added after adding the asset from step 1), and choose Unit Test Runner. Select the test you wish to execute and press play.
- Run tests from command line by executing run_tests script.
unity3d-levelup is a library built for easily modeling game structure and user progression, and allows rapid protoyping using a standard and simplified model.  It acts as sort of a 'blueprint' for the game, modeling worlds/levels, gates to levels, missions and rewards that can be completed and achieved.  All this is backed by SOOMLA's core tools, and can be easily integrated with more SOOMLA modules, like unity3d-store for IAP, or unity3d-profile for social related functions.

Contribution
---

We want you!

Fork -> Clone -> Implement -> Insert Comments -> Test -> Pull-Request.

We have great RESPECT for contributors.

Code Documentation
---

unity3d-levelup follows strict code documentation conventions. If you would like to contribute please read our [Documentation Guidelines](https://github.com/soomla/unity3d-store/tree/master/documentation.md) and follow them. Clear, consistent  comments will make our code easy to understand.

SOOMLA, Elsewhere ...
---

+ [Framework Website](http://www.soom.la/)
+ [On Facebook](https://www.facebook.com/pages/The-SOOMLA-Project/389643294427376).
+ [On AngelList](https://angel.co/the-soomla-project)

License
---
Apache License. Copyright (c) 2012-2014 SOOMLA. http://www.soom.la
+ http://opensource.org/licenses/Apache-2.0
