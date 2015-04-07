using System;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace ConsoleApplication1
{
    /// <summary>
    /// https://www.cineworld.co.uk/developer/api/films
    /// </summary>
    public class Films
    {
        [JsonProperty("films")]
        public Film[] FilmArray { get; set; }

        internal static Films SendRequest(
            string key,
            bool full = false,
            string territory = null,
            int? cinema = null,
            int? film = null,
            string category = null,
            string @event = null,
            int? distributor = null,
            params DateTime[] dates)
        {
            var sb = new StringBuilder("http://www.cineworld.co.uk/api/quickbook/films?key=Bbc6hz2P&full=");

            sb.Append(full.ToString().ToLower());

            if (!string.IsNullOrWhiteSpace(territory)) sb.Append("&territory=" + territory);
            if (cinema.HasValue) sb.Append("&cinema=" + cinema);
            if (film.HasValue) sb.Append("&film=" + film);
            if (!string.IsNullOrWhiteSpace(category)) sb.Append("&category=" + category);
            if (!string.IsNullOrWhiteSpace(@event)) sb.Append("&event=" + @event);
            if (distributor.HasValue) sb.Append("&distributor=" + distributor);

            dates
                .Select(d => string.Format("&date={0:yyyyMMdd}", d))
                .ToList()
                .ForEach(s => sb.Append(s));

            var uri = new Uri(sb.ToString());

            using (var webClient = new WebClient())
            {
                var jsonString = webClient.DownloadString(uri);

                return JsonConvert.DeserializeObject<Films>(jsonString);
            }
        }
    }
}
