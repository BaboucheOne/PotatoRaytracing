using System;
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
            <supersampling>false</supersampling>
            <supersamplingDivision>4</supersamplingDivision>
            <screenTiles>4</screenTiles>
            <videoDuration>5</videoDuration>
            <videoFPS>10</videoFPS>
        </option>";

        private static int width = 0;
        private static int height = 0;
        private static double fov = 0.0;
        private static bool supersampling = true;
        private static int supersamplingDivision = 0;
        private static int screenTiles = 0;
        private static int videoDuration = 0;
        private static int videoFPS = 0;

        public static Option CreateOption()
        {
            ReadOptionFromFile();

            return new Option(width, height, fov, supersampling, supersamplingDivision, screenTiles, videoDuration, videoFPS);
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

            //TODO: Generer les exception autrement.
            if(!IsPowerOf4(screenTiles))
            {
                throw new ArgumentException("ScreenTiles must be power of 4. (1, 4, 16, ...)");
            }

            if(width != height)
            {
                throw new ArgumentException("Width and Height must be equal");
            }

            if(!IsResoltionFit(width))
            {
                throw new ArgumentException("Width do not conform to supported resolution (32 to 4096)");
            }

            if ((width / screenTiles) < 1)
            {
                throw new ArgumentException("Width / screenTiles must be equal or greater than 1");
            }
        }

        private static void AssignOptionValueFromXMLFile(XmlNode node)
        {
            width = int.Parse(node.SelectSingleNode("width").InnerText);
            height = int.Parse(node.SelectSingleNode("height").InnerText);
            fov = double.Parse(node.SelectSingleNode("fov").InnerText);
            supersampling = bool.Parse(node.SelectSingleNode("supersampling").InnerText);
            supersamplingDivision = int.Parse(node.SelectSingleNode("supersamplingDivision").InnerText);
            screenTiles = int.Parse(node.SelectSingleNode("screenTiles").InnerText);
            videoDuration = int.Parse(node.SelectSingleNode("videoDuration").InnerText);
            videoFPS = int.Parse(node.SelectSingleNode("videoFPS").InnerText);
        }

        private static bool IsPowerOf4(int x)
        {
            double i = Math.Log(x) / Math.Log(4);
            return i == Math.Floor(i);
        }

        //TODO: Chercher comment on peux reduire le tout (sans generer resolution).
        private static bool IsResoltionFit(int width)
        {
            for (int i = 5; i < 12; i++)
            {
                if(width == (int)Math.Pow(2, i)) return true;
            }

            return false;
        }
    }
}
