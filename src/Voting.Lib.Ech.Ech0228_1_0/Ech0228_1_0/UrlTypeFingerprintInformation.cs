//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// This code was generated by XmlSchemaClassGenerator
namespace Ech0228_1_0
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("UrlTypeFingerprintInformation", Namespace="http://www.ech.ch/xmlns/eCH-0228/1", AnonymousType=true)]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class UrlTypeFingerprintInformation
    {
        
        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// <para xml:lang="en">Maximum length: 255.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(255)]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("eVotingFingerprint", Order=0)]
        public string EVotingFingerprint { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<UrlTypeFingerprintInformationFingerprintDesignation> _fingerprintDesignation;
        
        [System.Xml.Serialization.XmlElementAttribute("fingerprintDesignation", Order=1)]
        public System.Collections.Generic.List<UrlTypeFingerprintInformationFingerprintDesignation> FingerprintDesignation
        {
            get
            {
                return _fingerprintDesignation;
            }
            set
            {
                _fingerprintDesignation = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the FingerprintDesignation collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool FingerprintDesignationSpecified
        {
            get
            {
                return (this.FingerprintDesignation != null);
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="UrlTypeFingerprintInformation" /> class.</para>
        /// </summary>
        public UrlTypeFingerprintInformation()
        {
            this._fingerprintDesignation = new System.Collections.Generic.List<UrlTypeFingerprintInformationFingerprintDesignation>();
        }
    }
}
