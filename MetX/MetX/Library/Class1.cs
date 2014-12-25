using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MetX.Library
{
    public class Class1
    {
        public TextBox textBlox_George;

        private int _mary;

        public void Bad()
        {
            if (X() == 0)
            {
                var z = 22;
            }

            if (X() == 0 || Y() == 1) { _fred = "something bad"; }
        }

        public int X()
        {
            return 0;
        }

        public int Y()
        {
            return 1;
        }

        private string _fred;

        public string Fred
        {
            get { return _fred; }
        }

        public Class1(int mary)
        {
            _mary = mary;
            _fred = "George";
        }
    }
}
