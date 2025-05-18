# C# Code Style Guidelines

## General Principles

- Write clean, readable, and maintainable code
- Follow Microsoft's [C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- Use consistent naming and formatting
- Write comprehensive, but concise documentation
- Write unit tests for all code

## SOLID Principles

### Single Responsibility Principle (SRP)

A class should have only one reason to change, meaning it should have only one job or responsibility.

### Open/Closed Principle (OCP)

Software entities should be open for extension but closed for modification.

### Liskov Substitution Principle (LSP)

Objects of a superclass should be replaceable with objects of a subclass without affecting the correctness of the program.

### Interface Segregation Principle (ISP)

Many client-specific interfaces are better than one general-purpose interface.

### Dependency Inversion Principle (DIP)

High-level modules should not depend on low-level modules. Both should depend on abstractions.

## Clean Architecture

Follow the clean architecture principles:

1. **Independent of frameworks**: The architecture does not depend on libraries or frameworks.
2. **Testable**: The business rules can be tested without the UI, database, or any external elements.
3. **Independent of UI**: The UI can change without changing the rest of the system.
4. **Independent of database**: The business rules are not bound to the database.
5. **Independent of any external agency**: The business rules don't know anything about the outside world.

### Layers

- **Domain Layer**: Contains enterprise-wide logic and types
- **Application Layer**: Contains business logic and types
- **Infrastructure Layer**: Contains all external concerns
- **Presentation Layer**: Contains UI related concerns

## Design Patterns

### Creational Patterns

- **Factory Method**: Use for creating objects without specifying the exact class
- **Abstract Factory**: Create families of related objects
- **Builder**: Separate the construction of complex objects
- **Singleton**: Ensure a class has only one instance (use with caution)
- **Dependency Injection**: Inject dependencies rather than creating them

### Structural Patterns

- **Adapter**: Allow incompatible interfaces to work together
- **Bridge**: Separate abstraction from implementation
- **Composite**: Compose objects into tree structures
- **Decorator**: Add responsibilities to objects dynamically
- **Facade**: Provide a simplified interface to a complex subsystem

### Behavioral Patterns

- **Command**: Encapsulate a request as an object
- **Iterator**: Access elements without exposing underlying structure
- **Observer**: Define a one-to-many dependency between objects
- **Strategy**: Define a family of algorithms, encapsulate each one
- **Template Method**: Define the skeleton of an algorithm, deferring some steps

## C# Specific Practices

- Use `var` only when the type is obvious
- Prefer expression-bodied members when the method is a one-liner
- Use pattern matching for type checking and casting
- Use `nameof()` instead of hardcoded strings for property names
- Use records for immutable data
- Use nullable reference types
- Prefer async/await over direct Task manipulation
- Use LINQ for collection processing, but ensure it's readable

## Project Structure

- Organize code by feature rather than technical concern
- Keep files small and focused
- Use folders to organize related code
- Follow a consistent folder hierarchy

## Error Handling

- Use exceptions for exceptional cases, not for control flow
- Create custom exceptions for domain-specific errors
- Always include meaningful error messages
- Handle exceptions at the appropriate level

## Performance Considerations

- Use `StringBuilder` for string concatenation in loops
- Consider using `Span<T>` for memory-efficient operations
- Avoid premature optimization
- Profile before optimizing
