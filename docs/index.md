# ISO 20022 Library Documentation

## Overview

Iso20022Library is a .NET library that provides tools for working with ISO 20022 financial message standards. The library supports generating, parsing, validating, and processing ISO 20022 messages in XML format.

ISO 20022 is an international standard for financial messaging that enables financial institutions worldwide to communicate in a consistent and standardized way. This library simplifies working with these complex message formats.

## Key Features

- Generation of ISO 20022 compliant messages
- Validation of XML messages against ISO 20022 schemas
- Strongly-typed C# object model for ISO 20022 messages
- Builder pattern for easier message construction
- Support for different message types and versions

## Project Structure

The project is organized into several layers following clean architecture principles:

- **Domain**: Contains the core business logic and entities
- **Application**: Contains application-specific business rules and interfaces
- **Infrastructure**: Contains external concerns like file system access and serialization
- **Messages**: Contains the generated C# classes from ISO 20022 XSD schemas
- **Tests**: Contains unit tests for the library

## Documentation

- [Architecture and Design](architecture.md) - UML class diagrams and architectural overview
- [Message Generation](message-generation.md) - How ISO 20022 XSD schemas are converted to C# code
- [Testing Strategy](testing.md) - Unit testing approach and code coverage information
- [Usage Examples](examples.md) - Code examples showing how to use the library

## Getting Started

To start using the library, add a reference to the Iso20022Library package and create a message builder:

```csharp
// Example code will be provided soon
```

## Supported Message Types

Currently, the library supports the following ISO 20022 message types:

- pain.001.001.02 (Customer Credit Transfer Initiation)
- More message types to be added

## License

This project is licensed under the MIT License - see the LICENSE file for details.
