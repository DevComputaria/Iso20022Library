using System;
using System.Collections.Generic;
using System.Linq;
using Iso20022Library.Domain.Common.Enums;
using Iso20022Library.Domain.Common.Interfaces;
using Iso20022Library.Infrastructure.Xml;
using Iso20022Library.Messages.Payments.Pacs.Generated.Pacs00900109;

namespace Iso20022Library.Application.Builders.Pacs
{
    /// <summary>
    /// Builder for PACS.009.001.09 (Financial Institution Credit Transfer V09) messages.
    /// Provides a fluent interface for constructing credit transfer transactions between financial institutions.
    /// </summary>
    public class Pacs00900109Builder : IMessageBuilder
    {
        private readonly Document _document;
        private readonly FinancialInstitutionCreditTransferV09 _fiCreditTransfer;
        private GroupHeader93? _groupHeader;
        private readonly List<CreditTransferTransaction44> _creditTransferTransactions;
        private readonly List<SupplementaryData1> _supplementaryData;

        public Pacs00900109Builder()
        {
            _document = new Document();
            _fiCreditTransfer = new FinancialInstitutionCreditTransferV09();
            _document.FICdtTrf = _fiCreditTransfer;
            _creditTransferTransactions = new List<CreditTransferTransaction44>();
            _supplementaryData = new List<SupplementaryData1>();
        }

        /// <summary>
        /// Sets the message identification for the group header.
        /// </summary>
        /// <param name="messageId">Unique message identification</param>
        /// <returns>The builder for method chaining</returns>
        public Pacs00900109Builder WithMessageId(string messageId)
        {
            EnsureGroupHeader();
            _groupHeader!.MsgId = messageId;
            return this;
        }

        /// <summary>
        /// Sets the creation date and time for the group header.
        /// </summary>
        /// <param name="creationDateTime">Creation date and time</param>
        /// <returns>The builder for method chaining</returns>
        public Pacs00900109Builder WithCreationDateTime(DateTime creationDateTime)
        {
            EnsureGroupHeader();
            _groupHeader!.CreDtTm = creationDateTime;
            return this;
        }

        /// <summary>
        /// Sets the batch booking indicator for the group header.
        /// </summary>
        /// <param name="batchBooking">Batch booking indicator</param>
        /// <returns>The builder for method chaining</returns>
        public Pacs00900109Builder WithBatchBooking(bool batchBooking)
        {
            EnsureGroupHeader();
            _groupHeader!.BtchBookg = batchBooking;
            _groupHeader!.BtchBookgSpecified = true;
            return this;
        }

        /// <summary>
        /// Sets the number of transactions in the group header.
        /// </summary>
        /// <param name="numberOfTransactions">Number of transactions</param>
        /// <returns>The builder for method chaining</returns>
        public Pacs00900109Builder WithNumberOfTransactions(string numberOfTransactions)
        {
            EnsureGroupHeader();
            _groupHeader!.NbOfTxs = numberOfTransactions;
            return this;
        }

        /// <summary>
        /// Sets the control sum for the group header.
        /// </summary>
        /// <param name="controlSum">Control sum</param>
        /// <returns>The builder for method chaining</returns>
        public Pacs00900109Builder WithControlSum(decimal? controlSum)
        {
            EnsureGroupHeader();
            if (controlSum.HasValue)
            {
                _groupHeader!.CtrlSum = controlSum.Value;
                _groupHeader!.CtrlSumSpecified = true;
            }
            return this;
        }

        /// <summary>
        /// Sets the total interbank settlement amount for the group header.
        /// </summary>
        /// <param name="amount">Settlement amount</param>
        /// <param name="currency">Currency code (ISO 4217)</param>
        /// <returns>The builder for method chaining</returns>
        public Pacs00900109Builder WithTotalInterbankSettlementAmount(decimal amount, string currency)
        {
            EnsureGroupHeader();
            _groupHeader!.TtlIntrBkSttlmAmt = new ActiveCurrencyAndAmount
            {
                Ccy = currency,
                Value = amount
            };
            return this;
        }

