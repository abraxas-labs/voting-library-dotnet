//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// This code was generated by XmlSchemaClassGenerator version 2.1.963.0
namespace Ech0110_4_0
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.1.963.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("ElectionResultTypeMajoralElection", Namespace="http://www.ech.ch/xmlns/eCH-0110/4", AnonymousType=true)]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ElectionResultTypeMajoralElection
    {
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<CandidateResultType> _candidate;
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("candidate", Order=0)]
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
        
        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="ElectionResultTypeMajoralElection" /> class.</para>
        /// </summary>
        public ElectionResultTypeMajoralElection()
        {
            this._candidate = new System.Collections.Generic.List<CandidateResultType>();
        }
        
        [System.Xml.Serialization.XmlElementAttribute("countOfInvalidVotesTotal", Order=1)]
        public ResultDetailType CountOfInvalidVotesTotal { get; set; }
        
        [System.Xml.Serialization.XmlElementAttribute("countOfBlankVotesTotal", Order=2)]
        public ResultDetailType CountOfBlankVotesTotal { get; set; }
        
        [System.Xml.Serialization.XmlElementAttribute("countOfIndividualVotesTotal", Order=3)]
        public ResultDetailType CountOfIndividualVotesTotal { get; set; }
    }
}