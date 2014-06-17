@echo off
@set ILMerge="C:\Program Files (x86)\Microsoft\ILMerge\ILMerge.exe"
@set VS_HOME="C:\Program Files (x86)\Microsoft Visual Studio 11.0\VC\vcvarsall.bat"
@set POSTBUILD_HOME="C:\Program Files\Xenocode\Postbuild 2010 for .NET\XBuild.exe"

@echo off
@echo ����Ŀ¼���½ű�
@echo ==============================================================================
@set BIN=bin\Release
@set OUT=bin\Fuscated
SubWCRev .\ AssemblyVersion.tpl AssemblyVersion.cs

rd /S /Q "%BIN%"
rd /S /Q "%OUT%"
@echo.
@echo ------------------------------------------------------------
@echo ���ɽ������...
@echo ------------------------------------------------------------
@call %VS_HOME%
MSBuild Paway.Form.sln /t:Rebuild /p:Configuration=Release /nologo  /v:minimal

@set OUT=..\Fuscated
cd "%BIN%"
@echo.
@echo ------------------------------------------------------------
@echo �ϲ����� Paway.Windows.Forms...
@echo ------------------------------------------------------------
rename Paway.Forms.dll Paway.Forms.temp.dll
%ILMerge% /log:merge.log /target:library /keyfile:"../../Mobot.Z.snk" /v4 ^
/out:Paway.Forms.dll ^
Paway.Utils.dll ^
Paway.Forms.temp.dll ^
Paway.Helper.dll ^
Paway.Resource.dll ^
Paway.Win32.dll ^
Paway.Custom.dll
xcopy Paway.Forms.dll	"%OUT%\"	/I /R /Y /Q
xcopy Paway.Forms.xml	"%OUT%\"	/I /R /Y /Q

@echo.
@echo ==============================================================================
@echo Paway ������ϡ�
@echo.

@pause
