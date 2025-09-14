using System;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Iso20022Library.Application.Builders;
using Iso20022Library.Application.Builders.Pacs;
using Iso20022Library.Domain.Common.Enums;
using Iso20022Library.Messages.Payments.Pacs.Generated.Pacs00900109;

namespace Iso20022Library.Tests.Builders
{
    [TestClass]
    public class Pacs00900109BuilderTests
    {
        private MessageBuilderFactory _factory = null!;
        private Pacs00900109Builder _builder = null!;

        [TestInitialize]
        public void Setup()
        {
            _factory = new MessageBuilderFactory();
            _builder = new Pacs00900109Builder();
        }

        [TestMethod]
        public void MessageBuilderFactory_GetBuilder_ReturnsPacs00900109Builder()
        {
            // Act
            var builder = _factory.GetBuilder(MessageType.Pacs00900109);

            // Assert
            Assert.IsNotNull(builder);
            Assert.IsInstanceOfType(builder, typeof(Pacs00900109Builder));
        }

        [TestMethod]
        public void WithMessageId_SetsMessageId_Correctly()
        {
            // Arrange
            var messageId = "MSG123456789";

            // Act
            var result = _builder.WithMessageId(messageId)
                                 .WithInstructingAgent("BKAUATWW")
                                 .WithInstructedAgent("DEUTDEFF")
                                 .AddCreditTransferTransaction()
                                     .WithPaymentIdentification("INSTR123", "E2E123")
                                     .WithInterbankSettlementAmount(1000m, "EUR")
                                     .WithInstructingAgent("BKAUATWW")
                                     .WithInstructedAgent("DEUTDEFF")
                                     .WithDebtor("DBTRBANK")
                                     .WithCreditor("CDTRBANK")
                                     .AddToBuilder();

            // Assert
            Assert.AreSame(_builder, result);
            var document = _builder.Build();
            Assert.AreEqual(messageId, document.FICdtTrf.GrpHdr.MsgId);
        }

        [TestMethod]
        public void WithCreationDateTime_SetsCreationDateTime_Correctly()
        {
            // Arrange
            var creationDateTime = new DateTime(2023, 12, 25, 14, 30, 0);

            // Act
            var result = _builder.WithCreationDateTime(creationDateTime);

            // Assert
            Assert.AreSame(_builder, result);
            var document = _builder.Build();
            Assert.AreEqual(creationDateTime, document.FICdtTrf.GrpHdr.CreDtTm);
        }

        [TestMethod]
        public void WithBatchBooking_SetsBatchBooking_Correctly()
        {
            // Arrange
            var batchBooking = true;

            // Act
            var result = _builder.WithBatchBooking(batchBooking);

            // Assert
            Assert.AreSame(_builder, result);
            var document = _builder.Build();
            Assert.AreEqual(batchBooking, document.FICdtTrf.GrpHdr.BtchBookg);
            Assert.IsTrue(document.FICdtTrf.GrpHdr.BtchBookgSpecified);
        }

        [TestMethod]
        public void WithNumberOfTransactions_SetsNumberOfTransactions_Correctly()
        {
            // Arrange
            var numberOfTransactions = "5";

            // Act
            var result = _builder.WithNumberOfTransactions(numberOfTransactions);

            // Assert
            Assert.AreSame(_builder, result);
            var document = _builder.Build();
            Assert.AreEqual(numberOfTransactions, document.FICdtTrf.GrpHdr.NbOfTxs);
        }

        [TestMethod]
        public void WithControlSum_SetsControlSum_Correctly()
        {
            // Arrange
            decimal controlSum = 1500.75m;

            // Act
            var result = _builder.WithControlSum(controlSum);

            // Assert
            Assert.AreSame(_builder, result);
            var document = _builder.Build();
            Assert.AreEqual(controlSum, document.FICdtTrf.GrpHdr.CtrlSum);
            Assert.IsTrue(document.FICdtTrf.GrpHdr.CtrlSumSpecified);
        }

