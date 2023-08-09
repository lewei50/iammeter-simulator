namespace HomeSimulator.Web.Models.OCPP
{
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class RemoteStartTransactionRequest
    {
        [Newtonsoft.Json.JsonProperty("connectorId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int ConnectorId { get; set; }

        [Newtonsoft.Json.JsonProperty("idTag", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [System.ComponentModel.DataAnnotations.StringLength(20)]
        public string IdTag { get; set; }

        [Newtonsoft.Json.JsonProperty("chargingProfile", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public ChargingProfile ChargingProfile { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class ChargingProfile
    {
        [Newtonsoft.Json.JsonProperty("chargingProfileId", Required = Newtonsoft.Json.Required.Always)]
        public int ChargingProfileId { get; set; }

        [Newtonsoft.Json.JsonProperty("transactionId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int TransactionId { get; set; }

        [Newtonsoft.Json.JsonProperty("stackLevel", Required = Newtonsoft.Json.Required.Always)]
        public int StackLevel { get; set; }

        [Newtonsoft.Json.JsonProperty("chargingProfilePurpose", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public ChargingProfilePurpose ChargingProfilePurpose { get; set; }

        [Newtonsoft.Json.JsonProperty("chargingProfileKind", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public ChargingProfileKind ChargingProfileKind { get; set; }

        [Newtonsoft.Json.JsonProperty("recurrencyKind", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public ChargingProfileRecurrencyKind RecurrencyKind { get; set; }

        [Newtonsoft.Json.JsonProperty("validFrom", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset ValidFrom { get; set; }

        [Newtonsoft.Json.JsonProperty("validTo", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset ValidTo { get; set; }

        [Newtonsoft.Json.JsonProperty("chargingSchedule", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public ChargingSchedule ChargingSchedule { get; set; } = new ChargingSchedule();


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public enum ChargingProfilePurpose
    {

        [System.Runtime.Serialization.EnumMember(Value = @"ChargePointMaxProfile")]
        ChargePointMaxProfile = 0,


        [System.Runtime.Serialization.EnumMember(Value = @"TxDefaultProfile")]
        TxDefaultProfile = 1,


        [System.Runtime.Serialization.EnumMember(Value = @"TxProfile")]
        TxProfile = 2,


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public enum ChargingProfileKind
    {

        [System.Runtime.Serialization.EnumMember(Value = @"Absolute")]
        Absolute = 0,


        [System.Runtime.Serialization.EnumMember(Value = @"Recurring")]
        Recurring = 1,


        [System.Runtime.Serialization.EnumMember(Value = @"Relative")]
        Relative = 2,


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public enum ChargingProfileRecurrencyKind
    {

        [System.Runtime.Serialization.EnumMember(Value = @"Daily")]
        Daily = 0,


        [System.Runtime.Serialization.EnumMember(Value = @"Weekly")]
        Weekly = 1,


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class RemoteStartTransactionResponse
    {
        [Newtonsoft.Json.JsonProperty("status", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public RemoteStartTransactionResponseStatus Status { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public enum RemoteStartTransactionResponseStatus
    {

        [System.Runtime.Serialization.EnumMember(Value = @"Accepted")]
        Accepted = 0,


        [System.Runtime.Serialization.EnumMember(Value = @"Rejected")]
        Rejected = 1,


    }
}