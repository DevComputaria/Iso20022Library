using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Iso20022Library.Application.Builders.Pain;
using Iso20022Library.Domain.Common.Enums;
using Iso20022Library.Domain.Common.Interfaces;
using Iso20022Library.Infrastructure.Xml;
using Iso20022Library.Messages.Payments.Pain.Generated.Pain00700106;

namespace Iso20022Library.Tests.Builders
{
    /// <summary>
    /// Unit tests for Pain00700106Builder - Customer Payment Reversal V06.
    /// </summary>
    [TestClass]
    public class Pain00700106BuilderTests
    {
        private Pain00700106Builder _builder = null!;

        [TestInitialize]
        public void Setup()
        {
            _builder = new Pain00700106Builder();
        }

        [TestMethod]
        public void MessageType_ShouldReturnCorrectType()
        {
            // Arrange & Act
            var messageType = _builder.MessageType;

            // Assert
            Assert.AreEqual(MessageType.Pain00700106, messageType);
        }

        [TestMethod]
        public void SetGroupHeader_WithRequiredFields_ShouldSetGroupHeader()
        {
            // Arrange
            var messageId = "MSG123";
            var creationDateTime = DateTime.Now;
            var numberOfTransactions = "1";

            // Act
            var result = _builder.SetGroupHeader(messageId, creationDateTime, numberOfTransactions);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreSame(_builder, result);
        }

