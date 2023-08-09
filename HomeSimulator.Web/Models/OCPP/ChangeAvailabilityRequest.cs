namespace HomeSimulator.Web.Models.OCPP
{
#pragma warning disable // Disable all warnings

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class ChangeAvailabilityRequest
    {
        [Newtonsoft.Json.JsonProperty("connectorId", Required = Newtonsoft.Json.Required.Always)]
        public int ConnectorId { get; set; }

        [Newtonsoft.Json.JsonProperty("type", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public ChangeAvailabilityRequestType Type { get; set; }


    }
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public enum ChangeAvailabilityRequestType
    {

        [System.Runtime.Serialization.EnumMember(Value = @"Inoperative")]
        Inoperative = 0,


        [System.Runtime.Serialization.EnumMember(Value = @"Operative")]
        Operative = 1,
        

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class ChangeAvailabilityResponse
    {
        [Newtonsoft.Json.JsonProperty("status", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public ChangeAvailabilityResponseStatus Status { get; set; }
        
        
    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public enum ChangeAvailabilityResponseStatus
    {

        [System.Runtime.Serialization.EnumMember(Value = @"Accepted")]
        Accepted = 0,


        [System.Runtime.Serialization.EnumMember(Value = @"Rejected")]
        Rejected = 1,


        [System.Runtime.Serialization.EnumMember(Value = @"Scheduled")]
        Scheduled = 2,


    }
}