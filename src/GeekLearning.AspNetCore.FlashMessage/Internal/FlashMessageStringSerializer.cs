namespace GeekLearning.AspNetCore.FlashMessage.Internal
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class FlashMessageStringSerializer
    {
        /// <summary>
        /// Deserializes a serialized collection of flash messages.
        /// </summary>
        /// <param name="serializedMessages"></param>
        /// <returns></returns>
        public static IEnumerable<FlashMessage> Deserialize(byte[] data)
        {
            var messages = new List<FlashMessage>();
            int messageCount;

            // Check if there is any data to read, if not we are done quickly.
            if (data.Length == 0)
            {
                return messages;
            }

            using (MemoryStream stream = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    // Read the number of message in the stream and deserialize each message.
                    messageCount = reader.ReadInt32();
                    while (messageCount > 0)
                    {
                        var model = new FlashMessage();
                        model.Message = reader.ReadString();
                        model.Title = reader.ReadString();
                        model.Type = (FlashMessageType)reader.ReadByte();
                        var actionCount = reader.ReadByte();
                        model.Actions = new List<FlashMessageAction>();
                        for (byte i = 0; i < actionCount; i++)
                        {
                            model.Actions.Add(new FlashMessageAction
                            {
                                Title = reader.ReadString(),
                                Action = reader.ReadString()
                            });
                        }

                        // Store message and decrement message counter.
                        messages.Add(model);
                        messageCount--;
                    }

                    return messages;
                }
            }
        }

        /// <summary>
        /// Serializes the passed list of messages to binary format.
        /// </summary>
        /// <param name="messages"></param>
        /// <returns></returns>
        public static byte[] Serialize(IEnumerable<FlashMessage> messages)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    // Write the number of message and serialize each message.
                    writer.Write(messages.Count());
                    foreach (var message in messages)
                    {
                        writer.Write(message.Message);
                        writer.Write(message.Title);
                        writer.Write((byte)message.Type);
                        writer.Write((byte)message.Actions.Count);
                        foreach (var action in message.Actions)
                        {
                            writer.Write(action.Title);
                            writer.Write(action.Action);
                        }
                    }

                    // Return the data as a byte array.
                    writer.Flush();
                    return stream.ToArray();
                }
            }
        }
    }
}
