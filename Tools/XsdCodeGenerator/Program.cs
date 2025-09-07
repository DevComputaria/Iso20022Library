using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace XsdCodeGenerator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Uso: XsdCodeGenerator <caminho-xsd> <namespace> <pasta-saida>");
                Console.WriteLine("Exemplo: XsdCodeGenerator pacs.002.001.11.xsd Iso20022Library.Messages.Payments.Pacs.Generated.Pacs00200111 ./Generated");
                return;
            }

            string xsdPath = args[0];
            string namespaceName = args[1];
            string outputPath = args[2];

            try
            {
                var generator = new XsdClassGenerator();
                generator.GenerateClasses(xsdPath, namespaceName, outputPath);
                Console.WriteLine("Classes geradas com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao gerar classes: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }
    }

    public class XsdClassGenerator
    {
        public void GenerateClasses(string xsdPath, string namespaceName, string outputPath)
        {
            if (!File.Exists(xsdPath))
            {
                throw new FileNotFoundException($"Arquivo XSD não encontrado: {xsdPath}");
            }

            // Criar pasta de saída se não existir
            Directory.CreateDirectory(outputPath);

            // Encontrar xsd.exe
            string xsdExePath = FindXsdExe();
            if (string.IsNullOrEmpty(xsdExePath))
            {
                throw new InvalidOperationException("xsd.exe não encontrado. Certifique-se de que o .NET Framework SDK está instalado.");
            }

            Console.WriteLine($"Usando xsd.exe: {xsdExePath}");
            Console.WriteLine($"Processando XSD: {xsdPath}");
            Console.WriteLine($"Namespace: {namespaceName}");
            Console.WriteLine($"Pasta de saída: {outputPath}");

            // Executar xsd.exe
            string outputFileName = Path.Combine(outputPath, GetOutputFileName(xsdPath));
            RunXsdExe(xsdExePath, xsdPath, namespaceName, outputFileName);

            // Pós-processar o arquivo gerado
            PostProcessGeneratedFile(outputFileName, namespaceName);

            Console.WriteLine($"Arquivo gerado: {outputFileName}");
        }

        private string FindXsdExe()
        {
            // Procurar em locais comuns do .NET Framework SDK
            string[] possiblePaths = {
                @"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools\xsd.exe",
                @"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.7.2 Tools\xsd.exe",
                @"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.7.1 Tools\xsd.exe",
                @"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.7 Tools\xsd.exe",
                @"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.2 Tools\xsd.exe",
                @"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\xsd.exe",
                @"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6 Tools\xsd.exe",
                @"C:\Program Files (x86)\Microsoft SDKs\Windows\v8.1A\bin\NETFX 4.5.1 Tools\xsd.exe",
                @"C:\Program Files (x86)\Microsoft SDKs\Windows\v8.0A\bin\NETFX 4.0 Tools\xsd.exe",
                @"C:\Program Files\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools\x64\xsd.exe",
                @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\xsd.exe",
                @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\xsd.exe"
            };

            foreach (string path in possiblePaths)
            {
                if (File.Exists(path))
                {
                    return path;
                }
            }

            // Tentar encontrar via PATH
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "where",
                        Arguments = "xsd.exe",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                if (process.ExitCode == 0 && !string.IsNullOrWhiteSpace(output))
                {
                    return output.Trim().Split('\n')[0].Trim();
                }
            }
            catch
            {
                // Ignorar erro
            }

            return string.Empty;
        }

        private string GetOutputFileName(string xsdPath)
        {
            string fileName = Path.GetFileNameWithoutExtension(xsdPath);
            // Converter de formato pacs.002.001.11 para pacs_002_001_11
            fileName = fileName.Replace('.', '_');
            return $"{fileName}.cs";
        }

        private void RunXsdExe(string xsdExePath, string xsdPath, string namespaceName, string outputFileName)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = xsdExePath,
                    Arguments = $"/c /n:{namespaceName} \"{xsdPath}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WorkingDirectory = Path.GetDirectoryName(outputFileName)
                }
            };

            Console.WriteLine($"Executando: {process.StartInfo.FileName} {process.StartInfo.Arguments}");

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (!string.IsNullOrEmpty(output))
            {
                Console.WriteLine("Output:");
                Console.WriteLine(output);
            }

            if (!string.IsNullOrEmpty(error))
            {
                Console.WriteLine("Error:");
                Console.WriteLine(error);
            }

            if (process.ExitCode != 0)
            {
                throw new InvalidOperationException($"xsd.exe falhou com código de saída {process.ExitCode}");
            }

            // O xsd.exe gera o arquivo na pasta de trabalho, mover para o local correto se necessário
            string generatedFile = Path.Combine(Path.GetDirectoryName(outputFileName)!, Path.GetFileNameWithoutExtension(xsdPath) + ".cs");
            if (File.Exists(generatedFile) && generatedFile != outputFileName)
            {
                if (File.Exists(outputFileName))
                {
                    File.Delete(outputFileName);
                }
                File.Move(generatedFile, outputFileName);
            }
        }

        private void PostProcessGeneratedFile(string filePath, string namespaceName)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Arquivo gerado não encontrado: {filePath}");
            }

            string content = File.ReadAllText(filePath, Encoding.UTF8);

            // Adicionar header de auto-generated
            string header = @"//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by xsd.exe tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

