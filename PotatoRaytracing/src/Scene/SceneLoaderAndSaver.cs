using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PotatoRaytracing
{
    public static class SceneLoaderAndSaver //TODO: Maybe merge this into PotatoScene.cs
    {
        public static SceneFile Load(string scenePath)
        {
            if (!File.Exists(scenePath)) throw new FileNotFoundException(string.Format("Scene file {0} not found", scenePath), "scenePath");

            SceneFile file;

            using (TextReader reader = new StreamReader(scenePath))
            {
                file = JsonConvert.DeserializeObject<SceneFile>(reader.ReadToEnd());
            }

            return file;
        }

        public static void Save(string sceneName, PotatoSphere[] spheres, PotatoPlane[] planes, PotatoMesh[] meshes, List<PotatoLight> ligths)
        {
            FillLightsListWithCorrespondingLightClass(ligths, out List<PotatoDirectionalLight> directionalLights, out List<PotatoPointLight> pointsLights);

            SceneFile file = new SceneFile()
            {
                Spheres = spheres,
                Meshes = meshes,
                Planes = planes,
                PointLights = pointsLights.ToArray(),
                DirectionalLights = directionalLights.ToArray()
            };

            WriteSceneToFile(sceneName, file);
        }

        private static void WriteSceneToFile(string sceneName, SceneFile file)
        {
            JsonSerializer serializer = new JsonSerializer
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            using (StreamWriter sw = new StreamWriter(sceneName))
            using (JsonWriter writer = new JsonTextWriter(sw) { Formatting = Newtonsoft.Json.Formatting.Indented })
            {
                serializer.Serialize(writer, file);
            }
        }

        private static void FillLightsListWithCorrespondingLightClass(List<PotatoLight> ligths, out List<PotatoDirectionalLight> directionalLights, out List<PotatoPointLight> pointsLights)
        {
            directionalLights = new List<PotatoDirectionalLight>();
            pointsLights = new List<PotatoPointLight>();
            for (int i = 0; i < ligths.Count; i++)
            {
                if (ligths[i].Type == PotatoLight.LightType.Directional)
                {
                    directionalLights.Add((PotatoDirectionalLight)ligths[i]);
                }
                else
                {
                    pointsLights.Add((PotatoPointLight)ligths[i]);
                }
            }
        }
    }
}
