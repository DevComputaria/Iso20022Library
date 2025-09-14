using Iso20022Library.Application.Builders.Pacs;
using Iso20022Library.Messages.Payments.Pacs.Generated.Pacs00800109;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Iso20022Library.Tests.Builders.Pacs
{
    /// <summary>
    /// Unit tests for the Pacs00800109Builder class.
    /// Tests the builder pattern implementation for PACS.008.001.09 (FI To FI Customer Credit Transfer V09) messages.
    /// </summary>
    [TestClass]
    public class Pacs00800109BuilderTests
    {
        private Pacs00800109Builder _builder = null!;

        [TestInitialize]
        public void Setup()
        {
            _builder = new Pacs00800109Builder();
        }

        #region Group Header Tests

        [TestMethod]
        public void WithMessageId_ValidId_SetsMessageId()
        {
            // Arrange
            const string messageId = "MSG001";

            // Act
            var result = _builder.WithMessageId(messageId);

            // Assert
            Assert.AreSame(_builder, result, "Builder should return itself for method chaining");
            var document = _builder
                .WithInstructingAgent("DEUTDEFF")
                .WithInstructedAgent("BNPAFRPP")
                .AddCreditTransferTransaction()
                    .WithPaymentIdentification("ENDTOEND001")
                    .WithInterbankSettlementAmount(1000m, "EUR")
                    .WithDebtor("John Doe")
                    .WithCreditor("Jane Smith")
                    .AddTransaction()
                .Build();
            Assert.AreEqual(messageId, document.FIToFICstmrCdtTrf.GrpHdr.MsgId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WithMessageId_NullId_ThrowsArgumentException()
        {
            // Act
            _builder.WithMessageId(null!);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WithMessageId_EmptyId_ThrowsArgumentException()
        {
            // Act
            _builder.WithMessageId("");
        }

        [TestMethod]
        public void WithCreationDateTime_ValidDateTime_SetsCreationDateTime()
        {
            // Arrange
            var creationDateTime = new DateTime(2023, 12, 1, 10, 30, 0, DateTimeKind.Utc);

            // Act
            var result = _builder.WithCreationDateTime(creationDateTime);

            // Assert
            Assert.AreSame(_builder, result);
            var document = _builder
                .WithMessageId("MSG001")
                .WithInstructingAgent("DEUTDEFF")
                .WithInstructedAgent("BNPAFRPP")
                .AddCreditTransferTransaction()
                    .WithPaymentIdentification("ENDTOEND001")
                    .WithInterbankSettlementAmount(1000m, "EUR")
                    .WithDebtor("John Doe")
                    .WithCreditor("Jane Smith")
                    .AddTransaction()
                .Build();
            Assert.AreEqual(creationDateTime, document.FIToFICstmrCdtTrf.GrpHdr.CreDtTm);
        }

        [TestMethod]
        public void WithControlSum_ValidAmount_SetsControlSum()
        {
            // Arrange
            const decimal controlSum = 5000.50m;

            // Act
            var result = _builder.WithControlSum(controlSum);

            // Assert
            Assert.AreSame(_builder, result);
            var document = _builder
                .WithMessageId("MSG001")
                .WithInstructingAgent("DEUTDEFF")
                .WithInstructedAgent("BNPAFRPP")
                .AddCreditTransferTransaction()
                    .WithPaymentIdentification("ENDTOEND001")
                    .WithInterbankSettlementAmount(1000m, "EUR")
                    .WithDebtor("John Doe")
                    .WithCreditor("Jane Smith")
                    .AddTransaction()
                .Build();
            Assert.AreEqual(controlSum, document.FIToFICstmrCdtTrf.GrpHdr.CtrlSum);
            Assert.IsTrue(document.FIToFICstmrCdtTrf.GrpHdr.CtrlSumSpecified);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WithControlSum_NegativeAmount_ThrowsArgumentException()
        {
            // Act
            _builder.WithControlSum(-100m);
        }

        [TestMethod]
        public void WithTotalInterbankSettlementAmount_ValidAmountAndCurrency_SetsAmount()
        {
            // Arrange
            const decimal amount = 10000m;
            const string currency = "USD";

            // Act
            var result = _builder.WithTotalInterbankSettlementAmount(amount, currency);

            // Assert
            Assert.AreSame(_builder, result);
            var document = _builder
                .WithMessageId("MSG001")
                .WithInstructingAgent("DEUTDEFF")
                .WithInstructedAgent("BNPAFRPP")
                .AddCreditTransferTransaction()
                    .WithPaymentIdentification("ENDTOEND001")
                    .WithInterbankSettlementAmount(1000m, "EUR")
                    .WithDebtor("John Doe")
                    .WithCreditor("Jane Smith")
                    .AddTransaction()
                .Build();
            Assert.AreEqual(amount, document.FIToFICstmrCdtTrf.GrpHdr.TtlIntrBkSttlmAmt.Value);
            Assert.AreEqual(currency, document.FIToFICstmrCdtTrf.GrpHdr.TtlIntrBkSttlmAmt.Ccy);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WithTotalInterbankSettlementAmount_NegativeAmount_ThrowsArgumentException()
        {
            // Act
            _builder.WithTotalInterbankSettlementAmount(-100m, "EUR");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WithTotalInterbankSettlementAmount_NullCurrency_ThrowsArgumentException()
        {
            // Act
            _builder.WithTotalInterbankSettlementAmount(100m, null!);
        }

        [TestMethod]
        public void WithInterbankSettlementDate_ValidDate_SetsDate()
        {
            // Arrange
            var settlementDate = new DateTime(2023, 12, 2);

            // Act
            var result = _builder.WithInterbankSettlementDate(settlementDate);

            // Assert
            Assert.AreSame(_builder, result);
            var document = _builder
                .WithMessageId("MSG001")
                .WithInstructingAgent("DEUTDEFF")
                .WithInstructedAgent("BNPAFRPP")
                .AddCreditTransferTransaction()
                    .WithPaymentIdentification("ENDTOEND001")
                    .WithInterbankSettlementAmount(1000m, "EUR")
                    .WithDebtor("John Doe")
                    .WithCreditor("Jane Smith")
                    .AddTransaction()
                .Build();
            Assert.AreEqual(settlementDate, document.FIToFICstmrCdtTrf.GrpHdr.IntrBkSttlmDt);
            Assert.IsTrue(document.FIToFICstmrCdtTrf.GrpHdr.IntrBkSttlmDtSpecified);
        }

        [TestMethod]
        public void WithInstructingAgent_ValidBic_SetsInstructingAgent()
        {
            // Arrange
            const string bic = "DEUTDEFF";
            const string name = "Deutsche Bank";

            // Act
            var result = _builder.WithInstructingAgent(bic, name);

            // Assert
            Assert.AreSame(_builder, result);
            var document = _builder
                .WithMessageId("MSG001")
                .WithInstructedAgent("BNPAFRPP")
                .AddCreditTransferTransaction()
                    .WithPaymentIdentification("ENDTOEND001")
                    .WithInterbankSettlementAmount(1000m, "EUR")
                    .WithDebtor("John Doe")
                    .WithCreditor("Jane Smith")
                    .AddTransaction()
                .Build();
            Assert.AreEqual(bic, document.FIToFICstmrCdtTrf.GrpHdr.InstgAgt.FinInstnId.BICFI);
            Assert.AreEqual(name, document.FIToFICstmrCdtTrf.GrpHdr.InstgAgt.FinInstnId.Nm);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WithInstructingAgent_NullBic_ThrowsArgumentException()
        {
            // Act
            _builder.WithInstructingAgent(null!);
        }

        [TestMethod]
        public void WithInstructedAgent_ValidBic_SetsInstructedAgent()
        {
            // Arrange
            const string bic = "BNPAFRPP";
            const string name = "BNP Paribas";

            // Act
            var result = _builder.WithInstructedAgent(bic, name);

            // Assert
            Assert.AreSame(_builder, result);
            var document = _builder
                .WithMessageId("MSG001")
                .WithInstructingAgent("DEUTDEFF")
                .AddCreditTransferTransaction()
                    .WithPaymentIdentification("ENDTOEND001")
                    .WithInterbankSettlementAmount(1000m, "EUR")
                    .WithDebtor("John Doe")
                    .WithCreditor("Jane Smith")
                    .AddTransaction()
                .Build();
            Assert.AreEqual(bic, document.FIToFICstmrCdtTrf.GrpHdr.InstdAgt.FinInstnId.BICFI);
            Assert.AreEqual(name, document.FIToFICstmrCdtTrf.GrpHdr.InstdAgt.FinInstnId.Nm);
        }

        [TestMethod]
        public void WithPaymentTypeInformation_ValidParameters_SetsPaymentTypeInfo()
        {
            // Arrange
            const Priority2Code priority = Priority2Code.HIGH;
            const string serviceLevel = "SEPA";
            const string categoryPurpose = "SALA";

            // Act
            var result = _builder.WithPaymentTypeInformation(priority, serviceLevel, categoryPurpose);

            // Assert
            Assert.AreSame(_builder, result);
            var document = _builder
                .WithMessageId("MSG001")
                .WithInstructingAgent("DEUTDEFF")
                .WithInstructedAgent("BNPAFRPP")
                .AddCreditTransferTransaction()
                    .WithPaymentIdentification("ENDTOEND001")
                    .WithInterbankSettlementAmount(1000m, "EUR")
                    .WithDebtor("John Doe")
                    .WithCreditor("Jane Smith")
                    .AddTransaction()
                .Build();

            var paymentTypeInfo = document.FIToFICstmrCdtTrf.GrpHdr.PmtTpInf;
            Assert.IsNotNull(paymentTypeInfo);
            Assert.AreEqual(priority, paymentTypeInfo.InstrPrty);
            Assert.IsTrue(paymentTypeInfo.InstrPrtySpecified);
            Assert.AreEqual(serviceLevel, paymentTypeInfo.SvcLvl[0].Item);
            Assert.AreEqual(categoryPurpose, paymentTypeInfo.CtgyPurp.Item);
        }

        #endregion

        #region Credit Transfer Transaction Tests

        [TestMethod]
        public void AddCreditTransferTransaction_ValidTransaction_AddsTransaction()
        {
            // Act
            var transactionBuilder = _builder.AddCreditTransferTransaction();

            // Assert
            Assert.IsNotNull(transactionBuilder);
            Assert.IsInstanceOfType(transactionBuilder, typeof(Pacs00800109Builder.CreditTransferTransactionBuilder));
        }

        [TestMethod]
        public void CreditTransferTransaction_WithPaymentIdentification_SetsIdentification()
        {
            // Arrange
            const string endToEndId = "ENDTOEND001";
            const string instructionId = "INSTR001";
            const string transactionId = "TXN001";
            const string uetr = "550e8400-e29b-41d4-a716-446655440000";

            // Act & Assert
            var document = _builder
                .WithMessageId("MSG001")
                .WithInstructingAgent("DEUTDEFF")
                .WithInstructedAgent("BNPAFRPP")
                .AddCreditTransferTransaction()
                    .WithPaymentIdentification(endToEndId, instructionId, transactionId, uetr)
                    .WithInterbankSettlementAmount(1000m, "EUR")
                    .WithDebtor("John Doe")
                    .WithCreditor("Jane Smith")
                    .AddTransaction()
                .Build();

            var transaction = document.FIToFICstmrCdtTrf.CdtTrfTxInf.First();
            Assert.AreEqual(endToEndId, transaction.PmtId.EndToEndId);
            Assert.AreEqual(instructionId, transaction.PmtId.InstrId);
            Assert.AreEqual(transactionId, transaction.PmtId.TxId);
            Assert.AreEqual(uetr, transaction.PmtId.UETR);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreditTransferTransaction_WithPaymentIdentification_NullEndToEndId_ThrowsArgumentException()
        {
            // Act
            _builder.AddCreditTransferTransaction()
                .WithPaymentIdentification(null!);
        }

        [TestMethod]
        public void CreditTransferTransaction_WithInterbankSettlementAmount_SetsAmount()
        {
            // Arrange
            const decimal amount = 2500.75m;
            const string currency = "GBP";

            // Act & Assert
            var document = _builder
                .WithMessageId("MSG001")
                .WithInstructingAgent("DEUTDEFF")
                .WithInstructedAgent("BNPAFRPP")
                .AddCreditTransferTransaction()
                    .WithPaymentIdentification("ENDTOEND001")
                    .WithInterbankSettlementAmount(amount, currency)
                    .WithDebtor("John Doe")
                    .WithCreditor("Jane Smith")
                    .AddTransaction()
                .Build();

            var transaction = document.FIToFICstmrCdtTrf.CdtTrfTxInf.First();
            Assert.AreEqual(amount, transaction.IntrBkSttlmAmt.Value);
            Assert.AreEqual(currency, transaction.IntrBkSttlmAmt.Ccy);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreditTransferTransaction_WithInterbankSettlementAmount_ZeroAmount_ThrowsArgumentException()
        {
            // Act
            _builder.AddCreditTransferTransaction()
                .WithInterbankSettlementAmount(0m, "EUR");
        }

        [TestMethod]
        public void CreditTransferTransaction_WithDebtor_SetsDebtor()
        {
            // Arrange
            const string debtorName = "John Doe Corporation";
            const string country = "DE";

            // Act & Assert
            var document = _builder
                .WithMessageId("MSG001")
                .WithInstructingAgent("DEUTDEFF")
                .WithInstructedAgent("BNPAFRPP")
                .AddCreditTransferTransaction()
                    .WithPaymentIdentification("ENDTOEND001")
                    .WithInterbankSettlementAmount(1000m, "EUR")
                    .WithDebtor(debtorName, country)
                    .WithCreditor("Jane Smith")
                    .AddTransaction()
                .Build();

            var transaction = document.FIToFICstmrCdtTrf.CdtTrfTxInf.First();
            Assert.AreEqual(debtorName, transaction.Dbtr.Nm);
            Assert.AreEqual(country, transaction.Dbtr.CtryOfRes);
        }

        [TestMethod]
        public void CreditTransferTransaction_WithDebtorAccount_SetsAccount()
        {
            // Arrange
            const string iban = "DE89370400440532013000";
            const string currency = "EUR";

            // Act & Assert
            var document = _builder
                .WithMessageId("MSG001")
                .WithInstructingAgent("DEUTDEFF")
                .WithInstructedAgent("BNPAFRPP")
                .AddCreditTransferTransaction()
                    .WithPaymentIdentification("ENDTOEND001")
                    .WithInterbankSettlementAmount(1000m, "EUR")
                    .WithDebtor("John Doe")
                    .WithDebtorAccount(iban, currency)
                    .WithCreditor("Jane Smith")
                    .AddTransaction()
                .Build();

            var transaction = document.FIToFICstmrCdtTrf.CdtTrfTxInf.First();
            Assert.AreEqual(iban, transaction.DbtrAcct.Id.Item);
            Assert.AreEqual(currency, transaction.DbtrAcct.Ccy);
        }

        [TestMethod]
        public void CreditTransferTransaction_WithDebtorAgent_SetsAgent()
        {
            // Arrange
            const string bic = "DEUTDEFF";
            const string name = "Deutsche Bank";

            // Act & Assert
            var document = _builder
                .WithMessageId("MSG001")
                .WithInstructingAgent("DEUTDEFF")
                .WithInstructedAgent("BNPAFRPP")
                .AddCreditTransferTransaction()
                    .WithPaymentIdentification("ENDTOEND001")
                    .WithInterbankSettlementAmount(1000m, "EUR")
                    .WithDebtor("John Doe")
                    .WithDebtorAgent(bic, name)
                    .WithCreditor("Jane Smith")
                    .AddTransaction()
                .Build();

            var transaction = document.FIToFICstmrCdtTrf.CdtTrfTxInf.First();
            Assert.AreEqual(bic, transaction.DbtrAgt.FinInstnId.BICFI);
            Assert.AreEqual(name, transaction.DbtrAgt.FinInstnId.Nm);
        }

        [TestMethod]
        public void CreditTransferTransaction_WithCreditor_SetsCreditor()
        {
            // Arrange
            const string creditorName = "Jane Smith Ltd";
            const string country = "FR";

            // Act & Assert
            var document = _builder
                .WithMessageId("MSG001")
                .WithInstructingAgent("DEUTDEFF")
                .WithInstructedAgent("BNPAFRPP")
                .AddCreditTransferTransaction()
                    .WithPaymentIdentification("ENDTOEND001")
                    .WithInterbankSettlementAmount(1000m, "EUR")
                    .WithDebtor("John Doe")
                    .WithCreditor(creditorName, country)
                    .AddTransaction()
                .Build();

            var transaction = document.FIToFICstmrCdtTrf.CdtTrfTxInf.First();
            Assert.AreEqual(creditorName, transaction.Cdtr.Nm);
            Assert.AreEqual(country, transaction.Cdtr.CtryOfRes);
        }

        [TestMethod]
        public void CreditTransferTransaction_WithCreditorAccount_SetsAccount()
        {
            // Arrange
            const string iban = "FR1420041010050500013M02606";
            const string currency = "EUR";

            // Act & Assert
            var document = _builder
                .WithMessageId("MSG001")
                .WithInstructingAgent("DEUTDEFF")
                .WithInstructedAgent("BNPAFRPP")
                .AddCreditTransferTransaction()
                    .WithPaymentIdentification("ENDTOEND001")
                    .WithInterbankSettlementAmount(1000m, "EUR")
                    .WithDebtor("John Doe")
                    .WithCreditor("Jane Smith")
                    .WithCreditorAccount(iban, currency)
                    .AddTransaction()
                .Build();

            var transaction = document.FIToFICstmrCdtTrf.CdtTrfTxInf.First();
            Assert.AreEqual(iban, transaction.CdtrAcct.Id.Item);
            Assert.AreEqual(currency, transaction.CdtrAcct.Ccy);
        }

        [TestMethod]
        public void CreditTransferTransaction_WithCreditorAgent_SetsAgent()
        {
            // Arrange
            const string bic = "BNPAFRPP";
            const string name = "BNP Paribas";

            // Act & Assert
            var document = _builder
                .WithMessageId("MSG001")
                .WithInstructingAgent("DEUTDEFF")
                .WithInstructedAgent("BNPAFRPP")
                .AddCreditTransferTransaction()
                    .WithPaymentIdentification("ENDTOEND001")
                    .WithInterbankSettlementAmount(1000m, "EUR")
                    .WithDebtor("John Doe")
                    .WithCreditor("Jane Smith")
                    .WithCreditorAgent(bic, name)
                    .AddTransaction()
                .Build();

            var transaction = document.FIToFICstmrCdtTrf.CdtTrfTxInf.First();
            Assert.AreEqual(bic, transaction.CdtrAgt.FinInstnId.BICFI);
            Assert.AreEqual(name, transaction.CdtrAgt.FinInstnId.Nm);
        }

        [TestMethod]
        public void CreditTransferTransaction_WithRemittanceInformation_SetsRemittanceInfo()
        {
            // Arrange
            const string remittanceInfo = "Invoice payment for INV-2023-001";

            // Act & Assert
            var document = _builder
                .WithMessageId("MSG001")
                .WithInstructingAgent("DEUTDEFF")
                .WithInstructedAgent("BNPAFRPP")
                .AddCreditTransferTransaction()
                    .WithPaymentIdentification("ENDTOEND001")
                    .WithInterbankSettlementAmount(1000m, "EUR")
                    .WithDebtor("John Doe")
                    .WithCreditor("Jane Smith")
                    .WithRemittanceInformation(remittanceInfo)
                    .AddTransaction()
                .Build();

            var transaction = document.FIToFICstmrCdtTrf.CdtTrfTxInf.First();
            Assert.IsNotNull(transaction.RmtInf);
            Assert.AreEqual(1, transaction.RmtInf.Ustrd.Length);
            Assert.AreEqual(remittanceInfo, transaction.RmtInf.Ustrd[0]);
        }

        [TestMethod]
        public void CreditTransferTransaction_WithChargeBearer_SetsChargeBearer()
        {
            // Arrange
            const ChargeBearerType1Code chargeBearer = ChargeBearerType1Code.SLEV;

            // Act & Assert
            var document = _builder
                .WithMessageId("MSG001")
                .WithInstructingAgent("DEUTDEFF")
                .WithInstructedAgent("BNPAFRPP")
                .AddCreditTransferTransaction()
                    .WithPaymentIdentification("ENDTOEND001")
                    .WithInterbankSettlementAmount(1000m, "EUR")
                    .WithDebtor("John Doe")
                    .WithCreditor("Jane Smith")
                    .WithChargeBearer(chargeBearer)
                    .AddTransaction()
                .Build();

            var transaction = document.FIToFICstmrCdtTrf.CdtTrfTxInf.First();
            Assert.AreEqual(chargeBearer, transaction.ChrgBr);
        }

        [TestMethod]
        public void CreditTransferTransaction_WithPurpose_SetsPurpose()
        {
            // Arrange
            const string purposeCode = "SALA";

            // Act & Assert
            var document = _builder
                .WithMessageId("MSG001")
                .WithInstructingAgent("DEUTDEFF")
                .WithInstructedAgent("BNPAFRPP")
                .AddCreditTransferTransaction()
                    .WithPaymentIdentification("ENDTOEND001")
                    .WithInterbankSettlementAmount(1000m, "EUR")
                    .WithDebtor("John Doe")
                    .WithCreditor("Jane Smith")
                    .WithPurpose(purposeCode)
                    .AddTransaction()
                .Build();

            var transaction = document.FIToFICstmrCdtTrf.CdtTrfTxInf.First();
            Assert.IsNotNull(transaction.Purp);
            Assert.AreEqual(purposeCode, transaction.Purp.Item);
            Assert.AreEqual(ItemChoiceType12.Cd, transaction.Purp.ItemElementName);
        }

        #endregion

        #region Multiple Transactions Tests

        [TestMethod]
        public void Build_MultipleTransactions_UpdatesTransactionCount()
        {
            // Act
            var document = _builder
                .WithMessageId("MSG001")
                .WithInstructingAgent("DEUTDEFF")
                .WithInstructedAgent("BNPAFRPP")
                .AddCreditTransferTransaction()
                    .WithPaymentIdentification("ENDTOEND001")
                    .WithInterbankSettlementAmount(1000m, "EUR")
                    .WithDebtor("John Doe")
                    .WithCreditor("Jane Smith")
                    .AddTransaction()
                .AddCreditTransferTransaction()
                    .WithPaymentIdentification("ENDTOEND002")
                    .WithInterbankSettlementAmount(2000m, "EUR")
                    .WithDebtor("Alice Johnson")
                    .WithCreditor("Bob Wilson")
                    .AddTransaction()
                .Build();

            // Assert
            Assert.AreEqual("2", document.FIToFICstmrCdtTrf.GrpHdr.NbOfTxs);
            Assert.AreEqual(2, document.FIToFICstmrCdtTrf.CdtTrfTxInf.Length);
        }

        #endregion

        #region Validation Tests

        [TestMethod]
        public void Validate_MissingMessageId_ReturnsError()
        {
            // Act
            var errors = _builder.Validate();

            // Assert
            Assert.IsTrue(errors.Contains("Message ID is required"));
        }

        [TestMethod]
        public void Validate_MissingInstructingAgent_ReturnsError()
        {
            // Arrange
            _builder.WithMessageId("MSG001");

            // Act
            var errors = _builder.Validate();

            // Assert
            Assert.IsTrue(errors.Contains("Instructing agent BIC is required"));
        }

        [TestMethod]
        public void Validate_MissingInstructedAgent_ReturnsError()
        {
            // Arrange
            _builder.WithMessageId("MSG001")
                .WithInstructingAgent("DEUTDEFF");

            // Act
            var errors = _builder.Validate();

            // Assert
            Assert.IsTrue(errors.Contains("Instructed agent BIC is required"));
        }

        [TestMethod]
        public void Validate_NoTransactions_ReturnsError()
        {
            // Arrange
            _builder.WithMessageId("MSG001")
                .WithInstructingAgent("DEUTDEFF")
                .WithInstructedAgent("BNPAFRPP");

            // Act
            var errors = _builder.Validate();

            // Assert
            Assert.IsTrue(errors.Contains("At least one credit transfer transaction is required"));
        }

        [TestMethod]
        public void Validate_ValidMessage_ReturnsNoErrors()
        {
            // Arrange
            _builder.WithMessageId("MSG001")
                .WithInstructingAgent("DEUTDEFF")
                .WithInstructedAgent("BNPAFRPP")
                .AddCreditTransferTransaction()
                    .WithPaymentIdentification("ENDTOEND001")
                    .WithInterbankSettlementAmount(1000m, "EUR")
                    .WithDebtor("John Doe")
                    .WithCreditor("Jane Smith")
                    .AddTransaction();

            // Act
            var errors = _builder.Validate();

            // Assert
            Assert.AreEqual(0, errors.Count, $"Expected no validation errors, but got: {string.Join(", ", errors)}");
        }

        #endregion

        #region Build Tests

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Build_InvalidMessage_ThrowsInvalidOperationException()
        {
            // Act
            _builder.Build();
        }

        [TestMethod]
        public void Build_ValidMessage_ReturnsDocument()
        {
            // Act
            var document = _builder
                .WithMessageId("MSG001")
                .WithInstructingAgent("DEUTDEFF")
                .WithInstructedAgent("BNPAFRPP")
                .AddCreditTransferTransaction()
                    .WithPaymentIdentification("ENDTOEND001")
                    .WithInterbankSettlementAmount(1000m, "EUR")
                    .WithDebtor("John Doe")
                    .WithCreditor("Jane Smith")
                    .AddTransaction()
                .Build();

            // Assert
            Assert.IsNotNull(document);
            Assert.IsNotNull(document.FIToFICstmrCdtTrf);
            Assert.IsNotNull(document.FIToFICstmrCdtTrf.GrpHdr);
            Assert.IsNotNull(document.FIToFICstmrCdtTrf.CdtTrfTxInf);
            Assert.AreEqual(1, document.FIToFICstmrCdtTrf.CdtTrfTxInf.Length);
        }

        [TestMethod]
        public void BuildXml_ValidMessage_ReturnsXmlString()
        {
            // Act
            var xml = _builder
                .WithMessageId("MSG001")
                .WithInstructingAgent("DEUTDEFF")
                .WithInstructedAgent("BNPAFRPP")
                .AddCreditTransferTransaction()
                    .WithPaymentIdentification("ENDTOEND001")
                    .WithInterbankSettlementAmount(1000m, "EUR")
                    .WithDebtor("John Doe")
                    .WithCreditor("Jane Smith")
                    .AddTransaction()
                .BuildXml();

            // Assert
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.Contains("<?xml version=\"1.0\" encoding=\"utf-8\"?>"));
            Assert.IsTrue(xml.Contains("FIToFICstmrCdtTrf"));
            Assert.IsTrue(xml.Contains("MSG001"));
            Assert.IsTrue(xml.Contains("ENDTOEND001"));
        }

        #endregion

        #region Complex Scenario Tests

        [TestMethod]
        public void Build_CompleteMessage_AllFieldsSet()
        {
            // Arrange
            var creationDateTime = new DateTime(2023, 12, 1, 10, 30, 0, DateTimeKind.Utc);
            var settlementDate = new DateTime(2023, 12, 2);

            // Act
            var document = _builder
                .WithMessageId("MSG20231201103000")
                .WithCreationDateTime(creationDateTime)
                .WithControlSum(3000m)
                .WithTotalInterbankSettlementAmount(3000m, "EUR")
                .WithInterbankSettlementDate(settlementDate)
                .WithInstructingAgent("DEUTDEFF", "Deutsche Bank")
                .WithInstructedAgent("BNPAFRPP", "BNP Paribas")
                .WithPaymentTypeInformation(Priority2Code.HIGH, "SEPA", "SALA")
                .AddCreditTransferTransaction()
                    .WithPaymentIdentification("ENDTOEND001", "INSTR001", "TXN001", "550e8400-e29b-41d4-a716-446655440000")
                    .WithInterbankSettlementAmount(1500m, "EUR")
                    .WithInterbankSettlementDate(settlementDate)
                    .WithSettlementPriority(Priority3Code.HIGH)
                    .WithDebtor("John Doe Corporation", "DE")
                    .WithDebtorAccount("DE89370400440532013000", "EUR")
                    .WithDebtorAgent("DEUTDEFF", "Deutsche Bank")
                    .WithCreditor("Jane Smith Ltd", "FR")
                    .WithCreditorAccount("FR1420041010050500013M02606", "EUR")
                    .WithCreditorAgent("BNPAFRPP", "BNP Paribas")
                    .WithRemittanceInformation("Invoice payment for INV-2023-001")
                    .WithChargeBearer(ChargeBearerType1Code.SLEV)
                    .WithPurpose("SALA")
                    .AddTransaction()
                .AddCreditTransferTransaction()
                    .WithPaymentIdentification("ENDTOEND002", "INSTR002", "TXN002")
                    .WithInterbankSettlementAmount(1500m, "EUR")
                    .WithDebtor("Alice Johnson Inc", "DE")
                    .WithDebtorAccount("DE91370400440532014000", "EUR")
                    .WithDebtorAgent("DEUTDEFF", "Deutsche Bank")
                    .WithCreditor("Bob Wilson Corp", "FR")
                    .WithCreditorAccount("FR1420041010050500014M02607", "EUR")
                    .WithCreditorAgent("BNPAFRPP", "BNP Paribas")
                    .WithRemittanceInformation("Monthly salary payment")
                    .WithChargeBearer(ChargeBearerType1Code.SHAR)
                    .WithPurpose("SALA")
                    .AddTransaction()
                .Build();

            // Assert
            var groupHeader = document.FIToFICstmrCdtTrf.GrpHdr;
            Assert.AreEqual("MSG20231201103000", groupHeader.MsgId);
            Assert.AreEqual(creationDateTime, groupHeader.CreDtTm);
            Assert.AreEqual("2", groupHeader.NbOfTxs);
            Assert.AreEqual(3000m, groupHeader.CtrlSum);
            Assert.AreEqual(3000m, groupHeader.TtlIntrBkSttlmAmt.Value);
            Assert.AreEqual("EUR", groupHeader.TtlIntrBkSttlmAmt.Ccy);
            Assert.AreEqual(settlementDate, groupHeader.IntrBkSttlmDt);
            Assert.AreEqual("DEUTDEFF", groupHeader.InstgAgt.FinInstnId.BICFI);
            Assert.AreEqual("Deutsche Bank", groupHeader.InstgAgt.FinInstnId.Nm);
            Assert.AreEqual("BNPAFRPP", groupHeader.InstdAgt.FinInstnId.BICFI);
            Assert.AreEqual("BNP Paribas", groupHeader.InstdAgt.FinInstnId.Nm);

            var transactions = document.FIToFICstmrCdtTrf.CdtTrfTxInf;
            Assert.AreEqual(2, transactions.Length);

            // Verify first transaction
            var transaction1 = transactions[0];
            Assert.AreEqual("ENDTOEND001", transaction1.PmtId.EndToEndId);
            Assert.AreEqual("INSTR001", transaction1.PmtId.InstrId);
            Assert.AreEqual("TXN001", transaction1.PmtId.TxId);
            Assert.AreEqual("550e8400-e29b-41d4-a716-446655440000", transaction1.PmtId.UETR);
            Assert.AreEqual(1500m, transaction1.IntrBkSttlmAmt.Value);
            Assert.AreEqual("EUR", transaction1.IntrBkSttlmAmt.Ccy);
            Assert.AreEqual("John Doe Corporation", transaction1.Dbtr.Nm);
            Assert.AreEqual("Jane Smith Ltd", transaction1.Cdtr.Nm);

            // Verify second transaction
            var transaction2 = transactions[1];
            Assert.AreEqual("ENDTOEND002", transaction2.PmtId.EndToEndId);
            Assert.AreEqual("Alice Johnson Inc", transaction2.Dbtr.Nm);
            Assert.AreEqual("Bob Wilson Corp", transaction2.Cdtr.Nm);
        }

        #endregion
    }
}
