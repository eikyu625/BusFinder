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
    /// Interaction logic for Manager.xaml
    /// </summary>
    public partial class Manager : Window
    {
        public class ManagerAccount
        {
            public int ID { get; set; }
            public String Username { get; set; }
            public String Password { get; set; }
            public int Level { get; set; }
        }

        public class Station
        {
            public int ID { get; set; }
            public String Name { get; set; }
            public int Distance { get; set; }
        }

        public class Bus
        {
            public int ID { get; set; }
            public int BusNo { get; set; }
        }
        
        public int LoginUserLevel;
        
        public Manager()
        {
            InitializeComponent();
            initManager();
            initStation();
            initBus();
            

        }
        /*List<ManagerAccount> managerLists;
        List<Station> stationList;
        List<Bus> busList;
        List那么好用，咱干为啥用链表咧*/
        LinkList<ManagerAccount> managerLinkLists;
        LinkList<Station> stationLinkList;
        LinkList<Bus> busLinkList;

        public void initManager()
        {
            MysqlUtil mysql = new MysqlUtil();
            mysql.GetConnection();
            var rdr = mysql.ReadData("SELECT * FROM manager;");
            managerLinkLists = new LinkList<ManagerAccount>();
            if (rdr.HasRows)
            {
                //managerLists = new List<ManagerAccount>();
                
                while (rdr.Read())
                {
                    Console.WriteLine("{0} {1} {2} {3}", rdr.GetInt32(0), rdr.GetString(1),
                            rdr.GetString(2), rdr.GetString(3));
                    ManagerAccount manager = new ManagerAccount();
                    manager.ID = rdr.GetInt32(0);
                    manager.Username = rdr.GetString(1);
                    manager.Password = rdr.GetString(2);
                    manager.Level = rdr.GetInt32(3);
                    //managerLists.Add(manager);
                    managerLinkLists.Add(manager);
                }
            }
            mysql.Close();

        }


        public void initStation()
        {
            MysqlUtil mysql = new MysqlUtil();
            mysql.GetConnection();
            var rdr = mysql.ReadData("SELECT * FROM station;");
            stationLinkList = new LinkList<Station>();
            if (rdr.HasRows)
            {
                //stationList = new List<Station>();
                
                while (rdr.Read())
                {
                    Console.WriteLine("{0} {1} {2}", rdr.GetInt32(0), rdr.GetString(1),
                            rdr.GetInt32(2));
                    Station station = new Station();
                    station.ID = rdr.GetInt32(0);
                    station.Name = rdr.GetString(1);
                    station.Distance = rdr.GetInt32(2);
                    //stationList.Add(station);
                    stationLinkList.Add(station);
                }
            }
            mysql.Close();
        }

        public void initBus()
        {
            MysqlUtil mysql = new MysqlUtil();
            mysql.GetConnection();
            var rdr = mysql.ReadData("SELECT * FROM businfo;");
            busLinkList = new LinkList<Bus>();
            if (rdr.HasRows)
            {
                //busList = new List<Bus>();
                
                while (rdr.Read())
                {
                    Console.WriteLine("{0} {1} {2}", rdr.GetInt32(0), rdr.GetInt32(1),
                            rdr.GetString(2));
                    Bus bus = new Bus();
                    bus.ID = rdr.GetInt32(0);
                    bus.BusNo = rdr.GetInt32(1);
                    //busList.Add(bus);
                    busLinkList.Add(bus);
                }
            }
            mysql.Close();
        }

        public void initListBox()
        {
            //if(managerLists != null)
            //lbNotice.Content = "初始化列表...";
            if (managerLinkLists != null)
            {
                //managerLists.ForEach(userListBox.ContextMenu.Items.Add();
                /*foreach(ManagerAccount manager in managerLists)
                {
                    userListBox.Items.Add(manager.Username);
                }*/
                userListBox.Items.Clear();
                for (int i = 1; i <= managerLinkLists.Length(); i++)
                {
                    userListBox.Items.Add(managerLinkLists.IndexOf(i).Username);
                }

            }
            //if (stationList != null)
            if (stationLinkList != null)
            {
                //managerLists.ForEach(userListBox.ContextMenu.Items.Add();
                /*foreach (Station station in stationList)
                {
                    stationListBox.Items.Add(station.Name);
                }*/
                stationListBox.Items.Clear();
                for (int i = 1;i <= stationLinkList.Length(); i++)
                {
                    stationListBox.Items.Add(stationLinkList.IndexOf(i).Name);
                }
            }
            //if (busList != null)
            if (busLinkList != null)
            {
                /*foreach (Bus bus in busList)
                {
                    busListBox.Items.Add(bus.BusNo);
                }*/
                busListBox.Items.Clear();
                for (int i = 1; i <= busLinkList.Length(); i++)
                {
                    busListBox.Items.Add(busLinkList.IndexOf(i).BusNo);
                }
            }
            //lbNotice.Content = "数据加载完毕。";
        }
        private void bt_acc_add_Click(object sender, RoutedEventArgs e)
        {
            if(tbAcc.Text.Equals("") || tbPw.Text.Equals("") || tbLv.Text.Equals(""))
            {
                lbNotice.Content = "信息输入不完整！";
                return;
            }
            try
            {
                Int32.Parse(tbLv.Text);
            }
            catch (FormatException error)
            {
                lbNotice.Content = "权限必须是数字！";
                return;
            }
            MysqlUtil mysql = new MysqlUtil();
            mysql.GetConnection();
            if(mysql.InsertData("manager", "", tbAcc.Text, tbPw.Text, tbLv.Text) > 0)
            {
                //链表！！
                ManagerAccount manager = new ManagerAccount();
                manager.ID = managerLinkLists.Length() + 1;
                manager.Username = tbAcc.Text;
                manager.Password = tbPw.Text;
                manager.Level = Int32.Parse(tbLv.Text);                
                managerLinkLists.Add(manager);
                //链表！！
                lbNotice.Content = "添加成功。";
                //initManager();
                initListBox();
            }
            else
            {
                lbNotice.Content = "添加失败。";
            }
            mysql.Close();
        }

        private void Loaded_Window(object sender, RoutedEventArgs e)
        {
            initListBox();
            if (LoginUserLevel > 1)
            {
                bt_acc_add.IsEnabled = true;
                bt_acc_del.IsEnabled = true;
                bt_acc_edit.IsEnabled = true;
            }
        }

        private void userListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //tbAcc.Text = userListBox.SelectedIndex.ToString();
            if (userListBox.SelectedIndex == -1)
            {
                return;
            }
            /*tbAcc.Text = managerLists[userListBox.SelectedIndex].Username;
            tbPw.Text = managerLists[userListBox.SelectedIndex].Password;
            tbLv.Text = (managerLists[userListBox.SelectedIndex].Level).ToString();*/
            tbAcc.Text = managerLinkLists.IndexOf(userListBox.SelectedIndex+1).Username;
            tbPw.Text = managerLinkLists.IndexOf(userListBox.SelectedIndex+1).Password;
            tbLv.Text = managerLinkLists.IndexOf(userListBox.SelectedIndex+1).Level.ToString();
        }

        private void bt_acc_del_Click(object sender, RoutedEventArgs e)
        {
            if (tbAcc.Text.Equals(""))
            {
                lbNotice.Content = "未输入用户名";
                return;
            }
            MysqlUtil mysql = new MysqlUtil();
            mysql.GetConnection();
            if(mysql.UpdateData("DELETE FROM manager WHERE username = '" + tbAcc.Text + "';") > 0)
            {
                //链表！！
                for(int i = 1;i <= managerLinkLists.Length(); i++)
                {
                    if (managerLinkLists.IndexOf(i).Username.Equals(tbAcc.Text))
                    {
                        managerLinkLists.Delete(i);
                    }
                }
                //链表！！
                lbNotice.Content = "删除成功。";
                initListBox();
            }
            else
            {
                lbNotice.Content = "删除失败。";
            }
            mysql.Close();
        }

        private void bt_acc_edit_Click(object sender, RoutedEventArgs e)
        {
            if (userListBox.SelectedIndex == -1)
            {
                lbNotice.Content = "未选中编辑对象！";
                return;
            }
            try
            {
                Int32.Parse(tbLv.Text);
            }
            catch (FormatException error)
            {
                lbNotice.Content = "权限必须是数字！";
                return;
            }
            MysqlUtil mysql = new MysqlUtil();
            mysql.GetConnection();
            if (mysql.UpdateData("UPDATE manager SET username = '" + tbAcc.Text + "',password = '" + tbPw.Text + "',level = '" + tbLv.Text + "' WHERE id = " + managerLinkLists.IndexOf(userListBox.SelectedIndex + 1).ID + ";") > 0)
            {
                //链表！！

                managerLinkLists.IndexOf(userListBox.SelectedIndex + 1).Level = Int32.Parse(tbLv.Text);
                managerLinkLists.IndexOf(userListBox.SelectedIndex + 1).Username = tbAcc.Text;
                managerLinkLists.IndexOf(userListBox.SelectedIndex + 1).Password = tbPw.Text;
                //链表！！
                lbNotice.Content = "更新成功。";
                initListBox();
            }
            else
            {
                lbNotice.Content = "更新失败。";
            }
            mysql.Close();
        }

        private void bt_acc_accsort_Click(object sender, RoutedEventArgs e)
        {
            //冒泡！！
            ManagerAccount temp;
            for (int i = 1; i <= managerLinkLists.Length(); i++)
            {
                for (int j = i + 1; j <= managerLinkLists.Length(); j++)
                {
                    if (managerLinkLists.IndexOf(j).Username.CompareTo(managerLinkLists.IndexOf(i).Username) < 0)
                    {
                        temp = managerLinkLists.IndexOf(j);
                        managerLinkLists.Insert(managerLinkLists.IndexOf(i), j);
                        managerLinkLists.Delete(j + 1);
                        managerLinkLists.Insert(temp, i);
                        managerLinkLists.Delete(i + 1);
                    }
                }
            }
            //冒泡！！
            initListBox();
        }

        private void bt_acc_levelsort_Click(object sender, RoutedEventArgs e)
        {
            //冒泡！！
            ManagerAccount temp;
            for (int i = 1; i <= managerLinkLists.Length(); i++)
            {
                for (int j = i + 1; j <= managerLinkLists.Length(); j++)
                {
                    if (managerLinkLists.IndexOf(j).Level.CompareTo(managerLinkLists.IndexOf(i).Level) < 0)
                    {
                        temp = managerLinkLists.IndexOf(j);
                        managerLinkLists.Insert(managerLinkLists.IndexOf(i), j);
                        managerLinkLists.Delete(j + 1);
                        managerLinkLists.Insert(temp, i);
                        managerLinkLists.Delete(i + 1);
                    }
                }
            }
            //冒泡！！
            initListBox();
        }

        private void bt_station_add_Click(object sender, RoutedEventArgs e)
        {
            if (tbStation.Text.Equals("") || tbDis.Text.Equals(""))
            {
                lbNotice.Content = "信息输入不完整！";
                return;
            }
            try
            {
                Int32.Parse(tbDis.Text);
            }
            catch (FormatException error)
            {
                lbNotice.Content = "里程必须是数字！";
                return;
            }
            MysqlUtil mysql = new MysqlUtil();
            mysql.GetConnection();
            if (mysql.InsertData("station", "", tbStation.Text, tbDis.Text) > 0)
            {
                //链表！！
                Station station = new Station();
                station.ID = stationLinkList.Length() + 1;
                station.Name = tbStation.Text;
                station.Distance = Int32.Parse(tbDis.Text);
                stationLinkList.Add(station);
                //链表！！
                lbNotice.Content = "添加成功。";
                //initManager();
                initListBox();
            }
            else
            {
                lbNotice.Content = "添加失败。";
            }
            mysql.Close();

        }

        private void bt_station_del_Click(object sender, RoutedEventArgs e)
        {
            if (tbStation.Text.Equals(""))
            {
                lbNotice.Content = "未输入站点名！";
                return;
            }
            MysqlUtil mysql = new MysqlUtil();
            mysql.GetConnection();
            if (mysql.UpdateData("DELETE FROM station WHERE name = '" + tbStation.Text + "';") > 0)
            {
                //链表！！
                for (int i = 1; i <= stationLinkList.Length(); i++)
                {
                    if (stationLinkList.IndexOf(i).Name.Equals(tbStation.Text))
                    {
                        stationLinkList.Delete(i);
                    }
                }
                //链表！！
                lbNotice.Content = "删除成功。";
                initListBox();
            }
            else
            {
                lbNotice.Content = "删除失败。";
            }
            mysql.Close();
        }

        private void StationListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (stationListBox.SelectedIndex == -1)
            {
                return;
            }
            /*tbAcc.Text = managerLists[userListBox.SelectedIndex].Username;
            tbPw.Text = managerLists[userListBox.SelectedIndex].Password;
            tbLv.Text = (managerLists[userListBox.SelectedIndex].Level).ToString();*/
            tbStation.Text = stationLinkList.IndexOf(stationListBox.SelectedIndex + 1).Name;
            tbDis.Text = stationLinkList.IndexOf(stationListBox.SelectedIndex + 1).Distance.ToString();
        }

        private void bt_station_edit_Click(object sender, RoutedEventArgs e)
        {
            if (stationListBox.SelectedIndex == -1)
            {
                lbNotice.Content = "未选中编辑对象！";
                return;
            }
            try
            {
                Int32.Parse(tbDis.Text);
            }
            catch (FormatException error)
            {
                lbNotice.Content = "里程必须是数字！";
                return;
            }
            MysqlUtil mysql = new MysqlUtil();
            mysql.GetConnection();
            string sql = "UPDATE station SET name = '" + tbStation.Text + "',distance = '" + tbDis.Text + "' WHERE id = " + stationLinkList.IndexOf(stationListBox.SelectedIndex + 1).ID + ";";
            if (mysql.UpdateData(sql) > 0)
            {
                //链表！！

                stationLinkList.IndexOf(stationListBox.SelectedIndex + 1).Distance = Int32.Parse(tbDis.Text);
                stationLinkList.IndexOf(stationListBox.SelectedIndex + 1).Name = tbStation.Text;
                //链表！！
                lbNotice.Content = "更新成功。";
                initListBox();
            }
            else
            {
                lbNotice.Content = "更新失败。";
            }
            mysql.Close();
        }

        private void bt_station_stisort_Click(object sender, RoutedEventArgs e)
        {
            //冒泡！！
            Station temp;
            for (int i = 1; i <= stationLinkList.Length(); i++)
            {
                for (int j = i + 1; j <= stationLinkList.Length(); j++)
                {
                    if (stationLinkList.IndexOf(j).Name.CompareTo(stationLinkList.IndexOf(i).Name) < 0)
                    {
                        temp = stationLinkList.IndexOf(j);
                        stationLinkList.Insert(stationLinkList.IndexOf(i), j);
                        stationLinkList.Delete(j + 1);
                        stationLinkList.Insert(temp, i);
                        stationLinkList.Delete(i + 1);
                    }
                }
            }
            //冒泡！！
            initListBox();
        }

        private void bt_station_dissort_Click(object sender, RoutedEventArgs e)
        {
            //冒泡！！
            Station temp;
            for (int i = 1; i <= stationLinkList.Length(); i++)
            {
                for (int j = i + 1; j <= stationLinkList.Length(); j++)
                {
                    if (stationLinkList.IndexOf(j).Distance.CompareTo(stationLinkList.IndexOf(i).Distance) < 0)
                    {
                        temp = stationLinkList.IndexOf(j);
                        stationLinkList.Insert(stationLinkList.IndexOf(i), j);
                        stationLinkList.Delete(j + 1);
                        stationLinkList.Insert(temp, i);
                        stationLinkList.Delete(i + 1);
                    }
                }
            }
            //冒泡！！
            initListBox();
        }

        private void bt_bus_add_Click(object sender, RoutedEventArgs e)
        {
            if (tbBus.Text.Equals(""))
            {
                lbNotice.Content = "未输入公交号！";
                return;
            }
            try
            {
                Int32.Parse(tbBus.Text);
            }
            catch (FormatException error)
            {
                lbNotice.Content = "公交必须是数字！";
                return;
            }
            MysqlUtil mysql = new MysqlUtil();
            mysql.GetConnection();
            if (mysql.InsertData("businfo", "", tbBus.Text,"") > 0)
            {
                //链表！！
                Bus bus = new Bus();
                bus.ID = busLinkList.Length() + 1;
                bus.BusNo = Int32.Parse(tbBus.Text);
                busLinkList.Add(bus);
                //链表！！
                lbNotice.Content = "添加成功。";
                //initManager();
                initListBox();
            }
            else
            {
                lbNotice.Content = "添加失败。";
            }
            mysql.Close();

        }

        private void bt_bus_del_Click(object sender, RoutedEventArgs e)
        {
            if (tbBus.Text.Equals(""))
            {
                lbNotice.Content = "未输入公交号！";
                return;
            }
            MysqlUtil mysql = new MysqlUtil();
            mysql.GetConnection();
            if (mysql.UpdateData("DELETE FROM businfo WHERE no = '" + tbBus.Text + "';") > 0)
            {
                //链表！！
                for (int i = 1; i <= busLinkList.Length(); i++)
                {
                    if (busLinkList.IndexOf(i).BusNo.Equals(tbBus.Text))
                    {
                        busLinkList.Delete(i);
                    }
                }
                //链表！！
                lbNotice.Content = "删除成功。";
                initListBox();
            }
            else
            {
                lbNotice.Content = "删除失败。";
            }
            mysql.Close();

        }

        private void bt_bus_edit_Click(object sender, RoutedEventArgs e)
        {
            if (busListBox.SelectedIndex == -1)
            {
                lbNotice.Content = "未选中编辑对象！";
                return;
            }
            try
            {
                Int32.Parse(tbBus.Text);
            }
            catch (FormatException error)
            {
                lbNotice.Content = "公交必须是数字！";
                return;
            }
            MysqlUtil mysql = new MysqlUtil();
            mysql.GetConnection();
            if (mysql.UpdateData("UPDATE businfo SET no = '" + tbBus.Text + "' WHERE id = " + busLinkList.IndexOf(busListBox.SelectedIndex + 1).ID + ";") > 0)
            {
                //链表！！

                busLinkList.IndexOf(stationListBox.SelectedIndex + 1).BusNo = Int32.Parse(tbBus.Text);
                //链表！！
                lbNotice.Content = "更新成功。";
                initListBox();
            }
            else
            {
                lbNotice.Content = "更新失败。";
            }
            mysql.Close();

        }

        private void bt_bus_sort_Click(object sender, RoutedEventArgs e)
        {
            //冒泡！！
            Bus temp;
            for (int i = 1; i <= busLinkList.Length(); i++)
            {
                for (int j = i + 1; j <= busLinkList.Length(); j++)
                {
                    if (busLinkList.IndexOf(j).BusNo.CompareTo(busLinkList.IndexOf(i).BusNo) < 0)
                    {
                        temp = busLinkList.IndexOf(j);
                        busLinkList.Insert(busLinkList.IndexOf(i), j);
                        busLinkList.Delete(j + 1);
                        busLinkList.Insert(temp, i);
                        busLinkList.Delete(i + 1);
                    }
                }
            }
            //冒泡！！
            initListBox();
        }

        private void busListBox_SelctionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (busListBox.SelectedIndex == -1)
            {
                return;
            }
            /*tbAcc.Text = managerLists[userListBox.SelectedIndex].Username;
            tbPw.Text = managerLists[userListBox.SelectedIndex].Password;
            tbLv.Text = (managerLists[userListBox.SelectedIndex].Level).ToString();*/
            tbBus.Text = busLinkList.IndexOf(busListBox.SelectedIndex + 1).BusNo.ToString();
        }

        private void bt_bus_info_Click(object sender, RoutedEventArgs e)
        {
            if (tbBus.Text.Equals(""))
            {
                lbNotice.Content = "未输入公交号！";
                return;
            }
            BusInfo bg = new BusInfo();
            BusInfo.BusInfoUp.BusNo = busLinkList.IndexOf(busListBox.SelectedIndex + 1).BusNo.ToString();
            bg.BTShowMapBus1.IsEnabled = false;
            bg.BTShowMapBus1.Content = "载入中...";
            bg.Show();
        }
    }
}
