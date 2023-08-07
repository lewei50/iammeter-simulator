namespace HomeSimulator.Web.Models.OCPP
{
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.3.1.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class StartTransactionRequest
    {
        [Newtonsoft.Json.JsonProperty("connectorId", Required = Newtonsoft.Json.Required.Always)]
        public int ConnectorId { get; set; }

        [Newtonsoft.Json.JsonProperty("idTag", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [System.ComponentModel.DataAnnotations.StringLength(20)]
        public string IdTag { get; set; }

        [Newtonsoft.Json.JsonProperty("meterStart", Required = Newtonsoft.Json.Required.Always)]
        public int MeterStart { get; set; }

        [Newtonsoft.Json.JsonProperty("reservationId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int ReservationId { get; set; }

        [Newtonsoft.Json.JsonProperty("timestamp", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public System.DateTimeOffset Timestamp { get; set; }


    }
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.3.1.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class StartTransactionResponse
    {
        [Newtonsoft.Json.JsonProperty("idTagInfo", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public IdTagInfo IdTagInfo { get; set; } = new IdTagInfo();

        [Newtonsoft.Json.JsonProperty("transactionId", Required = Newtonsoft.Json.Required.Always)]
        public int TransactionId { get; set; }
    }
}