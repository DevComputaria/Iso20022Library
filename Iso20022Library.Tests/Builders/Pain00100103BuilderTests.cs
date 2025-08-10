using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Iso20022Library.Application.Builders.Pain;
using Iso20022Library.Messages.Payments.Pain.Generated.Pain00100103;

namespace Iso20022Library.Tests.Builders
{
    /// <summary>
    /// Unit tests for the Pain00100103Builder class.
    /// </summary>
    [TestClass]
    public class Pain00100103BuilderTests
    {
        /// <summary>
        /// Tests that the builder can create a basic Pain.001.001.03 message with required elements.
        /// </summary>
        [TestMethod]
        public void BuildXml_WithRequiredElements_ReturnsValidXml()
        {
            // Arrange
            var builder = new Pain00100103Builder();
            var creationDateTime = new DateTime(2024, 1, 1, 10, 0, 0);

            // Create initiating party
            var initiatingParty = new PartyIdentification32
            {
                Nm = "Test Company Ltd",
                Id = new Party6Choice
                {
                    OrgId = new OrganisationIdentification4
                    {
                        BicOrBei = "TESTGB2L"
                    }
                }
            };

            // Create debtor
            var debtor = new PartyIdentification32
            {
                Nm = "John Doe",
                PstlAdr = new PostalAddress6
                {
                    Ctry = "GB",
                    TwnNm = "London"
                }
            };

            // Create debtor account
            var debtorAccount = new CashAccount16
            {
                Id = new AccountIdentification4Choice
                {
                    Iban = "GB82WEST12345698765432"
                }
            };

            // Create debtor agent
            var debtorAgent = new BranchAndFinancialInstitutionIdentification4
            {
                FinInstnId = new FinancialInstitutionIdentification7
                {
                    Bic = "DEUTGB2L"
                }
            };

            // Create creditor
            var creditor = new PartyIdentification32
            {
                Nm = "Jane Smith",
                PstlAdr = new PostalAddress6
                {
                    Ctry = "GB",
                    TwnNm = "Manchester"
                }
            };

            // Create creditor account
            var creditorAccount = new CashAccount16
            {
                Id = new AccountIdentification4Choice
                {
                    Iban = "GB82WEST98765412345678"
                }
            };

            // Create amount
            var amount = new AmountType3Choice
            {
                InstdAmt = new ActiveOrHistoricCurrencyAndAmount
                {
                    Ccy = "GBP",
                    Value = 1000.00m
                }
            };

            // Act
            var xml = builder
                .WithGroupHeader("MSG001", creationDateTime, "1", initiatingParty, 1000.00m)
                .AddPaymentInstruction("PMTINF001", PaymentMethod3Code.Trf,
                    new DateTime(2024, 1, 2), debtor, debtorAccount, debtorAgent)
                .AddCreditTransferTransaction("E2E001", amount, creditor, creditorAccount)
                .UpdateGroupHeaderTotals()
                .BuildXml();

            // Assert
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.Contains("pain.001.001.03"));
            Assert.IsTrue(xml.Contains("MSG001"));
            Assert.IsTrue(xml.Contains("PMTINF001"));
            Assert.IsTrue(xml.Contains("E2E001"));
            Assert.IsTrue(xml.Contains("Test Company Ltd"));
            Assert.IsTrue(xml.Contains("John Doe"));
            Assert.IsTrue(xml.Contains("Jane Smith"));
            Assert.IsTrue(xml.Contains("1000.00"));
            Assert.IsTrue(xml.Contains("GBP"));
        }

        /// <summary>
        /// Tests that the builder validates required fields and throws appropriate exceptions.
        /// </summary>
        [TestMethod]
        public void Build_WithoutGroupHeader_ThrowsInvalidOperationException()
        {
            // Arrange
            var builder = new Pain00100103Builder();

            // Act & Assert
            var exception = Assert.ThrowsException<InvalidOperationException>(() => builder.Build());
            Assert.IsTrue(exception.Message.Contains("Group header is required"));
        }

        /// <summary>
        /// Tests that the builder validates required fields for payment instructions.
        /// </summary>
        [TestMethod]
        public void Build_WithGroupHeaderButNoPaymentInstructions_ThrowsInvalidOperationException()
        {
            // Arrange
            var builder = new Pain00100103Builder();
            var initiatingParty = new PartyIdentification32 { Nm = "Test Company" };

            // Act
            builder.WithGroupHeader("MSG001", DateTime.Now, "0", initiatingParty);

            // Assert
            var exception = Assert.ThrowsException<InvalidOperationException>(() => builder.Build());
            Assert.IsTrue(exception.Message.Contains("At least one payment instruction is required"));
        }

