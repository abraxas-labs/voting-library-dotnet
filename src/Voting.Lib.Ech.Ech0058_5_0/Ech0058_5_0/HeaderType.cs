//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// This code was generated by XmlSchemaClassGenerator
namespace Ech0058_5_0
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("headerType", Namespace="http://www.ech.ch/xmlns/eCH-0058/5")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("header", Namespace="http://www.ech.ch/xmlns/eCH-0058/5")]
    public partial class HeaderType
    {
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("senderId", Order=0)]
        public string SenderId { get; set; }
        
        [System.Xml.Serialization.XmlElementAttribute("originalSenderId", Order=1)]
        public string OriginalSenderId { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// <para xml:lang="en">Maximum length: 100.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(100)]
        [System.Xml.Serialization.XmlElementAttribute("declarationLocalReference", Order=2)]
        public string DeclarationLocalReference { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<string> _recipientId;
        
        [System.Xml.Serialization.XmlElementAttribute("recipientId", Order=3)]
        public System.Collections.Generic.List<string> RecipientId
        {
            get
            {
                return _recipientId;
            }
            set
            {
                _recipientId = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the RecipientId collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool RecipientIdSpecified
        {
            get
            {
                return (this.RecipientId != null);
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="HeaderType" /> class.</para>
        /// </summary>
        public HeaderType()
        {
            this._recipientId = new System.Collections.Generic.List<string>();
            this._attachment = new System.Collections.Generic.List<object>();
            this._namedMetaData = new System.Collections.Generic.List<NamedMetaDataType>();
        }
        
        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// <para xml:lang="en">Maximum length: 36.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(36)]
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("messageId", Order=4)]
        public string MessageId { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// <para xml:lang="en">Maximum length: 36.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(36)]
        [System.Xml.Serialization.XmlElementAttribute("referenceMessageId", Order=5)]
        public string ReferenceMessageId { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// <para xml:lang="en">Maximum length: 128.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(128)]
        [System.Xml.Serialization.XmlElementAttribute("businessProcessId", Order=6)]
        public string BusinessProcessId { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// <para xml:lang="en">Maximum length: 50.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(50)]
        [System.Xml.Serialization.XmlElementAttribute("ourBusinessReferenceId", Order=7)]
        public string OurBusinessReferenceId { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// <para xml:lang="en">Maximum length: 50.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(50)]
        [System.Xml.Serialization.XmlElementAttribute("yourBusinessReferenceId", Order=8)]
        public string YourBusinessReferenceId { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// <para xml:lang="en">Maximum length: 50.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(50)]
        [System.Xml.Serialization.XmlElementAttribute("uniqueIdBusinessTransaction", Order=9)]
        public string UniqueIdBusinessTransaction { get; set; }
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("messageType", Order=10)]
        public string MessageType { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// <para xml:lang="en">Maximum length: 36.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(36)]
        [System.Xml.Serialization.XmlElementAttribute("subMessageType", Order=11)]
        public string SubMessageType { get; set; }
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("sendingApplication", Order=12)]
        public SendingApplicationType SendingApplication { get; set; }
        
        [System.Xml.Serialization.XmlElementAttribute("partialDelivery", Order=13)]
        public PartialDeliveryType PartialDelivery { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// <para xml:lang="en">Maximum length: 100.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(100)]
        [System.Xml.Serialization.XmlElementAttribute("subject", Order=14)]
        public string Subject { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// <para xml:lang="en">Maximum length: 250.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(250)]
        [System.Xml.Serialization.XmlElementAttribute("comment", Order=15)]
        public string Comment { get; set; }
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("messageDate", Order=16, DataType="dateTime")]
        public System.DateTime MessageDate { get; set; }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("initialMessageDate", Order=17, DataType="dateTime")]
        public System.DateTime InitialMessageDateValue { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the InitialMessageDate property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool InitialMessageDateValueSpecified { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<System.DateTime> InitialMessageDate
        {
            get
            {
                if (this.InitialMessageDateValueSpecified)
                {
                    return this.InitialMessageDateValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.InitialMessageDateValue = value.GetValueOrDefault();
                this.InitialMessageDateValueSpecified = value.HasValue;
            }
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("eventDate", Order=18, DataType="date")]
        public System.DateTime EventDateValue { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the EventDate property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool EventDateValueSpecified { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<System.DateTime> EventDate
        {
            get
            {
                if (this.EventDateValueSpecified)
                {
                    return this.EventDateValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.EventDateValue = value.GetValueOrDefault();
                this.EventDateValueSpecified = value.HasValue;
            }
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("modificationDate", Order=19, DataType="date")]
        public System.DateTime ModificationDateValue { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the ModificationDate property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool ModificationDateValueSpecified { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<System.DateTime> ModificationDate
        {
            get
            {
                if (this.ModificationDateValueSpecified)
                {
                    return this.ModificationDateValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.ModificationDateValue = value.GetValueOrDefault();
                this.ModificationDateValueSpecified = value.HasValue;
            }
        }
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("action", Order=20)]
        public ActionType Action { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<object> _attachment;
        
        [System.Xml.Serialization.XmlElementAttribute("attachment", Order=21)]
        public System.Collections.Generic.List<object> Attachment
        {
            get
            {
                return _attachment;
            }
            set
            {
                _attachment = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the Attachment collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AttachmentSpecified
        {
            get
            {
                return (this.Attachment != null);
            }
        }
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("testDeliveryFlag", Order=22)]
        public bool TestDeliveryFlag { get; set; }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("responseExpected", Order=23)]
        public bool ResponseExpectedValue { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the ResponseExpected property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool ResponseExpectedValueSpecified { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<bool> ResponseExpected
        {
            get
            {
                if (this.ResponseExpectedValueSpecified)
                {
                    return this.ResponseExpectedValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.ResponseExpectedValue = value.GetValueOrDefault();
                this.ResponseExpectedValueSpecified = value.HasValue;
            }
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("businessCaseClosed", Order=24)]
        public bool BusinessCaseClosedValue { get; set; }
        
        /// <summary>
        /// <para xml:lang="en">Gets or sets a value indicating whether the BusinessCaseClosed property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool BusinessCaseClosedValueSpecified { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<bool> BusinessCaseClosed
        {
            get
            {
                if (this.BusinessCaseClosedValueSpecified)
                {
                    return this.BusinessCaseClosedValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.BusinessCaseClosedValue = value.GetValueOrDefault();
                this.BusinessCaseClosedValueSpecified = value.HasValue;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<NamedMetaDataType> _namedMetaData;
        
        [System.Xml.Serialization.XmlElementAttribute("namedMetaData", Order=25)]
        public System.Collections.Generic.List<NamedMetaDataType> NamedMetaData
        {
            get
            {
                return _namedMetaData;
            }
            set
            {
                _namedMetaData = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the NamedMetaData collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool NamedMetaDataSpecified
        {
            get
            {
                return (this.NamedMetaData != null);
            }
        }
        
        [System.Xml.Serialization.XmlElementAttribute("extension", Order=26)]
        public object Extension { get; set; }
    }
}
