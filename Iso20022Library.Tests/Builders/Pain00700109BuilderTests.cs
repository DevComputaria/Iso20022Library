using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Iso20022Library.Application.Builders.Pain;
using Iso20022Library.Domain.Common.Enums;
using Iso20022Library.Messages.Payments.Pain.Generated.Pain00700109;

namespace Iso20022Library.Tests.Builders
{
    [TestClass]
    public class Pain00700109BuilderTests
    {
        private Pain00700109Builder _builder = null!;

        [TestInitialize]
        public void Setup()
        {
            _builder = new Pain00700109Builder();
        }

        [TestMethod]
        public void Constructor_ShouldInitializeWithCorrectMessageType()
        {
            // Act & Assert
            Assert.AreEqual(MessageType.Pain00700109, _builder.MessageType);
        }

        [TestMethod]
        public void SetGroupHeader_WithValidParameters_ShouldSetGroupHeader()
        {
            // Arrange
            var messageId = "MSG001";
            var creationDateTime = DateTime.Now;
            var numberOfTransactions = "1";
            var controlSum = 1000.50m;
            var initiatingParty = Pain00700109Builder.CreateOrganizationParty("Test Bank", "TESTBANK");

            // Act
            var result = _builder.SetGroupHeader(messageId, creationDateTime, numberOfTransactions, controlSum, initiatingParty)
                                  .SetOriginalGroupInformation("ORIG001")
                                  .AddOriginalPaymentInstruction("PMT001");

            // Assert
            Assert.AreSame(_builder, result);
            var message = _builder.Build();
            Assert.IsNotNull(message.GrpHdr);
            Assert.AreEqual(messageId, message.GrpHdr.MsgId);
            Assert.AreEqual(creationDateTime, message.GrpHdr.CreDtTm);
            Assert.AreEqual(numberOfTransactions, message.GrpHdr.NbOfTxs);
            Assert.AreEqual(controlSum, message.GrpHdr.CtrlSum);
            Assert.IsTrue(message.GrpHdr.CtrlSumSpecified);
            Assert.AreEqual(initiatingParty.Nm, message.GrpHdr.InitgPty.Nm);
        }

