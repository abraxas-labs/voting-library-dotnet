//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// This code was generated by XmlSchemaClassGenerator
namespace Ech0045_4_0
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("VotingPersonOnlyTypePerson", Namespace="http://www.ech.ch/xmlns/eCH-0045/4", AnonymousType=true)]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class VotingPersonOnlyTypePerson
    {
        
        [System.Xml.Serialization.XmlElementAttribute("swiss", Order=0)]
        public SwissDomesticType Swiss { get; set; }
        
        [System.Xml.Serialization.XmlElementAttribute("swissAbroad", Order=1)]
        public SwissAbroadType SwissAbroad { get; set; }
        
        [System.Xml.Serialization.XmlElementAttribute("foreigner", Order=2)]
        public ForeignerType Foreigner { get; set; }
    }
}
