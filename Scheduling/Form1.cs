using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
namespace Scheduling
{
    public partial class Form1 : Form
    {
        CompoundResult _Result = new CompoundResult();
        EScheduling eScheduling = EScheduling.FCFS;
        ShProcess_Col shProcess_Col;

        public Form1()
        {
            InitializeComponent();
        }

        private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
        }

        private void ResetNames()
        {
            
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1[0, i].Value = $"{(char)(65 + i)}";
            }
            CheckDataInput();
        }

        private void AddRow_btn_Click(object sender, System.EventArgs e)
        {
            if (dataGridView1.Rows.Count >= 26) return;
            dataGridView1.Rows.Add();
            ResetNames();

        }

        private void dataGridView1_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            ResetNames();
        }

        private bool CheckDataInput()
        {
            bool flag = true;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                for (int y = 1; y < dataGridView1.ColumnCount-2; y++)
                {
                    if (dataGridView1[y, i].Value != null)
                    {
                        if (!Regex.IsMatch(dataGridView1[y, i].Value.ToString(), @"^\d+$"))
                        {
                            dataGridView1[y, i].Style.ForeColor = System.Drawing.Color.Red;
                            dataGridView1[y, i].Style.BackColor = System.Drawing.Color.WhiteSmoke;
                            flag = false;
                        }
                        else
                        {
                            dataGridView1[y, i].Style.ForeColor = System.Drawing.Color.Green;
                            dataGridView1[y, i].Style.BackColor = System.Drawing.Color.White;
                        }
                    }
                }
            }
            return flag;
        }

        private void SolveTable(ref CompoundResult _Result)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                var proc = _Result.cmpd_shProcesses.shProcesses.ToList().Find(p => p.info.Name == dataGridView1[0, i].Value.ToString());
                dataGridView1[4, i].Value = proc.result.Waitingtime;//response Time
                dataGridView1[5, i].Value = proc.result.TurnaroundTime;
            }
            RPavg.Text = _Result.cmpd_shProcesses.Average_WaitingTime.ToString();
            TaTavg.Text = _Result.cmpd_shProcesses.Average_TurnArounTime.ToString();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            if (dataGridView1.Rows.Count <= 0) return;
            List<ShProcess> _procList = new List<ShProcess>();
            try{
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    Info info = new Info($"{dataGridView1[0, i].Value}",
                        int.Parse(dataGridView1[1, i].Value.ToString()),
                        int.Parse(dataGridView1[2, i].Value.ToString()),
                        int.Parse(dataGridView1[3, i].Value.ToString()));
                    _procList.Add(new ShProcess(info, new Result(), false));
                }
            }
            catch
            {
                MessageBox.Show("Please input Valid Data Values");
            }
            

            shProcess_Col.shProcesses = _procList.ToArray();


            if (CheckDataInput())
            {
               
                
                    switch (eScheduling)
                    {
                        case EScheduling.FCFS:
                            _Result = shProcess_Col.S_FCFS();
                        break;
                        case EScheduling.SJF:
                            _Result = shProcess_Col.S_SJF();
                        break;
                        case EScheduling.SRT:
                            _Result = shProcess_Col.S_SRTF();
                        break;
                        case EScheduling.Priority_P:
                            _Result = shProcess_Col.S_Ppriority();
                        break;
                        case EScheduling.Priority_NP:
                            _Result = shProcess_Col.S_Priority();
                        break;
                        case EScheduling.RR:
                            _Result = shProcess_Col.S_RR(int.Parse(textBox1.Text));
                        break;
                    }
                SolveTable(ref _Result);
                panel1.Refresh();
            }

        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }

        private void button3_Click(object sender, System.EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1[0, i].Value = $"{(char)(65+i)}";
                dataGridView1[1, i].Value = "~";
                dataGridView1[2, i].Value = "~";
                dataGridView1[3, i].Value = "~";
                dataGridView1[4, i].Value = "~";
                dataGridView1[5, i].Value = "~";
            }
            CheckDataInput();
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            CheckDataInput();
        }

        private void comboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            eScheduling = (EScheduling)comboBox1.SelectedIndex;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            int w = 35, h = 29;
            var _graphics = e.Graphics;
            Brush x = new SolidBrush(Color.White);
            Brush z = new SolidBrush(Color.Yellow);

            try
            {
                for (int i = 0; i < _Result.cmpd_GetGraphicsData.graphicsDatas.Count(); i++)
                {
                    Thread.Sleep(10);
                    _graphics.FillRectangle(new SolidBrush(Color.LightGray), new Rectangle((w * i) + 5, (panel1.Height / 3), w, h));
                    _graphics.DrawRectangle(new Pen(Color.Gray), new Rectangle((w * i) + 5, (panel1.Height / 3), w, h));
                    _graphics.DrawLine(new Pen(Color.Black), (w * i) + 5, (panel1.Height / 3) + h, (w * i) + 5, (panel1.Height / 3) + h + 10);//down
                    _graphics.DrawLine(new Pen(Color.Black), (w * i) + w + 5, (panel1.Height / 3) + h, (w * i) + 5 + w, (panel1.Height / 3) + h + 10);//down
                    _graphics.DrawString($"{_Result.cmpd_GetGraphicsData.graphicsDatas[i].Name}", new Font(FontFamily.GenericSerif, 14), new SolidBrush(Color.Black), new Point((w * i) + 10, (panel1.Height / 3) + 5));
                    _graphics.DrawString($"{_Result.cmpd_GetGraphicsData.graphicsDatas[i].Start}", new Font(FontFamily.GenericSerif, 8), x, new Point((w * i) + 5, (panel1.Height / 3) + h));
                    _graphics.DrawString($"{_Result.cmpd_GetGraphicsData.graphicsDatas[i].End}", new Font(FontFamily.GenericSerif, 8), z, new Point((w * i) + w - 10, (panel1.Height / 3) + h));
                }
            }
            catch
            {
                
            }
            
        }

        private void button4_Click(object sender, System.EventArgs e)
        {
            Application.Exit();
        }

        public Point mouseLocation;
        private void Frm_MouseDown(object sender, MouseEventArgs e)
        {
            mouseLocation = new Point(-e.X, -e.Y);
        }

        private void Frm_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePos = Control.MousePosition;
                mousePos.Offset(mouseLocation.X, mouseLocation.Y);
                Location = mousePos;
            }
        }

        private void button5_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show("LQD");
        }
    }
}
