namespace TestAoniken.Models
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public class MessageEntity
    {
        public string Content { get; set; }
        public string ContentType { get; set; }
        public string To { get; set; }
        public TipoProveedorEnum Proveedor { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TipoOperacionEnum EventType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TipoOperacionRolEnum RolType { get; set; }
    }

    public enum TipoOperacionEnum
    {
        Payment,
        CompraTienda,
        Cancel,
        CuelguePico,
        HealthBeat,
        Refund,
        Notification,
        MerchantOrder
    }

    public enum TipoOperacionRolEnum
    {
        None,
        Playa,
        Tienda,
        Lubricantes,
        Gnc
    }

    public enum TipoProveedorEnum
    {
        MercadoLibre,
        Yvos,
        TodoPago,
        Aoniken,
        ShellBox,
        Freemoni,
        PlusPagos,
        Trafigura,
        Weigo,
        MercadoPagoPoint
    }
}
