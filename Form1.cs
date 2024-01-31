using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CsvHelper;
using CsvHelper.Configuration;

namespace GRM_Dataconverter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            label1.Visible = false;
        }

        public class Foo
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Time { get; set; }
            public string Lat { get; set; }
            public string Lon { get; set; }
            public string Mag { get; set; }
            public string MagValid { get; set; }
            public string CompX { get; set; }
            public string CompY { get; set; }
            public string CompZ { get; set; }
            public string GyroX { get; set; }
            public string GyroY { get; set; }
            public string GyroZ { get; set; }
            public string AccX { get; set; }
            public string AccY { get; set; }
            public string AccZ { get; set; }
            public string ImuTemp { get; set; }
            public string Track { get; set; }
            public string Loc { get; set; }
            public string Hdop { get; set; }
            public string FixQ { get; set; }
            public string Satellites { get; set; }
            public string Altitude { get; set; }
            public string HeightOver { get; set; }
            public string SpeedOver { get; set; }
            public string MagVar { get; set; }
            public string VarDir { get; set; }
            public string ModeInd { get; set; }
            public string Gga { get; set; }
            public string Rmc { get; set; }
            public string Eventcode { get; set; }
            public string Eventinfo { get; set; }
            public string Eventdatalength { get; set; }
            public string Eventdata { get; set; }
        }

        public class QuSpin
        {
            public string Mag_TS { get; set; }
            public string TimeStamp { get; set; }
            public string Mag2 { get; set; }
            public string Sens { get; set; }
            public string X { get; set; }
            public string Y { get; set; }
            public string Z { get; set; }
            public string Vector { get; set; }
            public string Lat2 { get; set; }
            public string Lon2 { get; set; }
            public string Height { get; set; }
            public string GPS_Y { get; set; }
            public string GPS_M { get; set; }
            public string GPS_D { get; set; }
            public string GPS_H { get; set; }
            public string GPS_Min { get; set; }
            public string GPS_S { get; set; }
            public string AIKA { get; set; }
            public string TEMP { get; set; }

        }

        public void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            label1.Visible = false;
            progressBar1.Value = 0;
            DialogResult result = openFileDialog1.ShowDialog();
            progressBar1.Value = 5;
            if (result == DialogResult.OK)
            {
                progressBar1.Value = 10;
                string tiedosto = openFileDialog1.FileName;

                textBox1.Text = tiedosto;
                progressBar1.Value = 20;
                using (var reader = new StreamReader(tiedosto))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = new List<QuSpin>();
                    progressBar1.Value = 30;
                    csv.Read();
                    csv.ReadHeader();
                    progressBar1.Value = 40;
                    while (csv.Read())
                    {
                        var record = new QuSpin
                        {
                            Mag_TS = csv.GetField("Mag_TS_US"),
                            TimeStamp = csv.GetField("TimeStamp_ms"),
                            Mag2 = csv.GetField("MagData"),
                            Sens = csv.GetField("Sensitivity"),
                            X = csv.GetField("X"),
                            Y = csv.GetField("Y"),
                            Z = csv.GetField("Z"),
                            Vector = csv.GetField("Vector_Sensitivity"),
                            Lat2 = csv.GetField("Latitude"),
                            Lon2 = csv.GetField("Longitude"),
                            Height = csv.GetField("Height"),
                            GPS_Y = csv.GetField("GPS_Year"),
                            GPS_M = csv.GetField("GPS_Month"),
                            GPS_D = csv.GetField("GPS_Day"),
                            GPS_H = csv.GetField("GPS_Hour"),
                            GPS_Min = csv.GetField("GPS_Minute"),
                            GPS_S = csv.GetField("GPS_Second"),
                            AIKA = ""
                            //TEMP = ""
                        };

                        records.Add(record);
                    }
                    progressBar1.Value = 50;
                    int len = records.Count;
                    progressBar1.Value = 55;
                    Console.WriteLine(records[1].GPS_S);
                    Console.WriteLine(records[1].GPS_S.Length);
                    Console.WriteLine(records[1].GPS_S.Substring(0, records[1].GPS_S.IndexOf('.')));
                    for (int i = 0; i < len; i++)
                    {
                        //records[i].TEMP = records[i].GPS_S.Substring(0, records[i].GPS_S.IndexOf('.'));
                        if (new[] {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9"}.Contains(records[i].GPS_S.Substring(0, records[i].GPS_S.IndexOf('.'))))
                        {
                            records[i].GPS_S = 0 + records[i].GPS_S;
                        }
                        if (records[i].GPS_S.Length > 6)
                        {
                            records[i].GPS_S = records[i].GPS_S.Substring(0, records[i].GPS_S.IndexOf('.')) + records[i].GPS_S.Substring(records[i].GPS_S.IndexOf('.'), 4);

                        }
                        else if (records[i].GPS_S.Length < 6)
                        {
                            records[i].GPS_S = records[i].GPS_S + 0;
                        }
                        else if (records[i].GPS_S.Length < 5)
                        {
                            records[i].GPS_S = records[i].GPS_S + 00;
                        }

                        if (records[i].GPS_H.Length < 2)
                        {
                            records[i].GPS_H = 0 + records[i].GPS_H;
                        }
                        if (records[i].GPS_Min.Length < 2)
                        {
                            records[i].GPS_Min = 0 + records[i].GPS_Min;
                        }
                        records[i].AIKA = records[i].GPS_H + records[i].GPS_Min + records[i].GPS_S;
                    }
                    progressBar1.Value = 60;
                    progressBar1.Value = 70;
                    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        HasHeaderRecord = false,
                        Delimiter = textBox3.Text,
                    };
                    progressBar1.Value = 80;
                    string tiedosto2 = System.IO.Path.GetDirectoryName(tiedosto) + "\\" + System.IO.Path.GetFileNameWithoutExtension(tiedosto) + "_converted" + ".csv";
                    textBox2.Text = tiedosto2;
                    using (var writer = new StreamWriter(tiedosto2))
                    using (var csv2 = new CsvWriter(writer, config))
                    {
                        csv2.WriteRecords(records);
                    }
                    progressBar1.Value = 90;
                    Console.WriteLine(records[1].AIKA);
                }
            }
            progressBar1.Value = 100;
            label1.Visible = true;
        }
        public void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            label1.Visible = false;
            progressBar1.Value = 0;
            // Show the dialog and get result.
            DialogResult result = openFileDialog1.ShowDialog();
            progressBar1.Value = 5;
            if (result == DialogResult.OK) // Test result.
            {
                progressBar1.Value = 10;
                string tiedosto = openFileDialog1.FileName;

                textBox1.Text = tiedosto;
                progressBar1.Value = 20;
                using (var reader = new StreamReader(tiedosto))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = new List<Foo>();
                    progressBar1.Value = 30;
                    csv.Read();
                    csv.ReadHeader();
                    progressBar1.Value = 40;
                    while (csv.Read())
                    {

                        var record = new Foo
                        {
                            Id = csv.GetField("Counter"),
                            Name = csv.GetField("Date"),
                            Time = csv.GetField("Time"),
                            Lat = csv.GetField("Latitude"),
                            Lon = csv.GetField("Longitude"),
                            Mag = csv.GetField("Mag"),
                            MagValid = csv.GetField(" MagValid"),
                            CompX = csv.GetField("CompassX"),
                            CompY = csv.GetField(" CompassY"),
                            CompZ = csv.GetField(" CompassZ"),
                            GyroX = csv.GetField("GyroscopeX"),
                            GyroY = csv.GetField(" GyroscopeY"),
                            GyroZ = csv.GetField(" GyroscopeZ"),
                            AccX = csv.GetField("AccelerometerX"),
                            AccY = csv.GetField(" AccelerometerY"),
                            AccZ = csv.GetField(" AccelerometerZ"),
                            ImuTemp = csv.GetField("ImuTemperature"),
                            Track = csv.GetField("Track"),
                            Loc = csv.GetField("LocationSource"),
                            Hdop = csv.GetField("Hdop"),
                            FixQ = csv.GetField("FixQuality"),
                            Satellites = csv.GetField(" SatellitesUsed"),
                            Altitude = csv.GetField(" Altitude"),
                            HeightOver = csv.GetField("HeightOverEllipsoid"),
                            SpeedOver = csv.GetField("SpeedOverGround"),
                            MagVar = csv.GetField("MagneticVariation"),
                            VarDir = csv.GetField("VariationDirection"),
                            ModeInd = csv.GetField("ModeIndicator"),
                            Gga = csv.GetField("GgaSentence"),
                            Rmc = csv.GetField("RmcSentence"),
                            Eventcode = csv.GetField("EventCode"),
                            Eventinfo = csv.GetField("EventInfo"),
                            Eventdatalength = csv.GetField("EventDataLength"),
                            Eventdata = csv.GetField("EventData")
                        };

                        records.Add(record);
                    }
                    Console.WriteLine(records[0]);
                    progressBar1.Value = 50;
                    Console.WriteLine(records[0].Time);
                    int len = records.Count;
                    progressBar1.Value = 55;
                    for (int i = 0; i < len; i++)
                    {
                        records[i].Time = records[i].Time.Replace(".", String.Empty);
                        records[i].Time = records[i].Time.Insert(6, ".");
                    }
                    progressBar1.Value = 60;
                    /*Console.WriteLine(records[0].Id);
                    Console.WriteLine(records[0].Name);
                    Console.WriteLine(records[0].Time);
                    Console.WriteLine(records[0].Lat);
                    Console.WriteLine(records[0].Lon);
                    Console.WriteLine(records[0].Mag);
                    Console.WriteLine(records[0].MagValid);
                    Console.WriteLine(records[0].CompX);
                    Console.WriteLine(records[0].CompY);
                    Console.WriteLine(records[0].CompZ);
                    Console.WriteLine(records[0].GyroX);
                    Console.WriteLine(records[0].GyroY);
                    Console.WriteLine(records[0].GyroZ);
                    Console.WriteLine(records[0].AccX);
                    Console.WriteLine(records[0].AccY);
                    Console.WriteLine(records[0].AccZ);
                    Console.WriteLine(records[0].ImuTemp);
                    Console.WriteLine(records[0].Track);
                    Console.WriteLine(records[0].Loc);
                    Console.WriteLine(records[0].Hdop);
                    Console.WriteLine(records[0].FixQ);
                    Console.WriteLine(records[0].Satellites);
                    Console.WriteLine(records[0].Altitude);*/
                    
                    progressBar1.Value = 70;
                    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        HasHeaderRecord = false,
                        Delimiter = textBox3.Text,
                    };
                    progressBar1.Value = 80;
                    string tiedosto2 = System.IO.Path.GetDirectoryName(tiedosto) + "\\" + System.IO.Path.GetFileNameWithoutExtension(tiedosto) + "_converted" + ".csv";
                    textBox2.Text = tiedosto2;
                    using (var writer = new StreamWriter(tiedosto2))
                    using (var csv2 = new CsvWriter(writer, config))
                    {
                        csv2.WriteRecords(records);
                    }
                    progressBar1.Value = 90;
                }
            }

            progressBar1.Value = 100;
            label1.Visible = true;
        }
    }
}