using Iso20022Library.Domain.Common.Interfaces;
using Iso20022Library.Infrastructure.Xml;
using Iso20022Library.Messages.Payments.Pain.Generated.Pain00100104;

namespace Iso20022Library.Application.Builders.Pain
{
    /// <summary>
    /// Builder for constructing and serializing ISO 20022 pain.001.001.04 messages (Customer Credit Transfer Initiation).
    /// </summary>
    /// <remarks>
    /// The pain.001.001.04 message is used to initiate credit transfer instructions from a debtor to a creditor.
    /// This builder handles both the construction of the message object and its serialization to XML format
    /// according to ISO 20022 standards.
    /// </remarks>
    public class Pain00100104Builder : IMessageBuilder
    {
        private readonly Document _document;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pain00100104Builder"/> class.
        /// </summary>
        public Pain00100104Builder()
        {
            _document = new Document
            {
                CstmrCdtTrfInitn = new CustomerCreditTransferInitiationV04()
            };
        }

        /// <summary>
        /// Sets the group header information for the payment message.
        /// </summary>
        /// <param name="groupHeader">The group header containing message identification, creation date/time and other control information.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        public Pain00100104Builder WithGroupHeader(GroupHeader48 groupHeader)
        {
            _document.CstmrCdtTrfInitn.GrpHdr = groupHeader;
            return this;
        }

        /// <summary>
        /// Creates a new group header with the specified parameters.
        /// </summary>
        /// <param name="messageId">Unique message identifier.</param>
        /// <param name="creationDateTime">Creation date and time of the message.</param>
        /// <param name="numberOfTransactions">Total number of transactions in the message.</param>
        /// <param name="initiatingParty">Party initiating the payment.</param>
        /// <param name="controlSum">Optional total sum of all transactions.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        public Pain00100104Builder WithGroupHeader(string messageId, DateTime creationDateTime, string numberOfTransactions, PartyIdentification43 initiatingParty, decimal? controlSum = null)
        {
            var groupHeader = new GroupHeader48
            {
                MsgId = messageId,
                CreDtTm = creationDateTime,
                NbOfTxs = numberOfTransactions,
                InitgPty = initiatingParty
            };

            if (controlSum.HasValue)
            {
                groupHeader.CtrlSum = controlSum.Value;
                groupHeader.CtrlSumSpecified = true;
            }

            _document.CstmrCdtTrfInitn.GrpHdr = groupHeader;
            return this;
        }

        /// <summary>
        /// Adds authorization information to the group header.
        /// </summary>
        /// <param name="authorization">The authorization information to add.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        public Pain00100104Builder AddAuthorization(Authorisation1Choice authorization)
        {
            if (_document.CstmrCdtTrfInitn.GrpHdr == null)
                _document.CstmrCdtTrfInitn.GrpHdr = new GroupHeader48();

            // Use the collection directly
            _document.CstmrCdtTrfInitn.GrpHdr.Authstn.Add(authorization);
            return this;
        }

        /// <summary>
        /// Sets the forwarding agent in the group header.
        /// </summary>
        /// <param name="forwardingAgent">The financial institution that receives the instruction from the initiating party.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        public Pain00100104Builder WithForwardingAgent(BranchAndFinancialInstitutionIdentification5 forwardingAgent)
        {
            if (_document.CstmrCdtTrfInitn.GrpHdr == null)
                _document.CstmrCdtTrfInitn.GrpHdr = new GroupHeader48();

            _document.CstmrCdtTrfInitn.GrpHdr.FwdgAgt = forwardingAgent;
            return this;
        }

