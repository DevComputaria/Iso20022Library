using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Iso20022Library.Application.Builders.Pacs;
using Iso20022Library.Messages.Payments.Pacs.Generated.Pacs00700110;
using Iso20022Library.Domain.Common.Enums;

namespace Iso20022Library.Tests.Builders.Pacs;

/// <summary>
/// Unit tests for the Pacs00700110Builder class.
/// Tests the creation of PACS.007.001.10 Payment Reversal messages.
/// </summary>
[TestClass]
public class Pacs00700110BuilderTests
{
    private Pacs00700110Builder _builder;

    [TestInitialize]
    public void Setup()
    {
        _builder = new Pacs00700110Builder();
    }

    #region Basic Functionality Tests

    [TestMethod]
    public void MessageType_ShouldReturnPacs00700110()
    {
        // Act
        var messageType = _builder.MessageType;

        // Assert
        Assert.AreEqual(MessageType.Pacs00700110, messageType);
    }

    [TestMethod]
    public void SetMessageId_WithValidData_ShouldSetGroupHeader()
    {
        // Arrange
        var messageId = "MSG12345";
        var creationDateTime = DateTime.UtcNow;

        // Act
        _builder.SetMessageId(messageId, creationDateTime)
                .AddReversalTransaction("REV123", "INST123", "E2E123", "TXN123");
        var document = _builder.Build();

        // Assert
        Assert.IsNotNull(document.FIToFIPmtRvsl.GrpHdr);
        Assert.AreEqual(messageId, document.FIToFIPmtRvsl.GrpHdr.MsgId);
        Assert.AreEqual(creationDateTime, document.FIToFIPmtRvsl.GrpHdr.CreDtTm);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void SetMessageId_WithEmptyMessageId_ShouldThrowException()
    {
        // Act
        _builder.SetMessageId("", DateTime.UtcNow);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void SetMessageId_WithNullMessageId_ShouldThrowException()
    {
        // Act
        _builder.SetMessageId(null!, DateTime.UtcNow);
    }

    #endregion

    #region Group Header Tests

    [TestMethod]
    public void WithGroupHeader_WithValidData_ShouldSetAllFields()
    {
        // Arrange
        var messageId = "MSG12345";
        var creationDateTime = DateTime.UtcNow;
        var numberOfTransactions = "1";
        var controlSum = 1000.50m;
        var groupReversal = true;

        var reversedAmount = new ActiveCurrencyAndAmount
        {
            Value = 1000.50m,
            Ccy = "EUR"
        };

        // Act
        _builder.WithGroupHeader(
            messageId,
            creationDateTime,
            numberOfTransactions,
            controlSum,
            groupReversal)
            .AddReversalTransaction("REV123", "INST123", "E2E123", "TXN123", reversedAmount);

        var document = _builder.Build();

        // Assert
        var header = document.FIToFIPmtRvsl.GrpHdr;
        Assert.IsNotNull(header);
        Assert.AreEqual(messageId, header.MsgId);
        Assert.AreEqual(creationDateTime, header.CreDtTm);
        Assert.AreEqual(numberOfTransactions, header.NbOfTxs);
        Assert.AreEqual(controlSum, header.CtrlSum);
        Assert.IsTrue(header.CtrlSumSpecified);
        Assert.AreEqual(groupReversal, header.GrpRvsl);
        Assert.IsTrue(header.GrpRvslSpecified);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void WithGroupHeader_WithEmptyNumberOfTransactions_ShouldThrowException()
    {
        // Act
        _builder.WithGroupHeader("MSG123", DateTime.UtcNow, "");
    }

    [TestMethod]
    public void SetInstructingAndInstructedAgents_WithValidData_ShouldSetAgents()
    {
        // Arrange
        var instructingBic = "BANKGB2L";
        var instructingName = "Test Bank UK";
        var instructedBic = "BANKDEFF";
        var instructedName = "Test Bank DE";

        // Act
        _builder.SetInstructingAndInstructedAgents(
            instructingBic,
            instructingName,
            instructedBic,
            instructedName)
            .AddReversalTransaction("REV123", "INST123", "E2E123", "TXN123");

        var document = _builder.Build();

        // Assert
        var header = document.FIToFIPmtRvsl.GrpHdr;
        Assert.IsNotNull(header);
        Assert.IsNotNull(header.InstgAgt);
        Assert.AreEqual(instructingBic, header.InstgAgt.FinInstnId.BICFI);
        Assert.AreEqual(instructingName, header.InstgAgt.FinInstnId.Nm);
        Assert.IsNotNull(header.InstdAgt);
        Assert.AreEqual(instructedBic, header.InstdAgt.FinInstnId.BICFI);
        Assert.AreEqual(instructedName, header.InstdAgt.FinInstnId.Nm);
    }

    #endregion

    #region Original Group Information Tests

    [TestMethod]
    public void WithOriginalGroupInformation_WithValidData_ShouldSetOriginalGroupInfo()
    {
        // Arrange
        var originalMessageId = "ORIG12345";
        var originalMessageNameId = "pacs.008.001.10";
        var originalCreationDateTime = DateTime.UtcNow.AddHours(-1);

        // Act
        _builder.SetMessageId("MSG123", DateTime.UtcNow)
                .WithOriginalGroupInformation(
                    originalMessageId,
                    originalMessageNameId,
                    originalCreationDateTime)
                .AddReversalTransaction("REV123", "INST123", "E2E123", "TXN123");

        var document = _builder.Build();

        // Assert
        var originalGroupInfo = document.FIToFIPmtRvsl.OrgnlGrpInf;
        Assert.IsNotNull(originalGroupInfo);
        Assert.AreEqual(originalMessageId, originalGroupInfo.OrgnlMsgId);
        Assert.AreEqual(originalMessageNameId, originalGroupInfo.OrgnlMsgNmId);
        Assert.AreEqual(originalCreationDateTime, originalGroupInfo.OrgnlCreDtTm);
        Assert.IsTrue(originalGroupInfo.OrgnlCreDtTmSpecified);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void WithOriginalGroupInformation_WithEmptyOriginalMessageId_ShouldThrowException()
    {
        // Act
        _builder.WithOriginalGroupInformation("");
    }

    #endregion

    #region Payment Transaction Tests

    [TestMethod]
    public void AddReversalTransaction_WithValidData_ShouldAddTransaction()
    {
        // Arrange
        var reversalId = "REV12345";
        var originalInstructionId = "INST12345";
        var originalEndToEndId = "E2E12345";
        var originalTransactionId = "TXN12345";

        // Act
        _builder.SetMessageId("MSG123", DateTime.UtcNow)
                .AddReversalTransaction(
                    reversalId,
                    originalInstructionId,
                    originalEndToEndId,
                    originalTransactionId);

        var document = _builder.Build();

        // Assert
        Assert.IsNotNull(document.FIToFIPmtRvsl.TxInf);
        Assert.AreEqual(1, document.FIToFIPmtRvsl.TxInf.Length);

        var transaction = document.FIToFIPmtRvsl.TxInf[0];
        Assert.AreEqual(reversalId, transaction.RvslId);
        Assert.AreEqual(originalInstructionId, transaction.OrgnlInstrId);
        Assert.AreEqual(originalEndToEndId, transaction.OrgnlEndToEndId);
        Assert.AreEqual(originalTransactionId, transaction.OrgnlTxId);

        // Check that group header transaction count is updated
        Assert.AreEqual("1", document.FIToFIPmtRvsl.GrpHdr.NbOfTxs);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AddReversalTransaction_WithEmptyReversalId_ShouldThrowException()
    {
        // Act
        _builder.AddReversalTransaction("");
    }

    [TestMethod]
    public void AddComprehensiveReversalTransaction_WithValidData_ShouldAddTransaction()
    {
        // Arrange
        var reversalId = "REV12345";
        var originalInstructionId = "INST12345";
        var originalEndToEndId = "E2E12345";
        var originalTransactionId = "TXN12345";
        var originalUETR = "00112233-4455-6677-8899-AABBCCDDEEFF";
        var exchangeRate = 1.1234m;

        var reversedAmount = new ActiveCurrencyAndAmount
        {
            Value = 1000.50m,
            Ccy = "EUR"
        };

        // Act
        _builder.SetMessageId("MSG123", DateTime.UtcNow)
                .AddComprehensiveReversalTransaction(
                    reversalId: reversalId,
                    originalInstructionId: originalInstructionId,
                    originalEndToEndId: originalEndToEndId,
                    originalTransactionId: originalTransactionId,
                    originalUETR: originalUETR,
                    reversedInterbankSettlementAmount: reversedAmount,
                    exchangeRate: exchangeRate);

        var document = _builder.Build();

        // Assert
        Assert.IsNotNull(document.FIToFIPmtRvsl.TxInf);
        Assert.AreEqual(1, document.FIToFIPmtRvsl.TxInf.Length);

        var transaction = document.FIToFIPmtRvsl.TxInf[0];
        Assert.AreEqual(reversalId, transaction.RvslId);
        Assert.AreEqual(originalInstructionId, transaction.OrgnlInstrId);
        Assert.AreEqual(originalEndToEndId, transaction.OrgnlEndToEndId);
        Assert.AreEqual(originalTransactionId, transaction.OrgnlTxId);
        Assert.AreEqual(originalUETR, transaction.OrgnlUETR);
        Assert.AreEqual(reversedAmount, transaction.RvsdIntrBkSttlmAmt);
        Assert.AreEqual(exchangeRate, transaction.XchgRate);
        Assert.IsTrue(transaction.XchgRateSpecified);
    }

    [TestMethod]
    public void AddMultipleTransactions_ShouldUpdateTransactionCount()
    {
        // Arrange & Act
        _builder.SetMessageId("MSG123", DateTime.UtcNow)
                .AddReversalTransaction("REV001", "INST001", "E2E001", "TXN001")
                .AddReversalTransaction("REV002", "INST002", "E2E002", "TXN002")
                .AddReversalTransaction("REV003", "INST003", "E2E003", "TXN003");

        var document = _builder.Build();

        // Assert
        Assert.AreEqual(3, document.FIToFIPmtRvsl.TxInf.Length);
        Assert.AreEqual("3", document.FIToFIPmtRvsl.GrpHdr.NbOfTxs);
    }

    #endregion

    #region Supplementary Data Tests

    [TestMethod]
    public void AddSupplementaryData_WithValidData_ShouldAddData()
    {
        // Arrange
        var supplementaryData = new SupplementaryData1
        {
            PlcAndNm = "Additional Info"
        };

        // Act
        _builder.SetMessageId("MSG123", DateTime.UtcNow)
                .AddReversalTransaction("REV123", "INST123", "E2E123", "TXN123")
                .AddSupplementaryData(supplementaryData);

        var document = _builder.Build();

        // Assert
        Assert.IsNotNull(document.FIToFIPmtRvsl.SplmtryData);
        Assert.AreEqual(1, document.FIToFIPmtRvsl.SplmtryData.Length);
        Assert.AreEqual(supplementaryData, document.FIToFIPmtRvsl.SplmtryData[0]);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddSupplementaryData_WithNullData_ShouldThrowException()
    {
        // Act
        _builder.AddSupplementaryData(null!);
    }

    #endregion

    #region XML Generation Tests

    [TestMethod]
    public void GenerateXml_WithValidMessage_ShouldReturnValidXml()
    {
        // Arrange
        _builder.SetMessageId("MSG12345", DateTime.UtcNow)
                .WithOriginalGroupInformation("ORIG12345", "pacs.008.001.10")
                .AddReversalTransaction("REV12345", "INST12345", "E2E12345", "TXN12345");

        // Act
        var xml = _builder.GenerateXml();

        // Assert
        Assert.IsNotNull(xml);
        Assert.IsTrue(xml.Contains("<?xml version=\"1.0\" encoding=\"utf-8\"?>"));
        Assert.IsTrue(xml.Contains("<Document"));
        Assert.IsTrue(xml.Contains("<FIToFIPmtRvsl>"));
        Assert.IsTrue(xml.Contains("<GrpHdr>"));
        Assert.IsTrue(xml.Contains("<MsgId>MSG12345</MsgId>"));
        Assert.IsTrue(xml.Contains("<OrgnlGrpInf>"));
        Assert.IsTrue(xml.Contains("<TxInf>"));
        Assert.IsTrue(xml.Contains("<RvslId>REV12345</RvslId>"));
    }

    [TestMethod]
    public void BuildXml_WithMessage_ShouldReturnValidXml()
    {
        // Arrange
        _builder.SetMessageId("MSG12345", DateTime.UtcNow)
                .AddReversalTransaction("REV12345", "INST12345", "E2E12345", "TXN12345");

        // Act
        var xml = _builder.BuildXml(new object());

        // Assert
        Assert.IsNotNull(xml);
        Assert.IsTrue(xml.Contains("<Document"));
        Assert.IsTrue(xml.Contains("<FIToFIPmtRvsl>"));
    }

    #endregion

    #region Validation Tests

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Build_WithoutGroupHeader_ShouldThrowException()
    {
        // Act
        _builder.Build();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Build_WithoutTransactions_ShouldThrowException()
    {
        // Arrange
        _builder.SetMessageId("MSG123", DateTime.UtcNow);

        // Act
        _builder.Build();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Build_WithMismatchedTransactionCount_ShouldThrowException()
    {
        // Arrange
        _builder.WithGroupHeader("MSG123", DateTime.UtcNow, "2", 1000m)
                .AddReversalTransaction("REV123", "INST123", "E2E123", "TXN123");

        // Act
        _builder.Build();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Build_WithMismatchedControlSum_ShouldThrowException()
    {
        // Arrange
        var reversedAmount = new ActiveCurrencyAndAmount
        {
            Value = 500.00m,
            Ccy = "EUR"
        };

        _builder.WithGroupHeader("MSG123", DateTime.UtcNow, "1", 1000m) // Control sum 1000 but transaction amount is 500
                .AddReversalTransaction("REV123", "INST123", "E2E123", "TXN123", reversedAmount);

        // Act
        _builder.Build();
    }

    #endregion

    #region Clone Tests

    [TestMethod]
    public void Clone_ShouldCreateIdenticalBuilder()
    {
        // Arrange
        var messageId = "MSG12345";
        var creationDateTime = DateTime.UtcNow;
        var originalMessageId = "ORIG12345";
        var reversalId = "REV12345";

        _builder.SetMessageId(messageId, creationDateTime)
                .WithOriginalGroupInformation(originalMessageId, "pacs.008.001.10")
                .AddReversalTransaction(reversalId, "INST12345", "E2E12345", "TXN12345");

        // Act
        var clonedBuilder = _builder.Clone();

        // Assert
        Assert.AreNotSame(_builder, clonedBuilder);
        Assert.AreEqual(_builder.MessageType, clonedBuilder.MessageType);

        var originalDocument = _builder.Build();
        var clonedDocument = clonedBuilder.Build();

        Assert.AreEqual(originalDocument.FIToFIPmtRvsl.GrpHdr.MsgId, clonedDocument.FIToFIPmtRvsl.GrpHdr.MsgId);
        Assert.AreEqual(originalDocument.FIToFIPmtRvsl.OrgnlGrpInf.OrgnlMsgId, clonedDocument.FIToFIPmtRvsl.OrgnlGrpInf.OrgnlMsgId);
        Assert.AreEqual(originalDocument.FIToFIPmtRvsl.TxInf.Length, clonedDocument.FIToFIPmtRvsl.TxInf.Length);
        Assert.AreEqual(originalDocument.FIToFIPmtRvsl.TxInf[0].RvslId, clonedDocument.FIToFIPmtRvsl.TxInf[0].RvslId);
    }

    #endregion

    #region Fluent Interface Tests

    [TestMethod]
    public void FluentInterface_AllMethods_ShouldReturnBuilderInstance()
    {
        // Arrange
        var supplementaryData = new SupplementaryData1 { PlcAndNm = "Test" };

        // Act & Assert
        var result = _builder
            .SetMessageId("MSG123", DateTime.UtcNow)
            .WithGroupHeader("MSG123", DateTime.UtcNow, "1", 1000m)
            .SetInstructingAndInstructedAgents("BANKGB2L", "Bank UK", "BANKDEFF", "Bank DE")
            .WithOriginalGroupInformation("ORIG123", "pacs.008.001.10")
            .AddReversalTransaction("REV123", "INST123", "E2E123", "TXN123")
            .AddComprehensiveReversalTransaction("REV124", null, "INST124", "E2E124", "TXN124")
            .AddSupplementaryData(supplementaryData);

        Assert.AreSame(_builder, result);
    }

    #endregion
}
