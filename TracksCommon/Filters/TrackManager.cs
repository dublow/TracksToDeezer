using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TracksCommon.Entities;

namespace TracksCommon.Filters
{
    public class TrackManager
    {
        private readonly SearchFilter deezerSearchFilter;
        private readonly IEnumerable<IFilter> filters;
        private readonly string url;
        private readonly string message;

        public TrackManager(SearchFilter deezerSearchFilter, string url, IEnumerable<IFilter> filters, string message)
        {
            this.deezerSearchFilter = deezerSearchFilter;
            this.url = url;
            this.filters = filters;
            this.message = message;
        }

        public string GetUrl(string artist, string title)
        {
            switch (deezerSearchFilter)
            {
                case SearchFilter.Full:
                    return string.Format(url, artist, title);
                case SearchFilter.ByArtist:
                    return string.Format(url, artist);
                case SearchFilter.ByTitle:
                    return string.Format(url, title);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public Search Filtering(DeezerSearch deezerSearch, string artist, string title)
        {
            Search search = null;
            foreach (var filter in filters)
            {
                search = filter.Create(deezerSearch, artist, title, message);
                if (search != null)
                    break;
            }

            return search;
        }

        public static IEnumerable<TrackManager> LoadDeezerTrackManagers()
        {
            var filters = new List<IFilter>
            {
                new DeezerFullFilter(),
                new DeezerArtistFilter(),
                new DeezerTitleFilter()
            };

            var trackManagers = new List<TrackManager>
            {
                new TrackManager(SearchFilter.Full, "https://api.deezer.com/search?strict=on&q=artist:\"{0}\" track:\"{1}\"", filters, "Search: Full"),
                new TrackManager(SearchFilter.ByTitle, "https://api.deezer.com/search?strict=on&q=track:\"{0}\"", filters, "Search: ByTitle"),
                new TrackManager(SearchFilter.ByArtist, "https://api.deezer.com/search?strict=on&q=artist:\"{0}\"", filters, "Search: ByArtist")
            };

            return trackManagers;
        }
    }
}
