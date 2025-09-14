using System;
using System.Collections.Generic;
using System.Linq;
using Iso20022Library.Domain.Common.Enums;
using Iso20022Library.Domain.Common.Interfaces;
using Iso20022Library.Infrastructure.Xml;
using Iso20022Library.Messages.Payments.Pacs.Generated.Pacs00700110;

namespace Iso20022Library.Application.Builders.Pacs;

/// <summary>
/// Builder for PACS.007.001.10 - FIToFIPaymentReversalV10 - Payment Reversal
/// This message is used to request the reversal of an interbank payment.
/// </summary>
public class Pacs00700110Builder : IMessageBuilder
{
    #region Private Fields

    private readonly Document _document;
    private readonly List<PaymentTransaction119> _paymentTransactions;
    private readonly List<SupplementaryData1> _supplementaryData;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the message type for this builder.
    /// </summary>
    public MessageType MessageType => MessageType.Pacs00700110;

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the Pacs00700110Builder class.
    /// </summary>
    public Pacs00700110Builder()
    {
        _document = new Document
        {
            FIToFIPmtRvsl = new FIToFIPaymentReversalV10()
        };
        _paymentTransactions = new List<PaymentTransaction119>();
        _supplementaryData = new List<SupplementaryData1>();
    }

    #endregion

    #region Group Header Methods

    /// <summary>
    /// Sets the message identification and creation date/time for the payment reversal.
    /// </summary>
    /// <param name="messageId">Unique message identification assigned by the instructing party.</param>
    /// <param name="creationDateTime">Date and time at which the message was created.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when messageId is null or empty.</exception>
    public Pacs00700110Builder SetMessageId(string messageId, DateTime creationDateTime)
    {
        if (string.IsNullOrWhiteSpace(messageId))
            throw new ArgumentException("Message ID cannot be null or empty.", nameof(messageId));

        _document.FIToFIPmtRvsl.GrpHdr = new GroupHeader89
        {
            MsgId = messageId,
            CreDtTm = creationDateTime
        };

        return this;
    }

    /// <summary>
    /// Sets the group header with comprehensive payment reversal information.
    /// </summary>
    /// <param name="messageId">Unique message identification.</param>
    /// <param name="creationDateTime">Date and time at which the message was created.</param>
    /// <param name="numberOfTransactions">Number of individual transactions contained in the message.</param>
    /// <param name="controlSum">Total of all individual amounts included in the message, irrespective of currencies.</param>
    /// <param name="groupReversal">Indicates whether the reversal applies to the whole group or individual transactions.</param>
    /// <param name="totalReversedAmount">Total amount of all reversed transactions.</param>
    /// <param name="interbankSettlementDate">Date on which the amount of money ceases to be available to the agent that owes it.</param>
    /// <param name="batchBooking">Identifies whether the financial institution processes the payment instruction as a batch or individual transaction.</param>
    /// <param name="instructingAgent">Agent that instructs the next party in the chain to carry out the reversal.</param>
    /// <param name="instructedAgent">Agent that is instructed by the previous party in the chain to carry out the reversal.</param>
    /// <param name="settlementInformation">Specifies the details on how the settlement of the transaction(s) between the instructing agent and the instructed agent is completed.</param>
    /// <param name="authorisation">User identification or any user key to be used to check whether the initiating party is allowed to initiate transactions from the account specified in the message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when messageId is null or empty.</exception>
    public Pacs00700110Builder WithGroupHeader(
        string messageId,
        DateTime creationDateTime,
        string numberOfTransactions,
        decimal? controlSum = null,
        bool? groupReversal = null,
        ActiveCurrencyAndAmount? totalReversedAmount = null,
        DateTime? interbankSettlementDate = null,
        bool? batchBooking = null,
        BranchAndFinancialInstitutionIdentification6? instructingAgent = null,
        BranchAndFinancialInstitutionIdentification6? instructedAgent = null,
        SettlementInstruction7? settlementInformation = null,
        Authorisation1Choice[]? authorisation = null)
    {
        if (string.IsNullOrWhiteSpace(messageId))
            throw new ArgumentException("Message ID cannot be null or empty.", nameof(messageId));

        if (string.IsNullOrWhiteSpace(numberOfTransactions))
            throw new ArgumentException("Number of transactions cannot be null or empty.", nameof(numberOfTransactions));

        _document.FIToFIPmtRvsl.GrpHdr = new GroupHeader89
        {
            MsgId = messageId,
            CreDtTm = creationDateTime,
            NbOfTxs = numberOfTransactions,
            CtrlSum = controlSum ?? 0,
            CtrlSumSpecified = controlSum.HasValue,
            GrpRvsl = groupReversal ?? false,
            GrpRvslSpecified = groupReversal.HasValue,
            TtlRvsdIntrBkSttlmAmt = totalReversedAmount,
            IntrBkSttlmDt = interbankSettlementDate ?? DateTime.MinValue,
            IntrBkSttlmDtSpecified = interbankSettlementDate.HasValue,
            BtchBookg = batchBooking ?? false,
            BtchBookgSpecified = batchBooking.HasValue,
            InstgAgt = instructingAgent,
            InstdAgt = instructedAgent,
            SttlmInf = settlementInformation,
            Authstn = authorisation
        };

        return this;
    }

