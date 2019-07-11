using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

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
            Pattern.Create(() => xsl.Load(It.IsPayload<XmlReader>("MsxslScript.xsl")));
            
            var document = new XPathDocument(new StringReader("<?xml version='1.0'?><data></data>"));
            xsl.Transform(document, null, TextWriter.Null, null);
        }
        
        public void XsltLoad()
        {
            var xsl = new XslTransform();
            
            Pattern.Create(() => xsl.Load((string)null));            
            Pattern.Create(() => xsl.Load((string)null, null));
            
            //Pattern.Create(() => xsl.Load((XmlReader) null));
            Pattern.Create(() => xsl.Load((XmlReader) null, null));
            Pattern.Create(() => xsl.Load((XmlReader) null, null, null));
            
            Pattern.Create(() => xsl.Load((IXPathNavigable) null));
            Pattern.Create(() => xsl.Load((IXPathNavigable) null, null));
            Pattern.Create(() => xsl.Load((IXPathNavigable) null, null, null));
            
            Pattern.Create(() => xsl.Load((XPathNavigator) null));
            Pattern.Create(() => xsl.Load((XPathNavigator) null, null));
            Pattern.Create(() => xsl.Load((XPathNavigator) null, null, null));
        }
        
        public void XsltTransform()
        {
            var xsl = new XslTransform();

            Pattern.Create(() => xsl.Transform((string) null, (string) null));
            Pattern.Create(() => xsl.Transform((string) null, (string) null, null));
            
            Pattern.Create(() => xsl.Transform((IXPathNavigable) null, (XsltArgumentList) null));
            Pattern.Create(() => xsl.Transform((IXPathNavigable) null, (XsltArgumentList) null, (XmlResolver) null));
            Pattern.Create(() => xsl.Transform((IXPathNavigable) null, (XsltArgumentList) null, (Stream) null));
            Pattern.Create(() => xsl.Transform((IXPathNavigable) null, (XsltArgumentList) null, (Stream) null, null));
            Pattern.Create(() => xsl.Transform((IXPathNavigable) null, (XsltArgumentList) null, (TextWriter) null));
            Pattern.Create(() => xsl.Transform((IXPathNavigable) null, (XsltArgumentList) null, (TextWriter) null, null));
            Pattern.Create(() => xsl.Transform((IXPathNavigable) null, (XsltArgumentList) null, (XmlWriter) null));
            Pattern.Create(() => xsl.Transform((IXPathNavigable) null, (XsltArgumentList) null, (XmlWriter) null, null));

            Pattern.Create(() => xsl.Transform((XPathNavigator) null, (XsltArgumentList) null));
            Pattern.Create(() => xsl.Transform((XPathNavigator) null, (XsltArgumentList) null, (XmlResolver) null));
            Pattern.Create(() => xsl.Transform((XPathNavigator) null, (XsltArgumentList) null, (Stream) null));
            Pattern.Create(() => xsl.Transform((XPathNavigator) null, (XsltArgumentList) null, (Stream) null, null));
            Pattern.Create(() => xsl.Transform((XPathNavigator) null, (XsltArgumentList) null, (TextWriter) null));
            Pattern.Create(() => xsl.Transform((XPathNavigator) null, (XsltArgumentList) null, (TextWriter) null, null));
            Pattern.Create(() => xsl.Transform((XPathNavigator) null, (XsltArgumentList) null, (XmlWriter) null));
            Pattern.Create(() => xsl.Transform((XPathNavigator) null, (XsltArgumentList) null, (XmlWriter) null, null));
        }
    }
}