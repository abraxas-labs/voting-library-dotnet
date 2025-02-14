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
    [System.Xml.Serialization.XmlTypeAttribute("guardianRelationshipType", Namespace="http://www.ech.ch/xmlns/eCH-0021/7")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GuardianRelationshipType
    {
        
        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// <para xml:lang="en">Maximum length: 36.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(36)]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("guardianRelationshipId", Order=0)]
        public string GuardianRelationshipId { get; set; }
        
        [System.Xml.Serialization.XmlElementAttribute("partner", Order=1)]
        public GuardianRelationshipTypePartner Partner { get; set; }
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("typeOfRelationship", Order=2)]
        public GuardianRelationshipTypeTypeOfRelationship TypeOfRelationship { get; set; }
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("guardianMeasureInfo", Order=3)]
        public GuardianMeasureInfoType GuardianMeasureInfo { get; set; }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("care", Order=4)]
        public CareType CareValue { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Care property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool CareValueSpecified { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<CareType> Care
        {
            get
            {
                if (this.CareValueSpecified)
                {
                    return this.CareValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.CareValue = value.GetValueOrDefault();
                this.CareValueSpecified = value.HasValue;
            }
        }
    }
}
