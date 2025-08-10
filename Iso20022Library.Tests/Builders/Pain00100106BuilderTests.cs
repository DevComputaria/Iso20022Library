using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Iso20022Library.Application.Builders.Pain;
using Iso20022Library.Messages.Payments.Pain.Generated.Pain00100106;

namespace Iso20022Library.Tests.Builders
{
    /// <summary>
    /// Unit tests for the Pain00100106Builder class.
    /// </summary>
    [TestClass]
    public class Pain00100106BuilderTests
    {
        private Pain00100106Builder _builder = null!;

        [TestInitialize]
        public void Setup()
        {
            _builder = new Pain00100106Builder();
        }

        [TestMethod]
        public void Constructor_ShouldInitializeDocumentStructure()
        {
            // Arrange & Act
            var builder = new Pain00100106Builder();
            builder.WithGroupHeader("MSG123456", DateTime.Now, "1", CreateSampleParty());
            builder.AddPaymentInstruction(CreateSamplePaymentInstruction());
            builder.AddCreditTransferTransaction(CreateSampleCreditTransferTransaction());

            // Assert
            var document = builder.Build();
            Assert.IsNotNull(document);
            Assert.IsNotNull(document.CstmrCdtTrfInitn);
            Assert.IsNotNull(document.CstmrCdtTrfInitn.PmtInf);
            Assert.IsNotNull(document.CstmrCdtTrfInitn.SplmtryData);
        }

        [TestMethod]
        public void WithGroupHeader_ShouldSetGroupHeaderCorrectly()
        {
            // Arrange
            var groupHeader = CreateSampleGroupHeader();

            // Act
            _builder.WithGroupHeader(groupHeader);
            _builder.AddPaymentInstruction(CreateSamplePaymentInstruction());
            _builder.AddCreditTransferTransaction(CreateSampleCreditTransferTransaction());
            var document = _builder.Build();

            // Assert
            Assert.AreEqual(groupHeader, document.CstmrCdtTrfInitn.GrpHdr);
            Assert.AreEqual("MSG123456", document.CstmrCdtTrfInitn.GrpHdr.MsgId);
        }

