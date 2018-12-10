@ECHO OFF

SET RUNTIME=%1

IF "%RUNTIME%"=="" (
  ECHO Error: No runtime provided! 1>&2
  ECHO Supported runtimes: 1>&2
  ECHO - win-x64 1>&2
  ECHO - linux-x64 1>&2
  ECHO - linux-arm 1>&2
  EXIT /b -1
)

SET WD=%~dp0

IF EXIST %WD%\..\publish\%RUNTIME% (
  RMDIR %WD%\..\publish\%RUNTIME% /S /Q
)

dotnet publish %WD%\..\Books.csproj -c Release -r %RUNTIME% -o publish\%RUNTIME% --self-contained
if ERRORLEVEL 1 (
   EXIT /b %ERRORLEVEL%
)

IF EXIST %WD%\..\publish\%RUNTIME%\publish (
  RMDIR %WD%\..\publish\%RUNTIME%\publish /S /Q
)

:EXIT