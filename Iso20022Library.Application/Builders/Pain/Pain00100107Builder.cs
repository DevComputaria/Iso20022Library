using System;
using System.Collections.ObjectModel;
using Iso20022Library.Domain.Common.Interfaces;
using Iso20022Library.Infrastructure.Xml;
using Iso20022Library.Messages.Payments.Pain.Generated.Pain00100107;

namespace Iso20022Library.Application.Builders.Pain
{
    /// <summary>
    /// Builder for constructing and serializing ISO 20022 pain.001.001.07 messages (Customer Credit Transfer Initiation V07).
    /// </summary>
    /// <remarks>
    /// The pain.001.001.07 message is used to initiate credit transfer instructions from a debtor to a creditor.
    /// This builder handles both the construction of the message object and its serialization to XML format
    /// according to ISO 20022 standards.
    /// </remarks>
    public class Pain00100107Builder : IMessageBuilder
    {
        private readonly Document _document;

        /// <summary>
        /// Initializes a new instance of the Pain00100107Builder class.
        /// </summary>
        public Pain00100107Builder()
        {
            _document = new Document
            {
                CstmrCdtTrfInitn = new CustomerCreditTransferInitiationV07()
            };
        }

        /// <summary>
        /// Sets the group header information for the message.
        /// The group header contains information that applies to the entire message.
        /// </summary>
        /// <param name="messageId">Unique identifier for the message.</param>
        /// <param name="creationDateTime">Date and time when the message was created.</param>
        /// <param name="numberOfTransactions">Total number of transactions in the message.</param>
        /// <param name="controlSum">Total amount of all transactions in the message.</param>
        /// <param name="initiatingParty">Party that initiates the payment.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when required parameters are null or empty.</exception>
        public Pain00100107Builder SetGroupHeader(
            string messageId,
            DateTime creationDateTime,
            string numberOfTransactions,
            decimal? controlSum = null,
            PartyIdentification43? initiatingParty = null)
        {
            ValidateParameter(messageId, nameof(messageId));
            ValidateParameter(numberOfTransactions, nameof(numberOfTransactions));

            _document.CstmrCdtTrfInitn.GrpHdr = new GroupHeader48
            {
                MsgId = messageId,
                CreDtTm = creationDateTime,
                NbOfTxs = numberOfTransactions,
                InitgPty = initiatingParty
            };

            if (controlSum.HasValue)
            {
                _document.CstmrCdtTrfInitn.GrpHdr.CtrlSum = controlSum.Value;
                _document.CstmrCdtTrfInitn.GrpHdr.CtrlSumSpecified = true;
            }

            return this;
        }

        /// <summary>
        /// Adds a payment instruction to the message.
        /// Each payment instruction contains common information for a group of transactions.
        /// </summary>
        /// <param name="paymentInformationId">Unique identifier for the payment instruction.</param>
        /// <param name="paymentMethod">Payment method used for the instruction.</param>
        /// <param name="batchBooking">Indicates whether individual transactions should be grouped for booking.</param>
        /// <param name="numberOfTransactions">Number of transactions in this instruction.</param>
        /// <param name="controlSum">Total amount of all transactions in this instruction.</param>
        /// <param name="paymentTypeInformation">Payment type information.</param>
        /// <param name="requestedExecutionDate">Date when the payment should be executed.</param>
        /// <param name="debtor">Party that owes money to the creditor.</param>
        /// <param name="debtorAccount">Account to be debited.</param>
        /// <param name="debtorAgent">Financial institution servicing the debtor account.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when required parameters are null or empty.</exception>
        public Pain00100107Builder AddPaymentInstruction(
            string paymentInformationId,
            PaymentMethod3Code paymentMethod,
            bool? batchBooking,
            string numberOfTransactions,
            decimal? controlSum,
            PaymentTypeInformation19? paymentTypeInformation,
            DateTime requestedExecutionDate,
            PartyIdentification43 debtor,
            CashAccount24 debtorAccount,
            BranchAndFinancialInstitutionIdentification5 debtorAgent)
        {
            ValidateParameter(paymentInformationId, nameof(paymentInformationId));
            ValidateParameter(numberOfTransactions, nameof(numberOfTransactions));
            ValidateParameter(debtor, nameof(debtor));
            ValidateParameter(debtorAccount, nameof(debtorAccount));
            ValidateParameter(debtorAgent, nameof(debtorAgent));

            var paymentInstruction = new PaymentInstruction20
            {
                PmtInfId = paymentInformationId,
                PmtMtd = paymentMethod,
                NbOfTxs = numberOfTransactions,
                PmtTpInf = paymentTypeInformation,
                ReqdExctnDt = requestedExecutionDate,
                Dbtr = debtor,
                DbtrAcct = debtorAccount,
                DbtrAgt = debtorAgent
            };

            if (batchBooking.HasValue)
            {
                paymentInstruction.BtchBookg = batchBooking.Value;
                paymentInstruction.BtchBookgSpecified = true;
            }

            if (controlSum.HasValue)
            {
                paymentInstruction.CtrlSum = controlSum.Value;
                paymentInstruction.CtrlSumSpecified = true;
            }

            _document.CstmrCdtTrfInitn.PmtInf.Add(paymentInstruction);
            return this;
        }

