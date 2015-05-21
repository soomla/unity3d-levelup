### v1.0.12 [view commit logs](https://github.com/soomla/unity3d-levelup/compare/v1.0.11...v1.0.12)

* Fixes
  * Making list gates attach
  * GetGate function now checks children of InitialWorld.Gate GateList (thanks @altunsercan)
  * Fixed issue where calling world.SumInnerWorldsRecords() will result in a NullReferenceException if any level of the hierarchy doesn't contain a score. (thanks @FanerYedermann)
  * Fixed bug where android storage returns -1 if not yet stored once. (thanks @FanerYedermann)

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