        [TestMethod]
        public void SetGroupHeader_WithoutOptionalParameters_ShouldSetBasicGroupHeader()
        {
            // Arrange
            var messageId = "MSG001";
            var creationDateTime = DateTime.Now;
            var numberOfTransactions = "1";

            // Act
            var result = _builder.SetGroupHeader(messageId, creationDateTime, numberOfTransactions)
                                  .SetOriginalGroupInformation("ORIG001")
                                  .AddOriginalPaymentInstruction("PMT001");

            // Assert
            Assert.AreSame(_builder, result);
            var message = _builder.Build();
            Assert.IsNotNull(message.GrpHdr);
            Assert.AreEqual(messageId, message.GrpHdr.MsgId);
            Assert.AreEqual(creationDateTime, message.GrpHdr.CreDtTm);
            Assert.AreEqual(numberOfTransactions, message.GrpHdr.NbOfTxs);
            Assert.IsFalse(message.GrpHdr.CtrlSumSpecified);
            Assert.IsNull(message.GrpHdr.InitgPty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetGroupHeader_WithNullMessageId_ShouldThrowArgumentNullException()
        {
            // Act
            _builder.SetGroupHeader(null!, DateTime.Now, "1");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetGroupHeader_WithEmptyMessageId_ShouldThrowArgumentNullException()
        {
            // Act
            _builder.SetGroupHeader("", DateTime.Now, "1");
        }

        [TestMethod]
        public void SetOriginalGroupInformation_WithValidParameters_ShouldSetOriginalGroupInformation()
        {
            // Arrange
            _builder.SetGroupHeader("MSG001", DateTime.Now, "1");
            var originalMessageId = "ORIG001";
            var originalMessageNameId = "pain.001.001.03";
            var originalCreationDateTime = DateTime.Now.AddDays(-1);
            var reversalReasons = new List<PaymentReversalReason9>
            {
                Pain00700109Builder.CreateReversalReason("AC04", new[] { "Account closed" })
            };

            // Act
            var result = _builder.SetOriginalGroupInformation(originalMessageId, originalMessageNameId, originalCreationDateTime, reversalReasons)
                                  .AddOriginalPaymentInstruction("PMT001");

            // Assert
            Assert.AreSame(_builder, result);
            var message = _builder.Build();
            Assert.IsNotNull(message.OrgnlGrpInf);
            Assert.AreEqual(originalMessageId, message.OrgnlGrpInf.OrgnlMsgId);
            Assert.AreEqual(originalMessageNameId, message.OrgnlGrpInf.OrgnlMsgNmId);
            Assert.AreEqual(originalCreationDateTime, message.OrgnlGrpInf.OrgnlCreDtTm);
            Assert.IsTrue(message.OrgnlGrpInf.OrgnlCreDtTmSpecified);
            Assert.AreEqual(1, message.OrgnlGrpInf.RvslRsnInf.Count);
        }

        [TestMethod]
        public void SetOriginalGroupInformation_WithMinimalParameters_ShouldSetBasicOriginalGroupInformation()
        {
            // Arrange
            _builder.SetGroupHeader("MSG001", DateTime.Now, "1");
            var originalMessageId = "ORIG001";

            // Act
            var result = _builder.SetOriginalGroupInformation(originalMessageId)
                                  .AddOriginalPaymentInstruction("PMT001");

            // Assert
            Assert.AreSame(_builder, result);
            var message = _builder.Build();
            Assert.IsNotNull(message.OrgnlGrpInf);
            Assert.AreEqual(originalMessageId, message.OrgnlGrpInf.OrgnlMsgId);
            Assert.IsNull(message.OrgnlGrpInf.OrgnlMsgNmId);
            Assert.IsFalse(message.OrgnlGrpInf.OrgnlCreDtTmSpecified);
            Assert.AreEqual(0, message.OrgnlGrpInf.RvslRsnInf.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetOriginalGroupInformation_WithNullOriginalMessageId_ShouldThrowArgumentNullException()
        {
            // Act
            _builder.SetOriginalGroupInformation(null!);
        }

        [TestMethod]
        public void AddOriginalPaymentInstruction_WithValidParameters_ShouldAddPaymentInstruction()
        {
            // Arrange
            _builder.SetGroupHeader("MSG001", DateTime.Now, "1")
                    .SetOriginalGroupInformation("ORIG001");
            var originalPaymentInformationId = "PMT001";
            var reversalReasons = new List<PaymentReversalReason9>
            {
                Pain00700109Builder.CreateReversalReason("AG02", new[] { "Invalid bank code" })
            };

            // Act
            var result = _builder.AddOriginalPaymentInstruction(originalPaymentInformationId, reversalReasons);

            // Assert
            Assert.AreSame(_builder, result);
            var message = _builder.Build();
            Assert.AreEqual(1, message.OrgnlPmtInfAndRvsl.Count);
            Assert.AreEqual(originalPaymentInformationId, message.OrgnlPmtInfAndRvsl[0].OrgnlPmtInfId);
            Assert.AreEqual(1, message.OrgnlPmtInfAndRvsl[0].RvslRsnInf.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddOriginalPaymentInstruction_WithNullPaymentInformationId_ShouldThrowArgumentNullException()
        {
            // Act
            _builder.AddOriginalPaymentInstruction(null!);
        }

        [TestMethod]
        public void AddPaymentTransactionReversal_WithValidParameters_ShouldAddTransactionReversal()
        {
            // Arrange
            _builder.SetGroupHeader("MSG001", DateTime.Now, "1")
                    .SetOriginalGroupInformation("ORIG001")
                    .AddOriginalPaymentInstruction("PMT001");
            var reversalId = "REV001";
            var originalInstructionId = "INSTR001";
            var originalEndToEndId = "E2E001";
            var originalUetr = "00000000-0000-4000-8000-000000000001";
            var reversedAmount = Pain00700109Builder.CreateAmount("EUR", 100.50m);
            var reversalReasons = new List<PaymentReversalReason9>
            {
                Pain00700109Builder.CreateReversalReason("MD01", new[] { "Mandate cancelled" })
            };
            var originalTxRef = Pain00700109Builder.CreateOriginalTransactionReference(100.50m, "EUR", DateTime.Now, PaymentMethod4Code.Trf);

            // Act
            var result = _builder.AddPaymentTransactionReversal(reversalId, originalInstructionId, originalEndToEndId, originalUetr, reversedAmount, reversalReasons, originalTxRef);

            // Assert
            Assert.AreSame(_builder, result);
            var message = _builder.Build();
            var transaction = message.OrgnlPmtInfAndRvsl[0].TxInf[0];
            Assert.AreEqual(reversalId, transaction.RvslId);
            Assert.AreEqual(originalInstructionId, transaction.OrgnlInstrId);
            Assert.AreEqual(originalEndToEndId, transaction.OrgnlEndToEndId);
            Assert.AreEqual(originalUetr, transaction.OrgnlUetr);
            Assert.AreEqual(reversedAmount.Value, transaction.RvsdInstdAmt.Value);
            Assert.AreEqual(reversedAmount.Ccy, transaction.RvsdInstdAmt.Ccy);
            Assert.AreEqual(1, transaction.RvslRsnInf.Count);
            Assert.IsNotNull(transaction.OrgnlTxRef);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddPaymentTransactionReversal_WithoutPaymentInstruction_ShouldThrowInvalidOperationException()
        {
            // Act
            _builder.AddPaymentTransactionReversal("REV001");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddPaymentTransactionReversal_WithNullReversalId_ShouldThrowArgumentNullException()
        {
            // Arrange
            _builder.SetGroupHeader("MSG001", DateTime.Now, "1")
                    .SetOriginalGroupInformation("ORIG001")
                    .AddOriginalPaymentInstruction("PMT001");

            // Act
            _builder.AddPaymentTransactionReversal(null!);
        }

        [TestMethod]
        public void Build_WithValidMessage_ShouldReturnValidMessage()
        {
            // Arrange
            _builder.SetGroupHeader("MSG001", DateTime.Now, "1")
                    .SetOriginalGroupInformation("ORIG001")
                    .AddOriginalPaymentInstruction("PMT001")
                    .AddPaymentTransactionReversal("REV001");

            // Act
            var result = _builder.Build();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(CustomerPaymentReversalV09));
            Assert.IsNotNull(result.GrpHdr);
            Assert.IsNotNull(result.OrgnlGrpInf);
            Assert.AreEqual(1, result.OrgnlPmtInfAndRvsl.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Build_WithoutGroupHeader_ShouldThrowInvalidOperationException()
        {
            // Act
            _builder.Build();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Build_WithoutOriginalGroupInformation_ShouldThrowInvalidOperationException()
        {
            // Arrange
            _builder.SetGroupHeader("MSG001", DateTime.Now, "1");

            // Act
            _builder.Build();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Build_WithoutPaymentInstructions_ShouldThrowInvalidOperationException()
        {
            // Arrange
            _builder.SetGroupHeader("MSG001", DateTime.Now, "1")
                    .SetOriginalGroupInformation("ORIG001");

            // Act
            _builder.Build();
        }

        [TestMethod]
        public void BuildXml_WithValidMessage_ShouldReturnXmlString()
        {
            // Arrange
            _builder.SetGroupHeader("MSG001", DateTime.Now, "1")
                    .SetOriginalGroupInformation("ORIG001")
                    .AddOriginalPaymentInstruction("PMT001")
                    .AddPaymentTransactionReversal("REV001");

            // Act
            var result = _builder.BuildXml();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains("<?xml"));
            Assert.IsTrue(result.Contains("pain.007.001.09"));
        }

        [TestMethod]
        public void BuildXml_WithConfig_ShouldReturnXmlString()
        {
            // Arrange
            _builder.SetGroupHeader("MSG001", DateTime.Now, "1")
                    .SetOriginalGroupInformation("ORIG001")
                    .AddOriginalPaymentInstruction("PMT001")
                    .AddPaymentTransactionReversal("REV001");

            // Act
            var result = _builder.BuildXml(new object());

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains("<?xml"));
            Assert.IsTrue(result.Contains("pain.007.001.09"));
        }

        [TestMethod]
        public void CreateReversalReason_WithValidParameters_ShouldCreateValidReason()
        {
            // Arrange
            var reasonCode = "AC04";
            var additionalInfo = new[] { "Account closed", "Contact bank" };
            var originator = Pain00700109Builder.CreateOrganizationParty("Test Bank");

            // Act
            var result = Pain00700109Builder.CreateReversalReason(reasonCode, additionalInfo, originator);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(reasonCode, result.Rsn.Cd);
            Assert.AreEqual(2, result.AddtlInf.Count);
            Assert.AreEqual("Account closed", result.AddtlInf[0]);
            Assert.AreEqual("Contact bank", result.AddtlInf[1]);
            Assert.AreEqual(originator.Nm, result.Orgtr.Nm);
        }

        [TestMethod]
        public void CreateProprietaryReversalReason_WithValidParameters_ShouldCreateValidReason()
        {
            // Arrange
            var proprietaryReason = "CUSTOM_REASON";
            var additionalInfo = new[] { "Custom reversal reason" };

            // Act
            var result = Pain00700109Builder.CreateProprietaryReversalReason(proprietaryReason, additionalInfo);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(proprietaryReason, result.Rsn.Prtry);
            Assert.AreEqual(1, result.AddtlInf.Count);
            Assert.AreEqual("Custom reversal reason", result.AddtlInf[0]);
        }

        [TestMethod]
        public void CreateAmount_WithValidParameters_ShouldCreateValidAmount()
        {
            // Arrange
            var currency = "EUR";
            var amount = 1234.56m;

            // Act
            var result = Pain00700109Builder.CreateAmount(currency, amount);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(currency, result.Ccy);
            Assert.AreEqual(amount, result.Value);
        }

        [TestMethod]
        public void CreateOrganizationParty_WithBicAndLei_ShouldCreateValidParty()
        {
            // Arrange
            var organizationName = "Test Bank Corp";
            var bic = "TESTDEFF";
            var lei = "ABC123DEF456GHI789JKL012";

            // Act
            var result = Pain00700109Builder.CreateOrganizationParty(organizationName, bic, lei);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(organizationName, result.Nm);
            Assert.IsNotNull(result.Id);
            Assert.IsNotNull(result.Id.OrgId);
            Assert.AreEqual(bic, result.Id.OrgId.AnyBic);
            Assert.AreEqual(lei, result.Id.OrgId.Lei);
        }

        [TestMethod]
        public void CreateOrganizationParty_WithNameOnly_ShouldCreateBasicParty()
        {
            // Arrange
            var organizationName = "Basic Corp";

            // Act
            var result = Pain00700109Builder.CreateOrganizationParty(organizationName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(organizationName, result.Nm);
            Assert.IsNull(result.Id);
        }

        [TestMethod]
        public void CreatePrivateParty_WithDateOfBirth_ShouldCreateValidParty()
        {
            // Arrange
            var firstName = "John";
            var lastName = "Doe";
            var dateOfBirth = new DateTime(1980, 5, 15);

            // Act
            var result = Pain00700109Builder.CreatePrivateParty(firstName, lastName, dateOfBirth);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("John Doe", result.Nm);
            Assert.IsNotNull(result.Id);
            Assert.IsNotNull(result.Id.PrvtId);
            Assert.IsNotNull(result.Id.PrvtId.DtAndPlcOfBirth);
            Assert.AreEqual(dateOfBirth, result.Id.PrvtId.DtAndPlcOfBirth.BirthDt);
        }

        [TestMethod]
        public void CreatePrivateParty_WithoutDateOfBirth_ShouldCreateBasicParty()
        {
            // Arrange
            var firstName = "Jane";
            var lastName = "Smith";

            // Act
            var result = Pain00700109Builder.CreatePrivateParty(firstName, lastName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Jane Smith", result.Nm);
            Assert.IsNotNull(result.Id);
            Assert.IsNotNull(result.Id.PrvtId);
            Assert.IsNull(result.Id.PrvtId.DtAndPlcOfBirth);
        }

        [TestMethod]
        public void CreateOriginalTransactionReference_WithAllParameters_ShouldCreateValidReference()
        {
            // Arrange
            var amount = 500.75m;
            var currency = "USD";
            var executionDate = DateTime.Now.AddDays(1);
            var paymentMethod = PaymentMethod4Code.Chk;

            // Act
            var result = Pain00700109Builder.CreateOriginalTransactionReference(amount, currency, executionDate, paymentMethod);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.IntrBkSttlmAmt);
            Assert.AreEqual(amount, result.IntrBkSttlmAmt.Value);
            Assert.AreEqual(currency, result.IntrBkSttlmAmt.Ccy);
            Assert.IsNotNull(result.ReqdExctnDt);
            Assert.AreEqual(executionDate, result.ReqdExctnDt.Dt);
            Assert.IsTrue(result.ReqdExctnDt.DtSpecified);
            Assert.AreEqual(paymentMethod, result.PmtMtd);
            Assert.IsTrue(result.PmtMtdSpecified);
        }

        [TestMethod]
        public void CreateOriginalTransactionReference_WithNoParameters_ShouldCreateEmptyReference()
        {
            // Act
            var result = Pain00700109Builder.CreateOriginalTransactionReference();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNull(result.IntrBkSttlmAmt);
            Assert.IsNull(result.ReqdExctnDt);
            Assert.IsFalse(result.PmtMtdSpecified);
        }

        [TestMethod]
        public void FluentInterface_CompleteWorkflow_ShouldBuildSuccessfully()
        {
            // Arrange & Act
            var result = _builder
                .SetGroupHeader("MSG001", DateTime.Now, "2", 1500.00m, Pain00700109Builder.CreateOrganizationParty("Initiating Bank"))
                .SetOriginalGroupInformation("ORIG001", "pain.001.001.03", DateTime.Now.AddHours(-2))
                .AddOriginalPaymentInstruction("PMT001")
                .AddPaymentTransactionReversal("REV001", "INSTR001", "E2E001", "00000000-0000-4000-8000-000000000001",
                    Pain00700109Builder.CreateAmount("EUR", 750.00m),
                    new[] { Pain00700109Builder.CreateReversalReason("AC04") })
                .AddPaymentTransactionReversal("REV002", "INSTR002", "E2E002", "00000000-0000-4000-8000-000000000002",
                    Pain00700109Builder.CreateAmount("EUR", 750.00m),
                    new[] { Pain00700109Builder.CreateReversalReason("AG02") })
                .Build();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.GrpHdr);
            Assert.IsNotNull(result.OrgnlGrpInf);
            Assert.AreEqual(1, result.OrgnlPmtInfAndRvsl.Count);
            Assert.AreEqual(2, result.OrgnlPmtInfAndRvsl[0].TxInf.Count);
        }

        [TestMethod]
        public void MultiplePaymentInstructions_ShouldBeHandledCorrectly()
        {
            // Arrange & Act
            var result = _builder
                .SetGroupHeader("MSG001", DateTime.Now, "3")
                .SetOriginalGroupInformation("ORIG001")
                .AddOriginalPaymentInstruction("PMT001")
                .AddPaymentTransactionReversal("REV001")
                .AddOriginalPaymentInstruction("PMT002")
                .AddPaymentTransactionReversal("REV002")
                .AddPaymentTransactionReversal("REV003")
                .Build();

            // Assert
            Assert.AreEqual(2, result.OrgnlPmtInfAndRvsl.Count);
            Assert.AreEqual(1, result.OrgnlPmtInfAndRvsl[0].TxInf.Count);
            Assert.AreEqual(2, result.OrgnlPmtInfAndRvsl[1].TxInf.Count);
        }
    }
}
