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
    [System.Xml.Serialization.XmlTypeAttribute("CandidateResultTypeCandidateListResultsInfo", Namespace="http://www.ech.ch/xmlns/eCH-0252/2", AnonymousType=true)]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CandidateResultTypeCandidateListResultsInfo
    {
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<CandidateListResultType> _candidateListResults;
        
        [System.Xml.Serialization.XmlElementAttribute("candidateListResults", Order=0)]
        public System.Collections.Generic.List<CandidateListResultType> CandidateListResults
        {
            get
            {
                return _candidateListResults;
            }
            set
            {
                _candidateListResults = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the CandidateListResults collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CandidateListResultsSpecified
        {
            get
            {
                return (this.CandidateListResults != null);
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="CandidateResultTypeCandidateListResultsInfo" /> class.</para>
        /// </summary>
        public CandidateResultTypeCandidateListResultsInfo()
        {
            this._candidateListResults = new System.Collections.Generic.List<CandidateListResultType>();
        }
        
        /// <summary>
        /// <para xml:lang="en">Minimum inclusive value: 0.</para>
        /// <para xml:lang="en">Maximum inclusive value: 9999999.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.RangeAttribute(typeof(uint), "0", "9999999", ConvertValueInInvariantCulture=true)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("countOfVotesFromBallotsWithoutListDesignation", Order=1)]
        public uint CountOfVotesFromBallotsWithoutListDesignationValue { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the CountOfVotesFromBallotsWithoutListDesignation property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool CountOfVotesFromBallotsWithoutListDesignationValueSpecified { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Minimum inclusive value: 0.</para>
        /// <para xml:lang="en">Maximum inclusive value: 9999999.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<uint> CountOfVotesFromBallotsWithoutListDesignation
        {
            get
            {
                if (this.CountOfVotesFromBallotsWithoutListDesignationValueSpecified)
                {
                    return this.CountOfVotesFromBallotsWithoutListDesignationValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.CountOfVotesFromBallotsWithoutListDesignationValue = value.GetValueOrDefault();
                this.CountOfVotesFromBallotsWithoutListDesignationValueSpecified = value.HasValue;
            }
        }
    }
}
