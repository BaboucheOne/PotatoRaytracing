using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace PotatoRaytracing
{
    public static class SceneLoaderAndSaver
    {
        public static SceneFile LoadScene(string scenePath)
        {
            if (!File.Exists(scenePath)) throw new Exception("Cannot find the scene file");

            using (FileStream fs = File.OpenRead(scenePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SceneFile));
                return serializer.Deserialize(fs) as SceneFile;
            }
        }

        public static void SaveScene(string sceneName, PotatoMesh[] meshes, PotatoPointLight[] pointLights)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SceneFile));
            SceneFile sceneFile = new SceneFile(meshes, pointLights);
            string xml = "";

            using (var sw = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sw))
                {
                    xmlSerializer.Serialize(writer, sceneFile);
                    xml = sw.ToString();
                }
            }

            File.WriteAllText("scene.xml", xml);
        }
    }
}
