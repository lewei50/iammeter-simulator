namespace HomeSimulator.Web.Models.OCPP
{
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class GetConfigurationRequest
    {
        [Newtonsoft.Json.JsonProperty("key", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public ICollection<string> Key { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class GetConfigurationResponse
    {
        [Newtonsoft.Json.JsonProperty("configurationKey", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<ConfigurationKey> ConfigurationKey { get; set; }

        [Newtonsoft.Json.JsonProperty("unknownKey", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<string> UnknownKey { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.9.0.0 (Newtonsoft.Json v9.0.0.0)")]
    public partial class ConfigurationKey
    {
        [Newtonsoft.Json.JsonProperty("key", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [System.ComponentModel.DataAnnotations.StringLength(50)]
        public string Key { get; set; }

        [Newtonsoft.Json.JsonProperty("readonly", Required = Newtonsoft.Json.Required.Always)]
        public bool Readonly { get; set; }

        [Newtonsoft.Json.JsonProperty("value", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.StringLength(500)]
        public string Value { get; set; }


    }
}