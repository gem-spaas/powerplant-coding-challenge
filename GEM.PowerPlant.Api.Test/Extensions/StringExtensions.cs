using System.Reflection;

namespace GEM.PowerPlant.Api.Test.Extensions
{
	public static class StringExtensions
	{
		public readonly static string Directory = "Data";
		public static string Load(this string resourceFile)
		{
			var result = string.Empty;

			var assembly = Assembly.GetExecutingAssembly();

			using var resourceStream = assembly.GetManifestResourceStream(string.Join(".", assembly.GetName().Name, Directory, resourceFile));

			if (resourceStream != null)
			{
				using var reader = new StreamReader(resourceStream);
				result = reader.ReadToEnd();
			}

			return result;

		}
	}
}
