
namespace GeekLearning.AspNetCore.FlashMessage
{
    using System;
    using System.Collections.Generic;
    /// <summary>
    /// The FlashMessag class represents an individual flash message.
    /// </summary>
    public class FlashMessage
    {
        public string Title { get; set; }

        public string Message { get; set; }

        public FlashMessageType Type { get; set; }

        public IList<FlashMessageAction> Actions { get; set; }

        public FlashMessage()
        {
            Type = FlashMessageType.Info;
        }
    }

    public class FlashMessageAction {
        public string Title { get; set; }
        public string Action { get; set; }
    }

    /// <summary>
    /// The FlashMessageType enum indicates the type of flash message.
    /// </summary>
    [Flags]
    public enum FlashMessageType : byte
    {
        Success = 1,
        Info = 2,
        Warning = 4,
        Error = 8,
        Modal = 16
    }
}
