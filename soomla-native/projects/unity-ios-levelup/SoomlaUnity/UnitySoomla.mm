#import "SoomlaConfig.h"
#import "Soomla.h"
#import "UnitySoomlaLevelUpEventDispatcher.h"

extern "C"{

    void soomla_SetLogDebug(bool debug) {
        DEBUG_LOG = debug;
    }

	void soomla_Init(const char* secret){
        [UnitySoomlaLevelUpEventDispatcher initialize];
        
		[Soomla initializeWithSecret:[NSString stringWithUTF8String:secret]];
	}
}