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
    [System.Xml.Serialization.XmlTypeAttribute("cantonalRegisterType", Namespace="http://www.ech.ch/xmlns/eCH-0045/4")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CantonalRegisterType
    {
        
        /// <summary>
        /// <para xml:lang="en">Maximum length: 50.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(50)]
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("registerIdentification", Order=0)]
        public string RegisterIdentification { get; set; }
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("cantonAbbreviation", Order=1)]
        public Ech0007_6_0.CantonAbbreviationType CantonAbbreviation { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Maximum length: 100.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(100)]
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("registerName", Order=2)]
        public string RegisterName { get; set; }
    }
}