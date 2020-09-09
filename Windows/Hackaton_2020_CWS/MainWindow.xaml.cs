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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Ports;

using MySql.Data.MySqlClient;

namespace Hackaton_2020_CWS
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private static int everyuser_count = 2;
        private int adduser_count = 0;
        private int current_age;
        SerialPort arduinoPort = new SerialPort();

        public string source = "Server=localhost;Port=3306;Database=hackaton_database;Uid=root;password=shk0523shk";
        public int grade = 0;
        public int class_ = 0;
        public int usercout = 0;
        public MainWindow()
        {
            InitializeComponent();
            //*******************************시리얼 연결 부분*******************************
            if (!arduinoPort.IsOpen)
            {
                int check = 0;
                foreach(string port in SerialPort.GetPortNames())
                {
                    if (port != null)
                    {
                        check--;
                        arduinoPort.PortName = port;
                    }
                }
                MessageBox.Show(arduinoPort.PortName);
                if (check < 0)
                {
                    arduinoPort.BaudRate = 9600;
                    arduinoPort.DataBits = 8;
                    arduinoPort.StopBits = StopBits.One;
                    arduinoPort.Parity = Parity.None;
                    arduinoPort.DataReceived += new SerialDataReceivedEventHandler(arduinoPort_DataReceived);
                    arduinoPort.Open();
                }
            }
            //******************************************************************************
            if (!arduinoPort.IsOpen)
            {
                MessageBox.Show("연결 안됨");
            }
        }

        private void arduinoPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Console.WriteLine(arduinoPort.ReadLine());
            ID_SEARCH(source, "랩8", arduinoPort.ReadLine());

        }

        private void ID_SEARCH(string adress ,string location ,string id)
        {
            MySqlConnection con = new MySqlConnection(adress);
            string command_2 = "SELECT * FROM information WHERE id=" + id;
            string chk = "";
            try
            {
                con.Open();
                MySqlCommand command_read = new MySqlCommand(command_2, con);
                MySqlDataReader reader = command_read.ExecuteReader();
                if (reader.Read())
                {
                    chk = Convert.ToString(reader["chk"]);
                }
                if (chk.Equals("False") == true) chk = "True";
                reader.Close();
                string command_1 = "UPDATE information SET location=\'" + location + "\', chk=" + Convert.ToBoolean(chk) + " WHERE id=" + Int32.Parse(id);
                MySqlCommand command = new MySqlCommand(command_1, con);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void Connection_MEMBER(string Source, int grade, int class_, int number)
        {
            string school_num = "";
            string name = "";
            bool check = false;
            MySqlConnection con = new MySqlConnection(Source);
            string Command = "SELECT * FROM information WHERE grade=" + grade.ToString() + " AND class=" + class_.ToString() + " AND num="+number.ToString();
            MySqlCommand read_command = new MySqlCommand(Command, con);
            try {
                con.Open();
                MySqlDataReader reader = read_command.ExecuteReader();
                for (; reader.Read();)
                {
                    school_num += Convert.ToString(reader["grade"]);
                    school_num += Convert.ToString(reader["class"]);
                    string num = Convert.ToString(reader["num"]);
                    if (Int32.Parse(num) < 10) school_num += "0" + num;
                    else school_num += num;
                    name += Convert.ToString(reader["name"]);
                    check = Convert.ToBoolean(reader["chk"]);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
                if (usercout++ == 0)
                {
                    user1.Content = string_make(school_num, name, check);
                    user1.FontSize = 36;
                    user1.Visibility = Visibility.Visible;
                }
                else
                {
                    user2.Content = string_make(school_num, name, check);
                    user2.FontSize = 36;
                    user2.Visibility = Visibility.Visible;
                }
            }  
        }

        private string string_make(string number, string name, bool check)
        {
            string value = "";

            value += number + "        ";
            value += name + "                       출석 여부:             ";
            if (check == true) value += "   출석";
            else value += "미출석";
                return value;
        }

        private void mainobject_hide()
        {
            mainpage_image.Visibility = Visibility.Hidden;
            mainpage_textblock.Visibility = Visibility.Hidden;
        }

        private void subobject_show()
        {
            FirstClass.Visibility = Visibility.Visible;
            SecondClass.Visibility = Visibility.Visible;
            ThirdClass.Visibility = Visibility.Visible;
        }

        private void subobject_hide()
        {
            FirstClass.Visibility = Visibility.Hidden;
            SecondClass.Visibility = Visibility.Hidden;
            ThirdClass.Visibility = Visibility.Hidden;
        }

        private void subobjectcolor_default()
        {
            FirstClass.Background = Brushes.LightGray;
            SecondClass.Background = Brushes.LightGray;
            ThirdClass.Background = Brushes.LightGray;
        }

        private void user_default()
        {
            user1.Visibility = Visibility.Hidden;
            user2.Visibility = Visibility.Hidden;
        }

        private void agecolor_default()
        {
            one_button.Style = Resources["RoundButtonTemplate"] as Style;
            two_button.Style = Resources["RoundButtonTemplate"] as Style;
            three_button.Style = Resources["RoundButtonTemplate"] as Style;
        }


        private void Home_button_Click(object sender, RoutedEventArgs e)
        {
            user_default();
            agecolor_default();
            subobject_hide();
            subobjectcolor_default();
            mainpage_image.Visibility = Visibility.Visible;
            mainpage_textblock.Visibility = Visibility.Visible;
        }

        private void One_button_Click(object sender, RoutedEventArgs e)
        {
            agecolor_default();
            current_age = 1;
            one_button.Style = Resources["roundButtonTemplate"] as Style;
            user_default();
            subobjectcolor_default();
            mainobject_hide();
            subobject_show();
        }

        private void Two_button_Click(object sender, RoutedEventArgs e)
        {
            agecolor_default();
            current_age = 2;
            two_button.Style = Resources["roundButtonTemplate"] as Style;
            user_default();
            subobjectcolor_default();
            mainobject_hide();
            subobject_show();
        }

        private void Three_button_Click(object sender, RoutedEventArgs e)
        {
            agecolor_default();
            current_age = 3;
            three_button.Style = Resources["roundButtonTemplate"] as Style;
            user_default();
            subobjectcolor_default();
            mainobject_hide();
            subobject_show();
        }

        private void FirstClass_Click(object sender, RoutedEventArgs e)
        {
            subobjectcolor_default();
            user_default();
            FirstClass.Background = Brushes.White;
            mainobject_hide();
            subobject_show();
        }

        private void SecondClass_Click(object sender, RoutedEventArgs e)
        {
            user_default();
            /*
            if (current_age == 1)
            {
                user1.Content = string_make("1201", "sad", true);
                user1.FontSize = 36;
                user2.Content = string_make("1202", "asd", false);
                user2.FontSize = 36;
                user1.Visibility = Visibility.Visible;
                user2.Visibility = Visibility.Visible;
            }
            */
            Connection_MEMBER(source, current_age, 2, 3);
            Connection_MEMBER(source, current_age, 2, 9);
            usercout = 0;
            subobjectcolor_default();
            SecondClass.Background = Brushes.White;
            mainobject_hide();
            subobject_show();
        }

        private void ThirdClass_Click(object sender, RoutedEventArgs e)
        {
            subobjectcolor_default();
            user_default();
            ThirdClass.Background = Brushes.White;
            mainobject_hide();
            subobject_show();
        }
    }
}
