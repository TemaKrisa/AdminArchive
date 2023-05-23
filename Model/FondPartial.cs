using System.Windows;

namespace AdminArchive.Model
{
    partial class Fond
    {
        public string FullNumber
        {
            get
            {
                return Index + "" + Number + "" + Literal;
            }
        }      
    }
}
