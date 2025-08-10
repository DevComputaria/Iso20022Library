using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Iso20022Library.Application.Builders.Pain;
using Iso20022Library.Application.Builders;
using Iso20022Library.Domain.Common.Enums;
using Iso20022Library.Messages.Payments.Pain.Generated.Pain00100110;

namespace Iso20022Library.Tests.Builders
{
    /// <summary>
    /// Unit tests for the Pain00100110Builder class.
    /// Tests the functionality of building ISO 20022 pain.001.001.10 messages.
    /// </summary>
    [TestClass]
    public class Pain00100110BuilderTests
    {
        private Pain00100110Builder _builder = null!;

        /// <summary>
        /// Initializes test setup before each test method.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _builder = new Pain00100110Builder();
        }

        /// <summary>
        /// Tests that the builder has the correct message type.
        /// </summary>
        [TestMethod]
        public void MessageType_ShouldBePain00100110()
        {
            // Act
            var messageType = _builder.MessageType;

            // Assert
            Assert.AreEqual(MessageType.Pain00100110, messageType);
        }

        /// <summary>
        /// Tests that the builder can be created through the factory.
        /// </summary>
        [TestMethod]
        public void Factory_ShouldCreatePain00100110Builder()
        {
            // Arrange
            var factory = new MessageBuilderFactory();

            // Act
            var builder = factory.GetBuilder(MessageType.Pain00100110);

            // Assert
            Assert.IsNotNull(builder);
            Assert.IsInstanceOfType(builder, typeof(Pain00100110Builder));
        }

        /// <summary>
        /// Tests that the builder can create a basic message with group header.
        /// </summary>
        [TestMethod]
        public void SetGroupHeader_WithValidParameters_ShouldSucceed()
        {
            // Arrange
            var messageId = "MSG001";
            var creationDateTime = DateTime.Now;
            var numberOfTransactions = "1";
            var initiatingParty = new PartyIdentification135
            {
                Nm = "Test Initiator"
            };

            // Act
            var result = _builder.SetGroupHeader(messageId, creationDateTime, numberOfTransactions, null, initiatingParty);

            // Assert
            Assert.AreSame(_builder, result); // Should return the same instance for method chaining
            var document = _builder.GetDocument() as Document;
            Assert.IsNotNull(document);
            Assert.IsNotNull(document.CstmrCdtTrfInitn);
            Assert.IsNotNull(document.CstmrCdtTrfInitn.GrpHdr);
            Assert.AreEqual(messageId, document.CstmrCdtTrfInitn.GrpHdr.MsgId);
            Assert.AreEqual(creationDateTime, document.CstmrCdtTrfInitn.GrpHdr.CreDtTm);
            Assert.AreEqual(numberOfTransactions, document.CstmrCdtTrfInitn.GrpHdr.NbOfTxs);
            Assert.AreEqual(initiatingParty, document.CstmrCdtTrfInitn.GrpHdr.InitgPty);
        }

        /// <summary>
        /// Tests that SetGroupHeader throws ArgumentNullException for null messageId.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetGroupHeader_WithNullMessageId_ShouldThrowArgumentNullException()
        {
            // Act
            _builder.SetGroupHeader(null!, DateTime.Now, "1", null, null);
        }

        /// <summary>
        /// Tests that SetGroupHeader throws ArgumentNullException for null numberOfTransactions.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetGroupHeader_WithNullNumberOfTransactions_ShouldThrowArgumentNullException()
        {
            // Act
            _builder.SetGroupHeader("MSG001", DateTime.Now, null!, null, null);
        }

        /// <summary>
        /// Tests that SetGroupHeader correctly sets control sum when provided.
        /// </summary>
        [TestMethod]
        public void SetGroupHeader_WithControlSum_ShouldSetControlSum()
        {
            // Arrange
            var messageId = "MSG001";
            var creationDateTime = DateTime.Now;
            var numberOfTransactions = "1";
            var controlSum = 1000.50m;

            // Act
            _builder.SetGroupHeader(messageId, creationDateTime, numberOfTransactions, controlSum, null);

            // Assert
            var document = _builder.GetDocument() as Document;
            Assert.IsNotNull(document?.CstmrCdtTrfInitn?.GrpHdr);
            Assert.AreEqual(controlSum, document.CstmrCdtTrfInitn.GrpHdr.CtrlSum);
            Assert.IsTrue(document.CstmrCdtTrfInitn.GrpHdr.CtrlSumSpecified);
        }

