using System.Reflection;

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
[assembly: AssemblyVersion("1.0.1.$WCREV$")]
[assembly: AssemblyFileVersion("1.0.1.0")]
[assembly: AssemblyDefaultAlias("$WCDATE$")]