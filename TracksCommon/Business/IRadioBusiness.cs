using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Siriona.Library.Annotations;
using TracksCommon.Entities;

namespace TracksCommon.Business
{
    public interface IRadioBusiness
    {
        void Update(int id, string trackId, string message, IEnumerable<Genre> genres);
    }
}
