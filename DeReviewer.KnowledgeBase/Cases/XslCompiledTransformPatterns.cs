using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace DeReviewer.KnowledgeBase.Cases
{
    public class XslCompiledTransformPatterns
    {
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