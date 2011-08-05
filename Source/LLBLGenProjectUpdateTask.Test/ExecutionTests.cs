//-----------------------------------------------------------------------
// <copyright file="ExecutionTests.cs" company="Tasty Codes">
//     Copyright (c) 2011 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------

namespace LLBLGenProjectUpdateTask.Test
{
    using System;
    using Microsoft.Build.BuildEngine;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Execution tests.
    /// </summary>
    [TestClass]
    public sealed class ExecutionTests
    {
        /// <summary>
        /// Invalid action tests.
        /// </summary>
        [TestMethod]
        public void ExecutionInvalidAction()
        {
            var engineMock = new Mock<IBuildEngine>();

            var task = new LLBLGenProjectUpdate();
            task.BuildEngine = engineMock.Object;
            task.Action = null;
            Assert.IsFalse(task.Execute());

            task = new LLBLGenProjectUpdate();
            task.BuildEngine = engineMock.Object;
            task.Action = "Not an action";
            Assert.IsFalse(task.Execute());
        }
    }
}
