using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Iso20022Library.Application.Builders.Pain;
using Iso20022Library.Domain.Common.Enums;
using Iso20022Library.Messages.Payments.Pain.Generated.Pain00700108;

namespace Iso20022Library.Tests.Builders
{
    /// <summary>
    /// Unit tests for Pain00700108Builder - Customer Payment Reversal V08.
    /// </summary>
    [TestClass]
    public class Pain00700108BuilderTests
    {
        private Pain00700108Builder _builder;

        [TestInitialize]
        public void Setup()
        {
            _builder = new Pain00700108Builder();
        }

        [TestMethod]
        public void Constructor_ShouldInitializeBuilder()
        {
            // Act & Assert
            Assert.IsNotNull(_builder);
            Assert.AreEqual(MessageType.Pain00700108, _builder.MessageType);
        }

        [TestMethod]
        public void SetGroupHeader_WithValidParameters_ShouldSetGroupHeader()
        {
            // Arrange
            var messageId = "MSG001";
            var creationDateTime = new DateTime(2024, 1, 15, 10, 30, 0);
            var numberOfTransactions = "5";
            var controlSum = 1500.75m;
            var initiatingParty = Pain00700108Builder.CreatePartyIdentification("Test Bank");

            // Act
            var result = _builder.SetGroupHeader(messageId, creationDateTime, numberOfTransactions, controlSum, initiatingParty)
                                 .SetOriginalGroupInformation("ORIG001"); // Required for Build to succeed
            var document = _builder.Build();

            // Assert
            Assert.AreSame(_builder, result);
            Assert.IsNotNull(document.CstmrPmtRvsl.GrpHdr);
            Assert.AreEqual(messageId, document.CstmrPmtRvsl.GrpHdr.MsgId);
            Assert.AreEqual(creationDateTime, document.CstmrPmtRvsl.GrpHdr.CreDtTm);
            Assert.AreEqual(numberOfTransactions, document.CstmrPmtRvsl.GrpHdr.NbOfTxs);
            Assert.AreEqual(controlSum, document.CstmrPmtRvsl.GrpHdr.CtrlSum);
            Assert.IsTrue(document.CstmrPmtRvsl.GrpHdr.CtrlSumSpecified);
            Assert.AreEqual("Test Bank", document.CstmrPmtRvsl.GrpHdr.InitgPty.Nm);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetGroupHeader_WithNullMessageId_ShouldThrowException()
        {
            // Act
            _builder.SetGroupHeader(null, DateTime.Now, "1");
        }

        [TestMethod]
        public void SetOriginalGroupInformation_WithValidParameters_ShouldSetOriginalGroupInformation()
        {
            // Arrange
            var originalMessageId = "ORIG001";
            var originalMessageNameId = "pain.001.001.03";
            var originalCreationDateTime = new DateTime(2024, 1, 10, 9, 0, 0);
            var reversalReasons = new List<PaymentReversalReason8>
            {
                Pain00700108Builder.CreatePaymentReversalReason(
                    Pain00700108Builder.CreatePartyIdentification("Test Bank"),
                    Pain00700108Builder.CreateReversalReasonWithCode("CUST"),
                    new[] { "Customer request" })
            };

            // Act
            var result = _builder.SetGroupHeader("MSG001", DateTime.Now, "1") // Required for Build to succeed
                                 .SetOriginalGroupInformation(originalMessageId, originalMessageNameId, originalCreationDateTime, reversalReasons);
            var document = _builder.Build();

            // Assert
            Assert.AreSame(_builder, result);
            Assert.IsNotNull(document.CstmrPmtRvsl.OrgnlGrpInf);
            Assert.AreEqual(originalMessageId, document.CstmrPmtRvsl.OrgnlGrpInf.OrgnlMsgId);
            Assert.AreEqual(originalMessageNameId, document.CstmrPmtRvsl.OrgnlGrpInf.OrgnlMsgNmId);
            Assert.AreEqual(originalCreationDateTime, document.CstmrPmtRvsl.OrgnlGrpInf.OrgnlCreDtTm);
            Assert.IsTrue(document.CstmrPmtRvsl.OrgnlGrpInf.OrgnlCreDtTmSpecified);
            Assert.AreEqual(1, document.CstmrPmtRvsl.OrgnlGrpInf.RvslRsnInf.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetOriginalGroupInformation_WithNullOriginalMessageId_ShouldThrowException()
        {
            // Act
            _builder.SetOriginalGroupInformation(null);
        }

        [TestMethod]
        public void AddOriginalPaymentInstruction_WithAllFields_ShouldSetAllFields()
        {
            // Arrange
            var reversalPaymentInformationId = "RVSL123";
            var originalPaymentInformationId = "PMT123";
            var originalNumberOfTransactions = "3";
            var originalControlSum = 750.25m;
            var batchBooking = true;
            var paymentInformationReversal = false;
            var reversalReasons = new List<PaymentReversalReason8>
            {
                Pain00700108Builder.CreatePaymentReversalReason(
                    Pain00700108Builder.CreatePartyIdentification("Test Bank"),
                    Pain00700108Builder.CreateReversalReasonWithCode("CUST"),
                    new[] { "Customer request" })
            };

            // Act
            var result = _builder.SetGroupHeader("MSG001", DateTime.Now, "1") // Required for Build to succeed
                                 .SetOriginalGroupInformation("ORIG001")
                                 .AddOriginalPaymentInstruction(
                reversalPaymentInformationId,
                originalPaymentInformationId,
                originalNumberOfTransactions,
                originalControlSum,
                batchBooking,
                paymentInformationReversal,
                reversalReasons);

            var document = _builder.Build();

            // Assert
            Assert.AreSame(_builder, result);
            Assert.AreEqual(1, document.CstmrPmtRvsl.OrgnlPmtInfAndRvsl.Count);

            var instruction = document.CstmrPmtRvsl.OrgnlPmtInfAndRvsl.First();
            Assert.AreEqual(reversalPaymentInformationId, instruction.RvslPmtInfId);
            Assert.AreEqual(originalPaymentInformationId, instruction.OrgnlPmtInfId);
            Assert.AreEqual(originalNumberOfTransactions, instruction.OrgnlNbOfTxs);
            Assert.AreEqual(originalControlSum, instruction.OrgnlCtrlSum);
            Assert.IsTrue(instruction.OrgnlCtrlSumSpecified);
            Assert.AreEqual(batchBooking, instruction.BtchBookg);
            Assert.IsTrue(instruction.BtchBookgSpecified);
            Assert.AreEqual(paymentInformationReversal, instruction.PmtInfRvsl);
            Assert.IsTrue(instruction.PmtInfRvslSpecified);
            Assert.AreEqual(1, instruction.RvslRsnInf.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddOriginalPaymentInstruction_WithNullOriginalPaymentInformationId_ShouldThrowException()
        {
            // Act
            _builder.AddOriginalPaymentInstruction("RVSL123", null);
        }

        [TestMethod]
        public void AddPaymentTransactionReversal_WithValidTransaction_ShouldAddTransaction()
        {
            // Arrange
            _builder.SetGroupHeader("MSG001", DateTime.Now, "1")
                   .SetOriginalGroupInformation("ORIG001")
                   .AddOriginalPaymentInstruction("RVSL123", "PMT123");

            var transaction = Pain00700108Builder.CreatePaymentTransactionReversal(
                "TXN001",
                "INST001",
                "E2E001",
                Pain00700108Builder.CreateAmount(100.50m, "EUR"));

            // Act
            var result = _builder.AddPaymentTransactionReversal(transaction);
            var document = _builder.Build();

            // Assert
            Assert.AreSame(_builder, result);
            Assert.AreEqual(1, document.CstmrPmtRvsl.OrgnlPmtInfAndRvsl.First().TxInf.Count);
            Assert.AreEqual("TXN001", document.CstmrPmtRvsl.OrgnlPmtInfAndRvsl.First().TxInf.First().RvslId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddPaymentTransactionReversal_WithNullTransaction_ShouldThrowException()
        {
            // Act
            _builder.AddPaymentTransactionReversal(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddPaymentTransactionReversal_WithoutPaymentInstruction_ShouldThrowException()
        {
            // Arrange
            var transaction = Pain00700108Builder.CreatePaymentTransactionReversal("TXN001");

            // Act
            _builder.AddPaymentTransactionReversal(transaction);
        }

        [TestMethod]
        public void CreatePaymentTransactionReversal_WithValidParameters_ShouldReturnConfiguredTransaction()
        {
            // Arrange
            var reversalId = "TXN001";
            var originalInstructionId = "INST001";
            var originalEndToEndId = "E2E001";
            var originalAmount = Pain00700108Builder.CreateAmount(100.50m, "EUR");
            var reversedAmount = Pain00700108Builder.CreateAmount(100.50m, "EUR");
            var chargeBearer = ChargeBearerType1Code.Debt;

            // Act
            var transaction = Pain00700108Builder.CreatePaymentTransactionReversal(
                reversalId,
                originalInstructionId,
                originalEndToEndId,
                originalAmount,
                reversedAmount,
                chargeBearer);

            // Assert
            Assert.IsNotNull(transaction);
            Assert.AreEqual(reversalId, transaction.RvslId);
            Assert.AreEqual(originalInstructionId, transaction.OrgnlInstrId);
            Assert.AreEqual(originalEndToEndId, transaction.OrgnlEndToEndId);
            Assert.AreEqual(originalAmount, transaction.OrgnlInstdAmt);
            Assert.AreEqual(reversedAmount, transaction.RvsdInstdAmt);
            Assert.AreEqual(chargeBearer, transaction.ChrgBr);
            Assert.IsTrue(transaction.ChrgBrSpecified);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreatePaymentTransactionReversal_WithNullReversalId_ShouldThrowException()
        {
            // Act
            Pain00700108Builder.CreatePaymentTransactionReversal(null);
        }

        [TestMethod]
        public void CreatePaymentReversalReason_WithValidParameters_ShouldReturnConfiguredReason()
        {
            // Arrange
            var originator = Pain00700108Builder.CreatePartyIdentification("Test Bank");
            var reason = Pain00700108Builder.CreateReversalReasonWithCode("CUST");
            var additionalInfo = new[] { "Customer request", "Wrong amount" };

            // Act
            var reversalReason = Pain00700108Builder.CreatePaymentReversalReason(originator, reason, additionalInfo);

            // Assert
            Assert.IsNotNull(reversalReason);
            Assert.AreEqual(originator, reversalReason.Orgtr);
            Assert.AreEqual(reason, reversalReason.Rsn);
            Assert.AreEqual(2, reversalReason.AddtlInf.Count);
            Assert.IsTrue(reversalReason.AddtlInf.Contains("Customer request"));
            Assert.IsTrue(reversalReason.AddtlInf.Contains("Wrong amount"));
        }

        [TestMethod]
        public void CreateReversalReasonWithCode_WithValidCode_ShouldReturnReasonChoice()
        {
            // Arrange
            var code = "CUST";

            // Act
            var reason = Pain00700108Builder.CreateReversalReasonWithCode(code);

            // Assert
            Assert.IsNotNull(reason);
            Assert.AreEqual(code, reason.Cd);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateReversalReasonWithCode_WithNullCode_ShouldThrowException()
        {
            // Act
            Pain00700108Builder.CreateReversalReasonWithCode(null);
        }

        [TestMethod]
        public void CreateReversalReasonWithProprietary_WithValidProprietary_ShouldReturnReasonChoice()
        {
            // Arrange
            var proprietary = "CUSTOM_REASON";

            // Act
            var reason = Pain00700108Builder.CreateReversalReasonWithProprietary(proprietary);

            // Assert
            Assert.IsNotNull(reason);
            Assert.AreEqual(proprietary, reason.Prtry);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateReversalReasonWithProprietary_WithNullProprietary_ShouldThrowException()
        {
            // Act
            Pain00700108Builder.CreateReversalReasonWithProprietary(null);
        }

        [TestMethod]
        public void CreatePartyIdentification_WithValidName_ShouldReturnPartyIdentification()
        {
            // Arrange
            var name = "Test Bank";

            // Act
            var party = Pain00700108Builder.CreatePartyIdentification(name);

            // Assert
            Assert.IsNotNull(party);
            Assert.AreEqual(name, party.Nm);
        }

        [TestMethod]
        public void CreateAmount_WithValidParameters_ShouldReturnAmount()
        {
            // Arrange
            var value = 150.75m;
            var currency = "USD";

            // Act
            var amount = Pain00700108Builder.CreateAmount(value, currency);

            // Assert
            Assert.IsNotNull(amount);
            Assert.AreEqual(value, amount.Value);
            Assert.AreEqual(currency, amount.Ccy);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateAmount_WithNullCurrency_ShouldThrowException()
        {
            // Act
            Pain00700108Builder.CreateAmount(100.00m, null);
        }

        [TestMethod]
        public void CreateOriginalTransactionReference_WithValidParameters_ShouldReturnReference()
        {
            // Arrange
            var executionDate = new DateTime(2024, 1, 20);
            var collectionDate = new DateTime(2024, 1, 25);
            var paymentMethod = PaymentMethod4Code.Chk;

            // Act
            var reference = Pain00700108Builder.CreateOriginalTransactionReference(
                requestedExecutionDate: executionDate,
                requestedCollectionDate: collectionDate,
                paymentMethod: paymentMethod);

            // Assert
            Assert.IsNotNull(reference);
            Assert.IsNotNull(reference.ReqdExctnDt);
            Assert.AreEqual(executionDate, reference.ReqdExctnDt.DtTm);
            Assert.IsTrue(reference.ReqdExctnDt.DtTmSpecified);
            Assert.AreEqual(collectionDate, reference.ReqdColltnDt);
            Assert.IsTrue(reference.ReqdColltnDtSpecified);
            Assert.AreEqual(paymentMethod, reference.PmtMtd);
            Assert.IsTrue(reference.PmtMtdSpecified);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Build_WithoutGroupHeader_ShouldThrowException()
        {
            // Act
            _builder.Build();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Build_WithoutOriginalGroupInformation_ShouldThrowException()
        {
            // Arrange
            _builder.SetGroupHeader("MSG001", DateTime.Now, "1");

            // Act
            _builder.Build();
        }

        [TestMethod]
        public void ToXml_WithCompleteMessage_ShouldReturnValidXml()
        {
            // Arrange
            _builder.SetGroupHeader("MSG001", new DateTime(2024, 1, 15, 10, 30, 0), "1", 100.50m,
                      Pain00700108Builder.CreatePartyIdentification("Test Bank"))
                   .SetOriginalGroupInformation("ORIG001", "pain.001.001.03", new DateTime(2024, 1, 10, 9, 0, 0))
                   .AddOriginalPaymentInstruction("RVSL123", "PMT123", "1", 100.50m)
                   .AddPaymentTransactionReversal(Pain00700108Builder.CreatePaymentTransactionReversal(
                       "TXN001",
                       "INST001",
                       "E2E001",
                       Pain00700108Builder.CreateAmount(100.50m, "EUR")));

            // Act
            var xml = _builder.ToXml();

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(xml));
            Assert.IsTrue(xml.Contains("<?xml"));
            Assert.IsTrue(xml.Contains("<Document"));
            Assert.IsTrue(xml.Contains("<CstmrPmtRvsl>"));
            Assert.IsTrue(xml.Contains("pain.007.001.08"));
        }

        [TestMethod]
        public void FluentInterface_ShouldAllowMethodChaining()
        {
            // Act & Assert - Should not throw exceptions
            var result = _builder
                .SetGroupHeader("MSG001", DateTime.Now, "1")
                .SetOriginalGroupInformation("ORIG001")
                .AddOriginalPaymentInstruction("RVSL123", "PMT123")
                .AddPaymentTransactionReversal(Pain00700108Builder.CreatePaymentTransactionReversal("TXN001"));

            Assert.AreSame(_builder, result);
        }

        [TestMethod]
        public void CompleteWorkflow_WithMultipleTransactions_ShouldBuildCorrectly()
        {
            // Arrange & Act
            var document = _builder
                .SetGroupHeader("MSG001", new DateTime(2024, 1, 15, 10, 30, 0), "3", 350.25m,
                    Pain00700108Builder.CreatePartyIdentification("Test Bank"))
                .SetOriginalGroupInformation("ORIG001", "pain.001.001.03", new DateTime(2024, 1, 10, 9, 0, 0))
                .AddOriginalPaymentInstruction("RVSL123", "PMT123", "3", 350.25m)
                .AddPaymentTransactionReversal(Pain00700108Builder.CreatePaymentTransactionReversal(
                    "TXN001", "INST001", "E2E001", Pain00700108Builder.CreateAmount(100.00m, "EUR")))
                .AddPaymentTransactionReversal(Pain00700108Builder.CreatePaymentTransactionReversal(
                    "TXN002", "INST002", "E2E002", Pain00700108Builder.CreateAmount(150.00m, "EUR")))
                .AddPaymentTransactionReversal(Pain00700108Builder.CreatePaymentTransactionReversal(
                    "TXN003", "INST003", "E2E003", Pain00700108Builder.CreateAmount(100.25m, "EUR")))
                .Build();

            // Assert
            Assert.IsNotNull(document);
            Assert.IsNotNull(document.CstmrPmtRvsl);
            Assert.AreEqual("MSG001", document.CstmrPmtRvsl.GrpHdr.MsgId);
            Assert.AreEqual("3", document.CstmrPmtRvsl.GrpHdr.NbOfTxs);
            Assert.AreEqual(350.25m, document.CstmrPmtRvsl.GrpHdr.CtrlSum);
            Assert.AreEqual("ORIG001", document.CstmrPmtRvsl.OrgnlGrpInf.OrgnlMsgId);
            Assert.AreEqual(1, document.CstmrPmtRvsl.OrgnlPmtInfAndRvsl.Count);
            Assert.AreEqual(3, document.CstmrPmtRvsl.OrgnlPmtInfAndRvsl.First().TxInf.Count);
        }
    }
}
