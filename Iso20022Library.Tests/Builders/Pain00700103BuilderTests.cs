using System;
using System.Linq;
using Iso20022Library.Application.Builders;
using Iso20022Library.Application.Builders.Pain;
using Iso20022Library.Domain.Common.Enums;
using Iso20022Library.Domain.Common.Interfaces;
using Iso20022Library.Infrastructure.Xml;
using Iso20022Library.Messages.Payments.Pain.Generated.Pain00700103;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Iso20022Library.Tests.Builders
{
    /// <summary>
    /// Unit tests for the Pain00700103Builder class.
    /// Tests cover the construction of ISO 20022 pain.007.001.03 (Customer Payment Reversal V03) messages.
    /// </summary>
    [TestClass]
    public class Pain00700103BuilderTests
    {
        private Pain00700103Builder _builder = null!;

        [TestInitialize]
        public void Setup()
        {
            _builder = new Pain00700103Builder();
        }

        [TestMethod]
        public void Constructor_ShouldInitializeCorrectly()
        {
            // Arrange & Act
            var builder = new Pain00700103Builder();

            // Assert
            Assert.IsNotNull(builder);
            Assert.AreEqual(MessageType.Pain00700103, builder.MessageType);
        }

        [TestMethod]
        public void SetGroupHeader_WithRequiredParameters_ShouldSetHeaderCorrectly()
        {
            // Arrange
            var messageId = "MSG001";
            var creationDateTime = DateTime.UtcNow;
            var numberOfTransactions = "5";

            // Act
            var result = _builder.SetGroupHeader(messageId, creationDateTime, numberOfTransactions);

            // Assert
            Assert.AreSame(_builder, result);

            // Complete the required components for validation
            _builder.SetOriginalGroupInformation("ORIG001", "pain.001.001.03")
                   .AddOriginalPaymentInstruction("PMT001", "100.00", "EUR");

            var document = _builder.Build();

            Assert.IsNotNull(document.CstmrPmtRvsl);
            Assert.IsNotNull(document.CstmrPmtRvsl.GrpHdr);
            Assert.AreEqual(messageId, document.CstmrPmtRvsl.GrpHdr.MsgId);
            Assert.AreEqual(creationDateTime, document.CstmrPmtRvsl.GrpHdr.CreDtTm);
            Assert.AreEqual(numberOfTransactions, document.CstmrPmtRvsl.GrpHdr.NbOfTxs);
        }

        [TestMethod]
        public void SetGroupHeader_WithOptionalParameters_ShouldSetAllProperties()
        {
            // Arrange
            var messageId = "MSG001";
            var creationDateTime = DateTime.UtcNow;
            var numberOfTransactions = "5";
            var controlSum = 1000.50m;
            var groupReversal = true;
            var initiatingParty = new PartyIdentification43 { Nm = "Test Bank" };

            // Act
            _builder.SetGroupHeader(messageId, creationDateTime, numberOfTransactions,
                controlSum, groupReversal, initiatingParty);

            // Complete the required components for validation
            _builder.SetOriginalGroupInformation("ORIG001", "pain.001.001.03")
                   .AddOriginalPaymentInstruction("PMT001", "100.00", "EUR");

            // Assert
            var document = _builder.Build();
            var header = document.CstmrPmtRvsl.GrpHdr;

            Assert.AreEqual(controlSum, header.CtrlSum);
            Assert.IsTrue(header.CtrlSumSpecified);
            Assert.AreEqual(groupReversal, header.GrpRvsl);
            Assert.IsTrue(header.GrpRvslSpecified);
            Assert.AreEqual(initiatingParty.Nm, header.InitgPty.Nm);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetGroupHeader_WithNullMessageId_ShouldThrowException()
        {
            // Act & Assert
            _builder.SetGroupHeader(null!, DateTime.UtcNow, "1");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetGroupHeader_WithNullNumberOfTransactions_ShouldThrowException()
        {
            // Act & Assert
            _builder.SetGroupHeader("MSG001", DateTime.UtcNow, null!);
        }

        [TestMethod]
        public void SetOriginalGroupInformation_WithRequiredParameters_ShouldSetCorrectly()
        {
            // Arrange
            _builder.SetGroupHeader("MSG001", DateTime.UtcNow, "1");
            var originalMessageId = "ORIG001";
            var originalMessageNameId = "pain.001.001.03";

            // Act
            var result = _builder.SetOriginalGroupInformation(originalMessageId, originalMessageNameId);

            // Assert
            Assert.AreSame(_builder, result);

            // Complete the required components for validation
            _builder.AddOriginalPaymentInstruction("PMT001", "100.00", "EUR");

            var document = _builder.Build();
            var originalInfo = document.CstmrPmtRvsl.OrgnlGrpInf;

            Assert.IsNotNull(originalInfo);
            Assert.AreEqual(originalMessageId, originalInfo.OrgnlMsgId);
            Assert.AreEqual(originalMessageNameId, originalInfo.OrgnlMsgNmId);
        }

        [TestMethod]
        public void SetOriginalGroupInformation_WithOptionalParameters_ShouldSetAllProperties()
        {
            // Arrange
            _builder.SetGroupHeader("MSG001", DateTime.UtcNow, "1");
            var originalMessageId = "ORIG001";
            var originalMessageNameId = "pain.001.001.03";
            var originalCreationDateTime = DateTime.UtcNow.AddDays(-1);
            var reversalReasons = new[]
            {
                Pain00700103Builder.CreatePaymentReversalReason(
                    reason: Pain00700103Builder.CreateReversalReasonWithCode("DUPL"))
            };

            // Act
            _builder.SetOriginalGroupInformation(originalMessageId, originalMessageNameId,
                originalCreationDateTime, reversalReasons);

            // Complete the required components for validation
            _builder.AddOriginalPaymentInstruction("PMT001", "100.00", "EUR");

            // Assert
            var document = _builder.Build();
            var originalInfo = document.CstmrPmtRvsl.OrgnlGrpInf;

            Assert.AreEqual(originalCreationDateTime, originalInfo.OrgnlCreDtTm);
            Assert.IsTrue(originalInfo.OrgnlCreDtTmSpecified);
            Assert.AreEqual(1, originalInfo.RvslRsnInf.Count);
            Assert.AreEqual("DUPL", originalInfo.RvslRsnInf[0].Rsn.Cd);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetOriginalGroupInformation_WithoutGroupHeader_ShouldThrowException()
        {
            // Act & Assert
            _builder.SetOriginalGroupInformation("ORIG001", "pain.001.001.03");
        }

        [TestMethod]
        public void AddOriginalPaymentInstruction_WithRequiredParameters_ShouldAddCorrectly()
        {
            // Arrange
            SetupBasicMessage();
            var originalPaymentInformationId = "PMT001";

            // Act
            var result = _builder.AddOriginalPaymentInstruction(null, originalPaymentInformationId);

            // Assert
            Assert.AreSame(_builder, result);
            var document = _builder.Build();
            var instructions = document.CstmrPmtRvsl.OrgnlPmtInfAndRvsl;

            Assert.AreEqual(1, instructions.Count);
            Assert.AreEqual(originalPaymentInformationId, instructions[0].OrgnlPmtInfId);
        }

        [TestMethod]
        public void AddOriginalPaymentInstruction_WithOptionalParameters_ShouldSetAllProperties()
        {
            // Arrange
            SetupBasicMessage();
            var reversalPaymentInformationId = "RVSL001";
            var originalPaymentInformationId = "PMT001";
            var originalNumberOfTransactions = "3";
            var originalControlSum = 1500.75m;
            var batchBooking = true;
            var paymentInformationReversal = false;

            // Act
            _builder.AddOriginalPaymentInstruction(reversalPaymentInformationId, originalPaymentInformationId,
                originalNumberOfTransactions, originalControlSum, batchBooking, paymentInformationReversal);

            // Assert
            var document = _builder.Build();
            var instruction = document.CstmrPmtRvsl.OrgnlPmtInfAndRvsl[0];

            Assert.AreEqual(reversalPaymentInformationId, instruction.RvslPmtInfId);
            Assert.AreEqual(originalNumberOfTransactions, instruction.OrgnlNbOfTxs);
            Assert.AreEqual(originalControlSum, instruction.OrgnlCtrlSum);
            Assert.IsTrue(instruction.OrgnlCtrlSumSpecified);
            Assert.AreEqual(batchBooking, instruction.BtchBookg);
            Assert.IsTrue(instruction.BtchBookgSpecified);
            Assert.AreEqual(paymentInformationReversal, instruction.PmtInfRvsl);
            Assert.IsTrue(instruction.PmtInfRvslSpecified);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddOriginalPaymentInstruction_WithoutOriginalGroupInfo_ShouldThrowException()
        {
            // Arrange
            _builder.SetGroupHeader("MSG001", DateTime.UtcNow, "1");

            // Act & Assert
            _builder.AddOriginalPaymentInstruction(null, "PMT001");
        }

        [TestMethod]
        public void CreatePaymentTransactionReversal_WithAllParameters_ShouldCreateCorrectly()
        {
            // Arrange
            var reversalId = "RVSL001";
            var originalInstructionId = "INSTR001";
            var originalEndToEndId = "E2E001";
            var originalAmount = Pain00700103Builder.CreateCurrencyAndAmount(100.00m, "EUR");
            var reversedAmount = Pain00700103Builder.CreateCurrencyAndAmount(100.00m, "EUR");
            var chargeBearer = ChargeBearerType1Code.Shar;

            // Act
            var transaction = Pain00700103Builder.CreatePaymentTransactionReversal(
                reversalId, originalInstructionId, originalEndToEndId,
                originalAmount, reversedAmount, chargeBearer);

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
        public void AddPaymentTransactionReversal_ShouldAddToLastInstruction()
        {
            // Arrange
            SetupBasicMessage();
            _builder.AddOriginalPaymentInstruction(null, "PMT001");

            var transaction = Pain00700103Builder.CreatePaymentTransactionReversal(
                "RVSL001", "INSTR001", "E2E001",
                Pain00700103Builder.CreateCurrencyAndAmount(100.00m, "EUR"),
                Pain00700103Builder.CreateCurrencyAndAmount(100.00m, "EUR"));

            // Act
            var result = _builder.AddPaymentTransactionReversal(transaction);

            // Assert
            Assert.AreSame(_builder, result);
            var document = _builder.Build();
            var transactions = document.CstmrPmtRvsl.OrgnlPmtInfAndRvsl[0].TxInf;

            Assert.AreEqual(1, transactions.Count);
            Assert.AreEqual(transaction, transactions[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddPaymentTransactionReversal_WithoutPaymentInstructions_ShouldThrowException()
        {
            // Arrange
            SetupBasicMessage();
            var transaction = Pain00700103Builder.CreatePaymentTransactionReversal(
                "RVSL001", "INSTR001", "E2E001", null, null);

            // Act & Assert
            _builder.AddPaymentTransactionReversal(transaction);
        }

        [TestMethod]
        public void CreatePaymentReversalReason_WithAllParameters_ShouldCreateCorrectly()
        {
            // Arrange
            var originator = new PartyIdentification43 { Nm = "Test Bank" };
            var reason = Pain00700103Builder.CreateReversalReasonWithCode("DUPL");
            var additionalInfo = new[] { "Duplicate payment", "Requested by customer" };

            // Act
            var reversalReason = Pain00700103Builder.CreatePaymentReversalReason(
                originator, reason, additionalInfo);

            // Assert
            Assert.IsNotNull(reversalReason);
            Assert.AreEqual(originator, reversalReason.Orgtr);
            Assert.AreEqual(reason, reversalReason.Rsn);
            Assert.AreEqual(2, reversalReason.AddtlInf.Count);
            Assert.AreEqual("Duplicate payment", reversalReason.AddtlInf[0]);
            Assert.AreEqual("Requested by customer", reversalReason.AddtlInf[1]);
        }

        [TestMethod]
        public void CreateReversalReasonWithCode_ShouldCreateCorrectly()
        {
            // Arrange
            var code = "DUPL";

            // Act
            var reason = Pain00700103Builder.CreateReversalReasonWithCode(code);

            // Assert
            Assert.IsNotNull(reason);
            Assert.AreEqual(code, reason.Cd);
            Assert.IsNull(reason.Prtry);
        }

        [TestMethod]
        public void CreateReversalReasonWithProprietary_ShouldCreateCorrectly()
        {
            // Arrange
            var proprietary = "CUST_REQ";

            // Act
            var reason = Pain00700103Builder.CreateReversalReasonWithProprietary(proprietary);

            // Assert
            Assert.IsNotNull(reason);
            Assert.AreEqual(proprietary, reason.Prtry);
            Assert.IsNull(reason.Cd);
        }

        [TestMethod]
        public void CreateCurrencyAndAmount_ShouldCreateCorrectly()
        {
            // Arrange
            var amount = 123.45m;
            var currency = "USD";

            // Act
            var currencyAmount = Pain00700103Builder.CreateCurrencyAndAmount(amount, currency);

            // Assert
            Assert.IsNotNull(currencyAmount);
            Assert.AreEqual(amount, currencyAmount.Value);
            Assert.AreEqual(currency, currencyAmount.Ccy);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateCurrencyAndAmount_WithNullCurrency_ShouldThrowException()
        {
            // Act & Assert
            Pain00700103Builder.CreateCurrencyAndAmount(100.00m, null!);
        }

        [TestMethod]
        public void AddSupplementaryData_ShouldAddCorrectly()
        {
            // Arrange
            SetupBasicMessage();
            _builder.AddOriginalPaymentInstruction(null, "PMT001");

            var supplementaryData = new SupplementaryData1
            {
                PlcAndNm = "Test Data",
                Envlp = new SupplementaryDataEnvelope1()
            };

            // Act
            var result = _builder.AddSupplementaryData(supplementaryData);

            // Assert
            Assert.AreSame(_builder, result);
            var document = _builder.Build();

            Assert.AreEqual(1, document.CstmrPmtRvsl.SplmtryData.Count);
            Assert.AreEqual(supplementaryData, document.CstmrPmtRvsl.SplmtryData[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddSupplementaryData_WithoutInitialization_ShouldThrowException()
        {
            // Arrange
            var supplementaryData = new SupplementaryData1();

            // Act & Assert
            _builder.AddSupplementaryData(supplementaryData);
        }

        [TestMethod]
        public void Build_WithCompleteMessage_ShouldReturnValidDocument()
        {
            // Arrange
            SetupCompleteMessage();

            // Act
            var document = _builder.Build();

            // Assert
            Assert.IsNotNull(document);
            Assert.IsNotNull(document.CstmrPmtRvsl);
            Assert.IsNotNull(document.CstmrPmtRvsl.GrpHdr);
            Assert.IsNotNull(document.CstmrPmtRvsl.OrgnlGrpInf);
            Assert.IsTrue(document.CstmrPmtRvsl.OrgnlPmtInfAndRvsl.Count > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Build_WithoutGroupHeader_ShouldThrowException()
        {
            // Act & Assert
            _builder.Build();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Build_WithoutOriginalGroupInfo_ShouldThrowException()
        {
            // Arrange
            _builder.SetGroupHeader("MSG001", DateTime.UtcNow, "1");

            // Act & Assert
            _builder.Build();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Build_WithoutPaymentInstructions_ShouldThrowException()
        {
            // Arrange
            SetupBasicMessage();

            // Act & Assert
            _builder.Build();
        }

        [TestMethod]
        public void BuildXml_WithCompleteMessage_ShouldReturnValidXml()
        {
            // Arrange
            SetupCompleteMessage();

            // Act
            var xml = _builder.BuildXml();

            // Assert
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.Contains("<?xml"));
            Assert.IsTrue(xml.Contains("CstmrPmtRvsl"));
            Assert.IsTrue(xml.Contains("GrpHdr"));
            Assert.IsTrue(xml.Contains("OrgnlGrpInf"));
            Assert.IsTrue(xml.Contains("OrgnlPmtInfAndRvsl"));
        }

        [TestMethod]
        public void MessageBuilderFactory_ShouldCreatePain00700103Builder()
        {
            // Arrange
            var factory = new MessageBuilderFactory();

            // Act
            var builder = factory.GetBuilder(MessageType.Pain00700103);

            // Assert
            Assert.IsNotNull(builder);
            Assert.IsInstanceOfType(builder, typeof(Pain00700103Builder));
            var concreteBuilder = (Pain00700103Builder)builder;
            Assert.AreEqual(MessageType.Pain00700103, concreteBuilder.MessageType);
        }

        [TestMethod]
        public void CompleteWorkflow_ShouldCreateValidReversalMessage()
        {
            // Arrange & Act
            var document = _builder
                .SetGroupHeader("MSG001", DateTime.UtcNow, "2", 200.00m, true,
                    new PartyIdentification43 { Nm = "Initiating Bank" })
                .SetOriginalGroupInformation("ORIG001", "pain.001.001.03", DateTime.UtcNow.AddDays(-1))
                .AddOriginalPaymentInstruction("RVSL001", "PMT001", "2", 200.00m, true, false,
                    new[] { Pain00700103Builder.CreatePaymentReversalReason(
                        reason: Pain00700103Builder.CreateReversalReasonWithCode("DUPL")) })
                .AddPaymentTransactionReversal(Pain00700103Builder.CreatePaymentTransactionReversal(
                    "RVSL_TXN001", "INSTR001", "E2E001",
                    Pain00700103Builder.CreateCurrencyAndAmount(100.00m, "EUR"),
                    Pain00700103Builder.CreateCurrencyAndAmount(100.00m, "EUR"),
                    ChargeBearerType1Code.Shar))
                .AddPaymentTransactionReversal(Pain00700103Builder.CreatePaymentTransactionReversal(
                    "RVSL_TXN002", "INSTR002", "E2E002",
                    Pain00700103Builder.CreateCurrencyAndAmount(100.00m, "EUR"),
                    Pain00700103Builder.CreateCurrencyAndAmount(100.00m, "EUR")))
                .Build();

            // Assert
            Assert.IsNotNull(document);
            Assert.AreEqual("MSG001", document.CstmrPmtRvsl.GrpHdr.MsgId);
            Assert.AreEqual("2", document.CstmrPmtRvsl.GrpHdr.NbOfTxs);
            Assert.AreEqual(200.00m, document.CstmrPmtRvsl.GrpHdr.CtrlSum);
            Assert.IsTrue(document.CstmrPmtRvsl.GrpHdr.GrpRvsl);
            Assert.AreEqual("Initiating Bank", document.CstmrPmtRvsl.GrpHdr.InitgPty.Nm);

            Assert.AreEqual("ORIG001", document.CstmrPmtRvsl.OrgnlGrpInf.OrgnlMsgId);
            Assert.AreEqual("pain.001.001.03", document.CstmrPmtRvsl.OrgnlGrpInf.OrgnlMsgNmId);

            Assert.AreEqual(1, document.CstmrPmtRvsl.OrgnlPmtInfAndRvsl.Count);
            var instruction = document.CstmrPmtRvsl.OrgnlPmtInfAndRvsl[0];
            Assert.AreEqual("RVSL001", instruction.RvslPmtInfId);
            Assert.AreEqual("PMT001", instruction.OrgnlPmtInfId);
            Assert.AreEqual(2, instruction.TxInf.Count);
            Assert.AreEqual("RVSL_TXN001", instruction.TxInf[0].RvslId);
            Assert.AreEqual("RVSL_TXN002", instruction.TxInf[1].RvslId);
        }

        private void SetupBasicMessage()
        {
            _builder.SetGroupHeader("MSG001", DateTime.UtcNow, "1")
                   .SetOriginalGroupInformation("ORIG001", "pain.001.001.03");
        }

        private void SetupCompleteMessage()
        {
            SetupBasicMessage();
            _builder.AddOriginalPaymentInstruction(null, "PMT001");
        }
    }
}
