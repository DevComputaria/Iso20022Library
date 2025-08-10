using System;
using System.IO;
using Iso20022Library.Domain.Common.Interfaces;
using Iso20022Library.Infrastructure.Xml;
using Iso20022Library.Messages.Payments.Pain.Generated.Pain00100103;

namespace Iso20022Library.Application.Builders.Pain
{
    /// <summary>
    /// Builder for constructing and serializing ISO 20022 pain.001.001.03 messages (Customer Credit Transfer Initiation V03).
    /// </summary>
    /// <remarks>
    /// The pain.001.001.03 message is used to initiate credit transfer instructions from a debtor to a creditor.
    /// This version includes enhanced features compared to earlier versions, with improved data structures
    /// and additional functionality for payment processing. This builder handles both the construction 
    /// of the message object and its serialization to XML format according to ISO 20022 standards.
    /// </remarks>
    public class Pain00100103Builder : IMessageBuilder
    {
        private readonly Document _document;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pain00100103Builder"/> class.
        /// Sets up the basic document structure with the customer credit transfer initiation.
        /// </summary>
        public Pain00100103Builder()
        {
            _document = new Document
            {
                CstmrCdtTrfInitn = new CustomerCreditTransferInitiationV03()
            };
        }

        /// <summary>
        /// Sets the group header information for the payment message.
        /// The group header contains control and identification information for the entire message.
        /// </summary>
        /// <param name="groupHeader">The group header containing message identification, creation date/time and other control information.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        public Pain00100103Builder WithGroupHeader(GroupHeader32 groupHeader)
        {
            _document.CstmrCdtTrfInitn.GrpHdr = groupHeader;
            return this;
        }