        /// <summary>
        /// Tests that the UpdateGroupHeaderTotals method correctly calculates transaction counts and amounts.
        /// </summary>
        [TestMethod]
        public void UpdateGroupHeaderTotals_WithMultipleTransactions_CalculatesCorrectTotals()
        {
            // Arrange
            var builder = new Pain00100103Builder();
            var creationDateTime = new DateTime(2024, 1, 1, 10, 0, 0);

            var initiatingParty = new PartyIdentification32 { Nm = "Test Company" };
            var debtor = new PartyIdentification32 { Nm = "Debtor" };
            var debtorAccount = new CashAccount16
            {
                Id = new AccountIdentification4Choice { Iban = "GB82WEST12345698765432" }
            };
            var debtorAgent = new BranchAndFinancialInstitutionIdentification4
            {
                FinInstnId = new FinancialInstitutionIdentification7 { Bic = "DEUTGB2L" }
            };

            var creditor1 = new PartyIdentification32 { Nm = "Creditor 1" };
            var creditor2 = new PartyIdentification32 { Nm = "Creditor 2" };
            var creditorAccount = new CashAccount16
            {
                Id = new AccountIdentification4Choice { Iban = "GB82WEST98765412345678" }
            };

            var amount1 = new AmountType3Choice
            {
                InstdAmt = new ActiveOrHistoricCurrencyAndAmount { Ccy = "GBP", Value = 500.00m }
            };
            var amount2 = new AmountType3Choice
            {
                InstdAmt = new ActiveOrHistoricCurrencyAndAmount { Ccy = "GBP", Value = 750.00m }
            };

            // Act
            builder
                .WithGroupHeader("MSG001", creationDateTime, "2", initiatingParty)
                .AddPaymentInstruction("PMTINF001", PaymentMethod3Code.Trf,
                    new DateTime(2024, 1, 2), debtor, debtorAccount, debtorAgent)
                .AddCreditTransferTransaction("E2E001", amount1, creditor1, creditorAccount)
                .AddCreditTransferTransaction("E2E002", amount2, creditor2, creditorAccount)
                .UpdateGroupHeaderTotals();

            var document = builder.Build();

            // Assert
            Assert.AreEqual("2", document.CstmrCdtTrfInitn.GrpHdr.NbOfTxs);
            Assert.AreEqual(1250.00m, document.CstmrCdtTrfInitn.GrpHdr.CtrlSum);
            Assert.IsTrue(document.CstmrCdtTrfInitn.GrpHdr.CtrlSumSpecified);

            // Check payment instruction totals
            var paymentInstruction = document.CstmrCdtTrfInitn.PmtInf[0];
            Assert.AreEqual("2", paymentInstruction.NbOfTxs);
            Assert.AreEqual(1250.00m, paymentInstruction.CtrlSum);
            Assert.IsTrue(paymentInstruction.CtrlSumSpecified);
        }

        /// <summary>
        /// Tests that the builder correctly handles authorization information.
        /// </summary>
        [TestMethod]
        public void AddAuthorization_WithValidAuthorization_AddsToGroupHeader()
        {
            // Arrange
            var builder = new Pain00100103Builder();
            var authorization = new Authorisation1Choice
            {
                Cd = Authorisation1Code.Auth,
                CdSpecified = true
            };

            var initiatingParty = new PartyIdentification32 { Nm = "Test Company" };
            var debtor = new PartyIdentification32 { Nm = "Debtor" };
            var debtorAccount = new CashAccount16
            {
                Id = new AccountIdentification4Choice { Iban = "GB82WEST12345698765432" }
            };
            var debtorAgent = new BranchAndFinancialInstitutionIdentification4
            {
                FinInstnId = new FinancialInstitutionIdentification7 { Bic = "DEUTGB2L" }
            };
            var creditor = new PartyIdentification32 { Nm = "Creditor" };
            var creditorAccount = new CashAccount16
            {
                Id = new AccountIdentification4Choice { Iban = "GB82WEST98765412345678" }
            };
            var amount = new AmountType3Choice
            {
                InstdAmt = new ActiveOrHistoricCurrencyAndAmount { Ccy = "GBP", Value = 100.00m }
            };

            // Act
            builder
                .WithGroupHeader("MSG001", DateTime.Now, "1", initiatingParty)
                .AddAuthorization(authorization)
                .AddPaymentInstruction("PMTINF001", PaymentMethod3Code.Trf,
                    DateTime.Now.AddDays(1), debtor, debtorAccount, debtorAgent)
                .AddCreditTransferTransaction("E2E001", amount, creditor, creditorAccount);

            var document = builder.Build();

            // Assert
            Assert.IsNotNull(document.CstmrCdtTrfInitn.GrpHdr.Authstn);
            Assert.AreEqual(1, document.CstmrCdtTrfInitn.GrpHdr.Authstn.Count);
            Assert.AreEqual(Authorisation1Code.Auth, document.CstmrCdtTrfInitn.GrpHdr.Authstn[0].Cd);
        }
    }
}
