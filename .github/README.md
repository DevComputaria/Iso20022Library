# GitHub Actions Workflows

This repository includes several GitHub Actions workflows to automate building, testing, and releasing the Iso20022Library.

## Available Workflows

### 1. CI/CD Pipeline (`ci.yml`)

**Triggers:**
- Push to `main` or `develop` branches
- Pull requests to `main` or `develop` branches

**Jobs:**
- **build-and-test**: Builds the solution and runs all tests
- **code-quality**: Performs static code analysis
- **package**: Creates NuGet packages (only on `main` branch)
- **security-scan**: Runs security vulnerability scanning

**Features:**
- Tests multiple .NET versions
- Caches NuGet packages for faster builds
- Uploads test results and artifacts
- Creates NuGet packages for releases

### 2. Pull Request Validation (`pr-validation.yml`)

**Triggers:**
- Pull request opened, synchronized, or reopened

**Features:**
- Lightweight validation for PRs
- Comments build status on PRs
- Fast feedback for contributors

### 3. Release and Publish (`release.yml`)

**Triggers:**
- GitHub release published
- Manual workflow dispatch

**Features:**
- Builds and tests the solution
- Creates versioned NuGet packages
- Publishes to NuGet.org (requires API key)
- Creates GitHub release assets

## Setup Requirements

### Required Secrets

For the workflows to function properly, you need to configure the following repository secrets:

1. **NUGET_API_KEY**: API key for publishing to NuGet.org
   - Go to Repository Settings → Secrets and variables → Actions
   - Add a new secret named `NUGET_API_KEY`
   - Get your API key from https://www.nuget.org/account/apikeys

### Optional Configuration

- **Reviewers**: Update the `dependabot.yml` file to use your GitHub username
- **Branch Protection**: Consider enabling branch protection rules for `main` branch

## Usage Examples

### Running Tests Locally

Before pushing changes, you can run the same commands locally:

```bash
# Restore dependencies
dotnet restore

# Build solution
dotnet build --no-restore --configuration Release

# Run tests
dotnet test --no-build --configuration Release --verbosity normal
```

### Creating a Release

1. **Automatic Release** (recommended):
   - Create a new release on GitHub
   - Tag it with a version (e.g., `v1.0.0`)
   - The workflow will automatically build and publish

2. **Manual Release**:
   - Go to Actions → Release and Publish
   - Click "Run workflow"
   - Enter the version number
   - Click "Run workflow"

### Version Naming Convention

- **Stable releases**: `1.0.0`, `1.1.0`, `2.0.0`
- **Pre-releases**: `1.0.0-alpha.1`, `1.0.0-beta.1`, `1.0.0-rc.1`
- **Development builds**: `1.0.0-alpha.20250127120000` (auto-generated)

## Workflow Status

You can monitor workflow status in several ways:

1. **Repository Badge**: Add this to your README.md:
   ```markdown
   ![CI/CD](https://github.com/yourusername/Iso20022Library/workflows/CI/CD%20Pipeline/badge.svg)
   ```

2. **Actions Tab**: View detailed logs and results in the repository's Actions tab

3. **Pull Request Checks**: Status checks appear automatically on PRs

## Troubleshooting

### Common Issues

1. **NuGet publish fails**: Ensure `NUGET_API_KEY` secret is configured correctly
2. **Tests fail on CI but pass locally**: Check for environment-specific dependencies
3. **Package version conflicts**: Ensure version numbers follow semantic versioning

### Getting Help

- Check the Actions tab for detailed error logs
- Review the workflow YAML files for configuration details
- Ensure all required secrets are properly configured

## Contributing

When contributing to this repository:

1. All PRs will automatically trigger the PR validation workflow
2. Ensure tests pass locally before pushing
3. Follow the established coding standards
4. Update documentation if adding new features

The workflows are designed to maintain code quality and ensure reliable releases of the Iso20022Library.