        /// <summary>
        /// Adds a payment instruction to the message.
        /// </summary>
        /// <param name="paymentInstruction">The payment instruction to add.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        public Pain00100104Builder AddPaymentInstruction(PaymentInstruction6 paymentInstruction)
        {
            _document.CstmrCdtTrfInitn.PmtInf.Add(paymentInstruction);
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
        public Pain00100104Builder AddPaymentInstruction(
            string paymentInfoId,
            PaymentMethod3Code paymentMethod,
            DateTime requestedExecutionDate,
            PartyIdentification43 debtor,
            CashAccount24 debtorAccount,
            BranchAndFinancialInstitutionIdentification5 debtorAgent)
        {
            var paymentInstruction = new PaymentInstruction6
            {
                PmtInfId = paymentInfoId,
                PmtMtd = paymentMethod,
                ReqdExctnDt = requestedExecutionDate,
                Dbtr = debtor,
                DbtrAcct = debtorAccount,
                DbtrAgt = debtorAgent
                // Do not set CdtTrfTxInf here; it will be empty by default
            };

            return AddPaymentInstruction(paymentInstruction);
        }

        /// <summary>
        /// Adds a credit transfer transaction to the last payment instruction.
        /// </summary>
        /// <param name="creditTransfer">The credit transfer transaction to add.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        public Pain00100104Builder AddCreditTransferTransaction(CreditTransferTransaction1 creditTransfer)
        {
            if (_document.CstmrCdtTrfInitn.PmtInf == null || _document.CstmrCdtTrfInitn.PmtInf.Count == 0)
                throw new InvalidOperationException("Cannot add a credit transfer transaction without a payment instruction.");

            var lastPaymentInstruction = _document.CstmrCdtTrfInitn.PmtInf[_document.CstmrCdtTrfInitn.PmtInf.Count - 1];
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
        public Pain00100104Builder AddCreditTransferTransaction(
            string endToEndId,
            AmountType3Choice amount,
            PartyIdentification43 creditor,
            CashAccount24 creditorAccount)
        {
            if (_document.CstmrCdtTrfInitn.PmtInf == null || _document.CstmrCdtTrfInitn.PmtInf.Count == 0)
                throw new InvalidOperationException("Cannot add a credit transfer transaction without a payment instruction.");

            var transaction = new CreditTransferTransaction1
            {
                PmtId = new PaymentIdentification1 { EndToEndId = endToEndId },
                Amt = amount,
                Cdtr = creditor,
                CdtrAcct = creditorAccount
            };

            return AddCreditTransferTransaction(transaction);
        }

        /// <summary>
        /// Adds supplementary data to the message.
        /// </summary>
        /// <param name="supplementaryData">The supplementary data to add.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        public Pain00100104Builder AddSupplementaryData(SupplementaryData1 supplementaryData)
        {
            _document.CstmrCdtTrfInitn.SplmtryData.Add(supplementaryData);
            return this;
        }

        /// <summary>
        /// Updates the control sum and number of transactions in the group header based on payment instructions.
        /// </summary>
        /// <returns>The current builder instance for method chaining.</returns>
        public Pain00100104Builder UpdateGroupHeaderTotals()
        {
            if (_document.CstmrCdtTrfInitn.GrpHdr == null)
                _document.CstmrCdtTrfInitn.GrpHdr = new GroupHeader48();

            // Use .Count for Collection<T> instead of .Length
            if (_document.CstmrCdtTrfInitn.PmtInf == null || _document.CstmrCdtTrfInitn.PmtInf.Count == 0)
                return this;

            int totalTransactions = 0;
            decimal totalAmount = 0;

            foreach (var paymentInstruction in _document.CstmrCdtTrfInitn.PmtInf)
            {
                if (paymentInstruction.CdtTrfTxInf != null)
                {
                    totalTransactions += paymentInstruction.CdtTrfTxInf.Count;

                    foreach (var transaction in paymentInstruction.CdtTrfTxInf)
                    {
                        // The generated AmountType3Choice may have InstdAmt or EqvtAmt; prefer InstdAmt for sum
                        if (transaction.Amt?.InstdAmt != null)
                        {
                            totalAmount += transaction.Amt.InstdAmt.Value;
                        }
                        // If EqvtAmt is needed, add handling here
                    }
                }
            }

            _document.CstmrCdtTrfInitn.GrpHdr.NbOfTxs = totalTransactions.ToString();
            _document.CstmrCdtTrfInitn.GrpHdr.CtrlSum = totalAmount;
            _document.CstmrCdtTrfInitn.GrpHdr.CtrlSumSpecified = totalAmount > 0;

            return this;
        }

        /// <summary>
        /// Builds the document object.
        /// </summary>
        /// <returns>The completed document object.</returns>
        public Document Build()
        {
            // Validate the document if necessary before returning
            return _document;
        }

        /// <summary>
        /// Builds an XML representation of the pain.001.001.04 message.
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
    }
}