set mypath=%cd%
echo %mypath%

set SEARCH_BASE="src\bin\Release"
set FILTER=*.DLL
for /r %SEARCH_BASE% %%i in (%FILTER%) do (
signtool verify /pa /q "%%i" 1>nul 2>nul 
if errorlevel 1 (
signtool sign /n "James Weston" /fd SHA256 /d "MeaMod.DNS" /du "https://www.meamod.com" /tr http://timestamp.digicert.com /td sha256 "%%i"
)
)

dotnet pack -c Release --no-build

nuget sign "src\bin\Release\*.nupkg" -CertificateSubjectName "James Weston" -Timestamper http://timestamp.digicert.com -NonInteractive

echo nuget push "src\bin\Release\*.nupkg" -Source nuget.org -SkipDuplicate -NonInteractive

move "src\bin\Release\*.nupkg" nugetpackagearchive
