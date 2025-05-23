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
    [System.Xml.Serialization.XmlTypeAttribute("ElectionInformationTypeElection", Namespace="http://www.ech.ch/xmlns/eCH-0228/1", AnonymousType=true)]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ElectionInformationTypeElection
    {
        
        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// <para xml:lang="en">Maximum length: 50.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(50)]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("electionIdentification", Order=0)]
        public string ElectionIdentification { get; set; }
        
        [System.Xml.Serialization.XmlElementAttribute("electionPosition", Order=1)]
        public string ElectionPosition { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<Ech0155_4_0.ElectionDescriptionInformationTypeElectionDescriptionInfo> _electionDescription;
        
        [System.Xml.Serialization.XmlArrayAttribute("electionDescription", Order=2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("electionDescriptionInfo", Namespace="http://www.ech.ch/xmlns/eCH-0155/4")]
        public System.Collections.Generic.List<Ech0155_4_0.ElectionDescriptionInformationTypeElectionDescriptionInfo> ElectionDescription
        {
            get
            {
                return _electionDescription;
            }
            set
            {
                _electionDescription = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the ElectionDescription collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ElectionDescriptionSpecified
        {
            get
            {
                return (this.ElectionDescription != null);
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="ElectionInformationTypeElection" /> class.</para>
        /// </summary>
        public ElectionInformationTypeElection()
        {
            this._electionDescription = new System.Collections.Generic.List<Ech0155_4_0.ElectionDescriptionInformationTypeElectionDescriptionInfo>();
        }
        
        [System.Xml.Serialization.XmlElementAttribute("numberOfMandates", Order=3)]
        public string NumberOfMandates { get; set; }
    }
}
