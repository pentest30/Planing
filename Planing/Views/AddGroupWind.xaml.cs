using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using Planing.Core.Models;

namespace Planing.Views
{
    /// <summary>
    /// Interaction logic for AddGroupWind.xaml
    /// </summary>
    public partial class AddGroupWind
    {
        DbModel _db = new DbModel();
        public delegate void UpdateDg();
        public UpdateDg UpdateDataDg;
        public AddGroupWind()
        {
            InitializeComponent();
            CbArticle.ItemsSource = _db.Sections.ToList();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var item = Grid.DataContext as Groupe;
            var start = Convert.ToInt32(TxtStart.Text);
            var end = Convert.ToInt32(TxtEnd.Text);
            if (end >= start)
            {
                for (int i = start; i <=end; i++)
                {
                    if (item != null)
                    {
                        var firstOrDefault = _db.Sections.FirstOrDefault(x => x.Id == item.SectionId);
                        if (firstOrDefault != null)
                            item.Semestre = firstOrDefault.Semestre;
                        item.Name = "Group "+ i.ToString(CultureInfo.InvariantCulture);
                        item.Code ="G"+ i.ToString(CultureInfo.InvariantCulture);
                        _db.Groupes.Add(item);
                        _db.SaveChanges();
                    }
                }
            }
           
            if (UpdateDataDg != null && item != null) UpdateDataDg();
        }
    }
}
