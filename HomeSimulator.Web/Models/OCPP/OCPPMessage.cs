namespace HomeSimulator.Web.Models.OCPP;

using System.Text.Json.Serialization;

public class OCPPMessage
{
    /// <summary>
    /// Message type
    /// </summary>
    public string MessageType { get; set; }

    /// <summary>
    /// Message ID
    /// </summary>
    public string UniqueId { get; set; }

    /// <summary>
    /// Action
    /// </summary>
    public string Action { get; set; }

    /// <summary>
    /// JSON-Payload
    /// </summary>
    public string JsonPayload { get; set; }

    /// <summary>
    /// Error-Code
    /// </summary>
    public string ErrorCode { get; set; }

    /// <summary>
    /// Error-Description
    /// </summary>
    public string ErrorDescription { get; set; }

    /// <summary>
    /// TaskCompletionSource for asynchronous API result
    /// </summary>
    [JsonIgnore]
    public TaskCompletionSource<string> TaskCompletionSource { get; set; }


    /// <summary>
    /// Empty constructor
    /// </summary>
    public OCPPMessage()
    {
    }

    /// <summary>
    /// Constructor
    /// </summary>
    public OCPPMessage(string messageType, string uniqueId, string action, string jsonPayload)
    {
        MessageType = messageType;
        UniqueId = uniqueId;
        Action = action;
        JsonPayload = jsonPayload;
    }
}