        /// <summary>
        /// Sets the interbank settlement date for the group header.
        /// </summary>
        /// <param name="settlementDate">Settlement date</param>
        /// <returns>The builder for method chaining</returns>
        public Pacs00900109Builder WithInterbankSettlementDate(DateTime? settlementDate)
        {
            EnsureGroupHeader();
            if (settlementDate.HasValue)
            {
                _groupHeader!.IntrBkSttlmDt = settlementDate.Value;
                _groupHeader!.IntrBkSttlmDtSpecified = true;
            }
            return this;
        }

        /// <summary>
        /// Sets the settlement method for the group header.
        /// </summary>
        /// <param name="settlementMethod">Settlement method</param>
        /// <returns>The builder for method chaining</returns>
        public Pacs00900109Builder WithSettlementMethod(SettlementMethod1Code settlementMethod)
        {
            EnsureGroupHeader();
            _groupHeader!.SttlmInf = new SettlementInstruction7
            {
                SttlmMtd = settlementMethod
            };
            return this;
        }

        /// <summary>
        /// Sets the instructing agent for the group header.
        /// </summary>
        /// <param name="bic">BIC of the instructing agent</param>
        /// <param name="name">Name of the instructing agent (optional)</param>
        /// <returns>The builder for method chaining</returns>
        public Pacs00900109Builder WithInstructingAgent(string bic, string? name = null)
        {
            EnsureGroupHeader();
            _groupHeader!.InstgAgt = new BranchAndFinancialInstitutionIdentification6
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
        /// <returns>The builder for method chaining</returns>
        public Pacs00900109Builder WithInstructedAgent(string bic, string? name = null)
        {
            EnsureGroupHeader();
            _groupHeader!.InstdAgt = new BranchAndFinancialInstitutionIdentification6
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
        /// Adds a credit transfer transaction to the message.
        /// </summary>
        /// <returns>A credit transfer transaction builder for fluent configuration</returns>
        public CreditTransferTransactionBuilder AddCreditTransferTransaction()
        {
            return new CreditTransferTransactionBuilder(this);
        }

        /// <summary>
        /// Adds supplementary data to the message.
        /// </summary>
        /// <param name="envelope">XML envelope containing the supplementary data</param>
        /// <returns>The builder for method chaining</returns>
        public Pacs00900109Builder AddSupplementaryData(System.Xml.XmlElement envelope)
        {
            _supplementaryData.Add(new SupplementaryData1
            {
                Envlp = envelope
            });
            return this;
        }

        /// <summary>
        /// Builds the XML representation of the PACS.009.001.09 message.
        /// </summary>
        /// <returns>XML string representation of the message</returns>
        public string BuildXml()
        {
            ValidateMessage();
            FinalizeMessage();
            return XmlSerializationService.Serialize(_document);
        }

        /// <summary>
        /// Builds the XML representation of the PACS.009.001.09 message with custom message object.
        /// </summary>
        /// <param name="message">The message object (not used, kept for interface compatibility)</param>
        /// <returns>XML string representation of the message</returns>
        public string BuildXml(object message)
        {
            return BuildXml();
        }

        /// <summary>
        /// Builds the message document.
        /// </summary>
        /// <returns>The document instance</returns>
        public Document Build()
        {
            ValidateMessage();
            FinalizeMessage();
            return _document;
        }

        private void EnsureGroupHeader()
        {
            if (_groupHeader == null)
            {
                _groupHeader = new GroupHeader93();
                _fiCreditTransfer.GrpHdr = _groupHeader;
            }
        }

        private void ValidateMessage()
        {
            if (_groupHeader == null)
                throw new InvalidOperationException("Group header is required for PACS.009.001.09 messages.");

            if (string.IsNullOrWhiteSpace(_groupHeader.MsgId))
                throw new InvalidOperationException("Message ID is required.");

            if (_groupHeader.InstgAgt == null)
                throw new InvalidOperationException("Instructing agent is required.");

            if (_groupHeader.InstdAgt == null)
                throw new InvalidOperationException("Instructed agent is required.");

            if (_creditTransferTransactions.Count == 0)
                throw new InvalidOperationException("At least one credit transfer transaction is required.");
        }

        private void FinalizeMessage()
        {
            // Update number of transactions
            if (_groupHeader != null)
            {
                _groupHeader.NbOfTxs = _creditTransferTransactions.Count.ToString();
            }

            // Set transactions and supplementary data
            _fiCreditTransfer.CdtTrfTxInf = _creditTransferTransactions.ToArray();

            if (_supplementaryData.Count > 0)
            {
                _fiCreditTransfer.SplmtryData = _supplementaryData.ToArray();
            }
        }

        /// <summary>
        /// Builder for individual credit transfer transactions within a PACS.009.001.09 message.
        /// </summary>
        public class CreditTransferTransactionBuilder
        {
            private readonly Pacs00900109Builder _parentBuilder;
            private readonly CreditTransferTransaction44 _transaction;

            internal CreditTransferTransactionBuilder(Pacs00900109Builder parentBuilder)
            {
                _parentBuilder = parentBuilder;
                _transaction = new CreditTransferTransaction44();
            }

            /// <summary>
            /// Sets the payment identification for the transaction.
            /// </summary>
            /// <param name="instructionId">Instruction identification</param>
            /// <param name="endToEndId">End-to-end identification</param>
            /// <param name="transactionId">Transaction identification (optional)</param>
            /// <param name="uetr">UETR (optional)</param>
            /// <returns>The transaction builder for method chaining</returns>
            public CreditTransferTransactionBuilder WithPaymentIdentification(
                string instructionId,
                string endToEndId,
                string? transactionId = null,
                string? uetr = null)
            {
                _transaction.PmtId = new PaymentIdentification13
                {
                    InstrId = instructionId,
                    EndToEndId = endToEndId,
                    TxId = transactionId,
                    UETR = uetr
                };
                return this;
            }

            /// <summary>
            /// Sets the payment type information for the transaction.
            /// </summary>
            /// <param name="instructionPriority">Instruction priority (optional)</param>
            /// <param name="serviceLevel">Service level (optional)</param>
            /// <param name="localInstrument">Local instrument (optional)</param>
            /// <param name="categoryPurpose">Category purpose (optional)</param>
            /// <returns>The transaction builder for method chaining</returns>
            public CreditTransferTransactionBuilder WithPaymentTypeInformation(
                Priority2Code? instructionPriority = null,
                ServiceLevel8Choice? serviceLevel = null,
                LocalInstrument2Choice? localInstrument = null,
                CategoryPurpose1Choice? categoryPurpose = null)
            {
                _transaction.PmtTpInf = new PaymentTypeInformation28
                {
                    InstrPrty = instructionPriority ?? default(Priority2Code),
                    InstrPrtySpecified = instructionPriority.HasValue,
                    SvcLvl = serviceLevel != null ? new[] { serviceLevel } : null,
                    LclInstrm = localInstrument,
                    CtgyPurp = categoryPurpose
                };
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
                _transaction.IntrBkSttlmAmt = new ActiveCurrencyAndAmount
                {
                    Ccy = currency,
                    Value = amount
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
            /// Sets the instructing agent for the transaction.
            /// </summary>
            /// <param name="bic">BIC of the instructing agent</param>
            /// <param name="name">Name of the instructing agent (optional)</param>
            /// <returns>The transaction builder for method chaining</returns>
            public CreditTransferTransactionBuilder WithInstructingAgent(string bic, string? name = null)
            {
                _transaction.InstgAgt = new BranchAndFinancialInstitutionIdentification6
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
            /// Sets the instructed agent for the transaction.
            /// </summary>
            /// <param name="bic">BIC of the instructed agent</param>
            /// <param name="name">Name of the instructed agent (optional)</param>
            /// <returns>The transaction builder for method chaining</returns>
            public CreditTransferTransactionBuilder WithInstructedAgent(string bic, string? name = null)
            {
                _transaction.InstdAgt = new BranchAndFinancialInstitutionIdentification6
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
            /// Sets the debtor information for the transaction.
            /// </summary>
            /// <param name="bic">BIC of the debtor</param>
            /// <param name="name">Name of the debtor (optional)</param>
            /// <returns>The transaction builder for method chaining</returns>
            public CreditTransferTransactionBuilder WithDebtor(string bic, string? name = null)
            {
                _transaction.Dbtr = new BranchAndFinancialInstitutionIdentification6
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
            /// Sets the debtor account information for the transaction.
            /// </summary>
            /// <param name="iban">IBAN of the debtor account</param>
            /// <param name="currency">Account currency (optional)</param>
            /// <returns>The transaction builder for method chaining</returns>
            public CreditTransferTransactionBuilder WithDebtorAccount(string iban, string? currency = null)
            {
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
            /// <param name="bic">BIC of the creditor</param>
            /// <param name="name">Name of the creditor (optional)</param>
            /// <returns>The transaction builder for method chaining</returns>
            public CreditTransferTransactionBuilder WithCreditor(string bic, string? name = null)
            {
                _transaction.Cdtr = new BranchAndFinancialInstitutionIdentification6
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
            /// Sets the creditor account information for the transaction.
            /// </summary>
            /// <param name="iban">IBAN of the creditor account</param>
            /// <param name="currency">Account currency (optional)</param>
            /// <returns>The transaction builder for method chaining</returns>
            public CreditTransferTransactionBuilder WithCreditorAccount(string iban, string? currency = null)
            {
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
            /// Sets the purpose of the transaction.
            /// </summary>
            /// <param name="purpose">Purpose code or proprietary purpose</param>
            /// <returns>The transaction builder for method chaining</returns>
            public CreditTransferTransactionBuilder WithPurpose(Purpose2Choice purpose)
            {
                _transaction.Purp = purpose;
                return this;
            }

            /// <summary>
            /// Sets the remittance information for the transaction.
            /// </summary>
            /// <param name="remittanceInformation">Unstructured remittance information</param>
            /// <returns>The transaction builder for method chaining</returns>
            public CreditTransferTransactionBuilder WithRemittanceInformation(params string[] remittanceInformation)
            {
                _transaction.RmtInf = remittanceInformation;
                return this;
            }

            /// <summary>
            /// Sets the underlying customer credit transfer for the transaction.
            /// </summary>
            /// <param name="underlyingTransaction">Underlying customer credit transfer</param>
            /// <returns>The transaction builder for method chaining</returns>
            public CreditTransferTransactionBuilder WithUnderlyingCustomerCreditTransfer(CreditTransferTransaction45 underlyingTransaction)
            {
                _transaction.UndrlygCstmrCdtTrf = underlyingTransaction;
                return this;
            }

            /// <summary>
            /// Adds supplementary data to the transaction.
            /// </summary>
            /// <param name="envelope">XML envelope containing the supplementary data</param>
            /// <returns>The transaction builder for method chaining</returns>
            public CreditTransferTransactionBuilder AddSupplementaryData(System.Xml.XmlElement envelope)
            {
                var currentData = _transaction.SplmtryData?.ToList() ?? new List<SupplementaryData1>();
                currentData.Add(new SupplementaryData1
                {
                    Envlp = envelope
                });
                _transaction.SplmtryData = currentData.ToArray();
                return this;
            }

            /// <summary>
            /// Adds the configured transaction to the parent builder and returns it.
            /// </summary>
            /// <returns>The parent builder for method chaining</returns>
            public Pacs00900109Builder AddToBuilder()
            {
                _parentBuilder._creditTransferTransactions.Add(_transaction);
                return _parentBuilder;
            }
        }
    }
}
