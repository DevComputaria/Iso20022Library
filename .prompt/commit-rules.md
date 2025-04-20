# Commit Message Guidelines

This project follows the [Conventional Commits](https://www.conventionalcommits.org/) specification for creating descriptive and standardized commit messages.

## Format

```
<type>(<scope>): <short summary>
<BLANK LINE>
<body>
<BLANK LINE>
<footer>
```

## Types

- **feat**: A new feature
- **fix**: A bug fix
- **docs**: Documentation only changes
- **style**: Changes that do not affect the meaning of the code (white-space, formatting, etc)
- **refactor**: A code change that neither fixes a bug nor adds a feature
- **perf**: A code change that improves performance
- **test**: Adding missing tests or correcting existing tests
- **build**: Changes that affect the build system or external dependencies
- **ci**: Changes to CI configuration files and scripts
- **chore**: Other changes that don't modify src or test files
- **revert**: Reverts a previous commit

## Scope

The scope should be the name of the component or module affected by the change.

## Commit Message Body

- Use the imperative, present tense: "change" not "changed" nor "changes"
- Include motivation for the change and contrast with previous behavior
- Be extremely detailed with the file changes and the reason for change
- Reference issues and pull requests liberally

## Examples

```
feat(authentication): implement JWT token validation

Add JWT validation middleware to authenticate API requests.
- Created JwtValidator class to handle token parsing and validation
- Added user claim extraction from tokens
- Configured DI in Startup.cs for the validator
- Added unit tests for validation logic

Resolves: #123
```

```
fix(data): correct SQL query for user lookup

Fixed incorrect join condition in UserRepository that was causing duplicate users
to be returned in some cases. Modified the inner join to be a left join and added
a distinct clause to ensure unique results.

Performance is improved by 15% and resolves customer reported issue #456
```
