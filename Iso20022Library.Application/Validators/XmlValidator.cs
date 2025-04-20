using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml;

namespace Iso20022Library.Application.Validators
{
    /// <summary>
    /// Provides validation capabilities for XML documents against XSD schemas.
    /// </summary>
    /// <remarks>
    /// This validator supports validating XML strings against XML Schema Definition (XSD) files
    /// to ensure they conform to the specified structure and constraints defined in the ISO 20022 standard.
    /// It captures all validation errors during the process and returns them as a formatted string.
    /// </remarks>
    public class XmlValidator
    {
        /// <summary>
        /// Validates an XML string against an XSD schema.
        /// </summary>
        /// <param name="xml">The XML content as a string.</param>
        /// <param name="xsdPath">The file path to the XSD schema.</param>
        /// <param name="validationErrors">When this method returns, contains the validation error messages if validation fails; otherwise, an empty string.</param>
        /// <returns><c>true</c> if the XML is valid according to the schema; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// This method loads the XSD schema and validates the provided XML content against it.
        /// Any validation errors are captured and returned through the <paramref name="validationErrors"/> parameter.
        /// </remarks>
        /// <exception cref="System.IO.FileNotFoundException">Thrown if the XSD schema file cannot be found.</exception>
        /// <exception cref="XmlSchemaException">Thrown if there is an error in the XSD schema.</exception>
        /// <example>
        /// <code>
        /// string xml = "&lt;root&gt;&lt;child&gt;value&lt;/child&gt;&lt;/root&gt;";
        /// string xsdPath = "schema.xsd";
        /// 
        /// if (XmlValidator.Validate(xml, xsdPath, out string errors))
        /// {
        ///     Console.WriteLine("XML is valid");
        /// }
        /// else
        /// {
        ///     Console.WriteLine($"XML validation failed: {errors}");
        /// }
        /// </code>
        /// </example>
        public static bool Validate(string xml, string xsdPath, out string validationErrors)
        {
            var schemaSet = new XmlSchemaSet();
            schemaSet.Add(null, xsdPath);

            var sb = new StringBuilder();
            var settings = new XmlReaderSettings
            {
                ValidationType = ValidationType.Schema,
                Schemas = schemaSet
            };

            settings.ValidationEventHandler += (sender, args) =>
            {
                sb.AppendLine(args.Message);
            };

            try
            {
                using var reader = XmlReader.Create(new StringReader(xml), settings);
                while (reader.Read()) { }
            }
            catch (Exception ex)
            {
                sb.AppendLine($"Exception: {ex.Message}");
            }

            validationErrors = sb.ToString();
            return string.IsNullOrEmpty(validationErrors);
        }
    }
}