        [TestMethod]
        public void WithGroupHeader_WithParameters_ShouldCreateGroupHeaderCorrectly()
        {
            // Arrange
            var messageId = "MSG123456";
            var creationDateTime = DateTime.Now;
            var numberOfTransactions = "5";
            var initiatingParty = CreateSampleParty();
            var controlSum = 1000.00m;

            // Act
            _builder.WithGroupHeader(messageId, creationDateTime, numberOfTransactions, initiatingParty, controlSum);
            _builder.AddPaymentInstruction(CreateSamplePaymentInstruction());
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
        [ExpectedException(typeof(ArgumentNullException))]
        public void WithGroupHeader_NullGroupHeader_ShouldThrowArgumentNullException()
        {
            // Act
            _builder.WithGroupHeader((GroupHeader48)null!);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WithGroupHeader_NullMessageId_ShouldThrowArgumentNullException()
        {
            // Act
            _builder.WithGroupHeader(null!, DateTime.Now, "1", CreateSampleParty());
        }

        [TestMethod]
        public void AddAuthorization_ShouldAddAuthorizationToGroupHeader()
        {
            // Arrange
            var authorization = new Authorisation1Choice
            {
                Cd = Authorisation1Code.Auth,
                CdSpecified = true
            };

            // Act
            _builder.WithGroupHeader("MSG123456", DateTime.Now, "1", CreateSampleParty());
            _builder.AddAuthorization(authorization);
            _builder.AddPaymentInstruction(CreateSamplePaymentInstruction());
            _builder.AddCreditTransferTransaction(CreateSampleCreditTransferTransaction());
            var document = _builder.Build();

            // Assert
            Assert.IsNotNull(document.CstmrCdtTrfInitn.GrpHdr);
            Assert.AreEqual(1, document.CstmrCdtTrfInitn.GrpHdr.Authstn.Count);
            Assert.AreEqual(authorization, document.CstmrCdtTrfInitn.GrpHdr.Authstn[0]);
        }

        [TestMethod]
        public void WithForwardingAgent_ShouldSetForwardingAgentCorrectly()
        {
            // Arrange
            var forwardingAgent = CreateSampleFinancialInstitution();

            // Act
            _builder.WithGroupHeader("MSG123456", DateTime.Now, "1", CreateSampleParty());
            _builder.WithForwardingAgent(forwardingAgent);
            _builder.AddPaymentInstruction(CreateSamplePaymentInstruction());
            _builder.AddCreditTransferTransaction(CreateSampleCreditTransferTransaction());
            var document = _builder.Build();

            // Assert
            Assert.IsNotNull(document.CstmrCdtTrfInitn.GrpHdr);
            Assert.AreEqual(forwardingAgent, document.CstmrCdtTrfInitn.GrpHdr.FwdgAgt);
        }

        [TestMethod]
        public void AddPaymentInstruction_ShouldAddPaymentInstructionCorrectly()
        {
            // Arrange
            var paymentInstruction = CreateSamplePaymentInstruction();

            // Act
            _builder.WithGroupHeader("MSG123456", DateTime.Now, "1", CreateSampleParty());
            _builder.AddPaymentInstruction(paymentInstruction);
            _builder.AddCreditTransferTransaction(CreateSampleCreditTransferTransaction());
            var document = _builder.Build();

            // Assert
            Assert.AreEqual(1, document.CstmrCdtTrfInitn.PmtInf.Count);
            Assert.AreEqual(paymentInstruction, document.CstmrCdtTrfInitn.PmtInf[0]);
        }

        [TestMethod]
        public void AddPaymentInstruction_WithParameters_ShouldCreatePaymentInstructionCorrectly()
        {
            // Arrange
            var paymentInfoId = "PMT123";
            var paymentMethod = PaymentMethod3Code.Trf;
            var requestedExecutionDate = DateTime.Today.AddDays(1);
            var debtor = CreateSampleParty();
            var debtorAccount = CreateSampleCashAccount();
            var debtorAgent = CreateSampleFinancialInstitution();

            // Act
            _builder.WithGroupHeader("MSG123456", DateTime.Now, "1", CreateSampleParty());
            _builder.AddPaymentInstruction(paymentInfoId, paymentMethod, requestedExecutionDate, debtor, debtorAccount, debtorAgent);
            _builder.AddCreditTransferTransaction(CreateSampleCreditTransferTransaction());
            var document = _builder.Build();

            // Assert
            Assert.AreEqual(1, document.CstmrCdtTrfInitn.PmtInf.Count);
            var pmtInf = document.CstmrCdtTrfInitn.PmtInf[0];
            Assert.AreEqual(paymentInfoId, pmtInf.PmtInfId);
            Assert.AreEqual(paymentMethod, pmtInf.PmtMtd);
            Assert.AreEqual(requestedExecutionDate, pmtInf.ReqdExctnDt);
            Assert.AreEqual(debtor, pmtInf.Dbtr);
            Assert.AreEqual(debtorAccount, pmtInf.DbtrAcct);
            Assert.AreEqual(debtorAgent, pmtInf.DbtrAgt);
        }

        [TestMethod]
        public void SetBatchBooking_ShouldSetBatchBookingCorrectly()
        {
            // Arrange
            _builder.WithGroupHeader("MSG123456", DateTime.Now, "1", CreateSampleParty());
            _builder.AddPaymentInstruction(CreateSamplePaymentInstruction());

            // Act
            _builder.SetBatchBooking(true);
            _builder.AddCreditTransferTransaction(CreateSampleCreditTransferTransaction());
            var document = _builder.Build();

            // Assert
            var pmtInf = document.CstmrCdtTrfInitn.PmtInf[0];
            Assert.IsTrue(pmtInf.BtchBookg);
            Assert.IsTrue(pmtInf.BtchBookgSpecified);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetBatchBooking_NoPaymentInstruction_ShouldThrowInvalidOperationException()
        {
            // Act
            _builder.SetBatchBooking(true);
        }

        [TestMethod]
        public void SetChargeBearer_ShouldSetChargeBearerCorrectly()
        {
            // Arrange
            _builder.WithGroupHeader("MSG123456", DateTime.Now, "1", CreateSampleParty());
            _builder.AddPaymentInstruction(CreateSamplePaymentInstruction());

            // Act
            _builder.SetChargeBearer(ChargeBearerType1Code.Shar);
            _builder.AddCreditTransferTransaction(CreateSampleCreditTransferTransaction());
            var document = _builder.Build();

            // Assert
            var pmtInf = document.CstmrCdtTrfInitn.PmtInf[0];
            Assert.AreEqual(ChargeBearerType1Code.Shar, pmtInf.ChrgBr);
            Assert.IsTrue(pmtInf.ChrgBrSpecified);
        }

        [TestMethod]
        public void AddCreditTransferTransaction_ShouldAddTransactionCorrectly()
        {
            // Arrange
            _builder.WithGroupHeader("MSG123456", DateTime.Now, "1", CreateSampleParty());
            _builder.AddPaymentInstruction(CreateSamplePaymentInstruction());
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
            _builder.WithGroupHeader("MSG123456", DateTime.Now, "1", CreateSampleParty());
            _builder.AddPaymentInstruction(CreateSamplePaymentInstruction());
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
            _builder.WithGroupHeader("MSG123456", DateTime.Now, "1", CreateSampleParty());
            _builder.AddPaymentInstruction(CreateSamplePaymentInstruction());
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
        public void AddUnstructuredRemittance_ShouldAddRemittanceCorrectly()
        {
            // Arrange
            _builder.WithGroupHeader("MSG123456", DateTime.Now, "1", CreateSampleParty());
            _builder.AddPaymentInstruction(CreateSamplePaymentInstruction());
            _builder.AddCreditTransferTransaction(CreateSampleCreditTransferTransaction());
            var remittanceText = "Invoice 12345";

            // Act
            _builder.AddUnstructuredRemittance(remittanceText);
            var document = _builder.Build();

            // Assert
            var transaction = document.CstmrCdtTrfInitn.PmtInf[0].CdtTrfTxInf[0];
            Assert.IsNotNull(transaction.RmtInf);
            Assert.AreEqual(1, transaction.RmtInf.Ustrd.Count);
            Assert.AreEqual(remittanceText, transaction.RmtInf.Ustrd[0]);
        }

        [TestMethod]
        public void AddSupplementaryData_ShouldAddSupplementaryDataCorrectly()
        {
            // Arrange
            _builder.WithGroupHeader("MSG123456", DateTime.Now, "1", CreateSampleParty());
            _builder.AddPaymentInstruction(CreateSamplePaymentInstruction());
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
        public void UpdateGroupHeaderTotals_ShouldCalculateTotalsCorrectly()
        {
            // Arrange
            _builder.WithGroupHeader("MSG123", DateTime.Now, "0", CreateSampleParty());
            _builder.AddPaymentInstruction(CreateSamplePaymentInstruction());
            _builder.AddCreditTransferTransaction("TXN1", CreateSampleAmount(100.00m), CreateSampleParty(), CreateSampleCashAccount());
            _builder.AddCreditTransferTransaction("TXN2", CreateSampleAmount(250.00m), CreateSampleParty(), CreateSampleCashAccount());

            // Act
            _builder.UpdateGroupHeaderTotals();
            var document = _builder.Build();

            // Assert
            var grpHdr = document.CstmrCdtTrfInitn.GrpHdr;
            Assert.AreEqual("2", grpHdr.NbOfTxs);
            Assert.AreEqual(350.00m, grpHdr.CtrlSum);
            Assert.IsTrue(grpHdr.CtrlSumSpecified);
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
            Assert.IsTrue(xml.Contains("urn:iso:std:iso:20022:tech:xsd:pain.001.001.06"));
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
        [ExpectedException(typeof(InvalidOperationException))]
        public void Build_MissingGroupHeader_ShouldThrowInvalidOperationException()
        {
            // Arrange
            _builder.AddPaymentInstruction(CreateSamplePaymentInstruction());

            // Act
            _builder.Build();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Build_MissingPaymentInstruction_ShouldThrowInvalidOperationException()
        {
            // Arrange
            _builder.WithGroupHeader("MSG123", DateTime.Now, "0", CreateSampleParty());

            // Act
            _builder.Build();
        }

        [TestMethod]
        public void Reset_ShouldCreateNewBuilderInstance()
        {
            // Arrange
            _builder.WithGroupHeader("MSG123", DateTime.Now, "1", CreateSampleParty());
            _builder.AddPaymentInstruction(CreateSamplePaymentInstruction());

            // Act
            var newBuilder = _builder.Reset();

            // Assert
            Assert.AreNotSame(_builder, newBuilder);
            Assert.AreEqual(0, newBuilder.GetPaymentInstructionCount());
        }

        [TestMethod]
        public void Clone_ShouldCreateCopyWithSameData()
        {
            // Arrange
            SetupCompleteMessage();

            // Act
            var clonedBuilder = _builder.Clone();

            // Assert
            Assert.AreNotSame(_builder, clonedBuilder);
            Assert.AreEqual(_builder.GetPaymentInstructionCount(), clonedBuilder.GetPaymentInstructionCount());
            Assert.AreEqual(_builder.GetTotalTransactionCount(), clonedBuilder.GetTotalTransactionCount());
        }

        [TestMethod]
        public void GetPaymentInstructionCount_ShouldReturnCorrectCount()
        {
            // Arrange
            _builder.AddPaymentInstruction(CreateSamplePaymentInstruction());
            _builder.AddPaymentInstruction(CreateSamplePaymentInstruction());

            // Act
            var count = _builder.GetPaymentInstructionCount();

            // Assert
            Assert.AreEqual(2, count);
        }

        [TestMethod]
        public void GetTotalTransactionCount_ShouldReturnCorrectCount()
        {
            // Arrange
            _builder.AddPaymentInstruction(CreateSamplePaymentInstruction());
            _builder.AddCreditTransferTransaction(CreateSampleCreditTransferTransaction());
            _builder.AddCreditTransferTransaction(CreateSampleCreditTransferTransaction());
            _builder.AddPaymentInstruction(CreateSamplePaymentInstruction());
            _builder.AddCreditTransferTransaction(CreateSampleCreditTransferTransaction());

            // Act
            var count = _builder.GetTotalTransactionCount();

            // Assert
            Assert.AreEqual(3, count);
        }

        #region Helper Methods

        private void SetupCompleteMessage()
        {
            _builder.WithGroupHeader("MSG123456", DateTime.Now, "1", CreateSampleParty());
            _builder.AddPaymentInstruction(CreateSamplePaymentInstruction());
            _builder.AddCreditTransferTransaction(CreateSampleCreditTransferTransaction());
        }

        private Document CreateCompleteDocument()
        {
            SetupCompleteMessage();
            return _builder.Build();
        }

        private GroupHeader48 CreateSampleGroupHeader()
        {
            return new GroupHeader48
            {
                MsgId = "MSG123456",
                CreDtTm = DateTime.Now,
                NbOfTxs = "1",
                InitgPty = CreateSampleParty()
            };
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

        private PaymentInstruction16 CreateSamplePaymentInstruction()
        {
            return new PaymentInstruction16
            {
                PmtInfId = "PMT123456",
                PmtMtd = PaymentMethod3Code.Trf,
                ReqdExctnDt = DateTime.Today.AddDays(1),
                Dbtr = CreateSampleParty(),
                DbtrAcct = CreateSampleCashAccount(),
                DbtrAgt = CreateSampleFinancialInstitution()
            };
        }

        private CreditTransferTransaction20 CreateSampleCreditTransferTransaction()
        {
            return new CreditTransferTransaction20
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
