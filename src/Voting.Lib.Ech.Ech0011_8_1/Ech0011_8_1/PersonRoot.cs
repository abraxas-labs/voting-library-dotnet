//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// This code was generated by XmlSchemaClassGenerator version 2.1.963.0
namespace Ech0011_8_1
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.1.963.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("personRoot", Namespace="http://www.ech.ch/xmlns/eCH-0011/8", AnonymousType=true)]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("personRoot", Namespace="http://www.ech.ch/xmlns/eCH-0011/8")]
    public partial class PersonRoot
    {
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("reportedPerson", Order=0)]
        public ReportedPersonType ReportedPerson { get; set; }
    }
}