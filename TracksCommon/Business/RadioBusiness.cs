using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TracksCommon.Gateways;

namespace TracksCommon.Business
{
    public class RadioBusiness : IRadioBusiness
    {
        private readonly IRadioGateway radioGateway;

        public RadioBusiness(IRadioGateway radioGateway)
        {
            this.radioGateway = radioGateway;
        }

        public void Update(int id, string trackId, string message, IEnumerable<Entities.Genre> genres)
        {
            radioGateway.Update(id, trackId, message);

            foreach (var genre in genres)
            {
                if(genre.Id != "-1")
                    radioGateway.AddGenre(genre);

                radioGateway.AddGenreToTrack(id, int.Parse(genre.Id));
            }
        }
    }
}
