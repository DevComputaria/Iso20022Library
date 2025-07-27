# Changelog

## [Unreleased]
### Added
- **Completed Pain00200108Builder** for ISO 20022 pain.002.001.08 messages (Customer Payment Status Report V08).
- Implemented BuildXml(object message) method to fully comply with IMessageBuilder interface.
- Fixed builder to use correct generated class property names and structure for Pain00200108.
- Updated and fixed all unit tests to match actual generated class properties and structure.
- Builder includes support for:
  - GroupHeader52 configuration
  - Original group information and status
  - Original payment instructions
  - Payment transactions with status
  - Transaction status reasons
  - Supplementary data
- Pain00200108Builder is registered in MessageBuilderFactory to support MessageType.Pain00200108.
- **All tests now passing (96/96)** - comprehensive test coverage for all builder functionality.

### Fixed
- **Critical bug in AddPaymentTransaction method**: Fixed incorrect assignment where originalTransactionId parameter was overwriting OrgnlEndToEndId instead of properly setting OrgnlInstrId property.
- **Test validation issues**: Updated all failing tests to include required minimal builder setup (SetMessageIdentification, SetOriginalGroupInformation, AddOriginalPaymentInstruction).
- **Test assertions**: Fixed test assertions to validate correct properties (OrgnlInstrId vs OrgnlEndToEndId) according to generated code structure.

### Changed
- Move: The file `pain_001_001_04.cs` was moved from `Payments/Pain/Generated` to `Payments/Pain/Pain00100104`.
- Refactor: Updated all references and `using` statements to use the new namespace `Iso20022Library.Messages.Payments.Pain.Pain00100104` in both the builder and test files.
- Cleanup: Removed the old file from the `Generated` folder to avoid duplication.
- Refactored Pain00100104Builder and related test to use generated ISO 20022 classes with `Collection<T>` for collections, and updated all property access to match generated code.
- Updated all references to use the new namespace `Iso20022Library.Messages.Payments.Pain.Generated.Pain00100104`.
- Updated test to use correct enum values, property names (e.g., `Iban`, `Bicfi`, `InstdAmt`), and collection initializers.
- Verified build and all tests pass after migration to generated code and namespace update.
- Added Pain00100102Builder for ISO 20022 pain.001.001.02 messages, following the builder pattern.
- Registered Pain00100102Builder in MessageBuilderFactory to support MessageType.Pain00100102.

### Added
- Completed Pain00200106Builder for ISO 20022 pain.002.001.06 messages (Customer Payment Status Report V06).
- Added comprehensive unit tests for Pain00200106Builder with 19 test methods covering all functionality.
- Builder includes support for:
  - GroupHeader52 configuration
  - OriginalGroupHeader1 setup
  - Adding single and multiple OriginalPaymentInstruction12 entries
  - Adding single and multiple SupplementaryData1 entries
  - Complete validation and error handling
  - XML serialization functionality
  - Builder pattern methods (Reset, Clone, Clear operations)
- Fixed test compilation error in CreateSampleSupplementaryData helper method by correctly creating SupplementaryDataEnvelope1 object.
- All tests pass successfully (19/19) confirming proper builder functionality.
- Builder is already registered in MessageBuilderFactory for MessageType.Pain00200106.

- Completed Pain00200107Builder for ISO 20022 pain.002.001.07 messages (Customer Payment Status Report V07).
- Added comprehensive unit tests for Pain00200107Builder with 19 test methods covering all functionality.
- Builder includes support for:
  - GroupHeader52 configuration (with InitgPty property)
  - OriginalGroupHeader1 setup
  - Adding single and multiple OriginalPaymentInstruction18 entries
  - Adding single and multiple SupplementaryData1 entries
  - Complete validation and error handling
  - XML serialization functionality
  - Builder pattern methods (Reset, Clone, Clear operations)
- Fixed test helper methods to use correct types and properties for pain.002.001.07:
  - Updated GroupHeader52 helper to use InitgPty instead of InstgAgt
  - Fixed BICFI property name to Bicfi (lowercase 'i')
  - Corrected enum usage from ExternalPaymentGroupStatus1Code to TransactionGroupStatus3Code
- All tests pass successfully (19/19) confirming proper builder functionality.
- Builder is registered in MessageBuilderFactory for MessageType.Pain00200107.

## [2025-06-29] Regenerated Pain.001.001.04 C# class from XSD using xscgen
- Used xscgen with explicit XML-to-C# namespace mapping to generate Iso20022Library.Messages.Payments.Pain.Pain00100104.cs from pain.001.001.04.xsd.
- Marked the old pain_001_001_04.cs as obsolete and safe to remove.
- Verified that all references and tests compile and pass with the new generated class.

## [2025-06-29] Added all generated pain message types to MessageType enum and fixed Pain00700110Builder
- Added all generated pain message types (Pain00100106, Pain00100107, Pain00100108, Pain00100109, Pain00100110, Pain00200104, Pain00200106, Pain00200107, Pain00200108, Pain00200109, Pain00200110, Pain00200111, Pain00700103, Pain00700105, Pain00700106, Pain00700107, Pain00700108, Pain00700109) to the MessageType enum.
- Fixed Pain00700110Builder to use the correct types and properties from the generated pain.007.001.10 C# classes, including methods for adding OriginalPaymentInstruction37 objects to OrgnlPmtInfAndRvsl.
- Ensured all code compiles without errors after these changes.