        /// <summary>
        /// Creates a new group header with the specified parameters.
        /// </summary>
        /// <param name="messageId">Unique message identifier used to distinguish this message from others.</param>
        /// <param name="creationDateTime">Creation date and time of the message.</param>
        /// <param name="numberOfTransactions">Total number of transactions in the message.</param>
        /// <param name="initiatingParty">Party initiating the payment instruction.</param>
        /// <param name="controlSum">Optional total sum of all transaction amounts for validation purposes.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        public Pain00100103Builder WithGroupHeader(
            string messageId,
            DateTime creationDateTime,
            string numberOfTransactions,
            PartyIdentification32 initiatingParty,
            decimal? controlSum = null)
        {
            ValidateParameter(messageId, nameof(messageId));
            ValidateParameter(numberOfTransactions, nameof(numberOfTransactions));
            ValidateParameter(initiatingParty, nameof(initiatingParty));

            var groupHeader = new GroupHeader32
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
        /// Authorization information specifies the authorization required for the message.
        /// </summary>
        /// <param name="authorization">The authorization information to add.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        public Pain00100103Builder AddAuthorization(Authorisation1Choice authorization)
        {
            ValidateParameter(authorization, nameof(authorization));

            if (_document.CstmrCdtTrfInitn.GrpHdr == null)
                _document.CstmrCdtTrfInitn.GrpHdr = new GroupHeader32();

            _document.CstmrCdtTrfInitn.GrpHdr.Authstn.Add(authorization);
            return this;
        }

        /// <summary>
        /// Sets the forwarding agent in the group header.
        /// The forwarding agent is the financial institution that receives the instruction from the initiating party.
        /// </summary>
        /// <param name="forwardingAgent">The financial institution that forwards the payment instruction.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        public Pain00100103Builder WithForwardingAgent(BranchAndFinancialInstitutionIdentification4 forwardingAgent)
        {
            ValidateParameter(forwardingAgent, nameof(forwardingAgent));

            if (_document.CstmrCdtTrfInitn.GrpHdr == null)
                _document.CstmrCdtTrfInitn.GrpHdr = new GroupHeader32();

            _document.CstmrCdtTrfInitn.GrpHdr.FwdgAgt = forwardingAgent;
            return this;
        }

        /// <summary>
        /// Adds a payment instruction to the message.
        /// A payment instruction contains details about the payment method, execution date, and parties involved.
        /// </summary>
        /// <param name="paymentInstruction">The payment instruction to add.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        public Pain00100103Builder AddPaymentInstruction(PaymentInstruction6 paymentInstruction)
        {
            ValidateParameter(paymentInstruction, nameof(paymentInstruction));

            _document.CstmrCdtTrfInitn.PmtInf.Add(paymentInstruction);
            return this;
        }

        /// <summary>
        /// Creates and adds a new payment instruction with the specified parameters.
        /// </summary>
        /// <param name="paymentInfoId">Unique identification for the payment information block.</param>
        /// <param name="paymentMethod">Method of payment (e.g., transfer, cheque).</param>
        /// <param name="requestedExecutionDate">Date on which the payment should be executed.</param>
        /// <param name="debtor">Party making the payment.</param>
        /// <param name="debtorAccount">Account from which the payment will be made.</param>
        /// <param name="debtorAgent">Financial institution servicing the debtor's account.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        public Pain00100103Builder AddPaymentInstruction(
            string paymentInfoId,
            PaymentMethod3Code paymentMethod,
            DateTime requestedExecutionDate,
            PartyIdentification32 debtor,
            CashAccount16 debtorAccount,
            BranchAndFinancialInstitutionIdentification4 debtorAgent)
        {
            ValidateParameter(paymentInfoId, nameof(paymentInfoId));
            ValidateParameter(debtor, nameof(debtor));
            ValidateParameter(debtorAccount, nameof(debtorAccount));
            ValidateParameter(debtorAgent, nameof(debtorAgent));

            var paymentInstruction = new PaymentInstruction6
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
        /// Sets payment type information for the last payment instruction.
        /// This provides additional details about the nature and purpose of the payment.
        /// </summary>
        /// <param name="paymentTypeInformation">Payment type information to set.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        public Pain00100103Builder SetPaymentTypeInformation(PaymentTypeInformation19 paymentTypeInformation)
        {
            ValidateParameter(paymentTypeInformation, nameof(paymentTypeInformation));

            if (_document.CstmrCdtTrfInitn.PmtInf.Count == 0)
                throw new InvalidOperationException("Cannot set payment type information without a payment instruction. Add a payment instruction first.");

            var lastPaymentInstruction = _document.CstmrCdtTrfInitn.PmtInf[_document.CstmrCdtTrfInitn.PmtInf.Count - 1];
            lastPaymentInstruction.PmtTpInf = paymentTypeInformation;

            return this;
        }

        /// <summary>
        /// Sets the charge bearer type for the last payment instruction.
        /// This specifies who bears the charges for the payment transaction.
        /// </summary>
        /// <param name="chargeBearer">The charge bearer type (e.g., debtor, creditor, shared).</param>
        /// <returns>The current builder instance for method chaining.</returns>
        public Pain00100103Builder SetChargeBearer(ChargeBearerType1Code chargeBearer)
        {
            if (_document.CstmrCdtTrfInitn.PmtInf.Count == 0)
                throw new InvalidOperationException("Cannot set charge bearer without a payment instruction. Add a payment instruction first.");

            var lastPaymentInstruction = _document.CstmrCdtTrfInitn.PmtInf[_document.CstmrCdtTrfInitn.PmtInf.Count - 1];
            lastPaymentInstruction.ChrgBr = chargeBearer;
            lastPaymentInstruction.ChrgBrSpecified = true;

            return this;
        }

        /// <summary>
        /// Sets the debtor agent account for the last payment instruction.
        /// This is the account held by the debtor agent that will be used for the payment.
        /// </summary>
        /// <param name="debtorAgentAccount">The debtor agent's account information.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        public Pain00100103Builder SetDebtorAgentAccount(CashAccount16 debtorAgentAccount)
        {
            ValidateParameter(debtorAgentAccount, nameof(debtorAgentAccount));

            if (_document.CstmrCdtTrfInitn.PmtInf.Count == 0)
                throw new InvalidOperationException("Cannot set debtor agent account without a payment instruction. Add a payment instruction first.");

            var lastPaymentInstruction = _document.CstmrCdtTrfInitn.PmtInf[_document.CstmrCdtTrfInitn.PmtInf.Count - 1];
            lastPaymentInstruction.DbtrAgtAcct = debtorAgentAccount;

            return this;
        }

        /// <summary>
        /// Sets batch booking preference for the last payment instruction.
        /// Batch booking indicates whether individual transactions should be booked separately or as a batch.
        /// </summary>
        /// <param name="batchBooking">True for batch booking, false for individual transaction booking.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        public Pain00100103Builder SetBatchBooking(bool batchBooking)
        {
            if (_document.CstmrCdtTrfInitn.PmtInf.Count == 0)
                throw new InvalidOperationException("Cannot set batch booking without a payment instruction. Add a payment instruction first.");

            var lastPaymentInstruction = _document.CstmrCdtTrfInitn.PmtInf[_document.CstmrCdtTrfInitn.PmtInf.Count - 1];
            lastPaymentInstruction.BtchBookg = batchBooking;
            lastPaymentInstruction.BtchBookgSpecified = true;

            return this;
        }

        /// <summary>
        /// Adds a credit transfer transaction to the last payment instruction.
        /// Each credit transfer transaction represents an individual payment within the instruction.
        /// </summary>
        /// <param name="creditTransfer">The credit transfer transaction to add.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        public Pain00100103Builder AddCreditTransferTransaction(CreditTransferTransactionInformation10 creditTransfer)
        {
            ValidateParameter(creditTransfer, nameof(creditTransfer));

            if (_document.CstmrCdtTrfInitn.PmtInf.Count == 0)
                throw new InvalidOperationException("Cannot add a credit transfer transaction without a payment instruction. Add a payment instruction first.");

            var lastPaymentInstruction = _document.CstmrCdtTrfInitn.PmtInf[_document.CstmrCdtTrfInitn.PmtInf.Count - 1];
            lastPaymentInstruction.CdtTrfTxInf.Add(creditTransfer);
            return this;
        }

        /// <summary>
        /// Creates and adds a credit transfer transaction to the last payment instruction.
        /// This is a convenience method for creating simple credit transfer transactions.
        /// </summary>
        /// <param name="endToEndId">Unique identifier for the transaction that flows through the entire payment chain.</param>
        /// <param name="amount">Amount of the transfer including currency information.</param>
        /// <param name="creditor">Party receiving the payment.</param>
        /// <param name="creditorAccount">Account to be credited with the payment.</param>
        /// <param name="instructionId">Optional unique instruction identification.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        public Pain00100103Builder AddCreditTransferTransaction(
            string endToEndId,
            AmountType3Choice amount,
            PartyIdentification32 creditor,
            CashAccount16 creditorAccount,
            string? instructionId = null)
        {
            ValidateParameter(endToEndId, nameof(endToEndId));
            ValidateParameter(amount, nameof(amount));
            ValidateParameter(creditor, nameof(creditor));
            ValidateParameter(creditorAccount, nameof(creditorAccount));

            var paymentId = new PaymentIdentification1 { EndToEndId = endToEndId };
            if (!string.IsNullOrEmpty(instructionId))
            {
                paymentId.InstrId = instructionId;
            }

            var transaction = new CreditTransferTransactionInformation10
            {
                PmtId = paymentId,
                Amt = amount,
                Cdtr = creditor,
                CdtrAcct = creditorAccount
            };

            return AddCreditTransferTransaction(transaction);
        }

