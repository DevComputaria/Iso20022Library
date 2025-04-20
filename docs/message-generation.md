# ISO 20022 Message Generation

## Overview

This document explains how ISO 20022 messages are defined in XSD (XML Schema Definition) format and how C# classes are generated from these schemas using the xsd.exe tool provided by .NET.

## What is ISO 20022?

ISO 20022 is an international standard for financial messaging that defines a common platform for the development of messages. It consists of a standardized methodology, process, repository, and a set of XML schemas that financial institutions can use to create consistent messaging standards.

## XSD Schema Structure

ISO 20022 messages are defined using XSD schemas. These schemas define:

- The structure of the message
- The data types and elements
- Validation rules
- Namespaces and other metadata

Example of an ISO 20022 XSD schema structure (pain.001.001.02):

```xml
<?xml version="1.0" encoding="UTF-8"?>
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" 
           xmlns="urn:iso:std:iso:20022:tech:xsd:pain.001.001.02" 
           elementFormDefault="qualified" 
           targetNamespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.02">
  
  <xs:element name="Document" type="Document"/>
  
  <xs:complexType name="AccountIdentification3Choice">
    <xs:sequence>
      <xs:choice>
        <xs:element name="IBAN" type="IBANIdentifier"/>
        <xs:element name="BBAN" type="BBANIdentifier"/>
        <xs:element name="UPIC" type="UPICIdentifier"/>
        <xs:element name="PrtryAcct" type="SimpleIdentificationInformation2"/>
      </xs:choice>
    </xs:sequence>
  </xs:complexType>
  
  <!-- Many more type definitions -->
  
</xs:schema>
```

## XSD to C# Generation Process

### Using xsd.exe

The xsd.exe tool is a command-line utility provided with the .NET SDK that generates C# classes from XSD schemas. The classes represent the elements and types defined in the schema.

#### Steps to Generate C# Classes:

1. **Download the ISO 20022 XSD Schema**:
   - ISO 20022 schemas can be obtained from the ISO 20022 website or from financial institutions.
   - Place the XSD file in a directory in your project (e.g., `Pain00100102/Xsd/`).

2. **Run the xsd.exe Tool**:
   ```
   xsd.exe pain.001.001.02.xsd /classes /namespace:Iso20022Library.Messages.Pain00100102.Generated
   ```

   Parameters explained:
   - `/classes`: Generates classes instead of a dataset
   - `/namespace:`: Specifies the namespace for the generated classes

3. **Include Generated Code in Your Project**:
   - The tool will generate a C# file (e.g., `pain_001_001_02.cs`) containing all the classes.
   - Add this file to your project in a logical location (e.g., `Pain00100102/Generated/`).

### Generated C# Classes

The xsd.exe tool generates C# classes with the following characteristics:

- Each XSD complex type becomes a C# class
- Each XSD simple type becomes a property or enum
- XML attributes become C# properties
- XML element relationships are represented with properties and arrays
- XML serialization attributes are added to enable serialization/deserialization

Example of a generated class:

```csharp
/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.02")]
public partial class pain00100102 {
    
    private GroupHeader1 grpHdrField;
    
    private PaymentInformation1[] pmtInfField;
    
    /// <remarks/>
    public GroupHeader1 GrpHdr {
        get {
            return this.grpHdrField;
        }
        set {
            this.grpHdrField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("PmtInf")]
    public PaymentInformation1[] PmtInf {
        get {
            return this.pmtInfField;
        }
        set {
            this.pmtInfField = value;
        }
    }
}
```

### XSD Enumerations

Enumerations in the XSD are converted to C# enum types:

```csharp
/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:iso:std:iso:20022:tech:xsd:pain.001.001.02")]
public enum ChargeBearerType1Code {
    
    /// <remarks/>
    DEBT,
    
    /// <remarks/>
    CRED,
    
    /// <remarks/>
    SHAR,
    
    /// <remarks/>
    SLEV,
}
```

### XSD Choice Elements

Choice elements in XSD (where only one of several possible elements can be used) are represented using a property with multiple XmlElement attributes and an enum to indicate which choice is being used:

```csharp
/// <remarks/>
[System.Xml.Serialization.XmlElementAttribute("IBAN", typeof(string))]
[System.Xml.Serialization.XmlElementAttribute("BBAN", typeof(string))]
[System.Xml.Serialization.XmlElementAttribute("UPIC", typeof(string))]
[System.Xml.Serialization.XmlElementAttribute("PrtryAcct", typeof(SimpleIdentificationInformation2))]
[System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
public object Item {
    get {
        return this.itemField;
    }
    set {
        this.itemField = value;
    }
}

/// <remarks/>
[System.Xml.Serialization.XmlIgnoreAttribute()]
public ItemChoiceType3 ItemElementName {
    get {
        return this.itemElementNameField;
    }
    set {
        this.itemElementNameField = value;
    }
}
```

## Using Generated Classes

Once the classes are generated, you can use them to:

1. **Create ISO 20022 messages**: Instantiate and populate the classes
2. **Serialize to XML**: Use System.Xml.Serialization to convert objects to XML
3. **Deserialize from XML**: Parse XML back into objects
4. **Validate messages**: Ensure they conform to the ISO 20022 standard

Example of creating and serializing a pain.001.001.02 message:

```csharp
// Create a document
var document = new Document();
document.pain_001_001_02 = new pain00100102();

// Set up group header
document.pain_001_001_02.GrpHdr = new GroupHeader1 {
    MsgId = "MSG-001",
    CreDtTm = DateTime.Now,
    NbOfTxs = "1",
    InitgPty = new PartyIdentification8 {
        Nm = "Initiating Company"
    }
};

// Add payment information
document.pain_001_001_02.PmtInf = new PaymentInformation1[] {
    new PaymentInformation1 {
        PmtInfId = "PMT-001",
        PmtMtd = PaymentMethod3Code.TRF,
        ReqdExctnDt = DateTime.Now.AddDays(1)
        // Add more details...
    }
};

// Serialize to XML
var serializer = new XmlSerializer(typeof(Document));
using var writer = new Utf8StringWriter();
serializer.Serialize(writer, document);
string xml = writer.ToString();
```

## Best Practices

1. **Keep Original XSD Files**: Maintain the original XSD files in your project for reference and validation.
2. **Separate Generated Code**: Keep generated code in a distinct namespace or folder.
3. **Avoid Modifying Generated Code**: Don't modify the generated code directly; it may be overwritten when regenerating.
4. **Create Builder Classes**: Use the builder pattern to simplify the creation of complex ISO 20022 messages.
5. **Validate XML**: Always validate the produced XML against the original XSD schema.
