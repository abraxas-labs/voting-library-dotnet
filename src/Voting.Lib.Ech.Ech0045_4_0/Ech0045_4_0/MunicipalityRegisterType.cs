//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// This code was generated by XmlSchemaClassGenerator version 2.1.963.0
namespace Ech0045_4_0
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.1.963.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("municipalityRegisterType", Namespace="http://www.ech.ch/xmlns/eCH-0045/4")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class MunicipalityRegisterType
    {
        
        /// <summary>
        /// <para xml:lang="en">Maximum length: 50.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(50)]
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("registerIdentification", Order=0)]
        public string RegisterIdentification { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Maximum length: 40.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(40)]
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("municipalityName", Order=1)]
        public string MunicipalityName { get; set; }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("cantonAbbreviation", Order=2)]
        public Ech0007_6_0.CantonAbbreviationType CantonAbbreviationValue { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the CantonAbbreviation property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool CantonAbbreviationValueSpecified { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<Ech0007_6_0.CantonAbbreviationType> CantonAbbreviation
        {
            get
            {
                if (this.CantonAbbreviationValueSpecified)
                {
                    return this.CantonAbbreviationValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.CantonAbbreviationValue = value.GetValueOrDefault();
                this.CantonAbbreviationValueSpecified = value.HasValue;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Maximum length: 100.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(100)]
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("registerName", Order=3)]
        public string RegisterName { get; set; }
    }
}