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
    [System.Xml.Serialization.XmlTypeAttribute("eventResultDelivery", Namespace="http://www.ech.ch/xmlns/eCH-0110/4")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class EventResultDelivery
    {
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("reportingBody", Order=0)]
        public ReportingBodyType ReportingBody { get; set; }
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("contestInformation", Order=1)]
        public Ech0155_4_0.ContestType ContestInformation { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<CountingCircleResultsType> _countingCircleResults;
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("countingCircleResults", Order=2)]
        public System.Collections.Generic.List<CountingCircleResultsType> CountingCircleResults
        {
            get
            {
                return _countingCircleResults;
            }
            set
            {
                _countingCircleResults = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="EventResultDelivery" /> class.</para>
        /// </summary>
        public EventResultDelivery()
        {
            this._countingCircleResults = new System.Collections.Generic.List<CountingCircleResultsType>();
        }
        
        [System.Xml.Serialization.XmlElementAttribute("rawData", Order=3)]
        public Ech0222_1_0.RawDataType RawData { get; set; }
        
        [System.Xml.Serialization.XmlElementAttribute("extension", Order=4)]
        public Ech0155_4_0.ExtensionType Extension { get; set; }
    }
}