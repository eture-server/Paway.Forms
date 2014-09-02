@echo off
@set POSTBUILD_HOME="C:\Program Files\Xenocode\Postbuild 2010 for .NET\XBuild.exe"

@set BIN=bin\Release
@set OUT=bin\Fuscated
rd /S /Q "%OUT%"
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
