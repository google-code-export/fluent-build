@Echo Off

set pathtorar="C:\Program Files\WinRAR\winrar.exe"
set basepath="C:\Projects\fluent-build\Plugins\TeamCity\fluentbuild-runner\out\artifacts"
set plugindir="C:\Users\Kudos\.BuildServer\plugins"

echo "Delete old zips"
del %basepath%\agent\fluentbuild-runner.zip
del %basepath%\fluentbuild-runner.zip

echo "Zip Agent"
rem a=archive, afzip=zip, r=recursive, ep1=exclude root folder names from archive
%pathtorar% a -afzip -r -ep1 %basepath%\agent\fluentbuild-runner.zip  %basepath%\agent\*.*

rem delete the dir
rmdir /S /Q %basepath%\agent\fluentbuild-runner

echo "Zip Plugin"
rem a=archive, afzip=zip, r=recursive, ep1=exclude root folder names from archive
%pathtorar% a -afzip -r -ep1 %basepath%\fluentbuild-runner.zip  %basepath%\*.*

echo "Stop Services"
net stop TeamCity
net stop TCBuildAgent

echo "Clean Plugin"
del /Q %plugindir%\fluentbuild-runner.zip
rmdir /S /Q %plugindir%\.unpacked\fluent-build

echo "Copy Plugin"
copy %basepath%\fluentbuild-runner.zip %plugindir%\fluentbuild-runner.zip

echo "Delete Agent Log"
del /Q "C:\TeamCity\buildAgent\logs\teamcity-agent.log"

echo "Start Services"
net start TeamCity
net start TCBuildAgent

