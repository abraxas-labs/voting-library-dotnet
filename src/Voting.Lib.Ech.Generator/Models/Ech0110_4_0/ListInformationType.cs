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
    [System.Xml.Serialization.XmlTypeAttribute("listInformationType", Namespace="http://www.ech.ch/xmlns/eCH-0110/4")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ListInformationType
    {
        
        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// <para xml:lang="en">Maximum length: 50.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(50)]
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("listIdentification", Order=0)]
        public string ListIdentification { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// <para xml:lang="en">Maximum length: 6.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(6)]
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("listIndentureNumber", Order=1)]
        public string ListIndentureNumber { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<Ech0155_4_0.ListDescriptionInformationTypeListDescriptionInfo> _listDescription;
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlArrayAttribute("listDescription", Order=2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("listDescriptionInfo", Namespace="http://www.ech.ch/xmlns/eCH-0155/4")]
        public System.Collections.Generic.List<Ech0155_4_0.ListDescriptionInformationTypeListDescriptionInfo> ListDescription
        {
            get
            {
                return _listDescription;
            }
            set
            {
                _listDescription = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="ListInformationType" /> class.</para>
        /// </summary>
        public ListInformationType()
        {
            this._listDescription = new System.Collections.Generic.List<Ech0155_4_0.ListDescriptionInformationTypeListDescriptionInfo>();
        }
    }
}