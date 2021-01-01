using System;
using System.Drawing;
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
            <fov>40</fov>
            <bias>0,001</bias>
            <gamma>1</gamma>
            <supersampling>false</supersampling>
            <supersamplingDivision>4</supersamplingDivision>
            <screenTiles>4</screenTiles>
            <recursionDepth>1</recursionDepth>
            <videoDuration>5</videoDuration>
            <videoFPS>10</videoFPS>
            <useSolidColor>false</useSolidColor>
            <cubemap>Resources\\Textures\\cubemap5.bmp</cubemap>
            <solidColor>0 0 0</solidColor>
        </option>";

        private static int width = 512;
        private static int height = 512;
        private static float fov = 60.0f;
        private static double bias = 0.001;
        private static float gamma = 1f;
        private static bool supersampling = false;
        private static int supersamplingDivision = 4;
        private static int screenTiles = 4;
        private static int recursionDepth = 1;
        private static int videoDuration = 5;
        private static int videoFPS = 10;
        private static bool useSolidColor = false;
        private static string cubemap = @"Resources\\Textures\\cubemap5.bmp";
        private static Color solidColor = Color.Black;

        public static Option CreateOption()
        {
            ReadOptionFromFile();

            return new Option(width, height, fov, bias, gamma, supersampling, supersamplingDivision, screenTiles, recursionDepth, videoDuration, videoFPS, useSolidColor, cubemap, solidColor);
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

            if (width <= 0) throw new ArgumentException("Wight must be positive", "width");
            if (height <= 0) throw new ArgumentException("Height must be positive", "height");
            if (fov < 0) throw new ArgumentException("Fov must be positive", "fov");
            if (bias > 1) throw new ArgumentException("Bias must be <= 1", "bias");
            if (gamma < 0) throw new ArgumentException("Gamma must be >= 0", "gamma");
            if (supersamplingDivision < 1) throw new ArgumentException("supersamplingDivision must be >= 1", "supersamplingDivision");
            if (screenTiles < 1) throw new ArgumentException("ScreenTiles must be upper or equal to 1", "screenTiles");
            if (recursionDepth < 0) throw new ArgumentException("recursionDepth mus be positive", "recursionDepth");
        }

        private static void AssignOptionValueFromXMLFile(XmlNode node)
        {
            width = int.Parse(node.SelectSingleNode("width").InnerText);
            height = int.Parse(node.SelectSingleNode("height").InnerText);
            fov = float.Parse(node.SelectSingleNode("fov").InnerText);
            bias = double.Parse(node.SelectSingleNode("bias").InnerText);
            gamma = float.Parse(node.SelectSingleNode("gamma").InnerText);
            supersampling = bool.Parse(node.SelectSingleNode("supersampling").InnerText);
            supersamplingDivision = int.Parse(node.SelectSingleNode("supersamplingDivision").InnerText);
            screenTiles = int.Parse(node.SelectSingleNode("screenTiles").InnerText);
            recursionDepth = int.Parse(node.SelectSingleNode("recursionDepth").InnerText);
            videoDuration = int.Parse(node.SelectSingleNode("videoDuration").InnerText);
            videoFPS = int.Parse(node.SelectSingleNode("videoFPS").InnerText);
            useSolidColor = bool.Parse(node.SelectSingleNode("useSolidColor").InnerText);
            cubemap = node.SelectSingleNode("cubemap").InnerText;
            solidColor = Extensions.ParseColor(node.SelectSingleNode("solidColor").InnerText);
        }
    }
}
