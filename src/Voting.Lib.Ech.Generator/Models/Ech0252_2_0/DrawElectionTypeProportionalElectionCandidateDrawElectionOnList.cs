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
    [System.Xml.Serialization.XmlTypeAttribute("DrawElectionTypeProportionalElectionCandidateDrawElectionOnList", Namespace="http://www.ech.ch/xmlns/eCH-0252/2", AnonymousType=true)]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DrawElectionTypeProportionalElectionCandidateDrawElectionOnList
    {
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("isDrawPending", Order=0)]
        public bool IsDrawPendingValue { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the IsDrawPending property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool IsDrawPendingValueSpecified { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<bool> IsDrawPending
        {
            get
            {
                if (this.IsDrawPendingValueSpecified)
                {
                    return this.IsDrawPendingValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.IsDrawPendingValue = value.GetValueOrDefault();
                this.IsDrawPendingValueSpecified = value.HasValue;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// <para xml:lang="en">Maximum length: 50.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(50)]
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("listIdentification", Order=1)]
        public string ListIdentification { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<string> _candidateIdentification;
        
        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// <para xml:lang="en">Maximum length: 50.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(50)]
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("candidateIdentification", Order=2)]
        public System.Collections.Generic.List<string> CandidateIdentification
        {
            get
            {
                return _candidateIdentification;
            }
            set
            {
                _candidateIdentification = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="DrawElectionTypeProportionalElectionCandidateDrawElectionOnList" /> class.</para>
        /// </summary>
        public DrawElectionTypeProportionalElectionCandidateDrawElectionOnList()
        {
            this._candidateIdentification = new System.Collections.Generic.List<string>();
            this._winningCandidateIdentification = new System.Collections.Generic.List<string>();
            this._namedElement = new System.Collections.Generic.List<NamedElementType>();
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<string> _winningCandidateIdentification;
        
        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// <para xml:lang="en">Maximum length: 50.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(50)]
        [System.Xml.Serialization.XmlElementAttribute("winningCandidateIdentification", Order=3)]
        public System.Collections.Generic.List<string> WinningCandidateIdentification
        {
            get
            {
                return _winningCandidateIdentification;
            }
            set
            {
                _winningCandidateIdentification = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the WinningCandidateIdentification collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool WinningCandidateIdentificationSpecified
        {
            get
            {
                return ((this.WinningCandidateIdentification != null) 
                            && (this.WinningCandidateIdentification.Count != 0));
            }
        }
        
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