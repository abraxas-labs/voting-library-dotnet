//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// This code was generated by XmlSchemaClassGenerator
namespace Ech0157_4_0
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("EventInitialDeliveryElectionGroupBallot", Namespace="http://www.ech.ch/xmlns/eCH-0157/4", AnonymousType=true)]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class EventInitialDeliveryElectionGroupBallot
    {
        
        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// <para xml:lang="en">Maximum length: 50.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(50)]
        [System.Xml.Serialization.XmlElementAttribute("electionGroupIdentification", Order=0)]
        public string ElectionGroupIdentification { get; set; }
        
        [System.Xml.Serialization.XmlElementAttribute("electionGroupDescription", Order=1)]
        public Ech0155_4_0.ElectionGroupDescriptionType ElectionGroupDescription { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// <para xml:lang="en">Maximum length: 50.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(50)]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("domainOfInfluenceIdentification", Order=2)]
        public string DomainOfInfluenceIdentification { get; set; }
        
        [System.Xml.Serialization.XmlElementAttribute("electionGroupPosition", Order=3)]
        public string ElectionGroupPosition { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<EventInitialDeliveryElectionGroupBallotElectionInformation> _electionInformation;
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("electionInformation", Order=4)]
        public System.Collections.Generic.List<EventInitialDeliveryElectionGroupBallotElectionInformation> ElectionInformation
        {
            get
            {
                return _electionInformation;
            }
            set
            {
                _electionInformation = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="EventInitialDeliveryElectionGroupBallot" /> class.</para>
        /// </summary>
        public EventInitialDeliveryElectionGroupBallot()
        {
            this._electionInformation = new System.Collections.Generic.List<EventInitialDeliveryElectionGroupBallotElectionInformation>();
        }
    }
}
