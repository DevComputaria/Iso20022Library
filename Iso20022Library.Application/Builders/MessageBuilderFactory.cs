using Iso20022Library.Domain.Common.Enums;
using Iso20022Library.Domain.Common.Interfaces;

namespace Iso20022Library.Application.Builders;

/// <summary>
/// Fábrica responsável por fornecer o builder apropriado para cada tipo de mensagem ISO 20022.
/// </summary>
public class MessageBuilderFactory
{
    /// <summary>
    /// Dicionário interno de builders registrados por tipo de mensagem.
    /// </summary>
    private readonly Dictionary<MessageType, IMessageBuilder> _builders = new()
    {
        { MessageType.Pain00100103, new Pain00100102Builder() }
    };

    /// <summary>
    /// Retorna o builder correspondente ao tipo de mensagem especificado.
    /// </summary>
    /// <param name="type">Tipo da mensagem ISO 20022.</param>
    /// <returns>Instância de <see cref="IMessageBuilder"/> correspondente ao tipo solicitado.</returns>
    /// <exception cref="NotSupportedException">Lançada quando não há builder registrado para o tipo fornecido.</exception>
    public IMessageBuilder GetBuilder(MessageType type)
    {
        if (_builders.TryGetValue(type, out var builder))
            return builder;

        throw new NotSupportedException($"Message type {type} not supported.");
    }
}
