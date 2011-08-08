//-----------------------------------------------------------------------
// <copyright file="TypeName.cs" company="Tasty Codes">
//     Copyright (c) 2011 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------

namespace LLBLGenProjectUpdateTask
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Represents the parts of a type name.
    /// </summary>
    internal sealed class TypeName
    {
        private const string NameExp = @"^[a-zA-Z\d_]+[a-zA-Z\d\._-]*[^\.]$";
        private const string CultureExp = @"^[a-zA-Z]{2}[a-zA-Z-]*[^-]$";
        private const string TokenExp = @"^[a-fA-F\d]{16}$";
        private string assembly, culture, name, ns, publicKeyToken;

        /// <summary>
        /// Gets or sets the type's assembly.
        /// </summary>
        public string Assembly
        {
            get
            {
                return this.assembly ?? string.Empty;
            }

            set
            {
                if (!string.IsNullOrEmpty(value) && !Regex.IsMatch(value, NameExp))
                {
                    throw new FormatException("The specified value is not in the correct format for an assembly name.");
                }

                this.assembly = value;
            }
        }

        /// <summary>
        /// Gets or sets the type's culture.
        /// </summary>
        public string Culture
        {
            get
            {
                return this.culture ?? string.Empty;
            }

            set
            {
                if (!string.IsNullOrEmpty(value) && !Regex.IsMatch(value, CultureExp))
                {
                    throw new FormatException("The specified value is not in the correct format for an assembly culture.");
                }

                this.culture = value;
            }
        }

        /// <summary>
        /// Gets or sets the type's name.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name ?? string.Empty;
            }

            set
            {
                if (!string.IsNullOrEmpty(value) && !Regex.IsMatch(value, NameExp))
                {
                    throw new FormatException("The specified value is not in the correct format for a type name.");
                }

                this.name = value;
            }
        }

        /// <summary>
        /// Gets or sets the type's namespace.
        /// </summary>
        public string Namespace
        {
            get
            {
                return this.ns ?? string.Empty;
            }

            set
            {
                if (!string.IsNullOrEmpty(value) && !Regex.IsMatch(value, NameExp))
                {
                    throw new FormatException("The specified value is not in the correct format for a namespace name.");
                }

                this.ns = value;
            }
        }

        /// <summary>
        /// Gets or sets the type's public key token.
        /// </summary>
        public string PublicKeyToken
        {
            get
            {
                return this.publicKeyToken ?? string.Empty;
            }

            set
            {
                if (!string.IsNullOrEmpty(value) && !Regex.IsMatch(value, NameExp))
                {
                    throw new FormatException("The specified value is not in the correct format for a public key token.");
                }

                this.publicKeyToken = value;
            }
        }

        /// <summary>
        /// Gets or sets the type's version.
        /// </summary>
        public Version Version { get; set; }

        /// <summary>
        /// Parses the given value into a <see cref="TypeName"/>.
        /// </summary>
        /// <param name="value">The value to parse.</param>
        /// <returns>The parsed <see cref="TypeName"/>.</returns>
        public static TypeName Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value", "value must contain a value.");
            }

            string[] parts = (from p in value.Split(',')
                              let t = p.Trim()
                              where !string.IsNullOrEmpty(t)
                              select t).ToArray();

            if (parts.Length >= 2)
            {
                string[] nameParts = parts[0].Split('.');

                if (nameParts.Length >= 2)
                {
                    string ns = string.Join(".", nameParts.Take(nameParts.Length - 1).ToArray());
                    string name = nameParts[nameParts.Length - 1];

                    TypeName typeName = new TypeName()
                    {
                        Assembly = parts[1],
                        Name = name,
                        Namespace = ns
                    };

                    foreach (string part in parts.Skip(2))
                    {
                        bool valid = true;
                        string[] keyValue = (from p in part.Split('=')
                                             let t = p.Trim()
                                             where !string.IsNullOrEmpty(t)
                                             select t).ToArray();

                        if (keyValue.Length == 2)
                        {
                            switch (keyValue[0].ToUpperInvariant())
                            {
                                case "VERSION":
                                    typeName.Version = new Version(keyValue[1]);
                                    break;
                                case "CULTURE":
                                    typeName.Culture = keyValue[1];
                                    break;
                                case "PUBLICKEYTOKEN":
                                    typeName.PublicKeyToken = keyValue[1];
                                    break;
                                default:
                                    valid = false;
                                    break;
                            }
                        }
                        else
                        {
                            valid = false;
                        }

                        if (!valid)
                        {
                            throw new FormatException(string.Format(CultureInfo.InvariantCulture, "Unknown type name token: '{0}'.", part));
                        }
                    }

                    return typeName;
                }
                else
                {
                    throw new FormatException("The specified value does not contain a namespace and a type name.");
                }
            }
            else
            {
                throw new FormatException("The specified value is not in a valid format for a type name.");
            }
        }

        /// <summary>
        /// Returns the string representation of this instance.
        /// </summary>
        /// <returns>A string representing this instance.</returns>
        public override string ToString()
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(this.Namespace) && !string.IsNullOrEmpty(this.Name) && !string.IsNullOrEmpty(this.Assembly))
            {
                result = string.Concat(this.Namespace, ".", this.Name, ", ", this.Assembly);

                if (this.Version != null)
                {
                    result = string.Concat(result, ", Version=", this.Version.ToString(4));
                }

                if (!string.IsNullOrEmpty(this.Culture))
                {
                    result = string.Concat(result, ", Culture=", this.Culture);
                }

                if (!string.IsNullOrEmpty(this.PublicKeyToken))
                {
                    result = string.Concat(result, ", PublicKeyToken=", this.PublicKeyToken);
                }
            }

            return result;
        }

        /// <summary>
        /// Clones this instance into a new <see cref="TypeName"/> instance.
        /// </summary>
        /// <returns>A cloned <see cref="TypeName"/> instance.</returns>
        public TypeName Clone()
        {
            TypeName clone = new TypeName();
            clone.assembly = this.assembly;
            clone.culture = this.culture;
            clone.name = this.name;
            clone.ns = this.ns;
            clone.publicKeyToken = this.publicKeyToken;

            if (this.Version != null)
            {
                clone.Version = new Version(this.Version.ToString());
            }

            return clone;
        }
    }
}
