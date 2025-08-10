using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Iso20022Library.Application.Builders;
using Iso20022Library.Application.Builders.Pain;
using Iso20022Library.Domain.Common.Enums;
using Iso20022Library.Messages.Payments.Pain.Generated.Pain00100108;

namespace Iso20022Library.Tests.Builders
{
    /// <summary>
    /// Unit tests for Pain00100108Builder functionality.
    /// </summary>
    [TestClass]
    public class Pain00100108BuilderTests
    {
        private MessageBuilderFactory _factory = null!;
        private Pain00100108Builder _builder = null!;

        [TestInitialize]
        public void Setup()
        {
            _factory = new MessageBuilderFactory();
            _builder = new Pain00100108Builder();
        }

        [TestMethod]
        public void Factory_CanRetrievePain00100108Builder()
        {
            // Act
            var builder = _factory.GetBuilder(MessageType.Pain00100108);

            // Assert
            Assert.IsNotNull(builder);
            Assert.IsInstanceOfType(builder, typeof(Pain00100108Builder));
            
            // Cast to concrete type to access GetMessageType method
            var concreteBuilder = (Pain00100108Builder)builder;
            Assert.AreEqual("pain.001.001.08", concreteBuilder.GetMessageType());
        }

        [TestMethod]
        public void Builder_CanSetGroupHeaderWithBasicInformation()
        {
            // Arrange
            var messageId = "MSG123456789";
            var creationDateTime = DateTime.Now;
            var numberOfTransactions = "1";

            // Act
            var result = _builder.SetGroupHeader(
                messageId: messageId,
                creationDateTime: creationDateTime,
                numberOfTransactions: numberOfTransactions);

            // Assert
            Assert.AreSame(_builder, result); // Should return same instance for fluent API
            Assert.AreEqual("pain.001.001.08", _builder.GetMessageType());
        }

        [TestMethod]
        public void Builder_ThrowsExceptionWhenBuildingWithoutGroupHeader()
        {
            // Act & Assert
            Assert.ThrowsException<InvalidOperationException>(() => _builder.Build());
        }

        [TestMethod]
        public void Builder_ThrowsExceptionWhenBuildingWithoutPaymentInstruction()
        {
            // Arrange
            _builder.SetGroupHeader("MSG123", DateTime.Now, "1");

            // Act & Assert
            Assert.ThrowsException<InvalidOperationException>(() => _builder.Build());
        }

        [TestMethod]
        public void Builder_CanAddPaymentInstructionWithDateChoice()
        {
            // Arrange
            var messageId = "MSG123456789";
            var creationDateTime = DateTime.Now;
            var numberOfTransactions = "1";
            var paymentInformationId = "PMT123";
            var executionDate = DateTime.Today.AddDays(1);

            var debtor = new PartyIdentification43 { Nm = "Test Debtor" };
            var debtorAccount = new CashAccount24 
            { 
                Id = new AccountIdentification4Choice
                {
                    Iban = "GB29NWBK60161331926819"
                }
            };
            var debtorAgent = new BranchAndFinancialInstitutionIdentification5
            {
                FinInstnId = new FinancialInstitutionIdentification8
                {
                    Bicfi = "NWBKGB2L"
                }
            };

            // Act
            _builder.SetGroupHeader(messageId, creationDateTime, numberOfTransactions);
            
            var result = _builder.AddPaymentInstruction(
                paymentInformationId: paymentInformationId,
                paymentMethod: PaymentMethod3Code.Trf,
                batchBooking: true,
                numberOfTransactions: numberOfTransactions,
                controlSum: 100.00m,
                paymentTypeInformation: null,
                requestedExecutionDate: executionDate,
                debtor: debtor,
                debtorAccount: debtorAccount,
                debtorAgent: debtorAgent);

            // Assert
            Assert.AreSame(_builder, result);
        }
    }
}
