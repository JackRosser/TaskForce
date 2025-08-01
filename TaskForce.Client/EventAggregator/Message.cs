namespace TaskForce.Client.EventAggregator
{
    public class Message
    {
        public Message() { }

        public string FunctionName { get; set; } = string.Empty;
        /// <summary>
        /// Gets or Sets the code.
        /// </summary>
        public string ErrorCode { get; set; } = string.Empty;
        /// <summary>
        /// Gets or Sets the description.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 0:Information
        /// 1:Warning
        /// 2:Error
        /// </summary>
        public MessageType MessageType { get; set; }

        /// <summary>
        /// true : exposed with message
        /// if message is readed will can be removed because just exposed
        /// </summary>
        public bool Read { get; set; }

        public void SetAsError(string functionName, string errorCode, string description)
        {
            Read = false;
            MessageType = MessageType.Error;
            FunctionName = functionName;
            ErrorCode = errorCode;
            Description = description;

        }
        public void SetAsSuccess(string functionName, string errorCode, string description)
        {
            Read = false;
            MessageType = MessageType.Success;
            FunctionName = functionName;
            ErrorCode = errorCode;
            Description = description;
        }
    }
}
