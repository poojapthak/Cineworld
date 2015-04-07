using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using System.Text.RegularExpressions;

namespace ConsoleApplication1
{
    public class Film
    {
        private static JsonSchema Schema = JsonSchema.Parse(@"{
                '$schema': 'http://json-schema.org/draft-04/schema#',
                'edi': {
                    'type': 'integer'
                },
                '@title': {
                    'type': 'string'
                },
                '@id': {
                    'type': 'integer'
                },
                'classification': {
                    'type': 'string'
                },
                'advisory': {
                    'type': 'string'
                },
                'poster_url': {
                    'type': 'string'
                },
                'film_url': {
                    'type': 'string'
                },
                'still_url': {
                    'type': 'string'
                },
                '3D': {
                    'type': 'boolean'
                },
                'imax': {
                    'type': 'boolean'
                }
            }");

        private static readonly Regex TitlePrefixRegex = new Regex(@"^(2D |3D |IMAX )+- (.*?)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex TitleArticleRegex = new Regex(@"^(A |An |The )+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private string _title;

        public int Edi { get; set; }
        public int Id { get; set; }
        public string Classification { get; set; }
        public string Advisory { get; set; }
        [JsonProperty("poster_url")]
        public Uri PosterUrl { get; set; }
        [JsonProperty("film_url")]
        public Uri FilmUrl { get; set; }
        [JsonProperty("still_url")]
        public Uri StillUrl { get; set; }
        [JsonProperty("3D")]
        public bool _3D { get; set; }
        public bool Imax { get; set; }

        public string Title
        {
            get { return _title; }
            set
            {
                // Strip leading; 2D, 3D and IMAX.
                var match = TitlePrefixRegex.Match(value);

                _title = match.Groups.Count <= 1 ? value : match.Groups[2].Value;

                // Move articles (A, An and The) to the end.
                match = TitleArticleRegex.Match(_title);

                if (match.Groups.Count <= 1) return;

                var articles = match.Groups[1].Captures.OfType<Capture>().Select(c => c.Value);

                foreach (var article in articles)
                {
                    _title = _title.Remove(0, article.Length);
                    _title += ", " + article.Trim();
                }
            }
        }

        public override string ToString()
        {
            return string.Format("{0:D8}: {1}{2}{3}", Edi, Title, (_3D ? " (3D)" : ""), (Imax ? " (IMAX)" : ""));
        }
    }
}
