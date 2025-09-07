using Microsoft.VisualStudio.TestTools.UnitTesting;
using Iso20022Library.Application.Builders;
using Iso20022Library.Domain.Common.Enums;
using Iso20022Library.Application.Builders.Pacs;
using Iso20022Library.Messages.Payments.Pacs.Generated.Pacs00300108;
using System;

namespace Iso20022Library.Tests.Builders
{
    [TestClass]
    public class Pacs00300108BuilderIntegrationTests
    {
        private MessageBuilderFactory _factory = null!;

        [TestInitialize]
        public void Setup()
        {
            _factory = new MessageBuilderFactory();
        }

        [TestMethod]
        public void Factory_CanCreatePacs00300108Builder()
        {
            // Act
            var builder = _factory.GetBuilder(MessageType.Pacs00300108);

            // Assert
            Assert.IsNotNull(builder);
            Assert.IsInstanceOfType(builder, typeof(Pacs00300108Builder));

            var pacsBuilder = (Pacs00300108Builder)builder;
            Assert.AreEqual(MessageType.Pacs00300108, pacsBuilder.MessageType);
        }

        [TestMethod]
        public void Pacs00300108Builder_CanBuildBasicMessage()
        {
            // Arrange
            var builder = (Pacs00300108Builder)_factory.GetBuilder(MessageType.Pacs00300108);

            // Act
            builder.WithGroupHeader(
                messageId: "MSG123456789",
                creationDateTime: DateTime.UtcNow,
                numberOfTransactions: "1");

            var paymentId = Pacs00300108Builder.CreatePaymentIdentification("E2E123456");
            var amount = Pacs00300108Builder.CreateActiveAmount(100.00m, "EUR");

            builder.AddDirectDebitTransaction(
                paymentIdentification: paymentId,
                interBankSettlementAmount: amount);

            var xml = builder.BuildXml();

            // Assert
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.Contains("FIToFICstmrDrctDbt"));
            Assert.IsTrue(xml.Contains("MSG123456789"));
            Assert.IsTrue(xml.Contains("E2E123456"));
        }

        [TestMethod]
        public void Pacs00300108Builder_ValidatesRequiredFields()
        {
            // Arrange
            var builder = (Pacs00300108Builder)_factory.GetBuilder(MessageType.Pacs00300108);

            // Act & Assert - Should throw because no group header
            var exception = Assert.ThrowsException<InvalidOperationException>(() => builder.BuildXml());
            Assert.IsTrue(exception.Message.Contains("Group header is required"));
        }

        [TestMethod]
        public void Pacs00300108Builder_CountsTransactionsCorrectly()
        {
            // Arrange
            var builder = (Pacs00300108Builder)_factory.GetBuilder(MessageType.Pacs00300108);

            builder.WithGroupHeader(
                messageId: "MSG123456789",
                creationDateTime: DateTime.UtcNow,
                numberOfTransactions: "2");

            // Act
            var paymentId1 = Pacs00300108Builder.CreatePaymentIdentification("E2E123456");
            var paymentId2 = Pacs00300108Builder.CreatePaymentIdentification("E2E789012");
            var amount = Pacs00300108Builder.CreateActiveAmount(100.00m, "EUR");

            builder.AddDirectDebitTransaction(paymentId1, amount);
            builder.AddDirectDebitTransaction(paymentId2, amount);

            // Assert
            Assert.AreEqual(2, builder.GetTransactionCount());
        }
    }
}
