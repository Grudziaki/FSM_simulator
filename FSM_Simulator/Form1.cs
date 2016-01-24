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
        public static List<StateChange> list_of_possible_next_states = new List<StateChange>();
        //lista wszystkich kolejek automatu FSM
        public static List<MessageQueue> list_of_message_queues = new List<MessageQueue>();
        //Obecny stan automatu
        public static State state_present = new State();
        //Nazwa automatu
        public static string FSM_name;

        public static int message_counter = 0;

        public Form1()
        {
            try
            {
                InitializeComponent();
                show_possible_changes();
                
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

        private void button1_Click(object sender, EventArgs e)
        {
            label4.Text = state_present.state_name;
            show_possible_changes();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public void show_possible_changes() 
        {
            richTextBox1.Clear();
            Utils.check_possible_states();
            foreach (StateChange change in list_of_possible_next_states)
            {
                richTextBox1.AppendText(Environment.NewLine + change.next_state_string + " " + change.type);
            } 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Utils.change_state();
            show_possible_changes();
            label4.Text = state_present.state_name;
        }
    }
}
