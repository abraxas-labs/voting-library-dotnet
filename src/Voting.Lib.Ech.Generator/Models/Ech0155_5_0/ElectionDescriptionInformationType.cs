//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// This code was generated by XmlSchemaClassGenerator version 2.1.963.0
namespace Ech0155_5_0
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.1.963.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("electionDescriptionInformationType", Namespace="http://www.ech.ch/xmlns/eCH-0155/5")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ElectionGroupDescriptionType))]
    public partial class ElectionDescriptionInformationType
    {
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<ElectionDescriptionInformationTypeElectionDescriptionInfo> _electionDescriptionInfo;
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("electionDescriptionInfo", Order=0)]
        public System.Collections.Generic.List<ElectionDescriptionInformationTypeElectionDescriptionInfo> ElectionDescriptionInfo
        {
            get
            {
                return _electionDescriptionInfo;
            }
            set
            {
                _electionDescriptionInfo = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="ElectionDescriptionInformationType" /> class.</para>
        /// </summary>
        public ElectionDescriptionInformationType()
        {
            this._electionDescriptionInfo = new System.Collections.Generic.List<ElectionDescriptionInformationTypeElectionDescriptionInfo>();
        }
    }
}