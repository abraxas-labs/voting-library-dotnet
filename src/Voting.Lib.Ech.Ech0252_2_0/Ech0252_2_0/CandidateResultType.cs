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
    [System.Xml.Serialization.XmlTypeAttribute("candidateResultType", Namespace="http://www.ech.ch/xmlns/eCH-0252/2")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CandidateResultType
    {
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("candidateOrWriteInCandidate", Order=0)]
        public CandidateOrWriteInCandidateType CandidateOrWriteInCandidate { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Minimum inclusive value: 0.</para>
        /// <para xml:lang="en">Maximum inclusive value: 9999999.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.RangeAttribute(typeof(uint), "0", "9999999", ConvertValueInInvariantCulture=true)]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("countOfVotesTotal", Order=1)]
        public uint CountOfVotesTotal { get; set; }
        
        [System.Xml.Serialization.XmlElementAttribute("candidateListResultsInfo", Order=2)]
        public CandidateResultTypeCandidateListResultsInfo CandidateListResultsInfo { get; set; }
    }
}
