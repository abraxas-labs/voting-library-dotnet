//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// This code was generated by XmlSchemaClassGenerator
namespace Ech0155_5_1
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("partyAffiliationInformationType", Namespace="http://www.ech.ch/xmlns/eCH-0155/5")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class PartyAffiliationInformationType
    {
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<PartyAffiliationInformationTypePartyAffiliationInfo> _partyAffiliationInfo;
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("partyAffiliationInfo", Order=0)]
        public System.Collections.Generic.List<PartyAffiliationInformationTypePartyAffiliationInfo> PartyAffiliationInfo
        {
            get
            {
                return _partyAffiliationInfo;
            }
            set
            {
                _partyAffiliationInfo = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="PartyAffiliationInformationType" /> class.</para>
        /// </summary>
        public PartyAffiliationInformationType()
        {
            this._partyAffiliationInfo = new System.Collections.Generic.List<PartyAffiliationInformationTypePartyAffiliationInfo>();
        }
    }
}
