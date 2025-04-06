using Iso20022Library.Application.Builders;
using Iso20022Library.Domain.Common.Enums;
using Iso20022Library.Domain.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iso20022Library.Tests.Builders
{
    [TestClass]
    public class MessageBuilderFactoryTests
    {
        [TestMethod]
        public void GetBuilder_ShouldReturnBuilder_WhenMessageTypeIsKnown()
        {
            var factory = new MessageBuilderFactory();
            IMessageBuilder builder = factory.GetBuilder(MessageType.Pain00100102);
            Assert.IsNotNull(builder);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetBuilder_ShouldThrow_WhenMessageTypeIsUnknown()
        {
            var factory = new MessageBuilderFactory();
            _ = factory.GetBuilder((MessageType)999); // tipo inválido
        }
    }

}