    /// <summary>
    /// Sets the instructing and instructed agents for the payment reversal.
    /// </summary>
    /// <param name="instructingAgentBic">BIC of the agent that instructs the next party in the chain.</param>
    /// <param name="instructingAgentName">Name of the instructing agent.</param>
    /// <param name="instructedAgentBic">BIC of the agent that is instructed.</param>
    /// <param name="instructedAgentName">Name of the instructed agent.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public Pacs00700110Builder SetInstructingAndInstructedAgents(
        string instructingAgentBic,
        string? instructingAgentName = null,
        string? instructedAgentBic = null,
        string? instructedAgentName = null)
    {
        if (_document.FIToFIPmtRvsl.GrpHdr == null)
        {
            SetMessageId("DEFAULT", DateTime.UtcNow);
        }

        if (!string.IsNullOrWhiteSpace(instructingAgentBic))
        {
            _document.FIToFIPmtRvsl.GrpHdr!.InstgAgt = new BranchAndFinancialInstitutionIdentification6
            {
                FinInstnId = new FinancialInstitutionIdentification18
                {
                    BICFI = instructingAgentBic,
                    Nm = instructingAgentName
                }
            };
        }

        if (!string.IsNullOrWhiteSpace(instructedAgentBic))
        {
            _document.FIToFIPmtRvsl.GrpHdr!.InstdAgt = new BranchAndFinancialInstitutionIdentification6
            {
                FinInstnId = new FinancialInstitutionIdentification18
                {
                    BICFI = instructedAgentBic,
                    Nm = instructedAgentName
                }
            };
        }

        return this;
    }

    #endregion

    #region Original Group Information Methods

    /// <summary>
    /// Sets the original group information for the payment reversal.
    /// </summary>
    /// <param name="originalMessageId">Point to point reference assigned by the original instructing party.</param>
    /// <param name="originalMessageNameId">Specifies the original message name identifier to which the message refers.</param>
    /// <param name="originalCreationDateTime">Date and time at which the original message was created.</param>
    /// <param name="reversalReasonInformation">Provides detailed information on the reversal reason.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when originalMessageId is null or empty.</exception>
    public Pacs00700110Builder WithOriginalGroupInformation(
        string originalMessageId,
        string? originalMessageNameId = null,
        DateTime? originalCreationDateTime = null,
        PaymentReversalReason9[]? reversalReasonInformation = null)
    {
        if (string.IsNullOrWhiteSpace(originalMessageId))
            throw new ArgumentException("Original message ID cannot be null or empty.", nameof(originalMessageId));

        _document.FIToFIPmtRvsl.OrgnlGrpInf = new OriginalGroupHeader16
        {
            OrgnlMsgId = originalMessageId,
            OrgnlMsgNmId = originalMessageNameId,
            OrgnlCreDtTm = originalCreationDateTime ?? DateTime.MinValue,
            OrgnlCreDtTmSpecified = originalCreationDateTime.HasValue,
            RvslRsnInf = reversalReasonInformation
        };

        return this;
    }

    #endregion

    #region Payment Transaction Methods

    /// <summary>
    /// Adds a payment reversal transaction to the message.
    /// </summary>
    /// <param name="reversalId">Unique identification assigned by the instructing party to unambiguously identify the reversal transaction.</param>
    /// <param name="originalInstructionId">Unique identification assigned by the original instructing party.</param>
    /// <param name="originalEndToEndId">Unique identification assigned by the original initiating party.</param>
    /// <param name="originalTransactionId">Unique identification assigned by the original first instructing agent.</param>
    /// <param name="reversedInterbankSettlementAmount">Amount of money to be moved between the instructing agent and the instructed agent in the original instruction.</param>
    /// <param name="interbankSettlementDate">Date on which the amount of money ceases to be available to the agent that owes it.</param>
    /// <param name="reversalReason">Provides information on the reason of the return of the transaction.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when reversalId is null or empty.</exception>
    public Pacs00700110Builder AddReversalTransaction(
        string reversalId,
        string? originalInstructionId = null,
        string? originalEndToEndId = null,
        string? originalTransactionId = null,
        ActiveCurrencyAndAmount? reversedInterbankSettlementAmount = null,
        DateTime? interbankSettlementDate = null,
        PaymentReversalReason9[]? reversalReason = null)
    {
        if (string.IsNullOrWhiteSpace(reversalId))
            throw new ArgumentException("Reversal ID cannot be null or empty.", nameof(reversalId));

        var transaction = new PaymentTransaction119
        {
            RvslId = reversalId,
            OrgnlInstrId = originalInstructionId,
            OrgnlEndToEndId = originalEndToEndId,
            OrgnlTxId = originalTransactionId,
            RvsdIntrBkSttlmAmt = reversedInterbankSettlementAmount,
            IntrBkSttlmDt = interbankSettlementDate ?? DateTime.MinValue,
            IntrBkSttlmDtSpecified = interbankSettlementDate.HasValue,
            RvslRsnInf = reversalReason
        };

        _paymentTransactions.Add(transaction);

        // Update the number of transactions in the group header
        if (_document.FIToFIPmtRvsl.GrpHdr != null)
        {
            _document.FIToFIPmtRvsl.GrpHdr.NbOfTxs = _paymentTransactions.Count.ToString();
        }

        return this;
    }

    /// <summary>
    /// Adds a comprehensive payment reversal transaction with full details.
    /// </summary>
    /// <param name="reversalId">Unique identification for the reversal transaction.</param>
    /// <param name="originalGroupInformation">Set of elements used to provide information on the original group.</param>
    /// <param name="originalInstructionId">Unique identification assigned by the original instructing party.</param>
    /// <param name="originalEndToEndId">Unique identification assigned by the original initiating party.</param>
    /// <param name="originalTransactionId">Unique identification assigned by the original first instructing agent.</param>
    /// <param name="originalUETR">Universally unique identifier to provide an end-to-end reference of a payment transaction.</param>
    /// <param name="originalClearingSystemReference">Unique reference assigned by a clearing system.</param>
    /// <param name="originalInterbankSettlementAmount">Amount of money to be moved between the instructing agent and the instructed agent in the original instruction.</param>
    /// <param name="reversedInterbankSettlementAmount">Amount of money to be moved between the instructing agent and the instructed agent in the reversal instruction.</param>
    /// <param name="interbankSettlementDate">Date on which the amount of money ceases to be available to the agent that owes it.</param>
    /// <param name="settlementPriority">Indicator of the urgency or order of importance that the instructing party would like the instructed party to apply to the processing of the instruction.</param>
    /// <param name="settlementTimeIndication">Provides information on the occurred settlement time(s) of the payment transaction.</param>
    /// <param name="reversedInstructedAmount">Amount of money to be moved between the debtor and creditor, before deduction of charges, in the original instruction.</param>
    /// <param name="exchangeRate">Factor used to convert an amount from one currency into another.</param>
    /// <param name="compensationAmount">Amount of money asked or paid as compensation for the processing of the instruction.</param>
    /// <param name="chargeBearer">Specifies which party/parties will bear the charges associated with the processing of the payment transaction.</param>
    /// <param name="chargesInformation">Provides information on the charges to be paid by the charge bearer(s) related to the payment transaction.</param>
    /// <param name="instructingAgent">Agent that instructs the next party in the chain to carry out the instruction.</param>
    /// <param name="instructedAgent">Agent that is instructed by the previous party in the chain to carry out the instruction.</param>
    /// <param name="reversalReasonInformation">Provides detailed information on the reversal reason.</param>
    /// <param name="originalTransactionReference">Key elements used to refer the original transaction that is being reversed by this confirmation.</param>
    /// <param name="supplementaryData">Additional information that cannot be captured in the structured elements and/or any other specific block.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when reversalId is null or empty.</exception>
    public Pacs00700110Builder AddComprehensiveReversalTransaction(
        string reversalId,
        OriginalGroupInformation29? originalGroupInformation = null,
        string? originalInstructionId = null,
        string? originalEndToEndId = null,
        string? originalTransactionId = null,
        string? originalUETR = null,
        string? originalClearingSystemReference = null,
        ActiveOrHistoricCurrencyAndAmount? originalInterbankSettlementAmount = null,
        ActiveCurrencyAndAmount? reversedInterbankSettlementAmount = null,
        DateTime? interbankSettlementDate = null,
        Priority3Code? settlementPriority = null,
        SettlementDateTimeIndication1? settlementTimeIndication = null,
        ActiveOrHistoricCurrencyAndAmount? reversedInstructedAmount = null,
        decimal? exchangeRate = null,
        ActiveOrHistoricCurrencyAndAmount? compensationAmount = null,
        ChargeBearerType1Code? chargeBearer = null,
        Charges7[]? chargesInformation = null,
        BranchAndFinancialInstitutionIdentification6? instructingAgent = null,
        BranchAndFinancialInstitutionIdentification6? instructedAgent = null,
        PaymentReversalReason9[]? reversalReasonInformation = null,
        OriginalTransactionReference31? originalTransactionReference = null,
        SupplementaryData1[]? supplementaryData = null)
    {
        if (string.IsNullOrWhiteSpace(reversalId))
            throw new ArgumentException("Reversal ID cannot be null or empty.", nameof(reversalId));

        var transaction = new PaymentTransaction119
        {
            RvslId = reversalId,
            OrgnlGrpInf = originalGroupInformation,
            OrgnlInstrId = originalInstructionId,
            OrgnlEndToEndId = originalEndToEndId,
            OrgnlTxId = originalTransactionId,
            OrgnlUETR = originalUETR,
            OrgnlClrSysRef = originalClearingSystemReference,
            OrgnlIntrBkSttlmAmt = originalInterbankSettlementAmount,
            RvsdIntrBkSttlmAmt = reversedInterbankSettlementAmount,
            IntrBkSttlmDt = interbankSettlementDate ?? DateTime.MinValue,
            IntrBkSttlmDtSpecified = interbankSettlementDate.HasValue,
            SttlmPrty = settlementPriority ?? Priority3Code.NORM,
            SttlmPrtySpecified = settlementPriority.HasValue,
            SttlmTmIndctn = settlementTimeIndication,
            RvsdInstdAmt = reversedInstructedAmount,
            XchgRate = exchangeRate ?? 0,
            XchgRateSpecified = exchangeRate.HasValue,
            CompstnAmt = compensationAmount,
            ChrgBr = chargeBearer ?? ChargeBearerType1Code.SLEV,
            ChrgBrSpecified = chargeBearer.HasValue,
            ChrgsInf = chargesInformation,
            InstgAgt = instructingAgent,
            InstdAgt = instructedAgent,
            RvslRsnInf = reversalReasonInformation,
            OrgnlTxRef = originalTransactionReference,
            SplmtryData = supplementaryData
        };

        _paymentTransactions.Add(transaction);

        // Update the number of transactions in the group header
        if (_document.FIToFIPmtRvsl.GrpHdr != null)
        {
            _document.FIToFIPmtRvsl.GrpHdr.NbOfTxs = _paymentTransactions.Count.ToString();
        }

        return this;
    }

    #endregion

    #region Supplementary Data Methods

    /// <summary>
    /// Adds supplementary data to the payment reversal message.
    /// </summary>
    /// <param name="supplementaryData">Additional information that cannot be captured in the structured elements.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when supplementaryData is null.</exception>
    public Pacs00700110Builder AddSupplementaryData(SupplementaryData1 supplementaryData)
    {
        if (supplementaryData == null)
            throw new ArgumentNullException(nameof(supplementaryData));

        _supplementaryData.Add(supplementaryData);
        return this;
    }

    #endregion

    #region Build Methods

    /// <summary>
    /// Generates the XML representation of the payment reversal message.
    /// </summary>
    /// <returns>The XML string representation of the PACS.007.001.10 message.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the message is not properly configured.</exception>
    public string GenerateXml()
    {
        var document = Build();
        return XmlSerializationService.Serialize(document);
    }

    /// <summary>
    /// Builds the FIToFIPaymentReversalV10 document.
    /// </summary>
    /// <returns>The constructed FIToFIPaymentReversalV10 document.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the message is not properly configured.</exception>
    public Document Build()
    {
        ValidateMessage();

        // Set the payment transactions
        _document.FIToFIPmtRvsl.TxInf = _paymentTransactions.ToArray();

        // Set supplementary data if any
        if (_supplementaryData.Any())
        {
            _document.FIToFIPmtRvsl.SplmtryData = _supplementaryData.ToArray();
        }

        return _document;
    }

    /// <summary>
    /// Builds the XML representation of the payment reversal message.
    /// </summary>
    /// <param name="message">The message object (not used, the builder creates its own document).</param>
    /// <returns>The XML string representation of the PACS.007.001.10 message.</returns>
    public string BuildXml(object message)
    {
        var document = Build();
        return XmlSerializationService.Serialize(document);
    }

    /// <summary>
    /// Creates a copy of the current builder.
    /// </summary>
    /// <returns>A new Pacs00700110Builder instance with the same configuration.</returns>
    public Pacs00700110Builder Clone()
    {
        var clonedBuilder = new Pacs00700110Builder();

        // Clone the document structure
        if (_document.FIToFIPmtRvsl.GrpHdr != null)
        {
            var originalHeader = _document.FIToFIPmtRvsl.GrpHdr;
            clonedBuilder.WithGroupHeader(
                originalHeader.MsgId,
                originalHeader.CreDtTm,
                originalHeader.NbOfTxs,
                originalHeader.CtrlSumSpecified ? originalHeader.CtrlSum : null,
                originalHeader.GrpRvslSpecified ? originalHeader.GrpRvsl : null,
                originalHeader.TtlRvsdIntrBkSttlmAmt,
                originalHeader.IntrBkSttlmDtSpecified ? originalHeader.IntrBkSttlmDt : null,
                originalHeader.BtchBookgSpecified ? originalHeader.BtchBookg : null,
                originalHeader.InstgAgt,
                originalHeader.InstdAgt,
                originalHeader.SttlmInf,
                originalHeader.Authstn);
        }

        // Clone original group information
        if (_document.FIToFIPmtRvsl.OrgnlGrpInf != null)
        {
            var originalGrpInf = _document.FIToFIPmtRvsl.OrgnlGrpInf;
            clonedBuilder.WithOriginalGroupInformation(
                originalGrpInf.OrgnlMsgId,
                originalGrpInf.OrgnlMsgNmId,
                originalGrpInf.OrgnlCreDtTmSpecified ? originalGrpInf.OrgnlCreDtTm : null,
                originalGrpInf.RvslRsnInf);
        }

        // Clone transactions
        foreach (var transaction in _paymentTransactions)
        {
            clonedBuilder._paymentTransactions.Add(transaction);
        }

        // Clone supplementary data
        foreach (var data in _supplementaryData)
        {
            clonedBuilder._supplementaryData.Add(data);
        }

        return clonedBuilder;
    }

    #endregion

    #region Validation Methods

    /// <summary>
    /// Validates the message structure before XML generation.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the message structure is invalid.</exception>
    private void ValidateMessage()
    {
        if (_document.FIToFIPmtRvsl.GrpHdr == null)
            throw new InvalidOperationException("Group header is required for PACS.007.001.10 message.");

        if (string.IsNullOrEmpty(_document.FIToFIPmtRvsl.GrpHdr.MsgId))
            throw new InvalidOperationException("Message ID is required in the group header.");

        if (string.IsNullOrEmpty(_document.FIToFIPmtRvsl.GrpHdr.NbOfTxs))
            throw new InvalidOperationException("Number of transactions is required in the group header.");

        if (!_paymentTransactions.Any())
            throw new InvalidOperationException("At least one payment transaction is required.");

        // Validate that the number of transactions matches the actual count
        if (int.TryParse(_document.FIToFIPmtRvsl.GrpHdr.NbOfTxs, out int declaredCount) &&
            declaredCount != _paymentTransactions.Count)
        {
            throw new InvalidOperationException(
                $"Declared number of transactions ({declaredCount}) does not match actual count ({_paymentTransactions.Count}).");
        }

        // Validate control sum if specified
        if (_document.FIToFIPmtRvsl.GrpHdr.CtrlSumSpecified)
        {
            var totalAmount = _paymentTransactions
                .Where(t => t.RvsdIntrBkSttlmAmt != null)
                .Sum(t => t.RvsdIntrBkSttlmAmt!.Value);

            if (Math.Abs(_document.FIToFIPmtRvsl.GrpHdr.CtrlSum - totalAmount) > 0.01m)
            {
                throw new InvalidOperationException(
                    $"Control sum ({_document.FIToFIPmtRvsl.GrpHdr.CtrlSum}) does not match total transaction amount ({totalAmount}).");
            }
        }

        // Validate each transaction has a reversal ID
        foreach (var transaction in _paymentTransactions)
        {
            if (string.IsNullOrEmpty(transaction.RvslId))
                throw new InvalidOperationException("Each payment transaction must have a reversal ID.");
        }
    }

    #endregion
}
