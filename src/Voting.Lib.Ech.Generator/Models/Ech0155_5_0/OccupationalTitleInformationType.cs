//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// This code was generated by XmlSchemaClassGenerator version 2.1.963.0
namespace Ech0155_5_0
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.1.963.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("occupationalTitleInformationType", Namespace="http://www.ech.ch/xmlns/eCH-0155/5")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class OccupationalTitleInformationType
    {
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<OccupationalTitleInformationTypeOccupationalTitleInfo> _occupationalTitleInfo;
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("occupationalTitleInfo", Order=0)]
        public System.Collections.Generic.List<OccupationalTitleInformationTypeOccupationalTitleInfo> OccupationalTitleInfo
        {
            get
            {
                return _occupationalTitleInfo;
            }
            set
            {
                _occupationalTitleInfo = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="OccupationalTitleInformationType" /> class.</para>
        /// </summary>
        public OccupationalTitleInformationType()
        {
            this._occupationalTitleInfo = new System.Collections.Generic.List<OccupationalTitleInformationTypeOccupationalTitleInfo>();
        }
    }
}