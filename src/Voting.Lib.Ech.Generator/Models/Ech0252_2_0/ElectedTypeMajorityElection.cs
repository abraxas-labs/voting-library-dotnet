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
    [System.Xml.Serialization.XmlTypeAttribute("ElectedTypeMajorityElection", Namespace="http://www.ech.ch/xmlns/eCH-0252/2", AnonymousType=true)]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ElectedTypeMajorityElection
    {
        
        /// <summary>
        /// <para xml:lang="en">Minimum inclusive value: 0.</para>
        /// <para xml:lang="en">Maximum inclusive value: 9999999.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.RangeAttribute(typeof(decimal), "0", "9999999", ConvertValueInInvariantCulture=true)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("absoluteMajority", Order=0)]
        public uint AbsoluteMajorityValue { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the AbsoluteMajority property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool AbsoluteMajorityValueSpecified { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Minimum inclusive value: 0.</para>
        /// <para xml:lang="en">Maximum inclusive value: 9999999.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<uint> AbsoluteMajority
        {
            get
            {
                if (this.AbsoluteMajorityValueSpecified)
                {
                    return this.AbsoluteMajorityValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.AbsoluteMajorityValue = value.GetValueOrDefault();
                this.AbsoluteMajorityValueSpecified = value.HasValue;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<ElectedTypeMajorityElectionElectedCandidate> _electedCandidate;
        
        [System.Xml.Serialization.XmlElementAttribute("electedCandidate", Order=1)]
        public System.Collections.Generic.List<ElectedTypeMajorityElectionElectedCandidate> ElectedCandidate
        {
            get
            {
                return _electedCandidate;
            }
            set
            {
                _electedCandidate = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the ElectedCandidate collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ElectedCandidateSpecified
        {
            get
            {
                return ((this.ElectedCandidate != null) 
                            && (this.ElectedCandidate.Count != 0));
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="ElectedTypeMajorityElection" /> class.</para>
        /// </summary>
        public ElectedTypeMajorityElection()
        {
            this._electedCandidate = new System.Collections.Generic.List<ElectedTypeMajorityElectionElectedCandidate>();
        }
    }
}