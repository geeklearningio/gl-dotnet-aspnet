namespace GeekLearning.AspNetCore.FlashMessage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IFlashMessageManager
    {
        void Push(FlashMessage message);
        IEnumerable<FlashMessage> Peek();
    }
}
