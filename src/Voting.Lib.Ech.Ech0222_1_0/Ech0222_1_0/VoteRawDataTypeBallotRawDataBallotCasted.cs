//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// This code was generated by XmlSchemaClassGenerator
namespace Ech0222_1_0
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("VoteRawDataTypeBallotRawDataBallotCasted", Namespace="http://www.ech.ch/xmlns/eCH-0222/1", AnonymousType=true)]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class VoteRawDataTypeBallotRawDataBallotCasted
    {
        
        [System.Xml.Serialization.XmlElementAttribute("ballotCastedNumber", Order=0)]
        public string BallotCastedNumber { get; set; }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.Generic.List<VoteRawDataTypeBallotRawDataBallotCastedQuestionRawData> _questionRawData;
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute(AllowEmptyStrings=true)]
        [System.Xml.Serialization.XmlElementAttribute("questionRawData", Order=1)]
        public System.Collections.Generic.List<VoteRawDataTypeBallotRawDataBallotCastedQuestionRawData> QuestionRawData
        {
            get
            {
                return _questionRawData;
            }
            set
            {
                _questionRawData = value;
            }
        }
        
        /// <summary>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="VoteRawDataTypeBallotRawDataBallotCasted" /> class.</para>
        /// </summary>
        public VoteRawDataTypeBallotRawDataBallotCasted()
        {
            this._questionRawData = new System.Collections.Generic.List<VoteRawDataTypeBallotRawDataBallotCastedQuestionRawData>();
        }
    }
}
