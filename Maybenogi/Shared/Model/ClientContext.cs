using System;
using System.Collections.Generic;
using System.Text;

namespace Maybenogi.Shared.Model
{
    public class ClientContext
    {
        public string ProcessName { get; set; }
        public int ProcessId { get; set; }
        
        public long AccountId { get; set; }
        public string AccountName { get; set; }
        public EClientState ClientState { get; set; }


        public bool IsValid
        {
            get => ProcessId != 0;
        }
    }

    public enum EClientState
    {
        None,
        Main,
        Sub,
    }
}