        /// <summary>
        /// Sets the creditor agent for the last credit transfer transaction.
        /// The creditor agent is the financial institution servicing the creditor's account.
        /// </summary>
        /// <param name="creditorAgent">The financial institution information.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        public Pain00100103Builder SetCreditorAgent(BranchAndFinancialInstitutionIdentification4 creditorAgent)
        {
            ValidateParameter(creditorAgent, nameof(creditorAgent));

            var lastTransaction = GetLastCreditTransferTransaction();
            lastTransaction.CdtrAgt = creditorAgent;

            return this;
        }

        /// <summary>
        /// Sets the creditor agent account for the last credit transfer transaction.
        /// This is the account held by the creditor agent.
        /// </summary>
        /// <param name="creditorAgentAccount">The creditor agent's account information.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        public Pain00100103Builder SetCreditorAgentAccount(CashAccount16 creditorAgentAccount)
        {
            ValidateParameter(creditorAgentAccount, nameof(creditorAgentAccount));

            var lastTransaction = GetLastCreditTransferTransaction();
            lastTransaction.CdtrAgtAcct = creditorAgentAccount;

            return this;
        }

        /// <summary>
        /// Sets remittance information for the last credit transfer transaction.
        /// Remittance information provides details about the purpose of the payment.
        /// </summary>
        /// <param name="remittanceInformation">The remittance information to set.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        public Pain00100103Builder SetRemittanceInformation(RemittanceInformation5 remittanceInformation)
        {
            ValidateParameter(remittanceInformation, nameof(remittanceInformation));

            var lastTransaction = GetLastCreditTransferTransaction();
            lastTransaction.RmtInf = remittanceInformation;

            return this;
        }

