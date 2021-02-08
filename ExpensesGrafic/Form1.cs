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
        private Persona _selectedPersona = null;

        private void RefreshDataSource()
        {
            _personaData.loadPersonas();
            Personas = _personaData.GetAllPersonas();
            dataGridViewNueva.DataSource = Personas;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshDataSource();
            var primerPersona = Personas.FirstOrDefault();
        }

        private void ButonGastoClick(object sender, EventArgs e)
        {
            if (_selectedPersona != null)
            {
                new FormGasto(_selectedPersona).ShowDialog();
                ResetForm();
            }
            
        }

        private void EjecutaCadaQueCambieTexto(object textbox, EventArgs paramEvento)
        {
            dataGridViewNueva.DataSource = Personas.Where(p=> p.Nombre.ToLower().Contains(textBox1.Text)).ToList();
        }

        private void CuandoDtgvCambiaCeldaSeleccioneda(object sender, EventArgs e)
        {
            if (dataGridViewNueva.SelectedRows.Count == 0)
            {
                _selectedPersona = null;
                textBox2.Clear();
                dataGridViewNueva.ClearSelection();
            }
            else
            {
                _selectedPersona = Personas
                    .Where(p=> p.NumeroPersona == ((int)dataGridViewNueva.CurrentRow.Cells["NumeroPersona"].Value))
                    .FirstOrDefault();
                textBox2.Text = _selectedPersona.Nombre;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox2.Text))
            {
                if (_selectedPersona != null)
                {
                    _personaData.ActualizarPersona(_selectedPersona, textBox2.Text);
                }
                else
                {
                    _personaData.NuevaPersona(textBox2.Text);
                }
                ResetForm();
            }
        }
        private void ResetForm()
        {
            RefreshDataSource();
            textBox2.Clear();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            
            if (_selectedPersona != null)
            {
                var dialogResult = MessageBox.Show("Seguro que desea eliminar esta persona???", "Eliminar Persona", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    _personaData.EliminarPersona(_selectedPersona);
                }
            }
            ResetForm();
        }
    }
}
