using AutoMapper;
using EmmBlog;
using Newtonsoft.Json;
using Repository;
using System.IO;
using System.Text;
using UnitTestProject.Helpers;

namespace UnitTestProject
{
    internal sealed class MapperHelper : Singleton<MapperHelper>
    {

        private Mapper Mapper { get; }
        
        public MapperHelper(){
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new TestMapping());  //mapping between Web and Business layer objects
            });
            Mapper = new Mapper(config);
        }

        internal static TDest GetMapped<TDest>(object source)
        {
            return Instance.Mapper.Map<object, TDest>(source);
        }
        internal static TDest GetMappedJSON<TDest>(object source)
        {
            string s = JsonConvert.SerializeObject(source, Formatting.Indented,
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }
            );

            return Instance.Mapper.Map<object, TDest>(JsonConvert.DeserializeObject<TDest>(s));
        }
    }
}
