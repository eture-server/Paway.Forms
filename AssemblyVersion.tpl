using System.Reflection;

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
[assembly: AssemblyVersion("1.0.0.$WCREV$")]
[assembly: AssemblyFileVersion("1.0.0.$WCREV$")]
[assembly: AssemblyDefaultAlias("$WCDATE$")]