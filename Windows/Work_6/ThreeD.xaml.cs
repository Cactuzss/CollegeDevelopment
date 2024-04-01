using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace practice.Windows.Work_6
{
    /// <summary>
    /// Логика взаимодействия для ThreeD.xaml
    /// </summary>
    public partial class ThreeD : Window
    {
        private GeometryModel3D mGeometry;
        private bool mDown;
        private Point mLastPos;

        public ThreeD()
        {
            InitializeComponent();
            //BuildCube();

            MeshGeometry3D mesh = ObjParse("D:\\ISP-33\\Хрусталёв\\Practice\\practice\\Resources\\forg.obj");
            mGeometry = new GeometryModel3D(mesh, new DiffuseMaterial(Brushes.Green));
            mGeometry.Transform = new Transform3DGroup();

            group.Children.Add(mGeometry);

            mLastPos = new Point(0, 0);
        }


        private void BuildTriangle()
        {
            MeshGeometry3D mesh = new MeshGeometry3D();

            mesh.Positions.Add(new Point3D(-1, -1, 1));
            mesh.Positions.Add(new Point3D(1, -1, 1));
            mesh.Positions.Add(new Point3D(1, 1, 1));

            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(2);

            mGeometry = new GeometryModel3D(mesh, new DiffuseMaterial(Brushes.Green));
            mGeometry.Transform = new Transform3DGroup();

            group.Children.Add(mGeometry);
        }
        public static MeshGeometry3D ObjParse(string path) //, TextBlock info = null)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            MeshGeometry3D mesh
            = new MeshGeometry3D();
            int v = 0, f = 0;
            var vn = new List<Vector3D>();
            var vt = new List<System.Windows.Point>();
            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (string.IsNullOrEmpty(line)) continue; var l = line.Split(' ');
                    switch (l[0])
                    {
                        case "v":
                            mesh.Positions.Add(new Point3D
                                (double.Parse(l[1].Replace('.', ',')),
                                 double.Parse(l[2].Replace('.', ',')),
                                 double.Parse(l[3].Replace('.', ','))));
                            ++v; break;

                        case "f":
                            string[][] n = { l[1].Split('/'), l[2].Split('/'), l[3].Split('/') };

                            mesh.TriangleIndices.Add(int.Parse(n[0][0].ToString()) - 1); 
                            mesh.TriangleIndices.Add(int.Parse(n[1][0].ToString()) - 1); 
                            mesh.TriangleIndices.Add(int.Parse(n[2][0].ToString()) - 1); 

                            ++f; break;

                        default: continue;
                    }
                }
            }

            stopwatch.Stop();
            //if (info != null) info.Text = $"Вершин: {v}\nПовержностей: {f}\nмпортировано за {stopwatch.ElapsedMilliseconds} MC";
            return mesh;
        }
        private void BuildCube()
        {
            MeshGeometry3D mesh = new MeshGeometry3D();

            mesh.Positions.Add(new Point3D(-1, -1, 1));
            mesh.Positions.Add(new Point3D(1, -1, 1));
            mesh.Positions.Add(new Point3D(1, 1, 1));
            mesh.Positions.Add(new Point3D(-1, 1, 1));
            mesh.Positions.Add(new Point3D(-1, -1, -1));
            mesh.Positions.Add(new Point3D(1, -1, -1));
            mesh.Positions.Add(new Point3D(1, 1, -1));
            mesh.Positions.Add(new Point3D(-1, 1, -1));

            void addmesh(MeshGeometry3D memesh, int a, int b, int c)
            {
                memesh.TriangleIndices.Add(a); memesh.TriangleIndices.Add(b); memesh.TriangleIndices.Add(c);
            }

            addmesh(mesh, 0, 1, 2);
            addmesh(mesh, 2, 3, 0);
            addmesh(mesh, 1, 5, 6);
            addmesh(mesh, 1, 6, 2);

            addmesh(mesh, 3, 2, 6);
            addmesh(mesh, 3, 6, 7);
            addmesh(mesh, 0, 5, 1);
            addmesh(mesh, 0, 4, 5);

            addmesh(mesh, 0, 3, 7);
            addmesh(mesh, 0, 7, 4);
            addmesh(mesh, 7, 6, 5);
            addmesh(mesh, 7, 5, 4);

            mGeometry = new GeometryModel3D(mesh, new DiffuseMaterial(Brushes.Green));
            mGeometry.Transform = new Transform3DGroup();

            group.Children.Add(mGeometry);

        }
        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            camera.Position = new Point3D(camera.Position.X, camera.Position.Y, camera.Position.Z - e.Delta * 1);
        }
        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (mDown)
            {
                Point pos = Mouse.GetPosition(viewport);
                Point actual = new Point(pos.X - viewport.ActualWidth / 2, viewport.ActualHeight / 2 - pos.Y);
                double dx = actual.X - mLastPos.X, dy = actual.Y - mLastPos.Y;

                double mouseAngle = 0;
                if (dx != 0 && dy != 0)
                {
                    mouseAngle = Math.Asin(Math.Abs(dy) / Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2)));
                    if (dx < 0 && dy > 0) mouseAngle = Math.PI / 2;
                    else if (dx < 0 && dy < 0) mouseAngle = Math.PI;
                    else if (dx > 0 && dy < 0) mouseAngle += Math.PI * 1.5;
                }
                else if (dx == 0 && dy != 0) mouseAngle = Math.Sign(dy) > 0 ? Math.PI / 2 : Math.PI * 1.5;
                else if (dx != 0 && dy == 0) mouseAngle = Math.Sign(dx) > 0 ? 0 : Math.PI;

                double axisAngle = mouseAngle + Math.PI / 2;

                Vector3D axis = new Vector3D(Math.Cos(axisAngle) * 4, Math.Sin(axisAngle) * 4, 0);
                double rotation = 0.01 * Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));

                Transform3DGroup group = mGeometry.Transform as Transform3DGroup;

                QuaternionRotation3D r = new QuaternionRotation3D(new Quaternion(axis, rotation * 180 / Math.PI));

                group.Children.Add(new RotateTransform3D(r));
                mLastPos = actual;

            }
        }
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;
            mDown = true;

            Point pos = Mouse.GetPosition(viewport);
            mLastPos = new Point(pos.X - viewport.ActualWidth / 2, viewport.ActualHeight / 2 - pos.Y);
        }
        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mDown = false;
        }

    }
}
