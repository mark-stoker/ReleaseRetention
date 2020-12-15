using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReleaseRetentionLibrary;
using ReleaseRetentionLibrary.Interfaces;

namespace ReleaseRetentionTesting.SampleData
{
	public class ConstructTestData
	{
		public static List<IProject> GetProjectsData()
		{
			var projects = new List<IProject>();

			using (StreamReader r = new StreamReader(@".\SampleData\Projects.json"))
			{
				string jsonstring = r.ReadToEnd();
				JObject obj = JObject.Parse(jsonstring);
				var jsonArray = JArray.Parse(obj["Projects"].ToString());

				foreach (var element in jsonArray)
				{
					var desirialisedElement = JsonConvert.DeserializeObject<Project>(element.ToString());
					projects.Add(desirialisedElement);
				}
			}

			return projects;
		}

		public static List<IEnvironment> GetEnvironmentsData()
		{
			var environments = new List<IEnvironment>();

			using (StreamReader r = new StreamReader(@".\SampleData\Environments.json"))
			{
				string jsonstring = r.ReadToEnd();
				JObject obj = JObject.Parse(jsonstring);
				var jsonArray = JArray.Parse(obj["Environments"].ToString());

				foreach (var element in jsonArray)
				{
					var desirialisedElement = JsonConvert.DeserializeObject<Environment>(element.ToString());
					environments.Add(desirialisedElement);
				}
			}

			return environments;
		}

		public static List<IRelease> GetReleasesData()
		{
			var releases = new List<IRelease>();

			using (StreamReader r = new StreamReader(@".\SampleData\Releases.json"))
			{
				string jsonstring = r.ReadToEnd();
				JObject obj = JObject.Parse(jsonstring);
				var jsonArray = JArray.Parse(obj["Releases"].ToString());

				foreach (var element in jsonArray)
				{
					var desirialisedElement = JsonConvert.DeserializeObject<Release>(element.ToString());
					releases.Add(desirialisedElement);
				}
			}

			return releases;
		}

		public static List<IDeployment> GetDeploymentsData()
		{
			var deployments = new List<IDeployment>();

			using (StreamReader r = new StreamReader(@".\SampleData\Deployments.json"))
			{
				string jsonstring = r.ReadToEnd();
				JObject obj = JObject.Parse(jsonstring);
				var jsonArray = JArray.Parse(obj["Deployments"].ToString());

				foreach (var element in jsonArray)
				{
					var desirialisedElement = JsonConvert.DeserializeObject<Deployment>(element.ToString());
					deployments.Add(desirialisedElement);
				}
			}

			return deployments.ToList();
		}
	}
}