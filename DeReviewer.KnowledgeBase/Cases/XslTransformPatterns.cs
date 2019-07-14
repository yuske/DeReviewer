using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable RedundantCast

// Suppress the warning of using obsolete type XslTransform
#pragma warning disable 618

namespace DeReviewer.KnowledgeBase.Cases
{
    public class XslTransformPatterns : Case
    {
        public void XsltLoadWithPayload()
        {
            var xsl = new XslTransform();
            Pattern.CreateByName(it => xsl.Load(it.IsPayloadFrom("MsxslScript.xsl").Cast<XmlReader>()));
            
            var document = new XPathDocument(new StringReader("<?xml version='1.0'?><data></data>"));
            Pattern.CreateByName(it => xsl.Transform(document, null, TextWriter.Null, null));
        }

        private void XsltLoad()
        {
            var xsl = new XslTransform();
            
            Pattern.CreateBySignature(it => xsl.Load((string)null));            
            Pattern.CreateBySignature(it => xsl.Load((string)null, null));
            
            //Pattern.Create(it => xsl.Load((XmlReader) null));
            Pattern.CreateBySignature(it => xsl.Load((XmlReader) null, null));
            Pattern.CreateBySignature(it => xsl.Load((XmlReader) null, null, null));
            
            Pattern.CreateBySignature(it => xsl.Load((IXPathNavigable) null));
            Pattern.CreateBySignature(it => xsl.Load((IXPathNavigable) null, null));
            Pattern.CreateBySignature(it => xsl.Load((IXPathNavigable) null, null, null));
            
            Pattern.CreateBySignature(it => xsl.Load((XPathNavigator) null));
            Pattern.CreateBySignature(it => xsl.Load((XPathNavigator) null, null));
            Pattern.CreateBySignature(it => xsl.Load((XPathNavigator) null, null, null));
        }

        private void XsltTransform()
        {
            var xsl = new XslTransform();

            Pattern.CreateBySignature(it => xsl.Transform((string) null, (string) null));
            Pattern.CreateBySignature(it => xsl.Transform((string) null, (string) null, null));
            
            Pattern.CreateBySignature(it => xsl.Transform((IXPathNavigable) null, (XsltArgumentList) null));
            Pattern.CreateBySignature(it => xsl.Transform((IXPathNavigable) null, (XsltArgumentList) null, (XmlResolver) null));
            Pattern.CreateBySignature(it => xsl.Transform((IXPathNavigable) null, (XsltArgumentList) null, (Stream) null));
            Pattern.CreateBySignature(it => xsl.Transform((IXPathNavigable) null, (XsltArgumentList) null, (Stream) null, null));
            Pattern.CreateBySignature(it => xsl.Transform((IXPathNavigable) null, (XsltArgumentList) null, (TextWriter) null));
            Pattern.CreateBySignature(it => xsl.Transform((IXPathNavigable) null, (XsltArgumentList) null, (TextWriter) null, null));
            Pattern.CreateBySignature(it => xsl.Transform((IXPathNavigable) null, (XsltArgumentList) null, (XmlWriter) null));
            Pattern.CreateBySignature(it => xsl.Transform((IXPathNavigable) null, (XsltArgumentList) null, (XmlWriter) null, null));

            Pattern.CreateBySignature(it => xsl.Transform((XPathNavigator) null, (XsltArgumentList) null));
            Pattern.CreateBySignature(it => xsl.Transform((XPathNavigator) null, (XsltArgumentList) null, (XmlResolver) null));
            Pattern.CreateBySignature(it => xsl.Transform((XPathNavigator) null, (XsltArgumentList) null, (Stream) null));
            Pattern.CreateBySignature(it => xsl.Transform((XPathNavigator) null, (XsltArgumentList) null, (Stream) null, null));
            Pattern.CreateBySignature(it => xsl.Transform((XPathNavigator) null, (XsltArgumentList) null, (TextWriter) null));
            Pattern.CreateBySignature(it => xsl.Transform((XPathNavigator) null, (XsltArgumentList) null, (TextWriter) null, null));
            Pattern.CreateBySignature(it => xsl.Transform((XPathNavigator) null, (XsltArgumentList) null, (XmlWriter) null));
            Pattern.CreateBySignature(it => xsl.Transform((XPathNavigator) null, (XsltArgumentList) null, (XmlWriter) null, null));
        }
    }
}