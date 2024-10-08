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
    [System.Xml.Serialization.XmlTypeAttribute("ElectedTypeProportionalElectionListElectedCandidate", Namespace="http://www.ech.ch/xmlns/eCH-0252/2", AnonymousType=true)]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ElectedTypeProportionalElectionListElectedCandidate
    {
        
        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// <para xml:lang="en">Maximum length: 50.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(50)]
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("candidateIdentification", Order=0)]
        public string CandidateIdentification { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<string> _candidateReferenceOnPosition;
        
        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// <para xml:lang="en">Maximum length: 10.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(10)]
        [System.Xml.Serialization.XmlElementAttribute("candidateReferenceOnPosition", Order=1)]
        public System.Collections.Generic.List<string> CandidateReferenceOnPosition
        {
            get
            {
                return _candidateReferenceOnPosition;
            }
            set
            {
                _candidateReferenceOnPosition = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the CandidateReferenceOnPosition collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CandidateReferenceOnPositionSpecified
        {
            get
            {
                return ((this.CandidateReferenceOnPosition != null) 
                            && (this.CandidateReferenceOnPosition.Count != 0));
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="ElectedTypeProportionalElectionListElectedCandidate" /> class.</para>
        /// </summary>
        public ElectedTypeProportionalElectionListElectedCandidate()
        {
            this._candidateReferenceOnPosition = new System.Collections.Generic.List<string>();
            this._namedElement = new System.Collections.Generic.List<NamedElementType>();
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("electedByDraw", Order=2)]
        public bool ElectedByDrawValue { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the ElectedByDraw property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool ElectedByDrawValueSpecified { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<bool> ElectedByDraw
        {
            get
            {
                if (this.ElectedByDrawValueSpecified)
                {
                    return this.ElectedByDrawValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.ElectedByDrawValue = value.GetValueOrDefault();
                this.ElectedByDrawValueSpecified = value.HasValue;
            }
        }
        
        [System.Xml.Serialization.XmlElementAttribute("sortID", Order=3)]
        public string SortId { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<NamedElementType> _namedElement;
        
        [System.Xml.Serialization.XmlElementAttribute("namedElement", Order=4)]
        public System.Collections.Generic.List<NamedElementType> NamedElement
        {
            get
            {
                return _namedElement;
            }
            set
            {
                _namedElement = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the NamedElement collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool NamedElementSpecified
        {
            get
            {
                return ((this.NamedElement != null) 
                            && (this.NamedElement.Count != 0));
            }
        }
    }
}
