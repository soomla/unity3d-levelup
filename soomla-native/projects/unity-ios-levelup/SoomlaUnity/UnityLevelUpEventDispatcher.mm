
#import "UnityLevelUpEventDispatcher.h"
#import "LevelUpEventHandling.h"
#import "Gate.h"
#import "Level.h"
#import "World.h"
#import "Mission.h"
#import "Score.h"
#import "SoomlaUtils.h"

@implementation UnityLevelUpEventDispatcher

+ (void)initialize {
    static UnityLevelUpEventDispatcher* instance = nil;
    if (!instance) {
        instance = [[UnityLevelUpEventDispatcher alloc] init];
    }
}

- (id) init {
    if (self = [super init]) {
        [LevelUpEventHandling observeAllEventsWithObserver:self withSelector:@selector(handleEvent:)];
    }

    return self;
}

- (void)handleEvent:(NSNotification*)notification{

	if ([notification.name isEqualToString:EVENT_GATE_OPENED]) {
        Gate* gate = [[notification userInfo] objectForKey:DICT_ELEMENT_GATE];
        NSString* gateJson = [SoomlaUtils dictToJsonString:[gate toDictionary]];
        UnitySendMessage("LevelUpEvents", "onGateOpended", [gateJson UTF8String]);
	}
	else if ([notification.name isEqualToString:EVENT_LEVEL_ENDED]) {
        Level* level = [[notification userInfo] objectForKey:DICT_ELEMENT_LEVEL];
        NSString* levelJson = [SoomlaUtils dictToJsonString:[level toDictionary]];
        UnitySendMessage("LevelUpEvents", "onLevelEnded", [levelJson UTF8String]);
	}
	else if ([notification.name isEqualToString:EVENT_LEVEL_STARTED]) {
	    Level* level = [[notification userInfo] objectForKey:DICT_ELEMENT_LEVEL];
        NSString* levelJson = [SoomlaUtils dictToJsonString:[level toDictionary]];
        UnitySendMessage("LevelUpEvents", "onLevelStarted", [levelJson UTF8String]);
	}
	else if ([notification.name isEqualToString:EVENT_MISSION_COMPLETED]) {
	    Mission* mission = [[notification userInfo] objectForKey:DICT_ELEMENT_MISSION];
        NSString* missionJson = [SoomlaUtils dictToJsonString:[mission toDictionary]];
        UnitySendMessage("LevelUpEvents", "onMissionCompleted", [missionJson UTF8String]);
    }
	else if ([notification.name isEqualToString:EVENT_MISSION_COMPLETION_REVOKED]) {
        Mission* mission = [[notification userInfo] objectForKey:DICT_ELEMENT_MISSION];
        NSString* missionJson = [SoomlaUtils dictToJsonString:[mission toDictionary]];
        UnitySendMessage("LevelUpEvents", "onMissionCompletionRevoked", [missionJson UTF8String]);

    }
	else if ([notification.name isEqualToString:EVENT_SCORE_RECORD_CHANGED]) {
        Score* score = [[notification userInfo] objectForKey:DICT_ELEMENT_SCORE];
        NSString* scoreJson = [SoomlaUtils dictToJsonString:[score toDictionary]];
        UnitySendMessage("LevelUpEvents", "onScoreRecordChanged", [scoreJson UTF8String]);
    }
	else if ([notification.name isEqualToString:EVENT_WORLD_COMPLETED]) {
        World* world = [[notification userInfo] objectForKey:DICT_ELEMENT_WORLD];
        NSString* worldJson = [SoomlaUtils dictToJsonString:[world toDictionary]];
        UnitySendMessage("LevelUpEvents", "onWorldCompleted", [worldJson UTF8String]);
    }
    else if ([notification.name isEqualToString:EVENT_WORLD_BADGE_ASSIGNED]) {
        World* world = [[notification userInfo] objectForKey:DICT_ELEMENT_WORLD];
        NSString* worldJson = [SoomlaUtils dictToJsonString:[world toDictionary]];
        UnitySendMessage("LevelUpEvents", "onWorldBadgeAssigned", [worldJson UTF8String]);
    }
}

@end
