::���ϲ�
@echo off
@set ILMerge="C:\Program Files (x86)\Microsoft\ILMerge\ILMerge.exe"

@set BIN=NOPI
@set OUT=..\Fuscated
cd "%BIN%"
@echo.
@echo ------------------------------------------------------------
@echo �ϲ����� NOPI...
@echo ------------------------------------------------------------
rename NPOI.dll NPOI.temp.dll
%ILMerge% /log:merge.log /target:library /keyfile:"../../Mobot.Z.snk" /v4 ^
/out:NPOI.dll ^
NPOI.temp.dll ^
NPOI.OOXML.dll ^
NPOI.OpenXml4Net.dll ^
NPOI.OpenXmlFormats.dll
xcopy NPOI.dll		"%OUT%\"	/I /R /Y /Q

@echo.
@echo ==============================================================================
@echo NPOI ������ϡ�
@echo.

@pause
