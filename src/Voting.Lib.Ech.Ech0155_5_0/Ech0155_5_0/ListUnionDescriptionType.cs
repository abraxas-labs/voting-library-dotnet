//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// This code was generated by XmlSchemaClassGenerator
namespace Ech0155_5_0
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("listUnionDescriptionType", Namespace="http://www.ech.ch/xmlns/eCH-0155/5")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ListUnionDescriptionType
    {
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<ListUnionDescriptionTypeListUnionDescriptionInfo> _listUnionDescriptionInfo;
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("listUnionDescriptionInfo", Order=0)]
        public System.Collections.Generic.List<ListUnionDescriptionTypeListUnionDescriptionInfo> ListUnionDescriptionInfo
        {
            get
            {
                return _listUnionDescriptionInfo;
            }
            set
            {
                _listUnionDescriptionInfo = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="ListUnionDescriptionType" /> class.</para>
        /// </summary>
        public ListUnionDescriptionType()
        {
            this._listUnionDescriptionInfo = new System.Collections.Generic.List<ListUnionDescriptionTypeListUnionDescriptionInfo>();
        }
    }
}