        /// <summary>
        /// Updates the control sum and number of transactions in the group header based on payment instructions.
        /// This method automatically calculates totals from all payment instructions and their transactions.
        /// </summary>
        /// <returns>The current builder instance for method chaining.</returns>
        public Pain00100103Builder UpdateGroupHeaderTotals()
        {
            if (_document.CstmrCdtTrfInitn.GrpHdr == null)
                _document.CstmrCdtTrfInitn.GrpHdr = new GroupHeader32();

            if (_document.CstmrCdtTrfInitn.PmtInf.Count == 0)
                return this;

            int totalTransactions = 0;
            decimal totalAmount = 0;

            foreach (var paymentInstruction in _document.CstmrCdtTrfInitn.PmtInf)
            {
                if (paymentInstruction.CdtTrfTxInf != null && paymentInstruction.CdtTrfTxInf.Count > 0)
                {
                    totalTransactions += paymentInstruction.CdtTrfTxInf.Count;

                    foreach (var transaction in paymentInstruction.CdtTrfTxInf)
                    {
                        // Extract amount from the transaction
                        if (transaction.Amt?.InstdAmt != null)
                        {
                            totalAmount += transaction.Amt.InstdAmt.Value;
                        }
                        else if (transaction.Amt?.EqvtAmt?.Amt != null)
                        {
                            totalAmount += transaction.Amt.EqvtAmt.Amt.Value;
                        }
                    }
                }

                // Update payment instruction totals
                if (paymentInstruction.CdtTrfTxInf != null && paymentInstruction.CdtTrfTxInf.Count > 0)
                {
                    paymentInstruction.NbOfTxs = paymentInstruction.CdtTrfTxInf.Count.ToString();

                    // Calculate control sum for this payment instruction
                    decimal instructionSum = 0;
                    foreach (var transaction in paymentInstruction.CdtTrfTxInf)
                    {
                        if (transaction.Amt?.InstdAmt != null)
                        {
                            instructionSum += transaction.Amt.InstdAmt.Value;
                        }
                        else if (transaction.Amt?.EqvtAmt?.Amt != null)
                        {
                            instructionSum += transaction.Amt.EqvtAmt.Amt.Value;
                        }
                    }

                    if (instructionSum > 0)
                    {
                        paymentInstruction.CtrlSum = instructionSum;
                        paymentInstruction.CtrlSumSpecified = true;
                    }
                }
            }

            _document.CstmrCdtTrfInitn.GrpHdr.NbOfTxs = totalTransactions.ToString();
            if (totalAmount > 0)
            {
                _document.CstmrCdtTrfInitn.GrpHdr.CtrlSum = totalAmount;
                _document.CstmrCdtTrfInitn.GrpHdr.CtrlSumSpecified = true;
            }

            return this;
        }

        /// <summary>
        /// Builds the complete document object.
        /// This method performs final validation and returns the constructed document.
        /// </summary>
        /// <returns>The completed document object ready for serialization.</returns>
        public Document Build()
        {
            ValidateDocument();
            return _document;
        }

        /// <summary>
        /// Builds the document and returns its XML representation.
        /// This is the primary method for generating the final XML output.
        /// </summary>
        /// <returns>A string containing the XML representation of the Pain.001.001.03 message.</returns>
        public string BuildXml()
        {
            var document = Build();
            return XmlSerializationService.Serialize(document);
        }

        /// <summary>
        /// Builds an XML representation of the provided message object.
        /// This method implements the IMessageBuilder interface for generic message building.
        /// </summary>
        /// <param name="message">The message object to serialize. Must be an instance of <see cref="Document"/>.</param>
        /// <returns>A string containing the XML representation of the message.</returns>
        /// <exception cref="InvalidCastException">Thrown when the provided message is not of type <see cref="Document"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the message parameter is null.</exception>
        public string BuildXml(object message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            if (message is not Document doc)
                throw new InvalidCastException($"Invalid message type. Expected Document, but received {message.GetType().Name}.");

            return XmlSerializationService.Serialize(doc);
        }

        /// <summary>
        /// Saves the message as an XML file at the specified path.
        /// This is a convenience method for writing the XML output directly to a file.
        /// </summary>
        /// <param name="filePath">The file path where the XML should be saved.</param>
        /// <exception cref="ArgumentException">Thrown when the file path is null or empty.</exception>
        public void SaveToFile(string filePath)
        {
            ValidateParameter(filePath, nameof(filePath));

            var xml = BuildXml();
            File.WriteAllText(filePath, xml);
        }

