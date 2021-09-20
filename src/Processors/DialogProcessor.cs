using Draw.src.Model;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Draw
{
	/// <summary>
	/// Класът, който ще бъде използван при управляване на диалога.
	/// </summary>
	public class DialogProcessor : DisplayProcessor
	{
		#region Constructor
		
		public DialogProcessor()
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Избран елемент.
		/// </summary>
		/// 
		float sizeScroll = 10;
		float angleV = 0f;
		private List<Shape> selection = new List<Shape>();
		public List<Shape> Selection
		{
			get { return selection; }
			set { selection = value; }
		}

		/// <summary>
		/// Дали в момента диалога е в състояние на "влачене" на избрания елемент.
		/// </summary>
		private bool isDragging;
		public bool IsDragging {
			get { return isDragging; }
			set { isDragging = value; }
		}
		
		/// <summary>
		/// Последна позиция на мишката при "влачене".
		/// Използва се за определяне на вектора на транслация.
		/// </summary>
		private PointF lastLocation;
		public PointF LastLocation {
			get { return lastLocation; }
			set { lastLocation = value; }
		}
		
		#endregion
		
		/// <summary>
		/// Добавя примитив - правоъгълник на произволно място върху клиентската област.
		/// </summary>
		public void AddRandomRectangle()
		{
			Random rnd = new Random();
			int x = rnd.Next(100,1000);
			int y = rnd.Next(100,600);
			
			RectangleShape rect = new RectangleShape(new Rectangle(x,y,100,200));
			rect.FillColor = Color.DarkGoldenrod;
            rect.BorderColor = Color.Crimson;
            rect.BorderWidth = 2.25f;
			ShapeList.Add(rect);
		}

        public void AddRandomEllipse()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);

            EllipseShape ell = new EllipseShape(new Rectangle(x, y, 100, 200));
            ell.FillColor = Color.Blue;
            ell.BorderColor = Color.DarkRed;
            ell.BorderWidth = 3.25f;
            ShapeList.Add(ell);
        }
		
        public void AddRandomTriange()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);

            TriangleShape tri = new TriangleShape(new Rectangle(x, y, 100, 200));
            tri.FillColor = Color.GreenYellow;
            tri.BorderColor = Color.Aquamarine;
            tri.BorderWidth = 4.25f;

            ShapeList.Add(tri);
        }
		public void AddRandomPoint()
		{
			Random rnd = new Random();
			int x = rnd.Next(100, 1000);
			int y = rnd.Next(100, 600);

			PointShape point = new PointShape(new Rectangle(x, y, 6, 6));
			point.FillColor = Color.Red;
			point.BorderColor = Color.Black;
			ShapeList.Add(point);
		}

		public void AddRandomLine()
		{
			Random rnd = new Random();
			int x = rnd.Next(100, 1000);
			int y = rnd.Next(100, 600);
			int length = rnd.Next(40, 500);

			LineShape line = new LineShape(new Rectangle(x, y, length, 1));

			line.FillColor = Color.Red;
			line.BorderColor = Color.Black;
			ShapeList.Add(line);
		}
		/// <summary>
		/// Проверява дали дадена точка е в елемента.
		/// Обхожда в ред обратен на визуализацията с цел намиране на
		/// "най-горния" елемент т.е. този който виждаме под мишката.
		/// </summary>
		/// <param name="point">Указана точка</param>
		/// <returns>Елемента на изображението, на който принадлежи дадената точка.</returns>
		public Shape ContainsPoint(PointF point)
		{
			for(int i = ShapeList.Count - 1; i >= 0; i--){
				if (ShapeList[i].Contains(point)){
					ShapeList[i].FillColor = Color.Red;
                    ShapeList[i].BorderColor = Color.Green;
                    ShapeList[i].BorderWidth = 3.75f;
					ShapeList[i].Angle = angleV;
					return ShapeList[i];
				}	
			}
			return null;
		}

        /// <summary>
        /// Транслация на избраният елемент на вектор определен от <paramref name="p>p</paramref>
        /// </summary>
        /// <param name="p">Вектор на транслация.</param>
        /// 

        public void TranslateTo(PointF p)
		{
			if (selection != null) {
				foreach (var item in selection)
				{
					item.Location = new PointF(item.Location.X + p.X - lastLocation.X, item.Location.Y + p.Y - lastLocation.Y);
					
				}
				lastLocation = p;
			}
		}

        public void Rotate(float angle, string btn = " ")
        {
			//Selection.Angle = angle;
			//Shape s = Selection;
			//ShapeList.Remove(Selection);
			//ShapeList.Add(s);
			angleV = angle;



		}

		public void CreateGroupShape()
		{
			if (Selection.Count < 2) return;

			float minX = float.PositiveInfinity;
			float minY = float.PositiveInfinity;
			float maxX = float.NegativeInfinity;
			float maxY = float.NegativeInfinity;

			foreach (var item in Selection)
			{
				if (minX > item.Location.X) minX = item.Location.X;
				if (minY > item.Location.Y) minY = item.Location.Y;
				if (maxX < item.Location.X + item.Width) maxX = item.Location.X + item.Width;
				if (maxY < item.Location.Y + item.Height) maxY = item.Location.Y + item.Height;
			}

			var group = new GroupShape(new RectangleF(minX, minY, maxX - minX, maxY - minY));
			group.GroupItems = Selection;

			Selection = new List<Shape>();
			Selection.Add(group);

			foreach (var item in group.GroupItems)
			{
				ShapeList.Remove(item);
			}
			ShapeList.Add(group);
		}

		internal void SetSelectedFieldColor(Color color)
		{
			foreach (var item in Selection)
			{

				item.FillColor = color;

			}
		}

		public void SetOpacity(int opacity)
		{
			foreach (var item in Selection)
			{
				item.FillColor = Color.FromArgb(opacity, item.FillColor);
				item.BorderColor = Color.FromArgb(opacity, item.BorderColor);
			}
		}
		public void DeleteShape()
		{
			foreach (var item in Selection)
			{
				ShapeList.Remove(item);
			}

			Selection.Clear();
		}

		public void SetShapeSize(float size)
		{

			float changeValue = size - sizeScroll;
			sizeScroll = size;
			changeValue = changeValue * 5;
			foreach (var item in Selection)
			{
				if (item.GetType().ToString() == "Draw.src.Model.LineShape")
				{
					item.Width += changeValue;
				}
				else if (item.GetType().ToString() == "Draw.src.Model.GroupShape")
				{
					foreach (var subItem in item.GroupItems)
					{
						if (subItem.GetType().ToString() == "Draw.src.Model.LineShape")
						{
							subItem.Width += changeValue;
						}
						else if (subItem.GetType().ToString() != "Draw.src.Model.PointShape")
						{
							subItem.Width += changeValue;
							subItem.Height += changeValue;
						}
						else
						{
							continue;
						}
					}

					item.Width += changeValue;
					item.Height += changeValue;
				}
				else
				{
					item.Width += changeValue;
					item.Height += changeValue;
				}
			}
		}

	}

}
