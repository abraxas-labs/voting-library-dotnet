//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// This code was generated by XmlSchemaClassGenerator version 2.1.963.0
namespace Ech0252_2_0
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.1.963.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("delivery", Namespace="http://www.ech.ch/xmlns/eCH-0252/2", AnonymousType=true)]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("delivery", Namespace="http://www.ech.ch/xmlns/eCH-0252/2")]
    public partial class Delivery
    {
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlElementAttribute("deliveryHeader", Order=0)]
        public Ech0058_5_0.HeaderType DeliveryHeader { get; set; }
        
        [System.Xml.Serialization.XmlElementAttribute("voteBaseDelivery", Order=1)]
        public EventVoteBaseDeliveryType VoteBaseDelivery { get; set; }
        
        [System.Xml.Serialization.XmlElementAttribute("electionInformationDelivery", Order=2)]
        public EventElectionInformationDeliveryType ElectionInformationDelivery { get; set; }
        
        [System.Xml.Serialization.XmlElementAttribute("electionResultDelivery", Order=3)]
        public EventElectionResultDeliveryType ElectionResultDelivery { get; set; }
        
        [System.Xml.Serialization.XmlAttributeAttribute("minorVersion", Namespace="http://www.ech.ch/xmlns/eCH-0252/2", Form=System.Xml.Schema.XmlSchemaForm.Qualified)]
        public string MinorVersion { get; set; }
    }
}