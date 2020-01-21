write-host "-------------------------------"
write-host "Konsole.IConsole"
write-host "-------------------------------"

dotnet build src/Konsole.sln --configuration Release

$packdir = 'C:\src\nuget\test-packages\IConsole'
$file = (gci 'src/Konsole/bin/Release' | select -last 1)
copy-item $file.fullname -d $packdir
Write-Host "'$file' : copied to '$packdir'"