@echo off
@set POSTBUILD_HOME="C:\Program Files (x86)\Xenocode\Postbuild 2010 for .NET\XBuild.exe"

@echo off
@echo 发布目录更新脚本
@echo ==============================================================================
@set BIN=bin\Release
@set OUT=bin\Fuscated

@echo.
@echo ------------------------------------------------------------
@echo 混淆程序集 Mobot.Imaging...
@echo ------------------------------------------------------------
%POSTBUILD_HOME% Mobot.Imaging.postbuild /allstrings /o "%OUT%"

@echo.
@echo ==============================================================================
@echo Paway 更新完毕。
@echo.

@pause
