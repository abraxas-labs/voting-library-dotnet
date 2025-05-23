//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// This code was generated by XmlSchemaClassGenerator
namespace Ech0010_5_1
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("swissAddressInformationType", Namespace="http://www.ech.ch/xmlns/eCH-0010/5")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("swissAddressInformationType", Namespace="http://www.ech.ch/xmlns/eCH-0010/5")]
    public partial class SwissAddressInformationType
    {
        
        /// <summary>
        /// <para xml:lang="en">Maximum length: 60.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(60)]
        [System.Xml.Serialization.XmlElementAttribute("addressLine1", Order=0)]
        public string AddressLine1 { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Maximum length: 60.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(60)]
        [System.Xml.Serialization.XmlElementAttribute("addressLine2", Order=1)]
        public string AddressLine2 { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Maximum length: 60.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(60)]
        [System.Xml.Serialization.XmlElementAttribute("street", Order=2)]
        public string Street { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Maximum length: 12.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(12)]
        [System.Xml.Serialization.XmlElementAttribute("houseNumber", Order=3)]
        public string HouseNumber { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Maximum length: 10.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(10)]
        [System.Xml.Serialization.XmlElementAttribute("dwellingNumber", Order=4)]
        public string DwellingNumber { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Maximum length: 40.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(40)]
        [System.Xml.Serialization.XmlElementAttribute("locality", Order=5)]
        public string Locality { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Maximum length: 40.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(40)]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("town", Order=6)]
        public string Town { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Minimum inclusive value: 1000.</para>
        /// <para xml:lang="en">Maximum inclusive value: 9999.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.RangeAttribute(typeof(uint), "1000", "9999", ConvertValueInInvariantCulture=true)]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("swissZipCode", Order=7)]
        public uint SwissZipCode { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Maximum length: 2.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(2)]
        [System.Xml.Serialization.XmlElementAttribute("swissZipCodeAddOn", Order=8)]
        public string SwissZipCodeAddOn { get; set; }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("swissZipCodeId", Order=9)]
        public int SwissZipCodeIdValue { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the SwissZipCodeId property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool SwissZipCodeIdValueSpecified { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<int> SwissZipCodeId
        {
            get
            {
                if (this.SwissZipCodeIdValueSpecified)
                {
                    return this.SwissZipCodeIdValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.SwissZipCodeIdValue = value.GetValueOrDefault();
                this.SwissZipCodeIdValueSpecified = value.HasValue;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// <para xml:lang="en">Maximum length: 2.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(2)]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("country", Order=10)]
        public string Country { get; set; }
    }
}
