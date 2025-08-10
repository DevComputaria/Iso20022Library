using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Iso20022Library.Application.Builders.Pain;
using Iso20022Library.Messages.Payments.Pain.Generated.Pain00100107;

namespace Iso20022Library.Tests.Builders
{
    /// <summary>
    /// Unit tests for the Pain00100107Builder class.
    /// </summary>
    [TestClass]
    public class Pain00100107BuilderTests
    {
        private Pain00100107Builder _builder = null!;

        [TestInitialize]
        public void Setup()
        {
            _builder = new Pain00100107Builder();
        }

        [TestMethod]
        public void Constructor_ShouldInitializeDocumentStructure()
        {
            // Arrange & Act
            var builder = new Pain00100107Builder();
            builder.SetGroupHeader("MSG123456", DateTime.Now, "1", null, CreateSampleParty());
            builder.AddPaymentInstruction(
                "PMT123", PaymentMethod3Code.Trf, null, "1", null, null,
                DateTime.Today.AddDays(1), CreateSampleParty(), CreateSampleCashAccount(), CreateSampleFinancialInstitution());
            builder.AddCreditTransferTransaction(CreateSampleCreditTransferTransaction());

            // Assert
            var document = builder.Build();
            Assert.IsNotNull(document);
            Assert.IsNotNull(document.CstmrCdtTrfInitn);
            Assert.IsNotNull(document.CstmrCdtTrfInitn.PmtInf);
            Assert.IsNotNull(document.CstmrCdtTrfInitn.SplmtryData);
        }

        [TestMethod]
        public void SetGroupHeader_ShouldSetGroupHeaderCorrectly()
        {
            // Arrange
            var messageId = "MSG123456";
            var creationDateTime = DateTime.Now;
            var numberOfTransactions = "5";
            var initiatingParty = CreateSampleParty();
            var controlSum = 1000.00m;

            // Act
            _builder.SetGroupHeader(messageId, creationDateTime, numberOfTransactions, controlSum, initiatingParty);
            _builder.AddPaymentInstruction(
                "PMT123", PaymentMethod3Code.Trf, null, "1", null, null,
                DateTime.Today.AddDays(1), CreateSampleParty(), CreateSampleCashAccount(), CreateSampleFinancialInstitution());
            _builder.AddCreditTransferTransaction(CreateSampleCreditTransferTransaction());
            var document = _builder.Build();

            // Assert
            var grpHdr = document.CstmrCdtTrfInitn.GrpHdr;
            Assert.AreEqual(messageId, grpHdr.MsgId);
            Assert.AreEqual(creationDateTime, grpHdr.CreDtTm);
            Assert.AreEqual(numberOfTransactions, grpHdr.NbOfTxs);
            Assert.AreEqual(initiatingParty, grpHdr.InitgPty);
            Assert.AreEqual(controlSum, grpHdr.CtrlSum);
            Assert.IsTrue(grpHdr.CtrlSumSpecified);
        }

