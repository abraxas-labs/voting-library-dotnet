//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// This code was generated by XmlSchemaClassGenerator
namespace Ech0098_4_0
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("deliveryAddressType", Namespace="http://www.ech.ch/xmlns/eCH-0098/4")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DeliveryAddressType
    {
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("deliveryAddress", Order=0)]
        public Ech0010_6_0.MailAddressType DeliveryAddress { get; set; }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("addressValidSince", Order=1, DataType="date")]
        public System.DateTime AddressValidSinceValue { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the AddressValidSince property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool AddressValidSinceValueSpecified { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<System.DateTime> AddressValidSince
        {
            get
            {
                if (this.AddressValidSinceValueSpecified)
                {
                    return this.AddressValidSinceValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.AddressValidSinceValue = value.GetValueOrDefault();
                this.AddressValidSinceValueSpecified = value.HasValue;
            }
        }
    }
}
