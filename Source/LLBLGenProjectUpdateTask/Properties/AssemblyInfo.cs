//-----------------------------------------------------------------------
// <copyright file="AssemblyInfo.cs" company="Tasty Codes">
//     Copyright (c) 2011 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("LLBLGenProjectUpdateTask")]
[assembly: AssemblyDescription("Provides update functionality to LLBLGen Pro v3.x projects from MSBuild.")]
[assembly: Guid("bfd96e0a-c51d-46a4-9128-658aaf1b8f22")]

#if DEBUG
[assembly: InternalsVisibleTo("LLBLGenProjectUpdateTask.Test")]
#endif