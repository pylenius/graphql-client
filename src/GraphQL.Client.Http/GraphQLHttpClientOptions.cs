using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Dahomey.Json;
using Dahomey.Json.Serialization.Converters.Factories;

namespace GraphQL.Client.Http {

	/// <summary>
	/// The Options that the <see cref="GraphQLHttpClient"/> will use
	/// </summary>
	public class GraphQLHttpClientOptions {

		/// <summary>
		/// The GraphQL EndPoint to be used
		/// </summary>
		public Uri EndPoint { get; set; }

		/// <summary>
		/// The <see cref="JsonSerializerOptions"/> that is going to be used
		/// </summary>
		public JsonSerializerOptions JsonSerializerOptions { get; set; } = GetDefaultJsonSerializerOptions();

		/// <summary>
		/// The <see cref="System.Net.Http.HttpMessageHandler"/> that is going to be used
		/// </summary>
		public HttpMessageHandler HttpMessageHandler { get; set; } = new HttpClientHandler();

		/// <summary>
		/// The <see cref="MediaTypeHeaderValue"/> that will be send on POST
		/// </summary>
		public MediaTypeHeaderValue MediaType { get; set; } = MediaTypeHeaderValue.Parse("application/json; charset=utf-8"); // This should be "application/graphql" also "application/x-www-form-urlencoded" is Accepted

		/// <summary>
		/// The back-off strategy for automatic websocket/subscription reconnects. Calculates the delay before the next connection attempt is made.<br/>
		/// default formula: min(n, 5) * 1,5 * random(0.0, 1.0)
		/// </summary>
		public Func<int, TimeSpan> BackOffStrategy { get; set; } = n => {
			var rnd = new Random();
			return TimeSpan.FromSeconds(Math.Min(n, 5) * 1.5 + rnd.NextDouble());
		};

		/// <summary>
		/// If <see langword="true"/>, the websocket connection is also used for regular queries and mutations
		/// </summary>
		public bool UseWebSocketForQueriesAndMutations { get; set; } = false;

		/// <summary>
		/// Request preprocessing function. Can be used i.e. to inject authorization info into a GraphQL request payload.
		/// </summary>
		public Func<GraphQLRequest, GraphQLHttpClient, Task<GraphQLRequest>> PreprocessRequest { get; set; } = (request, client) => Task.FromResult(request);

		/// <summary>
		/// Generates the default <see cref="JsonSerializerOptions"/>
		/// </summary>
		/// <returns></returns>
		public static JsonSerializerOptions GetDefaultJsonSerializerOptions() {
			var options = new JsonSerializerOptions {
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			};

			//options.Converters.Add(new JsonSerializerOptionsState(options));
			//options.Converters.Add(new DictionaryConverterFactory());
			//options.Converters.Add(new CollectionConverterFactory());
			//options.Converters.Add(new JsonNodeConverterFactory());
			//options.Converters.Add(new ObjectConverterFactory());

			options.SetupExtensions();

			return options;
		}
	}
}