        [TestMethod]
        public void SetGroupHeader_WithoutControlSum_ShouldNotSetControlSumSpecified()
        {
            // Arrange
            var messageId = "MSG123456";
            var creationDateTime = DateTime.Now;
            var numberOfTransactions = "1";
            var initiatingParty = CreateSampleParty();

            // Act
            _builder.SetGroupHeader(messageId, creationDateTime, numberOfTransactions, null, initiatingParty);
            _builder.AddPaymentInstruction(
                "PMT123", PaymentMethod3Code.Trf, null, "1", null, null,
                DateTime.Today.AddDays(1), CreateSampleParty(), CreateSampleCashAccount(), CreateSampleFinancialInstitution());
            _builder.AddCreditTransferTransaction(CreateSampleCreditTransferTransaction());
            var document = _builder.Build();

            // Assert
            var grpHdr = document.CstmrCdtTrfInitn.GrpHdr;
            Assert.IsFalse(grpHdr.CtrlSumSpecified);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetGroupHeader_NullMessageId_ShouldThrowArgumentNullException()
        {
            // Act
            _builder.SetGroupHeader(null!, DateTime.Now, "1", null, CreateSampleParty());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetGroupHeader_EmptyMessageId_ShouldThrowArgumentException()
        {
            // Act
            _builder.SetGroupHeader("", DateTime.Now, "1", null, CreateSampleParty());
        }

        [TestMethod]
        public void AddPaymentInstruction_ShouldAddPaymentInstructionCorrectly()
        {
            // Arrange
            var paymentInfoId = "PMT123";
            var paymentMethod = PaymentMethod3Code.Trf;
            var batchBooking = true;
            var numberOfTransactions = "2";
            var controlSum = 500.00m;
            var requestedExecutionDate = DateTime.Today.AddDays(1);
            var debtor = CreateSampleParty();
            var debtorAccount = CreateSampleCashAccount();
            var debtorAgent = CreateSampleFinancialInstitution();

            // Act
            _builder.SetGroupHeader("MSG123", DateTime.Now, "1", null, CreateSampleParty());
            _builder.AddPaymentInstruction(paymentInfoId, paymentMethod, batchBooking, numberOfTransactions,
                controlSum, null, requestedExecutionDate, debtor, debtorAccount, debtorAgent);
            _builder.AddCreditTransferTransaction(CreateSampleCreditTransferTransaction());
            var document = _builder.Build();

            // Assert
            Assert.AreEqual(1, document.CstmrCdtTrfInitn.PmtInf.Count);
            var pmtInf = document.CstmrCdtTrfInitn.PmtInf[0];
            Assert.AreEqual(paymentInfoId, pmtInf.PmtInfId);
            Assert.AreEqual(paymentMethod, pmtInf.PmtMtd);
            Assert.AreEqual(batchBooking, pmtInf.BtchBookg);
            Assert.IsTrue(pmtInf.BtchBookgSpecified);
            Assert.AreEqual(numberOfTransactions, pmtInf.NbOfTxs);
            Assert.AreEqual(controlSum, pmtInf.CtrlSum);
            Assert.IsTrue(pmtInf.CtrlSumSpecified);
            Assert.AreEqual(requestedExecutionDate, pmtInf.ReqdExctnDt);
            Assert.AreEqual(debtor, pmtInf.Dbtr);
            Assert.AreEqual(debtorAccount, pmtInf.DbtrAcct);
            Assert.AreEqual(debtorAgent, pmtInf.DbtrAgt);
        }

        [TestMethod]
        public void AddPaymentInstruction_WithoutOptionalFields_ShouldNotSetSpecifiedFlags()
        {
            // Arrange & Act
            _builder.SetGroupHeader("MSG123", DateTime.Now, "1", null, CreateSampleParty());
            _builder.AddPaymentInstruction("PMT123", PaymentMethod3Code.Trf, null, "1", null, null,
                DateTime.Today.AddDays(1), CreateSampleParty(), CreateSampleCashAccount(), CreateSampleFinancialInstitution());
            _builder.AddCreditTransferTransaction(CreateSampleCreditTransferTransaction());
            var document = _builder.Build();

            // Assert
            var pmtInf = document.CstmrCdtTrfInitn.PmtInf[0];
            Assert.IsFalse(pmtInf.BtchBookgSpecified);
            Assert.IsFalse(pmtInf.CtrlSumSpecified);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddPaymentInstruction_NullPaymentInfoId_ShouldThrowArgumentNullException()
        {
            // Act
            _builder.AddPaymentInstruction(null!, PaymentMethod3Code.Trf, null, "1", null, null,
                DateTime.Today.AddDays(1), CreateSampleParty(), CreateSampleCashAccount(), CreateSampleFinancialInstitution());
        }

        [TestMethod]
        public void AddCreditTransferTransaction_ShouldAddTransactionCorrectly()
        {
            // Arrange
            _builder.SetGroupHeader("MSG123", DateTime.Now, "1", null, CreateSampleParty());
            _builder.AddPaymentInstruction("PMT123", PaymentMethod3Code.Trf, null, "1", null, null,
                DateTime.Today.AddDays(1), CreateSampleParty(), CreateSampleCashAccount(), CreateSampleFinancialInstitution());
            var transaction = CreateSampleCreditTransferTransaction();

            // Act
            _builder.AddCreditTransferTransaction(transaction);
            var document = _builder.Build();

            // Assert
            var pmtInf = document.CstmrCdtTrfInitn.PmtInf[0];
            Assert.AreEqual(1, pmtInf.CdtTrfTxInf.Count);
            Assert.AreEqual(transaction, pmtInf.CdtTrfTxInf[0]);
        }

        [TestMethod]
        public void AddCreditTransferTransaction_WithParameters_ShouldCreateTransactionCorrectly()
        {
            // Arrange
            _builder.SetGroupHeader("MSG123", DateTime.Now, "1", null, CreateSampleParty());
            _builder.AddPaymentInstruction("PMT123", PaymentMethod3Code.Trf, null, "1", null, null,
                DateTime.Today.AddDays(1), CreateSampleParty(), CreateSampleCashAccount(), CreateSampleFinancialInstitution());
            var endToEndId = "TXN123";
            var amount = CreateSampleAmount();
            var creditor = CreateSampleParty();
            var creditorAccount = CreateSampleCashAccount();
            var instructionId = "INSTR123";

            // Act
            _builder.AddCreditTransferTransaction(endToEndId, amount, creditor, creditorAccount, instructionId);
            var document = _builder.Build();

            // Assert
            var transaction = document.CstmrCdtTrfInitn.PmtInf[0].CdtTrfTxInf[0];
            Assert.AreEqual(endToEndId, transaction.PmtId.EndToEndId);
            Assert.AreEqual(instructionId, transaction.PmtId.InstrId);
            Assert.AreEqual(amount, transaction.Amt);
            Assert.AreEqual(creditor, transaction.Cdtr);
            Assert.AreEqual(creditorAccount, transaction.CdtrAcct);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddCreditTransferTransaction_NoPaymentInstruction_ShouldThrowInvalidOperationException()
        {
            // Act
            _builder.AddCreditTransferTransaction(CreateSampleCreditTransferTransaction());
        }

        [TestMethod]
        public void SetCreditorAgent_ShouldSetCreditorAgentCorrectly()
        {
            // Arrange
            _builder.SetGroupHeader("MSG123", DateTime.Now, "1", null, CreateSampleParty());
            _builder.AddPaymentInstruction("PMT123", PaymentMethod3Code.Trf, null, "1", null, null,
                DateTime.Today.AddDays(1), CreateSampleParty(), CreateSampleCashAccount(), CreateSampleFinancialInstitution());
            _builder.AddCreditTransferTransaction(CreateSampleCreditTransferTransaction());
            var creditorAgent = CreateSampleFinancialInstitution();

            // Act
            _builder.SetCreditorAgent(creditorAgent);
            var document = _builder.Build();

            // Assert
            var transaction = document.CstmrCdtTrfInitn.PmtInf[0].CdtTrfTxInf[0];
            Assert.AreEqual(creditorAgent, transaction.CdtrAgt);
        }

        [TestMethod]
        public void SetRemittanceInformation_ShouldSetRemittanceCorrectly()
        {
            // Arrange
            _builder.SetGroupHeader("MSG123", DateTime.Now, "1", null, CreateSampleParty());
            _builder.AddPaymentInstruction("PMT123", PaymentMethod3Code.Trf, null, "1", null, null,
                DateTime.Today.AddDays(1), CreateSampleParty(), CreateSampleCashAccount(), CreateSampleFinancialInstitution());
            _builder.AddCreditTransferTransaction(CreateSampleCreditTransferTransaction());
            var remittanceInfo = new RemittanceInformation11();
            remittanceInfo.Ustrd.Add("Invoice 12345");

            // Act
            _builder.SetRemittanceInformation(remittanceInfo);
            var document = _builder.Build();

            // Assert
            var transaction = document.CstmrCdtTrfInitn.PmtInf[0].CdtTrfTxInf[0];
            Assert.AreEqual(remittanceInfo, transaction.RmtInf);
        }

        [TestMethod]
        public void SetPurpose_ShouldSetPurposeCorrectly()
        {
            // Arrange
            _builder.SetGroupHeader("MSG123", DateTime.Now, "1", null, CreateSampleParty());
            _builder.AddPaymentInstruction("PMT123", PaymentMethod3Code.Trf, null, "1", null, null,
                DateTime.Today.AddDays(1), CreateSampleParty(), CreateSampleCashAccount(), CreateSampleFinancialInstitution());
            _builder.AddCreditTransferTransaction(CreateSampleCreditTransferTransaction());
            var purpose = new Purpose2Choice { Cd = "SALA" };

            // Act
            _builder.SetPurpose(purpose);
            var document = _builder.Build();

            // Assert
            var transaction = document.CstmrCdtTrfInitn.PmtInf[0].CdtTrfTxInf[0];
            Assert.AreEqual(purpose, transaction.Purp);
        }

        [TestMethod]
        public void AddInstructionForCreditorAgent_ShouldAddInstructionCorrectly()
        {
            // Arrange
            _builder.SetGroupHeader("MSG123", DateTime.Now, "1", null, CreateSampleParty());
            _builder.AddPaymentInstruction("PMT123", PaymentMethod3Code.Trf, null, "1", null, null,
                DateTime.Today.AddDays(1), CreateSampleParty(), CreateSampleCashAccount(), CreateSampleFinancialInstitution());
            _builder.AddCreditTransferTransaction(CreateSampleCreditTransferTransaction());
            var instruction = new InstructionForCreditorAgent1 { Cd = Instruction3Code.Phob, CdSpecified = true };

            // Act
            _builder.AddInstructionForCreditorAgent(instruction);
            var document = _builder.Build();

            // Assert
            var transaction = document.CstmrCdtTrfInitn.PmtInf[0].CdtTrfTxInf[0];
            Assert.AreEqual(1, transaction.InstrForCdtrAgt.Count);
            Assert.AreEqual(instruction, transaction.InstrForCdtrAgt[0]);
        }

        [TestMethod]
        public void SetUltimateDebtor_ShouldSetUltimateDebtorCorrectly()
        {
            // Arrange
            _builder.SetGroupHeader("MSG123", DateTime.Now, "1", null, CreateSampleParty());
            _builder.AddPaymentInstruction("PMT123", PaymentMethod3Code.Trf, null, "1", null, null,
                DateTime.Today.AddDays(1), CreateSampleParty(), CreateSampleCashAccount(), CreateSampleFinancialInstitution());
            _builder.AddCreditTransferTransaction(CreateSampleCreditTransferTransaction());
            var ultimateDebtor = CreateSampleParty();

            // Act
            _builder.SetUltimateDebtor(ultimateDebtor);
            var document = _builder.Build();

            // Assert
            var transaction = document.CstmrCdtTrfInitn.PmtInf[0].CdtTrfTxInf[0];
            Assert.AreEqual(ultimateDebtor, transaction.UltmtDbtr);
        }

        [TestMethod]
        public void SetUltimateCreditor_ShouldSetUltimateCreditorCorrectly()
        {
            // Arrange
            _builder.SetGroupHeader("MSG123", DateTime.Now, "1", null, CreateSampleParty());
            _builder.AddPaymentInstruction("PMT123", PaymentMethod3Code.Trf, null, "1", null, null,
                DateTime.Today.AddDays(1), CreateSampleParty(), CreateSampleCashAccount(), CreateSampleFinancialInstitution());
            _builder.AddCreditTransferTransaction(CreateSampleCreditTransferTransaction());
            var ultimateCreditor = CreateSampleParty();

            // Act
            _builder.SetUltimateCreditor(ultimateCreditor);
            var document = _builder.Build();

            // Assert
            var transaction = document.CstmrCdtTrfInitn.PmtInf[0].CdtTrfTxInf[0];
            Assert.AreEqual(ultimateCreditor, transaction.UltmtCdtr);
        }

        [TestMethod]
        public void AddSupplementaryData_ShouldAddSupplementaryDataCorrectly()
        {
            // Arrange
            _builder.SetGroupHeader("MSG123", DateTime.Now, "1", null, CreateSampleParty());
            _builder.AddPaymentInstruction("PMT123", PaymentMethod3Code.Trf, null, "1", null, null,
                DateTime.Today.AddDays(1), CreateSampleParty(), CreateSampleCashAccount(), CreateSampleFinancialInstitution());
            _builder.AddCreditTransferTransaction(CreateSampleCreditTransferTransaction());
            var supplementaryData = new SupplementaryData1
            {
                PlcAndNm = "TestData"
            };

            // Act
            _builder.AddSupplementaryData(supplementaryData);
            var document = _builder.Build();

            // Assert
            Assert.AreEqual(1, document.CstmrCdtTrfInitn.SplmtryData.Count);
            Assert.AreEqual(supplementaryData, document.CstmrCdtTrfInitn.SplmtryData[0]);
        }

        [TestMethod]
        public void BuildXml_WithValidMessage_ShouldReturnXmlString()
        {
            // Arrange
            var document = CreateCompleteDocument();

            // Act
            var xml = _builder.BuildXml(document);

            // Assert
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.Contains("<?xml"));
            Assert.IsTrue(xml.Contains("CstmrCdtTrfInitn"));
            Assert.IsTrue(xml.Contains("urn:iso:std:iso:20022:tech:xsd:pain.001.001.07"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildXml_NullMessage_ShouldThrowArgumentNullException()
        {
            // Act
            _builder.BuildXml(null!);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void BuildXml_InvalidMessageType_ShouldThrowInvalidCastException()
        {
            // Act
            _builder.BuildXml("not a document");
        }

        [TestMethod]
        public void BuildXml_WithoutParameter_ShouldReturnXmlString()
        {
            // Arrange
            SetupCompleteMessage();

            // Act
            var xml = _builder.BuildXml();

            // Assert
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.Contains("<?xml"));
            Assert.IsTrue(xml.Contains("CstmrCdtTrfInitn"));
        }

        [TestMethod]
        public void GetMessageType_ShouldReturnCorrectMessageType()
        {
            // Act
            var messageType = _builder.GetMessageType();

            // Assert
            Assert.AreEqual("pain.001.001.07", messageType);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetCreditorAgent_NoTransactions_ShouldThrowInvalidOperationException()
        {
            // Act
            _builder.SetCreditorAgent(CreateSampleFinancialInstitution());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetRemittanceInformation_NoTransactions_ShouldThrowInvalidOperationException()
        {
            // Act
            _builder.SetRemittanceInformation(new RemittanceInformation11());
        }

        #region Helper Methods

        private void SetupCompleteMessage()
        {
            _builder.SetGroupHeader("MSG123456", DateTime.Now, "1", null, CreateSampleParty());
            _builder.AddPaymentInstruction("PMT123", PaymentMethod3Code.Trf, null, "1", null, null,
                DateTime.Today.AddDays(1), CreateSampleParty(), CreateSampleCashAccount(), CreateSampleFinancialInstitution());
            _builder.AddCreditTransferTransaction(CreateSampleCreditTransferTransaction());
        }

        private Document CreateCompleteDocument()
        {
            SetupCompleteMessage();
            return _builder.Build();
        }

        private PartyIdentification43 CreateSampleParty()
        {
            return new PartyIdentification43
            {
                Nm = "Sample Company Ltd",
                PstlAdr = new PostalAddress6
                {
                    Ctry = "US",
                    TwnNm = "New York"
                }
            };
        }

        private BranchAndFinancialInstitutionIdentification5 CreateSampleFinancialInstitution()
        {
            return new BranchAndFinancialInstitutionIdentification5
            {
                FinInstnId = new FinancialInstitutionIdentification8
                {
                    Bicfi = "BANKUS33XXX",
                    Nm = "Sample Bank"
                }
            };
        }

        private CashAccount24 CreateSampleCashAccount()
        {
            return new CashAccount24
            {
                Id = new AccountIdentification4Choice
                {
                    Iban = "US1234567890123456"
                },
                Ccy = "USD"
            };
        }

        private CreditTransferTransaction26 CreateSampleCreditTransferTransaction()
        {
            return new CreditTransferTransaction26
            {
                PmtId = new PaymentIdentification1
                {
                    EndToEndId = "TXN123456"
                },
                Amt = CreateSampleAmount(),
                Cdtr = CreateSampleParty(),
                CdtrAcct = CreateSampleCashAccount()
            };
        }

        private AmountType4Choice CreateSampleAmount(decimal amount = 100.00m)
        {
            return new AmountType4Choice
            {
                InstdAmt = new ActiveOrHistoricCurrencyAndAmount
                {
                    Ccy = "USD",
                    Value = amount
                }
            };
        }

        #endregion
    }
}