using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

// ReSharper disable RedundantCast

namespace DeReviewer.KnowledgeBase.Cases
{
    public class XslCompiledTransformPatterns : Case
    {
        public void XsltLoadWithPayload()
        {
            var xsl = new XslCompiledTransform();
            
            // load calls with enableScript == true
            Pattern.Create(() => 
                xsl.Load(It.IsPayload<XmlReader>("MsxslScript.xsl"), XsltSettings.TrustedXslt, null));
            
            var document = new XPathDocument(new StringReader("<?xml version='1.0'?><data></data>"));
            xsl.Transform(document, XmlWriter.Create(TextWriter.Null));
        }
        
        public void XsltLoad()
        {
            var xsl = new XslCompiledTransform();
            
            // load calls with enableScript == true
            xsl.Load((string)null, XsltSettings.TrustedXslt, null);
            xsl.Load((IXPathNavigable)null, XsltSettings.TrustedXslt, null);
            xsl.Load((XmlReader)null, XsltSettings.TrustedXslt, null);
        }
        
        public void XsltUsingSafe()
        {
            var xsl = new XslCompiledTransform();

            Pattern.Create(() => xsl.Transform((string) null, (string) null));
            Pattern.Create(() => xsl.Transform((string) null, (XmlWriter) null));
            Pattern.Create(() => xsl.Transform((string) null, (XsltArgumentList) null, (Stream) null));
            Pattern.Create(() => xsl.Transform((string) null, (XsltArgumentList) null, (TextWriter) null));
            Pattern.Create(() => xsl.Transform((string) null, (XsltArgumentList) null, (XmlWriter) null));
            
            Pattern.Create(() => xsl.Transform((XmlReader) null, (XmlWriter) null));
            Pattern.Create(() => xsl.Transform((XmlReader) null, (XsltArgumentList) null, (Stream) null));
            Pattern.Create(() => xsl.Transform((XmlReader) null, (XsltArgumentList) null, (TextWriter) null));
            Pattern.Create(() => xsl.Transform((XmlReader) null, (XsltArgumentList) null, (XmlWriter) null));
            Pattern.Create(() => xsl.Transform((XmlReader) null, (XsltArgumentList) null, (XmlWriter) null, null));
            
            Pattern.Create(() => xsl.Transform((IXPathNavigable) null, (XmlWriter) null));
            Pattern.Create(() => xsl.Transform((IXPathNavigable) null, (XsltArgumentList) null, (Stream) null));
            Pattern.Create(() => xsl.Transform((IXPathNavigable) null, (XsltArgumentList) null, (TextWriter) null));
            Pattern.Create(() => xsl.Transform((IXPathNavigable) null, (XsltArgumentList) null, (XmlWriter) null));
            Pattern.Create(() => xsl.Transform((IXPathNavigable) null, (XsltArgumentList) null, (XmlWriter) null, null));
        }
    }
}