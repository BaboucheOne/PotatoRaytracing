using System;
using System.IO;
using System.Xml;

namespace PotatoRaytracing
{
    public static class OptionFactory
    {
        private const string optionPath = @"Resources\\Options.xml";
        private const string optionXMLNode = "/option";
        private const string basicOptionFileTemplate = @"
        <option>
            <width>512</width>
            <height>512</height>
            <fov>60</fov>
            <bias>60</bias>
            <supersampling>false</supersampling>
            <supersamplingDivision>4</supersamplingDivision>
            <screenTiles>4</screenTiles>
            <recursionDepth>1</recursionDepth>
            <videoDuration>5</videoDuration>
            <videoFPS>10</videoFPS>
            <cubemap>Resources\\Textures\\cubemap5.bmp</cubemap>
        </option>";

        private static int width = 512;
        private static int height = 512;
        private static double fov = 60.0;
        private static double bias = 0.001;
        private static bool supersampling = false;
        private static int supersamplingDivision = 4;
        private static int screenTiles = 4;
        private static int recursionDepth = 1;
        private static int videoDuration = 5;
        private static int videoFPS = 10;
        private static string cubemap = @"Resources\\Textures\\cubemap5.bmp";

        public static Option CreateOption()
        {
            ReadOptionFromFile();

            return new Option(width, height, fov, bias, supersampling, supersamplingDivision, screenTiles, recursionDepth, videoDuration, videoFPS, cubemap);
        }

        private static void ReadOptionFromFile()
        {
            if (!File.Exists(optionPath)) CreateOptionFileTemplate();

            ReadXMLOptiondocument();
        }

        private static void CreateOptionFileTemplate()
        {
            File.WriteAllText(optionPath, basicOptionFileTemplate);
        }

        private static XmlNode GetOptionNodeFromOptionXML()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(optionPath);
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

            if(bias > 1)
            {
                throw new ArgumentException("Bias must be <= 1");
            }
        }

        private static void AssignOptionValueFromXMLFile(XmlNode node)
        {
            width = int.Parse(node.SelectSingleNode("width").InnerText);
            height = int.Parse(node.SelectSingleNode("height").InnerText);
            fov = double.Parse(node.SelectSingleNode("fov").InnerText);
            bias = double.Parse(node.SelectSingleNode("bias").InnerText);
            supersampling = bool.Parse(node.SelectSingleNode("supersampling").InnerText);
            supersamplingDivision = int.Parse(node.SelectSingleNode("supersamplingDivision").InnerText);
            screenTiles = int.Parse(node.SelectSingleNode("screenTiles").InnerText);
            recursionDepth = int.Parse(node.SelectSingleNode("recursionDepth").InnerText);
            videoDuration = int.Parse(node.SelectSingleNode("videoDuration").InnerText);
            videoFPS = int.Parse(node.SelectSingleNode("videoFPS").InnerText);
            cubemap = node.SelectSingleNode("cubemap").InnerText;
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
