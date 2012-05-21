tools\OpenCover\OpenCover.Console.exe -target:"tools\nunit\nunit-console.exe" -targetargs:"/nologo /noshadow src\FluentBuild\bin\Debug\FluentBuild.dll" -register:user -output:compile\results.xml -filter:"+[*]* -[*]*Tests" 
tools\ReportGenerator\ReportGenerator.exe -reports:compile\results.xml -targetDir:compile\Report
taskkill /f /im nunit-agent.exe
start compile\Report\index.htm