        /// <summary>
        /// Gets the last credit transfer transaction from the last payment instruction.
        /// This is a helper method used internally for setting transaction-level properties.
        /// </summary>
        /// <returns>The last credit transfer transaction.</returns>
        /// <exception cref="InvalidOperationException">Thrown when there are no payment instructions or transactions.</exception>
        private CreditTransferTransactionInformation10 GetLastCreditTransferTransaction()
        {
            if (_document.CstmrCdtTrfInitn.PmtInf.Count == 0)
                throw new InvalidOperationException("Cannot access credit transfer transaction without a payment instruction. Add a payment instruction first.");

            var lastPaymentInstruction = _document.CstmrCdtTrfInitn.PmtInf[_document.CstmrCdtTrfInitn.PmtInf.Count - 1];

            if (lastPaymentInstruction.CdtTrfTxInf.Count == 0)
                throw new InvalidOperationException("Cannot access credit transfer transaction. Add a credit transfer transaction first.");

            return lastPaymentInstruction.CdtTrfTxInf[lastPaymentInstruction.CdtTrfTxInf.Count - 1];
        }

        /// <summary>
        /// Validates the document structure and required fields before building.
        /// This method ensures that all mandatory elements are present and properly configured.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when required elements are missing or invalid.</exception>
        private void ValidateDocument()
        {
            if (_document.CstmrCdtTrfInitn == null)
                throw new InvalidOperationException("Customer credit transfer initiation is required.");

            if (_document.CstmrCdtTrfInitn.GrpHdr == null)
                throw new InvalidOperationException("Group header is required. Use WithGroupHeader method to set it.");

            if (string.IsNullOrEmpty(_document.CstmrCdtTrfInitn.GrpHdr.MsgId))
                throw new InvalidOperationException("Message ID is required in the group header.");

            if (_document.CstmrCdtTrfInitn.GrpHdr.InitgPty == null)
                throw new InvalidOperationException("Initiating party is required in the group header.");

            if (_document.CstmrCdtTrfInitn.PmtInf.Count == 0)
                throw new InvalidOperationException("At least one payment instruction is required. Use AddPaymentInstruction method.");

            // Validate each payment instruction
            foreach (var paymentInstruction in _document.CstmrCdtTrfInitn.PmtInf)
            {
                if (string.IsNullOrEmpty(paymentInstruction.PmtInfId))
                    throw new InvalidOperationException("Payment information ID is required for all payment instructions.");

                if (paymentInstruction.Dbtr == null)
                    throw new InvalidOperationException("Debtor is required for all payment instructions.");

                if (paymentInstruction.DbtrAcct == null)
                    throw new InvalidOperationException("Debtor account is required for all payment instructions.");

                if (paymentInstruction.DbtrAgt == null)
                    throw new InvalidOperationException("Debtor agent is required for all payment instructions.");

                if (paymentInstruction.CdtTrfTxInf.Count == 0)
                    throw new InvalidOperationException("At least one credit transfer transaction is required for each payment instruction.");

                // Validate each credit transfer transaction
                foreach (var transaction in paymentInstruction.CdtTrfTxInf)
                {
                    if (transaction.PmtId?.EndToEndId == null)
                        throw new InvalidOperationException("End-to-end ID is required for all credit transfer transactions.");

                    if (transaction.Amt == null)
                        throw new InvalidOperationException("Amount is required for all credit transfer transactions.");

                    if (transaction.Cdtr == null)
                        throw new InvalidOperationException("Creditor is required for all credit transfer transactions.");

                    if (transaction.CdtrAcct == null)
                        throw new InvalidOperationException("Creditor account is required for all credit transfer transactions.");
                }
            }
        }

        /// <summary>
        /// Validates that a parameter is not null or empty.
        /// This is a helper method for consistent parameter validation throughout the builder.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="parameterName">The name of the parameter for error reporting.</param>
        /// <exception cref="ArgumentNullException">Thrown when the value is null.</exception>
        /// <exception cref="ArgumentException">Thrown when a string value is empty.</exception>
        private static void ValidateParameter(object value, string parameterName)
        {
            if (value == null)
                throw new ArgumentNullException(parameterName);

            if (value is string stringValue && string.IsNullOrEmpty(stringValue))
                throw new ArgumentException($"Parameter '{parameterName}' cannot be null or empty.", parameterName);
        }
    }
}
