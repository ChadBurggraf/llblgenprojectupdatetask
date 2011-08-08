//-----------------------------------------------------------------------
// <copyright file="LLBLGenProjectUpdateAction.cs" company="Tasty Codes">
//     Copyright (c) 2011 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------

namespace LLBLGenProjectUpdateTask
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Defines the possible LLBLGenProjectUpdate task actions.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "I didn't make up such an absurd name.")]
    public enum LLBLGenProjectUpdateAction
    {
        /// <summary>
        /// Identifies the type shortcuts update action.
        /// </summary>
        TypeShortcuts
    }
}
