@echo off
@set ILMerge="C:\Program Files\Microsoft\ILMerge\ILMerge.exe"
@set VS_HOME="C:\Program Files\Microsoft Visual Studio 11.0\VC\vcvarsall.bat"
@set POSTBUILD_HOME="C:\Program Files\Xenocode\Postbuild 2010 for .NET\XBuild.exe"

@echo off
@echo 发布目录更新脚本
@echo ==============================================================================
@set BIN=bin\Release
@set OUT=bin\Fuscated
SubWCRev .\ AssemblyVersion.tpl AssemblyVersion.cs

rd /S /Q "%BIN%"
rd /S /Q "%OUT%"
@echo.
@echo ------------------------------------------------------------
@echo 生成解决方案...
@echo ------------------------------------------------------------
@call %VS_HOME%
MSBuild Paway.Form.sln /t:Rebuild /p:Configuration=Release /nologo  /v:minimal

::@set OUT=..\Fuscated
::cd "%BIN%"
@echo.
@echo ------------------------------------------------------------
@echo 合并程序集 Paway.Windows.Forms...
@echo ------------------------------------------------------------
::%ILMerge% /log:merge.log /target:library /keyfile:"../../Mobot.Z.snk" /v4 ^
::/out:Paway.Forms.dll ^
::Paway.Helper.dll ^
::Paway.Windows.Forms.dll ^
::Paway.Resource.dll ^
::Paway.Windows.Win32.dll
::xcopy Paway.Forms.dll	"%OUT%\"	/I /R /Y /Q
::xcopy Paway.Resource.dll	"%OUT%\"	/I /R /Y /Q
xcopy %BIN%\Paway.Forms.xml	"%OUT%\"	/I /R /Y /Q

@echo.
@echo ------------------------------------------------------------
@echo 混淆程序集 Paway.Forms...
@echo ------------------------------------------------------------
%POSTBUILD_HOME% Paway.Forms.postbuild /allstrings /o "%OUT%"

@echo.
@echo ==============================================================================
@echo Paway 更新完毕。
@echo.

@pause
