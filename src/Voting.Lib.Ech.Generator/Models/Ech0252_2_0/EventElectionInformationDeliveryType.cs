//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// This code was generated by XmlSchemaClassGenerator version 2.1.963.0
namespace Ech0252_2_0
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.1.963.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("eventElectionInformationDeliveryType", Namespace="http://www.ech.ch/xmlns/eCH-0252/2")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class EventElectionInformationDeliveryType
    {
        
        /// <summary>
        /// <para xml:lang="en">Minimum inclusive value: 1.</para>
        /// <para xml:lang="en">Maximum inclusive value: 26.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.RangeAttribute(typeof(decimal), "1", "26", ConvertValueInInvariantCulture=true)]
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("cantonId", Order=0)]
        public byte CantonId { get; set; }
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("pollingDay", Order=1, DataType="date")]
        public System.DateTime PollingDay { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<ElectionAssociationType> _electionAssociation;
        
        [System.Xml.Serialization.XmlElementAttribute("electionAssociation", Order=2)]
        public System.Collections.Generic.List<ElectionAssociationType> ElectionAssociation
        {
            get
            {
                return _electionAssociation;
            }
            set
            {
                _electionAssociation = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the ElectionAssociation collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ElectionAssociationSpecified
        {
            get
            {
                return ((this.ElectionAssociation != null) 
                            && (this.ElectionAssociation.Count != 0));
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="EventElectionInformationDeliveryType" /> class.</para>
        /// </summary>
        public EventElectionInformationDeliveryType()
        {
            this._electionAssociation = new System.Collections.Generic.List<ElectionAssociationType>();
            this._electionGroupInfo = new System.Collections.Generic.List<ElectionGroupInfoType>();
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<ElectionGroupInfoType> _electionGroupInfo;
        
        /// <summary>
        /// <para>There is always an electionGroup, if it is not needed to keep several elections together, there is only one election under it</para>
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("There is always an electionGroup, if it is not needed to keep several elections t" +
            "ogether, there is only one election under it")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("electionGroupInfo", Order=3)]
        public System.Collections.Generic.List<ElectionGroupInfoType> ElectionGroupInfo
        {
            get
            {
                return _electionGroupInfo;
            }
            set
            {
                _electionGroupInfo = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Minimum inclusive value: 1.</para>
        /// <para xml:lang="en">Maximum inclusive value: 999.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.RangeAttribute(typeof(decimal), "1", "999", ConvertValueInInvariantCulture=true)]
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("numberOfEntries", Order=4)]
        public ushort NumberOfEntries { get; set; }
        
        [System.Xml.Serialization.XmlElementAttribute("extension", Order=5)]
        public Ech0155_5_0.ExtensionType Extension { get; set; }
    }
}