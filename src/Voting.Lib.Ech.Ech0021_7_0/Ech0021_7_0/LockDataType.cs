//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// This code was generated by XmlSchemaClassGenerator version 2.1.963.0
namespace Ech0021_7_0
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.1.963.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("lockDataType", Namespace="http://www.ech.ch/xmlns/eCH-0021/7")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class LockDataType
    {
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("dataLock", Order=0)]
        public DataLockType DataLock { get; set; }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("dataLockValidFrom", Order=1, DataType="date")]
        public System.DateTime DataLockValidFromValue { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the DataLockValidFrom property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool DataLockValidFromValueSpecified { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<System.DateTime> DataLockValidFrom
        {
            get
            {
                if (this.DataLockValidFromValueSpecified)
                {
                    return this.DataLockValidFromValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.DataLockValidFromValue = value.GetValueOrDefault();
                this.DataLockValidFromValueSpecified = value.HasValue;
            }
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("dataLockValidTill", Order=2, DataType="date")]
        public System.DateTime DataLockValidTillValue { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the DataLockValidTill property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool DataLockValidTillValueSpecified { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<System.DateTime> DataLockValidTill
        {
            get
            {
                if (this.DataLockValidTillValueSpecified)
                {
                    return this.DataLockValidTillValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.DataLockValidTillValue = value.GetValueOrDefault();
                this.DataLockValidTillValueSpecified = value.HasValue;
            }
        }
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("paperLock", Order=3)]
        public PaperLockType PaperLock { get; set; }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("paperLockValidFrom", Order=4, DataType="date")]
        public System.DateTime PaperLockValidFromValue { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the PaperLockValidFrom property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool PaperLockValidFromValueSpecified { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<System.DateTime> PaperLockValidFrom
        {
            get
            {
                if (this.PaperLockValidFromValueSpecified)
                {
                    return this.PaperLockValidFromValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.PaperLockValidFromValue = value.GetValueOrDefault();
                this.PaperLockValidFromValueSpecified = value.HasValue;
            }
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("paperLockValidTill", Order=5, DataType="date")]
        public System.DateTime PaperLockValidTillValue { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the PaperLockValidTill property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool PaperLockValidTillValueSpecified { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<System.DateTime> PaperLockValidTill
        {
            get
            {
                if (this.PaperLockValidTillValueSpecified)
                {
                    return this.PaperLockValidTillValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.PaperLockValidTillValue = value.GetValueOrDefault();
                this.PaperLockValidTillValueSpecified = value.HasValue;
            }
        }
    }
}