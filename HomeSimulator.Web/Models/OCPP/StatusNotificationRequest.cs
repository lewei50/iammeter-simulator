﻿namespace HomeSimulator.Web.Models.OCPP
{
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.3.1.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class StatusNotificationRequest
    {
        [Newtonsoft.Json.JsonProperty("connectorId", Required = Newtonsoft.Json.Required.Always)]
        public int ConnectorId { get; set; }

        [Newtonsoft.Json.JsonProperty("errorCode", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public StatusNotificationRequestErrorCode ErrorCode { get; set; }

        [Newtonsoft.Json.JsonProperty("info", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.StringLength(50)]
        public string Info { get; set; }

        [Newtonsoft.Json.JsonProperty("status", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public StatusNotificationRequestStatus Status { get; set; }

        [Newtonsoft.Json.JsonProperty("timestamp", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset? Timestamp { get; set; }

        [Newtonsoft.Json.JsonProperty("vendorId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.StringLength(255)]
        public string VendorId { get; set; }

        [Newtonsoft.Json.JsonProperty("vendorErrorCode", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.StringLength(50)]
        public string VendorErrorCode { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.3.1.0 (Newtonsoft.Json v9.0.0.0)")]
    public enum StatusNotificationRequestErrorCode
    {
        [System.Runtime.Serialization.EnumMember(Value = @"ConnectorLockFailure")]
        ConnectorLockFailure = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"EVCommunicationError")]
        EVCommunicationError = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"GroundFailure")]
        GroundFailure = 2,

        [System.Runtime.Serialization.EnumMember(Value = @"HighTemperature")]
        HighTemperature = 3,

        [System.Runtime.Serialization.EnumMember(Value = @"InternalError")]
        InternalError = 4,

        [System.Runtime.Serialization.EnumMember(Value = @"LocalListConflict")]
        LocalListConflict = 5,

        [System.Runtime.Serialization.EnumMember(Value = @"NoError")]
        NoError = 6,

        [System.Runtime.Serialization.EnumMember(Value = @"OtherError")]
        OtherError = 7,

        [System.Runtime.Serialization.EnumMember(Value = @"OverCurrentFailure")]
        OverCurrentFailure = 8,

        [System.Runtime.Serialization.EnumMember(Value = @"PowerMeterFailure")]
        PowerMeterFailure = 9,

        [System.Runtime.Serialization.EnumMember(Value = @"PowerSwitchFailure")]
        PowerSwitchFailure = 10,

        [System.Runtime.Serialization.EnumMember(Value = @"ReaderFailure")]
        ReaderFailure = 11,

        [System.Runtime.Serialization.EnumMember(Value = @"ResetFailure")]
        ResetFailure = 12,

        [System.Runtime.Serialization.EnumMember(Value = @"UnderVoltage")]
        UnderVoltage = 13,

        [System.Runtime.Serialization.EnumMember(Value = @"OverVoltage")]
        OverVoltage = 14,

        [System.Runtime.Serialization.EnumMember(Value = @"WeakSignal")]
        WeakSignal = 15,

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.3.1.0 (Newtonsoft.Json v9.0.0.0)")]
    public enum StatusNotificationRequestStatus
    {
        [System.Runtime.Serialization.EnumMember(Value = @"Available")]
        Available = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"Preparing")]
        Preparing = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"Charging")]
        Charging = 2,

        [System.Runtime.Serialization.EnumMember(Value = @"SuspendedEVSE")]
        SuspendedEVSE = 3,

        [System.Runtime.Serialization.EnumMember(Value = @"SuspendedEV")]
        SuspendedEV = 4,

        [System.Runtime.Serialization.EnumMember(Value = @"Finishing")]
        Finishing = 5,

        [System.Runtime.Serialization.EnumMember(Value = @"Reserved")]
        Reserved = 6,

        [System.Runtime.Serialization.EnumMember(Value = @"Unavailable")]
        Unavailable = 7,

        [System.Runtime.Serialization.EnumMember(Value = @"Faulted")]
        Faulted = 8,

    }
    public class StatusNotificationResponse
    {
    }
}