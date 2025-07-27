using Iso20022Library.Application.Builders.Pain;
using Iso20022Library.Messages.Payments.Pain.Generated.Pain00200107;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;

namespace Iso20022Library.Tests.Builders
{
    /// <summary>
    /// Unit tests for the Pain00200107Builder class.
    /// </summary>
    /// <remarks>
    /// These tests validate the functionality of the ISO 20022 pain.002.001.07 
    /// (Customer Payment Status Report V07) message builder, ensuring proper construction,
    /// validation, and XML serialization of payment status report messages.
    /// </remarks>
    [TestClass]
    public class Pain00200107BuilderTests
    {
        private Pain00200107Builder _builder = null!;

        /// <summary>
        /// Initializes the test environment before each test method.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            _builder = new Pain00200107Builder();
        }

        /// <summary>
        /// Tests that the builder initializes correctly with default values.
        /// </summary>
        [TestMethod]
        public void Constructor_ShouldInitializeCorrectly()
        {
            // Arrange & Act
            var builder = new Pain00200107Builder();

            // Assert
            Assert.IsNotNull(builder);
            Assert.AreEqual(0, builder.GetOriginalPaymentInstructionCount());
            Assert.AreEqual(0, builder.GetSupplementaryDataCount());
        }

        /// <summary>
        /// Tests successful creation of a complete pain.002.001.07 message.
        /// </summary>
        [TestMethod]
        public void WithMethods_CompleteMessage_ShouldBuildCorrectly()
        {
            // Arrange
            var groupHeader = CreateTestGroupHeader52();
            var originalGroupHeader = CreateTestOriginalGroupHeader1();
            var originalPaymentInstruction = CreateTestOriginalPaymentInstruction18();

            // Act
            _builder
                .WithGroupHeader(groupHeader)
                .WithOriginalGroupInformationAndStatus(originalGroupHeader)
                .AddOriginalPaymentInstructionAndStatus(originalPaymentInstruction);

            var xml = _builder.BuildXml();

            // Assert
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.Contains("<?xml version=\"1.0\" encoding=\"utf-8\"?>"));
            Assert.IsTrue(xml.Contains("<Document"));
            Assert.IsTrue(xml.Contains("<CstmrPmtStsRpt>"));
            Assert.IsTrue(xml.Contains("MSG20240101001"));
        }

        /// <summary>
        /// Tests adding multiple original payment instruction information entries.
        /// </summary>
        [TestMethod]
        public void AddOriginalPaymentInstructionAndStatus_MultipleEntries_ShouldAcceptAll()
        {
            // Arrange
            var originalPaymentInfo1 = CreateTestOriginalPaymentInstruction18("ORIG001");
            var originalPaymentInfo2 = CreateTestOriginalPaymentInstruction18("ORIG002");

            // Act
            _builder
                .AddOriginalPaymentInstructionAndStatus(originalPaymentInfo1)
                .AddOriginalPaymentInstructionAndStatus(originalPaymentInfo2);

            // Assert
            Assert.AreEqual(2, _builder.GetOriginalPaymentInstructionCount());
        }

        /// <summary>
        /// Tests adding supplementary data to the message.
        /// </summary>
        [TestMethod]
        public void AddSupplementaryData_ValidData_ShouldAccept()
        {
            // Arrange
            var supplementaryData = CreateTestSupplementaryData1();

            // Act
            _builder.AddSupplementaryData(supplementaryData);

            // Assert
            Assert.AreEqual(1, _builder.GetSupplementaryDataCount());
        }

        /// <summary>
        /// Tests that null arguments are properly validated.
        /// </summary>
        [TestMethod]
        public void WithMethods_NullArguments_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() => _builder.WithGroupHeader(null!));
            Assert.ThrowsException<ArgumentNullException>(() => _builder.WithOriginalGroupInformationAndStatus(null!));
            Assert.ThrowsException<ArgumentNullException>(() => _builder.AddOriginalPaymentInstructionAndStatus(null!));
            Assert.ThrowsException<ArgumentNullException>(() => _builder.AddSupplementaryData(null!));
        }

        /// <summary>
        /// Tests BuildXml method with valid internal document.
        /// </summary>
        [TestMethod]
        public void BuildXml_ValidInternalDocument_ShouldReturnXmlString()
        {
            // Arrange
            var groupHeader = CreateTestGroupHeader52();
            var originalGroupHeader = CreateTestOriginalGroupHeader1();

            _builder
                .WithGroupHeader(groupHeader)
                .WithOriginalGroupInformationAndStatus(originalGroupHeader);

            // Act  
            var xml = _builder.BuildXml();

            // Assert
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.Contains("<?xml version=\"1.0\" encoding=\"utf-8\"?>"));
            Assert.IsTrue(xml.Contains("<Document"));
            Assert.IsTrue(xml.Contains("<CstmrPmtStsRpt>"));
            Assert.IsTrue(xml.Contains("MSG20240101001"));
        }

        /// <summary>
        /// Tests BuildXml method with external document parameter.
        /// </summary>
        [TestMethod]
        public void BuildXml_ValidExternalDocument_ShouldReturnXmlString()
        {
            // Arrange
            var document = CreateCompleteDocument();

            // Act  
            var xml = _builder.BuildXml(document);

            // Assert
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.Contains("<?xml version=\"1.0\" encoding=\"utf-8\"?>"));
            Assert.IsTrue(xml.Contains("<Document"));
            Assert.IsTrue(xml.Contains("<CstmrPmtStsRpt>"));
            Assert.IsTrue(xml.Contains("MSG20240101001"));
        }

        /// <summary>
        /// Tests BuildXml method with null document.
        /// </summary>
        [TestMethod]
        public void BuildXml_NullDocument_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() => _builder.BuildXml(null!));
        }

        /// <summary>
        /// Tests BuildXml method with invalid document type.
        /// </summary>
        [TestMethod]
        public void BuildXml_InvalidDocumentType_ShouldThrowInvalidCastException()
        {
            // Assert
            Assert.ThrowsException<InvalidCastException>(() => _builder.BuildXml("invalid document"));
        }

        /// <summary>
        /// Tests the fluent interface pattern implementation.
        /// </summary>
        [TestMethod]
        public void FluentInterface_AllMethods_ShouldReturnBuilderInstance()
        {
            // Arrange
            var groupHeader = CreateTestGroupHeader52();
            var originalGroupHeader = CreateTestOriginalGroupHeader1();
            var originalPaymentInfo = CreateTestOriginalPaymentInstruction18();
            var supplementaryData = CreateTestSupplementaryData1();

            // Act & Assert
            var result = _builder
                .WithGroupHeader(groupHeader)
                .WithOriginalGroupInformationAndStatus(originalGroupHeader)
                .AddOriginalPaymentInstructionAndStatus(originalPaymentInfo)
                .AddSupplementaryData(supplementaryData);

            Assert.AreSame(_builder, result, "All fluent methods should return the same builder instance");
        }

        /// <summary>
        /// Tests that duplicate original payment instruction information can be added.
        /// </summary>
        [TestMethod]
        public void AddOriginalPaymentInstructionAndStatus_DuplicateEntries_ShouldAcceptAll()
        {
            // Arrange
            var originalPaymentInfo = CreateTestOriginalPaymentInstruction18();

            // Act
            _builder
                .AddOriginalPaymentInstructionAndStatus(originalPaymentInfo)
                .AddOriginalPaymentInstructionAndStatus(originalPaymentInfo);

            // Assert
            Assert.AreEqual(2, _builder.GetOriginalPaymentInstructionCount());
        }

        /// <summary>
        /// Tests that duplicate supplementary data can be added.
        /// </summary>
        [TestMethod]
        public void AddSupplementaryData_DuplicateEntries_ShouldAcceptAll()
        {
            // Arrange
            var supplementaryData = CreateTestSupplementaryData1();

            // Act
            _builder
                .AddSupplementaryData(supplementaryData)
                .AddSupplementaryData(supplementaryData);

            // Assert
            Assert.AreEqual(2, _builder.GetSupplementaryDataCount());
        }

        /// <summary>
        /// Tests building with minimal required fields only.
        /// </summary>
        [TestMethod]
        public void BuildXml_MinimalRequiredFields_ShouldSucceed()
        {
            // Arrange
            var groupHeader = CreateTestGroupHeader52();
            var originalGroupHeader = CreateTestOriginalGroupHeader1();

            // Act
            var xml = _builder
                .WithGroupHeader(groupHeader)
                .WithOriginalGroupInformationAndStatus(originalGroupHeader)
                .BuildXml();

            // Assert
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.Contains("MSG20240101001"));
        }

        /// <summary>
        /// Tests building with supplementary data included.
        /// </summary>
        [TestMethod]
        public void BuildXml_WithSupplementaryData_ShouldIncludeData()
        {
            // Arrange
            var groupHeader = CreateTestGroupHeader52();
            var originalGroupHeader = CreateTestOriginalGroupHeader1();
            var supplementaryData = CreateTestSupplementaryData1();

            // Act
            var xml = _builder
                .WithGroupHeader(groupHeader)
                .WithOriginalGroupInformationAndStatus(originalGroupHeader)
                .AddSupplementaryData(supplementaryData)
                .BuildXml();

            // Assert
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.Contains("TestLocation"));
        }

        /// <summary>
        /// Tests that BuildXml throws InvalidOperationException when validation fails.
        /// </summary>
        [TestMethod]
        public void BuildXml_InvalidState_ShouldThrowInvalidOperationException()
        {
            // Arrange - builder without required fields

            // Act & Assert
            Assert.ThrowsException<InvalidOperationException>(() => _builder.BuildXml());
        }

        /// <summary>
        /// Tests clearing original payment instructions.
        /// </summary>
        [TestMethod]
        public void ClearOriginalPaymentInstructions_ShouldRemoveAllInstructions()
        {
            // Arrange
            var originalPaymentInfo = CreateTestOriginalPaymentInstruction18();
            _builder.AddOriginalPaymentInstructionAndStatus(originalPaymentInfo);

            // Act
            _builder.ClearOriginalPaymentInstructions();

            // Assert
            Assert.AreEqual(0, _builder.GetOriginalPaymentInstructionCount());
        }

        /// <summary>
        /// Tests clearing supplementary data.
        /// </summary>
        [TestMethod]
        public void ClearSupplementaryData_ShouldRemoveAllData()
        {
            // Arrange
            var supplementaryData = CreateTestSupplementaryData1();
            _builder.AddSupplementaryData(supplementaryData);

            // Act
            _builder.ClearSupplementaryData();

            // Assert
            Assert.AreEqual(0, _builder.GetSupplementaryDataCount());
        }

        /// <summary>
        /// Tests the Reset method functionality.
        /// </summary>
        [TestMethod]
        public void Reset_ShouldClearAllData()
        {
            // Arrange
            var groupHeader = CreateTestGroupHeader52();
            var originalGroupHeader = CreateTestOriginalGroupHeader1();
            var originalPaymentInfo = CreateTestOriginalPaymentInstruction18();
            var supplementaryData = CreateTestSupplementaryData1();

            _builder
                .WithGroupHeader(groupHeader)
                .WithOriginalGroupInformationAndStatus(originalGroupHeader)
                .AddOriginalPaymentInstructionAndStatus(originalPaymentInfo)
                .AddSupplementaryData(supplementaryData);

            // Act
            _builder.Reset();

            // Assert
            Assert.AreEqual(0, _builder.GetOriginalPaymentInstructionCount());
            Assert.AreEqual(0, _builder.GetSupplementaryDataCount());
        }

        /// <summary>
        /// Tests the Clone method functionality.
        /// </summary>
        [TestMethod]
        public void Clone_ShouldCreateIdenticalBuilder()
        {
            // Arrange
            var groupHeader = CreateTestGroupHeader52();
            var originalGroupHeader = CreateTestOriginalGroupHeader1();
            var originalPaymentInfo = CreateTestOriginalPaymentInstruction18();
            var supplementaryData = CreateTestSupplementaryData1();

            _builder
                .WithGroupHeader(groupHeader)
                .WithOriginalGroupInformationAndStatus(originalGroupHeader)
                .AddOriginalPaymentInstructionAndStatus(originalPaymentInfo)
                .AddSupplementaryData(supplementaryData);

            // Act
            var clonedBuilder = _builder.Clone();

            // Assert
            Assert.AreNotSame(_builder, clonedBuilder);
            Assert.AreEqual(_builder.GetOriginalPaymentInstructionCount(), clonedBuilder.GetOriginalPaymentInstructionCount());
            Assert.AreEqual(_builder.GetSupplementaryDataCount(), clonedBuilder.GetSupplementaryDataCount());

            var originalXml = _builder.BuildXml();
            var clonedXml = clonedBuilder.BuildXml();
            Assert.AreEqual(originalXml, clonedXml);
        }

        #region Helper Methods

        /// <summary>
        /// Creates a test GroupHeader52 instance.
        /// </summary>
        /// <returns>A configured GroupHeader52 instance.</returns>
        private static GroupHeader52 CreateTestGroupHeader52()
        {
            return new GroupHeader52
            {
                MsgId = "MSG20240101001",
                CreDtTm = new DateTime(2024, 1, 1, 10, 0, 0, DateTimeKind.Utc),
                InitgPty = new PartyIdentification43
                {
                    Nm = "Test Bank"
                }
            };
        }

        /// <summary>
        /// Creates a test OriginalGroupHeader1 instance.
        /// </summary>
        /// <returns>A configured OriginalGroupHeader1 instance.</returns>
        private static OriginalGroupHeader1 CreateTestOriginalGroupHeader1()
        {
            return new OriginalGroupHeader1
            {
                OrgnlMsgId = "ORIG20240101001",
                OrgnlMsgNmId = "pain.001.001.03",
                OrgnlCreDtTm = new DateTime(2024, 1, 1, 9, 0, 0, DateTimeKind.Utc),
                OrgnlCreDtTmSpecified = true
            };
        }

        /// <summary>
        /// Creates a test OriginalPaymentInstruction18 instance.
        /// </summary>
        /// <param name="instructionId">Optional instruction ID. Defaults to "ORIG001".</param>
        /// <returns>A configured OriginalPaymentInstruction18 instance.</returns>
        private static OriginalPaymentInstruction18 CreateTestOriginalPaymentInstruction18(string instructionId = "ORIG001")
        {
            return new OriginalPaymentInstruction18
            {
                OrgnlPmtInfId = instructionId,
                OrgnlNbOfTxs = "1",
                OrgnlCtrlSum = 1000.00m,
                OrgnlCtrlSumSpecified = true,
                PmtInfSts = TransactionGroupStatus3Code.Acsc
            };
        }

        /// <summary>
        /// Creates a test SupplementaryData1 instance.
        /// </summary>
        /// <returns>A configured SupplementaryData1 instance.</returns>
        private static SupplementaryData1 CreateTestSupplementaryData1()
        {
            return new SupplementaryData1
            {
                PlcAndNm = "TestLocation",
                Envlp = new SupplementaryDataEnvelope1
                {
                    Any = new System.Xml.XmlDocument().CreateElement("TestData")
                }
            };
        }

        /// <summary>
        /// Creates a complete Document instance for testing.
        /// </summary>
        /// <returns>A fully configured Document instance.</returns>
        private Document CreateCompleteDocument()
        {
            var document = new Document
            {
                CstmrPmtStsRpt = new CustomerPaymentStatusReportV07
                {
                    GrpHdr = CreateTestGroupHeader52(),
                    OrgnlGrpInfAndSts = CreateTestOriginalGroupHeader1()
                }
            };

            document.CstmrPmtStsRpt.OrgnlPmtInfAndSts.Add(CreateTestOriginalPaymentInstruction18());

            return document;
        }

        #endregion
    }
}