        [TestMethod]
        public void WithTotalInterbankSettlementAmount_SetsAmount_Correctly()
        {
            // Arrange
            decimal amount = 2500.00m;
            string currency = "EUR";

            // Act
            var result = _builder.WithTotalInterbankSettlementAmount(amount, currency);

            // Assert
            Assert.AreSame(_builder, result);
            var document = _builder.Build();
            Assert.IsNotNull(document.FICdtTrf.GrpHdr.TtlIntrBkSttlmAmt);
            Assert.AreEqual(amount, document.FICdtTrf.GrpHdr.TtlIntrBkSttlmAmt.Value);
            Assert.AreEqual(currency, document.FICdtTrf.GrpHdr.TtlIntrBkSttlmAmt.Ccy);
        }

        [TestMethod]
        public void WithInterbankSettlementDate_SetsDate_Correctly()
        {
            // Arrange
            var settlementDate = new DateTime(2023, 12, 26);

            // Act
            var result = _builder.WithInterbankSettlementDate(settlementDate);

            // Assert
            Assert.AreSame(_builder, result);
            var document = _builder.Build();
            Assert.AreEqual(settlementDate, document.FICdtTrf.GrpHdr.IntrBkSttlmDt);
            Assert.IsTrue(document.FICdtTrf.GrpHdr.IntrBkSttlmDtSpecified);
        }

        [TestMethod]
        public void WithInstructingAgent_SetsInstructingAgent_Correctly()
        {
            // Arrange
            var bic = "BKAUATWW";
            var name = "Bank Austria AG";

            // Act
            var result = _builder.WithInstructingAgent(bic, name);

            // Assert
            Assert.AreSame(_builder, result);
            var document = _builder.Build();
            Assert.IsNotNull(document.FICdtTrf.GrpHdr.InstgAgt);
            Assert.AreEqual(bic, document.FICdtTrf.GrpHdr.InstgAgt.FinInstnId.BICFI);
            Assert.AreEqual(name, document.FICdtTrf.GrpHdr.InstgAgt.FinInstnId.Nm);
        }

        [TestMethod]
        public void WithInstructedAgent_SetsInstructedAgent_Correctly()
        {
            // Arrange
            var bic = "DEUTDEFF";
            var name = "Deutsche Bank AG";

            // Act
            var result = _builder.WithInstructedAgent(bic, name);

            // Assert
            Assert.AreSame(_builder, result);
            var document = _builder.Build();
            Assert.IsNotNull(document.FICdtTrf.GrpHdr.InstdAgt);
            Assert.AreEqual(bic, document.FICdtTrf.GrpHdr.InstdAgt.FinInstnId.BICFI);
            Assert.AreEqual(name, document.FICdtTrf.GrpHdr.InstdAgt.FinInstnId.Nm);
        }

        [TestMethod]
        public void AddCreditTransferTransaction_ReturnsTransactionBuilder()
        {
            // Act
            var transactionBuilder = _builder.AddCreditTransferTransaction();

            // Assert
            Assert.IsNotNull(transactionBuilder);
            Assert.IsInstanceOfType(transactionBuilder, typeof(Pacs00900109Builder.CreditTransferTransactionBuilder));
        }

        [TestMethod]
        public void AddSupplementaryData_AddsSupplementaryData_Correctly()
        {
            // Arrange
            var xmlDoc = new XmlDocument();
            var element = xmlDoc.CreateElement("TestData");
            element.InnerText = "Test Value";

            // Act
            var result = _builder.AddSupplementaryData(element);

            // Assert
            Assert.AreSame(_builder, result);
            var document = _builder.Build();
            Assert.IsNotNull(document.FICdtTrf.SplmtryData);
            Assert.AreEqual(1, document.FICdtTrf.SplmtryData.Length);
            Assert.AreSame(element, document.FICdtTrf.SplmtryData[0].Envlp);
        }

        [TestMethod]
        public void BuildXml_WithValidMessage_ReturnsXmlString()
        {
            // Arrange
            SetupCompleteMessage();

            // Act
            var xml = _builder.BuildXml();

            // Assert
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.Contains("FICdtTrf"));
            Assert.IsTrue(xml.Contains("MSG123"));
            Assert.IsTrue(xml.Contains("BKAUATWW"));
            Assert.IsTrue(xml.Contains("DEUTDEFF"));
        }

