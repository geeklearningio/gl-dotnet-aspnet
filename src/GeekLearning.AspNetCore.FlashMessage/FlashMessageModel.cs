namespace GeekLearning.AspNetCore
{
    /// <summary>
    /// The FlashMessageModel class represents an individual flash message.
    /// </summary>
    public class FlashMessageModel
    {
        public string Title { get; set; }

        public string Message { get; set; }

        public FlashMessageType Type { get; set; }

        public FlashMessageModel()
        {
            Type = FlashMessageType.Info;
        }
    }

    /// <summary>
    /// The FlashMessageType enum indicates the type of flash message.
    /// </summary>
    public enum FlashMessageType : byte
    {
        Success,
        Info,
        Warning,
        Error,
    }
}
