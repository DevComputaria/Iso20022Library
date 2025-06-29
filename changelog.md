# Changelog

## [Unreleased]
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

## [2025-06-29] Regenerated Pain.001.001.04 C# class from XSD using xscgen
- Used xscgen with explicit XML-to-C# namespace mapping to generate Iso20022Library.Messages.Payments.Pain.Pain00100104.cs from pain.001.001.04.xsd.
- Marked the old pain_001_001_04.cs as obsolete and safe to remove.
- Verified that all references and tests compile and pass with the new generated class.
