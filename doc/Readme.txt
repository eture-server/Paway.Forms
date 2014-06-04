手机自动化测试项目
================================================================================

目录说明
----------------------------------------
src	源码目录
dist	程序发布目录
doc	项目文档

编译说明
----------------------------------------
开发环境：Microsoft Visual Studio 2010
为方便开发调试，需要安装如下程序：
  KryptonSuite 4.31 (UI界面)

Xenocode PostBuild 混淆选项
----------------------------------------
1. 添加ComponentFactory.Krypton.Toolkit.dll/log4net，及本程序涉及的其他程序集；
2. 使用ClickOnce，单程序集模式；
3. 排除混淆log4net.*（所有类型和所有可见性的对象）；
4. 勾选“Rename All Managed Namespaces”和“Rename All Managed Namespaces In Non-Primary Assemblies”
5. 勾选“Enable output compression”启用压缩；
6. 勾选“Suppress ILDASM and other external reflection tools”选项；

--
范传根
