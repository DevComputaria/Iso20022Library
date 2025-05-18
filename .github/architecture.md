# Project Architecture

The Iso20022Library project is designed following the principles of Clean Architecture. This ensures that the system is maintainable, testable, and scalable. Below is an overview of the architecture and its components.

## Layers

### 1. Domain Layer
- **Purpose**: Contains the core business logic and types that are independent of any external dependencies.
- **Components**:
  - Interfaces
  - Enums

### 2. Application Layer
- **Purpose**: Implements the business logic and orchestrates the use of domain entities.
- **Components**:
  - Builders (e.g., `MessageBuilderFactory`, `Pain00100102Builder`)
  - Validators (e.g., `XmlValidator`)

### 3. Infrastructure Layer
- **Purpose**: Handles external concerns such as XML serialization and other infrastructure-related tasks.
- **Components**:
  - XML Serialization Service (e.g., `XmlSerializationService`)

### 4. Presentation Layer
- **Purpose**: Manages the user interface or API endpoints. (Not implemented in this library as it focuses on backend logic.)

### 5. Messages Layer
- **Purpose**: Contains the ISO 20022 message definitions and schemas.
- **Components**:
  - Organized by business domains (e.g., Payments, Securities, Cash Management)
  - Subfolders for specific message types (e.g., `Pain`, `Pacs`, `Camt`)

## Folder Structure

```plaintext
Iso20022Library/
├── src/
│   ├── Iso20022Library.Domain/          # Core business logic
│   ├── Iso20022Library.Application/     # Builders, Validators
│   ├── Iso20022Library.Infrastructure/  # XML Serialization
│   └── Iso20022Library.Messages/        # ISO 20022 messages and schemas
├── tests/
│   └── Iso20022Library.Tests/           # Unit and integration tests
├── docs/                                # Documentation
└── scripts/                             # Utility scripts
```

## Design Principles

The project adheres to the following principles:

1. **Clean Architecture**: Separation of concerns between layers.
2. **SOLID Principles**: Ensures maintainable and scalable code.
3. **Testability**: Each layer is independently testable.

## Dependencies

- **Domain Layer**: No dependencies.
- **Application Layer**: Depends on the Domain Layer.
- **Infrastructure Layer**: Depends on the Application Layer.
- **Messages Layer**: Independent of other layers.

## Testing

The project uses MSTest for unit and integration testing. Tests are located in the `Iso20022Library.Tests` project and are organized by feature.