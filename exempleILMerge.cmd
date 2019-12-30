::仅合并
@echo off
@set ILMerge="C:\Program Files (x86)\Microsoft\ILMerge\ILMerge.exe"

@set BIN=bin\Release
@set OUT=..\Fuscated
cd "%BIN%"
@echo.
@echo ------------------------------------------------------------
@echo 合并程序集 Paway.Forms...
@echo ------------------------------------------------------------
@del /S /Q "Paway.Forms.temp.dll"
rename Paway.Forms.dll Paway.Forms.temp.dll
%ILMerge% /log:merge.log /target:library /keyfile:"../../Mobot.Z.snk" /v4 ^
/out:Paway.Forms.dll ^
Paway.Forms.temp.dll ^
Paway.Core.dll ^
Paway.Helper.dll ^
Paway.Utils.dll ^
Paway.Win32.dll
xcopy Paway.Forms.dll		"%OUT%\"	/I /R /Y /Q

@echo.
@echo ==============================================================================
@echo Paway 更新完毕。
@echo.

@pause
