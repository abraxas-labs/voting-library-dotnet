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
    [System.Xml.Serialization.XmlTypeAttribute("electionResultType", Namespace="http://www.ech.ch/xmlns/eCH-0252/2")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ElectionResultType
    {
        
        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// <para xml:lang="en">Maximum length: 50.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(50)]
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("electionIdentification", Order=0)]
        public string ElectionIdentification { get; set; }
        
        [System.Xml.Serialization.XmlElementAttribute("majoralElection", Order=1)]
        public ElectionResultTypeMajoralElection MajoralElection { get; set; }
        
        [System.Xml.Serialization.XmlElementAttribute("proportionalElection", Order=2)]
        public ElectionResultTypeProportionalElection ProportionalElection { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<NamedElementType> _namedElement;
        
        [System.Xml.Serialization.XmlElementAttribute("namedElement", Order=3)]
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
        
        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="ElectionResultType" /> class.</para>
        /// </summary>
        public ElectionResultType()
        {
            this._namedElement = new System.Collections.Generic.List<NamedElementType>();
        }
    }
}