# 📦 Iso20022Library

Biblioteca modular em C# (.NET 8) para gerar, serializar e validar mensagens do padrão **ISO 20022**, com suporte a estrutura **Clean Architecture**, **validação via XSD** e geração de **documentação automática com DocFX**.

---

## 📚 Funcionalidades

- ✅ Geração de XML para mensagens ISO 20022 (ex: `pain.001.001.03`)
- ✅ Validação de mensagens com arquivos `.xsd`
- ✅ Serialização com UTF-8
- ✅ Estrutura baseada em Clean Architecture
- ✅ Testes automatizados com MSTest
- ✅ Geração de documentação via DocFX

---

## 🧱 Estrutura do Projeto

```plaintext
Iso20022Library/
├── src/
│   ├── Iso20022Library.Domain/          # Interfaces e enums
│   ├── Iso20022Library.Application/     # Builders, Validadores
│   ├── Iso20022Library.Infrastructure/  # Serialização XML
│   └── Iso20022Library.Messages/        # XSD e classes geradas
├── tests/
│   └── Iso20022Library.Tests/           # Testes com MSTest
└── docfx_project/                       # Geração da documentação
```

---

## 🚀 Como Usar

### 1. Gerar XML de uma mensagem

```csharp
var doc = new Document { /* preencher dados da mensagem */ };
var factory = new MessageBuilderFactory();
var builder = factory.GetBuilder(MessageType.Pain00100103);
string xml = builder.BuildXml(doc);
```

### 2. Validar contra o XSD

```csharp
bool valido = XmlValidator.Validate(xml, "caminho/pain.001.001.03.xsd", out string erros);
```

---

## 🧪 Executar Testes

```bash
dotnet test
```

---

## 📘 Gerar Documentação

```bash
cd docfx_project
docfx build
```

Acesse a documentação em: `docfx_project/_site/index.html`

---

## 📂 Mensagens Suportadas

- [x] `pain.001.001.03`
- [x] `pain.001.001.02`
- [ ] `pain.002.001.09`
- [ ] `camt.053.001.08`
- (Adicione mais via XSD em `Messages/`)

---

## 🛠️ Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/)
- [DocFX](https://dotnet.github.io/docfx/) (para gerar a documentação)

---

## 📄 Licença

MIT License © 2025
