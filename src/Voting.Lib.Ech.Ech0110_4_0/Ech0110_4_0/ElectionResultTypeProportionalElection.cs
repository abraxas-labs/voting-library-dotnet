//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// This code was generated by XmlSchemaClassGenerator
namespace Ech0110_4_0
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("ElectionResultTypeProportionalElection", Namespace="http://www.ech.ch/xmlns/eCH-0110/4", AnonymousType=true)]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ElectionResultTypeProportionalElection
    {
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("countOfChangedBallotsWithPartyAffiliation", Order=0)]
        public ResultDetailType CountOfChangedBallotsWithPartyAffiliation { get; set; }
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("countOfChangedBallotsWithoutPartyAffiliation", Order=1)]
        public ResultDetailType CountOfChangedBallotsWithoutPartyAffiliation { get; set; }
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("countOfEmptyVotesOfChangedBallotsWithoutPartyAffiliation", Order=2)]
        public ResultDetailType CountOfEmptyVotesOfChangedBallotsWithoutPartyAffiliation { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<ListResultsType> _list;
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("list", Order=3)]
        public System.Collections.Generic.List<ListResultsType> List
        {
            get
            {
                return _list;
            }
            set
            {
                _list = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="ElectionResultTypeProportionalElection" /> class.</para>
        /// </summary>
        public ElectionResultTypeProportionalElection()
        {
            this._list = new System.Collections.Generic.List<ListResultsType>();
            this._candidate = new System.Collections.Generic.List<CandidateResultType>();
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<CandidateResultType> _candidate;
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("candidate", Order=4)]
        public System.Collections.Generic.List<CandidateResultType> Candidate
        {
            get
            {
                return _candidate;
            }
            set
            {
                _candidate = value;
            }
        }
    }
}
