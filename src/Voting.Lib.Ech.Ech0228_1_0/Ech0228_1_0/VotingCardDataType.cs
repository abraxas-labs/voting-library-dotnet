//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// This code was generated by XmlSchemaClassGenerator
namespace Ech0228_1_0
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("votingCardDataType", Namespace="http://www.ech.ch/xmlns/eCH-0228/1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class VotingCardDataType
    {
        
        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// <para xml:lang="en">Maximum length: 50.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(50)]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("votingCardSequenceNumber", Order=0)]
        public string VotingCardSequenceNumber { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// <para xml:lang="en">Maximum length: 25.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(25)]
        [System.Xml.Serialization.XmlElementAttribute("frankingArea", Order=1)]
        public string FrankingArea { get; set; }
        
        [System.Xml.Serialization.XmlElementAttribute("votingPerson", Order=2)]
        public VotingPersonType VotingPerson { get; set; }
        
        [System.Xml.Serialization.XmlElementAttribute("personIdentification", Order=3)]
        public PersonIdentificationType PersonIdentification { get; set; }
        
        [System.Xml.Serialization.XmlElementAttribute("votingPlaceInformation", Order=4)]
        public VotingPlaceInformationType VotingPlaceInformation { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<Ech0010_6_0.OrganisationMailAddressType> _votingCardReturnAddress;
        
        [System.Xml.Serialization.XmlElementAttribute("votingCardReturnAddress", Order=5)]
        public System.Collections.Generic.List<Ech0010_6_0.OrganisationMailAddressType> VotingCardReturnAddress
        {
            get
            {
                return _votingCardReturnAddress;
            }
            set
            {
                _votingCardReturnAddress = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the VotingCardReturnAddress collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool VotingCardReturnAddressSpecified
        {
            get
            {
                return (this.VotingCardReturnAddress != null);
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="VotingCardDataType" /> class.</para>
        /// </summary>
        public VotingCardDataType()
        {
            this._votingCardReturnAddress = new System.Collections.Generic.List<Ech0010_6_0.OrganisationMailAddressType>();
            this._individualLogisticCode = new System.Collections.Generic.List<NamedCodeType>();
        }
        
        [System.Xml.Serialization.XmlElementAttribute("votingCardIndividualCodes", Order=6)]
        public VotingCardIndividualCodesType VotingCardIndividualCodes { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<NamedCodeType> _individualLogisticCode;
        
        [System.Xml.Serialization.XmlElementAttribute("individualLogisticCode", Order=7)]
        public System.Collections.Generic.List<NamedCodeType> IndividualLogisticCode
        {
            get
            {
                return _individualLogisticCode;
            }
            set
            {
                _individualLogisticCode = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the IndividualLogisticCode collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IndividualLogisticCodeSpecified
        {
            get
            {
                return (this.IndividualLogisticCode != null);
            }
        }
    }
}
