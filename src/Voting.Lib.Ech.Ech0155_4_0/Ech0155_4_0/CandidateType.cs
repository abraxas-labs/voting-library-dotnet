//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// This code was generated by XmlSchemaClassGenerator
namespace Ech0155_4_0
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("candidateType", Namespace="http://www.ech.ch/xmlns/eCH-0155/4")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("candidate", Namespace="http://www.ech.ch/xmlns/eCH-0155/4")]
    public partial class CandidateType
    {
        
        /// <summary>
        /// <para xml:lang="en">Minimum inclusive value: 7560000000001.</para>
        /// <para xml:lang="en">Maximum inclusive value: 7569999999999.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.RangeAttribute(typeof(ulong), "7560000000001", "7569999999999", ConvertValueInInvariantCulture=true)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("vn", Order=0)]
        public ulong VnValue { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Vn property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool VnValueSpecified { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Minimum inclusive value: 7560000000001.</para>
        /// <para xml:lang="en">Maximum inclusive value: 7569999999999.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<ulong> Vn
        {
            get
            {
                if (this.VnValueSpecified)
                {
                    return this.VnValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.VnValue = value.GetValueOrDefault();
                this.VnValueSpecified = value.HasValue;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// <para xml:lang="en">Maximum length: 50.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(50)]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("candidateIdentification", Order=1)]
        public string CandidateIdentification { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Maximum inclusive value: 26.</para>
        /// <para xml:lang="en">Minimum inclusive value: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.RangeAttribute(typeof(byte), "1", "26", ConvertValueInInvariantCulture=true)]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("BfSNumberCanton", Order=2)]
        public byte BfSNumberCantonValue { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the BfSNumberCanton property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool BfSNumberCantonValueSpecified { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Maximum inclusive value: 26.</para>
        /// <para xml:lang="en">Minimum inclusive value: 1.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<byte> BfSNumberCanton
        {
            get
            {
                if (this.BfSNumberCantonValueSpecified)
                {
                    return this.BfSNumberCantonValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.BfSNumberCantonValue = value.GetValueOrDefault();
                this.BfSNumberCantonValueSpecified = value.HasValue;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// <para xml:lang="en">Maximum length: 100.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(100)]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("familyName", Order=3)]
        public string FamilyName { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// <para xml:lang="en">Maximum length: 100.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(100)]
        [System.Xml.Serialization.XmlElementAttribute("firstName", Order=4)]
        public string FirstName { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// <para xml:lang="en">Maximum length: 100.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(100)]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("callName", Order=5)]
        public string CallName { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<CandidateTextInformationTypeCandidateTextInfo> _candidateText;
        
        [System.Xml.Serialization.XmlArrayAttribute("candidateText", Order=6)]
        [System.Xml.Serialization.XmlArrayItemAttribute("candidateTextInfo", Namespace="http://www.ech.ch/xmlns/eCH-0155/4")]
        public System.Collections.Generic.List<CandidateTextInformationTypeCandidateTextInfo> CandidateText
        {
            get
            {
                return _candidateText;
            }
            set
            {
                _candidateText = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the CandidateText collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CandidateTextSpecified
        {
            get
            {
                return (this.CandidateText != null);
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="CandidateType" /> class.</para>
        /// </summary>
        public CandidateType()
        {
            this._candidateText = new System.Collections.Generic.List<CandidateTextInformationTypeCandidateTextInfo>();
            this._occupationalTitle = new System.Collections.Generic.List<OccupationalTitleInformationTypeOccupationalTitleInfo>();
            this._swiss = new System.Collections.Generic.List<string>();
            this._role = new System.Collections.Generic.List<RoleInformationTypeRoleInfo>();
            this._partyAffiliation = new System.Collections.Generic.List<PartyAffiliationformationTypePartyAffiliationInfo>();
        }
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("dateOfBirth", Order=7, DataType="date")]
        public System.DateTime DateOfBirth { get; set; }
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("sex", Order=8)]
        public Ech0044_4_1.SexType Sex { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<OccupationalTitleInformationTypeOccupationalTitleInfo> _occupationalTitle;
        
        [System.Xml.Serialization.XmlArrayAttribute("occupationalTitle", Order=9)]
        [System.Xml.Serialization.XmlArrayItemAttribute("occupationalTitleInfo", Namespace="http://www.ech.ch/xmlns/eCH-0155/4")]
        public System.Collections.Generic.List<OccupationalTitleInformationTypeOccupationalTitleInfo> OccupationalTitle
        {
            get
            {
                return _occupationalTitle;
            }
            set
            {
                _occupationalTitle = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the OccupationalTitle collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool OccupationalTitleSpecified
        {
            get
            {
                return (this.OccupationalTitle != null);
            }
        }
        
        [System.Xml.Serialization.XmlElementAttribute("contactAddress", Order=10)]
        public Ech0010_6_0.PersonMailAddressType ContactAddress { get; set; }
        
        [System.Xml.Serialization.XmlElementAttribute("politicalAddress", Order=11)]
        public PoliticalAddressInfoType PoliticalAddress { get; set; }
        
        [System.Xml.Serialization.XmlElementAttribute("dwellingAddress", Order=12)]
        public Ech0010_6_0.AddressInformationType DwellingAddress { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<string> _swiss;
        
        [System.Xml.Serialization.XmlArrayAttribute("swiss", Order=13)]
        [System.Xml.Serialization.XmlArrayItemAttribute("origin", Namespace="http://www.ech.ch/xmlns/eCH-0155/4")]
        public System.Collections.Generic.List<string> Swiss
        {
            get
            {
                return _swiss;
            }
            set
            {
                _swiss = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the Swiss collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SwissSpecified
        {
            get
            {
                return (this.Swiss != null);
            }
        }
        
        [System.Xml.Serialization.XmlElementAttribute("foreigner", Order=14)]
        public CandidateTypeForeigner Foreigner { get; set; }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("mrMrs", Order=15)]
        public Ech0010_6_0.MrMrsType MrMrsValue { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the MrMrs property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool MrMrsValueSpecified { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<Ech0010_6_0.MrMrsType> MrMrs
        {
            get
            {
                if (this.MrMrsValueSpecified)
                {
                    return this.MrMrsValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.MrMrsValue = value.GetValueOrDefault();
                this.MrMrsValueSpecified = value.HasValue;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Maximum length: 50.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(50)]
        [System.Xml.Serialization.XmlElementAttribute("title", Order=16)]
        public string Title { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Minimum length: 2.</para>
        /// <para xml:lang="en">Maximum length: 2.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(2)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(2)]
        [System.Xml.Serialization.XmlElementAttribute("languageOfCorrespondence", Order=17)]
        public string LanguageOfCorrespondence { get; set; }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("incumbentYesNo", Order=18)]
        public bool IncumbentYesNoValue { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the IncumbentYesNo property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool IncumbentYesNoValueSpecified { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<bool> IncumbentYesNo
        {
            get
            {
                if (this.IncumbentYesNoValueSpecified)
                {
                    return this.IncumbentYesNoValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.IncumbentYesNoValue = value.GetValueOrDefault();
                this.IncumbentYesNoValueSpecified = value.HasValue;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// <para xml:lang="en">Maximum length: 10.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(10)]
        [System.Xml.Serialization.XmlElementAttribute("candidateReference", Order=19)]
        public string CandidateReference { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<RoleInformationTypeRoleInfo> _role;
        
        [System.Xml.Serialization.XmlArrayAttribute("role", Order=20)]
        [System.Xml.Serialization.XmlArrayItemAttribute("roleInfo", Namespace="http://www.ech.ch/xmlns/eCH-0155/4")]
        public System.Collections.Generic.List<RoleInformationTypeRoleInfo> Role
        {
            get
            {
                return _role;
            }
            set
            {
                _role = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the Role collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool RoleSpecified
        {
            get
            {
                return (this.Role != null);
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<PartyAffiliationformationTypePartyAffiliationInfo> _partyAffiliation;
        
        [System.Xml.Serialization.XmlArrayAttribute("partyAffiliation", Order=21)]
        [System.Xml.Serialization.XmlArrayItemAttribute("partyAffiliationInfo", Namespace="http://www.ech.ch/xmlns/eCH-0155/4")]
        public System.Collections.Generic.List<PartyAffiliationformationTypePartyAffiliationInfo> PartyAffiliation
        {
            get
            {
                return _partyAffiliation;
            }
            set
            {
                _partyAffiliation = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the PartyAffiliation collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PartyAffiliationSpecified
        {
            get
            {
                return (this.PartyAffiliation != null);
            }
        }
    }
}
