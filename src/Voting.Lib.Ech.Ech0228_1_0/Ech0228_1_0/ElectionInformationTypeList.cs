//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// This code was generated by XmlSchemaClassGenerator
namespace Ech0228_1_0
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("ElectionInformationTypeList", Namespace="http://www.ech.ch/xmlns/eCH-0228/1", AnonymousType=true)]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ElectionInformationTypeList
    {
        
        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// <para xml:lang="en">Maximum length: 50.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(50)]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("listIdentification", Order=0)]
        public string ListIdentification { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// <para xml:lang="en">Maximum length: 6.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(6)]
        [System.Xml.Serialization.XmlElementAttribute("listIndentureNumber", Order=1)]
        public string ListIndentureNumber { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<Ech0155_4_0.ListDescriptionInformationTypeListDescriptionInfo> _listDescription;
        
        [System.Xml.Serialization.XmlArrayAttribute("listDescription", Order=2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("listDescriptionInfo", Namespace="http://www.ech.ch/xmlns/eCH-0155/4")]
        public System.Collections.Generic.List<Ech0155_4_0.ListDescriptionInformationTypeListDescriptionInfo> ListDescription
        {
            get
            {
                return _listDescription;
            }
            set
            {
                _listDescription = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the ListDescription collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ListDescriptionSpecified
        {
            get
            {
                return (this.ListDescription != null);
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="ElectionInformationTypeList" /> class.</para>
        /// </summary>
        public ElectionInformationTypeList()
        {
            this._listDescription = new System.Collections.Generic.List<Ech0155_4_0.ListDescriptionInformationTypeListDescriptionInfo>();
            this._listUnionBallotText = new System.Collections.Generic.List<Ech0155_4_0.ListUnionBallotTextTypeListUnionBallotTextInfo>();
            this._individualListVerificationCodes = new System.Collections.Generic.List<NamedCodeType>();
            this._candidatePosition = new System.Collections.Generic.List<ElectionInformationTypeListCandidatePosition>();
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("isEmptyList", Order=3)]
        public bool IsEmptyListValue { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the IsEmptyList property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool IsEmptyListValueSpecified { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<bool> IsEmptyList
        {
            get
            {
                if (this.IsEmptyListValueSpecified)
                {
                    return this.IsEmptyListValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.IsEmptyListValue = value.GetValueOrDefault();
                this.IsEmptyListValueSpecified = value.HasValue;
            }
        }
        
        [System.Xml.Serialization.XmlElementAttribute("listOrderOfPrecedence", Order=4)]
        public string ListOrderOfPrecedence { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<Ech0155_4_0.ListUnionBallotTextTypeListUnionBallotTextInfo> _listUnionBallotText;
        
        [System.Xml.Serialization.XmlArrayAttribute("listUnionBallotText", Order=5)]
        [System.Xml.Serialization.XmlArrayItemAttribute("listUnionBallotTextInfo", Namespace="http://www.ech.ch/xmlns/eCH-0155/4")]
        public System.Collections.Generic.List<Ech0155_4_0.ListUnionBallotTextTypeListUnionBallotTextInfo> ListUnionBallotText
        {
            get
            {
                return _listUnionBallotText;
            }
            set
            {
                _listUnionBallotText = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the ListUnionBallotText collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ListUnionBallotTextSpecified
        {
            get
            {
                return (this.ListUnionBallotText != null);
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<NamedCodeType> _individualListVerificationCodes;
        
        [System.Xml.Serialization.XmlElementAttribute("individualListVerificationCodes", Order=6)]
        public System.Collections.Generic.List<NamedCodeType> IndividualListVerificationCodes
        {
            get
            {
                return _individualListVerificationCodes;
            }
            set
            {
                _individualListVerificationCodes = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the IndividualListVerificationCodes collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IndividualListVerificationCodesSpecified
        {
            get
            {
                return (this.IndividualListVerificationCodes != null);
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<ElectionInformationTypeListCandidatePosition> _candidatePosition;
        
        [System.Xml.Serialization.XmlElementAttribute("candidatePosition", Order=7)]
        public System.Collections.Generic.List<ElectionInformationTypeListCandidatePosition> CandidatePosition
        {
            get
            {
                return _candidatePosition;
            }
            set
            {
                _candidatePosition = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the CandidatePosition collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CandidatePositionSpecified
        {
            get
            {
                return (this.CandidatePosition != null);
            }
        }
    }
}
