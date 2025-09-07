using Iso20022Library.Domain.Common.Interfaces;
using Iso20022Library.Infrastructure.Xml;
using Iso20022Library.Messages.Payments.Pacs.Generated.Pacs00300108;
using Iso20022Library.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Iso20022Library.Application.Builders.Pacs
{
    /// <summary>
    /// Builder for constructing and serializing ISO 20022 pacs.003.001.08 messages (FI To FI Customer Direct Debit V08).
    /// </summary>
    /// <remarks>
    /// The pacs.003.001.08 message is used by financial institutions to process direct debit transactions between institutions.
    /// This message enables the automated collection of payments from customer accounts based on mandates and authorization.
    /// 
    /// Key features of this message:
    /// - Processes direct debit collections between financial institutions
    /// - Supports both single and batch direct debit transactions
    /// - Includes mandate information and debtor/creditor details
    /// - Supports various direct debit schemes and business rules
    /// - Handles settlement instructions and payment type information
    /// 
    /// This builder handles the construction of the complete message structure and its serialization to XML format
    /// according to ISO 20022 standards for direct debit processing.
    /// </remarks>
    public class Pacs00300108Builder : IMessageBuilder
    {
        private readonly Document _document;
        private readonly List<DirectDebitTransactionInformation24> _directDebitTransactions;
        private readonly List<SupplementaryData1> _supplementaryData;

        /// <summary>
        /// Gets the message type supported by this builder.
        /// </summary>
        public MessageType MessageType => MessageType.Pacs00300108;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pacs00300108Builder"/> class.
        /// </summary>
        public Pacs00300108Builder()
        {
            _document = new Document
            {
                FIToFICstmrDrctDbt = new FIToFICustomerDirectDebitV08()
            };
            _directDebitTransactions = new List<DirectDebitTransactionInformation24>();
            _supplementaryData = new List<SupplementaryData1>();
        }

        /// <summary>
        /// Sets the group header for the FI to FI customer direct debit message.
        /// </summary>
        /// <param name="messageId">Unique identifier for the message.</param>
        /// <param name="creationDateTime">Date and time when the message was created.</param>
        /// <param name="numberOfTransactions">Total number of direct debit transactions.</param>
        /// <param name="controlSum">Total amount of all transactions (optional).</param>
        /// <param name="totalInterBankSettlementAmount">Total settlement amount (optional).</param>
        /// <param name="interBankSettlementDate">Settlement date (optional).</param>
        /// <param name="instructingAgent">The instructing financial institution (optional).</param>
        /// <param name="instructedAgent">The instructed financial institution (optional).</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentException">Thrown when messageId is null or empty.</exception>
        public Pacs00300108Builder WithGroupHeader(
            string messageId,
            DateTime creationDateTime,
            string numberOfTransactions,
            decimal? controlSum = null,
            ActiveCurrencyAndAmount totalInterBankSettlementAmount = null,
            DateTime? interBankSettlementDate = null,
            BranchAndFinancialInstitutionIdentification6 instructingAgent = null,
            BranchAndFinancialInstitutionIdentification6 instructedAgent = null)
        {
            if (string.IsNullOrEmpty(messageId))
                throw new ArgumentException("Message ID cannot be null or empty.", nameof(messageId));

            _document.FIToFICstmrDrctDbt.GrpHdr = new GroupHeader94
            {
                MsgId = messageId,
                CreDtTm = creationDateTime,
                NbOfTxs = numberOfTransactions,
                CtrlSum = controlSum ?? 0,
                CtrlSumSpecified = controlSum.HasValue,
                TtlIntrBkSttlmAmt = totalInterBankSettlementAmount,
                IntrBkSttlmDt = interBankSettlementDate ?? DateTime.MinValue,
                IntrBkSttlmDtSpecified = interBankSettlementDate.HasValue,
                InstgAgt = instructingAgent,
                InstdAgt = instructedAgent
            };

            return this;
        }

        /// <summary>
        /// Adds a direct debit transaction to the message.
        /// </summary>
        /// <param name="paymentIdentification">Payment identification information.</param>
        /// <param name="interBankSettlementAmount">Settlement amount between banks.</param>
        /// <param name="instructedAmount">Amount to be debited (optional).</param>
        /// <param name="requestedCollectionDate">Date when collection is requested (optional).</param>
        /// <param name="directDebitTransaction">Direct debit transaction details.</param>
        /// <param name="creditor">Creditor information.</param>
        /// <param name="creditorAccount">Creditor account information (optional).</param>
        /// <param name="creditorAgent">Creditor's financial institution (optional).</param>
        /// <param name="debtor">Debtor information.</param>
        /// <param name="debtorAccount">Debtor account information (optional).</param>
        /// <param name="debtorAgent">Debtor's financial institution (optional).</param>
        /// <param name="paymentTypeInformation">Payment type information (optional).</param>
        /// <param name="settlementTimeIndication">Settlement time indication (optional).</param>
        /// <param name="ultimateCreditor">Ultimate creditor (optional).</param>
        /// <param name="ultimateDebtor">Ultimate debtor (optional).</param>
        /// <param name="initiatingParty">Initiating party (optional).</param>
        /// <param name="instructingAgent">Instructing agent (optional).</param>
        /// <param name="instructedAgent">Instructed agent (optional).</param>
        /// <param name="remittanceInformation">Remittance information (optional).</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when required parameters are null.</exception>
        public Pacs00300108Builder AddDirectDebitTransaction(
            PaymentIdentification7 paymentIdentification,
            ActiveCurrencyAndAmount interBankSettlementAmount,
            ActiveOrHistoricCurrencyAndAmount instructedAmount = null,
            DateTime? requestedCollectionDate = null,
            DirectDebitTransaction10 directDebitTransaction = null,
            PartyIdentification135 creditor = null,
            CashAccount38 creditorAccount = null,
            BranchAndFinancialInstitutionIdentification6 creditorAgent = null,
            PartyIdentification135 debtor = null,
            CashAccount38 debtorAccount = null,
            BranchAndFinancialInstitutionIdentification6 debtorAgent = null,
            PaymentTypeInformation27 paymentTypeInformation = null,
            SettlementDateTimeIndication1 settlementTimeIndication = null,
            PartyIdentification135 ultimateCreditor = null,
            PartyIdentification135 ultimateDebtor = null,
            PartyIdentification135 initiatingParty = null,
            BranchAndFinancialInstitutionIdentification6 instructingAgent = null,
            BranchAndFinancialInstitutionIdentification6 instructedAgent = null,
            RemittanceInformation16 remittanceInformation = null)
        {
            if (paymentIdentification == null)
                throw new ArgumentNullException(nameof(paymentIdentification));
            if (interBankSettlementAmount == null)
                throw new ArgumentNullException(nameof(interBankSettlementAmount));

            var transaction = new DirectDebitTransactionInformation24
            {
                PmtId = paymentIdentification,
                IntrBkSttlmAmt = interBankSettlementAmount,
                InstdAmt = instructedAmount,
                ReqdColltnDt = requestedCollectionDate ?? DateTime.MinValue,
                ReqdColltnDtSpecified = requestedCollectionDate.HasValue,
                DrctDbtTx = directDebitTransaction,
                Cdtr = creditor,
                CdtrAcct = creditorAccount,
                CdtrAgt = creditorAgent,
                Dbtr = debtor,
                DbtrAcct = debtorAccount,
                DbtrAgt = debtorAgent,
                PmtTpInf = paymentTypeInformation,
                SttlmTmIndctn = settlementTimeIndication,
                UltmtCdtr = ultimateCreditor,
                UltmtDbtr = ultimateDebtor,
                InitgPty = initiatingParty,
                InstgAgt = instructingAgent,
                InstdAgt = instructedAgent,
                RmtInf = remittanceInformation
            };

            _directDebitTransactions.Add(transaction);
            return this;
        }

        /// <summary>
        /// Creates a payment identification with end-to-end ID and instruction ID.
        /// </summary>
        /// <param name="endToEndId">End-to-end identification.</param>
        /// <param name="instructionId">Instruction identification (optional).</param>
        /// <param name="transactionId">Transaction identification assigned by instructing agent (optional).</param>
        /// <param name="clearingSystemReference">Clearing system reference (optional).</param>
        /// <returns>A new PaymentIdentification7 instance.</returns>
        public static PaymentIdentification7 CreatePaymentIdentification(
            string endToEndId,
            string instructionId = null,
            string transactionId = null,
            string clearingSystemReference = null)
        {
            return new PaymentIdentification7
            {
                EndToEndId = endToEndId,
                InstrId = instructionId,
                TxId = transactionId,
                ClrSysRef = clearingSystemReference
            };
        }

        /// <summary>
        /// Creates a direct debit transaction with mandate information.
        /// </summary>
        /// <param name="mandateRelatedInformation">Mandate related information.</param>
        /// <param name="creditorSchemeId">Creditor scheme identification (optional).</param>
        /// <param name="preNotificationId">Pre-notification identification (optional).</param>
        /// <param name="preNotificationDate">Pre-notification date (optional).</param>
        /// <returns>A new DirectDebitTransaction10 instance.</returns>
        public static DirectDebitTransaction10 CreateDirectDebitTransaction(
            MandateRelatedInformation14 mandateRelatedInformation,
            PartyIdentification135 creditorSchemeId = null,
            string preNotificationId = null,
            DateTime? preNotificationDate = null)
        {
            return new DirectDebitTransaction10
            {
                MndtRltdInf = mandateRelatedInformation,
                CdtrSchmeId = creditorSchemeId,
                PreNtfctnId = preNotificationId,
                PreNtfctnDt = preNotificationDate ?? DateTime.MinValue,
                PreNtfctnDtSpecified = preNotificationDate.HasValue
            };
        }

        /// <summary>
        /// Creates an active currency and amount.
        /// </summary>
        /// <param name="value">The monetary value.</param>
        /// <param name="currency">The currency code (default: EUR).</param>
        /// <returns>A new ActiveCurrencyAndAmount instance.</returns>
        public static ActiveCurrencyAndAmount CreateActiveAmount(decimal value, string currency = "EUR")
        {
            return new ActiveCurrencyAndAmount
            {
                Value = value,
                Ccy = currency
            };
        }

        /// <summary>
        /// Creates an active or historic currency and amount.
        /// </summary>
        /// <param name="value">The monetary value.</param>
        /// <param name="currency">The currency code (default: EUR).</param>
        /// <returns>A new ActiveOrHistoricCurrencyAndAmount instance.</returns>
        public static ActiveOrHistoricCurrencyAndAmount CreateActiveOrHistoricAmount(decimal value, string currency = "EUR")
        {
            return new ActiveOrHistoricCurrencyAndAmount
            {
                Value = value,
                Ccy = currency
            };
        }

        /// <summary>
        /// Creates party identification information.
        /// </summary>
        /// <param name="name">Party name (optional).</param>
        /// <param name="postalAddress">Postal address (optional).</param>
        /// <param name="identification">Party identification (optional).</param>
        /// <param name="countryOfResidence">Country of residence (optional).</param>
        /// <param name="contactDetails">Contact details (optional).</param>
        /// <returns>A new PartyIdentification135 instance.</returns>
        public static PartyIdentification135 CreatePartyIdentification(
            string name = null,
            PostalAddress24 postalAddress = null,
            Party38Choice identification = null,
            string countryOfResidence = null,
            Contact4 contactDetails = null)
        {
            return new PartyIdentification135
            {
                Nm = name,
                PstlAdr = postalAddress,
                Id = identification,
                CtryOfRes = countryOfResidence,
                CtctDtls = contactDetails
            };
        }

        /// <summary>
        /// Creates a cash account with identification and details.
        /// </summary>
        /// <param name="identification">Account identification.</param>
        /// <param name="type">Account type (optional).</param>
        /// <param name="currency">Account currency (optional).</param>
        /// <param name="name">Account name (optional).</param>
        /// <param name="proxy">Proxy account identification (optional).</param>
        /// <returns>A new CashAccount38 instance.</returns>
        public static CashAccount38 CreateCashAccount(
            AccountIdentification4Choice identification,
            CashAccountType2Choice type = null,
            string currency = null,
            string name = null,
            ProxyAccountIdentification1 proxy = null)
        {
            return new CashAccount38
            {
                Id = identification,
                Tp = type,
                Ccy = currency,
                Nm = name,
                Prxy = proxy
            };
        }

        /// <summary>
        /// Creates a financial institution identification.
        /// </summary>
        /// <param name="financialInstitutionId">Financial institution identification.</param>
        /// <param name="branchId">Branch identification (optional).</param>
        /// <returns>A new BranchAndFinancialInstitutionIdentification6 instance.</returns>
        public static BranchAndFinancialInstitutionIdentification6 CreateFinancialInstitution(
            FinancialInstitutionIdentification18 financialInstitutionId,
            BranchData3 branchId = null)
        {
            return new BranchAndFinancialInstitutionIdentification6
            {
                FinInstnId = financialInstitutionId,
                BrnchId = branchId
            };
        }

        /// <summary>
        /// Adds supplementary data to the message.
        /// </summary>
        /// <param name="data">The supplementary data to add.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when data is null.</exception>
        public Pacs00300108Builder AddSupplementaryData(SupplementaryData1 data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            _supplementaryData.Add(data);
            return this;
        }

        /// <summary>
        /// Creates supplementary data with a placeholder name and envelope.
        /// </summary>
        /// <param name="placeholderName">Name of the supplementary data element (optional).</param>
        /// <param name="envelope">XML envelope containing the supplementary data (optional).</param>
        /// <returns>A new SupplementaryData1 instance.</returns>
        public static SupplementaryData1 CreateSupplementaryData(string placeholderName = null, System.Xml.XmlElement envelope = null)
        {
            return new SupplementaryData1
            {
                PlcAndNm = placeholderName,
                Envlp = envelope
            };
        }

        /// <summary>
        /// Gets the current count of direct debit transactions.
        /// </summary>
        /// <returns>The number of direct debit transactions added to the message.</returns>
        public int GetTransactionCount()
        {
            return _directDebitTransactions.Count;
        }

        /// <summary>
        /// Gets the current count of supplementary data entries.
        /// </summary>
        /// <returns>The number of supplementary data entries added to the message.</returns>
        public int GetSupplementaryDataCount()
        {
            return _supplementaryData.Count;
        }

        /// <summary>
        /// Validates the message and builds the XML representation.
        /// </summary>
        /// <returns>The XML string representation of the pacs.003.001.08 message.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the message is not valid for XML generation.</exception>
        public string BuildXml()
        {
            ValidateMessage();

            // Update arrays with current data
            _document.FIToFICstmrDrctDbt.DrctDbtTxInf = _directDebitTransactions.ToArray();
            _document.FIToFICstmrDrctDbt.SplmtryData = _supplementaryData.Count > 0 ? _supplementaryData.ToArray() : null;

            // Update number of transactions in group header
            if (_document.FIToFICstmrDrctDbt.GrpHdr != null)
            {
                _document.FIToFICstmrDrctDbt.GrpHdr.NbOfTxs = _directDebitTransactions.Count.ToString();
            }

            return XmlSerializationService.Serialize(_document);
        }

        /// <summary>
        /// Builds the XML string representation of the pacs.003.001.08 message from a provided object.
        /// </summary>
        /// <param name="message">The message object to serialize (not used in this implementation).</param>
        /// <returns>The XML string representation of the message.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the message is not properly configured.</exception>
        public string BuildXml(object message)
        {
            return BuildXml();
        }

        /// <summary>
        /// Creates a deep copy of the current builder with the same configuration.
        /// </summary>
        /// <returns>A new instance of Pacs00300108Builder with the same data.</returns>
        public Pacs00300108Builder Clone()
        {
            var clonedBuilder = new Pacs00300108Builder();

            // Clone the document structure
            if (_document.FIToFICstmrDrctDbt.GrpHdr != null)
            {
                var originalHeader = _document.FIToFICstmrDrctDbt.GrpHdr;
                clonedBuilder.WithGroupHeader(
                    originalHeader.MsgId,
                    originalHeader.CreDtTm,
                    originalHeader.NbOfTxs,
                    originalHeader.CtrlSumSpecified ? originalHeader.CtrlSum : null,
                    originalHeader.TtlIntrBkSttlmAmt,
                    originalHeader.IntrBkSttlmDtSpecified ? originalHeader.IntrBkSttlmDt : null,
                    originalHeader.InstgAgt,
                    originalHeader.InstdAgt);
            }

            // Clone transactions
            foreach (var transaction in _directDebitTransactions)
            {
                clonedBuilder._directDebitTransactions.Add(transaction);
            }

            // Clone supplementary data
            foreach (var data in _supplementaryData)
            {
                clonedBuilder._supplementaryData.Add(data);
            }

            return clonedBuilder;
        }

        /// <summary>
        /// Validates the message structure before XML generation.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the message structure is invalid.</exception>
        private void ValidateMessage()
        {
            if (_document.FIToFICstmrDrctDbt.GrpHdr == null)
                throw new InvalidOperationException("Group header is required. Use WithGroupHeader() method to set it.");

            if (string.IsNullOrEmpty(_document.FIToFICstmrDrctDbt.GrpHdr.MsgId))
                throw new InvalidOperationException("Message ID in group header cannot be null or empty.");

            if (_directDebitTransactions.Count == 0)
                throw new InvalidOperationException("At least one direct debit transaction is required. Use AddDirectDebitTransaction() method to add transactions.");

            // Validate that all transactions have required fields
            for (int i = 0; i < _directDebitTransactions.Count; i++)
            {
                var transaction = _directDebitTransactions[i];
                if (transaction.PmtId == null)
                    throw new InvalidOperationException($"Payment identification is required for transaction at index {i}.");
                if (transaction.IntrBkSttlmAmt == null)
                    throw new InvalidOperationException($"Inter-bank settlement amount is required for transaction at index {i}.");
            }
        }
    }
}
