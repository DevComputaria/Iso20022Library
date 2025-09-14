using Iso20022Library.Domain.Common.Interfaces;
using Iso20022Library.Infrastructure.Xml;
using Iso20022Library.Messages.Payments.Pacs.Generated.Pacs00800109;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Iso20022Library.Application.Builders.Pacs
{
    /// <summary>
    /// Builder for PACS.008.001.09 (FI To FI Customer Credit Transfer V09) messages.
    /// Provides a fluent interface for constructing credit transfer transactions between financial institutions.
    /// </summary>
    public class Pacs00800109Builder : IMessageBuilder
    {
        private readonly FIToFICustomerCreditTransferV09 _fiToFiCustomerCreditTransfer;
        private readonly List<CreditTransferTransaction43> _creditTransferTransactions;

        /// <summary>
        /// Initializes a new instance of the Pacs00800109Builder.
        /// </summary>
        public Pacs00800109Builder()
        {
            _fiToFiCustomerCreditTransfer = new FIToFICustomerCreditTransferV09();
            _creditTransferTransactions = new List<CreditTransferTransaction43>();
            InitializeGroupHeader();
        }

        /// <summary>
        /// Builds the XML representation of the message from a pre-constructed document.
        /// This method satisfies the IMessageBuilder interface requirement.
        /// </summary>
        /// <param name="documentObject">The document object to serialize</param>
        /// <returns>The XML representation of the document</returns>
        public string BuildXml(object documentObject)
        {
            if (documentObject is Document document)
            {
                return XmlSerializationService.Serialize(document);
            }
            throw new ArgumentException("Invalid document type for PACS.008.001.09 builder", nameof(documentObject));
        }

        /// <summary>
        /// Initializes the group header with default values.
        /// </summary>
        private void InitializeGroupHeader()
        {
            _fiToFiCustomerCreditTransfer.GrpHdr = new GroupHeader93
            {
                CreDtTm = DateTime.UtcNow,
                NbOfTxs = "0",
                SttlmInf = new SettlementInstruction7()
            };
        }

        #region Group Header Methods

        /// <summary>
        /// Sets the message identification for the group header.
        /// </summary>
        /// <param name="messageId">Unique message identification</param>
        /// <returns>The builder instance for method chaining</returns>
        public Pacs00800109Builder WithMessageId(string messageId)
        {
            if (string.IsNullOrWhiteSpace(messageId))
                throw new ArgumentException("Message ID cannot be null or empty", nameof(messageId));

            _fiToFiCustomerCreditTransfer.GrpHdr.MsgId = messageId;
            return this;
        }

        /// <summary>
        /// Sets the creation date and time for the group header.
        /// </summary>
        /// <param name="creationDateTime">Message creation date and time</param>
        /// <returns>The builder instance for method chaining</returns>
        public Pacs00800109Builder WithCreationDateTime(DateTime creationDateTime)
        {
            _fiToFiCustomerCreditTransfer.GrpHdr.CreDtTm = creationDateTime;
            return this;
        }

        /// <summary>
        /// Sets the number of transactions in the group header.
        /// This is automatically updated when transactions are added.
        /// </summary>
        /// <param name="numberOfTransactions">Number of transactions</param>
        /// <returns>The builder instance for method chaining</returns>
        public Pacs00800109Builder WithNumberOfTransactions(string numberOfTransactions)
        {
            if (string.IsNullOrWhiteSpace(numberOfTransactions))
                throw new ArgumentException("Number of transactions cannot be null or empty", nameof(numberOfTransactions));

            _fiToFiCustomerCreditTransfer.GrpHdr.NbOfTxs = numberOfTransactions;
            return this;
        }

        /// <summary>
        /// Sets the total control sum for the group header.
        /// </summary>
        /// <param name="controlSum">Total control sum of all transactions</param>
        /// <returns>The builder instance for method chaining</returns>
        public Pacs00800109Builder WithControlSum(decimal controlSum)
        {
            if (controlSum < 0)
                throw new ArgumentException("Control sum cannot be negative", nameof(controlSum));

            _fiToFiCustomerCreditTransfer.GrpHdr.CtrlSum = controlSum;
            _fiToFiCustomerCreditTransfer.GrpHdr.CtrlSumSpecified = true;
            return this;
        }

        /// <summary>
        /// Sets the total interbank settlement amount.
        /// </summary>
        /// <param name="amount">Settlement amount</param>
        /// <param name="currency">Currency code (ISO 4217)</param>
        /// <returns>The builder instance for method chaining</returns>
        public Pacs00800109Builder WithTotalInterbankSettlementAmount(decimal amount, string currency)
        {
            if (amount < 0)
                throw new ArgumentException("Amount cannot be negative", nameof(amount));
            if (string.IsNullOrWhiteSpace(currency))
                throw new ArgumentException("Currency cannot be null or empty", nameof(currency));

            _fiToFiCustomerCreditTransfer.GrpHdr.TtlIntrBkSttlmAmt = new ActiveCurrencyAndAmount
            {
                Value = amount,
                Ccy = currency
            };
            return this;
        }

        /// <summary>
        /// Sets the interbank settlement date.
        /// </summary>
        /// <param name="settlementDate">Settlement date</param>
        /// <returns>The builder instance for method chaining</returns>
        public Pacs00800109Builder WithInterbankSettlementDate(DateTime settlementDate)
        {
            _fiToFiCustomerCreditTransfer.GrpHdr.IntrBkSttlmDt = settlementDate;
            _fiToFiCustomerCreditTransfer.GrpHdr.IntrBkSttlmDtSpecified = true;
            return this;
        }

        /// <summary>
        /// Sets the settlement method for the group header.
        /// </summary>
        /// <param name="settlementMethod">Settlement method code</param>
        /// <returns>The builder instance for method chaining</returns>
        public Pacs00800109Builder WithSettlementMethod(SettlementMethod1Code settlementMethod)
        {
            _fiToFiCustomerCreditTransfer.GrpHdr.SttlmInf.SttlmMtd = settlementMethod;
            return this;
        }

        /// <summary>
        /// Sets the instructing agent for the group header.
        /// </summary>
        /// <param name="bic">BIC of the instructing agent</param>
        /// <param name="name">Name of the instructing agent (optional)</param>
        /// <returns>The builder instance for method chaining</returns>
        public Pacs00800109Builder WithInstructingAgent(string bic, string? name = null)
        {
            if (string.IsNullOrWhiteSpace(bic))
                throw new ArgumentException("BIC cannot be null or empty", nameof(bic));

            _fiToFiCustomerCreditTransfer.GrpHdr.InstgAgt = new BranchAndFinancialInstitutionIdentification6
            {
                FinInstnId = new FinancialInstitutionIdentification18
                {
                    BICFI = bic,
                    Nm = name
                }
            };
            return this;
        }

        /// <summary>
        /// Sets the instructed agent for the group header.
        /// </summary>
        /// <param name="bic">BIC of the instructed agent</param>
        /// <param name="name">Name of the instructed agent (optional)</param>
        /// <returns>The builder instance for method chaining</returns>
        public Pacs00800109Builder WithInstructedAgent(string bic, string? name = null)
        {
            if (string.IsNullOrWhiteSpace(bic))
                throw new ArgumentException("BIC cannot be null or empty", nameof(bic));

            _fiToFiCustomerCreditTransfer.GrpHdr.InstdAgt = new BranchAndFinancialInstitutionIdentification6
            {
                FinInstnId = new FinancialInstitutionIdentification18
                {
                    BICFI = bic,
                    Nm = name
                }
            };
            return this;
        }

        #endregion

        #region Payment Type Information Methods

        /// <summary>
        /// Sets the payment type information for the group header.
        /// </summary>
        /// <param name="instructionPriority">Instruction priority</param>
        /// <param name="serviceLevel">Service level code</param>
        /// <param name="categoryPurpose">Category purpose code</param>
        /// <returns>The builder instance for method chaining</returns>
        public Pacs00800109Builder WithPaymentTypeInformation(
            Priority2Code? instructionPriority = null,
            string? serviceLevel = null,
            string? categoryPurpose = null)
        {
            _fiToFiCustomerCreditTransfer.GrpHdr.PmtTpInf = new PaymentTypeInformation28();

            if (instructionPriority.HasValue)
            {
                _fiToFiCustomerCreditTransfer.GrpHdr.PmtTpInf.InstrPrty = instructionPriority.Value;
                _fiToFiCustomerCreditTransfer.GrpHdr.PmtTpInf.InstrPrtySpecified = true;
            }

            if (!string.IsNullOrWhiteSpace(serviceLevel))
            {
                _fiToFiCustomerCreditTransfer.GrpHdr.PmtTpInf.SvcLvl = new ServiceLevel8Choice[]
                {
                    new ServiceLevel8Choice
                    {
                        Item = serviceLevel,
                        ItemElementName = ItemChoiceType6.Cd
                    }
                };
            }

            if (!string.IsNullOrWhiteSpace(categoryPurpose))
            {
                _fiToFiCustomerCreditTransfer.GrpHdr.PmtTpInf.CtgyPurp = new CategoryPurpose1Choice
                {
                    Item = categoryPurpose,
                    ItemElementName = ItemChoiceType8.Cd
                };
            }

            return this;
        }

        #endregion

        #region Credit Transfer Transaction Methods

        /// <summary>
        /// Starts building a new credit transfer transaction.
        /// </summary>
        /// <returns>A credit transfer transaction builder</returns>
        public CreditTransferTransactionBuilder AddCreditTransferTransaction()
        {
            return new CreditTransferTransactionBuilder(this);
        }

        /// <summary>
        /// Adds a credit transfer transaction to the message.
        /// </summary>
        /// <param name="transaction">The credit transfer transaction to add</param>
        /// <returns>The builder instance for method chaining</returns>
        internal Pacs00800109Builder AddTransaction(CreditTransferTransaction43 transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            _creditTransferTransactions.Add(transaction);
            UpdateTransactionCount();
            return this;
        }

        /// <summary>
        /// Updates the transaction count in the group header.
        /// </summary>
        private void UpdateTransactionCount()
        {
            _fiToFiCustomerCreditTransfer.GrpHdr.NbOfTxs = _creditTransferTransactions.Count.ToString();
        }

        #endregion

        #region Validation and Build Methods

        /// <summary>
        /// Validates the message structure before building.
        /// </summary>
        /// <returns>A list of validation errors, empty if valid</returns>
        public List<string> Validate()
        {
            var errors = new List<string>();

            // Validate group header
            if (string.IsNullOrWhiteSpace(_fiToFiCustomerCreditTransfer.GrpHdr?.MsgId))
                errors.Add("Message ID is required");

            if (_fiToFiCustomerCreditTransfer.GrpHdr?.InstgAgt?.FinInstnId?.BICFI == null)
                errors.Add("Instructing agent BIC is required");

            if (_fiToFiCustomerCreditTransfer.GrpHdr?.InstdAgt?.FinInstnId?.BICFI == null)
                errors.Add("Instructed agent BIC is required");

            // Validate transactions
            if (!_creditTransferTransactions.Any())
                errors.Add("At least one credit transfer transaction is required");

            foreach (var transaction in _creditTransferTransactions)
            {
                if (string.IsNullOrWhiteSpace(transaction.PmtId?.EndToEndId))
                    errors.Add("End-to-end identification is required for all transactions");

                if (transaction.IntrBkSttlmAmt?.Value <= 0)
                    errors.Add("Interbank settlement amount must be greater than zero");

                if (string.IsNullOrWhiteSpace(transaction.IntrBkSttlmAmt?.Ccy))
                    errors.Add("Currency is required for all transactions");

                if (transaction.Dbtr == null)
                    errors.Add("Debtor information is required for all transactions");

                if (transaction.Cdtr == null)
                    errors.Add("Creditor information is required for all transactions");
            }

            return errors;
        }

        /// <summary>
        /// Builds the complete PACS.008.001.09 document.
        /// </summary>
        /// <returns>The complete PACS.008.001.09 document</returns>
        public Document Build()
        {
            var validationErrors = Validate();
            if (validationErrors.Any())
            {
                throw new InvalidOperationException($"Validation failed: {string.Join(", ", validationErrors)}");
            }

            _fiToFiCustomerCreditTransfer.CdtTrfTxInf = _creditTransferTransactions.ToArray();

            return new Document
            {
                FIToFICstmrCdtTrf = _fiToFiCustomerCreditTransfer
            };
        }

        /// <summary>
        /// Builds the complete PACS.008.001.09 document and serializes it to XML.
        /// </summary>
        /// <returns>The XML representation of the PACS.008.001.09 document</returns>
        public string BuildXml()
        {
            var document = Build();
            return XmlSerializationService.Serialize(document);
        }

        #endregion

        #region Nested Builder Classes

        /// <summary>
        /// Builder for individual credit transfer transactions within a PACS.008.001.09 message.
        /// </summary>
        public class CreditTransferTransactionBuilder
        {
            private readonly Pacs00800109Builder _parentBuilder;
            private readonly CreditTransferTransaction43 _transaction;

            internal CreditTransferTransactionBuilder(Pacs00800109Builder parentBuilder)
            {
                _parentBuilder = parentBuilder;
                _transaction = new CreditTransferTransaction43
                {
                    PmtId = new PaymentIdentification13()
                };
            }

            /// <summary>
            /// Sets the payment identification for the transaction.
            /// </summary>
            /// <param name="endToEndId">End-to-end identification</param>
            /// <param name="instructionId">Instruction identification (optional)</param>
            /// <param name="transactionId">Transaction identification (optional)</param>
            /// <param name="uetr">Unique End-to-end Transaction Reference (optional)</param>
            /// <returns>The transaction builder for method chaining</returns>
            public CreditTransferTransactionBuilder WithPaymentIdentification(
                string endToEndId,
                string? instructionId = null,
                string? transactionId = null,
                string? uetr = null)
            {
                if (string.IsNullOrWhiteSpace(endToEndId))
                    throw new ArgumentException("End-to-end ID cannot be null or empty", nameof(endToEndId));

                _transaction.PmtId.EndToEndId = endToEndId;
                _transaction.PmtId.InstrId = instructionId;
                _transaction.PmtId.TxId = transactionId;
                _transaction.PmtId.UETR = uetr;

                return this;
            }

            /// <summary>
            /// Sets the interbank settlement amount for the transaction.
            /// </summary>
            /// <param name="amount">Settlement amount</param>
            /// <param name="currency">Currency code (ISO 4217)</param>
            /// <returns>The transaction builder for method chaining</returns>
            public CreditTransferTransactionBuilder WithInterbankSettlementAmount(decimal amount, string currency)
            {
                if (amount <= 0)
                    throw new ArgumentException("Amount must be greater than zero", nameof(amount));
                if (string.IsNullOrWhiteSpace(currency))
                    throw new ArgumentException("Currency cannot be null or empty", nameof(currency));

                _transaction.IntrBkSttlmAmt = new ActiveCurrencyAndAmount
                {
                    Value = amount,
                    Ccy = currency
                };

                return this;
            }

            /// <summary>
            /// Sets the interbank settlement date for the transaction.
            /// </summary>
            /// <param name="settlementDate">Settlement date</param>
            /// <returns>The transaction builder for method chaining</returns>
            public CreditTransferTransactionBuilder WithInterbankSettlementDate(DateTime settlementDate)
            {
                _transaction.IntrBkSttlmDt = settlementDate;
                _transaction.IntrBkSttlmDtSpecified = true;
                return this;
            }

            /// <summary>
            /// Sets the settlement priority for the transaction.
            /// </summary>
            /// <param name="priority">Settlement priority</param>
            /// <returns>The transaction builder for method chaining</returns>
            public CreditTransferTransactionBuilder WithSettlementPriority(Priority3Code priority)
            {
                _transaction.SttlmPrty = priority;
                _transaction.SttlmPrtySpecified = true;
                return this;
            }

            /// <summary>
            /// Sets the debtor information for the transaction.
            /// </summary>
            /// <param name="name">Debtor name</param>
            /// <param name="country">Debtor country (optional)</param>
            /// <returns>The transaction builder for method chaining</returns>
            public CreditTransferTransactionBuilder WithDebtor(string name, string? country = null)
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Debtor name cannot be null or empty", nameof(name));

                _transaction.Dbtr = new PartyIdentification135
                {
                    Nm = name,
                    CtryOfRes = country
                };

                return this;
            }

            /// <summary>
            /// Sets the debtor account information for the transaction.
            /// </summary>
            /// <param name="iban">IBAN of the debtor account</param>
            /// <param name="currency">Account currency (optional)</param>
            /// <returns>The transaction builder for method chaining</returns>
            public CreditTransferTransactionBuilder WithDebtorAccount(string iban, string? currency = null)
            {
                if (string.IsNullOrWhiteSpace(iban))
                    throw new ArgumentException("IBAN cannot be null or empty", nameof(iban));

                _transaction.DbtrAcct = new CashAccount38
                {
                    Id = new AccountIdentification4Choice
                    {
                        Item = iban
                    },
                    Ccy = currency
                };

                return this;
            }

            /// <summary>
            /// Sets the debtor agent information for the transaction.
            /// </summary>
            /// <param name="bic">BIC of the debtor agent</param>
            /// <param name="name">Name of the debtor agent (optional)</param>
            /// <returns>The transaction builder for method chaining</returns>
            public CreditTransferTransactionBuilder WithDebtorAgent(string bic, string? name = null)
            {
                if (string.IsNullOrWhiteSpace(bic))
                    throw new ArgumentException("BIC cannot be null or empty", nameof(bic));

                _transaction.DbtrAgt = new BranchAndFinancialInstitutionIdentification6
                {
                    FinInstnId = new FinancialInstitutionIdentification18
                    {
                        BICFI = bic,
                        Nm = name
                    }
                };

                return this;
            }

            /// <summary>
            /// Sets the creditor information for the transaction.
            /// </summary>
            /// <param name="name">Creditor name</param>
            /// <param name="country">Creditor country (optional)</param>
            /// <returns>The transaction builder for method chaining</returns>
            public CreditTransferTransactionBuilder WithCreditor(string name, string? country = null)
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Creditor name cannot be null or empty", nameof(name));

                _transaction.Cdtr = new PartyIdentification135
                {
                    Nm = name,
                    CtryOfRes = country
                };

                return this;
            }

            /// <summary>
            /// Sets the creditor account information for the transaction.
            /// </summary>
            /// <param name="iban">IBAN of the creditor account</param>
            /// <param name="currency">Account currency (optional)</param>
            /// <returns>The transaction builder for method chaining</returns>
            public CreditTransferTransactionBuilder WithCreditorAccount(string iban, string? currency = null)
            {
                if (string.IsNullOrWhiteSpace(iban))
                    throw new ArgumentException("IBAN cannot be null or empty", nameof(iban));

                _transaction.CdtrAcct = new CashAccount38
                {
                    Id = new AccountIdentification4Choice
                    {
                        Item = iban
                    },
                    Ccy = currency
                };

                return this;
            }

            /// <summary>
            /// Sets the creditor agent information for the transaction.
            /// </summary>
            /// <param name="bic">BIC of the creditor agent</param>
            /// <param name="name">Name of the creditor agent (optional)</param>
            /// <returns>The transaction builder for method chaining</returns>
            public CreditTransferTransactionBuilder WithCreditorAgent(string bic, string? name = null)
            {
                if (string.IsNullOrWhiteSpace(bic))
                    throw new ArgumentException("BIC cannot be null or empty", nameof(bic));

                _transaction.CdtrAgt = new BranchAndFinancialInstitutionIdentification6
                {
                    FinInstnId = new FinancialInstitutionIdentification18
                    {
                        BICFI = bic,
                        Nm = name
                    }
                };

                return this;
            }

            /// <summary>
            /// Sets remittance information for the transaction.
            /// </summary>
            /// <param name="unstructuredInformation">Unstructured remittance information</param>
            /// <returns>The transaction builder for method chaining</returns>
            public CreditTransferTransactionBuilder WithRemittanceInformation(string unstructuredInformation)
            {
                if (string.IsNullOrWhiteSpace(unstructuredInformation))
                    throw new ArgumentException("Unstructured information cannot be null or empty", nameof(unstructuredInformation));

                _transaction.RmtInf = new RemittanceInformation16
                {
                    Ustrd = new[] { unstructuredInformation }
                };

                return this;
            }

            /// <summary>
            /// Sets the charge bearer for the transaction.
            /// </summary>
            /// <param name="chargeBearer">Charge bearer code</param>
            /// <returns>The transaction builder for method chaining</returns>
            public CreditTransferTransactionBuilder WithChargeBearer(ChargeBearerType1Code chargeBearer)
            {
                _transaction.ChrgBr = chargeBearer;
                return this;
            }

            /// <summary>
            /// Sets the purpose of the transaction.
            /// </summary>
            /// <param name="purposeCode">Purpose code</param>
            /// <returns>The transaction builder for method chaining</returns>
            public CreditTransferTransactionBuilder WithPurpose(string purposeCode)
            {
                if (string.IsNullOrWhiteSpace(purposeCode))
                    throw new ArgumentException("Purpose code cannot be null or empty", nameof(purposeCode));

                _transaction.Purp = new Purpose2Choice
                {
                    Item = purposeCode,
                    ItemElementName = ItemChoiceType12.Cd
                };

                return this;
            }

            /// <summary>
            /// Completes building the transaction and returns to the parent builder.
            /// </summary>
            /// <returns>The parent PACS.008.001.09 builder</returns>
            public Pacs00800109Builder AddTransaction()
            {
                return _parentBuilder.AddTransaction(_transaction);
            }
        }

        #endregion
    }
}
