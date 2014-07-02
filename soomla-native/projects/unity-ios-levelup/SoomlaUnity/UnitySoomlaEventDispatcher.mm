
#import "UnitySoomlaEventDispatcher.h"
#import "SoomlaEventHandling.h"
#import "Reward.h"
#import "SoomlaUtils.h"

@implementation UnitySoomlaEventDispatcher

+ (void)initialize {
    static UnitySoomlaEventDispatcher* instance = nil;
    if (!instance) {
        instance = [[UnitySoomlaEventDispatcher alloc] init];
    }
}

- (id) init {
    if (self = [super init]) {
        [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(rewardGiven:) name:EVENT_REWARD_GIVEN object:nil];
        [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(rewardTaken:) name:EVENT_REWARD_TAKEN object:nil];
    }

    return self;
}


- (void)rewardGiven:(NSNotification*)notification{
    Reward* reward = notification.userInfo[DICT_ELEMENT_REWARD];
    NSDictionary *dict = [reward toDictionary];
    UnitySendMessage("CoreEvents", "onRewardGiven", [[SoomlaUtils dictToJsonString:dict] UTF8String]);
}

- (void)rewardTaken:(NSNotification*)notification{
    Reward* reward = notification.userInfo[DICT_ELEMENT_REWARD];
    NSDictionary *dict = [reward toDictionary];
    UnitySendMessage("CoreEvents", "onRewardTaken", [[SoomlaUtils dictToJsonString:dict] UTF8String]);
}


@end
