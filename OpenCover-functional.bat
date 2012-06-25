tools\OpenCover\OpenCover.Console.exe -target:"tools\nunit\nunit-console.exe" -targetargs:"/nologo /noshadow tests\FluentBuild.Tests\bin\Debug\FluentBuild.Tests.dll" -register:user -output:compile\results.xml -filter:"+[*]* -[*]*Tests" 
tools\ReportGenerator\ReportGenerator.exe -reports:compile\results.xml -targetDir:compile\Report
taskkill /f /im nunit-agent.exe
start compile\Report\index.htm