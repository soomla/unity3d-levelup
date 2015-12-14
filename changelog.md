### v1.1.2 [view commit logs](https://github.com/soomla/unity3d-levelup/compare/v1.1.1...v1.1.2)

* New Features
  * Added taking "Time.timeScale" into account for level duration

* Changes
  * Minor improvements and rearrangements to Editor script

### v1.1.1 [view commit logs](https://github.com/soomla/unity3d-levelup/compare/v1.1.0...v1.1.1)

* Changes
  * Minor improvements to Editor script

### v1.1.0 [view commit logs](https://github.com/soomla/unity3d-levelup/compare/v1.0.18...v1.1.0)

* Features
  * Added checking for latest version in Soomla Settings panel
  * Added "Remove Soomla" to Soomla menu
  * Added a new way of event handling without prefabs

* Fixes
  * Minor fixes to Level

* Changes
  * Moved all SOOMLA plugins into 'Soomla' folders

### v1.0.18 [view commit logs](https://github.com/soomla/unity3d-levelup/compare/v1.0.17...v1.0.18)

* Fixes
  * Can't read DB_WORLD_KEY_PREFIX from real device

### v1.0.17 [view commit logs](https://github.com/soomla/unity3d-levelup/compare/v1.0.16...v1.0.17)

* Changes
  * Added clean up progress in LevelUp

### v1.0.16 [view commit logs](https://github.com/soomla/unity3d-levelup/compare/v1.0.15...v1.0.16)

* Changes
  * Supporting changes in submodules

### v1.0.15 [view commit logs](https://github.com/soomla/unity3d-levelup/compare/v1.0.14...v1.0.15)

* Changes
  * New build way

### v1.0.14 [view commit logs](https://github.com/soomla/unity3d-levelup/compare/v1.0.13...v1.0.14)

* Changes
  * Merged with latest versions of all submodules. See their changelog.md for details.

### v1.0.13 [view commit logs](https://github.com/soomla/unity3d-levelup/compare/v1.0.11...v1.0.13)

* Fixes
  * Making list gates attach
  * GetGate function now checks children of InitialWorld.Gate GateList (thanks @altunsercan)
  * Fixed issue where calling world.SumInnerWorldsRecords() will result in a NullReferenceException if any level of the hierarchy doesn't contain a score. (thanks @FanerYedermann)
  * Fixed bug where android storage returns -1 if not yet stored once. (thanks @FanerYedermann)

* Changes
  * Get methods on ScoreStorage, now retrieve -1 when there is no value, summing methods enhanced accordingly.

* New Features
  * Added recursive sum of all scores in a world and its inner worlds (thanks @FanerYedermann)


### v1.0.11 [view commit logs](https://github.com/soomla/unity3d-levelup/compare/v1.0.10...v1.0.11)

* Changes
  * Updated to Unity 5 compatible submodules

### v1.0.10 [view commit logs](https://github.com/soomla/unity3d-levelup/compare/v1.0.9...v1.0.10)

* Changes
  * Updated submodules
  * Added post build script for core

### v1.0.9 [view commit logs](https://github.com/soomla/unity3d-levelup/compare/v1.0.8...v1.0.9)

* New Features
  * Adding ParentWorld property to World
  * Last completed inner world ID getter and event

### v1.0.8 [view commit logs](https://github.com/soomla/unity3d-levelup/compare/v1.0.7...v1.0.8)

* Fixes
  * Fixing GatesList creation
  * Fixing score not saving at init

* Changes
  * Updated new features from submodules

### v1.0.7 [view commit logs](https://github.com/soomla/unity3d-levelup/compare/v1.0.6...v1.0.7)

* Changes
  * Updated new features from submodules

### v1.0.6 [view commit logs](https://github.com/soomla/unity3d-levelup/compare/v1.0.5...v1.0.6)

* Changes
  * Updated new features from submodules

### v1.0.5 [view commit logs](https://github.com/soomla/unity3d-levelup/compare/v1.0.4...v1.0.5)

* New Features
  * Added event when latest score is changed
  * Added event when gate is closing
  *
* Fixes
  * Attaching gate to events only when needed
  * WorldStorage should set completed each time

### v1.0.4 [view commit logs](https://github.com/soomla/unity3d-levelup/compare/v1.0.3...v1.0.4)

* Fixes
  * Fixed issues with missions and scores

* New Features
  * Updated new features from submodules

### v1.0.3 [view commit logs](https://github.com/soomla/unity3d-levelup/compare/v1.0.2...v1.0.3)

* Changes
  * Updated changes from all submodules

* New Features
  * Added saving of times a level was completed

### v1.0.2 [view commit logs](https://github.com/soomla/unity3d-levelup/compare/v1.0.1...v1.0.2)

* Fixes
  * Fixing issues: timers not reset when calling end(false), unable to call end after pause
  * Fixed the small level test in BasicTest.cs

* Updated all submodules to their newest version.