        /// <summary>
        /// Adds a credit transfer transaction to the last payment instruction.
        /// Each credit transfer transaction represents an individual payment within the instruction.
        /// </summary>
        /// <param name="creditTransfer">The credit transfer transaction to add.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no payment instruction exists.</exception>
        /// <exception cref="ArgumentNullException">Thrown when creditTransfer is null.</exception>
        public Pain00100107Builder AddCreditTransferTransaction(CreditTransferTransaction26 creditTransfer)
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
        /// This is a convenience method for creating simple credit transfer transactions.
        /// </summary>
        /// <param name="endToEndId">Unique identifier for the transaction that flows through the entire payment chain.</param>
        /// <param name="amount">Amount of the transfer including currency information.</param>
        /// <param name="creditor">Party receiving the payment.</param>
        /// <param name="creditorAccount">Account to be credited.</param>
        /// <param name="instructionId">Optional instruction identifier.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when required parameters are null or empty.</exception>
        /// <exception cref="InvalidOperationException">Thrown when no payment instruction exists.</exception>
        public Pain00100107Builder AddCreditTransferTransaction(
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

            var transaction = new CreditTransferTransaction26
            {
                PmtId = paymentId,
                Amt = amount,
                Cdtr = creditor,
                CdtrAcct = creditorAccount
            };

            return AddCreditTransferTransaction(transaction);
        }

        /// <summary>
        /// Sets the creditor agent (receiving financial institution) for the last credit transfer transaction.
        /// </summary>
        /// <param name="creditorAgent">Financial institution servicing the creditor account.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when creditorAgent is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when no transactions exist.</exception>
        public Pain00100107Builder SetCreditorAgent(BranchAndFinancialInstitutionIdentification5 creditorAgent)
        {
            ValidateParameter(creditorAgent, nameof(creditorAgent));

            var lastTransaction = GetLastTransaction();
            lastTransaction.CdtrAgt = creditorAgent;
            return this;
        }

        /// <summary>
        /// Sets the remittance information for the last credit transfer transaction.
        /// </summary>
        /// <param name="remittanceInformation">Information about the remittance, such as invoice details.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when remittanceInformation is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when no transactions exist.</exception>
        public Pain00100107Builder SetRemittanceInformation(RemittanceInformation11 remittanceInformation)
        {
            ValidateParameter(remittanceInformation, nameof(remittanceInformation));

            var lastTransaction = GetLastTransaction();
            lastTransaction.RmtInf = remittanceInformation;
            return this;
        }

        /// <summary>
        /// Sets the purpose of the last credit transfer transaction.
        /// </summary>
        /// <param name="purpose">Purpose of the payment.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when purpose is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when no transactions exist.</exception>
        public Pain00100107Builder SetPurpose(Purpose2Choice purpose)
        {
            ValidateParameter(purpose, nameof(purpose));

            var lastTransaction = GetLastTransaction();
            lastTransaction.Purp = purpose;
            return this;
        }

        /// <summary>
        /// Adds instructions for the creditor agent to the last credit transfer transaction.
        /// </summary>
        /// <param name="instruction">Instruction for the creditor agent.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when instruction is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when no transactions exist.</exception>
        public Pain00100107Builder AddInstructionForCreditorAgent(InstructionForCreditorAgent1 instruction)
        {
            ValidateParameter(instruction, nameof(instruction));

            var lastTransaction = GetLastTransaction();
            lastTransaction.InstrForCdtrAgt.Add(instruction);
            return this;
        }

