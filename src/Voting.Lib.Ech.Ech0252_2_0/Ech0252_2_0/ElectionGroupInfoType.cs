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
    [System.Xml.Serialization.XmlTypeAttribute("electionGroupInfoType", Namespace="http://www.ech.ch/xmlns/eCH-0252/2")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ElectionGroupInfoType
    {
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("electionGroup", Order=0)]
        public ElectionGroupInfoTypeElectionGroup ElectionGroup { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<CountingCircleType> _countingCircle;
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("countingCircle", Order=1)]
        public System.Collections.Generic.List<CountingCircleType> CountingCircle
        {
            get
            {
                return _countingCircle;
            }
            set
            {
                _countingCircle = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="ElectionGroupInfoType" /> class.</para>
        /// </summary>
        public ElectionGroupInfoType()
        {
            this._countingCircle = new System.Collections.Generic.List<CountingCircleType>();
            this._namedElement = new System.Collections.Generic.List<NamedElementType>();
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<NamedElementType> _namedElement;
        
        [System.Xml.Serialization.XmlElementAttribute("namedElement", Order=2)]
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
                return (this.NamedElement != null);
            }
        }
    }
}
