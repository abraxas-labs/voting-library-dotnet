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
    [System.Xml.Serialization.XmlTypeAttribute("countOfVotersInformationType", Namespace="http://www.ech.ch/xmlns/eCH-0110/4")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CountOfVotersInformationType
    {
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("countOfVotersTotal", Order=0)]
        public string CountOfVotersTotal { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<CountOfVotersInformationTypeSubtotalInfo> _subtotalInfo;
        
        [System.Xml.Serialization.XmlElementAttribute("subtotalInfo", Order=1)]
        public System.Collections.Generic.List<CountOfVotersInformationTypeSubtotalInfo> SubtotalInfo
        {
            get
            {
                return _subtotalInfo;
            }
            set
            {
                _subtotalInfo = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the SubtotalInfo collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SubtotalInfoSpecified
        {
            get
            {
                return ((this.SubtotalInfo != null) 
                            && (this.SubtotalInfo.Count != 0));
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="CountOfVotersInformationType" /> class.</para>
        /// </summary>
        public CountOfVotersInformationType()
        {
            this._subtotalInfo = new System.Collections.Generic.List<CountOfVotersInformationTypeSubtotalInfo>();
        }
    }
}