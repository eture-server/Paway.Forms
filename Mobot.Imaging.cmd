@echo off
@set POSTBUILD_HOME="C:\Program Files (x86)\Xenocode\Postbuild 2010 for .NET\XBuild.exe"

@echo off
@echo ����Ŀ¼���½ű�
@echo ==============================================================================
@set BIN=bin\Release
@set OUT=bin\Fuscated

@echo.
@echo ------------------------------------------------------------
@echo �������� Mobot.Imaging...
@echo ------------------------------------------------------------
%POSTBUILD_HOME% Mobot.Imaging.postbuild /allstrings /o "%OUT%"

@echo.
@echo ==============================================================================
@echo Paway ������ϡ�
@echo.

@pause
