//-----------------------------------------------------------------------
// <copyright file="TypeNameTests.cs" company="Tasty Codes">
//     Copyright (c) 2011 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------

namespace LLBLGenProjectUpdateTask.Test
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Type name tests.
    /// </summary>
    [TestClass]
    public sealed class TypeNameTests
    {
        /// <summary>
        /// Clone tests.
        /// </summary>
        [TestMethod]
        public void TypeNameClone()
        {
            TypeName tn = new TypeName()
            {
                Namespace = "SurveyEngine.DataTypeConverters",
                Name = "CalendarQuestionConstraintType",
                Assembly = "SurveyEngine.DataTypeConverters",
                Version = new Version("2.3.5.2"),
                Culture = "neutral",
                PublicKeyToken = "79cd5921ea88a364"
            };

            TypeName clone = tn.Clone();
            Assert.IsFalse(object.ReferenceEquals(tn, clone));
            Assert.AreEqual(tn.Assembly, clone.Assembly);
            Assert.AreEqual(tn.Culture, clone.Culture);
            Assert.AreEqual(tn.Name, clone.Name);
            Assert.AreEqual(tn.Namespace, clone.Namespace);
            Assert.AreEqual(tn.PublicKeyToken, clone.PublicKeyToken);
            Assert.AreEqual(tn.Version, clone.Version);
        }

        /// <summary>
        /// ToString() tests.
        /// </summary>
        [TestMethod]
        public void TypeNameToString()
        {
            TypeName tn = new TypeName()
            {
                Namespace = "SurveyEngine.DataTypeConverters",
                Name = "CalendarQuestionConstraintType",
                Assembly = "SurveyEngine.DataTypeConverters",
                Version = new Version("2.3.5.2"),
                Culture = "neutral",
                PublicKeyToken = "79cd5921ea88a364"
            };

            Assert.AreEqual("SurveyEngine.DataTypeConverters.CalendarQuestionConstraintType, SurveyEngine.DataTypeConverters, Version=2.3.5.2, Culture=neutral, PublicKeyToken=79cd5921ea88a364", tn.ToString());

            tn.PublicKeyToken = null;
            Assert.AreEqual("SurveyEngine.DataTypeConverters.CalendarQuestionConstraintType, SurveyEngine.DataTypeConverters, Version=2.3.5.2, Culture=neutral", tn.ToString());

            tn.Culture = null;
            Assert.AreEqual("SurveyEngine.DataTypeConverters.CalendarQuestionConstraintType, SurveyEngine.DataTypeConverters, Version=2.3.5.2", tn.ToString());

            tn.Version = null;
            Assert.AreEqual("SurveyEngine.DataTypeConverters.CalendarQuestionConstraintType, SurveyEngine.DataTypeConverters", tn.ToString());

            tn.Assembly = null;
            Assert.AreEqual(string.Empty, tn.ToString());

            tn.Assembly = "SurveyEngine.DataTypeConverters";
            tn.Name = null;
            Assert.AreEqual(string.Empty, tn.ToString());

            tn.Name = "CalendarQuestionConstraintType";
            tn.Namespace = null;
            Assert.AreEqual(string.Empty, tn.ToString());
        }

        /// <summary>
        /// Parse tests.
        /// </summary>
        [TestMethod]
        public void TypeNameParse()
        {
            TypeName tn = TypeName.Parse("SurveyEngine.DataTypeConverters.CalendarQuestionConstraintType, SurveyEngine.DataTypeConverters, Version=2.3.5.2, Culture=neutral, PublicKeyToken=79cd5921ea88a364");
            Assert.IsNotNull(tn);
            Assert.AreEqual("SurveyEngine.DataTypeConverters", tn.Assembly);
            Assert.AreEqual("SurveyEngine.DataTypeConverters", tn.Namespace);
            Assert.AreEqual("CalendarQuestionConstraintType", tn.Name);
            Assert.AreEqual(new Version("2.3.5.2"), tn.Version);
            Assert.AreEqual("neutral", tn.Culture);
            Assert.AreEqual("79cd5921ea88a364", tn.PublicKeyToken);

            tn = TypeName.Parse("SurveyEngine.DataTypeConverters.CalendarQuestionConstraintType, SurveyEngine.DataTypeConverters, Version=2.3.5.2, Culture=neutral");
            Assert.IsNotNull(tn);
            Assert.AreEqual("SurveyEngine.DataTypeConverters", tn.Assembly);
            Assert.AreEqual("SurveyEngine.DataTypeConverters", tn.Namespace);
            Assert.AreEqual("CalendarQuestionConstraintType", tn.Name);
            Assert.AreEqual(new Version("2.3.5.2"), tn.Version);
            Assert.AreEqual("neutral", tn.Culture);
            Assert.AreEqual(string.Empty, tn.PublicKeyToken);

            tn = TypeName.Parse("SurveyEngine.DataTypeConverters.CalendarQuestionConstraintType, SurveyEngine.DataTypeConverters, Version=2.3.5.2");
            Assert.IsNotNull(tn);
            Assert.AreEqual("SurveyEngine.DataTypeConverters", tn.Assembly);
            Assert.AreEqual("SurveyEngine.DataTypeConverters", tn.Namespace);
            Assert.AreEqual("CalendarQuestionConstraintType", tn.Name);
            Assert.AreEqual(new Version("2.3.5.2"), tn.Version);
            Assert.AreEqual(string.Empty, tn.Culture);
            Assert.AreEqual(string.Empty, tn.PublicKeyToken);

            tn = TypeName.Parse("SurveyEngine.DataTypeConverters.CalendarQuestionConstraintType, SurveyEngine.DataTypeConverters");
            Assert.IsNotNull(tn);
            Assert.AreEqual("SurveyEngine.DataTypeConverters", tn.Assembly);
            Assert.AreEqual("SurveyEngine.DataTypeConverters", tn.Namespace);
            Assert.AreEqual("CalendarQuestionConstraintType", tn.Name);
            Assert.IsNull(tn.Version);
            Assert.AreEqual(string.Empty, tn.Culture);
            Assert.AreEqual(string.Empty, tn.PublicKeyToken);
        }
    }
}
