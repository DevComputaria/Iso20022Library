using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Iso20022Library.Application.Builders.Pacs;
using Iso20022Library.Messages.Payments.Pacs.Generated.Pacs00400110;

namespace Iso20022Library.Tests.Builders.Pacs
{
    [TestClass]
    public class Pacs00400110BuilderTests
    {
        private Pacs00400110Builder _builder;

        [TestInitialize]
        public void Setup()
        {
            _builder = new Pacs00400110Builder();
        }

        [TestMethod]
        public void Build_WithRequiredFields_ShouldCreateValidDocument()
        {
            // Arrange
            var messageId = "MSG001";
            var creationDateTime = DateTime.Now;
            var numberOfTransactions = "1";
            var returnId = "RTN001";

            // Act
            var document = _builder
                .WithGroupHeader(messageId, creationDateTime, numberOfTransactions)
                .WithOriginalGroupInformation("ORIG001", "pacs.008.001.08")
                .AddPaymentTransaction(returnId)
                .Build();

            // Assert
            Assert.IsNotNull(document);
            Assert.IsNotNull(document.PmtRtr);
            Assert.IsNotNull(document.PmtRtr.GrpHdr);
            Assert.AreEqual(messageId, document.PmtRtr.GrpHdr.MsgId);
            Assert.AreEqual(creationDateTime, document.PmtRtr.GrpHdr.CreDtTm);
            Assert.AreEqual(numberOfTransactions, document.PmtRtr.GrpHdr.NbOfTxs);
            Assert.IsNotNull(document.PmtRtr.TxInf);
            Assert.AreEqual(1, document.PmtRtr.TxInf.Length);
            Assert.AreEqual(returnId, document.PmtRtr.TxInf[0].RtrId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WithGroupHeader_WithEmptyMessageId_ShouldThrowException()
        {
            // Act
            _builder.WithGroupHeader("", DateTime.Now, "1");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WithGroupHeader_WithEmptyNumberOfTransactions_ShouldThrowException()
        {
            // Act
            _builder.WithGroupHeader("MSG001", DateTime.Now, "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WithOriginalGroupInformation_WithEmptyOriginalMessageId_ShouldThrowException()
        {
            // Act
            _builder.WithOriginalGroupInformation("", "pacs.008.001.08");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WithOriginalGroupInformation_WithEmptyOriginalMessageNameId_ShouldThrowException()
        {
            // Act
            _builder.WithOriginalGroupInformation("ORIG001", "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddPaymentTransaction_WithEmptyReturnId_ShouldThrowException()
        {
            // Act
            _builder.AddPaymentTransaction("");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Build_WithoutGroupHeader_ShouldThrowException()
        {
            // Act
            _builder.AddPaymentTransaction("RTN001").Build();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Build_WithoutTransactions_ShouldThrowException()
        {
            // Act
            _builder.WithGroupHeader("MSG001", DateTime.Now, "1").Build();
        }

        [TestMethod]
        public void BuildXml_WithValidDocument_ShouldReturnXmlString()
        {
            // Arrange
            var messageId = "MSG001";
            var creationDateTime = DateTime.Now;
            var numberOfTransactions = "1";
            var returnId = "RTN001";

            _builder
                .WithGroupHeader(messageId, creationDateTime, numberOfTransactions)
                .WithOriginalGroupInformation("ORIG001", "pacs.008.001.08")
                .AddPaymentTransaction(returnId);

            // Act
            var xml = _builder.BuildXml(null);

            // Assert
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.Contains("PmtRtr"));
            Assert.IsTrue(xml.Contains(messageId));
            Assert.IsTrue(xml.Contains(returnId));
        }

        [TestMethod]
        public void Clone_ShouldCreateIdenticalBuilder()
        {
            // Arrange
            var messageId = "MSG001";
            var creationDateTime = DateTime.Now;
            var numberOfTransactions = "1";
            var returnId = "RTN001";

            _builder
                .WithGroupHeader(messageId, creationDateTime, numberOfTransactions)
                .WithOriginalGroupInformation("ORIG001", "pacs.008.001.08")
                .AddPaymentTransaction(returnId);

            // Act
            var clonedBuilder = _builder.Clone();
            var originalDocument = _builder.Build();
            var clonedDocument = clonedBuilder.Build();

            // Assert
            Assert.AreEqual(originalDocument.PmtRtr.GrpHdr.MsgId, clonedDocument.PmtRtr.GrpHdr.MsgId);
            Assert.AreEqual(originalDocument.PmtRtr.TxInf.Length, clonedDocument.PmtRtr.TxInf.Length);
            Assert.AreEqual(originalDocument.PmtRtr.TxInf[0].RtrId, clonedDocument.PmtRtr.TxInf[0].RtrId);
        }

        [TestMethod]
        public void CreateReturnReasonInformation_ShouldCreateValidReturnReason()
        {
            // Act
            var returnReason = Pacs00400110Builder.CreateReturnReasonInformation(
                additionalReturnReasonInformation: new[] { "Invalid account number" });

            // Assert
            Assert.IsNotNull(returnReason);
            Assert.IsNotNull(returnReason.AddtlInf);
            Assert.AreEqual(1, returnReason.AddtlInf.Length);
            Assert.AreEqual("Invalid account number", returnReason.AddtlInf[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateFinancialInstitution_WithEmptyBic_ShouldThrowException()
        {
            // Act
            Pacs00400110Builder.CreateFinancialInstitution("");
        }

        [TestMethod]
        public void CreateFinancialInstitution_WithValidBic_ShouldCreateValidInstitution()
        {
            // Arrange
            var bic = "DEUTDEFF";

            // Act
            var institution = Pacs00400110Builder.CreateFinancialInstitution(bic);

            // Assert
            Assert.IsNotNull(institution);
            Assert.IsNotNull(institution.FinInstnId);
            Assert.AreEqual(bic, institution.FinInstnId.BICFI);
        }
    }
}
