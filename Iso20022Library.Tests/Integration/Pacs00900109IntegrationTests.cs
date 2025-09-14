using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Iso20022Library.Application.Builders;
using Iso20022Library.Application.Builders.Pacs;
using Iso20022Library.Domain.Common.Enums;

namespace Iso20022Library.Tests.Integration
{
    [TestClass]
    public class Pacs00900109IntegrationTests
    {
        [TestMethod]
        public void CompleteWorkflow_BuilderAndXmlGeneration_Success()
        {
            // Arrange
            var factory = new MessageBuilderFactory();
            var builder = (Pacs00900109Builder)factory.GetBuilder(MessageType.Pacs00900109);

            // Act
            var xml = builder
                .WithMessageId("PACS009MSG001")
                .WithCreationDateTime(DateTime.Now)
                .WithInstructingAgent("BKAUATWW", "Bank Austria")
                .WithInstructedAgent("DEUTDEFF", "Deutsche Bank")
                .WithTotalInterbankSettlementAmount(1000m, "EUR")
                .AddCreditTransferTransaction()
                    .WithPaymentIdentification("INSTR001", "E2E001")
                    .WithInterbankSettlementAmount(1000m, "EUR")
                    .WithInstructingAgent("BKAUATWW")
                    .WithInstructedAgent("DEUTDEFF")
                    .WithDebtor("DBTRBANK", "Debtor Bank")
                    .WithCreditor("CDTRBANK", "Creditor Bank")
                    .WithRemittanceInformation("FI Credit Transfer")
                    .AddToBuilder()
                .BuildXml();

            // Assert
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.Length > 0);
            Assert.IsTrue(xml.Contains("FICdtTrf"));
            Assert.IsTrue(xml.Contains("PACS009MSG001"));
            Assert.IsTrue(xml.Contains("BKAUATWW"));
            Assert.IsTrue(xml.Contains("DEUTDEFF"));
            Assert.IsTrue(xml.Contains("DBTRBANK"));
            Assert.IsTrue(xml.Contains("CDTRBANK"));
            Assert.IsTrue(xml.Contains("FI Credit Transfer"));
            Assert.IsTrue(xml.Contains("EUR"));
            Assert.IsTrue(xml.Contains("1000"));

            Console.WriteLine("✅ XML Generation Success!");
            Console.WriteLine($"📊 Length: {xml.Length} characters");
            Console.WriteLine($"🔍 First 300 characters:\n{xml.Substring(0, Math.Min(300, xml.Length))}...");
        }
    }
}