        [TestMethod]
        public void BuildXml_WithMessage_ReturnsXmlString()
        {
            // Arrange
            SetupCompleteMessage();
            var dummyMessage = new object();

            // Act
            var xml = _builder.BuildXml(dummyMessage);

            // Assert
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.Contains("FICdtTrf"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void BuildXml_WithoutGroupHeader_ThrowsException()
        {
            // Act
            _builder.BuildXml();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void BuildXml_WithoutMessageId_ThrowsException()
        {
            // Arrange
            _builder.WithInstructingAgent("BKAUATWW")
                   .WithInstructedAgent("DEUTDEFF");

            // Act
            _builder.BuildXml();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void BuildXml_WithoutInstructingAgent_ThrowsException()
        {
            // Arrange
            _builder.WithMessageId("MSG123")
                   .WithInstructedAgent("DEUTDEFF");

            // Act
            _builder.BuildXml();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void BuildXml_WithoutInstructedAgent_ThrowsException()
        {
            // Arrange
            _builder.WithMessageId("MSG123")
                   .WithInstructingAgent("BKAUATWW");

            // Act
            _builder.BuildXml();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void BuildXml_WithoutTransactions_ThrowsException()
        {
            // Arrange
            _builder.WithMessageId("MSG123")
                   .WithInstructingAgent("BKAUATWW")
                   .WithInstructedAgent("DEUTDEFF");

            // Act
            _builder.BuildXml();
        }

        // Transaction Builder Tests

        [TestMethod]
        public void TransactionBuilder_WithPaymentIdentification_SetsPaymentId_Correctly()
        {
            // Arrange
            var instructionId = "INSTR123";
            var endToEndId = "E2E123";
            var transactionId = "TXN123";
            var uetr = "UETR123";

            // Act
            var xml = _builder
                .WithMessageId("MSG123")
                .WithInstructingAgent("BKAUATWW")
                .WithInstructedAgent("DEUTDEFF")
                .AddCreditTransferTransaction()
                    .WithPaymentIdentification(instructionId, endToEndId, transactionId, uetr)
                    .WithInterbankSettlementAmount(1000m, "EUR")
                    .WithInstructingAgent("BKAUATWW")
                    .WithInstructedAgent("DEUTDEFF")
                    .WithDebtor("DBTRBANK")
                    .WithCreditor("CDTRBANK")
                    .AddToBuilder()
                .BuildXml();

            // Assert
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.Contains(instructionId));
            Assert.IsTrue(xml.Contains(endToEndId));
            Assert.IsTrue(xml.Contains(transactionId));
            Assert.IsTrue(xml.Contains(uetr));
        }

        [TestMethod]
        public void TransactionBuilder_WithPaymentTypeInformation_SetsPaymentType_Correctly()
        {
            // Arrange
            var priority = Priority2Code.HIGH;
            var serviceLevel = new ServiceLevel8Choice();

            // Act
            SetupCompleteMessage();
            var document = _builder
                .AddCreditTransferTransaction()
                    .WithPaymentIdentification("INSTR123", "E2E123")
                    .WithInterbankSettlementAmount(1000m, "EUR")
                    .WithInstructingAgent("BKAUATWW")
                    .WithInstructedAgent("DEUTDEFF")
                    .WithDebtor("DBTRBANK")
                    .WithCreditor("CDTRBANK")
                    .WithPaymentTypeInformation(priority, serviceLevel)
                    .AddToBuilder()
                .Build();

            // Assert
            var transaction = document.FICdtTrf.CdtTrfTxInf[0];
            Assert.IsNotNull(transaction.PmtTpInf);
            Assert.AreEqual(priority, transaction.PmtTpInf.InstrPrty);
            Assert.IsTrue(transaction.PmtTpInf.InstrPrtySpecified);
        }

        [TestMethod]
        public void TransactionBuilder_WithInterbankSettlementAmount_SetsAmount_Correctly()
        {
            // Arrange
            decimal amount = 1500.75m;
            string currency = "USD";

            // Act
            SetupCompleteMessage();
            var document = _builder
                .AddCreditTransferTransaction()
                    .WithPaymentIdentification("INSTR123", "E2E123")
                    .WithInterbankSettlementAmount(amount, currency)
                    .WithInstructingAgent("BKAUATWW")
                    .WithInstructedAgent("DEUTDEFF")
                    .WithDebtor("DBTRBANK")
                    .WithCreditor("CDTRBANK")
                    .AddToBuilder()
                .Build();

            // Assert
            var transaction = document.FICdtTrf.CdtTrfTxInf[0];
            Assert.IsNotNull(transaction.IntrBkSttlmAmt);
            Assert.AreEqual(amount, transaction.IntrBkSttlmAmt.Value);
            Assert.AreEqual(currency, transaction.IntrBkSttlmAmt.Ccy);
        }

        [TestMethod]
        public void TransactionBuilder_WithInterbankSettlementDate_SetsDate_Correctly()
        {
            // Arrange
            var settlementDate = new DateTime(2023, 12, 26);

            // Act
            SetupCompleteMessage();
            var document = _builder
                .AddCreditTransferTransaction()
                    .WithPaymentIdentification("INSTR123", "E2E123")
                    .WithInterbankSettlementAmount(1000m, "EUR")
                    .WithInterbankSettlementDate(settlementDate)
                    .WithInstructingAgent("BKAUATWW")
                    .WithInstructedAgent("DEUTDEFF")
                    .WithDebtor("DBTRBANK")
                    .WithCreditor("CDTRBANK")
                    .AddToBuilder()
                .Build();

            // Assert
            var transaction = document.FICdtTrf.CdtTrfTxInf[0];
            Assert.AreEqual(settlementDate, transaction.IntrBkSttlmDt);
            Assert.IsTrue(transaction.IntrBkSttlmDtSpecified);
        }

        [TestMethod]
        public void TransactionBuilder_WithSettlementPriority_SetsPriority_Correctly()
        {
            // Arrange
            var priority = Priority3Code.HIGH;

            // Act
            SetupCompleteMessage();
            var document = _builder
                .AddCreditTransferTransaction()
                    .WithPaymentIdentification("INSTR123", "E2E123")
                    .WithInterbankSettlementAmount(1000m, "EUR")
                    .WithSettlementPriority(priority)
                    .WithInstructingAgent("BKAUATWW")
                    .WithInstructedAgent("DEUTDEFF")
                    .WithDebtor("DBTRBANK")
                    .WithCreditor("CDTRBANK")
                    .AddToBuilder()
                .Build();

            // Assert
            var transaction = document.FICdtTrf.CdtTrfTxInf[0];
            Assert.AreEqual(priority, transaction.SttlmPrty);
            Assert.IsTrue(transaction.SttlmPrtySpecified);
        }

        [TestMethod]
        public void TransactionBuilder_WithDebtor_SetsDebtor_Correctly()
        {
            // Arrange
            var bic = "DBTRBANK";
            var name = "Debtor Bank";

            // Act
            SetupCompleteMessage();
            var document = _builder
                .AddCreditTransferTransaction()
                    .WithPaymentIdentification("INSTR123", "E2E123")
                    .WithInterbankSettlementAmount(1000m, "EUR")
                    .WithInstructingAgent("BKAUATWW")
                    .WithInstructedAgent("DEUTDEFF")
                    .WithDebtor(bic, name)
                    .WithCreditor("CDTRBANK")
                    .AddToBuilder()
                .Build();

            // Assert
            var transaction = document.FICdtTrf.CdtTrfTxInf[0];
            Assert.IsNotNull(transaction.Dbtr);
            Assert.AreEqual(bic, transaction.Dbtr.FinInstnId.BICFI);
            Assert.AreEqual(name, transaction.Dbtr.FinInstnId.Nm);
        }

        [TestMethod]
        public void TransactionBuilder_WithDebtorAccount_SetsAccount_Correctly()
        {
            // Arrange
            var iban = "GB82WEST12345698765432";
            var currency = "GBP";

            // Act
            SetupCompleteMessage();
            var document = _builder
                .AddCreditTransferTransaction()
                    .WithPaymentIdentification("INSTR123", "E2E123")
                    .WithInterbankSettlementAmount(1000m, "EUR")
                    .WithInstructingAgent("BKAUATWW")
                    .WithInstructedAgent("DEUTDEFF")
                    .WithDebtor("DBTRBANK")
                    .WithDebtorAccount(iban, currency)
                    .WithCreditor("CDTRBANK")
                    .AddToBuilder()
                .Build();

            // Assert
            var transaction = document.FICdtTrf.CdtTrfTxInf[0];
            Assert.IsNotNull(transaction.DbtrAcct);
            Assert.AreEqual(iban, transaction.DbtrAcct.Id.Item);
            Assert.AreEqual(currency, transaction.DbtrAcct.Ccy);
        }

        [TestMethod]
        public void TransactionBuilder_WithCreditor_SetsCreditor_Correctly()
        {
            // Arrange
            var bic = "CDTRBANK";
            var name = "Creditor Bank";

            // Act
            SetupCompleteMessage();
            var document = _builder
                .AddCreditTransferTransaction()
                    .WithPaymentIdentification("INSTR123", "E2E123")
                    .WithInterbankSettlementAmount(1000m, "EUR")
                    .WithInstructingAgent("BKAUATWW")
                    .WithInstructedAgent("DEUTDEFF")
                    .WithDebtor("DBTRBANK")
                    .WithCreditor(bic, name)
                    .AddToBuilder()
                .Build();

            // Assert
            var transaction = document.FICdtTrf.CdtTrfTxInf[0];
            Assert.IsNotNull(transaction.Cdtr);
            Assert.AreEqual(bic, transaction.Cdtr.FinInstnId.BICFI);
            Assert.AreEqual(name, transaction.Cdtr.FinInstnId.Nm);
        }

        [TestMethod]
        public void TransactionBuilder_WithCreditorAccount_SetsAccount_Correctly()
        {
            // Arrange
            var iban = "DE89370400440532013000";
            var currency = "EUR";

            // Act
            SetupCompleteMessage();
            var document = _builder
                .AddCreditTransferTransaction()
                    .WithPaymentIdentification("INSTR123", "E2E123")
                    .WithInterbankSettlementAmount(1000m, "EUR")
                    .WithInstructingAgent("BKAUATWW")
                    .WithInstructedAgent("DEUTDEFF")
                    .WithDebtor("DBTRBANK")
                    .WithCreditor("CDTRBANK")
                    .WithCreditorAccount(iban, currency)
                    .AddToBuilder()
                .Build();

            // Assert
            var transaction = document.FICdtTrf.CdtTrfTxInf[0];
            Assert.IsNotNull(transaction.CdtrAcct);
            Assert.AreEqual(iban, transaction.CdtrAcct.Id.Item);
            Assert.AreEqual(currency, transaction.CdtrAcct.Ccy);
        }

        [TestMethod]
        public void TransactionBuilder_WithRemittanceInformation_SetsRemittance_Correctly()
        {
            // Arrange
            var remittanceInfo = new[] { "Payment for services", "Invoice 12345" };

            // Act
            SetupCompleteMessage();
            var document = _builder
                .AddCreditTransferTransaction()
                    .WithPaymentIdentification("INSTR123", "E2E123")
                    .WithInterbankSettlementAmount(1000m, "EUR")
                    .WithInstructingAgent("BKAUATWW")
                    .WithInstructedAgent("DEUTDEFF")
                    .WithDebtor("DBTRBANK")
                    .WithCreditor("CDTRBANK")
                    .WithRemittanceInformation(remittanceInfo)
                    .AddToBuilder()
                .Build();

            // Assert
            var transaction = document.FICdtTrf.CdtTrfTxInf[0];
            Assert.IsNotNull(transaction.RmtInf);
            Assert.AreEqual(2, transaction.RmtInf.Length);
            Assert.AreEqual(remittanceInfo[0], transaction.RmtInf[0]);
            Assert.AreEqual(remittanceInfo[1], transaction.RmtInf[1]);
        }

        [TestMethod]
        public void TransactionBuilder_AddSupplementaryData_AddsData_Correctly()
        {
            // Arrange
            var xmlDoc = new XmlDocument();
            var element = xmlDoc.CreateElement("TransactionData");
            element.InnerText = "Additional transaction info";

            // Act
            SetupCompleteMessage();
            var document = _builder
                .AddCreditTransferTransaction()
                    .WithPaymentIdentification("INSTR123", "E2E123")
                    .WithInterbankSettlementAmount(1000m, "EUR")
                    .WithInstructingAgent("BKAUATWW")
                    .WithInstructedAgent("DEUTDEFF")
                    .WithDebtor("DBTRBANK")
                    .WithCreditor("CDTRBANK")
                    .AddSupplementaryData(element)
                    .AddToBuilder()
                .Build();

            // Assert
            var transaction = document.FICdtTrf.CdtTrfTxInf[0];
            Assert.IsNotNull(transaction.SplmtryData);
            Assert.AreEqual(1, transaction.SplmtryData.Length);
            Assert.AreSame(element, transaction.SplmtryData[0].Envlp);
        }

        [TestMethod]
        public void Build_WithMultipleTransactions_UpdatesTransactionCount()
        {
            // Arrange
            SetupCompleteMessage();

            // Act
            var document = _builder
                .AddCreditTransferTransaction()
                    .WithPaymentIdentification("INSTR1", "E2E1")
                    .WithInterbankSettlementAmount(1000m, "EUR")
                    .WithInstructingAgent("BKAUATWW")
                    .WithInstructedAgent("DEUTDEFF")
                    .WithDebtor("DBTR1")
                    .WithCreditor("CDTR1")
                    .AddToBuilder()
                .AddCreditTransferTransaction()
                    .WithPaymentIdentification("INSTR2", "E2E2")
                    .WithInterbankSettlementAmount(2000m, "EUR")
                    .WithInstructingAgent("BKAUATWW")
                    .WithInstructedAgent("DEUTDEFF")
                    .WithDebtor("DBTR2")
                    .WithCreditor("CDTR2")
                    .AddToBuilder()
                .Build();

            // Assert
            Assert.AreEqual("2", document.FICdtTrf.GrpHdr.NbOfTxs);
            Assert.AreEqual(2, document.FICdtTrf.CdtTrfTxInf.Length);
        }

        [TestMethod]
        public void FluentInterface_ComplexScenario_WorksCorrectly()
        {
            // Arrange & Act
            var xml = _builder
                .WithMessageId("PACS009MSG001")
                .WithCreationDateTime(DateTime.Now)
                .WithBatchBooking(false)
                .WithControlSum(5000m)
                .WithTotalInterbankSettlementAmount(5000m, "EUR")
                .WithInterbankSettlementDate(DateTime.Today.AddDays(1))
                .WithInstructingAgent("BKAUATWW", "Bank Austria")
                .WithInstructedAgent("DEUTDEFF", "Deutsche Bank")
                .AddCreditTransferTransaction()
                    .WithPaymentIdentification("INSTR001", "E2E001")
                    .WithInterbankSettlementAmount(3000m, "EUR")
                    .WithInstructingAgent("BKAUATWW")
                    .WithInstructedAgent("DEUTDEFF")
                    .WithDebtor("DBTRBANK1", "Debtor Bank 1")
                    .WithDebtorAccount("AT611904300234573201", "EUR")
                    .WithCreditor("CDTRBANK1", "Creditor Bank 1")
                    .WithCreditorAccount("DE89370400440532013000", "EUR")
                    .WithRemittanceInformation("FI transfer 1")
                    .AddToBuilder()
                .AddCreditTransferTransaction()
                    .WithPaymentIdentification("INSTR002", "E2E002")
                    .WithInterbankSettlementAmount(2000m, "EUR")
                    .WithInstructingAgent("BKAUATWW")
                    .WithInstructedAgent("DEUTDEFF")
                    .WithDebtor("DBTRBANK2", "Debtor Bank 2")
                    .WithCreditor("CDTRBANK2", "Creditor Bank 2")
                    .WithRemittanceInformation("FI transfer 2")
                    .AddToBuilder()
                .BuildXml();

            // Assert
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.Contains("PACS009MSG001"));
            Assert.IsTrue(xml.Contains("BKAUATWW"));
            Assert.IsTrue(xml.Contains("DEUTDEFF"));
            Assert.IsTrue(xml.Contains("INSTR001"));
            Assert.IsTrue(xml.Contains("INSTR002"));
            Assert.IsTrue(xml.Contains("FI transfer 1"));
            Assert.IsTrue(xml.Contains("FI transfer 2"));
        }

        private void SetupCompleteMessage()
        {
            _builder
                .WithMessageId("MSG123")
                .WithCreationDateTime(DateTime.Now)
                .WithInstructingAgent("BKAUATWW")
                .WithInstructedAgent("DEUTDEFF")
                .AddCreditTransferTransaction()
                    .WithPaymentIdentification("INSTR123", "E2E123")
                    .WithInterbankSettlementAmount(1000m, "EUR")
                    .WithInstructingAgent("BKAUATWW")
                    .WithInstructedAgent("DEUTDEFF")
                    .WithDebtor("DBTRBANK")
                    .WithCreditor("CDTRBANK")
                    .AddToBuilder();
        }
    }
}
