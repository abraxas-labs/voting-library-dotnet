//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// This code was generated by XmlSchemaClassGenerator version 2.1.963.0
namespace Ech0098_4_0
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.1.963.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("datePartiallyKnownType", Namespace="http://www.ech.ch/xmlns/eCH-0098/4")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("datePartiallyKnownType", Namespace="http://www.ech.ch/xmlns/eCH-0098/4")]
    public partial class DatePartiallyKnownType
    {
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("yearMonthDay", Order=0, DataType="date")]
        public System.DateTime YearMonthDayValue { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the YearMonthDay property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool YearMonthDayValueSpecified { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<System.DateTime> YearMonthDay
        {
            get
            {
                if (this.YearMonthDayValueSpecified)
                {
                    return this.YearMonthDayValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.YearMonthDayValue = value.GetValueOrDefault();
                this.YearMonthDayValueSpecified = value.HasValue;
            }
        }
        
        [System.Xml.Serialization.XmlElementAttribute("yearMonth", Order=1)]
        public string YearMonth { get; set; }
        
        [System.Xml.Serialization.XmlElementAttribute("year", Order=2)]
        public string Year { get; set; }
    }
}