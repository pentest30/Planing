using System.Globalization;
using System.Windows;
using Planing.Core.Models;
using Planing.Models;
using Planing.ModelView;

namespace Planing
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App:Application
    {
          
        public App()
        {
              AutoMapper.Mapper.CreateMap<Tc, TcViewModel>()
                .ForMember(x => x.AnneeScolaire, o => o.Ignore())
                .ForMember(x => x.AnneeId, o => o.MapFrom(x => x.Section.AnneeId))
                 .ForMember(x => x.SpecialiteId, o => o.MapFrom(x => x.Section.SpecialiteId))
                 .ForMember(x => x.AnneeScolaire, o => o.MapFrom(x => x.AnneeScolaire.Name))
                .ForMember(x => x.Specialite, o => o.MapFrom(x => x.Section.Specialite.Name))
                .ForMember(x => x.Teacher, o => o.MapFrom(x => x.Teacher.Nom.ToString(CultureInfo.InvariantCulture)))
                .ForMember(x => x.Groupe, o => o.MapFrom(x => x.Groupe.Name.ToString(CultureInfo.InvariantCulture)))
                .ForMember(x => x.Section, o => o.MapFrom(x => x.Section.Name.ToString(CultureInfo.InvariantCulture)))
                .ForMember(x => x.Teacher, o => o.MapFrom(x => x.Teacher.Nom.ToString(CultureInfo.InvariantCulture)))
                .ForMember(x => x.Course, o => o.MapFrom(x => x.Course.Name.ToString(CultureInfo.InvariantCulture)))
                .ForMember(x => x.ClassRoomtype, o => o.MapFrom(x => x.ClassRoomType.Name.ToString(CultureInfo.InvariantCulture)));
            AutoMapper.Mapper.CreateMap<TcViewModel, Tc>() .ForMember(x => x.AnneeScolaire, o => o.Ignore());
           
    
        }
       
    }
}
