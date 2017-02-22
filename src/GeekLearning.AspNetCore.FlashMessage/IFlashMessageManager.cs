namespace GeekLearning.AspNetCore.FlashMessage
{
    using System.Collections.Generic;

    public interface IFlashMessageManager
    {
        void Push(FlashMessage message);

        IEnumerable<FlashMessage> Peek();
    }
}
