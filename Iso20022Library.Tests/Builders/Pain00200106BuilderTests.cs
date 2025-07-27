using Iso20022Library.Application.Builders.Pain;
using Iso20022Library.Messages.Payments.Pain.Generated.Pain00200106;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;

namespace Iso20022Library.Tests.Builders
{
    /// <summary>
    /// Unit tests for the Pain00200106Builder class.
    /// </summary>
    /// <remarks>
    /// These tests validate the functionality of the ISO 20022 pain.002.001.06 
    /// (Customer Payment Status Report V06) message builder, ensuring proper construction,
    /// validation, and XML serialization of payment status report messages.
    /// </remarks>
    [TestClass]
    public class Pain00200106BuilderTests
    {
        private Pain00200106Builder _builder = null!;

        /// <summary>
        /// Initializes the test environment before each test method.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            _builder = new Pain00200106Builder();
        }

        /// <summary>
        /// Tests that the builder initializes correctly with default values.
        /// </summary>
        [TestMethod]
        public void Constructor_ShouldInitializeCorrectly()
        {
            // Arrange & Act
            var builder = new Pain00200106Builder();

            // Assert
            Assert.IsNotNull(builder);
            Assert.IsFalse(builder.IsValid(), "Builder should not be valid without required fields");
            Assert.AreEqual(0, builder.GetOriginalPaymentInstructionCount());
            Assert.AreEqual(0, builder.GetSupplementaryDataCount());
        }

        /// <summary>
        /// Tests successful creation of a complete pain.002.001.06 message.
        /// </summary>
        [TestMethod]
        public void BuildCompleteMessage_ShouldCreateValidDocument()
        {
            // Arrange
            var groupHeader = CreateSampleGroupHeader();
            var originalGroupHeader = CreateSampleOriginalGroupHeader();
            var originalPaymentInstruction = CreateSampleOriginalPaymentInstruction();

            // Act
            var document = _builder
                .WithGroupHeader(groupHeader)
                .WithOriginalGroupInformationAndStatus(originalGroupHeader)
                .AddOriginalPaymentInstructionAndStatus(originalPaymentInstruction)
                .Build();

            // Assert
            Assert.IsNotNull(document);
            Assert.IsNotNull(document.CstmrPmtStsRpt);
            Assert.AreSame(groupHeader, document.CstmrPmtStsRpt.GrpHdr);
            Assert.AreSame(originalGroupHeader, document.CstmrPmtStsRpt.OrgnlGrpInfAndSts);
            Assert.AreEqual(1, document.CstmrPmtStsRpt.OrgnlPmtInfAndSts.Count);
            Assert.AreSame(originalPaymentInstruction, document.CstmrPmtStsRpt.OrgnlPmtInfAndSts[0]);
        }

