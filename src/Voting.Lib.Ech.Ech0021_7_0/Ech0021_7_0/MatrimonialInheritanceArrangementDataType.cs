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
    [System.Xml.Serialization.XmlTypeAttribute("matrimonialInheritanceArrangementDataType", Namespace="http://www.ech.ch/xmlns/eCH-0021/7")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class MatrimonialInheritanceArrangementDataType
    {
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("matrimonialInheritanceArrangement", Order=0)]
        public Ech0011_8_1.YesNoType MatrimonialInheritanceArrangement { get; set; }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("matrimonialInheritanceArrangementValidFrom", Order=1, DataType="date")]
        public System.DateTime MatrimonialInheritanceArrangementValidFromValue { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the MatrimonialInheritanceArrangementValidFrom property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool MatrimonialInheritanceArrangementValidFromValueSpecified { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<System.DateTime> MatrimonialInheritanceArrangementValidFrom
        {
            get
            {
                if (this.MatrimonialInheritanceArrangementValidFromValueSpecified)
                {
                    return this.MatrimonialInheritanceArrangementValidFromValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.MatrimonialInheritanceArrangementValidFromValue = value.GetValueOrDefault();
                this.MatrimonialInheritanceArrangementValidFromValueSpecified = value.HasValue;
            }
        }
    }
}
