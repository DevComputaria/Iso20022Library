using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Iso20022Library.Application.Builders.Pain;
using Iso20022Library.Application.Builders;
using Iso20022Library.Domain.Common.Enums;
using Iso20022Library.Messages.Payments.Pain.Generated.Pain00100109;

namespace Iso20022Library.Tests.Builders
{
    /// <summary>
    /// Unit tests for the Pain00100109Builder class.
    /// Tests the functionality of building ISO 20022 pain.001.001.09 messages.
    /// </summary>
    [TestClass]
    public class Pain00100109BuilderTests
    {
        private Pain00100109Builder _builder = null!;

        /// <summary>
        /// Initializes test setup before each test method.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _builder = new Pain00100109Builder();
        }

        /// <summary>
        /// Tests that the builder has the correct message type.
        /// </summary>
        [TestMethod]
        public void MessageType_ShouldBePain00100109()
        {
            // Act
            var messageType = _builder.MessageType;

            // Assert
            Assert.AreEqual(MessageType.Pain00100109, messageType);
        }

        /// <summary>
        /// Tests that the builder can be created through the factory.
        /// </summary>
        [TestMethod]
        public void Factory_ShouldCreatePain00100109Builder()
        {
            // Arrange
            var factory = new MessageBuilderFactory();

            // Act
            var builder = factory.GetBuilder(MessageType.Pain00100109);

            // Assert
            Assert.IsNotNull(builder);
            Assert.IsInstanceOfType(builder, typeof(Pain00100109Builder));
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
            _builder.SetGroupHeader(null, DateTime.Now, "1", null, null);
        }

        /// <summary>
        /// Tests that SetGroupHeader throws ArgumentNullException for null numberOfTransactions.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetGroupHeader_WithNullNumberOfTransactions_ShouldThrowArgumentNullException()
        {
            // Act
            _builder.SetGroupHeader("MSG001", DateTime.Now, null, null, null);
        }

