using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Draw
{
	/// <summary>
	/// Върху главната форма е поставен потребителски контрол,
	/// в който се осъществява визуализацията
	/// </summary>
	public partial class MainForm : Form
	{
		/// <summary>
		/// Агрегирания диалогов процесор във формата улеснява манипулацията на модела.
		/// </summary>
		private DialogProcessor dialogProcessor = new DialogProcessor();
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}

		/// <summary>
		/// Изход от програмата. Затваря главната форма, а с това и програмата.
		/// </summary>
		void ExitToolStripMenuItemClick(object sender, EventArgs e)
		{
			Close();
		}
		
		/// <summary>
		/// Събитието, което се прихваща, за да се превизуализира при изменение на модела.
		/// </summary>
		void ViewPortPaint(object sender, PaintEventArgs e)
		{
			dialogProcessor.ReDraw(sender, e);
		}
		
		/// <summary>
		/// Бутон, който поставя на произволно място правоъгълник със зададените размери.
		/// Променя се лентата със състоянието и се инвалидира контрола, в който визуализираме.
		/// </summary>
		void DrawRectangleSpeedButtonClick(object sender, EventArgs e)
		{
			dialogProcessor.AddRandomRectangle();
			
			statusBar.Items[0].Text = "Последно действие: Рисуване на правоъгълник";
			
			viewPort.Invalidate();
		}

		/// <summary>
		/// Прихващане на координатите при натискането на бутон на мишката и проверка (в обратен ред) дали не е
		/// щракнато върху елемент. Ако е така то той се отбелязва като селектиран и започва процес на "влачене".
		/// Промяна на статуса и инвалидиране на контрола, в който визуализираме.
		/// Реализацията се диалогът с потребителя, при който се избира "най-горния" елемент от екрана.
		/// </summary>
		void ViewPortMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (pickUpSpeedButton.Checked)
			{
				var sel = dialogProcessor.ContainsPoint(e.Location);
				if (sel == null)
				{
					return;
				}
				if (dialogProcessor.Selection.Contains(sel))
				{
					dialogProcessor.Selection.Remove(sel);
				}
				else
				{
					dialogProcessor.Selection.Add(sel);
				}

				//if (dialogProcessor.Selection != null) {
				statusBar.Items[0].Text = "Последно действие: Селекция на примитив";
				dialogProcessor.IsDragging = true;
				dialogProcessor.LastLocation = e.Location;
				viewPort.Invalidate();
				//}

			}
		}

		/// <summary>
		/// Прихващане на преместването на мишката.
		/// Ако сме в режм на "влачене", то избрания елемент се транслира.
		/// </summary>
		void ViewPortMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (dialogProcessor.IsDragging) {
				if (dialogProcessor.Selection != null) statusBar.Items[0].Text = "Последно действие: Влачене";
				dialogProcessor.TranslateTo(e.Location);
				viewPort.Invalidate();
			}
		}

		/// <summary>
		/// Прихващане на отпускането на бутона на мишката.
		/// Излизаме от режим "влачене".
		/// </summary>
		void ViewPortMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			dialogProcessor.IsDragging = false;
		}

        private void SpeedMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void DrawEllipseButton_Click(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomEllipse();

            statusBar.Items[0].Text = "Последно действие: Рисуване на Елипса";

            viewPort.Invalidate();
        }

        private void DrawTriangeButton_Click(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomTriange();

            statusBar.Items[0].Text = "Последно действие: Рисуване на Триъгълник";

            viewPort.Invalidate();
        }

        private void RotateTrackBar_Scroll(object sender, EventArgs e)
        {
            dialogProcessor.Rotate((float)RotateTrackBar.Value);
            statusBar.Items[0].Text = "Последно действие: Завъртане";
            viewPort.Invalidate();
        }

		private void groupShapeBtn_Click(object sender, EventArgs e)
		{
			dialogProcessor.CreateGroupShape();
			statusBar.Items[0].Text = "Последно действие: Групиране на примитиви";
			viewPort.Invalidate();
		}

		private void SizeTrackBar_Scroll(object sender, EventArgs e)
		{
			dialogProcessor.SetShapeSize((int)SizeTrackBar.Value);
			statusBar.Items[0].Text = "Последно действие: Увеличаване размера на избраните примитиви";
			viewPort.Invalidate();
		}



		private void drawPointBtn_Click(object sender, EventArgs e)
		{
			dialogProcessor.AddRandomPoint();

			statusBar.Items[0].Text = "Последно действие: Рисуване на Точка";

			viewPort.Invalidate();
		}

		private void drawLineBtn_Click_1(object sender, EventArgs e)
		{
			dialogProcessor.AddRandomLine();

			statusBar.Items[0].Text = "Последно действие: Рисуване на Линия";

			viewPort.Invalidate();
		}

		private void colorBtn_Click(object sender, EventArgs e)
		{
			if (colorDialog1.ShowDialog() == DialogResult.OK)
			{
				dialogProcessor.SetSelectedFieldColor(colorDialog1.Color);
				viewPort.Invalidate();
			}
		}

		private void SetOpacitytrackBar_Scroll(object sender, EventArgs e)
		{
			dialogProcessor.SetOpacity(SetOpacitytrackBar.Value);
			statusBar.Items[0].Text = "Последно действие: Смяна на Прозрачност";

			viewPort.Invalidate();
		}
		byte[] bytes;
		private void copyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			BinaryFormatter formatter = new BinaryFormatter();
			MemoryStream stream = new MemoryStream();
			formatter.Serialize(stream, dialogProcessor.Selection);
			bytes = stream.ToArray();
		}

		private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			BinaryFormatter formatter = new BinaryFormatter();
			MemoryStream stream = new MemoryStream();
			stream.Write(bytes, 0, bytes.Length);
			stream.Position = 0;
			List<Shape> list = formatter.Deserialize(stream) as List<Shape>;

			float offSetX = Cursor.Position.X - list.First().Location.X;
			float offSetY = Cursor.Position.Y - list.First().Location.Y;

			foreach (var item in list)
			{
				item.Location = new PointF(item.Location.X + offSetX, item.Location.Y + offSetY);
				dialogProcessor.ShapeList.Add(item);
			}

			dialogProcessor.Selection.Clear();
			this.Refresh();
		}

		private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			dialogProcessor.DeleteShape();
			statusBar.Items[0].Text = "Последно действие: Изтриване на примитив(и)";
			viewPort.Invalidate();
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			saveFileDialog1.ShowDialog();
		}

		private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(this.saveFileDialog1.FileName, FileMode.Create, FileAccess.Write, FileShare.None);
			formatter.Serialize(stream, dialogProcessor.ShapeList); //serialize path of the file and list of all shapes
			stream.Close();
			statusBar.Items[0].Text = "Последно действие: Запазване на файл.";
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			openFileDialog1.ShowDialog();
		}

		private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(this.openFileDialog1.FileName, FileMode.Open, FileAccess.Read, FileShare.None);
			dialogProcessor.ShapeList = (List<Shape>)formatter.Deserialize(stream);
			stream.Close();
			dialogProcessor.Selection.Clear();
			saveFileDialog1.FileName = openFileDialog1.FileName;
			statusBar.Items[0].Text = "Последно действие: Отваряне на файл.";
			this.Refresh();
		}
	}
}
