namespace HomeSimulator.Web.Models.OCPP
{
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class SetChargingProfileRequest
    {
        [Newtonsoft.Json.JsonProperty("connectorId", Required = Newtonsoft.Json.Required.Always)]
        public int ConnectorId { get; set; }

        [Newtonsoft.Json.JsonProperty("csChargingProfiles", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public CsChargingProfiles CsChargingProfiles { get; set; } = new CsChargingProfiles();


    }



    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class CsChargingProfiles
    {
        [Newtonsoft.Json.JsonProperty("chargingProfileId", Required = Newtonsoft.Json.Required.Always)]
        public int ChargingProfileId { get; set; }

        [Newtonsoft.Json.JsonProperty("transactionId", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int? TransactionId { get; set; }

        [Newtonsoft.Json.JsonProperty("stackLevel", Required = Newtonsoft.Json.Required.Always)]
        public int StackLevel { get; set; }

        [Newtonsoft.Json.JsonProperty("chargingProfilePurpose", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public CsChargingProfilesChargingProfilePurpose ChargingProfilePurpose { get; set; }

        [Newtonsoft.Json.JsonProperty("chargingProfileKind", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public CsChargingProfilesChargingProfileKind ChargingProfileKind { get; set; }

        [Newtonsoft.Json.JsonProperty("recurrencyKind", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public CsChargingProfilesRecurrencyKind RecurrencyKind { get; set; }

        [Newtonsoft.Json.JsonProperty("validFrom", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset? ValidFrom { get; set; }

        [Newtonsoft.Json.JsonProperty("validTo", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset? ValidTo { get; set; }

        [Newtonsoft.Json.JsonProperty("chargingSchedule", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public ChargingSchedule ChargingSchedule { get; set; } = new ChargingSchedule();


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public enum CsChargingProfilesChargingProfilePurpose
    {

        [System.Runtime.Serialization.EnumMember(Value = @"ChargePointMaxProfile")]
        ChargePointMaxProfile = 0,


        [System.Runtime.Serialization.EnumMember(Value = @"TxDefaultProfile")]
        TxDefaultProfile = 1,


        [System.Runtime.Serialization.EnumMember(Value = @"TxProfile")]
        TxProfile = 2,


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public enum CsChargingProfilesChargingProfileKind
    {

        [System.Runtime.Serialization.EnumMember(Value = @"Absolute")]
        Absolute = 0,


        [System.Runtime.Serialization.EnumMember(Value = @"Recurring")]
        Recurring = 1,


        [System.Runtime.Serialization.EnumMember(Value = @"Relative")]
        Relative = 2,


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public enum CsChargingProfilesRecurrencyKind
    {

        [System.Runtime.Serialization.EnumMember(Value = @"Daily")]
        Daily = 0,


        [System.Runtime.Serialization.EnumMember(Value = @"Weekly")]
        Weekly = 1,


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class ChargingSchedule
    {
        [Newtonsoft.Json.JsonProperty("duration", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Duration { get; set; }

        [Newtonsoft.Json.JsonProperty("startSchedule", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset? StartSchedule { get; set; }

        [Newtonsoft.Json.JsonProperty("chargingRateUnit", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public ChargingScheduleChargingRateUnit ChargingRateUnit { get; set; }

        [Newtonsoft.Json.JsonProperty("chargingSchedulePeriod", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<ChargingSchedulePeriod> ChargingSchedulePeriod { get; set; } = new System.Collections.ObjectModel.Collection<ChargingSchedulePeriod>();

        [Newtonsoft.Json.JsonProperty("minChargingRate", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public double MinChargingRate { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public enum ChargingScheduleChargingRateUnit
    {

        [System.Runtime.Serialization.EnumMember(Value = @"A")]
        A = 0,


        [System.Runtime.Serialization.EnumMember(Value = @"W")]
        W = 1,


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class ChargingSchedulePeriod
    {
        [Newtonsoft.Json.JsonProperty("startPeriod", Required = Newtonsoft.Json.Required.Always)]
        public int StartPeriod { get; set; }

        [Newtonsoft.Json.JsonProperty("limit", Required = Newtonsoft.Json.Required.Always)]
        public double Limit { get; set; }

        [Newtonsoft.Json.JsonProperty("numberPhases", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int NumberPhases { get; set; }


    }


    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class SetChargingProfileResponse
    {
        [Newtonsoft.Json.JsonProperty("status", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public SetChargingProfileResponseStatus Status { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public enum SetChargingProfileResponseStatus
    {

        [System.Runtime.Serialization.EnumMember(Value = @"Accepted")]
        Accepted = 0,


        [System.Runtime.Serialization.EnumMember(Value = @"Rejected")]
        Rejected = 1,


        [System.Runtime.Serialization.EnumMember(Value = @"NotSupported")]
        NotSupported = 2,


    }
}