        /// <summary>
        /// Tests XML serialization of a complete pain.002.001.06 message.
        /// </summary>
        [TestMethod]
        public void BuildXml_WithValidData_ShouldReturnXmlString()
        {
            // Arrange
            var groupHeader = CreateSampleGroupHeader();
            var originalGroupHeader = CreateSampleOriginalGroupHeader();

            // Act
            _builder
                .WithGroupHeader(groupHeader)
                .WithOriginalGroupInformationAndStatus(originalGroupHeader);

            var xml = _builder.BuildXml();

            // Assert
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.Contains("<?xml version=\"1.0\" encoding=\"utf-8\"?>"));
            Assert.IsTrue(xml.Contains("CstmrPmtStsRpt"));
            Assert.IsTrue(xml.Contains("MSG20250726001"));
            Assert.IsTrue(xml.Contains("SAMPLE_BANK_V06"));
        }

        /// <summary>
        /// Tests that adding multiple original payment instructions works correctly.
        /// </summary>
        [TestMethod]
        public void AddMultipleOriginalPaymentInstructions_ShouldAddAllInstructions()
        {
            // Arrange
            var instructions = new Collection<OriginalPaymentInstruction12>
            {
                CreateSampleOriginalPaymentInstruction("PMT001"),
                CreateSampleOriginalPaymentInstruction("PMT002"),
                CreateSampleOriginalPaymentInstruction("PMT003")
            };

            // Act
            _builder
                .WithGroupHeader(CreateSampleGroupHeader())
                .WithOriginalGroupInformationAndStatus(CreateSampleOriginalGroupHeader())
                .AddOriginalPaymentInstructionsAndStatus(instructions);

            var document = _builder.Build();

            // Assert
            Assert.AreEqual(3, document.CstmrPmtStsRpt.OrgnlPmtInfAndSts.Count);
            Assert.AreEqual(3, _builder.GetOriginalPaymentInstructionCount());
        }

        /// <summary>
        /// Tests adding supplementary data to the message.
        /// </summary>
        [TestMethod]
        public void AddSupplementaryData_ShouldAddDataToMessage()
        {
            // Arrange
            var supplementaryData = CreateSampleSupplementaryData();

            // Act
            _builder
                .WithGroupHeader(CreateSampleGroupHeader())
                .WithOriginalGroupInformationAndStatus(CreateSampleOriginalGroupHeader())
                .AddSupplementaryData(supplementaryData);

            var document = _builder.Build();

            // Assert
            Assert.AreEqual(1, document.CstmrPmtStsRpt.SplmtryData.Count);
            Assert.AreEqual(1, _builder.GetSupplementaryDataCount());
            Assert.AreSame(supplementaryData, document.CstmrPmtStsRpt.SplmtryData[0]);
        }

        /// <summary>
        /// Tests adding multiple supplementary data entries.
        /// </summary>
        [TestMethod]
        public void AddSupplementaryDataCollection_ShouldAddAllEntries()
        {
            // Arrange
            var supplementaryDataCollection = new Collection<SupplementaryData1>
            {
                CreateSampleSupplementaryData("DATA001"),
                CreateSampleSupplementaryData("DATA002")
            };

            // Act
            _builder
                .WithGroupHeader(CreateSampleGroupHeader())
                .WithOriginalGroupInformationAndStatus(CreateSampleOriginalGroupHeader())
                .AddSupplementaryDataCollection(supplementaryDataCollection);

            var document = _builder.Build();

            // Assert
            Assert.AreEqual(2, document.CstmrPmtStsRpt.SplmtryData.Count);
            Assert.AreEqual(2, _builder.GetSupplementaryDataCount());
        }

        /// <summary>
        /// Tests validation behavior when required fields are missing.
        /// </summary>
        [TestMethod]
        public void IsValid_WithoutRequiredFields_ShouldReturnFalse()
        {
            // Act & Assert
            Assert.IsFalse(_builder.IsValid());

            // Add only group header
            _builder.WithGroupHeader(CreateSampleGroupHeader());
            Assert.IsFalse(_builder.IsValid());

            // Add original group header
            _builder.WithOriginalGroupInformationAndStatus(CreateSampleOriginalGroupHeader());
            Assert.IsTrue(_builder.IsValid());
        }

        /// <summary>
        /// Tests that Build throws an exception when group header is missing.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Build_WithoutGroupHeader_ShouldThrowException()
        {
            // Arrange
            _builder.WithOriginalGroupInformationAndStatus(CreateSampleOriginalGroupHeader());

            // Act
            _builder.Build();
        }

        /// <summary>
        /// Tests that Build throws an exception when original group information is missing.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Build_WithoutOriginalGroupInformation_ShouldThrowException()
        {
            // Arrange
            _builder.WithGroupHeader(CreateSampleGroupHeader());

            // Act
            _builder.Build();
        }

        /// <summary>
        /// Tests null parameter validation for WithGroupHeader method.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WithGroupHeader_WithNullParameter_ShouldThrowException()
        {
            // Act
            _builder.WithGroupHeader(null!);
        }

        /// <summary>
        /// Tests null parameter validation for WithOriginalGroupInformationAndStatus method.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WithOriginalGroupInformationAndStatus_WithNullParameter_ShouldThrowException()
        {
            // Act
            _builder.WithOriginalGroupInformationAndStatus(null!);
        }

        /// <summary>
        /// Tests null parameter validation for AddOriginalPaymentInstructionAndStatus method.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddOriginalPaymentInstructionAndStatus_WithNullParameter_ShouldThrowException()
        {
            // Act
            _builder.AddOriginalPaymentInstructionAndStatus(null!);
        }

        /// <summary>
        /// Tests null parameter validation for AddSupplementaryData method.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddSupplementaryData_WithNullParameter_ShouldThrowException()
        {
            // Act
            _builder.AddSupplementaryData(null!);
        }

        /// <summary>
        /// Tests the Reset functionality.
        /// </summary>
        [TestMethod]
        public void Reset_ShouldClearAllData()
        {
            // Arrange
            _builder
                .WithGroupHeader(CreateSampleGroupHeader())
                .WithOriginalGroupInformationAndStatus(CreateSampleOriginalGroupHeader())
                .AddOriginalPaymentInstructionAndStatus(CreateSampleOriginalPaymentInstruction())
                .AddSupplementaryData(CreateSampleSupplementaryData());

            // Act
            _builder.Reset();

            // Assert
            Assert.IsFalse(_builder.IsValid());
            Assert.AreEqual(0, _builder.GetOriginalPaymentInstructionCount());
            Assert.AreEqual(0, _builder.GetSupplementaryDataCount());
        }

        /// <summary>
        /// Tests the Clone functionality.
        /// </summary>
        [TestMethod]
        public void Clone_ShouldCreateIdenticalBuilder()
        {
            // Arrange
            var groupHeader = CreateSampleGroupHeader();
            var originalGroupHeader = CreateSampleOriginalGroupHeader();
            var originalPaymentInstruction = CreateSampleOriginalPaymentInstruction();
            var supplementaryData = CreateSampleSupplementaryData();

            _builder
                .WithGroupHeader(groupHeader)
                .WithOriginalGroupInformationAndStatus(originalGroupHeader)
                .AddOriginalPaymentInstructionAndStatus(originalPaymentInstruction)
                .AddSupplementaryData(supplementaryData);

            // Act
            var clonedBuilder = _builder.Clone();
            var originalDocument = _builder.Build();
            var clonedDocument = clonedBuilder.Build();

            // Assert
            Assert.AreNotSame(_builder, clonedBuilder);
            Assert.AreNotSame(originalDocument, clonedDocument);
            Assert.AreEqual(originalDocument.CstmrPmtStsRpt.OrgnlPmtInfAndSts.Count,
                           clonedDocument.CstmrPmtStsRpt.OrgnlPmtInfAndSts.Count);
            Assert.AreEqual(originalDocument.CstmrPmtStsRpt.SplmtryData.Count,
                           clonedDocument.CstmrPmtStsRpt.SplmtryData.Count);
        }

        /// <summary>
        /// Tests clearing original payment instructions.
        /// </summary>
        [TestMethod]
        public void ClearOriginalPaymentInstructions_ShouldRemoveAllInstructions()
        {
            // Arrange
            _builder
                .WithGroupHeader(CreateSampleGroupHeader())
                .WithOriginalGroupInformationAndStatus(CreateSampleOriginalGroupHeader())
                .AddOriginalPaymentInstructionAndStatus(CreateSampleOriginalPaymentInstruction())
                .AddOriginalPaymentInstructionAndStatus(CreateSampleOriginalPaymentInstruction("PMT002"));

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
            _builder
                .WithGroupHeader(CreateSampleGroupHeader())
                .WithOriginalGroupInformationAndStatus(CreateSampleOriginalGroupHeader())
                .AddSupplementaryData(CreateSampleSupplementaryData())
                .AddSupplementaryData(CreateSampleSupplementaryData("DATA002"));

            // Act
            _builder.ClearSupplementaryData();

            // Assert
            Assert.AreEqual(0, _builder.GetSupplementaryDataCount());
        }

        /// <summary>
        /// Tests BuildXml with an object parameter of wrong type.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void BuildXml_WithInvalidMessageType_ShouldThrowException()
        {
            // Act
            _builder.BuildXml("invalid message type");
        }

        /// <summary>
        /// Tests BuildXml with null object parameter.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildXml_WithNullMessage_ShouldThrowException()
        {
            // Act
            _builder.BuildXml(null!);
        }

        #region Helper Methods

        /// <summary>
        /// Creates a sample GroupHeader52 for testing purposes.
        /// </summary>
        /// <returns>A properly initialized GroupHeader52 instance.</returns>
        private static GroupHeader52 CreateSampleGroupHeader()
        {
            return new GroupHeader52
            {
                MsgId = "MSG20250726001",
                CreDtTm = DateTime.Now,
                InitgPty = new PartyIdentification43
                {
                    Nm = "SAMPLE_BANK_V06",
                    Id = new Party11Choice
                    {
                        OrgId = new OrganisationIdentification8
                        {
                            AnyBic = "SAMPLEGB2L"
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Creates a sample OriginalGroupHeader1 for testing purposes.
        /// </summary>
        /// <returns>A properly initialized OriginalGroupHeader1 instance.</returns>
        private static OriginalGroupHeader1 CreateSampleOriginalGroupHeader()
        {
            return new OriginalGroupHeader1
            {
                OrgnlMsgId = "ORIG_MSG_001_V06",
                OrgnlMsgNmId = "pain.001.001.06",
                OrgnlCreDtTm = DateTime.Now.AddHours(-1),
                OrgnlNbOfTxs = "10",
                OrgnlCtrlSum = 2500.00m,
                OrgnlCtrlSumSpecified = true,
                GrpSts = TransactionGroupStatus3Code.Accp,
                GrpStsSpecified = true
            };
        }

        /// <summary>
        /// Creates a sample OriginalPaymentInstruction12 for testing purposes.
        /// </summary>
        /// <param name="paymentInfoId">Optional payment information ID. Defaults to "PMT_INFO_001_V06".</param>
        /// <returns>A properly initialized OriginalPaymentInstruction12 instance.</returns>
        private static OriginalPaymentInstruction12 CreateSampleOriginalPaymentInstruction(string paymentInfoId = "PMT_INFO_001_V06")
        {
            var originalPayment = new OriginalPaymentInstruction12
            {
                OrgnlPmtInfId = paymentInfoId,
                PmtInfSts = TransactionGroupStatus3Code.Accp,
                PmtInfStsSpecified = true
            };

            // Add status reason information
            originalPayment.StsRsnInf.Add(new StatusReasonInformation9
            {
                Rsn = new StatusReason6Choice
                {
                    Cd = "AC03" // Accepted with change of details
                }
            });

            return originalPayment;
        }

        /// <summary>
        /// Creates a sample SupplementaryData1 for testing purposes.
        /// </summary>
        /// <param name="dataId">Optional data identifier. Defaults to "SUPP_DATA_001".</param>
        /// <returns>A properly initialized SupplementaryData1 instance.</returns>
        private static SupplementaryData1 CreateSampleSupplementaryData(string dataId = "SUPP_DATA_001")
        {
            return new SupplementaryData1
            {
                PlcAndNm = dataId,
                Envlp = new SupplementaryDataEnvelope1
                {
                    Any = new System.Xml.XmlDocument().CreateElement("TestData")
                }
            };
        }

        #endregion
    }
}
