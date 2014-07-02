
#import <Foundation/Foundation.h>

@interface UnityLevelUpEventDispatcher : NSObject {
    
}
- (id)init;
- (void)handleEvent:(NSNotification*)notification;
+ (void)initialize;

@end
