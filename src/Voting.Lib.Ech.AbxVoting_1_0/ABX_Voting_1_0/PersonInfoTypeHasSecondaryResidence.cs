//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// This code was generated by XmlSchemaClassGenerator version 2.1.963.0
namespace ABX_Voting_1_0
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.1.963.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("PersonInfoTypeHasSecondaryResidence", Namespace="http://www.abraxas.ch/xmlns/ABX-Voting/1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class PersonInfoTypeHasSecondaryResidence : ResidenceType
    {
        
        [System.Xml.Serialization.XmlElementAttribute("mainResidence", Order=0)]
        public Ech0007_6_0.SwissMunicipalityType MainResidence { get; set; }
    }
}