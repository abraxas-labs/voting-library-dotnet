//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// This code was generated by XmlSchemaClassGenerator
namespace Ech0021_7_0
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("HealthInsuranceDataTypeInsurance", Namespace="http://www.ech.ch/xmlns/eCH-0021/7", AnonymousType=true)]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class HealthInsuranceDataTypeInsurance
    {
        
        /// <summary>
        /// <para xml:lang="en">Maximum length: 100.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(100)]
        [System.Xml.Serialization.XmlElementAttribute("insuranceName", Order=0)]
        public string InsuranceName { get; set; }
        
        [System.Xml.Serialization.XmlElementAttribute("insuranceAddress", Order=1)]
        public Ech0010_5_1.OrganisationMailAddressType InsuranceAddress { get; set; }
    }
}