        /// <summary>
        /// Sets the ultimate debtor for the last credit transfer transaction.
        /// </summary>
        /// <param name="ultimateDebtor">Ultimate party that owes money to the creditor.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when ultimateDebtor is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when no transactions exist.</exception>
        public Pain00100107Builder SetUltimateDebtor(PartyIdentification43 ultimateDebtor)
        {
            ValidateParameter(ultimateDebtor, nameof(ultimateDebtor));

            var lastTransaction = GetLastTransaction();
            lastTransaction.UltmtDbtr = ultimateDebtor;
            return this;
        }

        /// <summary>
        /// Sets the ultimate creditor for the last credit transfer transaction.
        /// </summary>
        /// <param name="ultimateCreditor">Ultimate party that receives the payment.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when ultimateCreditor is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when no transactions exist.</exception>
        public Pain00100107Builder SetUltimateCreditor(PartyIdentification43 ultimateCreditor)
        {
            ValidateParameter(ultimateCreditor, nameof(ultimateCreditor));

            var lastTransaction = GetLastTransaction();
            lastTransaction.UltmtCdtr = ultimateCreditor;
            return this;
        }

        /// <summary>
        /// Adds supplementary data to the message.
        /// </summary>
        /// <param name="supplementaryData">Additional data related to the message.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when supplementaryData is null.</exception>
        public Pain00100107Builder AddSupplementaryData(SupplementaryData1 supplementaryData)
        {
            ValidateParameter(supplementaryData, nameof(supplementaryData));

            _document.CstmrCdtTrfInitn.SplmtryData.Add(supplementaryData);
            return this;
        }

        /// <summary>
        /// Builds and returns the completed message document.
        /// </summary>
        /// <returns>The completed ISO 20022 pain.001.001.07 document.</returns>
        public Document Build() => _document;

        /// <summary>
        /// Builds and serializes the message to XML format.
        /// </summary>
        /// <returns>XML representation of the pain.001.001.07 message.</returns>
        public string BuildXml()
        {
            return XmlSerializationService.Serialize(_document);
        }

        /// <summary>
        /// Builds and serializes the provided message to XML format.
        /// </summary>
        /// <param name="message">The message to serialize.</param>
        /// <returns>XML representation of the message.</returns>
        /// <exception cref="ArgumentNullException">Thrown when message is null.</exception>
        /// <exception cref="InvalidCastException">Thrown when message is not a valid Document.</exception>
        public string BuildXml(object message)
        {
            ValidateParameter(message, nameof(message));

            if (message is not Document doc)
                throw new InvalidCastException($"Expected message of type {typeof(Document).Name}, but received {message.GetType().Name}");

            return XmlSerializationService.Serialize(doc);
        }

        /// <summary>
        /// Gets the message type identifier for this builder.
        /// </summary>
        /// <returns>The message type identifier "pain.001.001.07".</returns>
        public string GetMessageType() => "pain.001.001.07";

        /// <summary>
        /// Validates that a parameter is not null or empty.
        /// </summary>
        /// <param name="parameter">The parameter to validate.</param>
        /// <param name="parameterName">The name of the parameter for error reporting.</param>
        /// <exception cref="ArgumentNullException">Thrown when the parameter is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the parameter is an empty string.</exception>
        private static void ValidateParameter(object? parameter, string parameterName)
        {
            if (parameter == null)
                throw new ArgumentNullException(parameterName);

            if (parameter is string str && string.IsNullOrWhiteSpace(str))
                throw new ArgumentException($"Parameter '{parameterName}' cannot be null or empty.", parameterName);
        }

        /// <summary>
        /// Gets the last credit transfer transaction from the last payment instruction.
        /// </summary>
        /// <returns>The last credit transfer transaction.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no transactions exist.</exception>
        private CreditTransferTransaction26 GetLastTransaction()
        {
            if (_document.CstmrCdtTrfInitn.PmtInf == null || _document.CstmrCdtTrfInitn.PmtInf.Count == 0)
                throw new InvalidOperationException("No payment instructions exist. Add a payment instruction first.");

            var lastPaymentInstruction = _document.CstmrCdtTrfInitn.PmtInf[_document.CstmrCdtTrfInitn.PmtInf.Count - 1];

            if (lastPaymentInstruction.CdtTrfTxInf == null || lastPaymentInstruction.CdtTrfTxInf.Count == 0)
                throw new InvalidOperationException("No credit transfer transactions exist in the last payment instruction. Add a transaction first.");

            return lastPaymentInstruction.CdtTrfTxInf[lastPaymentInstruction.CdtTrfTxInf.Count - 1];
        }
    }
}