using Iso20022Library.Application.Builders;
using Iso20022Library.Application.Validators;
using Iso20022Library.Domain.Common.Enums;
using Iso20022Library.Messages.Pain00100102.Generated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iso20022Library.Tests
{
    [TestClass]
    public class Pain00100102Tests
    {
        [TestMethod]
        public void BuildAndValidateXml_Pain00100102_ShouldPassValidation()
        {
            // Arrange
            var document = new Document
            {
                // Preencher com dados válidos mínimos se possível
            };

            var factory = new MessageBuilderFactory();
            var builder = factory.GetBuilder(MessageType.Pain00100102);
            var xml = builder.BuildXml(document);

            var xsdPath = Path.Combine("Messages", "Pain00100102", "Xsd", "pain.001.001.02.xsd");

            // Act
            bool isValid = XmlValidator.Validate(xml, xsdPath, out string errors);

            // Assert
            Assert.IsTrue(isValid, $"XML inválido: {errors}");
        }
    }

}
