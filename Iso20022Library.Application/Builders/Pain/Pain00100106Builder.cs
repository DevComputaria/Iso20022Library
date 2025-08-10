using System;
using Iso20022Library.Domain.Common.Interfaces;
using Iso20022Library.Infrastructure.Xml;
using Iso20022Library.Messages.Payments.Pain.Generated.Pain00100106;

namespace Iso20022Library.Application.Builders.Pain
{
    /// <summary>
    /// Builder for constructing and serializing ISO 20022 pain.001.001.06 messages (Customer Credit Transfer Initiation V06).
    /// </summary>
    /// <remarks>
    /// The pain.001.001.06 message is used to initiate credit transfer instructions from a debtor to a creditor.
    /// This version 06 includes enhanced features compared to earlier versions, with improved data structures,
    /// enhanced regulatory compliance features, and additional functionality for payment processing.
    /// This builder handles both the construction of the message object and its serialization to XML format
    /// according to ISO 20022 standards.
    /// </remarks>
    public class Pain00100106Builder : IMessageBuilder
    {
        private readonly Document _document;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pain00100106Builder"/> class.
        /// Sets up the basic document structure with the customer credit transfer initiation.
        /// </summary>
        public Pain00100106Builder()
        {
            _document = new Document
            {
                CstmrCdtTrfInitn = new CustomerCreditTransferInitiationV06()
            };
        }

