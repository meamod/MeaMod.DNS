namespace MeaMod.DNS.Model
{
    /// <summary>
    ///   Contains the IPv4 address of the named resource.
    /// </summary>
    public class ARecord : AddressRecord
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="ARecord"/> class.
        /// </summary>
        public ARecord() : base()
        {
            Type = DnsType.A;
        }

    }
}
