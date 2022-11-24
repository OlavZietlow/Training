using System.Web;
using System.Xml;
using System.Diagnostics.CodeAnalysis;

namespace Taschenrechner
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        decimal zahl1,zahl2,zahl3;
        decimal ergebnis;
        char opera,nextopera;


        private decimal ZahlRausziehen(ref string eingabeRest)
        {
            string zahlAlsString = "";
            int cut=0;
            {
                for (int i = 0; i < eingabeRest.Length; i++)
                    if (eingabeRest[i] != '+'
                        && eingabeRest[i] != '-'
                        && eingabeRest[i] != '*'
                        && eingabeRest[i] != '/')
                    {
                        zahlAlsString += eingabeRest[i].ToString();
                        cut = i;
                    }
                    else break;
                eingabeRest = eingabeRest.Remove(0, cut+1);
            }
            return StringToDecimal(zahlAlsString);
        }
        private decimal StringToDecimal(string zahlAlsString)
        {
            decimal zahl =0;
            try
            {
                zahl = decimal.Parse(zahlAlsString);  // Nicht immer Convert.To...() benutzen
            }
            catch (Exception e)
            {
                l_Exception.Visible = true;
                t_Exception.Visible = true;
                t_Exception.Text = e.Message;
            }
            return zahl;
        }
        private char OperatorRausziehen(ref string rechenterm)
        {
            char opera = rechenterm[0];
            rechenterm = rechenterm.Remove(0, 1);
            return opera;
        }
        private static bool HatOperator(string term)
        {
            if (term.Contains('+') || term.Contains('-') || term.Contains('*') || term.Contains('/'))
                return true;
            else
                return false;
        }
      
        private decimal DieRechnung(decimal ersteZahl,char opera, decimal zweiteZahl)
        {
            switch (opera)
            {
                case '+':
                    ergebnis = ersteZahl+zweiteZahl;
                    break;
                case '-':
                    ergebnis = ersteZahl- zweiteZahl;
                    break;
                case '*':
                    ergebnis = ersteZahl* zweiteZahl;
                    break;
                case '/':
                    try
                    {
                        ergebnis = ersteZahl/ zweiteZahl;
                    }
                    catch (Exception exc)
                    {
                        ergebnis = 0;
                        t_Exception.Visible = true;
                        l_Exception.Visible = true;
                        t_Exception.Text = exc.Message;
                    }
                    break;
            }
            return ergebnis;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            t_Exception.Visible = false;
            l_Exception.Visible = false;
            BackColor = Color.Linen;
        }
        private void Bt_gleich_Click(object sender, EventArgs e)
        {
            string rechenterm = tb_Rechenschritt.Text;
            if(!HatOperator(rechenterm))
            {
                l_Exception.Visible = true;
                t_Exception.Visible = true;
                t_Exception.Text = "Eine Gleichung ist das nicht.";
                return;     //Abbruch, weil kein Operator angegeben wurde
            }
            else
            {
                zahl1 = ZahlRausziehen(ref rechenterm);     // rechenterm wurde die erste Zahl vorne abgeschnitten
                opera = OperatorRausziehen(ref rechenterm); // opera, denn "Operator" ist ein keyword
                zahl2 = ZahlRausziehen(ref rechenterm);
                while (HatOperator(rechenterm))             // es hat MEHR als einen Operator
                {
                    nextopera = OperatorRausziehen(ref rechenterm);
                    zahl3 = ZahlRausziehen(ref rechenterm);
                    if (opera == '*' || opera == '/')
                    {
                        zahl1 = DieRechnung(zahl1, opera, zahl2);
                        zahl2 = zahl3;
                        opera = nextopera;
                    }
                    else        // also opera in {'+','-'}
                    {
                        if (nextopera == '+' || nextopera == '-')
                        {
                            zahl1 = DieRechnung(zahl1, opera, zahl2);
                            zahl2 = zahl3;
                            opera = nextopera;
                        }
                        else
                        {
                            zahl2 = DieRechnung(zahl2, nextopera, zahl3);
                        }
                    }
                }
                ergebnis = DieRechnung(zahl1, opera, zahl2);
            }
            tb_zahl1.Text = zahl1.ToString();
            tb_zahl2.Text = zahl2.ToString();
            tb_op.Text = opera.ToString();
            tb_Ergebnis.Text = ergebnis.ToString();
            //tb_Rechenschritt.Clear();
        }
        private void Bt_Zahl_Click(object sender, EventArgs e)
        {
            tb_Rechenschritt.Text += (sender as Button)!.Text;
            l_Exception.Visible = false;
            t_Exception.Visible = false;
        }
        private void bt_CLEAR_Char_Click(object sender, EventArgs e)
        {
            tb_Rechenschritt.Text = tb_Rechenschritt.Text.Remove(tb_Rechenschritt.Text.Length-1,1);
        }

        private void Bt_CLEAR_Click(object sender, EventArgs e)
        {
            tb_zahl1.Clear();
            tb_zahl2.Clear();
            tb_Ergebnis.Clear();
            tb_Rechenschritt.Clear();
            tb_op.Clear();
            t_Exception.Clear();
            l_Exception.Visible = false;
            t_Exception.Visible = false;
        }
    }
}