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
    [System.Xml.Serialization.XmlTypeAttribute("BallotTypeVariantBallot", Namespace="http://www.ech.ch/xmlns/eCH-0155/4", AnonymousType=true)]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class BallotTypeVariantBallot
    {
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<QuestionInformationType> _questionInformation;
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("questionInformation", Order=0)]
        public System.Collections.Generic.List<QuestionInformationType> QuestionInformation
        {
            get
            {
                return _questionInformation;
            }
            set
            {
                _questionInformation = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="BallotTypeVariantBallot" /> class.</para>
        /// </summary>
        public BallotTypeVariantBallot()
        {
            this._questionInformation = new System.Collections.Generic.List<QuestionInformationType>();
            this._tieBreakInformation = new System.Collections.Generic.List<TieBreakInformationType>();
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<TieBreakInformationType> _tieBreakInformation;
        
        [System.Xml.Serialization.XmlElementAttribute("tieBreakInformation", Order=1)]
        public System.Collections.Generic.List<TieBreakInformationType> TieBreakInformation
        {
            get
            {
                return _tieBreakInformation;
            }
            set
            {
                _tieBreakInformation = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Gets a value indicating whether the TieBreakInformation collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TieBreakInformationSpecified
        {
            get
            {
                return (this.TieBreakInformation != null);
            }
        }
    }
}
