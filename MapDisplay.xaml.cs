using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace 公交线路查询表
{
    /// <summary>
    /// Interaction logic for MapDisplay.xaml
    /// </summary>
    public partial class MapDisplay : Window
    {
        List<GeographiCoordinates> gcSet = new List<GeographiCoordinates>();
        public class GeographiCoordinates
        {
            public double longitude { get; set; }
            public double latitude { get; set; }
            public string placeName { get; set; }
        }
        public class busInfo
        {
            public  string busno { get; set; }
            public  List<Button> stationButtonSet { get; set; }
        }
        MapLayer pushpinLayer;
        MapLayer toolstipLayer;

        public MapDisplay()
        {
            InitializeComponent();
            
            pushpinLayer = new MapLayer();
            pushpinLayer.Name = "PushPinLayer";
            toolstipLayer = new MapLayer();
            toolstipLayer.Name = "ToolsTipLayer";
            BusMap.Children.Add(pushpinLayer);
            BusMap.Children.Add(toolstipLayer);


            

        }

        public void addToCoordinatesList(GeographiCoordinates newGCInfo)
        {
            gcSet.Add(newGCInfo);
        }

        //public void set
        public void AddButtonOnWindow(List<Button> buttons)
        {
            int LineWidth = 0;
            for(int d = 0;d < buttons.Count;d ++)
            {
                Button bt = buttons[d];
                bt.Name = bt.Content.ToString().Replace("\n","");
                bt.Click += new RoutedEventHandler(btMapZoom);
                bt.Height = 100;
                bt.Width = 55;
                bt.VerticalAlignment = VerticalAlignment.Top;
                bt.Style = (Style)this.FindResource("MaterialDesignFlatButton");
                LineWidth += 55;
                lineVB.Children.Add(bt);
            }
            lineVB.Width = LineWidth;
            


        }

        public void setBusNo(string busNo)
        {
            lbBusNo.Content = busNo;
        }
        private void btMapZoom(object sender, RoutedEventArgs e)
        {
            foreach(Pushpin pin in pushpinLayer.Children)
            {
                if (!pin.Name.Equals("当前位置"))
                {
                    pin.Background = new SolidColorBrush(Colors.Blue);
                }
                Button btn = sender as Button;
                if (pin.Name.Equals(btn.Name))
                {
                    pin.Background = new SolidColorBrush(Colors.Green);
                    Location zoomLocation = new Location();
                    zoomLocation = pin.Location;
                    BusMap.Center = zoomLocation;
                }
            }
        }

    

        private Button CreateTextBlock(string text, Boolean Light)
        {
            Button tb = new Button();


            
            //tb.FontSize = 16;
            if (Light)
            {
                tb.Foreground = new SolidColorBrush(Colors.BlueViolet);
            }
            //tb.Foreground = new SolidColorBrush(Colors.BlueViolet);
            String VerticalText = "";
            foreach (char letter in text)
            {
                String tp = Convert.ToString(letter) + "\n";
                VerticalText += tp;
            }
            tb.Content = VerticalText;
            return tb;
        }

        public void AddPointOnMap()
        {
            LocationCollection LC = new LocationCollection();

            foreach (GeographiCoordinates info in gcSet)
            {
                
                Pushpin pi = new Pushpin();
                TextBlock tb = new TextBlock();
                tb.Text = info.placeName;
                tb.Foreground = new SolidColorBrush(Colors.White);
                Location local = new Location(info.latitude, info.longitude);
                LC.Add(local);
                pi.Location = local;
                pi.Name = info.placeName;
                pi.Background = new SolidColorBrush(Colors.Blue);
                pushpinLayer.Children.Add(pi);
                toolstipLayer.AddChild(tb,local);
            }
            Main.GeographiCoordinates cgc = new Main.GeographiCoordinates();
            cgc = Main.getGeographiCoordinatesCurrent();
            Location localCurrent = new Location(cgc.latitude, cgc.longitude);
            Pushpin piCurrent = new Pushpin();
            TextBlock tbCurrent = new TextBlock();
            tbCurrent.Text = "当前位置";
            tbCurrent.Foreground = new SolidColorBrush(Colors.White);
            piCurrent.Name = "当前位置";
            piCurrent.Location = localCurrent;
            pushpinLayer.Children.Add(piCurrent);
            toolstipLayer.AddChild(tbCurrent, localCurrent);

            /*MapPolyline polyline = new MapPolyline();
            polyline.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Blue);
            polyline.StrokeThickness = 5;
            polyline.Opacity = 0.7;
            polyline.Locations = LC;

            BusMap.Children.Add(polyline);*/

            /*MapPolygon polygon = new MapPolygon();
            polygon.Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Blue);
            polygon.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Green);
            polygon.StrokeThickness = 5;
            polygon.Opacity = 0.7;
            polygon.Locations = new LocationCollection() {
        new Location(GeographiCoordinates.latitude,GeographiCoordinates.longitude),
        new Location(GeographiCoordinates.latitude + 10,GeographiCoordinates.longitude +10),
        new Location(GeographiCoordinates.latitude -10,GeographiCoordinates.longitude -10),
            };*/



        }


        private void MapDisplayWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            BusMap.Children.Remove(pushpinLayer);
            BusMap.Children.Remove(toolstipLayer);
            lineVB.Children.RemoveRange(0, lineVB.Children.Count);
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer sv = sender as ScrollViewer;
            //move twice make it flexible
            if (e.Delta > 0)
            {
                lineSV.LineLeft();
                lineSV.LineLeft();
            }
            else
            {
                lineSV.LineRight();
                lineSV.LineRight();
            }
            e.Handled = true;
        }
    }
}
