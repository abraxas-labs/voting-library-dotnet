//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// This code was generated by XmlSchemaClassGenerator version 2.1.963.0
namespace Ech0252_1_0
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.1.963.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("CountOfVotersInformationTypeSubtotalInfo", Namespace="http://www.ech.ch/xmlns/eCH-0252/1", AnonymousType=true)]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CountOfVotersInformationTypeSubtotalInfo
    {
        
        /// <summary>
        /// <para xml:lang="en">Minimum inclusive value: 0.</para>
        /// <para xml:lang="en">Maximum inclusive value: 9999999.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.RangeAttribute(typeof(decimal), "0", "9999999", ConvertValueInInvariantCulture=true)]
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("countOfVoters", Order=0)]
        public uint CountOfVoters { get; set; }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("voterType", Order=1)]
        public Ech0155_5_0.VoterTypeType VoterTypeValue { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the VoterType property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool VoterTypeValueSpecified { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<Ech0155_5_0.VoterTypeType> VoterType
        {
            get
            {
                if (this.VoterTypeValueSpecified)
                {
                    return this.VoterTypeValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.VoterTypeValue = value.GetValueOrDefault();
                this.VoterTypeValueSpecified = value.HasValue;
            }
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("sex", Order=2)]
        public Ech0044_4_1.SexType SexValue { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Sex property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool SexValueSpecified { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<Ech0044_4_1.SexType> Sex
        {
            get
            {
                if (this.SexValueSpecified)
                {
                    return this.SexValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.SexValue = value.GetValueOrDefault();
                this.SexValueSpecified = value.HasValue;
            }
        }
    }
}