        /// <summary>
        /// Tests that AddPaymentInstruction throws InvalidOperationException when group header is not set.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddPaymentInstruction_WithoutGroupHeader_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var debtor = new PartyIdentification135 { Nm = "Test Debtor" };
            var debtorAccount = new CashAccount38 { Id = new AccountIdentification4Choice() };
            var debtorAgent = new BranchAndFinancialInstitutionIdentification6();

            // Act
            _builder.AddPaymentInstruction(
                "PMT001",
                PaymentMethod3Code.Trf,
                true,
                "1",
                1000.00m,
                null,
                DateTime.Today,
                debtor,
                debtorAccount,
                debtorAgent);
        }

        /// <summary>
        /// Tests that AddPaymentInstruction with valid parameters succeeds.
        /// </summary>
        [TestMethod]
        public void AddPaymentInstruction_WithValidParameters_ShouldSucceed()
        {
            // Arrange
            SetupGroupHeader();
            var paymentInformationId = "PMT001";
            var paymentMethod = PaymentMethod3Code.Trf;
            var batchBooking = true;
            var numberOfTransactions = "1";
            var controlSum = 1000.00m;
            var requestedExecutionDate = DateTime.Today.AddDays(1);
            var debtor = new PartyIdentification135 { Nm = "Test Debtor" };
            var debtorAccount = new CashAccount38 { Id = new AccountIdentification4Choice() };
            var debtorAgent = new BranchAndFinancialInstitutionIdentification6();

            // Act
            var result = _builder.AddPaymentInstruction(
                paymentInformationId,
                paymentMethod,
                batchBooking,
                numberOfTransactions,
                controlSum,
                null,
                requestedExecutionDate,
                debtor,
                debtorAccount,
                debtorAgent);

            // Assert
            Assert.AreSame(_builder, result);
            var document = _builder.GetDocument() as Document;
            Assert.IsNotNull(document?.CstmrCdtTrfInitn?.PmtInf);
            Assert.AreEqual(1, document.CstmrCdtTrfInitn.PmtInf.Count);
            
            var paymentInstruction = document.CstmrCdtTrfInitn.PmtInf[0];
            Assert.AreEqual(paymentInformationId, paymentInstruction.PmtInfId);
            Assert.AreEqual(paymentMethod, paymentInstruction.PmtMtd);
            Assert.AreEqual(batchBooking, paymentInstruction.BtchBookg);
            Assert.IsTrue(paymentInstruction.BtchBookgSpecified);
            Assert.AreEqual(numberOfTransactions, paymentInstruction.NbOfTxs);
            Assert.AreEqual(controlSum, paymentInstruction.CtrlSum);
            Assert.IsTrue(paymentInstruction.CtrlSumSpecified);
            Assert.AreEqual(debtor, paymentInstruction.Dbtr);
            Assert.AreEqual(debtorAccount, paymentInstruction.DbtrAcct);
            Assert.AreEqual(debtorAgent, paymentInstruction.DbtrAgt);
        }

        /// <summary>
        /// Tests that AddCreditTransferTransaction throws InvalidOperationException when no payment instruction exists.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddCreditTransferTransaction_WithoutPaymentInstruction_ShouldThrowInvalidOperationException()
        {
            // Arrange
            SetupGroupHeader();
            var transaction = new CreditTransferTransaction40();

            // Act
            _builder.AddCreditTransferTransaction(transaction);
        }

        /// <summary>
        /// Tests that AddCreditTransferTransaction with valid parameters succeeds.
        /// </summary>
        [TestMethod]
        public void AddCreditTransferTransaction_WithValidParameters_ShouldSucceed()
        {
            // Arrange
            SetupGroupHeaderAndPaymentInstruction();
            var endToEndId = "TXN001";
            var amount = new AmountType4Choice { InstdAmt = new ActiveOrHistoricCurrencyAndAmount { Ccy = "EUR", Value = 1000.00m } };
            var creditor = new PartyIdentification135 { Nm = "Test Creditor" };
            var creditorAccount = new CashAccount38 { Id = new AccountIdentification4Choice() };

            // Act
            var result = _builder.AddCreditTransferTransaction(endToEndId, amount, creditor, creditorAccount);

            // Assert
            Assert.AreSame(_builder, result);
            var document = _builder.GetDocument() as Document;
            var paymentInstruction = document?.CstmrCdtTrfInitn?.PmtInf?[0];
            Assert.IsNotNull(paymentInstruction?.CdtTrfTxInf);
            Assert.AreEqual(1, paymentInstruction.CdtTrfTxInf.Count);
            
            var transaction = paymentInstruction.CdtTrfTxInf[0];
            Assert.AreEqual(endToEndId, transaction.PmtId.EndToEndId);
            Assert.AreEqual(amount, transaction.Amt);
            Assert.AreEqual(creditor, transaction.Cdtr);
            Assert.AreEqual(creditorAccount, transaction.CdtrAcct);
        }

        /// <summary>
        /// Tests that AddCreditTransferTransaction with instruction ID sets the instruction ID correctly.
        /// </summary>
        [TestMethod]
        public void AddCreditTransferTransaction_WithInstructionId_ShouldSetInstructionId()
        {
            // Arrange
            SetupGroupHeaderAndPaymentInstruction();
            var endToEndId = "TXN001";
            var instructionId = "INSTR001";
            var amount = new AmountType4Choice { InstdAmt = new ActiveOrHistoricCurrencyAndAmount { Ccy = "EUR", Value = 1000.00m } };
            var creditor = new PartyIdentification135 { Nm = "Test Creditor" };
            var creditorAccount = new CashAccount38 { Id = new AccountIdentification4Choice() };

            // Act
            _builder.AddCreditTransferTransaction(endToEndId, amount, creditor, creditorAccount, instructionId);

            // Assert
            var document = _builder.GetDocument() as Document;
            var transaction = document?.CstmrCdtTrfInitn?.PmtInf?[0]?.CdtTrfTxInf?[0];
            Assert.IsNotNull(transaction?.PmtId);
            Assert.AreEqual(instructionId, transaction.PmtId.InstrId);
        }

        /// <summary>
        /// Tests that SetCreditorAgent sets the creditor agent correctly.
        /// </summary>
        [TestMethod]
        public void SetCreditorAgent_WithValidAgent_ShouldSetCreditorAgent()
        {
            // Arrange
            SetupGroupHeaderPaymentInstructionAndTransaction();
            var creditorAgent = new BranchAndFinancialInstitutionIdentification6
            {
                FinInstnId = new FinancialInstitutionIdentification18 { Bicfi = "TESTBIC123" }
            };

            // Act
            var result = _builder.SetCreditorAgent(creditorAgent);

            // Assert
            Assert.AreSame(_builder, result);
            var document = _builder.GetDocument() as Document;
            var transaction = document?.CstmrCdtTrfInitn?.PmtInf?[0]?.CdtTrfTxInf?[0];
            Assert.AreEqual(creditorAgent, transaction?.CdtrAgt);
        }

        /// <summary>
        /// Tests that SetInstructionForDebtorAgent sets the instruction correctly (V10 specific feature).
        /// </summary>
        [TestMethod]
        public void SetInstructionForDebtorAgent_WithValidInstruction_ShouldSetInstruction()
        {
            // Arrange
            SetupGroupHeaderPaymentInstructionAndTransaction();
            var instruction = new InstructionForDebtorAgent1
            {
                Cd = "TEST"
            };

            // Act
            var result = _builder.SetInstructionForDebtorAgent(instruction);

            // Assert
            Assert.AreSame(_builder, result);
            var document = _builder.GetDocument() as Document;
            var transaction = document?.CstmrCdtTrfInitn?.PmtInf?[0]?.CdtTrfTxInf?[0];
            Assert.AreEqual(instruction, transaction?.InstrForDbtrAgt);
        }

        /// <summary>
        /// Tests that AddInstructionForCreditorAgent adds the instruction correctly.
        /// </summary>
        [TestMethod]
        public void AddInstructionForCreditorAgent_WithValidInstruction_ShouldAddInstruction()
        {
            // Arrange
            SetupGroupHeaderPaymentInstructionAndTransaction();
            var instruction = new InstructionForCreditorAgent3
            {
                Cd = "TEST"
            };

            // Act
            var result = _builder.AddInstructionForCreditorAgent(instruction);

            // Assert
            Assert.AreSame(_builder, result);
            var document = _builder.GetDocument() as Document;
            var transaction = document?.CstmrCdtTrfInitn?.PmtInf?[0]?.CdtTrfTxInf?[0];
            Assert.IsNotNull(transaction?.InstrForCdtrAgt);
            Assert.AreEqual(1, transaction.InstrForCdtrAgt.Count);
            Assert.AreEqual(instruction, transaction.InstrForCdtrAgt[0]);
        }

        /// <summary>
        /// Tests that Build throws InvalidOperationException when group header is not set.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Build_WithoutGroupHeader_ShouldThrowInvalidOperationException()
        {
            // Act
            _builder.Build();
        }

        /// <summary>
        /// Tests that Build throws InvalidOperationException when no payment instruction is added.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Build_WithoutPaymentInstruction_ShouldThrowInvalidOperationException()
        {
            // Arrange
            SetupGroupHeader();

            // Act
            _builder.Build();
        }

        /// <summary>
        /// Tests that Build throws InvalidOperationException when payment instruction has no transactions.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Build_WithoutCreditTransferTransaction_ShouldThrowInvalidOperationException()
        {
            // Arrange
            SetupGroupHeaderAndPaymentInstruction();

            // Act
            _builder.Build();
        }

        /// <summary>
        /// Tests that Build succeeds with complete document.
        /// </summary>
        [TestMethod]
        public void Build_WithCompleteDocument_ShouldSucceed()
        {
            // Arrange
            SetupGroupHeaderPaymentInstructionAndTransaction();

            // Act
            var result = _builder.Build();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Document));
        }

        /// <summary>
        /// Tests that BuildXml produces valid XML output.
        /// </summary>
        [TestMethod]
        public void BuildXml_WithCompleteDocument_ShouldProduceValidXml()
        {
            // Arrange
            SetupGroupHeaderPaymentInstructionAndTransaction();

            // Act
            var xml = _builder.BuildXml();

            // Assert
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.Contains("<?xml"));
            Assert.IsTrue(xml.Contains("urn:iso:std:iso:20022:tech:xsd:pain.001.001.10"));
            Assert.IsTrue(xml.Contains("CstmrCdtTrfInitn"));
        }

        /// <summary>
        /// Helper method to set up a basic group header.
        /// </summary>
        private void SetupGroupHeader()
        {
            _builder.SetGroupHeader("MSG001", DateTime.Now, "1", null, null);
        }

        /// <summary>
        /// Helper method to set up group header and payment instruction.
        /// </summary>
        private void SetupGroupHeaderAndPaymentInstruction()
        {
            SetupGroupHeader();
            
            var debtor = new PartyIdentification135 { Nm = "Test Debtor" };
            var debtorAccount = new CashAccount38 { Id = new AccountIdentification4Choice() };
            var debtorAgent = new BranchAndFinancialInstitutionIdentification6();

            _builder.AddPaymentInstruction(
                "PMT001",
                PaymentMethod3Code.Trf,
                null,
                "1",
                null,
                null,
                DateTime.Today,
                debtor,
                debtorAccount,
                debtorAgent);
        }

        /// <summary>
        /// Helper method to set up group header, payment instruction, and credit transfer transaction.
        /// </summary>
        private void SetupGroupHeaderPaymentInstructionAndTransaction()
        {
            SetupGroupHeaderAndPaymentInstruction();
            
            var amount = new AmountType4Choice { InstdAmt = new ActiveOrHistoricCurrencyAndAmount { Ccy = "EUR", Value = 1000.00m } };
            var creditor = new PartyIdentification135 { Nm = "Test Creditor" };
            var creditorAccount = new CashAccount38 { Id = new AccountIdentification4Choice() };

            _builder.AddCreditTransferTransaction("TXN001", amount, creditor, creditorAccount);
        }
    }
}
