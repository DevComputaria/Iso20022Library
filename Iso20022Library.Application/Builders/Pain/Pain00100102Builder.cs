using Iso20022Library.Domain.Common.Interfaces;
using Iso20022Library.Infrastructure.Xml;
using Iso20022Library.Messages.Payments.Pain.Generated.Pain00100102;
using System;
using System.Collections.ObjectModel;

namespace Iso20022Library.Application.Builders.Pain
{
    /// <summary>
    /// Builder for constructing and serializing ISO 20022 pain.001.001.02 messages (Customer Credit Transfer Initiation).
    /// </summary>
    /// <remarks>
    /// The pain.001.001.02 message is used to initiate credit transfer instructions from a debtor to a creditor.
    /// This builder handles both the construction of the message object and its serialization to XML format
    /// according to ISO 20022 standards.
    /// </remarks>
    public class Pain00100102Builder : IMessageBuilder
    {
        private readonly Document _document;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pain00100102Builder"/> class.
        /// </summary>
        public Pain00100102Builder()
        {
            _document = new Document();
        }

        // Add methods for building the message as needed, similar to Pain00100104Builder
        // For brevity, only a minimal implementation is provided here

        /// <summary>
        /// Builds the document object.
        /// </summary>
        /// <returns>The completed document object.</returns>
        public Document Build()
        {
            return _document;
        }

        /// <summary>
        /// Builds an XML representation of the pain.001.001.02 message.
        /// </summary>
        /// <param name="message">The message object to serialize. Must be an instance of <see cref="Document"/>.</param>
        /// <returns>A string containing the XML representation of the message.</returns>
        /// <exception cref="InvalidCastException">Thrown when the provided message is not of type <see cref="Document"/>.</exception>
        public string BuildXml(object message)
        {
            if (message is not Document doc)
                throw new InvalidCastException("Invalid message type.");
            return XmlSerializationService.Serialize(doc);
        }

        /// <summary>
        /// Builds the document and returns its XML representation.
        /// </summary>
        /// <returns>A string containing the XML representation of the message.</returns>
        public string BuildXml()
        {
            return XmlSerializationService.Serialize(_document);
        }

        /// <summary>
        /// Sets the group header information for the payment message.
        /// </summary>
        /// <param name="groupHeader">The group header containing message identification, creation date/time and other control information.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        public Pain00100102Builder WithGroupHeader(GroupHeader1 groupHeader)
        {
            if (_document.Pain00100102 == null)
                _document.Pain00100102 = new Pain00100102();
            _document.Pain00100102.GrpHdr = groupHeader;
            return this;
        }

        /// <summary>
        /// Creates a new group header with the specified parameters.
        /// </summary>
        /// <param name="messageId">Unique message identifier.</param>
        /// <param name="creationDateTime">Creation date and time of the message.</param>
        /// <param name="initiatingParty">Party initiating the payment.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        public Pain00100102Builder WithGroupHeader(string messageId, DateTime creationDateTime, PartyIdentification8 initiatingParty)
        {
            if (_document.Pain00100102 == null)
                _document.Pain00100102 = new Pain00100102();
            var groupHeader = new GroupHeader1
            {
                MsgId = messageId,
                CreDtTm = creationDateTime,
                InitgPty = initiatingParty
            };
            _document.Pain00100102.GrpHdr = groupHeader;
            return this;
        }

        /// <summary>
        /// Adds a payment instruction to the message.
        /// </summary>
        /// <param name="paymentInstruction">The payment instruction to add.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        public Pain00100102Builder AddPaymentInstruction(PaymentInstructionInformation1 paymentInstruction)
        {
            if (_document.Pain00100102 == null)
                _document.Pain00100102 = new Pain00100102();
            _document.Pain00100102.PmtInf.Add(paymentInstruction);
            return this;
        }

