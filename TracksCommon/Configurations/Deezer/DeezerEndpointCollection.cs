using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracksCommon.Configurations.Deezer
{
    public class DeezerEndpointCollection : ConfigurationElementCollection, IEnumerable<DeezerEndpointElement>
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new DeezerEndpointElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DeezerEndpointElement)element).Name;
        }

        IEnumerator<DeezerEndpointElement> IEnumerable<DeezerEndpointElement>.GetEnumerator()
        {
            foreach (var item in this.BaseGetAllKeys())
            {
                yield return (DeezerEndpointElement)BaseGet(item);
            }
        }
    }
}
