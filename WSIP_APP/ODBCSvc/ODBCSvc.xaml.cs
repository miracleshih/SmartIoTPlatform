using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using WSIP;

namespace ODBCSvc
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        string _connectionString = @"Data Source = (LocalDB)\MSSQLLocalDB; Initial Catalog = DB3; Integrated Security = True; MultipleActiveResultSets = True";
        public string connectionString
        {
            get { return _connectionString; }
            set
            {
                _connectionString = value;
                OnPropertyChanged("connectionString");
            }
        }

        string _tableName = "_HC_115";
        //string _tableName = "table1";
        public string tableName
        {
            get { return _tableName; }
            set
            {
                _tableName = value;
                OnPropertyChanged("tableName");
            }
        }


        string _txtLog = "";
        public string txtLog
        {
            get { return _txtLog; }
            set
            {
                _txtLog = value;
                OnPropertyChanged("txtLog");
            }
        }

        DateTime _dtStart = DateTime.Now;
        public DateTime dtStart
        {
            get { return _dtStart; }
            set
            {
                _dtStart = value;
                dtStop = _dtStart.AddSeconds(1);
                OnPropertyChanged("dtStart");
            }
        }

        DateTime _dtStop = DateTime.Now.AddDays(1);
        public DateTime dtStop
        {
            get { return _dtStop; }
            set
            {
                _dtStop = value;
                OnPropertyChanged("dtStop");
            }
        }

        int _totalRecords = 0;
        public int totalRecords
        {
            get { return _totalRecords; }
            set
            {
                _totalRecords = value;
                OnPropertyChanged("totalRecords");
            }
        }

        int _countRecords = 0;
        public int countRecords
        {
            get { return _countRecords; }
            set
            {
                _countRecords = value;
                OnPropertyChanged("countRecords");
            }
        }


        WODBC odBC;
        //SqlConnection conn = null;

        public MainWindow()
        {
            InitializeComponent();

            // CreateOdbcConnection();
            // string str = DateTime.Now.ToStringSQL();

            this.DataContext = this;

            //try
            //{
            //    conn = new SqlConnection(connectionString);
            //    conn.Open();
            //}
            //catch (Exception)
            //{
            //    conn = null;
            //}

            odBC = new WODBC(connectionString);
        }

        private void CreateOdbcConnection()
        {
            ////string connectionString = @"Data Source = (LocalDB)\MSSQLLocalDB; Initial Catalog = VMSDB; Integrated Security = True; MultipleActiveResultSets = True";
            //if (conn is null)
            //    return;
            ////using (var conn = new SqlConnection(connectionString))
            //{
            //    // conn.Open();
            //    string str = $"SELECT * FROM {tableName} where datatime > '{dtStart.ToStringSQL()}' and datatime <= '{dtStop.ToStringSQL()}'";
            //    using (SqlCommand command = new SqlCommand(str, conn))
            //    {
            //        SqlDataAdapter  sda = new SqlDataAdapter(command);
            //        DataTable  dt = new DataTable(tableName);
            //        sda.Fill(dt);
            //        MemoryStream ms = new MemoryStream();
            //        //dt.WriteXml("Employees.xml");
            //        dt.WriteXml(ms);
            //        ms.Seek(0, SeekOrigin.Begin);
            //        StreamReader sr = new StreamReader(ms);
            //        string str2 = sr.ReadToEnd();
            //        Console.WriteLine(str2);
            //        if(File.Exists("data_segment.txt"))
            //            File.AppendAllText("data_segment.txt", str2);
            //        else
            //            File.WriteAllText("data_segment.txt", str2);
            //        totalRecords = dt.Rows.Count;
            //    }
            //}

            string str = $"SELECT * FROM {tableName} where datatime > '{dtStart.ToStringSQL()}' and datatime <= '{dtStop.ToStringSQL()}'";
            string strResult;
            totalRecords = odBC.SQLQuery(str, out strResult);
            if (totalRecords < 0)
                return;

            if (File.Exists("data_segment.txt"))
                File.AppendAllText("data_segment.txt", strResult);
            else
                File.WriteAllText("data_segment.txt", strResult);
        }

        private void countBySec(string tabName)
        {
            //if (conn is null)
            //    return;

            countRecords = 0;
            DateTime dtcur = dtStart;
            for(; ; )
            {
                DateTime dtcur1 = dtcur.AddSeconds(1);
                string str = $"SELECT * FROM {tabName} where datatime > '{dtcur.ToStringSQL()}' and datatime <= '{dtcur1.ToStringSQL()}'";
                dtcur = dtcur1;
                if (dtcur > dtStop)
                    break;

                //using (SqlCommand command = new SqlCommand(str, conn))
                //{
                //    SqlDataAdapter sda = new SqlDataAdapter(command);
                //    DataTable dt = new DataTable(tabName);
                //    sda.Fill(dt);
                //    countRecords += dt.Rows.Count;

                //    MemoryStream ms = new MemoryStream();
                //    //dt.WriteXml("Employees.xml");
                //    dt.WriteXml(ms);
                //    ms.Seek(0, SeekOrigin.Begin);
                //    StreamReader sr = new StreamReader(ms);
                //    string str2 = sr.ReadToEnd();
                //    Console.WriteLine(str2);
                //    if (File.Exists("data_sec.txt"))
                //        File.AppendAllText("data_sec.txt", str2);
                //    else
                //        File.WriteAllText("data_sec.txt", str2);
                //}
                string strResult = "";
                int cnt = odBC.SQLQuery(str, out strResult);
                if (cnt < 0)
                    return;

                countRecords += cnt;
                if (File.Exists("data_sec.txt"))
                    File.AppendAllText("data_sec.txt", strResult);
                else
                    File.WriteAllText("data_sec.txt", strResult);

            }

        }


        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            //List<string> tabList = new List<string>()
            //{
            //    "_HC_19B",
            //};
            List<string> tabList = new List<string>()
            {
                "_HC_115","_HC_120B", "_HC_150", "_HC_19B", "_HC_1PLUS8B",
                "_HC_20B", "_HC_2P8M", "_HC_37B", "_HC_61B", "_HC_61B_MODE",
                "_HC_7B", "_HC_7B1", "_HC_84B", "_HC_A13", "_HC_CB",
                "_HC_CCV", "_HC_CDCC", "_HC_CW", "_HC_DRY_CAM", "_HC_FG",
                "_HC_FS13", "_HC_FW", "_HC_G63", "_HC_G98", "_HC_HCV100",
                "_HC_HCV65", "_HC_HCV90", "_HC_HIPA2", "_HC_INTEGRATED", "_HC_LE",
                "_HC_M85", "_HC_ORA_120", "_HC_RCP", "_HC_ST2", "_HC_SZ3",
                // "_HC_Taymer51_Defect", "_HC_Taymer53_Defect", "_HC_Taymer53_PLV", "_HC_Taymer53_Print", "_HC_POC_Test",
                // "ALLFILENAMES"
            };

            // txtLog = "";
            //Stopwatch stptotal = Stopwatch.StartNew();
            Stopwatch stp = Stopwatch.StartNew();
            //CreateOdbcConnection();
            //txtLog += $"Query by hour takes {stp.ElapsedMilliseconds} ms ";
            //stp.Restart();
            //countBySec("_HC_115");
            for(int i=0; i<tabList.Count; i++)
            {
                stp.Restart();
                countBySec(tabList[i]);
                txtLog += $"Table {tabList[i]} takes {stp.ElapsedMilliseconds} ms";
                // System.Threading.Thread.Sleep(500);
            }

            txtLog += "\n";
            // txtLog += $"Total takes {stptotal.ElapsedMilliseconds} ms\n";

            // txtLog += "Done\n";
        }

    }
}
