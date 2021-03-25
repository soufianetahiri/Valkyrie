﻿using System;
using System.IO;

namespace Valkyrie.Helper
{
    /// <summary>
    /// Stream reader for StandardOutput and StandardError stream readers
    /// Runs an eternal BeginRead loop on the underlaying stream bypassing the stream reader.
    /// 
    /// The TextReceived sends data received on the stream in non delimited chunks. Event subscriber can
    /// then split on newline characters etc as desired.
    /// </summary>
    public class AsyncStreamReader
    {

        public delegate void EventHandler<args>(object sender, string Data);
        public event EventHandler<string> DataReceived;

        protected readonly byte[] buffer = new byte[4096];
        private StreamReader reader;


        /// <summary>
        ///  If AsyncStreamReader is active
        /// </summary>
        public bool Active { get; private set; }


        public void Start()
        {
            if (!Active)
            {
                Active = true;
                BeginReadAsync();
            }
        }


        public void Stop()
        {
            Active = false;
        }


        public AsyncStreamReader(StreamReader readerToBypass)
        {
            this.reader = readerToBypass;
            this.Active = false;
        }


        protected void BeginReadAsync()
        {
            if (this.Active)
            {
                reader.BaseStream.BeginRead(this.buffer, 0, this.buffer.Length, new AsyncCallback(ReadCallback), null);
            }
        }

        private void ReadCallback(IAsyncResult asyncResult)
        {
            var bytesRead = reader.BaseStream.EndRead(asyncResult);

            string data = null;

            //Terminate async processing if callback has no bytes
            if (bytesRead > 0)
            {
                data = reader.CurrentEncoding.GetString(this.buffer, 0, bytesRead);
            }
            else
            {
                //callback without data - stop async
                this.Active = false;
            }

            //Send data to event subscriber - null if no longer active
            if (this.DataReceived != null)
            {
                this.DataReceived.Invoke(this, data);
            }

            //Wait for more data from stream
            this.BeginReadAsync();
        }


    }
}
