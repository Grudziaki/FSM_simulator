using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        
    }
}
