
#import "UnityLevelUpEventDispatcher.h"
#import "UnityCommons.h"
#import "SoomlaUtils.h"

#import "Gate.h"
#import "GateStorage.h"
#import "Level.h"
#import "LevelStorage.h"
#import "Mission.h"
#import "MissionStorage.h"
#import "Score.h"
#import "ScoreStorage.h"
#import "World.h"
#import "WorldStorage.h"


extern "C" {

    void gateStorage_SetOpen(const char* gateId, bool open, bool notify) {
        NSString* gateIdS = [NSString stringWithUTF8String:gateId];
        [GateStorage setOpen:open forGate:gateIdS andEvent:notify];
    }

    bool gateStorage_IsOpen(const char* gateId) {
        NSString* gateIdS = [NSString stringWithUTF8String:gateId];
        return [GateStorage isOpen:gateIdS];
    }
    
    
	void levelStorage_SetSlowestDurationMillis(const char* levelId, long long duration) {
        NSString* levelIdS = [NSString stringWithUTF8String:levelId];
        [LevelStorage setSlowestDurationMillis:duration forLevel:levelIdS];
    }
	
	long long levelStorage_GetSlowestDurationMillis(const char* levelId) {
        NSString* levelIdS = [NSString stringWithUTF8String:levelId];
        return [LevelStorage getSlowestDurationMillisForLevel:levelIdS];
    }

	void levelStorage_SetFastestDurationMillis(const char* levelId, long long duration) {
        NSString* levelIdS = [NSString stringWithUTF8String:levelId];
        [LevelStorage setFastestDurationMillis:duration forLevel:levelIdS];
    }
	
	long long levelStorage_GetFastestDurationMillis(const char* levelId) {
        NSString* levelIdS = [NSString stringWithUTF8String:levelId];
        return [LevelStorage getFastestDurationMillisForLevel:levelIdS];
    }
	
	int levelStorage_IncTimesStarted(const char* levelId) {
        NSString* levelIdS = [NSString stringWithUTF8String:levelId];
        return [LevelStorage incTimesStartedForLevel:levelIdS];
    }
	
	int levelStorage_DecTimesStarted(const char* levelId) {
        NSString* levelIdS = [NSString stringWithUTF8String:levelId];
        return [LevelStorage decTimesStartedForLevel:levelIdS];

    }
	
	int levelStorage_GetTimesStarted(const char* levelId) {
        NSString* levelIdS = [NSString stringWithUTF8String:levelId];
        return [LevelStorage getTimesStartedForLevel:levelIdS];

    }
	
	int levelStorage_IncTimesPlayed(const char* levelId) {
        NSString* levelIdS = [NSString stringWithUTF8String:levelId];
        return [LevelStorage incTimesPlayedForLevel:levelIdS];

    }
	
	int levelStorage_DecTimesPlayed(const char* levelId) {
        NSString* levelIdS = [NSString stringWithUTF8String:levelId];
        return [LevelStorage decTimesPlayedForLevel:levelIdS];

    }
	
	int levelStorage_GetTimesPlayed(const char* levelId) {
        NSString* levelIdS = [NSString stringWithUTF8String:levelId];
        return [LevelStorage getTimesPlayedForLevel:levelIdS];
    }
    
    void missionStorage_SetCompleted(const char* missionId, bool completed, bool notify) {
        NSString* missionIdS = [NSString stringWithUTF8String:missionId];
        [MissionStorage setCompleted:completed forMission:missionIdS andNotify:notify];
    }
    
    int missionStorage_GetTimesCompleted(const char* missionId) {
        NSString* missionIdS = [NSString stringWithUTF8String:missionId];
        return [MissionStorage getTimesCompleted:missionIdS];
    }
    
    
	void scoreStorage_SetLatestScore(const char* scoreId, double latest) {
        NSString* scoreIdS = [NSString stringWithUTF8String:scoreId];
        [ScoreStorage setLatest:latest toScore:scoreIdS];
    }
	
	double scoreStorage_GetLatestScore(const char* scoreId) {
        NSString* scoreIdS = [NSString stringWithUTF8String:scoreId];
        return [ScoreStorage getLatestScore:scoreIdS];
    }

	void scoreStorage_SetRecordScore(const char* scoreId, double record) {
        NSString* scoreIdS = [NSString stringWithUTF8String:scoreId];
        [ScoreStorage setRecord:record toScore:scoreIdS];

    }

	double scoreStorage_GetRecordScore(const char* scoreId) {
        NSString* scoreIdS = [NSString stringWithUTF8String:scoreId];
        return [ScoreStorage getRecordScore:scoreIdS];

    }
    
    void worldStorage_SetCompleted(const char* worldId, bool completed, bool notify) {
        NSString* worldIdS = [NSString stringWithUTF8String:worldId];
        [WorldStorage setCompleted:completed forWorld:worldIdS andNotify:notify];
    }
    
    bool worldStorage_IsCompleted(const char* worldId) {
        NSString* worldIdS = [NSString stringWithUTF8String:worldId];
        return [WorldStorage isWorldCompleted:worldIdS];
    }
    
    void worldStorage_GetAssignedReward(const char* worldId, char** json) {
        NSString* worldIdS = [NSString stringWithUTF8String:worldId];
        NSString* rewardId = [WorldStorage getAssignedReward:worldIdS];
        if (!rewardId) {
            rewardId = @"";
        }
        
        *json = Soom_AutonomousStringCopy([rewardId UTF8String]);
    }
    
    void worldStorage_SetReward(const char* worldId, const char* rewardId) {
        NSString* worldIdS = [NSString stringWithUTF8String:worldId];
        NSString* rewardIdS = [NSString stringWithUTF8String:rewardId];
        [WorldStorage setReward:rewardIdS forWorld:worldIdS];
    }
}
