//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// This code was generated by XmlSchemaClassGenerator
namespace Ech0011_8_1
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("nationalityDataType", Namespace="http://www.ech.ch/xmlns/eCH-0011/8")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class NationalityDataType
    {
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("nationalityStatus", Order=0)]
        public NationalityStatusType NationalityStatus { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<NationalityDataTypeCountryInfo> _countryInfo;
        
        [System.Xml.Serialization.XmlElementAttribute("countryInfo", Order=1)]
        public System.Collections.Generic.List<NationalityDataTypeCountryInfo> CountryInfo
        {
            get
            {
                return _countryInfo;
            }
            set
            {
                _countryInfo = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the CountryInfo collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CountryInfoSpecified
        {
            get
            {
                return (this.CountryInfo != null);
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="NationalityDataType" /> class.</para>
        /// </summary>
        public NationalityDataType()
        {
            this._countryInfo = new System.Collections.Generic.List<NationalityDataTypeCountryInfo>();
        }
    }
}
