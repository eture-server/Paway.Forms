@echo off
@set ILMerge="C:\Program Files (x86)\Microsoft\ILMerge\ILMerge.exe"
@set VS_HOME="C:\Program Files (x86)\Microsoft Visual Studio 11.0\VC\vcvarsall.bat"
@set POSTBUILD_HOME="C:\Program Files (x86)\Xenocode\Postbuild 2010 for .NET\XBuild.exe"

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

@echo.
@echo ------------------------------------------------------------
@echo �������� Paway.Forms...
@echo ------------------------------------------------------------
%POSTBUILD_HOME% Paway.Forms.postbuild /allstrings /o "%OUT%"
xcopy %BIN%\Paway.Forms.xml	"%OUT%\"	/I /R /Y /Q

@echo.
@echo ==============================================================================
@echo Paway ������ϡ�
@echo.

@pause
