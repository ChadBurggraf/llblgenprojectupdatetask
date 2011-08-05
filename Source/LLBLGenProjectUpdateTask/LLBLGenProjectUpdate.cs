//-----------------------------------------------------------------------
// <copyright file="LLBLGenProjectUpdate.cs" company="Tasty Codes">
//     Copyright (c) 2011 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------

namespace LLBLGenProjectUpdateTask
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Security.Permissions;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    /// <summary>
    /// Provides update functionality to LLBLGen Pro v3.x projects from MSBuild.
    /// </summary>
    [HostProtectionAttribute(SecurityAction.LinkDemand, SharedState = true, Synchronization = true, ExternalProcessMgmt = true, SelfAffectingProcessMgmt = true)]
    [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "I didn't make up such an absurd name.")]
    public sealed class LLBLGenProjectUpdate : Task
    {
        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns>True if the task succeeded, false otherwise.</returns>
        [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
        public override bool Execute()
        {
            throw new NotImplementedException();
        }
    }
}
