using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace genetico
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        bool tipoAlgoritmo = true;
        string resOptima = "",
            pesos = "", 
            ganancias = "";
        int elementos_en_mochila = 10;


        private void btnCalcular_Click(object sender, EventArgs e)
        {
            String poblacion_s, //numero introducido por el usuario
                   probabilidad_cruce, //numero introducido por el usuario
                   probabilidad_mutacion, //numero introducido por el usuario
                   gen; //numero introducido por el usuario

            double numero_decimal = 0.0, //variable para comparar con digito decimal
                   probabilidad_c, //variable para almacenar la probabilidad de cruce
                   probabilidad_m; //variable para almacenar la probabilidad de mutacion

            int numero_entero = 0, //variable para compara con digito entero
                    poblacion_i, //numero de la poblacion en numero entero
                    generaciones, //numero de generaciones
                    con_gen = 0; //contador de generaciones


            //Validaciones
            if ((tbxCruce.Text != "" && tbxGeneraciones.Text != "") && tbxPoblacion.Text != "")
            {
                poblacion_s = tbxPoblacion.Text;//Console.ReadLine();
                poblacion_i = Int32.Parse(poblacion_s); //convertir String a int base 32
                if(Int32.TryParse(poblacion_s, out numero_entero) && poblacion_i % 2 == 0) //validar población
                {
                    probabilidad_cruce = tbxCruce.Text;
                    probabilidad_c = Double.Parse(probabilidad_cruce);
                    if(Double.TryParse(probabilidad_cruce, out numero_decimal) && (probabilidad_c <= 0.80 && probabilidad_c >= 0.65)) //validar cruce
                    {
                        probabilidad_mutacion = tbxMutacion.Text;
                        probabilidad_m = Double.Parse(probabilidad_mutacion);
                        if (Double.TryParse(probabilidad_mutacion, out numero_decimal) && (probabilidad_m <= 0.01 && probabilidad_m >= 0.001)) //validar cruce
                        {
                            gen = tbxGeneraciones.Text;
                            generaciones = Int32.Parse(gen);
                            if (Int32.TryParse(gen, out numero_entero) && (generaciones > 0)) //validar las generaciones
                            {
                                Operaciones accion = new Operaciones();

                                if(tipoAlgoritmo)//Genetico
                                {
                                    accion.iniciar_poblacion(poblacion_i); //generar poblacion

                                    do
                                    {
                                        con_gen++;

                                        accion.calcular_inicio();

                                        accion.cal_final();

                                        accion.generarLosNuevosPadresDeLaPatria(poblacion_i);

                                        accion.cruze(probabilidad_c, poblacion_i);

                                        accion.mute(probabilidad_m);

                                    } while (con_gen <= generaciones);

                                    lblX.Text = "";

                                    string aux = accion.imprimir_ganadores();

                                    if (aux != "")
                                    {
                                        lblX.Text = "x = " + aux;
                                    }
                                    else
                                    {
                                        MessageBox.Show("No hay resultados optimos en todas las generaciones");
                                    }
                                } 
                                else // DE LA MOCHILA
                                {
                                    accion.iniciar_poblacion(poblacion_i, elementos_en_mochila , resOptima, ganancias, pesos); //generar poblacion
                                }
                                
                            }
                            else
                            {
                                MessageBox.Show("Se debe introducir un numero entero positivo");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Se debe introducir un numero entre esos rangos");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Se debe introducir un numero entre esos rangos");   
                    }
                }
                else
                {
                    MessageBox.Show("El tamaño debe ser un número entero y par");
                }   
            }
            else
            {
                MessageBox.Show("Favor de llenar todos los campos con la información necesaria");
            }
        } 

        private void btnSalir_Click(object sender, EventArgs e)
        {
            DialogResult mensaje = MessageBox.Show("¿Está seguro de salir del programa?", "Salir", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (mensaje == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void btnGenetico_Click(object sender, EventArgs e)
        {
            gbxMochila.Visible = false;
            lblFunción.Visible = true;
            btnTitulo.Text = "ALGORITMO GENÉTICO PURO";
            tipoAlgoritmo = true;
        }

        private void btnMochila_Click(object sender, EventArgs e)
        {
            gbxMochila.Visible = true;
            lblFunción.Visible = false;
            btnTitulo.Text = "PROBLEMA DE LA MOCHILA";
            tipoAlgoritmo = false;
        }

        private void btnResOpt_Click(object sender, EventArgs e)
        {
            if(ofdAbrirArchivo.ShowDialog() == DialogResult.OK)
            {
                using(StreamReader sr = new StreamReader(ofdAbrirArchivo.FileName))
                {
                    resOptima = sr.ReadToEnd();
                }
            }
        }

        private void btnPesos_Click(object sender, EventArgs e)
        {
            if (ofdAbrirArchivo.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader sr = new StreamReader(ofdAbrirArchivo.FileName))
                {
                    label3.Text = sr.ReadToEnd();
                     
                }
            }
        }

        private void btnGanancias_Click(object sender, EventArgs e)
        {
            if (ofdAbrirArchivo.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader sr = new StreamReader(ofdAbrirArchivo.FileName))
                {
                    ganancias = sr.ReadToEnd();
                }
            }
        }
    }
}
