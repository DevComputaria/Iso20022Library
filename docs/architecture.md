# Architecture and Design

## Project Structure

Iso20022Library follows a clean architecture approach with distinct layers:

```
Iso20022Library/
├── Application/       # Application services, validators, builders
├── Domain/           # Core business logic and entities
├── Infrastructure/   # External concerns (serialization, etc.)
├── Messages/         # Generated ISO 20022 message classes
└── Tests/            # Unit tests
```

## Class Diagrams

### Domain Layer

The Domain layer contains the core entities and interfaces for the application.

```mermaid
classDiagram
    namespace Domain.Common {
        class Enums {
            +MessageType
        }
        interface IMessageBuilder {
            +Build() : object
        }
    }
```

### Application Layer

The Application layer contains the business logic and implements the domain interfaces.

```mermaid
classDiagram
    namespace Application {
        class MessageBuilderFactory {
            +CreateBuilder(messageType: MessageType) : IMessageBuilder
        }
        class XmlValidator {
            +Validate(xml: string, schemaPath: string) : bool
        }
        class Utf8StringWriter {
            +Encoding: Encoding
        }
    }
```

### Infrastructure Layer

The Infrastructure layer handles external concerns like XML serialization.

```mermaid
classDiagram
    namespace Infrastructure.Xml {
        class XmlSerializationService {
            +Serialize<T>(obj: T) : string
        }
        class Utf8StringWriter {
            +Encoding: Encoding
        }
    }
```

### Messages Layer

The Messages layer contains the auto-generated classes from ISO 20022 XSD schemas.

```mermaid
classDiagram
    namespace Messages.Pain00100102.Generated {
        class Document {
            +pain_001_001_02: pain00100102
        }
        class pain00100102 {
            +GrpHdr: GroupHeader1
            +PmtInf: PaymentInformation1[]
        }
        class GroupHeader1 {
            +MsgId: string
            +CreDtTm: DateTime
            +NbOfTxs: string
            +InitgPty: PartyIdentification8
        }
        class PaymentInformation1 {
            +PmtInfId: string
            +PmtMtd: PaymentMethod3Code
            +ReqdExctnDt: DateTime
            +Dbtr: PartyIdentification8
            +DbtrAcct: CashAccount7
            +DbtrAgt: BranchAndFinancialInstitutionIdentification3
            +CdtTrfTxInf: CreditTransferTransactionInformation1[]
        }
    }
```

### Builders

The builder pattern is used to simplify the creation of complex ISO 20022 messages.

```mermaid
classDiagram
    IMessageBuilder <|-- Pain00100102Builder
    Pain00100102Builder --> pain00100102
    class IMessageBuilder {
        <<interface>>
        +Build() : object
    }
    class Pain00100102Builder {
        -message: pain00100102
        +WithGroupHeader(msgId: string, creationDateTime: DateTime) : Pain00100102Builder
        +WithPaymentInfo(paymentInfoId: string) : Pain00100102Builder
        +WithDebtor(name: string) : Pain00100102Builder
        +WithCreditor(name: string) : Pain00100102Builder
        +Build() : pain00100102
    }
```

## Component Interactions

The following diagram illustrates how the components interact to generate an ISO 20022 message:

```mermaid
sequenceDiagram
    Client->>+MessageBuilderFactory: CreateBuilder(MessageType)
    MessageBuilderFactory-->>-Client: IMessageBuilder
    Client->>+IMessageBuilder: Configure builder (with methods)
    IMessageBuilder-->>-Client: Builder (fluent interface)
    Client->>+IMessageBuilder: Build()
    IMessageBuilder-->>-Client: ISO 20022 message object
    Client->>+XmlSerializationService: Serialize(message)
    XmlSerializationService-->>-Client: XML string
    Client->>+XmlValidator: Validate(xml, schema)
    XmlValidator-->>-Client: Validation result
```

## Design Patterns

The library uses several design patterns:

1. **Builder Pattern** - For constructing complex ISO 20022 message objects
2. **Factory Pattern** - For creating the appropriate builder based on message type
3. **Strategy Pattern** - For handling different message validation strategies
4. **Adapter Pattern** - For adapting domain objects to ISO 20022 message format

## Extensibility

The library is designed to be extensible:

- New message types can be added by generating code from XSD and implementing a new builder
- The factory pattern allows for easy addition of new message builders
- The clean architecture approach separates concerns and makes modification easier
