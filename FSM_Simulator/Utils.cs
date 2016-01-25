using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;

namespace FSM_Simulator
{
    internal static class Utils
    {
        //serializuje obiekty do wyslania przez UDP
        public static String SerializeObject(Object pObject)
        {
            try
            {
                String XmlizedString = null;
                XmlSerializer xs = new XmlSerializer(pObject.GetType());
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                System.IO.StringWriter writer = new System.IO.StringWriter(sb);
                xs.Serialize(writer, pObject);
                XmlizedString = sb.ToString();
                return XmlizedString;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                return null;
            }
        }
        //deserializuje obiekty otrzymane przez UDP
        public static Object DeserializeObject(String pXmlizedString, Type type)
        {
            XmlSerializer xs = new XmlSerializer(type);
            StringReader reader = new StringReader(pXmlizedString);
            XmlTextReader xmlTextReader = new XmlTextReader(reader);
            return xs.Deserialize(xmlTextReader);
        }
        //sprawdza kolejki i możliwe nastepne stany
        public static void check_possible_states()
        {
            try
            {
                Form1.list_of_possible_next_states.Clear();
                foreach (StateChange change in Form1.state_present.list_of_state_changes)
                {
                    if ((change.type == StateChange.Type.SEND) || (change.type == StateChange.Type.TAU))
                    {
                        string name = change.next_state_string;
                        Form1.list_of_possible_next_states.Add(change);
                    }
                    else
                    {
                        try
                        {
                            MessageQueue m = Form1.list_of_message_queues.Single(r => r.from == change.from);
                            if (m.signals.Peek() == change.signal)
                                Form1.list_of_possible_next_states.Add(change);
                        }
                        catch
                        {

                        }
                    }
                }
            }
            catch (Exception e) 
            {
                MessageBox.Show("blad" + e);
            }
        }
        //zmienia stan
        public static void change_state(object obj)
        {

            try
            {
                if (Form1.list_of_possible_next_states.Count != 0)
                {
                    Random rand = new Random(DateTime.Now.ToString().GetHashCode());
                    int index = rand.Next(0, Form1.list_of_possible_next_states.Count);
                    //wybór przejscia
                    StateChange change = Form1.state_present.list_of_state_changes.Single(r => r == Form1.list_of_possible_next_states[index]);
                    if (change.type == StateChange.Type.SEND)
                    {
                        Message m = create_message(true, Form1.message_counter, Form1.FSM_name, change.to, change.signal);
                        send_message(m);
                        Form1.message_counter++;
                        Form1.state_present = Form1.list_of_states.Single(r => r.state_name == change.next_state_string);
                    }
                    if (change.type == StateChange.Type.RECIEVE)
                    {
                        MessageQueue q = Form1.list_of_message_queues.Single(r => r.from == change.from);
                        q.signals.Dequeue();
                        Form1.state_present = Form1.list_of_states.Single(r => r.state_name == change.next_state_string);
                    }
                    if (change.type == StateChange.Type.TAU)
                    {
                        Form1.state_present = Form1.list_of_states.Single(r => r.state_name == change.next_state_string);
                    }
                }
                else {}
            }
            catch (Exception e)
            {
                MessageBox.Show("Błąd zmiany stanu: " + e);
            }
        
        }
        public static void change_state()
        {

            try
            {
                if (Form1.list_of_possible_next_states.Count != 0)
                {
                    Random rand = new Random(DateTime.Now.ToString().GetHashCode());

                    int index = rand.Next(0, Form1.list_of_possible_next_states.Count);
                    //wybór przejscia
                    StateChange change = Form1.state_present.list_of_state_changes.Single(r => r == Form1.list_of_possible_next_states[index]);
                    if (change.type == StateChange.Type.SEND)
                    {
                        Message m = create_message(true, Form1.message_counter, Form1.FSM_name, change.to, change.signal);
                        send_message(m);
                        Form1.message_counter++;
                        Form1.state_present = Form1.list_of_states.Single(r => r.state_name == change.next_state_string);
                    }
                    if (change.type == StateChange.Type.RECIEVE)
                    {
                        MessageQueue q = Form1.list_of_message_queues.Single(r => r.from == change.from);
                        q.signals.Dequeue();
                        Form1.state_present = Form1.list_of_states.Single(r => r.state_name == change.next_state_string);
                    }
                    if (change.type == StateChange.Type.TAU)
                    {
                        Form1.state_present = Form1.list_of_states.Single(r => r.state_name == change.next_state_string);
                    }

                }
                else { }
            }

            catch (Exception e)
            {
                MessageBox.Show("Błąd zmiany stanu: " + e);
            }
        }
        //tworzy wiadomosc do wyslania
        public static Message create_message(bool type_, int increment, string from_, string to_, string signal_)
        {
            var msg = new Message();

            msg.type_of_information = type_;
            msg.i = increment;
            msg.from = from_;
            msg.to = to_;
            msg.signal = signal_;

            return msg;
        }
        //zajmuje sie odebrana wiadomoscia
        public static void message_handler(Message msg)
        {
            bool flag_exist = false;
                foreach (MessageQueue msg_Queue in Form1.list_of_message_queues)
                {
                    if (msg.from == msg_Queue.from)
                    {
                        msg_Queue.signals.Enqueue(msg.signal);
                        flag_exist = true;
                    }
                }

                if (flag_exist == false)
                {
                    MessageQueue msgQueue = new MessageQueue();
                    msgQueue.from = msg.from;
                    msgQueue.signals.Enqueue(msg.signal);
                    Form1.list_of_message_queues.Add(msgQueue);
                }
                msg.type_of_information = false;
                send_message(msg);
        }
        //wysyla wiadomosc na broadcascie przez UDP 
        //zrobić!!!
        public static void send_message(Message msg)
        {
            Form1.server.Send(msg);
        }
    }
}
