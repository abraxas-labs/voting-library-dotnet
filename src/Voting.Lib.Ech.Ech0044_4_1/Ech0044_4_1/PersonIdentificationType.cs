//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// This code was generated by XmlSchemaClassGenerator
namespace Ech0044_4_1
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("personIdentificationType", Namespace="http://www.ech.ch/xmlns/eCH-0044/4")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class PersonIdentificationType
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
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("localPersonId", Order=1)]
        public NamedPersonIdType LocalPersonId { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<NamedPersonIdType> _otherPersonId;
        
        [System.Xml.Serialization.XmlElementAttribute("otherPersonId", Order=2)]
        public System.Collections.Generic.List<NamedPersonIdType> OtherPersonId
        {
            get
            {
                return _otherPersonId;
            }
            set
            {
                _otherPersonId = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the OtherPersonId collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool OtherPersonIdSpecified
        {
            get
            {
                return (this.OtherPersonId != null);
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="PersonIdentificationType" /> class.</para>
        /// </summary>
        public PersonIdentificationType()
        {
            this._otherPersonId = new System.Collections.Generic.List<NamedPersonIdType>();
            this._euPersonId = new System.Collections.Generic.List<NamedPersonIdType>();
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<NamedPersonIdType> _euPersonId;
        
        [System.Xml.Serialization.XmlElementAttribute("euPersonId", Order=3)]
        public System.Collections.Generic.List<NamedPersonIdType> EuPersonId
        {
            get
            {
                return _euPersonId;
            }
            set
            {
                _euPersonId = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the EuPersonId collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EuPersonIdSpecified
        {
            get
            {
                return (this.EuPersonId != null);
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// <para xml:lang="en">Maximum length: 100.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(100)]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("officialName", Order=4)]
        public string OfficialName { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// <para xml:lang="en">Maximum length: 100.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(100)]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("firstName", Order=5)]
        public string FirstName { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// <para xml:lang="en">Maximum length: 100.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(100)]
        [System.Xml.Serialization.XmlElementAttribute("originalName", Order=6)]
        public string OriginalName { get; set; }
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("sex", Order=7)]
        public SexType Sex { get; set; }
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("dateOfBirth", Order=8)]
        public DatePartiallyKnownType DateOfBirth { get; set; }
    }
}
