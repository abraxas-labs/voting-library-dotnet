//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// This code was generated by XmlSchemaClassGenerator
namespace Ech0159_4_0
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("eventInitialDelivery", Namespace="http://www.ech.ch/xmlns/eCH-0159/4")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class EventInitialDelivery
    {
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("contest", Order=0)]
        public Ech0155_4_0.ContestType Contest { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<EventInitialDeliveryVoteInformation> _voteInformation;
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("voteInformation", Order=1)]
        public System.Collections.Generic.List<EventInitialDeliveryVoteInformation> VoteInformation
        {
            get
            {
                return _voteInformation;
            }
            set
            {
                _voteInformation = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="EventInitialDelivery" /> class.</para>
        /// </summary>
        public EventInitialDelivery()
        {
            this._voteInformation = new System.Collections.Generic.List<EventInitialDeliveryVoteInformation>();
        }
        
        [System.Xml.Serialization.XmlElementAttribute("extension", Order=2)]
        public Ech0155_4_0.ExtensionType Extension { get; set; }
    }
}
