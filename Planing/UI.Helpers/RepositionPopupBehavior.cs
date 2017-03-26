//using System;
//using System.Windows;
//using System.Windows.Controls.Primitives;
//namespace Planing.UI.Helpers
//{
//    //class RepositionPopupBehavior : Behavior<Popup>
//    //{
//    //    protected override void OnAttached()
//    //    {
//    //        base.OnAttached();
//    //        var window = Window.GetWindow(AssociatedObject.PlacementTarget);
//    //        if (window == null) { return; }
//    //        window.LocationChanged += OnLocationChanged;
//    //        window.SizeChanged     += OnSizeChanged;
//    //        AssociatedObject.Loaded += AssociatedObject_Loaded;
//    //    }

//    //    void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
//    //    {
//    //        //AssociatedObject.HorizontalOffset = 7;
//    //        //AssociatedObject.VerticalOffset = -AssociatedObject.Height;
//    //    }

//    //    /// <summary>
//    //    /// Called when the behavior is being detached from its <see cref="Behavior.AssociatedObject"/>, but before it has actually occurred.
//    //    /// </summary>
//    //    protected override void OnDetaching()
//    //    {
//    //        base.OnDetaching();
//    //        var window = Window.GetWindow(AssociatedObject.PlacementTarget);
//    //        if (window == null) { return; }
//    //        window.LocationChanged -= OnLocationChanged;
//    //        window.SizeChanged     -= OnSizeChanged;
//    //        AssociatedObject.Loaded -= AssociatedObject_Loaded;
//    //    }

     
//    //    /// <summary>
//    //    /// Handles the <see cref="Window.LocationChanged"/> routed event which occurs when the window's location changes.
//    //    /// </summary>
//    //    /// <param name="sender">
//    //    /// The source of the event.
//    //    /// </param>
//    //    /// <param name="e">
//    //    /// An object that contains the event data.
//    //    /// </param>
//    //    private void OnLocationChanged(object sender, EventArgs e)
//    //    {
//    //        var offset = AssociatedObject.HorizontalOffset;
//    //        AssociatedObject.HorizontalOffset = offset + 1;
//    //        AssociatedObject.HorizontalOffset = offset;
//    //    }

//    //    /// <summary>
//    //    /// Handles the <see cref="Window.SizeChanged"/> routed event which occurs when either then <see cref="Window.ActualHeight"/> or the
//    //    /// <see cref="Window.ActualWidth"/> properties change value.
//    //    /// </summary>
//    //    /// <param name="sender">
//    //    /// The source of the event.
//    //    /// </param>
//    //    /// <param name="e">
//    //    /// An object that contains the event data.
//    //    /// </param>
//    //    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
//    //    {
//    //        var offset = AssociatedObject.HorizontalOffset;
//    //        AssociatedObject.HorizontalOffset = offset + 1;
//    //        AssociatedObject.HorizontalOffset = offset;
//    //    }
//    }
//}
