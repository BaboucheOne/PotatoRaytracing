using System.IO;
using System.Xml;

namespace PotatoRaytracing
{
    public class OptionFactory
    {
        private const string optionFileName = "Options.xml";
        private const string optionXMLNode = "/option";
        private const string basicOptionFileTemplate = @"
        <option>
            <width>500</width>
            <height>500</height>
            <fov>60</fov>
            <supersampling>true</supersampling>
            <supersamplingDivision>4</supersamplingDivision>
        </option>";

        private static int width = 0;
        private static int height = 0;
        private static double fov = 0.0;
        private static bool supersampling = true;
        private static int supersamplingDivision = 0;

        public static Option CreateOption()
        {
            ReadOptionFromFile();

            return new Option(width, height, fov, supersampling, supersamplingDivision);
        }

        private static void ReadOptionFromFile()
        {
            if (!File.Exists(optionFileName)) CreateOptionFileTemplate();

            ReadXMLOptiondocument();
        }

        private static void CreateOptionFileTemplate()
        {
            File.WriteAllText("Options.xml", basicOptionFileTemplate);
        }

        private static XmlNode GetOptionNodeFromOptionXML()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(optionFileName);
            XmlNode node = doc.DocumentElement.SelectNodes(optionXMLNode)[0];
            return node;
        }

        private static void ReadXMLOptiondocument()
        {
            XmlNode node = GetOptionNodeFromOptionXML();

            AssignOptionValueFromXMLFile(node);
        }

        private static void AssignOptionValueFromXMLFile(XmlNode node)
        {
            width = int.Parse(node.SelectSingleNode("width").InnerText);
            height = int.Parse(node.SelectSingleNode("height").InnerText);
            fov = double.Parse(node.SelectSingleNode("fov").InnerText);
            supersampling = bool.Parse(node.SelectSingleNode("supersampling").InnerText);
            supersamplingDivision = int.Parse(node.SelectSingleNode("supersamplingDivision").InnerText);
        }
    }
}