";

            // Adicionar usings necessários se não existirem
            if (!content.Contains("using System.Xml.Serialization;"))
            {
                content = content.Replace("using System;", "using System;\nusing System.Xml.Serialization;");
            }

            if (!content.Contains("using System.ComponentModel.DataAnnotations;"))
            {
                content = content.Replace("using System.Xml.Serialization;", "using System.Xml.Serialization;\nusing System.ComponentModel.DataAnnotations;");
            }

            // Corrigir namespace se necessário
            // Se o namespace passado contém pontos e o xsd.exe gerou um namespace simples, corrigir
            if (namespaceName.Contains(".") && content.Contains($"namespace {namespaceName.Split('.').Last()} {{"))
            {
                string simpleNamespace = namespaceName.Split('.').Last();
                content = content.Replace($"namespace {simpleNamespace} {{", $"namespace {namespaceName}\n{{");
                Console.WriteLine($"Namespace corrigido de '{simpleNamespace}' para '{namespaceName}'");
            }

            // Adicionar atributos Serializable apenas se não existirem
            // Primeiro, verificar se a classe já tem o atributo
            var classMatches = Regex.Matches(content, @"(\s+)(public\s+(?:partial\s+)?class\s+(\w+))", RegexOptions.Multiline);
            foreach (Match match in classMatches)
            {
                string classDeclaration = match.Value;
                string className = match.Groups[3].Value;
                
                // Verificar se há atributo Serializable antes desta classe (olhar 300 caracteres antes)
                int startSearch = Math.Max(0, match.Index - 300);
                string beforeClass = content.Substring(startSearch, match.Index - startSearch);
                
                // Se não encontrar [Serializable] ou [System.Serializable] antes da classe, adicionar
                if (!beforeClass.Contains("[Serializable]") && !beforeClass.Contains("[System.Serializable]"))
                {
                    content = content.Replace(match.Value, $"\n    [Serializable]{match.Value}");
                }
            }

            var enumMatches = Regex.Matches(content, @"(\s+)(public\s+enum\s+(\w+))", RegexOptions.Multiline);
            foreach (Match match in enumMatches)
            {
                string enumDeclaration = match.Value;
                string enumName = match.Groups[3].Value;
                
                // Verificar se há atributo Serializable antes deste enum (olhar 300 caracteres antes)
                int startSearch = Math.Max(0, match.Index - 300);
                string beforeEnum = content.Substring(startSearch, match.Index - startSearch);
                
                // Se não encontrar [Serializable] ou [System.Serializable] antes do enum, adicionar
                if (!beforeEnum.Contains("[Serializable]") && !beforeEnum.Contains("[System.Serializable]"))
                {
                    content = content.Replace(match.Value, $"\n    [Serializable]{match.Value}");
                }
            }

            // Adicionar comentários XML para propriedades
            content = Regex.Replace(content, @"(\s+)(public\s+[\w\[\]<>?]+\s+(\w+)\s*{\s*get;\s*set;\s*})", 
                m => $"{m.Groups[1].Value}/// <summary>\n{m.Groups[1].Value}/// {m.Groups[3].Value}\n{m.Groups[1].Value}/// </summary>\n{m.Groups[1].Value}{m.Groups[2].Value}");

            // Adicionar nullable reference types support
            if (!content.Contains("#nullable enable"))
            {
                content = content.Replace("using System.ComponentModel.DataAnnotations;", 
                    "using System.ComponentModel.DataAnnotations;\n\n#nullable enable");
            }

            // Preparar conteúdo final
            if (!content.StartsWith("//------------------------------------------------------------------------------"))
            {
                content = header + content;
            }

            File.WriteAllText(filePath, content, Encoding.UTF8);

            Console.WriteLine("Pós-processamento concluído:");
            Console.WriteLine("- Adicionado header auto-generated");
            Console.WriteLine("- Adicionados usings necessários");
            Console.WriteLine("- Adicionados atributos [Serializable]");
            Console.WriteLine("- Adicionados comentários XML");
            Console.WriteLine("- Habilitado nullable reference types");
        }
    }
}