        /// <summary>
        /// Creates and adds a new payment instruction with the specified parameters.
        /// </summary>
        /// <param name="paymentInfoId">Unique identification for the payment information.</param>
        /// <param name="paymentMethod">Method of payment (e.g., transfer, cheque).</param>
        /// <param name="requestedExecutionDate">Date on which the payment should be executed.</param>
        /// <param name="debtor">Party making the payment.</param>
        /// <param name="debtorAccount">Account from which the payment will be made.</param>
        /// <param name="debtorAgent">Financial institution servicing the debtor's account.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        public Pain00100102Builder AddPaymentInstruction(
            string paymentInfoId,
            PaymentMethod3Code paymentMethod,
            DateTime requestedExecutionDate,
            PartyIdentification8 debtor,
            CashAccount7 debtorAccount,
            BranchAndFinancialInstitutionIdentification3 debtorAgent)
        {
            var paymentInstruction = new PaymentInstructionInformation1
            {
                PmtInfId = paymentInfoId,
                PmtMtd = paymentMethod,
                ReqdExctnDt = requestedExecutionDate,
                Dbtr = debtor,
                DbtrAcct = debtorAccount,
                DbtrAgt = debtorAgent
            };
            return AddPaymentInstruction(paymentInstruction);
        }

        /// <summary>
        /// Adds a credit transfer transaction to the last payment instruction.
        /// </summary>
        /// <param name="creditTransfer">The credit transfer transaction to add.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        public Pain00100102Builder AddCreditTransferTransaction(CreditTransferTransactionInformation1 creditTransfer)
        {
            if (_document.Pain00100102 == null || _document.Pain00100102.PmtInf.Count == 0)
                throw new InvalidOperationException("Cannot add a credit transfer transaction without a payment instruction.");
            var lastPaymentInstruction = _document.Pain00100102.PmtInf[_document.Pain00100102.PmtInf.Count - 1];
            lastPaymentInstruction.CdtTrfTxInf.Add(creditTransfer);
            return this;
        }

        /// <summary>
        /// Creates and adds a credit transfer transaction to the last payment instruction.
        /// </summary>
        /// <param name="endToEndId">Unique identifier for the transaction.</param>
        /// <param name="amount">Amount of the transfer.</param>
        /// <param name="creditor">Party receiving the payment.</param>
        /// <param name="creditorAccount">Account to be credited.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        public Pain00100102Builder AddCreditTransferTransaction(
            string endToEndId,
            AmountType2Choice amount,
            PartyIdentification8 creditor,
            CashAccount7 creditorAccount)
        {
            if (_document.Pain00100102 == null || _document.Pain00100102.PmtInf.Count == 0)
                throw new InvalidOperationException("Cannot add a credit transfer transaction without a payment instruction.");
            var transaction = new CreditTransferTransactionInformation1
            {
                PmtId = new PaymentIdentification1 { EndToEndId = endToEndId },
                Amt = amount,
                Cdtr = creditor,
                CdtrAcct = creditorAccount
            };
            return AddCreditTransferTransaction(transaction);
        }

        /// <summary>
        /// Updates the control sum and number of transactions in the group header based on payment instructions.
        /// </summary>
        /// <returns>The current builder instance for method chaining.</returns>
        public Pain00100102Builder UpdateGroupHeaderTotals()
        {
            if (_document.Pain00100102 == null)
                _document.Pain00100102 = new Pain00100102();
            if (_document.Pain00100102.GrpHdr == null)
                _document.Pain00100102.GrpHdr = new GroupHeader1();
            if (_document.Pain00100102.PmtInf == null || _document.Pain00100102.PmtInf.Count == 0)
                return this;
            int totalTransactions = 0;
            decimal totalAmount = 0;
            foreach (var paymentInstruction in _document.Pain00100102.PmtInf)
            {
                if (paymentInstruction.CdtTrfTxInf != null)
                {
                    totalTransactions += paymentInstruction.CdtTrfTxInf.Count;
                    foreach (var transaction in paymentInstruction.CdtTrfTxInf)
                    {
                        if (transaction.Amt?.InstdAmt != null)
                        {
                            totalAmount += transaction.Amt.InstdAmt.Value;
                        }
                    }
                }
            }
            _document.Pain00100102.GrpHdr.NbOfTxs = totalTransactions.ToString();
            _document.Pain00100102.GrpHdr.CtrlSum = totalAmount;
            _document.Pain00100102.GrpHdr.CtrlSumSpecified = totalAmount > 0;
            return this;
        }
    }
}
