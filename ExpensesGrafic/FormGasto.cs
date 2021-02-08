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
using Expenses.Utils;

namespace ExpensesGrafic
{
    public partial class FormGasto : Form
    {
        public FormGasto(Persona persona)
        {
            InitializeComponent();
            _selectedPersona = persona;
            label1.Text = $"{label1.Text} {persona.Nombre}" ;
            Load += new EventHandler((s, e) => RefreshDataSource());
        }

        private Persona _selectedPersona;
        private Gasto _selectedGasto = null;
        private void RefreshDataSource()
        {
            dataGridView1.DataSource = _selectedPersona.Gastos.Select(g=> new { g.Descripcion, g.Monto }).ToList();
        }

        private void ResetForm()
        {
            RefreshDataSource();
            textBox2.Clear();
            textBox1.Clear();
        }
        private void CuandoDtgvCambiaCeldaSeleccioneda(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                dataGridView1.ClearSelection();
            }
            else
            {
                _selectedGasto = _selectedPersona.Gastos
                    .Where(g => g.Descripcion == ((string)dataGridView1.CurrentRow.Cells["Descripcion"].Value)
                                && g.Monto == ((decimal)dataGridView1.CurrentRow.Cells["Monto"].Value))
                    .FirstOrDefault();
                textBox1.Text = _selectedGasto.Descripcion;
                textBox2.Text = _selectedGasto.Monto.ToString();
            }
        }
        private void Crear_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrEmpty(textBox2.Text))
            {
                _selectedPersona.CrearGasto(textBox1.Text, Util.DecimalPerfectParse(textBox2.Text, 0.0m));
                ResetForm();
            }
        }
        private void Eliminar_Click(object sender, EventArgs e)
        {

            if (_selectedGasto != null)
            {
                var dialogResult = MessageBox.Show("Seguro que desea eliminar este Gasto???", "Eliminar Gasto", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    _selectedPersona.EliminarGasto(_selectedGasto);
                }
            }
            ResetForm();
        }
    }
}
