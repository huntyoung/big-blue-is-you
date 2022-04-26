using System;
using System.Collections.Generic;
using System.Text;

namespace Components
{
    public class Text : Component
    {
        public string word;
        public List<string> partOfRules = new List<string>();

        public Text(string word)
        {
            this.word = word;
        }
    }
}
