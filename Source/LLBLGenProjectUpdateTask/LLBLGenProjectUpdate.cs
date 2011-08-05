//-----------------------------------------------------------------------
// <copyright file="LLBLGenProjectUpdate.cs" company="Tasty Codes">
//     Copyright (c) 2011 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------

namespace LLBLGenProjectUpdateTask
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
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
        private LLBLGenProjectUpdateAction parsedAction;

        /// <summary>
        /// Initializes a new instance of the LLBLGenProjectUpdate class.
        /// </summary>
        public LLBLGenProjectUpdate()
        {
            this.Action = LLBLGenProjectUpdateAction.TypeShortcuts.ToString();
        }

        /// <summary>
        /// Gets or sets the action to perform.
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets a collection of LLBLGen project file paths to update.
        /// </summary>
        public ITaskItem[] Projects { get; set; }

        /// <summary>
        /// Gets or sets a collection of destination paths to save the updated project files to, if applicable.
        /// </summary>
        public ITaskItem[] TargetProjects { get; set; }

        /// <summary>
        /// Gets or sets a collection of type shortcut replacement definitions.
        /// </summary>
        public ITaskItem[] TypeShortcuts { get; set; }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns>True if the task succeeded, false otherwise.</returns>
        [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
        public override bool Execute()
        {
            bool success = false;

            if (this.IsValid())
            {
                success = true;

                if (this.Projects != null)
                {
                    if (this.TargetProjects == null || this.TargetProjects.Length == 0 || this.TargetProjects.Length == this.Projects.Length)
                    {
                        for (int i = 0; i < this.Projects.Length; i++)
                        {
                            if (success)
                            {
                                var project = this.Projects[i];
                                string inputPath = project.GetMetadata("FullPath");

                                if (File.Exists(inputPath))
                                {
                                    string outputPath = this.TargetProjects != null && this.TargetProjects.Length > 0 ?
                                        this.TargetProjects[i].GetMetadata("FullPath") :
                                        inputPath;

                                    switch (this.parsedAction)
                                    {
                                        case LLBLGenProjectUpdateAction.TypeShortcuts:
                                            success = this.ExecuteTypeShortcuts(inputPath, outputPath);
                                            break;
                                        default:
                                            Log.LogError("Unknown action: '{0}'.", this.parsedAction);
                                            success = false;
                                            break;
                                    }
                                }
                                else
                                {
                                    Log.LogError("There is not project file at '{0}'.", inputPath);
                                    success = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        Log.LogError("When specifying target projects, the set of paths must be the same length as the set of input project paths.");
                        success = false;
                    }
                }
            }

            return success;
        }

        /// <summary>
        /// Executs the type shortcuts action.
        /// </summary>
        /// <param name="inputPath">The input project file path.</param>
        /// <param name="outputPath">The output project file path.</param>
        /// <returns>True if the action succeeded, false otherwise.</returns>
        private bool ExecuteTypeShortcuts(string inputPath, string outputPath)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a value indicating whether this instance is in a valid state for execution.
        /// </summary>
        /// <returns>True if this instance is valid, false otherwise.</returns>
        private bool IsValid()
        {
            bool valid = true;

            try
            {
                this.parsedAction = (LLBLGenProjectUpdateAction)Enum.Parse(typeof(LLBLGenProjectUpdateAction), this.Action, true);
            }
            catch (ArgumentException)
            {
                this.LogInvalidActionError();
                valid = false;
            }
            catch (OverflowException)
            {
                this.LogInvalidActionError();
                valid = false;
            }

            return valid;
        }

        /// <summary>
        /// Logs the invalid action error to the current log.
        /// </summary>
        private void LogInvalidActionError()
        {
            Log.LogError("Action is required and must be one of: {0}.", String.Join(", ", Enum.GetNames(typeof(LLBLGenProjectUpdateAction))));
        }
    }
}
