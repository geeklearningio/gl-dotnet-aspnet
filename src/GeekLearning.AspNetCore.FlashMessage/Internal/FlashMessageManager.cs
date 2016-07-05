using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekLearning.AspNetCore.FlashMessage.Internal
{
    public class FlashMessageManager : IFlashMessageManager
    {
        private IEnumerable<FlashMessage> incomingMessages = Enumerable.Empty<FlashMessage>();
        private List<FlashMessage> outgoingMessages = new List<FlashMessage>();

        public IEnumerable<FlashMessage> Peek()
        {
            var result = incomingMessages;
            incomingMessages = Enumerable.Empty<FlashMessage>();
            return result;
        }

        public void Incoming(IEnumerable<FlashMessage> messages)
        {
            if (messages != null)
            {
                this.incomingMessages = messages;
            }
        }

        public IEnumerable<FlashMessage> Outgoing()
        {
            return outgoingMessages.Concat(incomingMessages);
        }

        public void Push(FlashMessage message)
        {
            this.outgoingMessages.Add(message);
        }
    }
}
