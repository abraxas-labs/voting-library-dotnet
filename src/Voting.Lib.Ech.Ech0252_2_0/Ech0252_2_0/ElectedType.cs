//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// This code was generated by XmlSchemaClassGenerator
namespace Ech0252_2_0
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("electedType", Namespace="http://www.ech.ch/xmlns/eCH-0252/2")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ElectedType
    {
        
        [System.Xml.Serialization.XmlElementAttribute("majorityElection", Order=0)]
        public ElectedTypeMajorityElection MajorityElection { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<ElectedTypeProportionalElectionList> _proportionalElection;
        
        [System.Xml.Serialization.XmlArrayAttribute("proportionalElection", Order=1)]
        [System.Xml.Serialization.XmlArrayItemAttribute("list", Namespace="http://www.ech.ch/xmlns/eCH-0252/2")]
        public System.Collections.Generic.List<ElectedTypeProportionalElectionList> ProportionalElection
        {
            get
            {
                return _proportionalElection;
            }
            set
            {
                _proportionalElection = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the ProportionalElection collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ProportionalElectionSpecified
        {
            get
            {
                return (this.ProportionalElection != null);
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="ElectedType" /> class.</para>
        /// </summary>
        public ElectedType()
        {
            this._proportionalElection = new System.Collections.Generic.List<ElectedTypeProportionalElectionList>();
        }
    }
}
