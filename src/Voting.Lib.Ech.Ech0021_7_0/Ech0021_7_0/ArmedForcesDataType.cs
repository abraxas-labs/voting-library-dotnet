//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// This code was generated by XmlSchemaClassGenerator
namespace Ech0021_7_0
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("armedForcesDataType", Namespace="http://www.ech.ch/xmlns/eCH-0021/7")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ArmedForcesDataType
    {
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("armedForcesService", Order=0)]
        public Ech0011_8_1.YesNoType ArmedForcesServiceValue { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the ArmedForcesService property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool ArmedForcesServiceValueSpecified { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<Ech0011_8_1.YesNoType> ArmedForcesService
        {
            get
            {
                if (this.ArmedForcesServiceValueSpecified)
                {
                    return this.ArmedForcesServiceValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.ArmedForcesServiceValue = value.GetValueOrDefault();
                this.ArmedForcesServiceValueSpecified = value.HasValue;
            }
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("armedForcesLiability", Order=1)]
        public Ech0011_8_1.YesNoType ArmedForcesLiabilityValue { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the ArmedForcesLiability property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool ArmedForcesLiabilityValueSpecified { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<Ech0011_8_1.YesNoType> ArmedForcesLiability
        {
            get
            {
                if (this.ArmedForcesLiabilityValueSpecified)
                {
                    return this.ArmedForcesLiabilityValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.ArmedForcesLiabilityValue = value.GetValueOrDefault();
                this.ArmedForcesLiabilityValueSpecified = value.HasValue;
            }
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("armedForcesValidFrom", Order=2, DataType="date")]
        public System.DateTime ArmedForcesValidFromValue { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the ArmedForcesValidFrom property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool ArmedForcesValidFromValueSpecified { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<System.DateTime> ArmedForcesValidFrom
        {
            get
            {
                if (this.ArmedForcesValidFromValueSpecified)
                {
                    return this.ArmedForcesValidFromValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.ArmedForcesValidFromValue = value.GetValueOrDefault();
                this.ArmedForcesValidFromValueSpecified = value.HasValue;
            }
        }
    }
}
