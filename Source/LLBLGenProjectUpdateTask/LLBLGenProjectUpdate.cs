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
    using System.Text.RegularExpressions;
    using System.Xml;
    using System.Xml.XPath;
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
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", Justification = "Yeah, I don't plan on localizing this.")]
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
                                Log.LogMessage("Processing project at '{0}'.", inputPath);
                                
                                if (File.Exists(inputPath))
                                {
                                    XmlDocument projectDoc = new XmlDocument();

                                    try
                                    {
                                        projectDoc.Load(inputPath);
                                    }
                                    catch (XmlException)
                                    {
                                        Log.LogError("The project file at '{0}' is not a valid XML document.", inputPath);
                                        success = false;
                                    }
                                    catch (IOException)
                                    {
                                        Log.LogError("An I/O error occurred while opening the project file at '{0}'.", inputPath);
                                        success = false;
                                    }
                                    catch (UnauthorizedAccessException)
                                    {
                                        Log.LogError("Failed to open a read/write stream to the project file at '{0}'.", inputPath);
                                        success = false;
                                    }

                                    if (success)
                                    {
                                        switch (this.parsedAction)
                                        {
                                            case LLBLGenProjectUpdateAction.TypeShortcuts:
                                                success = this.ExecuteTypeShortcuts(projectDoc);
                                                break;
                                            default:
                                                Log.LogError("Unknown action: '{0}'.", this.parsedAction);
                                                success = false;
                                                break;
                                        }

                                        string outputPath = this.TargetProjects != null && this.TargetProjects.Length > 0 ?
                                            this.TargetProjects[i].GetMetadata("FullPath") :
                                            inputPath;

                                        projectDoc.Save(outputPath);
                                    }
                                }
                                else
                                {
                                    Log.LogError("There is no project file at '{0}'.", inputPath);
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
        /// Gets a value indicating whether the given <see cref="ITaskItem"/> defines
        /// the given metadata name.
        /// </summary>
        /// <param name="taskItem">The <see cref="ITaskItem"/> to check.</param>
        /// <param name="name">The metadata name to look for.</param>
        /// <returns>True if the metadata name is found, false otherwise.</returns>
        private static bool MetadataNameDefined(ITaskItem taskItem, string name)
        {
            foreach (string metadataName in taskItem.MetadataNames)
            {
                if (metadataName.Equals(name, StringComparison.Ordinal))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Merges the metadata type information from the given <see cref="ITaskItem"/>
        /// with the given <see cref="TypeName"/>, returning a new <see cref="TypeName"/> result instance.
        /// </summary>
        /// <param name="typeName">The <see cref="TypeName"/> to merge metadata with.</param>
        /// <param name="taskItem">The <see cref="ITaskItem"/> to get type metadata from.</param>
        /// <returns>A new, merged, <see cref="TypeName"/> instance.</returns>
        private static TypeName MergeWithMetadata(TypeName typeName, ITaskItem taskItem)
        {
            TypeName result = typeName.Clone();
            string value = (taskItem.GetMetadata("Assembly") ?? string.Empty).Trim();

            if (!string.IsNullOrEmpty(value))
            {
                result.Assembly = value;
            }

            if (MetadataNameDefined(taskItem, "RemoveAssembly"))
            {
                result.Assembly = null;
            }

            value = (taskItem.GetMetadata("Culture") ?? string.Empty).Trim();

            if (!string.IsNullOrEmpty(value))
            {
                result.Culture = value;
            }

            if (MetadataNameDefined(taskItem, "RemoveCulture"))
            {
                result.Culture = null;
            }

            value = (taskItem.GetMetadata("Name") ?? string.Empty).Trim();

            if (!string.IsNullOrEmpty(value))
            {
                result.Name = value;
            }

            if (MetadataNameDefined(taskItem, "RemoveName"))
            {
                result.Name = null;
            }

            value = (taskItem.GetMetadata("Namespace") ?? string.Empty).Trim();

            if (!string.IsNullOrEmpty(value))
            {
                result.Namespace = value;
            }

            if (MetadataNameDefined(taskItem, "RemoveNamespace"))
            {
                result.Namespace = null;
            }

            value = (taskItem.GetMetadata("PublicKeyToken") ?? string.Empty).Trim();

            if (!string.IsNullOrEmpty(value))
            {
                result.PublicKeyToken = value;
            }

            if (MetadataNameDefined(taskItem, "RemovePublicKeyToken"))
            {
                result.PublicKeyToken = null;
            }

            value = (taskItem.GetMetadata("Version") ?? string.Empty).Trim();

            if (!string.IsNullOrEmpty(value))
            {
                result.Version = new Version(value);
            }

            if (MetadataNameDefined(taskItem, "RemoveVersion"))
            {
                result.Version = null;
            }

            return result;
        }

        /// <summary>
        /// Executs the type shortcuts action.
        /// </summary>
        /// <param name="project">The project to operate on.</param>
        /// <returns>True if the action succeeded, false otherwise.</returns>
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", Justification = "Yeah, I don't plan on localizing this.")]
        private bool ExecuteTypeShortcuts(XmlDocument project)
        {
            bool success = true;
            
            if (this.TypeShortcuts != null)
            {
                foreach (var shortcut in this.TypeShortcuts)
                {
                    Regex exp = null;

                    try
                    {
                        exp = new Regex(shortcut.ItemSpec);
                    }
                    catch (ArgumentException)
                    {
                        Log.LogError("Type shortcut '{0}' did not parse into a valid regular expression.", shortcut.ItemSpec);
                        success = false;
                    }

                    if (exp != null)
                    {
                        foreach (XmlElement element in project.SelectNodes("/Project/TypeShortcuts/TypeShortcut"))
                        {
                            string shortcutType = element.Attributes["Type"].Value;

                            if (exp.IsMatch(shortcutType))
                            {
                                TypeName mergedTypeName = MergeWithMetadata(TypeName.Parse(shortcutType), shortcut);
                                element.SetAttribute("Type", mergedTypeName.ToString());
                                Log.LogMessage("Updated type '{0}' to '{1}'.", shortcutType, mergedTypeName);
                            }
                        }
                    }
                }
            }

            return success;
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
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", Justification = "Yeah, I don't plan on localizing this.")]
        private void LogInvalidActionError()
        {
            Log.LogError("Action is required and must be one of: {0}.", string.Join(", ", Enum.GetNames(typeof(LLBLGenProjectUpdateAction))));
        }
    }
}
