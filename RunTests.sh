# Executing Unit Tests via command line
# Command Structure: UNITY_EXECUTABLE_PATH -batchmode -projectPath PATH_TO_YOUR_PROJECT -executeMethod UnityTest.Batch.RunUnitTests -resultFilePath=PATH_TO_RES_FILE
# By default, the following output file will be created: ./Soomla/UnitTestResults.xml

/Applications/Unity/Unity.app/Contents/MacOS/Unity -batchmode -executeMethod UnityTest.Batch.RunUnitTests 
