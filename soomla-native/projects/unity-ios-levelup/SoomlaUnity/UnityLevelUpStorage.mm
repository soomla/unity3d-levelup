
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

char* AutonomousStringCopy (const char* string)
{
    if (string == NULL)
        return NULL;
    
    char* res = (char*)malloc(strlen(string) + 1);
    strcpy(res, string);
    return res;
}

extern "C" {

    void gateStorage_SetOpen(const char* sGateJson, bool open, bool notify) {
        NSString* gateJson = [NSString stringWithUTF8String:sGateJson];
        NSDictionary* gateDict = [SoomlaUtils jsonStringToDict:gateJson];
        Gate* gate = [Gate fromDictionary:gateDict];
        return [GateStorage setOpen:open forGate:gate andEvent:notify];
    }

    bool gateStorage_IsOpen(const char* sGateJson) {
        NSString* gateJson = [NSString stringWithUTF8String:sGateJson];
        NSDictionary* gateDict = [SoomlaUtils jsonStringToDict:gateJson];
        Gate* gate = [Gate fromDictionary:gateDict];
        return [GateStorage isOpen:gate];
    }
    
    
	void levelStorage_SetSlowestDuration(const char* sLevelJson, double duration) {
        NSString* levelJson = [NSString stringWithUTF8String:sLevelJson];
        NSDictionary* levelDict = [SoomlaUtils jsonStringToDict:levelJson];
        Level* level = (Level*)[Level fromDictionary:levelDict];
        return [LevelStorage setSlowestDuration:duration forLevel:level];
    }
	
	int levelStorage_GetSlowestDuration(const char* sLevelJson) {
        NSString* levelJson = [NSString stringWithUTF8String:sLevelJson];
        NSDictionary* levelDict = [SoomlaUtils jsonStringToDict:levelJson];
        Level* level = (Level*)[Level fromDictionary:levelDict];
        return [LevelStorage getSlowestDurationForLevel:level];
    }

	void levelStorage_SetFastestDuration(const char* sLevelJson, double duration) {
        NSString* levelJson = [NSString stringWithUTF8String:sLevelJson];
        NSDictionary* levelDict = [SoomlaUtils jsonStringToDict:levelJson];
        Level* level = (Level*)[Level fromDictionary:levelDict];
        return [LevelStorage setFastestDuration:duration forLevel:level];
    }
	
	int levelStorage_GetFastestDuration(const char* sLevelJson) {
        NSString* levelJson = [NSString stringWithUTF8String:sLevelJson];
        NSDictionary* levelDict = [SoomlaUtils jsonStringToDict:levelJson];
        Level* level = (Level*)[Level fromDictionary:levelDict];
        return [LevelStorage getFastestDurationforLevel:level];
    }
	
	int levelStorage_IncTimesStarted(const char* sLevelJson) {
        NSString* levelJson = [NSString stringWithUTF8String:sLevelJson];
        NSDictionary* levelDict = [SoomlaUtils jsonStringToDict:levelJson];
        Level* level = (Level*)[Level fromDictionary:levelDict];
        return [LevelStorage incTimesStartedForLevel:level];
    }
	
	int levelStorage_DecTimesStarted(const char* sLevelJson) {
        NSString* levelJson = [NSString stringWithUTF8String:sLevelJson];
        NSDictionary* levelDict = [SoomlaUtils jsonStringToDict:levelJson];
        Level* level = (Level*)[Level fromDictionary:levelDict];
        return [LevelStorage decTimesStartedForLevel:level];

    }
	
	int levelStorage_GetTimesStarted(const char* sLevelJson) {
        NSString* levelJson = [NSString stringWithUTF8String:sLevelJson];
        NSDictionary* levelDict = [SoomlaUtils jsonStringToDict:levelJson];
        Level* level = (Level*)[Level fromDictionary:levelDict];
        return [LevelStorage getTimesStartedForLevel:level];

    }
	
	int levelStorage_IncTimesPlayed(const char* sLevelJson) {
        NSString* levelJson = [NSString stringWithUTF8String:sLevelJson];
        NSDictionary* levelDict = [SoomlaUtils jsonStringToDict:levelJson];
        Level* level = (Level*)[Level fromDictionary:levelDict];
        return [LevelStorage incTimesPlayedForLevel:level];

    }
	
	int levelStorage_DecTimesPlayed(const char* sLevelJson) {
        NSString* levelJson = [NSString stringWithUTF8String:sLevelJson];
        NSDictionary* levelDict = [SoomlaUtils jsonStringToDict:levelJson];
        Level* level = (Level*)[Level fromDictionary:levelDict];
        return [LevelStorage decTimesPlayedForLevel:level];

    }
	
	int levelStorage_GetTimesPlayed(const char* sLevelJson) {
        NSString* levelJson = [NSString stringWithUTF8String:sLevelJson];
        NSDictionary* levelDict = [SoomlaUtils jsonStringToDict:levelJson];
        Level* level = (Level*)[Level fromDictionary:levelDict];
        return [LevelStorage getTimesPlayedForLevel:level];
    }
    
    void missionStorage_SetCompleted(const char* sMissionJson, bool completed, bool notify) {
        NSString* missionJson = [NSString stringWithUTF8String:sMissionJson];
        NSDictionary* missionDict = [SoomlaUtils jsonStringToDict:missionJson];
        Mission* mission = [Mission fromDictionary:missionDict];
        return [MissionStorage setCompleted:completed forMission:mission andNotify:notify];
    }
    
    bool missionStorage_IsCompleted(const char* sMissionJson) {
        NSString* missionJson = [NSString stringWithUTF8String:sMissionJson];
        NSDictionary* missionDict = [SoomlaUtils jsonStringToDict:missionJson];
        Mission* mission = [Mission fromDictionary:missionDict];
        return [MissionStorage isMissionCompleted:mission];
    }
    
    
	void scoreStorage_SetLatestScore(const char* sScoreJson, double latest) {
        NSString* scoreJson = [NSString stringWithUTF8String:sScoreJson];
        NSDictionary* scoreDict = [SoomlaUtils jsonStringToDict:scoreJson];
        Score* score = [Score fromDictionary:scoreDict];
        return [ScoreStorage setLatest:latest toScore:score];

    }
	
	int scoreStorage_GetLatestScore(const char* sScoreJson) {
        NSString* scoreJson = [NSString stringWithUTF8String:sScoreJson];
        NSDictionary* scoreDict = [SoomlaUtils jsonStringToDict:scoreJson];
        Score* score = [Score fromDictionary:scoreDict];
        return [ScoreStorage getLatestScore:score];
    }

	void scoreStorage_SetRecordScore(const char* sScoreJson, double record) {
        NSString* scoreJson = [NSString stringWithUTF8String:sScoreJson];
        NSDictionary* scoreDict = [SoomlaUtils jsonStringToDict:scoreJson];
        Score* score = [Score fromDictionary:scoreDict];
        return [ScoreStorage setRecord:record toScore:score];

    }

	int scoreStorage_GetRecordScore(const char* sScoreJson) {
        NSString* scoreJson = [NSString stringWithUTF8String:sScoreJson];
        NSDictionary* scoreDict = [SoomlaUtils jsonStringToDict:scoreJson];
        Score* score = [Score fromDictionary:scoreDict];
        return [ScoreStorage getRecordScore:score];

    }
    
    void worldStorage_SetCompleted(const char* sWorldJson, bool completed, bool notify) {
        NSString* worldJson = [NSString stringWithUTF8String:sWorldJson];
        NSDictionary* worldDict = [SoomlaUtils jsonStringToDict:worldJson];
        World* world = [World fromDictionary:worldDict];
        return [WorldStorage setCompleted:completed forWorld:world andNotify:notify];
    }
    
    bool worldStorage_IsCompleted(const char* sWorldJson) {
        NSString* worldJson = [NSString stringWithUTF8String:sWorldJson];
        NSDictionary* worldDict = [SoomlaUtils jsonStringToDict:worldJson];
        World* world = [World fromDictionary:worldDict];
        return [WorldStorage isWorldCompleted:world];
    }
    
    void worldStorage_GetAssignedReward(const char* sWorldJson, char** json) {
        NSString* worldJson = [NSString stringWithUTF8String:sWorldJson];
        NSDictionary* worldDict = [SoomlaUtils jsonStringToDict:worldJson];
        World* world = [World fromDictionary:worldDict];
        NSString* rewardId = [WorldStorage getAssignedReward:world];
        if (!rewardId) {
            rewardId = @"";
        }
        
        *json = AutonomousStringCopy([rewardId UTF8String]);
    }
    
    void worldStorage_SetBadge(const char* sWorldJson, const char* sRewardId) {
        NSString* worldJson = [NSString stringWithUTF8String:sWorldJson];
        NSString* rewardId = [NSString stringWithUTF8String:sRewardId];
        NSDictionary* worldDict = [SoomlaUtils jsonStringToDict:worldJson];
        World* world = [World fromDictionary:worldDict];
        [WorldStorage setReward:rewardId forWorld:world];
    }
}