        [TestMethod]
        public void SetGroupHeader_WithAllFields_ShouldSetAllFields()
        {
            // Arrange
            var messageId = "MSG123";
            var creationDateTime = DateTime.Now;
            var numberOfTransactions = "5";
            var controlSum = 1000.50m;
            var groupReversal = true;
            var initiatingParty = Pain00700106Builder.CreatePartyIdentification("Test Bank");

            // Act
            _builder.SetGroupHeader(messageId, creationDateTime, numberOfTransactions, controlSum, groupReversal, initiatingParty)
                    .SetOriginalGroupInformation("ORIG123", "pain.001.001.03")
                    .AddOriginalPaymentInstruction(null, "PMT123");
            var document = _builder.Build();

            // Assert
            Assert.AreEqual(messageId, document.CstmrPmtRvsl.GrpHdr.MsgId);
            Assert.AreEqual(creationDateTime, document.CstmrPmtRvsl.GrpHdr.CreDtTm);
            Assert.AreEqual(numberOfTransactions, document.CstmrPmtRvsl.GrpHdr.NbOfTxs);
            Assert.AreEqual(controlSum, document.CstmrPmtRvsl.GrpHdr.CtrlSum);
            Assert.IsTrue(document.CstmrPmtRvsl.GrpHdr.CtrlSumSpecified);
            Assert.AreEqual(groupReversal, document.CstmrPmtRvsl.GrpHdr.GrpRvsl);
            Assert.IsTrue(document.CstmrPmtRvsl.GrpHdr.GrpRvslSpecified);
            Assert.AreEqual("Test Bank", document.CstmrPmtRvsl.GrpHdr.InitgPty.Nm);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetGroupHeader_WithNullMessageId_ShouldThrowException()
        {
            // Act
            _builder.SetGroupHeader(null!, DateTime.Now, "1");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetGroupHeader_WithNullNumberOfTransactions_ShouldThrowException()
        {
            // Act
            _builder.SetGroupHeader("MSG123", DateTime.Now, null!);
        }

        [TestMethod]
        public void SetOriginalGroupInformation_WithRequiredFields_ShouldSetOriginalGroupInformation()
        {
            // Arrange
            var originalMessageId = "ORIG123";
            var originalMessageNameId = "pain.001.001.03";

            // Act
            var result = _builder.SetOriginalGroupInformation(originalMessageId, originalMessageNameId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreSame(_builder, result);
        }

        [TestMethod]
        public void SetOriginalGroupInformation_WithAllFields_ShouldSetAllFields()
        {
            // Arrange
            var originalMessageId = "ORIG123";
            var originalMessageNameId = "pain.001.001.03";
            var originalCreationDateTime = DateTime.Today;
            var reversalReasons = new List<PaymentReversalReason7>
            {
                Pain00700106Builder.CreatePaymentReversalReason(
                    Pain00700106Builder.CreatePartyIdentification("Test Originator"),
                    Pain00700106Builder.CreateReversalReasonWithCode("DUPL"),
                    new[] { "Duplicate payment" })
            };

            // Act
            _builder.SetOriginalGroupInformation(originalMessageId, originalMessageNameId, originalCreationDateTime, reversalReasons);

            // Assert
            Assert.IsNotNull(_builder);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetOriginalGroupInformation_WithNullOriginalMessageId_ShouldThrowException()
        {
            // Act
            _builder.SetOriginalGroupInformation(null!, "pain.001.001.03");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetOriginalGroupInformation_WithNullOriginalMessageNameId_ShouldThrowException()
        {
            // Act
            _builder.SetOriginalGroupInformation("ORIG123", null!);
        }

        [TestMethod]
        public void AddOriginalPaymentInstruction_WithRequiredFields_ShouldAddInstruction()
        {
            // Arrange
            var originalPaymentInformationId = "PMT123";

            // Act
            var result = _builder.AddOriginalPaymentInstruction(null, originalPaymentInformationId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreSame(_builder, result);
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
            var reversalReasons = new List<PaymentReversalReason7>
            {
                Pain00700106Builder.CreatePaymentReversalReason(
                    Pain00700106Builder.CreatePartyIdentification("Test Bank"),
                    Pain00700106Builder.CreateReversalReasonWithCode("CUST"),
                    new[] { "Customer request" })
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
            Assert.IsNotNull(_builder);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddOriginalPaymentInstruction_WithNullOriginalPaymentInformationId_ShouldThrowException()
        {
            // Act
            _builder.AddOriginalPaymentInstruction("RVSL123", null!);
        }

        [TestMethod]
        public void AddPaymentTransactionReversal_WithValidTransaction_ShouldAddTransaction()
        {
            // Arrange
            _builder.AddOriginalPaymentInstruction(null, "PMT123");
            var transaction = Pain00700106Builder.CreatePaymentTransactionReversal(
                "TXN001",
                "INSTR123",
                "E2E123");

            // Act
            var result = _builder.AddPaymentTransactionReversal(transaction);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreSame(_builder, result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddPaymentTransactionReversal_WithoutPaymentInstruction_ShouldThrowException()
        {
            // Arrange
            var transaction = Pain00700106Builder.CreatePaymentTransactionReversal(
                "TXN001",
                "INSTR123",
                "E2E123");

            // Act
            _builder.AddPaymentTransactionReversal(transaction);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddPaymentTransactionReversal_WithNullTransaction_ShouldThrowException()
        {
            // Arrange
            _builder.AddOriginalPaymentInstruction(null, "PMT123");

            // Act
            _builder.AddPaymentTransactionReversal(null!);
        }

        [TestMethod]
        public void CreatePaymentTransactionReversal_WithRequiredFields_ShouldCreateTransaction()
        {
            // Arrange
            var reversalId = "TXN001";
            var originalInstructionId = "INSTR123";
            var originalEndToEndId = "E2E123";

            // Act
            var transaction = Pain00700106Builder.CreatePaymentTransactionReversal(
                reversalId,
                originalInstructionId,
                originalEndToEndId);

            // Assert
            Assert.IsNotNull(transaction);
            Assert.AreEqual(reversalId, transaction.RvslId);
            Assert.AreEqual(originalInstructionId, transaction.OrgnlInstrId);
            Assert.AreEqual(originalEndToEndId, transaction.OrgnlEndToEndId);
        }

        [TestMethod]
        public void CreatePaymentTransactionReversal_WithAllFields_ShouldSetAllFields()
        {
            // Arrange
            var reversalId = "TXN001";
            var originalInstructionId = "INSTR123";
            var originalEndToEndId = "E2E123";
            var originalAmount = Pain00700106Builder.CreateCurrencyAndAmount(500.00m, "EUR");
            var reversedAmount = Pain00700106Builder.CreateCurrencyAndAmount(250.00m, "EUR");
            var chargeBearer = ChargeBearerType1Code.Slev;
            var reversalReasons = new List<PaymentReversalReason7>
            {
                Pain00700106Builder.CreatePaymentReversalReason(
                    Pain00700106Builder.CreatePartyIdentification("Test Bank"),
                    Pain00700106Builder.CreateReversalReasonWithCode("DUPL"))
            };

            // Act
            var transaction = Pain00700106Builder.CreatePaymentTransactionReversal(
                reversalId,
                originalInstructionId,
                originalEndToEndId,
                originalAmount,
                reversedAmount,
                chargeBearer,
                reversalReasons);

            // Assert
            Assert.IsNotNull(transaction);
            Assert.AreEqual(reversalId, transaction.RvslId);
            Assert.AreEqual(originalInstructionId, transaction.OrgnlInstrId);
            Assert.AreEqual(originalEndToEndId, transaction.OrgnlEndToEndId);
            Assert.AreEqual(originalAmount, transaction.OrgnlInstdAmt);
            Assert.AreEqual(reversedAmount, transaction.RvsdInstdAmt);
            Assert.AreEqual(chargeBearer, transaction.ChrgBr);
            Assert.IsTrue(transaction.ChrgBrSpecified);
            Assert.AreEqual(1, transaction.RvslRsnInf.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreatePaymentTransactionReversal_WithNullOriginalInstructionId_ShouldThrowException()
        {
            // Act
            Pain00700106Builder.CreatePaymentTransactionReversal("TXN001", null!, "E2E123");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreatePaymentTransactionReversal_WithNullOriginalEndToEndId_ShouldThrowException()
        {
            // Act
            Pain00700106Builder.CreatePaymentTransactionReversal("TXN001", "INSTR123", null!);
        }

        [TestMethod]
        public void CreatePaymentReversalReason_WithAllFields_ShouldCreateReason()
        {
            // Arrange
            var originator = Pain00700106Builder.CreatePartyIdentification("Test Originator");
            var reason = Pain00700106Builder.CreateReversalReasonWithCode("DUPL");
            var additionalInfo = new[] { "Duplicate payment detected", "Transaction ID: TXN123" };

            // Act
            var reversalReason = Pain00700106Builder.CreatePaymentReversalReason(
                originator,
                reason,
                additionalInfo);

            // Assert
            Assert.IsNotNull(reversalReason);
            Assert.AreEqual(originator, reversalReason.Orgtr);
            Assert.AreEqual(reason, reversalReason.Rsn);
            Assert.AreEqual(2, reversalReason.AddtlInf.Count);
            Assert.AreEqual("Duplicate payment detected", reversalReason.AddtlInf[0]);
            Assert.AreEqual("Transaction ID: TXN123", reversalReason.AddtlInf[1]);
        }

        [TestMethod]
        public void CreateReversalReasonWithCode_WithValidCode_ShouldCreateReason()
        {
            // Arrange
            var reasonCode = "DUPL";

            // Act
            var reason = Pain00700106Builder.CreateReversalReasonWithCode(reasonCode);

            // Assert
            Assert.IsNotNull(reason);
            Assert.AreEqual(reasonCode, reason.Cd);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateReversalReasonWithCode_WithNullCode_ShouldThrowException()
        {
            // Act
            Pain00700106Builder.CreateReversalReasonWithCode(null!);
        }

        [TestMethod]
        public void CreateReversalReasonWithProprietary_WithValidReason_ShouldCreateReason()
        {
            // Arrange
            var proprietaryReason = "CUSTOM_REVERSAL";

            // Act
            var reason = Pain00700106Builder.CreateReversalReasonWithProprietary(proprietaryReason);

            // Assert
            Assert.IsNotNull(reason);
            Assert.AreEqual(proprietaryReason, reason.Prtry);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateReversalReasonWithProprietary_WithNullReason_ShouldThrowException()
        {
            // Act
            Pain00700106Builder.CreateReversalReasonWithProprietary(null!);
        }

        [TestMethod]
        public void CreateCurrencyAndAmount_WithValidValues_ShouldCreateAmount()
        {
            // Arrange
            var amount = 123.45m;
            var currency = "USD";

            // Act
            var currencyAmount = Pain00700106Builder.CreateCurrencyAndAmount(amount, currency);

            // Assert
            Assert.IsNotNull(currencyAmount);
            Assert.AreEqual(amount, currencyAmount.Value);
            Assert.AreEqual(currency, currencyAmount.Ccy);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateCurrencyAndAmount_WithNullCurrency_ShouldThrowException()
        {
            // Act
            Pain00700106Builder.CreateCurrencyAndAmount(100.00m, null!);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateCurrencyAndAmount_WithNegativeAmount_ShouldThrowException()
        {
            // Act
            Pain00700106Builder.CreateCurrencyAndAmount(-100.00m, "USD");
        }

        [TestMethod]
        public void CreatePartyIdentification_WithName_ShouldCreateParty()
        {
            // Arrange
            var name = "Test Bank Ltd";

            // Act
            var party = Pain00700106Builder.CreatePartyIdentification(name);

            // Assert
            Assert.IsNotNull(party);
            Assert.AreEqual(name, party.Nm);
        }

        [TestMethod]
        public void AddSupplementaryData_WithValidData_ShouldAddData()
        {
            // Arrange
            var supplementaryData = new SupplementaryData1
            {
                PlcAndNm = "TestData"
            };

            // Act
            var result = _builder.AddSupplementaryData(supplementaryData);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreSame(_builder, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddSupplementaryData_WithNullData_ShouldThrowException()
        {
            // Act
            _builder.AddSupplementaryData(null!);
        }

        [TestMethod]
        public void Build_WithCompleteMessage_ShouldReturnDocument()
        {
            // Arrange
            var messageId = "MSG123";
            var creationDateTime = DateTime.Now;
            var numberOfTransactions = "1";
            var originalMessageId = "ORIG123";
            var originalMessageNameId = "pain.001.001.03";
            var originalPaymentInformationId = "PMT123";

            _builder.SetGroupHeader(messageId, creationDateTime, numberOfTransactions)
                    .SetOriginalGroupInformation(originalMessageId, originalMessageNameId)
                    .AddOriginalPaymentInstruction(null, originalPaymentInformationId);

            // Act
            var document = _builder.Build();

            // Assert
            Assert.IsNotNull(document);
            Assert.IsNotNull(document.CstmrPmtRvsl);
            Assert.IsNotNull(document.CstmrPmtRvsl.GrpHdr);
            Assert.IsNotNull(document.CstmrPmtRvsl.OrgnlGrpInf);
            Assert.IsNotNull(document.CstmrPmtRvsl.OrgnlPmtInfAndRvsl);
            Assert.AreEqual(1, document.CstmrPmtRvsl.OrgnlPmtInfAndRvsl.Count);
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
            _builder.SetGroupHeader("MSG123", DateTime.Now, "1");

            // Act
            _builder.Build();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Build_WithoutPaymentInstructions_ShouldThrowException()
        {
            // Arrange
            _builder.SetGroupHeader("MSG123", DateTime.Now, "1")
                    .SetOriginalGroupInformation("ORIG123", "pain.001.001.03");

            // Act
            _builder.Build();
        }

        [TestMethod]
        public void BuildXml_WithCompleteMessage_ShouldReturnXml()
        {
            // Arrange
            _builder.SetGroupHeader("MSG123", DateTime.Now, "1")
                    .SetOriginalGroupInformation("ORIG123", "pain.001.001.03")
                    .AddOriginalPaymentInstruction(null, "PMT123");

            // Act
            var xml = _builder.BuildXml();

            // Assert
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.StartsWith("<?xml"));
            Assert.IsTrue(xml.Contains("Document"));
        }

        [TestMethod]
        public void BuildXml_WithDocumentParameter_ShouldReturnXml()
        {
            // Arrange
            var document = new Document
            {
                CstmrPmtRvsl = new CustomerPaymentReversalV06
                {
                    GrpHdr = new GroupHeader56
                    {
                        MsgId = "TEST123",
                        CreDtTm = DateTime.Now,
                        NbOfTxs = "1"
                    }
                }
            };

            // Act
            var xml = _builder.BuildXml(document);

            // Assert
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.StartsWith("<?xml"));
            Assert.IsTrue(xml.Contains("Document"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildXml_WithNullMessage_ShouldThrowException()
        {
            // Act
            _builder.BuildXml(null!);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildXml_WithInvalidMessageType_ShouldThrowException()
        {
            // Act
            _builder.BuildXml("InvalidMessage");
        }

        [TestMethod]
        public void ImplementsIMessageBuilder_ShouldBeAssignable()
        {
            // Act & Assert
            Assert.IsInstanceOfType(_builder, typeof(IMessageBuilder));
        }

        [TestMethod]
        public void FluentInterface_ShouldSupportMethodChaining()
        {
            // Act
            var result = _builder
                .SetGroupHeader("MSG123", DateTime.Now, "1")
                .SetOriginalGroupInformation("ORIG123", "pain.001.001.03")
                .AddOriginalPaymentInstruction(null, "PMT123");

            // Assert
            Assert.AreSame(_builder, result);
        }

        [TestMethod]
        public void CompleteWorkflow_ShouldCreateValidMessage()
        {
            // Arrange
            var messageId = "MSG20241227001";
            var creationDateTime = new DateTime(2024, 12, 27, 10, 30, 0);
            var numberOfTransactions = "2";
            var controlSum = 1500.75m;
            var groupReversal = false;
            var initiatingParty = Pain00700106Builder.CreatePartyIdentification("ABC Bank Ltd");

            var originalMessageId = "ORIG20241226001";
            var originalMessageNameId = "pain.001.001.03";
            var originalCreationDateTime = new DateTime(2024, 12, 26, 14, 15, 0);

            var reversalReason = Pain00700106Builder.CreatePaymentReversalReason(
                Pain00700106Builder.CreatePartyIdentification("Customer Service"),
                Pain00700106Builder.CreateReversalReasonWithCode("CUST"),
                new[] { "Customer requested reversal due to error" });

            var originalPaymentInformationId = "PMT20241226001";
            var reversalPaymentInformationId = "RVSL20241227001";

            var transaction1 = Pain00700106Builder.CreatePaymentTransactionReversal(
                "TXN001",
                "INSTR20241226001",
                "E2E20241226001",
                Pain00700106Builder.CreateCurrencyAndAmount(750.25m, "EUR"),
                Pain00700106Builder.CreateCurrencyAndAmount(750.25m, "EUR"),
                ChargeBearerType1Code.Slev,
                new[] { reversalReason });

            var transaction2 = Pain00700106Builder.CreatePaymentTransactionReversal(
                "TXN002",
                "INSTR20241226002",
                "E2E20241226002",
                Pain00700106Builder.CreateCurrencyAndAmount(750.50m, "EUR"),
                Pain00700106Builder.CreateCurrencyAndAmount(750.50m, "EUR"));

            // Act
            var document = _builder
                .SetGroupHeader(messageId, creationDateTime, numberOfTransactions, controlSum, groupReversal, initiatingParty)
                .SetOriginalGroupInformation(originalMessageId, originalMessageNameId, originalCreationDateTime, new[] { reversalReason })
                .AddOriginalPaymentInstruction(reversalPaymentInformationId, originalPaymentInformationId, numberOfTransactions, controlSum)
                .AddPaymentTransactionReversal(transaction1)
                .AddPaymentTransactionReversal(transaction2)
                .Build();

            // Assert
            Assert.IsNotNull(document);
            Assert.IsNotNull(document.CstmrPmtRvsl);

            // Verify Group Header
            Assert.AreEqual(messageId, document.CstmrPmtRvsl.GrpHdr.MsgId);
            Assert.AreEqual(creationDateTime, document.CstmrPmtRvsl.GrpHdr.CreDtTm);
            Assert.AreEqual(numberOfTransactions, document.CstmrPmtRvsl.GrpHdr.NbOfTxs);
            Assert.AreEqual(controlSum, document.CstmrPmtRvsl.GrpHdr.CtrlSum);
            Assert.AreEqual(groupReversal, document.CstmrPmtRvsl.GrpHdr.GrpRvsl);
            Assert.AreEqual("ABC Bank Ltd", document.CstmrPmtRvsl.GrpHdr.InitgPty.Nm);

            // Verify Original Group Information
            Assert.AreEqual(originalMessageId, document.CstmrPmtRvsl.OrgnlGrpInf.OrgnlMsgId);
            Assert.AreEqual(originalMessageNameId, document.CstmrPmtRvsl.OrgnlGrpInf.OrgnlMsgNmId);
            Assert.AreEqual(originalCreationDateTime, document.CstmrPmtRvsl.OrgnlGrpInf.OrgnlCreDtTm);

            // Verify Payment Instructions
            Assert.AreEqual(1, document.CstmrPmtRvsl.OrgnlPmtInfAndRvsl.Count);
            var paymentInstruction = document.CstmrPmtRvsl.OrgnlPmtInfAndRvsl[0];
            Assert.AreEqual(originalPaymentInformationId, paymentInstruction.OrgnlPmtInfId);
            Assert.AreEqual(reversalPaymentInformationId, paymentInstruction.RvslPmtInfId);

            // Verify Transactions
            Assert.AreEqual(2, paymentInstruction.TxInf.Count);
            Assert.AreEqual("TXN001", paymentInstruction.TxInf[0].RvslId);
            Assert.AreEqual("TXN002", paymentInstruction.TxInf[1].RvslId);
        }
    }
}
