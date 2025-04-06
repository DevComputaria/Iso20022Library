using Iso20022Library.Application.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iso20022Library.Tests
{
    [TestClass]
    public class XmlValidatorTests
    {
        [TestMethod]
        public void Validate_ValidXmlAgainstSchema_ShouldReturnTrue()
        {
            // Arrange
            string xml = """
        <note xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
            <to>Tove</to>
            <from>Jani</from>
            <heading>Reminder</heading>
            <body>Don't forget me this weekend!</body>
        </note>
        """;

            string xsd = """
        <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
          <xs:element name="note">
            <xs:complexType>
              <xs:sequence>
                <xs:element name="to" type="xs:string"/>
                <xs:element name="from" type="xs:string"/>
                <xs:element name="heading" type="xs:string"/>
                <xs:element name="body" type="xs:string"/>
              </xs:sequence>
            </xs:complexType>
          </xs:element>
        </xs:schema>
        """;

            string tempXsdPath = Path.GetTempFileName();
            File.WriteAllText(tempXsdPath, xsd);

            // Act
            bool isValid = XmlValidator.Validate(xml, tempXsdPath, out string errors);

            // Assert
            Assert.IsTrue(isValid, $"Expected XML to be valid, but got errors: {errors}");

            // Cleanup
            File.Delete(tempXsdPath);
        }

        [TestMethod]
        public void Validate_InvalidXmlAgainstSchema_ShouldReturnFalse()
        {
            // Arrange
            string xml = """
        <note>
            <to>Tove</to>
            <heading>Reminder</heading>
            <body>Don't forget me this weekend!</body>
        </note>
        """;

            string xsd = """
        <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
          <xs:element name="note">
            <xs:complexType>
              <xs:sequence>
                <xs:element name="to" type="xs:string"/>
                <xs:element name="from" type="xs:string"/>
                <xs:element name="heading" type="xs:string"/>
                <xs:element name="body" type="xs:string"/>
              </xs:sequence>
            </xs:complexType>
          </xs:element>
        </xs:schema>
        """;

            string tempXsdPath = Path.GetTempFileName();
            File.WriteAllText(tempXsdPath, xsd);

            // Act
            bool isValid = XmlValidator.Validate(xml, tempXsdPath, out string errors);

            // Assert
            Assert.IsFalse(isValid, "Expected XML to be invalid but it was valid.");
            Assert.IsTrue(!string.IsNullOrWhiteSpace(errors), "Expected validation errors.");

            // Cleanup
            File.Delete(tempXsdPath);
        }
    }

}
