//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// This code was generated by XmlSchemaClassGenerator version 2.1.963.0
namespace Ech0222_1_0
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.1.963.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("electionRawDataType", Namespace="http://www.ech.ch/xmlns/eCH-0222/1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ElectionRawDataType
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
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<ElectionRawDataTypeBallotRawData> _ballotRawData;
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("ballotRawData", Order=1)]
        public System.Collections.Generic.List<ElectionRawDataTypeBallotRawData> BallotRawData
        {
            get
            {
                return _ballotRawData;
            }
            set
            {
                _ballotRawData = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="ElectionRawDataType" /> class.</para>
        /// </summary>
        public ElectionRawDataType()
        {
            this._ballotRawData = new System.Collections.Generic.List<ElectionRawDataTypeBallotRawData>();
        }
    }
}