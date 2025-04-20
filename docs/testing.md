# Testing Strategy

This document outlines the testing approach and code coverage for the Iso20022Library project.

## Testing Framework

The Iso20022Library uses MSTest as the testing framework. Tests are located in the `Iso20022Library.Tests` project.

```
Iso20022Library.Tests/
├── XmlValidatorTests.cs   # Tests for XML validation
├── Pain00100102Tests.cs   # Tests for pain.001.001.02 message handling
└── ...                    # Other test files
```

## Test Categories

The tests in the project are organized into several categories:

### 1. Unit Tests

These tests focus on testing individual components in isolation:

- **Builder Tests**: Verify that message builders correctly construct ISO 20022 messages.
- **Validator Tests**: Test XML validation against ISO 20022 schemas.
- **Serialization Tests**: Ensure proper XML serialization and deserialization.

### 2. Integration Tests

These tests verify the interaction between multiple components:

- **End-to-End Tests**: Test the complete flow from message creation to validation.
- **Infrastructure Tests**: Test interaction with external components like file system.

## Test Examples

### XML Validator Tests

```csharp
[TestClass]
public class XmlValidatorTests
{
    [TestMethod]
    public void Validate_ValidXmlAgainstSchema_ShouldReturnTrue()
    {
        // Arrange
        var validator = new XmlValidator();
        string validXml = "<xml>...</xml>";
        string schemaPath = "path/to/schema.xsd";
        
        // Act
        bool result = validator.Validate(validXml, schemaPath);
        
        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Validate_InvalidXmlAgainstSchema_ShouldReturnFalse()
    {
        // Arrange
        var validator = new XmlValidator();
        string invalidXml = "<xml>invalid</xml>";
        string schemaPath = "path/to/schema.xsd";
        
        // Act
        bool result = validator.Validate(invalidXml, schemaPath);
        
        // Assert
        Assert.IsFalse(result);
    }
}
```

### PAIN.001.001.02 Tests

```csharp
[TestClass]
public class Pain00100102Tests
{
    [TestMethod]
    public void BuildPain00100102_WithRequiredFields_ShouldCreateValidMessage()
    {
        // Arrange
        var factory = new MessageBuilderFactory();
        var builder = factory.CreateBuilder(MessageType.Pain00100102);
        
        // Act
        var message = builder
            .WithGroupHeader("MSG-001", DateTime.Now)
            .WithDebtorAccount("DE12345678901234567890")
            .WithPaymentInfo("PMT-001")
            .WithTransaction("TX-001", 100.00m, "EUR")
            .WithCreditor("Creditor Name", "GB12345678901234567890")
            .Build();
            
        var document = message as pain00100102;
        
        // Assert
        Assert.IsNotNull(document);
        Assert.AreEqual("MSG-001", document.GrpHdr.MsgId);
        Assert.AreEqual("PMT-001", document.PmtInf[0].PmtInfId);
        // More assertions...
    }
    
    [TestMethod]
    public void SerializePain00100102_ShouldProduceValidXml()
    {
        // Arrange
        var message = CreateSampleMessage();
        
        // Act
        string xml = XmlSerializationService.Serialize(message);
        bool isValid = new XmlValidator().Validate(xml, "Pain00100102/Xsd/pain.001.001.02.xsd");
        
        // Assert
        Assert.IsTrue(isValid);
    }
}
```

## Test Data

The tests use several approaches for test data:

1. **In-memory Data**: Simple data created directly in test methods
2. **Test Fixtures**: Reusable test data objects
3. **Sample Files**: External XML and XSD files for testing

## Code Coverage

The project aims for high code coverage in critical areas:

### Coverage Targets

- **Core Domain Logic**: 95%+ coverage
- **Application Services**: 90%+ coverage
- **Infrastructure**: 80%+ coverage
- **Message Builders**: 90%+ coverage

### Coverage Measurement

Code coverage is measured using the built-in code coverage tools in Visual Studio and the coverlet collector package.

To run tests with coverage:

```
dotnet test --collect:"XPlat Code Coverage"
```

### Coverage Report Generation

Coverage reports can be generated using the ReportGenerator tool:

```
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html
```

## Continuous Integration

Tests are automatically run in the CI pipeline with the following workflow:

1. Build the project
2. Run all unit tests
3. Generate and publish coverage reports
4. Fail the build if coverage falls below thresholds

## Test Maintenance

To maintain the health of the test suite:

1. **Keep Tests Independent**: Tests should not rely on each other
2. **Clean Up Resources**: Dispose of any resources created during tests
3. **Update Tests When Code Changes**: Keep tests in sync with code changes
4. **Refactor Tests**: Regularly refactor tests to reduce duplication

## Mocking Strategy

For unit tests that require isolation from dependencies:

1. **Explicit Interfaces**: All dependencies are exposed through interfaces
2. **Dependency Injection**: Use DI to inject mock implementations
3. **Test Doubles**: Use mocks, stubs, and fakes as appropriate

## Performance Testing

Performance tests focus on:

1. **Serialization Performance**: Measure time to serialize large messages
2. **Memory Usage**: Track memory consumption during message processing
3. **Throughput**: Measure the number of messages processed per second
