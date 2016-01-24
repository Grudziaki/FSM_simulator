using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSM_Simulator
{
    //opis pojedynczego stanu
    public class State
    {
        public string state_name;
        public List<StateChange> list_of_state_changes = new List<StateChange>();
    }
    //opis przejscia i jego parametrow
    public class StateChange
        {
            public enum Type { SEND, RECIEVE, TAU };

            public State next_state;
            public Type type;
            public string from; // dla RECIEVE
            public string to; // dla SEND     
            public string signal; // dla RECIEVE sygnal otrzymany, dla SEND do wyslania
        }
    //kolejka od konkretnego automatu
    public class MessageQueue
        {
            public string from;
            public Queue<string> signals = new Queue<string>();
        }
    //wiadomosc przesylana miedzy automatami
    public class Message
        {
            public bool type_of_information; // true - zawiera sygnal, false - informacja dla innych aplikacji
            public string from;
            public string to;
            public string signal;
        }
}
