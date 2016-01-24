using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
        public static List<string> list_of_possible_next_states = new List<string>();
        //lista wszystkich kolejek automatu FSM
        public static List<MessageQueue> list_of_message_queues = new List<MessageQueue>();
        //Obecny stan automatu
        public static State state_present;
        //Nazwa automatu
        public static string FSM_name;

        public static int message_counter = 0;

        public Form1()
        {
            InitializeComponent();
        }

        //wczytuje opis automatu z pliku .xml
        //zrobić
        public void load_from_xml(string filename)
        {
            try
            {
                XDocument config = XDocument.Load(filename);
                State stan = new State();
                foreach (XElement state in config.Descendants("State"))
                {
                    stan.state_name = (string)state.Attribute("State_Name");
                    stan.list_of_state_changes.Clear();
                    foreach (XElement state_hop in state.Descendants("State_Hop"))
                        stan.list_of_state_changes.Add(new StateChange((string)state_hop.Attribute("Next_State"), (StateChange.Type)Enum.Parse(typeof(StateChange.Type), (string)state_hop.Attribute("State_Type")), (string)state_hop.Attribute("Signal_From"), (string)state_hop.Attribute("Signal_To"), (string)state_hop.Attribute("Signal")));
                    list_of_states.Add(stan);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("blad pliku .xml" + e);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        
    }
}
