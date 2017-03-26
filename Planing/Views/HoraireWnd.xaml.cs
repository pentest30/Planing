using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Planing.Core.Models;
using Planing.Models;

namespace Planing.Views
{
    /// <summary>
    /// Interaction logic for HoraireWnd.xaml
    /// </summary>
    public class HoraireWnd:Window
    

{
    public delegate void UpdateDg(int id);

    public UpdateDg UpdateDataDg;
    private readonly DbModel _db = new DbModel();
    private readonly Dictionary<int, int[]> _seances;

    //public HoraireWnd(int id)
    //{

    //    InitializeComponent();
    //    var item = new Seance();
    //    item.TeacherId = id;
    //    Grid.DataContext = item;
    //    var days = new List<Day>
    //    {
    //        new Day {Id = 1, Jour = "Samedi"},
    //        new Day {Id = 2, Jour = "Dimanche"},
    //        new Day {Id = 3, Jour = "Lundi"},
    //        new Day {Id = 4, Jour = "Mardi"},
    //        new Day {Id = 5, Jour = "Mercredi"},
    //        new Day {Id = 6, Jour = "Jeudi"},
    //    };
    //    _seances = new Dictionary<int, int[]>();
    //    int j = 1;
    //    int k = 1;
    //    for (int f = 0; f < 6; f++)
    //    {
    //        var s = new int[6];

    //        for (int i = 0; i < 6; i++)
    //        {
    //            s[i] = j;
    //            j++;


    //        }
    //        _seances.Add(k, s);
    //        k++;
    //    }

    //    CbAnneeScolaire.ItemsSource = _db.AnneeScolaires.ToList();
    //    CbJours.ItemsSource = days;

    //}

    //private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    //{
    //    var item = Grid.DataContext as Seance;
    //    if (item != null && item.Id == 0)
    //    {
    //        var start = Convert.ToInt32(TxtStart.Text);
    //        var end = Convert.ToInt32(TxtEnd.Text);

    //        var edge = start;
    //        List<int> list = new List<int>();
    //        var item1 = item;
    //        foreach (var x in _seances.Where(x => x.Key == item1.Day))
    //        {
    //            list.AddRange(x.Value);
    //        }

    //        while (edge <= end)
    //        {
    //            var p = list.FirstOrDefault(x => (x - edge)%6 == 0);
    //            item.Number = (item.Day > 1) ? p : edge;
    //            var b = _db.Seances.Any(x => x.Day == item.Day
    //                                         && item.AnneeScolaireId == x.AnneeScolaireId
    //                                         && x.Number == item.Number
    //                                         && x.Semestre == item.Semestre
    //                                         && x.TeacherId == item.TeacherId);
    //            edge++;
    //            if (b) continue;
    //            _db.Seances.Add(item);
    //            _db.SaveChanges();

    //        }


    //    }


    //    if (UpdateDataDg != null && item != null) UpdateDataDg(item.TeacherId);
    //    if (item != null)
    //    {
    //        var ee = item.TeacherId;
    //        item = new Seance();
    //        item.TeacherId = ee;
    //    }
    //    Grid.DataContext = null;
    //    Grid.DataContext = item;

    //}
}
}
