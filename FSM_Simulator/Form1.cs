using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace FSM_Simulator
{
    public partial class Form1 : Form
    {
        //lista wszystkich stanów automatu FSM
        public static List<State> list_of_states = new List<State>();
        //lista możliwych przejsc po uwzglednieniu kolejek
        public static List<StateChange> list_of_possible_next_states = new List<StateChange>();
        //lista wszystkich kolejek automatu FSM
        public static List<MessageQueue> list_of_message_queues = new List<MessageQueue>();
        //Obecny stan automatu
        public static State state_present = new State();
        //Nazwa automatu
        public static string FSM_name;

        public static int message_counter = 0;

        public static Listener server;

        public static Boolean PsujOnOff = false;

        public static System.Threading.Timer Timer;

        public static int timeout;

        public static Boolean turnOnTimer = false;

        public Form1()
        {
            try
            {
                InitializeComponent();
                show_possible_changes();
                button2.Enabled = false;
                server = new Listener();
                server.CreateServer();
                server.StartListening();
            }
            catch(Exception e)
            {
                MessageBox.Show("blad: " + e);
            }
        }

        //wczytuje opis automatu z pliku .xml
        //zrobić
        public void load_from_xml(string filename)
        {
            try
            {
                XDocument config = XDocument.Load(filename);
                XElement name = config.Element("FSM");
                FSM_name = (string)name.Attribute("Name");
                foreach (XElement state in config.Descendants("State"))
                {
                    State stan = new State();
                    stan.state_name = (string)state.Attribute("State_Name");
                    stan.list_of_state_changes.Clear();
                    foreach (XElement state_hop in state.Descendants("State_Hop"))
                    {
                        stan.list_of_state_changes.Add(new StateChange((string)state_hop.Attribute("Next_State"), (StateChange.Type)Enum.Parse(typeof(StateChange.Type), (string)state_hop.Attribute("State_Type")), (string)state_hop.Attribute("Signal_From"), (string)state_hop.Attribute("Signal_To"), (string)state_hop.Attribute("Signal")));                    
                    }
                    list_of_states.Add(stan);
                }

                state_present = list_of_states[0];
            }
            catch (Exception e)
            {
                MessageBox.Show("blad pliku .xml" + e);
            }
        }

        public void refresh_queues()
        {
            richTextBox2.Clear();
            foreach (MessageQueue queue in list_of_message_queues)
            {
                if (queue.signals.Count() != 0)
                {
                    richTextBox2.AppendText(Environment.NewLine + "Kolejka: " + queue.from);
                    richTextBox2.AppendText(Environment.NewLine + "Sygnał: " + queue.signals.Peek());
                }
            }
            show_possible_changes();
        }

        public void show_possible_changes()
        {
            richTextBox1.Clear();
            Utils.check_possible_states();
            if (list_of_possible_next_states.Count() == 0)
                button2.Enabled = false;
            else
                button2.Enabled = true;
                foreach (StateChange change in list_of_possible_next_states)
                {
                    richTextBox1.AppendText(Environment.NewLine + change.next_state_string + " " + change.type);
                }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label4.Text = state_present.state_name;
            show_possible_changes();
            button2.Enabled = true;
            button1.Enabled = false;
            this.Text = FSM_name;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (turnOnTimer) {
                timeout = Convert.ToInt32(textBox1.Text);
                Timer = new System.Threading.Timer(new TimerCallback(Utils.change_state), null, 2000, timeout * 1000);
            }
            else
                Utils.change_state();
            show_possible_changes();
            refresh_queues();
            label4.Text = state_present.state_name;
        }
    
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                PsujOnOff = true;
            else
                PsujOnOff = false;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
                turnOnTimer = true;
            else
            {
                Timer.Change(Timeout.Infinite, Timeout.Infinite);
                turnOnTimer = false;
            }
        }
    }
}
