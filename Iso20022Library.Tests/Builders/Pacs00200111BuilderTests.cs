using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Iso20022Library.Application.Builders.Pacs;
using Iso20022Library.Domain.Common.Enums;
using Iso20022Library.Messages.Payments.Pacs.Generated.Pacs00200111;

namespace Iso20022Library.Tests.Builders
{
    [TestClass]
    public class Pacs00200111BuilderTests
    {
        private Pacs00200111Builder _builder = null!;

        [TestInitialize]
        public void Setup()
        {
            _builder = new Pacs00200111Builder();
        }

        [TestMethod]
        public void Constructor_ShouldInitializeWithCorrectMessageType()
        {
            // Act & Assert
            Assert.AreEqual(MessageType.Pacs00200111, _builder.MessageType);
        }

        [TestMethod]
        public void WithGroupHeader_WithValidParameters_ShouldSetGroupHeader()
        {
            // Arrange
            var messageId = "MSG001";
            var creationDateTime = DateTime.Now;

            // Act
            var result = _builder.WithGroupHeader(messageId, creationDateTime);

            // Assert
            Assert.AreSame(_builder, result);
            var document = _builder.GetDocument();
            Assert.IsNotNull(document.FIToFIPmtStsRpt.GrpHdr);
            Assert.AreEqual(messageId, document.FIToFIPmtStsRpt.GrpHdr.MsgId);
            Assert.AreEqual(creationDateTime, document.FIToFIPmtStsRpt.GrpHdr.CreDtTm);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WithGroupHeader_WithNullMessageId_ShouldThrowArgumentException()
        {
            // Act
            _builder.WithGroupHeader(null!, DateTime.Now);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WithGroupHeader_WithEmptyMessageId_ShouldThrowArgumentException()
        {
            // Act
            _builder.WithGroupHeader("", DateTime.Now);
        }

        [TestMethod]
        public void WithGroupHeader_WithGroupHeaderObject_ShouldSetGroupHeader()
        {
            // Arrange
            var groupHeader = new GroupHeader91
            {
                MsgId = "MSG001",
                CreDtTm = DateTime.Now
            };

            // Act
            var result = _builder.WithGroupHeader(groupHeader);

            // Assert
            Assert.AreSame(_builder, result);
            var document = _builder.GetDocument();
            Assert.AreSame(groupHeader, document.FIToFIPmtStsRpt.GrpHdr);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WithGroupHeader_WithNullGroupHeader_ShouldThrowArgumentNullException()
        {
            // Act
            _builder.WithGroupHeader((GroupHeader91)null!);
        }

        [TestMethod]
        public void AddOriginalGroupInformationAndStatus_WithValidObject_ShouldAddOriginalGroupInfo()
        {
            // Arrange
            _builder.WithGroupHeader("MSG001", DateTime.Now);
            var originalGroupInfo = new OriginalGroupHeader17
            {
                OrgnlMsgId = "ORIG001",
                OrgnlMsgNmId = "pacs.008.001.09",
                OrgnlCreDtTm = DateTime.Now.AddHours(-1),
                OrgnlCreDtTmSpecified = true
            };

            // Act
            var result = _builder.AddOriginalGroupInformationAndStatus(originalGroupInfo);

            // Assert
            Assert.AreSame(_builder, result);
            var document = _builder.GetDocument();
            Assert.IsNotNull(document.FIToFIPmtStsRpt.OrgnlGrpInfAndSts);
            Assert.AreEqual(1, document.FIToFIPmtStsRpt.OrgnlGrpInfAndSts.Length);
            Assert.AreSame(originalGroupInfo, document.FIToFIPmtStsRpt.OrgnlGrpInfAndSts[0]);
        }

        [TestMethod]
        public void AddOriginalGroupInformationAndStatus_WithValidParameters_ShouldCreateAndAddOriginalGroupInfo()
        {
            // Arrange
            _builder.WithGroupHeader("MSG001", DateTime.Now);
            var originalGroupId = "ORIG001";
            var originalMessageId = "ORIG001";
            var originalCreationDateTime = DateTime.Now.AddHours(-1);

            // Act
            var result = _builder.AddOriginalGroupInformationAndStatus(originalGroupId, originalMessageId, originalCreationDateTime);

            // Assert
            Assert.AreSame(_builder, result);
            var document = _builder.GetDocument();
            Assert.IsNotNull(document.FIToFIPmtStsRpt.OrgnlGrpInfAndSts);
            Assert.AreEqual(1, document.FIToFIPmtStsRpt.OrgnlGrpInfAndSts.Length);
            Assert.AreEqual(originalGroupId, document.FIToFIPmtStsRpt.OrgnlGrpInfAndSts[0].OrgnlMsgId);
            Assert.AreEqual(originalMessageId, document.FIToFIPmtStsRpt.OrgnlGrpInfAndSts[0].OrgnlMsgId);
            Assert.AreEqual(originalCreationDateTime, document.FIToFIPmtStsRpt.OrgnlGrpInfAndSts[0].OrgnlCreDtTm);
            Assert.IsTrue(document.FIToFIPmtStsRpt.OrgnlGrpInfAndSts[0].OrgnlCreDtTmSpecified);
        }

        [TestMethod]
        public void AddOriginalGroupInformationAndStatus_WithMultipleGroups_ShouldAddAllGroups()
        {
            // Arrange
            _builder.WithGroupHeader("MSG001", DateTime.Now);
            var firstGroup = new OriginalGroupHeader17
            {
                OrgnlMsgId = "ORIG001",
                OrgnlMsgNmId = "pacs.008.001.09",
                OrgnlCreDtTm = DateTime.Now.AddHours(-1),
                OrgnlCreDtTmSpecified = true
            };
            var secondGroup = new OriginalGroupHeader17
            {
                OrgnlMsgId = "ORIG002",
                OrgnlMsgNmId = "pacs.008.001.09",
                OrgnlCreDtTm = DateTime.Now.AddHours(-2),
                OrgnlCreDtTmSpecified = true
            };

            // Act
            _builder.AddOriginalGroupInformationAndStatus(firstGroup)
                    .AddOriginalGroupInformationAndStatus(secondGroup);

            // Assert
            var document = _builder.GetDocument();
            Assert.IsNotNull(document.FIToFIPmtStsRpt.OrgnlGrpInfAndSts);
            Assert.AreEqual(2, document.FIToFIPmtStsRpt.OrgnlGrpInfAndSts.Length);
            Assert.AreSame(firstGroup, document.FIToFIPmtStsRpt.OrgnlGrpInfAndSts[0]);
            Assert.AreSame(secondGroup, document.FIToFIPmtStsRpt.OrgnlGrpInfAndSts[1]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddOriginalGroupInformationAndStatus_WithNullObject_ShouldThrowArgumentNullException()
        {
            // Arrange
            _builder.WithGroupHeader("MSG001", DateTime.Now);

            // Act
            _builder.AddOriginalGroupInformationAndStatus((OriginalGroupHeader17)null!);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddOriginalGroupInformationAndStatus_WithNullOriginalMessageId_ShouldThrowArgumentException()
        {
            // Arrange
            _builder.WithGroupHeader("MSG001", DateTime.Now);

            // Act
            _builder.AddOriginalGroupInformationAndStatus(null!, "pacs.008.001.09", DateTime.Now);
        }

        [TestMethod]
        public void AddTransactionInformationAndStatus_WithValidObject_ShouldAddTransaction()
        {
            // Arrange
            _builder.WithGroupHeader("MSG001", DateTime.Now);
            var transaction = new PaymentTransaction123
            {
                StsId = "STS001",
                OrgnlInstrId = "INST001",
                OrgnlEndToEndId = "E2E001",
                TxSts = "ACCC"
            };

            // Act
            var result = _builder.AddTransactionInformationAndStatus(transaction);

            // Assert
            Assert.AreSame(_builder, result);
            var document = _builder.GetDocument();
            Assert.IsNotNull(document.FIToFIPmtStsRpt.TxInfAndSts);
            Assert.AreEqual(1, document.FIToFIPmtStsRpt.TxInfAndSts.Length);
            Assert.AreSame(transaction, document.FIToFIPmtStsRpt.TxInfAndSts[0]);
        }

        [TestMethod]
        public void AddTransactionInformationAndStatus_WithValidParameters_ShouldCreateAndAddTransaction()
        {
            // Arrange
            _builder.WithGroupHeader("MSG001", DateTime.Now);
            var statusId = "STS001";
            var originalInstructionId = "INST001";
            var originalEndToEndId = "E2E001";
            var transactionStatus = "ACCC";

            // Act
            var result = _builder.AddTransactionInformationAndStatus(statusId, originalInstructionId, originalEndToEndId,
                originalTransactionId: null, transactionStatus);

            // Assert
            Assert.AreSame(_builder, result);
            var document = _builder.GetDocument();
            Assert.IsNotNull(document.FIToFIPmtStsRpt.TxInfAndSts);
            Assert.AreEqual(1, document.FIToFIPmtStsRpt.TxInfAndSts.Length);
            Assert.AreEqual(statusId, document.FIToFIPmtStsRpt.TxInfAndSts[0].StsId);
            Assert.AreEqual(originalInstructionId, document.FIToFIPmtStsRpt.TxInfAndSts[0].OrgnlInstrId);
            Assert.AreEqual(originalEndToEndId, document.FIToFIPmtStsRpt.TxInfAndSts[0].OrgnlEndToEndId);
            Assert.AreEqual(transactionStatus, document.FIToFIPmtStsRpt.TxInfAndSts[0].TxSts);
        }

        [TestMethod]
        public void AddTransactionInformationAndStatus_WithMultipleTransactions_ShouldAddAllTransactions()
        {
            // Arrange
            _builder.WithGroupHeader("MSG001", DateTime.Now);

            // Act
            _builder.AddTransactionInformationAndStatus("STS001", "INST001", "E2E001",
                originalTransactionId: null, "ACCC")
                    .AddTransactionInformationAndStatus("STS002", "INST002", "E2E002",
                originalTransactionId: null, "RJCT");

            // Assert
            var document = _builder.GetDocument();
            Assert.IsNotNull(document.FIToFIPmtStsRpt.TxInfAndSts);
            Assert.AreEqual(2, document.FIToFIPmtStsRpt.TxInfAndSts.Length);
            Assert.AreEqual("STS001", document.FIToFIPmtStsRpt.TxInfAndSts[0].StsId);
            Assert.AreEqual("STS002", document.FIToFIPmtStsRpt.TxInfAndSts[1].StsId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddTransactionInformationAndStatus_WithNullTransaction_ShouldThrowArgumentNullException()
        {
            // Arrange
            _builder.WithGroupHeader("MSG001", DateTime.Now);

            // Act
            _builder.AddTransactionInformationAndStatus((PaymentTransaction123)null!);
        }

        [TestMethod]
        public void AddTransactionInformationAndStatus_WithNullStatusId_ShouldNotThrowException()
        {
            // Arrange
            _builder.WithGroupHeader("MSG001", DateTime.Now);

            // Act & Assert - This should not throw an exception as statusId is optional
            var result = _builder.AddTransactionInformationAndStatus(null, "INST001", "E2E001",
                originalTransactionId: null, "ACCC");
            Assert.AreSame(_builder, result);
        }

        [TestMethod]
        public void AddSupplementaryData_WithValidData_ShouldAddSupplementaryData()
        {
            // Arrange
            _builder.WithGroupHeader("MSG001", DateTime.Now);
            var xmlDoc = new XmlDocument();
            var element = xmlDoc.CreateElement("TestData");
            element.InnerText = "Test Content";
            var placementAndDate = "Test Location";

            // Act
            var result = _builder.AddSupplementaryData(element, placementAndDate);

            // Assert
            Assert.AreSame(_builder, result);
            var document = _builder.GetDocument();
            Assert.IsNotNull(document.FIToFIPmtStsRpt.SplmtryData);
            Assert.AreEqual(1, document.FIToFIPmtStsRpt.SplmtryData.Length);
            Assert.AreEqual(placementAndDate, document.FIToFIPmtStsRpt.SplmtryData[0].PlcAndNm);
            Assert.IsNotNull(document.FIToFIPmtStsRpt.SplmtryData[0].Envlp);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddSupplementaryData_WithNullEnvelope_ShouldThrowArgumentNullException()
        {
            // Arrange
            _builder.WithGroupHeader("MSG001", DateTime.Now);

            // Act
            _builder.AddSupplementaryData((System.Xml.XmlElement)null!);
        }

        [TestMethod]
        public void AddSupplementaryData_WithSupplementaryDataObject_ShouldAddSupplementaryData()
        {
            // Arrange
            _builder.WithGroupHeader("MSG001", DateTime.Now);
            var xmlDoc = new XmlDocument();
            var element = xmlDoc.CreateElement("TestData");
            var supplementaryData = new SupplementaryData1
            {
                PlcAndNm = "Test Location",
                Envlp = element
            };

            // Act
            var result = _builder.AddSupplementaryData(supplementaryData);

            // Assert
            Assert.AreSame(_builder, result);
            var document = _builder.GetDocument();
            Assert.IsNotNull(document.FIToFIPmtStsRpt.SplmtryData);
            Assert.AreEqual(1, document.FIToFIPmtStsRpt.SplmtryData.Length);
            Assert.AreEqual("Test Location", document.FIToFIPmtStsRpt.SplmtryData[0].PlcAndNm);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddSupplementaryData_WithNullSupplementaryData_ShouldThrowArgumentNullException()
        {
            // Arrange
            _builder.WithGroupHeader("MSG001", DateTime.Now);

            // Act
            _builder.AddSupplementaryData((SupplementaryData1)null!);
        }

        [TestMethod]
        public void CreateNumberOfTransactionsPerStatus_WithValidParameters_ShouldCreateCorrectObject()
        {
            // Arrange
            var numberOfTransactions = "5";
            var status = "ACCC";
            var controlSum = 1000.50m;

            // Act
            var result = Pacs00200111Builder.CreateNumberOfTransactionsPerStatus(numberOfTransactions, status, controlSum);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(numberOfTransactions, result.DtldNbOfTxs);
            Assert.AreEqual(status, result.DtldSts);
            Assert.AreEqual(controlSum, result.DtldCtrlSum);
            Assert.IsTrue(result.DtldCtrlSumSpecified);
        }

        [TestMethod]
        public void CreateNumberOfTransactionsPerStatus_WithoutControlSum_ShouldNotSetControlSum()
        {
            // Arrange
            var numberOfTransactions = "5";
            var status = "ACCC";

            // Act
            var result = Pacs00200111Builder.CreateNumberOfTransactionsPerStatus(numberOfTransactions, status);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(numberOfTransactions, result.DtldNbOfTxs);
            Assert.AreEqual(status, result.DtldSts);
            Assert.IsFalse(result.DtldCtrlSumSpecified);
        }

        [TestMethod]
        public void GetDocument_ShouldReturnValidDocument()
        {
            // Arrange
            _builder.WithGroupHeader("MSG001", DateTime.Now);

            // Act
            var document = _builder.GetDocument();

            // Assert
            Assert.IsNotNull(document);
            Assert.IsNotNull(document.FIToFIPmtStsRpt);
            Assert.IsNotNull(document.FIToFIPmtStsRpt.GrpHdr);
            Assert.AreEqual("MSG001", document.FIToFIPmtStsRpt.GrpHdr.MsgId);
        }

        [TestMethod]
        public void GetReport_ShouldReturnValidReport()
        {
            // Arrange
            _builder.WithGroupHeader("MSG001", DateTime.Now);

            // Act
            var report = _builder.GetReport();

            // Assert
            Assert.IsNotNull(report);
            Assert.IsNotNull(report.GrpHdr);
            Assert.AreEqual("MSG001", report.GrpHdr.MsgId);
        }

        [TestMethod]
        public void Clone_ShouldCreateDeepCopy()
        {
            // Arrange
            var originalGroup = new OriginalGroupHeader17
            {
                OrgnlMsgId = "ORIG001",
                OrgnlMsgNmId = "pacs.008.001.09",
                OrgnlCreDtTm = DateTime.Now.AddHours(-1),
                OrgnlCreDtTmSpecified = true
            };
            var transaction = new PaymentTransaction123
            {
                StsId = "STS001",
                OrgnlInstrId = "INST001",
                OrgnlEndToEndId = "E2E001",
                TxSts = "ACCC"
            };

            _builder.WithGroupHeader("MSG001", DateTime.Now)
                    .AddOriginalGroupInformationAndStatus(originalGroup)
                    .AddTransactionInformationAndStatus(transaction);

            // Act
            var clonedBuilder = _builder.Clone();

            // Assert
            Assert.AreNotSame(_builder, clonedBuilder);
            Assert.AreEqual(_builder.MessageType, clonedBuilder.MessageType);

            // Verify deep copy by comparing documents
            var originalDocument = _builder.GetDocument();
            var clonedDocument = clonedBuilder.GetDocument();

            Assert.AreNotSame(originalDocument, clonedDocument);
            Assert.AreEqual(originalDocument.FIToFIPmtStsRpt.GrpHdr.MsgId, clonedDocument.FIToFIPmtStsRpt.GrpHdr.MsgId);

            if (originalDocument.FIToFIPmtStsRpt.OrgnlGrpInfAndSts != null && clonedDocument.FIToFIPmtStsRpt.OrgnlGrpInfAndSts != null)
            {
                Assert.AreEqual(originalDocument.FIToFIPmtStsRpt.OrgnlGrpInfAndSts.Length, clonedDocument.FIToFIPmtStsRpt.OrgnlGrpInfAndSts.Length);
            }

            if (originalDocument.FIToFIPmtStsRpt.TxInfAndSts != null && clonedDocument.FIToFIPmtStsRpt.TxInfAndSts != null)
            {
                Assert.AreEqual(originalDocument.FIToFIPmtStsRpt.TxInfAndSts.Length, clonedDocument.FIToFIPmtStsRpt.TxInfAndSts.Length);
            }
        }

        [TestMethod]
        public void GetOriginalGroupInformationCount_ShouldReturnCorrectCount()
        {
            // Arrange
            _builder.WithGroupHeader("MSG001", DateTime.Now);

            // Act & Assert - Initially zero
            Assert.AreEqual(0, _builder.GetOriginalGroupInformationCount());

            // Add one group information
            _builder.AddOriginalGroupInformationAndStatus("ORIG001", "ORIG001", DateTime.Now.AddHours(-1));
            Assert.AreEqual(1, _builder.GetOriginalGroupInformationCount());

            // Add another group information
            _builder.AddOriginalGroupInformationAndStatus("ORIG002", "ORIG002", DateTime.Now.AddHours(-2));
            Assert.AreEqual(2, _builder.GetOriginalGroupInformationCount());
        }

        [TestMethod]
        public void GetTransactionInformationCount_ShouldReturnCorrectCount()
        {
            // Arrange
            _builder.WithGroupHeader("MSG001", DateTime.Now);

            // Act & Assert - Initially zero
            Assert.AreEqual(0, _builder.GetTransactionInformationCount());

            // Add one transaction
            _builder.AddTransactionInformationAndStatus("STS001", "INST001", "E2E001",
                originalTransactionId: null, "ACCC");
            Assert.AreEqual(1, _builder.GetTransactionInformationCount());

            // Add another transaction
            _builder.AddTransactionInformationAndStatus("STS002", "INST002", "E2E002",
                originalTransactionId: null, "RJCT");
            Assert.AreEqual(2, _builder.GetTransactionInformationCount());
        }

        [TestMethod]
        public void BuildXml_WithValidDocument_ShouldGenerateXml()
        {
            // Arrange
            _builder.WithGroupHeader("MSG001", DateTime.Now)
                    .AddTransactionInformationAndStatus("STS001", "INST001", "E2E001",
                        originalTransactionId: null, "ACCC");

            // Act
            var xml = _builder.BuildXml();

            // Assert
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.Length > 0);
            Assert.IsTrue(xml.Contains("FIToFIPmtStsRpt"));
            Assert.IsTrue(xml.Contains("MSG001"));
        }

        [TestMethod]
        public void ComplexScenario_WithAllFeatures_ShouldBuildCorrectDocument()
        {
            // Arrange
            var messageId = "MSG001";
            var creationDateTime = new DateTime(2024, 1, 15, 10, 30, 0);

            var xmlDoc = new XmlDocument();
            var element = xmlDoc.CreateElement("CustomData");
            element.InnerText = "Custom Content";

            // Act
            var document = _builder
                .WithGroupHeader(messageId, creationDateTime)
                .AddOriginalGroupInformationAndStatus("ORIG001", "ORIG001", creationDateTime.AddHours(-1))
                .AddOriginalGroupInformationAndStatus("ORIG002", "ORIG002", creationDateTime.AddHours(-2))
                .AddTransactionInformationAndStatus("STS001", "INST001", "E2E001",
                    originalTransactionId: null, "ACCC")
                .AddTransactionInformationAndStatus("STS002", "INST002", "E2E002",
                    originalTransactionId: null, "RJCT")
                .AddTransactionInformationAndStatus("STS003", "INST003", "E2E003",
                    originalTransactionId: null, "PDNG")
                .AddSupplementaryData(element, "Custom Location")
                .GetDocument();

            // Assert
            Assert.IsNotNull(document);
            Assert.IsNotNull(document.FIToFIPmtStsRpt);

            // Verify group header
            var groupHeader = document.FIToFIPmtStsRpt.GrpHdr;
            Assert.IsNotNull(groupHeader);
            Assert.AreEqual(messageId, groupHeader.MsgId);
            Assert.AreEqual(creationDateTime, groupHeader.CreDtTm);

            // Verify original group information
            Assert.IsNotNull(document.FIToFIPmtStsRpt.OrgnlGrpInfAndSts);
            Assert.AreEqual(2, document.FIToFIPmtStsRpt.OrgnlGrpInfAndSts.Length);
            Assert.AreEqual("ORIG001", document.FIToFIPmtStsRpt.OrgnlGrpInfAndSts[0].OrgnlMsgId);
            Assert.AreEqual("ORIG002", document.FIToFIPmtStsRpt.OrgnlGrpInfAndSts[1].OrgnlMsgId);

            // Verify transaction information
            Assert.IsNotNull(document.FIToFIPmtStsRpt.TxInfAndSts);
            Assert.AreEqual(3, document.FIToFIPmtStsRpt.TxInfAndSts.Length);
            Assert.AreEqual("STS001", document.FIToFIPmtStsRpt.TxInfAndSts[0].StsId);
            Assert.AreEqual("ACCC", document.FIToFIPmtStsRpt.TxInfAndSts[0].TxSts);
            Assert.AreEqual("STS002", document.FIToFIPmtStsRpt.TxInfAndSts[1].StsId);
            Assert.AreEqual("RJCT", document.FIToFIPmtStsRpt.TxInfAndSts[1].TxSts);
            Assert.AreEqual("STS003", document.FIToFIPmtStsRpt.TxInfAndSts[2].StsId);
            Assert.AreEqual("PDNG", document.FIToFIPmtStsRpt.TxInfAndSts[2].TxSts);

            // Verify supplementary data
            Assert.IsNotNull(document.FIToFIPmtStsRpt.SplmtryData);
            Assert.AreEqual(1, document.FIToFIPmtStsRpt.SplmtryData.Length);
            Assert.AreEqual("Custom Location", document.FIToFIPmtStsRpt.SplmtryData[0].PlcAndNm);
            Assert.IsNotNull(document.FIToFIPmtStsRpt.SplmtryData[0].Envlp);
        }
    }
}