        /// <summary>
        /// Tests that BuildXml method works with a complete message.
        /// </summary>
        [TestMethod]
        public void BuildXml_WithCompleteMessage_ShouldReturnXmlString()
        {
            // Arrange
            var messageId = "MSG001";
            var creationDateTime = DateTime.Now;
            var numberOfTransactions = "1";
            var controlSum = 1000.00m;
            var initiatingParty = new PartyIdentification135 { Nm = "Test Initiator" };
            
            var debtor = new PartyIdentification135 { Nm = "Test Debtor" };
            var debtorAccount = new CashAccount38 
            { 
                Id = new AccountIdentification4Choice { Iban = "DE89370400440532013000" }
            };
            var debtorAgent = new BranchAndFinancialInstitutionIdentification6
            {
                FinInstnId = new FinancialInstitutionIdentification18 { Bicfi = "DEUTDEFF" }
            };

            var creditTransfer = new CreditTransferTransaction34
            {
                PmtId = new PaymentIdentification6 { EndToEndId = "E2E001" },
                Amt = new AmountType4Choice 
                { 
                    InstdAmt = new ActiveOrHistoricCurrencyAndAmount { Ccy = "EUR", Value = 1000.00m }
                },
                Cdtr = new PartyIdentification135 { Nm = "Test Creditor" },
                CdtrAcct = new CashAccount38 
                { 
                    Id = new AccountIdentification4Choice { Iban = "DE89370400440532013001" }
                }
            };

            _builder.SetGroupHeader(messageId, creationDateTime, numberOfTransactions, controlSum, initiatingParty);
            _builder.AddPaymentInstruction("PMTINF001", PaymentMethod3Code.Trf, true, numberOfTransactions, 
                controlSum, null, DateTime.Today.AddDays(1), debtor, debtorAccount, debtorAgent);
            _builder.AddCreditTransferTransaction(creditTransfer);

            // Act
            var xml = _builder.BuildXml();

            // Assert
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.Contains("<?xml"));
            Assert.IsTrue(xml.Contains("CstmrCdtTrfInitn"));
            Assert.IsTrue(xml.Contains("urn:iso:std:iso:20022:tech:xsd:pain.001.001.09"));
            Assert.IsTrue(xml.Contains(messageId));
        }

        /// <summary>
        /// Tests that BuildXml with object parameter works correctly.
        /// </summary>
        [TestMethod]
        public void BuildXml_WithValidDocument_ShouldReturnXmlString()
        {
            // Arrange
            var document = new Document
            {
                CstmrCdtTrfInitn = new CustomerCreditTransferInitiationV09
                {
                    GrpHdr = new GroupHeader85
                    {
                        MsgId = "TEST001",
                        CreDtTm = DateTime.Now,
                        NbOfTxs = "1",
                        InitgPty = new PartyIdentification135 { Nm = "Test" }
                    }
                }
            };

            // Act
            var xml = _builder.BuildXml(document);

            // Assert
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.Contains("<?xml"));
            Assert.IsTrue(xml.Contains("CstmrCdtTrfInitn"));
            Assert.IsTrue(xml.Contains("TEST001"));
        }

        /// <summary>
        /// Tests that BuildXml throws ArgumentNullException for null message.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildXml_WithNullMessage_ShouldThrowArgumentNullException()
        {
            // Act
            _builder.BuildXml(null);
        }

        /// <summary>
        /// Tests that BuildXml throws InvalidCastException for invalid message type.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void BuildXml_WithInvalidMessageType_ShouldThrowInvalidCastException()
        {
            // Act
            _builder.BuildXml("Invalid message type");
        }

        /// <summary>
        /// Tests that Build method validates the document structure.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Build_WithoutGroupHeader_ShouldThrowInvalidOperationException()
        {
            // Act
            _builder.Build();
        }

        /// <summary>
        /// Tests that ToXml method works correctly.
        /// </summary>
        [TestMethod]
        public void ToXml_WithValidMessage_ShouldReturnXmlString()
        {
            // Arrange
            var messageId = "MSG001";
            var creationDateTime = DateTime.Now;
            var numberOfTransactions = "1";
            var initiatingParty = new PartyIdentification135 { Nm = "Test Initiator" };
            
            var debtor = new PartyIdentification135 { Nm = "Test Debtor" };
            var debtorAccount = new CashAccount38 
            { 
                Id = new AccountIdentification4Choice { Iban = "DE89370400440532013000" }
            };
            var debtorAgent = new BranchAndFinancialInstitutionIdentification6
            {
                FinInstnId = new FinancialInstitutionIdentification18 { Bicfi = "DEUTDEFF" }
            };

            var creditTransfer = new CreditTransferTransaction34
            {
                PmtId = new PaymentIdentification6 { EndToEndId = "E2E001" },
                Amt = new AmountType4Choice 
                { 
                    InstdAmt = new ActiveOrHistoricCurrencyAndAmount { Ccy = "EUR", Value = 1000.00m }
                },
                Cdtr = new PartyIdentification135 { Nm = "Test Creditor" },
                CdtrAcct = new CashAccount38 
                { 
                    Id = new AccountIdentification4Choice { Iban = "DE89370400440532013001" }
                }
            };

            _builder.SetGroupHeader(messageId, creationDateTime, numberOfTransactions, null, initiatingParty);
            _builder.AddPaymentInstruction("PMTINF001", PaymentMethod3Code.Trf, null, numberOfTransactions, 
                null, null, DateTime.Today.AddDays(1), debtor, debtorAccount, debtorAgent);
            _builder.AddCreditTransferTransaction(creditTransfer);

            // Act
            var xml = _builder.ToXml();

            // Assert
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.Contains("<?xml"));
            Assert.IsTrue(xml.Contains("CstmrCdtTrfInitn"));
            Assert.IsTrue(xml.Contains("urn:iso:std:iso:20022:tech:xsd:pain.001.001.09"));
        }
    }
}
