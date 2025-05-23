//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// This code was generated by XmlSchemaClassGenerator
namespace Ech0011_8_1
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("maritalDataType", Namespace="http://www.ech.ch/xmlns/eCH-0011/8")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MaritalDataRestrictedDivorceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MaritalDataRestrictedMaritalStatusPartnerType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MaritalDataRestrictedMarriageType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MaritalDataRestrictedPartnershipType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MaritalDataRestrictedUndoMarriedType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MaritalDataRestrictedUndoPartnershipType))]
    public partial class MaritalDataType
    {
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("maritalStatus", Order=0)]
        public MaritalStatusType MaritalStatus { get; set; }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("dateOfMaritalStatus", Order=1, DataType="date")]
        public System.DateTime DateOfMaritalStatusValue { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the DateOfMaritalStatus property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool DateOfMaritalStatusValueSpecified { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<System.DateTime> DateOfMaritalStatus
        {
            get
            {
                if (this.DateOfMaritalStatusValueSpecified)
                {
                    return this.DateOfMaritalStatusValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.DateOfMaritalStatusValue = value.GetValueOrDefault();
                this.DateOfMaritalStatusValueSpecified = value.HasValue;
            }
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("cancelationReason", Order=2)]
        public PartnershipAbolitionType CancelationReasonValue { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the CancelationReason property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool CancelationReasonValueSpecified { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<PartnershipAbolitionType> CancelationReason
        {
            get
            {
                if (this.CancelationReasonValueSpecified)
                {
                    return this.CancelationReasonValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.CancelationReasonValue = value.GetValueOrDefault();
                this.CancelationReasonValueSpecified = value.HasValue;
            }
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("officialProofOfMaritalStatusYesNo", Order=3)]
        public bool OfficialProofOfMaritalStatusYesNoValue { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the OfficialProofOfMaritalStatusYesNo property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool OfficialProofOfMaritalStatusYesNoValueSpecified { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<bool> OfficialProofOfMaritalStatusYesNo
        {
            get
            {
                if (this.OfficialProofOfMaritalStatusYesNoValueSpecified)
                {
                    return this.OfficialProofOfMaritalStatusYesNoValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.OfficialProofOfMaritalStatusYesNoValue = value.GetValueOrDefault();
                this.OfficialProofOfMaritalStatusYesNoValueSpecified = value.HasValue;
            }
        }
        
        [System.Xml.Serialization.XmlElementAttribute("separationData", Order=4)]
        public SeparationDataType SeparationData { get; set; }
    }
}
