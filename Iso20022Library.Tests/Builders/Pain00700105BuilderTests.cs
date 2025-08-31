using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Iso20022Library.Application.Builders;
using Iso20022Library.Application.Builders.Pain;
using Iso20022Library.Domain.Common.Enums;
using Iso20022Library.Messages.Payments.Pain.Generated.Pain00700105;

namespace Iso20022Library.Tests.Builders
{
    /// <summary>
    /// Unit tests for the Pain00700105Builder class.
    /// Tests cover the construction of ISO 20022 pain.007.001.05 (Customer Payment Reversal V05) messages.
    /// </summary>
    [TestClass]
    public class Pain00700105BuilderTests
    {
        private Pain00700105Builder _builder = null!;

        [TestInitialize]
        public void Setup()
        {
            _builder = new Pain00700105Builder();
        }

        [TestMethod]
        public void Constructor_ShouldInitializeCorrectly()
        {
            // Arrange & Act
            var builder = new Pain00700105Builder();

            // Assert
            Assert.IsNotNull(builder);
            Assert.AreEqual(MessageType.Pain00700105, builder.MessageType);
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
            _builder.SetOriginalGroupInformation("ORIG001", "pain.001.001.05")
                   .AddOriginalPaymentInstruction(null, "PMT001");

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
            _builder.SetOriginalGroupInformation("ORIG001", "pain.001.001.05")
                   .AddOriginalPaymentInstruction(null, "PMT001");

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
            var originalMessageNameId = "pain.001.001.05";

            // Act
            var result = _builder.SetOriginalGroupInformation(originalMessageId, originalMessageNameId);

            // Assert
            Assert.AreSame(_builder, result);

            // Complete the required components for validation
            _builder.AddOriginalPaymentInstruction(null, "PMT001");

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
            var originalMessageNameId = "pain.001.001.05";
            var originalCreationDateTime = DateTime.UtcNow.AddDays(-1);
            var reversalReasons = new[]
            {
                Pain00700105Builder.CreatePaymentReversalReason(
                    reason: Pain00700105Builder.CreateReversalReasonWithCode("DUPL"))
            };

            // Act
            _builder.SetOriginalGroupInformation(originalMessageId, originalMessageNameId,
                originalCreationDateTime, reversalReasons);

            // Complete the required components for validation
            _builder.AddOriginalPaymentInstruction(null, "PMT001");

            // Assert
            var document = _builder.Build();
            var originalInfo = document.CstmrPmtRvsl.OrgnlGrpInf;

            Assert.AreEqual(originalCreationDateTime, originalInfo.OrgnlCreDtTm);
            Assert.IsTrue(originalInfo.OrgnlCreDtTmSpecified);
            Assert.AreEqual(1, originalInfo.RvslRsnInf.Count);
            Assert.AreEqual("DUPL", originalInfo.RvslRsnInf[0].Rsn.Cd);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetOriginalGroupInformation_WithNullOriginalMessageId_ShouldThrowException()
        {
            // Act & Assert
            _builder.SetOriginalGroupInformation(null!, "pain.001.001.05");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetOriginalGroupInformation_WithNullOriginalMessageNameId_ShouldThrowException()
        {
            // Act & Assert
            _builder.SetOriginalGroupInformation("ORIG001", null!);
        }

        [TestMethod]
        public void AddOriginalPaymentInstruction_WithRequiredParameters_ShouldAddCorrectly()
        {
            // Arrange
            _builder.SetGroupHeader("MSG001", DateTime.UtcNow, "1")
                   .SetOriginalGroupInformation("ORIG001", "pain.001.001.05");
            var originalPaymentInformationId = "PMT001";

            // Act
            var result = _builder.AddOriginalPaymentInstruction(null, originalPaymentInformationId);

            // Assert
            Assert.AreSame(_builder, result);
            var document = _builder.Build();
            var paymentInstruction = document.CstmrPmtRvsl.OrgnlPmtInfAndRvsl.First();

            Assert.AreEqual(originalPaymentInformationId, paymentInstruction.OrgnlPmtInfId);
        }

        [TestMethod]
        public void AddOriginalPaymentInstruction_WithOptionalParameters_ShouldSetAllProperties()
        {
            // Arrange
            _builder.SetGroupHeader("MSG001", DateTime.UtcNow, "1")
                   .SetOriginalGroupInformation("ORIG001", "pain.001.001.05");

            var reversalPaymentInformationId = "RVSL001";
            var originalPaymentInformationId = "PMT001";
            var originalNumberOfTransactions = "10";
            var originalControlSum = 5000.00m;
            var batchBooking = true;
            var paymentInformationReversal = false;
            var reversalReasons = new[]
            {
                Pain00700105Builder.CreatePaymentReversalReason(
                    reason: Pain00700105Builder.CreateReversalReasonWithCode("CUST"))
            };

            // Act
            _builder.AddOriginalPaymentInstruction(
                reversalPaymentInformationId,
                originalPaymentInformationId,
                originalNumberOfTransactions,
                originalControlSum,
                batchBooking,
                paymentInformationReversal,
                reversalReasons);

            // Assert
            var document = _builder.Build();
            var paymentInstruction = document.CstmrPmtRvsl.OrgnlPmtInfAndRvsl.First();

            Assert.AreEqual(reversalPaymentInformationId, paymentInstruction.RvslPmtInfId);
            Assert.AreEqual(originalPaymentInformationId, paymentInstruction.OrgnlPmtInfId);
            Assert.AreEqual(originalNumberOfTransactions, paymentInstruction.OrgnlNbOfTxs);
            Assert.AreEqual(originalControlSum, paymentInstruction.OrgnlCtrlSum);
            Assert.IsTrue(paymentInstruction.OrgnlCtrlSumSpecified);
            Assert.AreEqual(batchBooking, paymentInstruction.BtchBookg);
            Assert.IsTrue(paymentInstruction.BtchBookgSpecified);
            Assert.AreEqual(paymentInformationReversal, paymentInstruction.PmtInfRvsl);
            Assert.IsTrue(paymentInstruction.PmtInfRvslSpecified);
            Assert.AreEqual(1, paymentInstruction.RvslRsnInf.Count);
            Assert.AreEqual("CUST", paymentInstruction.RvslRsnInf[0].Rsn.Cd);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddOriginalPaymentInstruction_WithNullOriginalPaymentInformationId_ShouldThrowException()
        {
            // Act & Assert
            _builder.AddOriginalPaymentInstruction(null, null!);
        }

        [TestMethod]
        public void AddPaymentTransactionReversal_WithValidTransaction_ShouldAddCorrectly()
        {
            // Arrange
            _builder.SetGroupHeader("MSG001", DateTime.UtcNow, "1")
                   .SetOriginalGroupInformation("ORIG001", "pain.001.001.05")
                   .AddOriginalPaymentInstruction(null, "PMT001");

            var transaction = Pain00700105Builder.CreatePaymentTransactionReversal(
                "RVSL001", "INST001", "E2E001");

            // Act
            var result = _builder.AddPaymentTransactionReversal(transaction);

            // Assert
            Assert.AreSame(_builder, result);
            var document = _builder.Build();
            var paymentInstruction = document.CstmrPmtRvsl.OrgnlPmtInfAndRvsl.First();
            var addedTransaction = paymentInstruction.TxInf.First();

            Assert.AreEqual("RVSL001", addedTransaction.RvslId);
            Assert.AreEqual("INST001", addedTransaction.OrgnlInstrId);
            Assert.AreEqual("E2E001", addedTransaction.OrgnlEndToEndId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddPaymentTransactionReversal_WithNullTransaction_ShouldThrowException()
        {
            // Act & Assert
            _builder.AddPaymentTransactionReversal(null!);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddPaymentTransactionReversal_WithoutPaymentInstruction_ShouldThrowException()
        {
            // Arrange
            var transaction = Pain00700105Builder.CreatePaymentTransactionReversal(
                "RVSL001", "INST001", "E2E001");

            // Act & Assert
            _builder.AddPaymentTransactionReversal(transaction);
        }

        [TestMethod]
        public void CreatePaymentTransactionReversal_WithAllParameters_ShouldCreateCorrectly()
        {
            // Arrange
            var reversalId = "RVSL001";
            var originalInstructionId = "INST001";
            var originalEndToEndId = "E2E001";
            var originalAmount = Pain00700105Builder.CreateCurrencyAndAmount(100.00m, "EUR");
            var reversedAmount = Pain00700105Builder.CreateCurrencyAndAmount(50.00m, "EUR");
            var chargeBearer = ChargeBearerType1Code.Slev;
            var reversalReasons = new[]
            {
                Pain00700105Builder.CreatePaymentReversalReason(
                    reason: Pain00700105Builder.CreateReversalReasonWithCode("DUPL"))
            };

            // Act
            var transaction = Pain00700105Builder.CreatePaymentTransactionReversal(
                reversalId, originalInstructionId, originalEndToEndId,
                originalAmount, reversedAmount, chargeBearer, reversalReasons);

            // Assert
            Assert.IsNotNull(transaction);
            Assert.AreEqual(reversalId, transaction.RvslId);
            Assert.AreEqual(originalInstructionId, transaction.OrgnlInstrId);
            Assert.AreEqual(originalEndToEndId, transaction.OrgnlEndToEndId);
            Assert.AreEqual(originalAmount.Value, transaction.OrgnlInstdAmt.Value);
            Assert.AreEqual(originalAmount.Ccy, transaction.OrgnlInstdAmt.Ccy);
            Assert.AreEqual(reversedAmount.Value, transaction.RvsdInstdAmt.Value);
            Assert.AreEqual(reversedAmount.Ccy, transaction.RvsdInstdAmt.Ccy);
            Assert.AreEqual(chargeBearer, transaction.ChrgBr);
            Assert.IsTrue(transaction.ChrgBrSpecified);
            Assert.AreEqual(1, transaction.RvslRsnInf.Count);
            Assert.AreEqual("DUPL", transaction.RvslRsnInf[0].Rsn.Cd);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreatePaymentTransactionReversal_WithNullOriginalInstructionId_ShouldThrowException()
        {
            // Act & Assert
            Pain00700105Builder.CreatePaymentTransactionReversal(null, null!, "E2E001");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreatePaymentTransactionReversal_WithNullOriginalEndToEndId_ShouldThrowException()
        {
            // Act & Assert
            Pain00700105Builder.CreatePaymentTransactionReversal(null, "INST001", null!);
        }

        [TestMethod]
        public void CreatePaymentReversalReason_WithAllParameters_ShouldCreateCorrectly()
        {
            // Arrange
            var originator = Pain00700105Builder.CreatePartyIdentification("Test Bank");
            var reason = Pain00700105Builder.CreateReversalReasonWithCode("DUPL");
            var additionalInfo = new[] { "Duplicate payment", "Customer request" };

            // Act
            var reversalReason = Pain00700105Builder.CreatePaymentReversalReason(
                originator, reason, additionalInfo);

            // Assert
            Assert.IsNotNull(reversalReason);
            Assert.AreEqual("Test Bank", reversalReason.Orgtr.Nm);
            Assert.AreEqual("DUPL", reversalReason.Rsn.Cd);
            Assert.AreEqual(2, reversalReason.AddtlInf.Count);
            Assert.IsTrue(reversalReason.AddtlInf.Contains("Duplicate payment"));
            Assert.IsTrue(reversalReason.AddtlInf.Contains("Customer request"));
        }

        [TestMethod]
        public void CreateReversalReasonWithCode_ShouldCreateCorrectly()
        {
            // Arrange
            var reasonCode = "CUST";

            // Act
            var reason = Pain00700105Builder.CreateReversalReasonWithCode(reasonCode);

            // Assert
            Assert.IsNotNull(reason);
            Assert.AreEqual(reasonCode, reason.Cd);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateReversalReasonWithCode_WithNullCode_ShouldThrowException()
        {
            // Act & Assert
            Pain00700105Builder.CreateReversalReasonWithCode(null!);
        }

        [TestMethod]
        public void CreateReversalReasonWithProprietary_ShouldCreateCorrectly()
        {
            // Arrange
            var proprietaryReason = "CUSTOM_REASON";

            // Act
            var reason = Pain00700105Builder.CreateReversalReasonWithProprietary(proprietaryReason);

            // Assert
            Assert.IsNotNull(reason);
            Assert.AreEqual(proprietaryReason, reason.Prtry);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateReversalReasonWithProprietary_WithNullReason_ShouldThrowException()
        {
            // Act & Assert
            Pain00700105Builder.CreateReversalReasonWithProprietary(null!);
        }

        [TestMethod]
        public void CreateCurrencyAndAmount_ShouldCreateCorrectly()
        {
            // Arrange
            var amount = 123.45m;
            var currency = "EUR";

            // Act
            var result = Pain00700105Builder.CreateCurrencyAndAmount(amount, currency);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(amount, result.Value);
            Assert.AreEqual(currency, result.Ccy);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateCurrencyAndAmount_WithNullCurrency_ShouldThrowException()
        {
            // Act & Assert
            Pain00700105Builder.CreateCurrencyAndAmount(100.00m, null!);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateCurrencyAndAmount_WithNegativeAmount_ShouldThrowException()
        {
            // Act & Assert
            Pain00700105Builder.CreateCurrencyAndAmount(-100.00m, "EUR");
        }

        [TestMethod]
        public void CreatePartyIdentification_WithName_ShouldCreateCorrectly()
        {
            // Arrange
            var name = "Test Party";

            // Act
            var party = Pain00700105Builder.CreatePartyIdentification(name);

            // Assert
            Assert.IsNotNull(party);
            Assert.AreEqual(name, party.Nm);
        }

        [TestMethod]
        public void AddSupplementaryData_ShouldAddCorrectly()
        {
            // Arrange
            _builder.SetGroupHeader("MSG001", DateTime.UtcNow, "1")
                   .SetOriginalGroupInformation("ORIG001", "pain.001.001.05")
                   .AddOriginalPaymentInstruction(null, "PMT001");

            var supplementaryData = new SupplementaryData1();

            // Act
            var result = _builder.AddSupplementaryData(supplementaryData);

            // Assert
            Assert.AreSame(_builder, result);
            var document = _builder.Build();
            Assert.AreEqual(1, document.CstmrPmtRvsl.SplmtryData.Count);
            Assert.AreSame(supplementaryData, document.CstmrPmtRvsl.SplmtryData.First());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddSupplementaryData_WithNullData_ShouldThrowException()
        {
            // Act & Assert
            _builder.AddSupplementaryData(null!);
        }

        [TestMethod]
        public void Build_WithCompleteMessage_ShouldReturnDocument()
        {
            // Arrange
            _builder.SetGroupHeader("MSG001", DateTime.UtcNow, "1")
                   .SetOriginalGroupInformation("ORIG001", "pain.001.001.05")
                   .AddOriginalPaymentInstruction(null, "PMT001");

            // Act
            var document = _builder.Build();

            // Assert
            Assert.IsNotNull(document);
            Assert.IsNotNull(document.CstmrPmtRvsl);
            Assert.IsNotNull(document.CstmrPmtRvsl.GrpHdr);
            Assert.IsNotNull(document.CstmrPmtRvsl.OrgnlGrpInf);
            Assert.AreEqual(1, document.CstmrPmtRvsl.OrgnlPmtInfAndRvsl.Count);
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
        public void Build_WithoutOriginalGroupInformation_ShouldThrowException()
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
            _builder.SetGroupHeader("MSG001", DateTime.UtcNow, "1")
                   .SetOriginalGroupInformation("ORIG001", "pain.001.001.05");

            // Act & Assert
            _builder.Build();
        }

        [TestMethod]
        public void BuildXml_ShouldReturnValidXml()
        {
            // Arrange
            _builder.SetGroupHeader("MSG001", DateTime.UtcNow, "1")
                   .SetOriginalGroupInformation("ORIG001", "pain.001.001.05")
                   .AddOriginalPaymentInstruction(null, "PMT001");

            // Act
            var xml = _builder.BuildXml();

            // Assert
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.Contains("<?xml"));
            Assert.IsTrue(xml.Contains("CstmrPmtRvsl"));
            Assert.IsTrue(xml.Contains("MSG001"));
        }

        [TestMethod]
        public void BuildXml_WithDocumentParameter_ShouldReturnValidXml()
        {
            // Arrange
            var document = new Document
            {
                CstmrPmtRvsl = new CustomerPaymentReversalV05
                {
                    GrpHdr = new GroupHeader56
                    {
                        MsgId = "TEST001",
                        CreDtTm = DateTime.UtcNow,
                        NbOfTxs = "1"
                    }
                }
            };

            // Act
            var xml = _builder.BuildXml(document);

            // Assert
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.Contains("<?xml"));
            Assert.IsTrue(xml.Contains("TEST001"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildXml_WithNullDocument_ShouldThrowException()
        {
            // Act & Assert
            _builder.BuildXml(null!);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildXml_WithInvalidDocumentType_ShouldThrowException()
        {
            // Act & Assert
            _builder.BuildXml("invalid document");
        }

        [TestMethod]
        public void MessageBuilderFactory_ShouldCreatePain00700105Builder()
        {
            // Arrange
            var factory = new MessageBuilderFactory();

            // Act
            var builder = factory.GetBuilder(MessageType.Pain00700105);

            // Assert
            Assert.IsNotNull(builder);
            Assert.IsInstanceOfType(builder, typeof(Pain00700105Builder));
            var typedBuilder = (Pain00700105Builder)builder;
            Assert.AreEqual(MessageType.Pain00700105, typedBuilder.MessageType);
        }

        [TestMethod]
        public void CompleteWorkflow_ShouldCreateValidMessage()
        {
            // Arrange
            var messageId = "REV001";
            var creationDateTime = DateTime.UtcNow;
            var numberOfTransactions = "2";
            var controlSum = 250.50m;
            var initiatingParty = Pain00700105Builder.CreatePartyIdentification("Central Bank");

            var originalMessageId = "ORIG123";
            var originalMessageNameId = "pain.001.001.05";
            var originalCreationDateTime = DateTime.UtcNow.AddDays(-1);

            var reversalReasons = new[]
            {
                Pain00700105Builder.CreatePaymentReversalReason(
                    originator: Pain00700105Builder.CreatePartyIdentification("Compliance Dept"),
                    reason: Pain00700105Builder.CreateReversalReasonWithCode("CUST"),
                    additionalInformation: new[] { "Customer request for reversal" })
            };

            var originalAmount1 = Pain00700105Builder.CreateCurrencyAndAmount(150.25m, "EUR");
            var originalAmount2 = Pain00700105Builder.CreateCurrencyAndAmount(100.25m, "EUR");

            var transaction1 = Pain00700105Builder.CreatePaymentTransactionReversal(
                "TXN_REV_001", "INST001", "E2E001", originalAmount1, originalAmount1,
                ChargeBearerType1Code.Slev, reversalReasons);

            var transaction2 = Pain00700105Builder.CreatePaymentTransactionReversal(
                "TXN_REV_002", "INST002", "E2E002", originalAmount2, originalAmount2,
                ChargeBearerType1Code.Debt, reversalReasons);

            // Act
            var document = _builder
                .SetGroupHeader(messageId, creationDateTime, numberOfTransactions, controlSum, false, initiatingParty)
                .SetOriginalGroupInformation(originalMessageId, originalMessageNameId, originalCreationDateTime, reversalReasons)
                .AddOriginalPaymentInstruction("REV_PMT_001", "PMT001", "2", controlSum, true, false, reversalReasons)
                .AddPaymentTransactionReversal(transaction1)
                .AddPaymentTransactionReversal(transaction2)
                .Build();

            // Assert
            Assert.IsNotNull(document);
            Assert.IsNotNull(document.CstmrPmtRvsl);

            // Verify group header
            var header = document.CstmrPmtRvsl.GrpHdr;
            Assert.AreEqual(messageId, header.MsgId);
            Assert.AreEqual(numberOfTransactions, header.NbOfTxs);
            Assert.AreEqual(controlSum, header.CtrlSum);
            Assert.IsFalse(header.GrpRvsl);
            Assert.AreEqual("Central Bank", header.InitgPty.Nm);

            // Verify original group information
            var originalInfo = document.CstmrPmtRvsl.OrgnlGrpInf;
            Assert.AreEqual(originalMessageId, originalInfo.OrgnlMsgId);
            Assert.AreEqual(originalMessageNameId, originalInfo.OrgnlMsgNmId);
            Assert.AreEqual(1, originalInfo.RvslRsnInf.Count);

            // Verify payment instruction
            var paymentInstruction = document.CstmrPmtRvsl.OrgnlPmtInfAndRvsl.First();
            Assert.AreEqual("REV_PMT_001", paymentInstruction.RvslPmtInfId);
            Assert.AreEqual("PMT001", paymentInstruction.OrgnlPmtInfId);
            Assert.AreEqual(2, paymentInstruction.TxInf.Count);

            // Verify transactions
            var firstTransaction = paymentInstruction.TxInf[0];
            Assert.AreEqual("TXN_REV_001", firstTransaction.RvslId);
            Assert.AreEqual("INST001", firstTransaction.OrgnlInstrId);
            Assert.AreEqual("E2E001", firstTransaction.OrgnlEndToEndId);

            var secondTransaction = paymentInstruction.TxInf[1];
            Assert.AreEqual("TXN_REV_002", secondTransaction.RvslId);
            Assert.AreEqual("INST002", secondTransaction.OrgnlInstrId);
            Assert.AreEqual("E2E002", secondTransaction.OrgnlEndToEndId);
        }
    }
}
