using Expenses.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExpensesGrafic
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            _personaData = new PersonaData();
        }
        public List<Persona> Personas { get; set; }
        private PersonaData _personaData;
        private void Form1_Load(object sender, EventArgs e)
        {
            _personaData.loadPersonas();
            Personas = _personaData.GetAllPersonas();
            dataGridViewNueva.DataSource = Personas;
            var primerPersona = Personas.FirstOrDefault();
        }

        private void EjecutaCadaQueCambieTexto(object textbox, EventArgs paramEvento)
        {
            dataGridViewNueva.DataSource = Personas.Where(p=> p.Nombre.ToLower().Contains(textBox1.Text)).ToList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox2.Text))
            {
                _personaData.NuevaPersona(textBox2.Text);
                _personaData.loadPersonas();
                Personas = _personaData.GetAllPersonas();
                dataGridViewNueva.DataSource = Personas;
                textBox2.Clear();

            }
        }
    }
}
