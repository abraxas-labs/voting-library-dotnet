//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// This code was generated by XmlSchemaClassGenerator version 2.1.963.0
namespace Ech0045_4_0
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.1.963.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("votingPersonType", Namespace="http://www.ech.ch/xmlns/eCH-0045/4")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class VotingPersonType
    {
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("person", Order=0)]
        public VotingPersonTypePerson Person { get; set; }
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("dataLock", Order=1)]
        public Ech0021_7_0.DataLockType DataLock { get; set; }
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("electoralAddress", Order=2)]
        public Ech0010_6_0.PersonMailAddressType ElectoralAddress { get; set; }
        
        [System.Xml.Serialization.XmlElementAttribute("deliveryAddress", Order=3)]
        public Ech0010_6_0.PersonMailAddressType DeliveryAddress { get; set; }
        
        [System.Xml.Serialization.XmlElementAttribute("email", Order=4)]
        public EmailType Email { get; set; }
        
        [System.Xml.Serialization.XmlElementAttribute("phone", Order=5)]
        public PhoneType Phone { get; set; }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("isEvoter", Order=6)]
        public bool IsEvoterValue { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the IsEvoter property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool IsEvoterValueSpecified { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<bool> IsEvoter
        {
            get
            {
                if (this.IsEvoterValueSpecified)
                {
                    return this.IsEvoterValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.IsEvoterValue = value.GetValueOrDefault();
                this.IsEvoterValueSpecified = value.HasValue;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<VotingPersonTypeDomainOfInfluenceInfo> _domainOfInfluenceInfo;
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("domainOfInfluenceInfo", Order=7)]
        public System.Collections.Generic.List<VotingPersonTypeDomainOfInfluenceInfo> DomainOfInfluenceInfo
        {
            get
            {
                return _domainOfInfluenceInfo;
            }
            set
            {
                _domainOfInfluenceInfo = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="VotingPersonType" /> class.</para>
        /// </summary>
        public VotingPersonType()
        {
            this._domainOfInfluenceInfo = new System.Collections.Generic.List<VotingPersonTypeDomainOfInfluenceInfo>();
        }
    }
}