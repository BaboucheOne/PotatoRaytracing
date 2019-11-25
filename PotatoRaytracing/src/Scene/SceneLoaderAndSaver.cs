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
            SceneFile sceneFile = null;

            sceneFile = ReadSceneXmlFile(scenePath);

            return sceneFile;
        }

        private static SceneFile ReadSceneXmlFile(string scenePath)
        {
            SceneFile sceneFile;
            using (FileStream fs = File.OpenRead(scenePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SceneFile));
                sceneFile = serializer.Deserialize(fs) as SceneFile;

                fs.Flush();
                fs.Dispose();
            }

            return sceneFile;
        }

        public static void SaveScene(string sceneName, PotatoSphere[] spheres, PotatoMesh[] meshes, PotatoPointLight[] pointLights)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SceneFile));
            SceneFile sceneFile = new SceneFile(spheres, meshes, pointLights);
            string xml = string.Empty;

            using (var sw = new StringWriter())
            {
                //TODO: Mettre l'encoding en ASCII pour lire le fichier. Trouver un moyen d'enlever le utf-16 du fichier mit automatiquement.
                using (XmlWriter writer = XmlWriter.Create(sw, settings: new XmlWriterSettings { Encoding = System.Text.Encoding.ASCII, Indent = true }))
                {
                    xmlSerializer.Serialize(writer, sceneFile);
                    xml = sw.ToString();
                    xml = xml.Replace("utf-16", "ascii"); //Encoding do not work, replace it.

                    writer.Flush();
                    writer.Dispose();
                }
            }

            File.WriteAllText(sceneName, xml);
        }
    }
}