        /// <summary>
        /// Sets the group header information for the payment message.
        /// </summary>
        /// <param name="groupHeader">The group header containing message identification, creation date/time and other control information.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when groupHeader is null.</exception>
        public Pain00100106Builder WithGroupHeader(GroupHeader48 groupHeader)
        {
            ValidateParameter(groupHeader, nameof(groupHeader));
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
        /// <exception cref="ArgumentNullException">Thrown when required parameters are null or empty.</exception>
        public Pain00100106Builder WithGroupHeader(
            string messageId, 
            DateTime creationDateTime, 
            string numberOfTransactions, 
            PartyIdentification43 initiatingParty, 
            decimal? controlSum = null)
        {
            ValidateParameter(messageId, nameof(messageId));
            ValidateParameter(numberOfTransactions, nameof(numberOfTransactions));
            ValidateParameter(initiatingParty, nameof(initiatingParty));

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
        /// <exception cref="ArgumentNullException">Thrown when authorization is null.</exception>
        public Pain00100106Builder AddAuthorization(Authorisation1Choice authorization)
        {
            ValidateParameter(authorization, nameof(authorization));
            
            if (_document.CstmrCdtTrfInitn.GrpHdr == null)
                _document.CstmrCdtTrfInitn.GrpHdr = new GroupHeader48();

            _document.CstmrCdtTrfInitn.GrpHdr.Authstn.Add(authorization);
            return this;
        }

        /// <summary>
        /// Sets the forwarding agent in the group header.
        /// </summary>
        /// <param name="forwardingAgent">The financial institution that receives the instruction from the initiating party.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when forwardingAgent is null.</exception>
        public Pain00100106Builder WithForwardingAgent(BranchAndFinancialInstitutionIdentification5 forwardingAgent)
        {
            ValidateParameter(forwardingAgent, nameof(forwardingAgent));
            
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
        /// <exception cref="ArgumentNullException">Thrown when paymentInstruction is null.</exception>
        public Pain00100106Builder AddPaymentInstruction(PaymentInstruction16 paymentInstruction)
        {
            ValidateParameter(paymentInstruction, nameof(paymentInstruction));
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
        /// <exception cref="ArgumentNullException">Thrown when required parameters are null or empty.</exception>
        public Pain00100106Builder AddPaymentInstruction(
            string paymentInfoId,
            PaymentMethod3Code paymentMethod,
            DateTime requestedExecutionDate,
            PartyIdentification43 debtor,
            CashAccount24 debtorAccount,
            BranchAndFinancialInstitutionIdentification5 debtorAgent)
        {
            ValidateParameter(paymentInfoId, nameof(paymentInfoId));
            ValidateParameter(debtor, nameof(debtor));
            ValidateParameter(debtorAccount, nameof(debtorAccount));
            ValidateParameter(debtorAgent, nameof(debtorAgent));

            var paymentInstruction = new PaymentInstruction16
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
        /// Sets the batch booking indicator for the last payment instruction.
        /// </summary>
        /// <param name="batchBooking">Boolean indicator for batch booking.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no payment instruction exists.</exception>
        public Pain00100106Builder SetBatchBooking(bool batchBooking)
        {
            if (_document.CstmrCdtTrfInitn.PmtInf.Count == 0)
                throw new InvalidOperationException("Cannot set batch booking without a payment instruction. Add a payment instruction first.");

            var lastPaymentInstruction = _document.CstmrCdtTrfInitn.PmtInf[_document.CstmrCdtTrfInitn.PmtInf.Count - 1];
            lastPaymentInstruction.BtchBookg = batchBooking;
            lastPaymentInstruction.BtchBookgSpecified = true;

            return this;
        }

        /// <summary>
        /// Sets the charge bearer for the last payment instruction.
        /// </summary>
        /// <param name="chargeBearer">Specifies which party should bear the charges associated with the processing of the payment transaction.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no payment instruction exists.</exception>
        public Pain00100106Builder SetChargeBearer(ChargeBearerType1Code chargeBearer)
        {
            if (_document.CstmrCdtTrfInitn.PmtInf.Count == 0)
                throw new InvalidOperationException("Cannot set charge bearer without a payment instruction. Add a payment instruction first.");

            var lastPaymentInstruction = _document.CstmrCdtTrfInitn.PmtInf[_document.CstmrCdtTrfInitn.PmtInf.Count - 1];
            lastPaymentInstruction.ChrgBr = chargeBearer;
            lastPaymentInstruction.ChrgBrSpecified = true;

            return this;
        }

        /// <summary>
        /// Sets payment type information for the last payment instruction.
        /// </summary>
        /// <param name="paymentTypeInformation">Payment type information containing instruction priority, service level, local instrument, and category purpose.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no payment instruction exists.</exception>
        /// <exception cref="ArgumentNullException">Thrown when paymentTypeInformation is null.</exception>
        public Pain00100106Builder SetPaymentTypeInformation(PaymentTypeInformation19 paymentTypeInformation)
        {
            ValidateParameter(paymentTypeInformation, nameof(paymentTypeInformation));
            
            if (_document.CstmrCdtTrfInitn.PmtInf.Count == 0)
                throw new InvalidOperationException("Cannot set payment type information without a payment instruction. Add a payment instruction first.");

            var lastPaymentInstruction = _document.CstmrCdtTrfInitn.PmtInf[_document.CstmrCdtTrfInitn.PmtInf.Count - 1];
            lastPaymentInstruction.PmtTpInf = paymentTypeInformation;

            return this;
        }

        /// <summary>
        /// Adds a credit transfer transaction to the last payment instruction.
        /// </summary>
        /// <param name="creditTransfer">The credit transfer transaction to add.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no payment instruction exists.</exception>
        /// <exception cref="ArgumentNullException">Thrown when creditTransfer is null.</exception>
        public Pain00100106Builder AddCreditTransferTransaction(CreditTransferTransaction20 creditTransfer)
        {
            ValidateParameter(creditTransfer, nameof(creditTransfer));
            
            if (_document.CstmrCdtTrfInitn.PmtInf == null || _document.CstmrCdtTrfInitn.PmtInf.Count == 0)
                throw new InvalidOperationException("Cannot add a credit transfer transaction without a payment instruction. Add a payment instruction first.");

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
        /// <param name="instructionId">Optional instruction identifier.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no payment instruction exists.</exception>
        /// <exception cref="ArgumentNullException">Thrown when required parameters are null or empty.</exception>
        public Pain00100106Builder AddCreditTransferTransaction(
            string endToEndId,
            AmountType4Choice amount,
            PartyIdentification43 creditor,
            CashAccount24 creditorAccount,
            string? instructionId = null)
        {
            ValidateParameter(endToEndId, nameof(endToEndId));
            ValidateParameter(amount, nameof(amount));
            ValidateParameter(creditor, nameof(creditor));
            ValidateParameter(creditorAccount, nameof(creditorAccount));
            
            if (_document.CstmrCdtTrfInitn.PmtInf == null || _document.CstmrCdtTrfInitn.PmtInf.Count == 0)
                throw new InvalidOperationException("Cannot add a credit transfer transaction without a payment instruction. Add a payment instruction first.");

            var paymentId = new PaymentIdentification1 { EndToEndId = endToEndId };
            if (!string.IsNullOrEmpty(instructionId))
            {
                paymentId.InstrId = instructionId;
            }

            var transaction = new CreditTransferTransaction20
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
        /// </summary>
        /// <param name="creditorAgent">Financial institution servicing the creditor's account.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no payment instruction or transaction exists.</exception>
        /// <exception cref="ArgumentNullException">Thrown when creditorAgent is null.</exception>
        public Pain00100106Builder SetCreditorAgent(BranchAndFinancialInstitutionIdentification5 creditorAgent)
        {
            ValidateParameter(creditorAgent, nameof(creditorAgent));
            
            if (_document.CstmrCdtTrfInitn.PmtInf.Count == 0)
                throw new InvalidOperationException("Cannot set creditor agent without a payment instruction. Add a payment instruction first.");

            var lastPaymentInstruction = _document.CstmrCdtTrfInitn.PmtInf[_document.CstmrCdtTrfInitn.PmtInf.Count - 1];
            if (lastPaymentInstruction.CdtTrfTxInf.Count == 0)
                throw new InvalidOperationException("Cannot set creditor agent without a credit transfer transaction. Add a transaction first.");

            var lastTransaction = lastPaymentInstruction.CdtTrfTxInf[lastPaymentInstruction.CdtTrfTxInf.Count - 1];
            lastTransaction.CdtrAgt = creditorAgent;

            return this;
        }

        /// <summary>
        /// Sets remittance information for the last credit transfer transaction.
        /// </summary>
        /// <param name="remittanceInformation">Remittance information for the payment.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no payment instruction or transaction exists.</exception>
        /// <exception cref="ArgumentNullException">Thrown when remittanceInformation is null.</exception>
        public Pain00100106Builder SetRemittanceInformation(RemittanceInformation10 remittanceInformation)
        {
            ValidateParameter(remittanceInformation, nameof(remittanceInformation));
            
            if (_document.CstmrCdtTrfInitn.PmtInf.Count == 0)
                throw new InvalidOperationException("Cannot set remittance information without a payment instruction. Add a payment instruction first.");

            var lastPaymentInstruction = _document.CstmrCdtTrfInitn.PmtInf[_document.CstmrCdtTrfInitn.PmtInf.Count - 1];
            if (lastPaymentInstruction.CdtTrfTxInf.Count == 0)
                throw new InvalidOperationException("Cannot set remittance information without a credit transfer transaction. Add a transaction first.");

            var lastTransaction = lastPaymentInstruction.CdtTrfTxInf[lastPaymentInstruction.CdtTrfTxInf.Count - 1];
            lastTransaction.RmtInf = remittanceInformation;

            return this;
        }

        /// <summary>
        /// Adds an unstructured remittance information to the last credit transfer transaction.
        /// </summary>
        /// <param name="unstructuredRemittance">Unstructured remittance information.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no payment instruction or transaction exists.</exception>
        /// <exception cref="ArgumentNullException">Thrown when unstructuredRemittance is null or empty.</exception>
        public Pain00100106Builder AddUnstructuredRemittance(string unstructuredRemittance)
        {
            ValidateParameter(unstructuredRemittance, nameof(unstructuredRemittance));
            
            if (_document.CstmrCdtTrfInitn.PmtInf.Count == 0)
                throw new InvalidOperationException("Cannot add remittance information without a payment instruction. Add a payment instruction first.");

            var lastPaymentInstruction = _document.CstmrCdtTrfInitn.PmtInf[_document.CstmrCdtTrfInitn.PmtInf.Count - 1];
            if (lastPaymentInstruction.CdtTrfTxInf.Count == 0)
                throw new InvalidOperationException("Cannot add remittance information without a credit transfer transaction. Add a transaction first.");

            var lastTransaction = lastPaymentInstruction.CdtTrfTxInf[lastPaymentInstruction.CdtTrfTxInf.Count - 1];
            
            if (lastTransaction.RmtInf == null)
                lastTransaction.RmtInf = new RemittanceInformation10();

            lastTransaction.RmtInf.Ustrd.Add(unstructuredRemittance);

            return this;
        }

        /// <summary>
        /// Adds supplementary data to the message.
        /// </summary>
        /// <param name="supplementaryData">The supplementary data to add.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when supplementaryData is null.</exception>
        public Pain00100106Builder AddSupplementaryData(SupplementaryData1 supplementaryData)
        {
            ValidateParameter(supplementaryData, nameof(supplementaryData));
            _document.CstmrCdtTrfInitn.SplmtryData.Add(supplementaryData);
            return this;
        }

        /// <summary>
        /// Updates the control sum and number of transactions in the group header based on payment instructions.
        /// </summary>
        /// <returns>The current builder instance for method chaining.</returns>
        public Pain00100106Builder UpdateGroupHeaderTotals()
        {
            if (_document.CstmrCdtTrfInitn.GrpHdr == null)
                _document.CstmrCdtTrfInitn.GrpHdr = new GroupHeader48();

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
                        // Handle InstdAmt for control sum calculation
                        if (transaction.Amt?.InstdAmt != null)
                        {
                            totalAmount += transaction.Amt.InstdAmt.Value;
                        }
                        // Handle EqvtAmt if needed
                        else if (transaction.Amt?.EqvtAmt?.Amt != null)
                        {
                            totalAmount += transaction.Amt.EqvtAmt.Amt.Value;
                        }
                    }
                }
            }

            _document.CstmrCdtTrfInitn.GrpHdr.NbOfTxs = totalTransactions.ToString();
            _document.CstmrCdtTrfInitn.GrpHdr.CtrlSum = totalAmount;
            _document.CstmrCdtTrfInitn.GrpHdr.CtrlSumSpecified = totalAmount > 0;

            return this;
        }

        /// <summary>
        /// Validates that the message has all required information before building.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when required information is missing.</exception>
        private void ValidateMessage()
        {
            if (_document.CstmrCdtTrfInitn == null)
                throw new InvalidOperationException("Customer Credit Transfer Initiation is required.");

            if (_document.CstmrCdtTrfInitn.GrpHdr == null)
                throw new InvalidOperationException("Group Header is required.");

            if (string.IsNullOrEmpty(_document.CstmrCdtTrfInitn.GrpHdr.MsgId))
                throw new InvalidOperationException("Message ID is required in Group Header.");

            if (_document.CstmrCdtTrfInitn.GrpHdr.InitgPty == null)
                throw new InvalidOperationException("Initiating Party is required in Group Header.");

            if (_document.CstmrCdtTrfInitn.PmtInf == null || _document.CstmrCdtTrfInitn.PmtInf.Count == 0)
                throw new InvalidOperationException("At least one Payment Instruction is required.");

            foreach (var paymentInstruction in _document.CstmrCdtTrfInitn.PmtInf)
            {
                if (string.IsNullOrEmpty(paymentInstruction.PmtInfId))
                    throw new InvalidOperationException("Payment Information ID is required for each Payment Instruction.");

                if (paymentInstruction.Dbtr == null)
                    throw new InvalidOperationException("Debtor is required for each Payment Instruction.");

                if (paymentInstruction.DbtrAcct == null)
                    throw new InvalidOperationException("Debtor Account is required for each Payment Instruction.");

                if (paymentInstruction.DbtrAgt == null)
                    throw new InvalidOperationException("Debtor Agent is required for each Payment Instruction.");

                if (paymentInstruction.CdtTrfTxInf == null || paymentInstruction.CdtTrfTxInf.Count == 0)
                    throw new InvalidOperationException("At least one Credit Transfer Transaction is required for each Payment Instruction.");

                foreach (var transaction in paymentInstruction.CdtTrfTxInf)
                {
                    if (transaction.PmtId?.EndToEndId == null)
                        throw new InvalidOperationException("End To End ID is required for each Credit Transfer Transaction.");

                    if (transaction.Amt == null)
                        throw new InvalidOperationException("Amount is required for each Credit Transfer Transaction.");

                    if (transaction.Cdtr == null)
                        throw new InvalidOperationException("Creditor is required for each Credit Transfer Transaction.");
                }
            }
        }

        /// <summary>
        /// Builds the document object with validation.
        /// </summary>
        /// <returns>The completed document object.</returns>
        /// <exception cref="InvalidOperationException">Thrown when required information is missing.</exception>
        public Document Build()
        {
            ValidateMessage();
            return _document;
        }

        /// <summary>
        /// Builds an XML representation of the pain.001.001.06 message.
        /// </summary>
        /// <param name="message">The message object to serialize. Must be an instance of <see cref="Document"/>.</param>
        /// <returns>A string containing the XML representation of the message.</returns>
        /// <exception cref="ArgumentNullException">Thrown when message is null.</exception>
        /// <exception cref="InvalidCastException">Thrown when the provided message is not of type <see cref="Document"/>.</exception>
        public string BuildXml(object message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            if (message is not Document doc)
                throw new InvalidCastException($"Invalid message type. Expected Document, but received {message.GetType().Name}.");

            return XmlSerializationService.Serialize(doc);
        }

        /// <summary>
        /// Builds the document and returns its XML representation with validation.
        /// </summary>
        /// <returns>A string containing the XML representation of the message.</returns>
        /// <exception cref="InvalidOperationException">Thrown when required information is missing.</exception>
        public string BuildXml()
        {
            var document = Build();
            return XmlSerializationService.Serialize(document);
        }

        /// <summary>
        /// Resets the builder to its initial state, clearing all data.
        /// </summary>
        /// <returns>A new builder instance with cleared data.</returns>
        public Pain00100106Builder Reset()
        {
            return new Pain00100106Builder();
        }

        /// <summary>
        /// Creates a copy of the current builder with the same data.
        /// </summary>
        /// <returns>A new builder instance with the same data as the current builder.</returns>
        public Pain00100106Builder Clone()
        {
            var clonedBuilder = new Pain00100106Builder();
            
            if (_document.CstmrCdtTrfInitn.GrpHdr != null)
            {
                clonedBuilder._document.CstmrCdtTrfInitn.GrpHdr = _document.CstmrCdtTrfInitn.GrpHdr;
            }

            foreach (var paymentInstruction in _document.CstmrCdtTrfInitn.PmtInf)
            {
                clonedBuilder._document.CstmrCdtTrfInitn.PmtInf.Add(paymentInstruction);
            }

            foreach (var supplementaryData in _document.CstmrCdtTrfInitn.SplmtryData)
            {
                clonedBuilder._document.CstmrCdtTrfInitn.SplmtryData.Add(supplementaryData);
            }

            return clonedBuilder;
        }

        /// <summary>
        /// Gets the current number of payment instructions in the message.
        /// </summary>
        /// <returns>The count of payment instructions.</returns>
        public int GetPaymentInstructionCount()
        {
            return _document.CstmrCdtTrfInitn.PmtInf?.Count ?? 0;
        }

        /// <summary>
        /// Gets the total number of credit transfer transactions across all payment instructions.
        /// </summary>
        /// <returns>The total count of credit transfer transactions.</returns>
        public int GetTotalTransactionCount()
        {
            if (_document.CstmrCdtTrfInitn.PmtInf == null)
                return 0;

            int totalCount = 0;
            foreach (var paymentInstruction in _document.CstmrCdtTrfInitn.PmtInf)
            {
                totalCount += paymentInstruction.CdtTrfTxInf?.Count ?? 0;
            }

            return totalCount;
        }

        /// <summary>
        /// Validates that a parameter is not null.
        /// </summary>
        /// <param name="parameter">The parameter to validate.</param>
        /// <param name="parameterName">The name of the parameter for exception messages.</param>
        /// <exception cref="ArgumentNullException">Thrown when parameter is null.</exception>
        private static void ValidateParameter(object parameter, string parameterName)
        {
            if (parameter == null)
                throw new ArgumentNullException(parameterName);
        }

        /// <summary>
        /// Validates that a string parameter is not null or empty.
        /// </summary>
        /// <param name="parameter">The string parameter to validate.</param>
        /// <param name="parameterName">The name of the parameter for exception messages.</param>
        /// <exception cref="ArgumentNullException">Thrown when parameter is null or empty.</exception>
        private static void ValidateParameter(string parameter, string parameterName)
        {
            if (string.IsNullOrEmpty(parameter))
                throw new ArgumentNullException(parameterName);
        }
    }
}
